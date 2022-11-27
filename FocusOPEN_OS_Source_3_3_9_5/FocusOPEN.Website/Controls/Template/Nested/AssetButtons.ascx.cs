/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;

namespace FocusOPEN.Website.Controls
{
	public partial class AssetButtons : BaseUserControl
	{
		[Flags]
		public enum Buttons : uint
		{
			None = 0,
			Lightbox = 1,
			Cart = 2,
			Info = 4,
			Download = 8,
			Restricted = 16,
			All = Lightbox | Cart | Info | Download | Restricted
		}

		#region Events

		public event AssetButtonEventHandler LightboxButtonClicked;
		public event AssetButtonEventHandler CartButtonClicked;

		#endregion

		#region Private variables

		private static readonly ILog m_Logger = LogManager.GetLogger(typeof(AssetButtons));

		private int m_AssetId;

		#endregion

		#region Constructor

		protected AssetButtons()
		{
			ShowButtons = Buttons.All;
		}

		#endregion

		#region Public Accessors

		public int AssetId
		{
			get
			{
				return m_AssetId;
			}
		}

		public int LightboxId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "LightboxId", 0);
			}
			set
			{
				ViewState["LightboxId"] = value;
			}
		}

		/// <summary>
		/// Specifies which buttons should be displayed
		/// </summary>
		public Buttons ShowButtons { get; set; }

		#endregion

		#region Private Accessors

		private Lightbox Lightbox
		{
			get
			{
				if (LightboxId == 0)
				{
					Lightbox lightbox = ContextInfo.LightboxManager.GetDefaultLightbox();
					LightboxId = lightbox.LightboxId.GetValueOrDefault();
					return lightbox;
				}

				return ContextInfo.LightboxManager.GetLightboxById(LightboxId);
			}
		}

		#endregion

		/// <summary>
		/// Initialises the asset buttons control with the specified asset
		/// </summary>
		public void Initialise(Asset asset)
		{
			m_AssetId = asset.AssetId.GetValueOrDefault();

			// Get the URL to be used for the asset popup
			string showAssetInfoUrl = SiteUtils.GetShowAssetInfoUrl(m_AssetId);

			// Setup info button
			InfoHyperlink.Visible = ShowButtons.IsSet(Buttons.Info);
			InfoHyperlink.NavigateUrl = showAssetInfoUrl;
			InfoHyperlink.ImageUrl = SiteUtils.GetIconPath("info0.gif");

			// Setup download button
			DownloadHyperLink.Visible = DisplayDownloadHyperlink(asset);
			DownloadHyperLink.NavigateUrl = AssetFileUrlHelper.GetDownloadUrl(m_AssetId);
			DownloadHyperLink.ImageUrl = SiteUtils.GetIconPath("download0.gif");

			// Setup lightbox button
			AddToLightboxImageButton.Visible = (ShowButtons.IsSet(Buttons.Lightbox));
			AddToLightboxImageButton.CommandArgument = m_AssetId.ToString();
			InitialiseLightboxButton(m_AssetId);

			// Setup cart button
			AddToCartImageButton.Visible = (ShowButtons.IsSet(Buttons.Cart));
			AddToCartImageButton.CommandArgument = m_AssetId.ToString();
			InitialiseCartButton(m_AssetId);
			
			// Setup restricted icon
			RestrictedImageIcon.Visible = (EntitySecurityManager.IsAssetRestricted(CurrentUser, asset) && ShowButtons.IsSet(Buttons.Restricted));
			RestrictedImageIcon.ImageUrl = SiteUtils.GetIconPath("restricted0.gif");
		}

		#region Helper Methods

		private void InitialiseLightboxButton(int assetId)
		{
			Lightbox defaultLightbox = ContextInfo.LightboxManager.GetDefaultLightbox();

			bool isAssetInLightbox = ContextInfo.LightboxManager.LightboxContainsAsset(Lightbox, assetId);
			bool isDefaultLightbox = Lightbox.LightboxId.Equals(defaultLightbox.LightboxId);

			string image;

			if (isAssetInLightbox)
			{
				image = "lightbox1.gif";
				AddToLightboxImageButton.AlternateText = "click here to remove from lightbox: ";
			}
			else
			{
				image = "lightbox0.gif";
				AddToLightboxImageButton.AlternateText = "click here to add to lightbox: ";
			}

			Lightbox lb = (EntitySecurityManager.CanManageLightbox(CurrentUser, Lightbox)) ? Lightbox : defaultLightbox;

			AddToLightboxImageButton.ImageUrl = SiteUtils.GetIconPath(image);
			AddToLightboxImageButton.AlternateText += lb.Name;

			if (isDefaultLightbox)
				AddToLightboxImageButton.AlternateText += " (default)";

			AddToLightboxImageButton.ToolTip = AddToLightboxImageButton.AlternateText;
		}

		private void InitialiseCartButton(int assetId)
		{
			string image;

			if (ContextInfo.CartManager.CartContainsAsset(assetId))
			{
				image = "cart1.gif";
				AddToCartImageButton.AlternateText = "click here to remove from cart";
			}
			else
			{
				image = "cart0.gif";
				AddToCartImageButton.AlternateText = "click here to add to cart";
			}

			AddToCartImageButton.ImageUrl = SiteUtils.GetIconPath(image);
			AddToCartImageButton.ToolTip = AddToCartImageButton.AlternateText;
		}

		private bool DisplayDownloadHyperlink(Asset asset)
		{
			if (ShowButtons.IsSet(Buttons.Download))
			{
				// Only display download link for users who can download the asset
				// when the direct download feature is enabled.
				if (WebsiteBrandManager.GetBrand().DirectDownloadEnabled)
					return (!EntitySecurityManager.IsAssetRestricted(CurrentUser, asset));

				// Super-admins can always download assets
				if (CurrentUser.UserRole.Equals(UserRole.SuperAdministrator))
					return true;

				// Upload users can always download their own assets
				if (asset.UploadedByUserId.Equals(CurrentUser.UserId))
					return true;
			}

			return false;
		}

		#endregion

		#region Overrides

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// If no message label control ID was specified, then climb
			// up through the parents looking for a BaseUserControl and
			// get the MessageLabelControlId from there.

			if (MessageLabelControlId == string.Empty)
			{
				// Start from this control's parent
				Control parent = Parent;

				while (parent != null)
				{
					if (parent is BaseUserControl)
					{
						BaseUserControl o = (BaseUserControl)parent;

						if (o.MessageLabelControlId != string.Empty)
						{
							MessageLabelControlId = o.MessageLabelControlId;
							break;
						}
					}

					parent = parent.Parent;
				}
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void AddLinkButton_Command(object sender, CommandEventArgs e)
		{
			try
			{
				AssetButtonAction action;

				int assetId = Convert.ToInt32(e.CommandArgument);
				int lightboxId = Lightbox.LightboxId.GetValueOrDefault();

				switch (e.CommandName.ToLower())
				{
					case "lightbox":

						if (EntitySecurityManager.CanManageLightbox(CurrentUser, Lightbox))
						{
							if (ContextInfo.LightboxManager.LightboxContainsAsset(Lightbox, assetId))
							{
								ContextInfo.LightboxManager.RemoveAssetFromLightbox(lightboxId, assetId);
								MessageLabel.SetSuccessMessage("asset removed from lightbox");
								action = AssetButtonAction.AssetRemoved;
							}
							else
							{
								ContextInfo.LightboxManager.AddAssetToLightbox(lightboxId, assetId);
								MessageLabel.SetSuccessMessage("asset added to lightbox");
								action = AssetButtonAction.AssetAdded;
							}

							InitialiseLightboxButton(assetId);

							if (SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.SelectedLightboxId == lightboxId)
							{
								switch (action)
								{
									case (AssetButtonAction.AssetAdded):

										// Set the offset to Int32.Max to force the persistent lightbox to the last page
										SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.LightboxOffSet = Int32.MaxValue;
										break;

									case (AssetButtonAction.AssetRemoved):
										SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.LightboxOffSet--;
										break;
								}
							}

							if (LightboxButtonClicked != null)
								LightboxButtonClicked(this, new AssetButtonEventArgs(assetId, action));
						}
						else
						{
							MessageLabel.SetErrorMessage("you do not have permission to add/remove assets for this lightbox");
							m_Logger.DebugFormat("User: {0} (UserId: {1}) tried to add/remove AssetId: {2} to/from LightboxId: {3} but was denied permission", CurrentUser.FullName, CurrentUser.UserId, assetId, lightboxId);
						}

						break;

					case "cart":

						if (ContextInfo.CartManager.CartContainsAsset(assetId))
						{
							ContextInfo.CartManager.RemoveAssetFromCart(assetId);
							MessageLabel.SetSuccessMessage("asset removed from cart");
							action = AssetButtonAction.AssetRemoved;
						}
						else
						{
							ContextInfo.CartManager.AddAssetToCart(assetId);
							MessageLabel.SetSuccessMessage("asset added to cart");
							action = AssetButtonAction.AssetAdded;
						}

						InitialiseCartButton(assetId);

						switch (action)
						{
							case (AssetButtonAction.AssetAdded):

								// Set the offset to Int32.Max to force the persistent cart to the last page
								SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.CartOffSet = Int32.MaxValue;
								break;

							case (AssetButtonAction.AssetRemoved):
								SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.CartOffSet--;
								break;
						}

						if (CartButtonClicked != null)
							CartButtonClicked(this, new AssetButtonEventArgs(assetId, action));

						break;
				}
			}
			catch (InvalidLightboxException lex)
			{
				MessageLabel.SetErrorMessage(lex.Message);
			}
		}

		#endregion
	}
}