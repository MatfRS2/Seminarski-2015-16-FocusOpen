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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public partial class PersistentCartLightbox : BaseUserControl
	{
		#region Events

		public event AssetButtonEventHandler LightboxButtonClicked;
		public event AssetButtonEventHandler CartButtonClicked;
		public event EventHandler LightboxChanged;
		public event EventHandler LightboxAssetsCopiedToCart;
		public event EventHandler EmptyCartLinkButtonClicked;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets the number of assets to display.
		/// </summary>
		public static int NumberOfAssetsToDisplay
		{
			get
			{
				return 4;
			}
		}

		/// <summary>
		/// Gets the selected lightbox id.
		/// </summary>
		public static int SelectedLightboxId
		{
			get
			{
				return PersistentLightboxCartInfo.SelectedLightboxId;
			}
		}

		/// <summary>
		/// Gets the persistent lightbox cart info.
		/// </summary>
		private static PersistentLightboxCartInfo PersistentLightboxCartInfo
		{
			get
			{
				return SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo;
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Init(object sender, EventArgs e)
		{
			LightboxDropDownList1.SafeSelectValue(PersistentLightboxCartInfo.SelectedLightboxId);
			LightboxDropDownList2.SafeSelectValue(PersistentLightboxCartInfo.SelectedLightboxId);

			Bind();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				string controlName = PersistentLightboxCartInfo.PersistentCartLightboxMode.ToString() + PersistentLightboxCartInfo.PersistentCartLightboxState;
				OpenPanel(controlName);
			}
		}

		protected void ModeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			DropDownList dd = (DropDownList) sender;

			string senderName = dd.ID;
			string selectedValue = dd.SelectedValue;

			string panel = string.Empty;

			switch (senderName)
			{
				case "CartOpen_ModeDropDownList":
				case "LightboxOpen_ModeDropDownList":

					switch (selectedValue)
					{
						case "Cart":

							panel = "CartOpen";
							break;

						case "Lightbox":

							panel = "LightboxOpen";
							break;
					}

					break;

				case "CartClosed_ModeDropDownList":
				case "LightboxClosed_ModeDropDownList":

					switch (selectedValue)
					{
						case "Cart":

							panel = "CartClosed";
							break;

						case "Lightbox":

							panel = "LightboxClosed";
							break;
					}

					break;
			}

			OpenPanel(panel);
		}

		protected void TogglePanelLinkButton_Click(object sender, CommandEventArgs e)
		{
			string state = e.CommandName;
			string mode = e.CommandArgument.ToString();

			OpenPanel(mode, state);
		}

		protected void LightboxDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Get the selected ID from the control that fired this event
			UserLightboxDropDownList o = (UserLightboxDropDownList) sender;
			
			// Update session info
			PersistentLightboxCartInfo.SelectedLightboxId = o.SelectedId;

			// Update both dropdowns (as we dont know which one fired it)
			LightboxDropDownList1.SafeSelectValue(o.SelectedId);
			LightboxDropDownList2.SafeSelectValue(o.SelectedId);
			
			// Reset selected lightbox offset to 0
			PersistentLightboxCartInfo.LightboxOffSet = 0;

			Bind();

			if (LightboxChanged != null)
				LightboxChanged(this, EventArgs.Empty);
		}

		private void AssetButtons1_LightboxButtonClicked(object sender, AssetButtonEventArgs e)
		{
			Bind();

			if (LightboxButtonClicked != null)
				LightboxButtonClicked(this, e);
		}

		private void AssetButtons1_CartButtonClicked(object sender, AssetButtonEventArgs e)
		{
			Bind();

			if (CartButtonClicked != null)
				CartButtonClicked(this, e);
		}


		protected void CopyAllToCartLinkButton_Click(object sender, EventArgs e)
		{
			// Get the lightbox
			Lightbox lb = ContextInfo.LightboxManager.GetLightboxById(SelectedLightboxId);

			// Get the assets and add them to the cart
			foreach (Asset asset in lb.GetAssetList())
			{
				ContextInfo.CartManager.AddAssetToCart(asset.AssetId.GetValueOrDefault());
			}

			// Change offset to last page to show new assets
			PersistentLightboxCartInfo.CartOffSet = Int32.MaxValue;

			// Switch to cart view
			OpenPanel("cart", "open");

			if (LightboxAssetsCopiedToCart != null)
				LightboxAssetsCopiedToCart(this, EventArgs.Empty);
		}

		protected void EmptyCartLinkButton_Click(object sender, EventArgs e)
		{
			ContextInfo.CartManager.EmptyCart(true);
			PersistentLightboxCartInfo.CartOffSet = 0;

			Bind();

			if (EmptyCartLinkButtonClicked != null)
				EmptyCartLinkButtonClicked(this, EventArgs.Empty);
		}

		protected void AssetRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Asset asset = Asset.Empty;

					if (e.Item.DataItem is Cart)
					{
						Cart cart = (Cart)e.Item.DataItem;
						asset = cart.Asset;
					}
					else if (e.Item.DataItem is LightboxAsset)
					{
						LightboxAsset lba = (LightboxAsset)e.Item.DataItem;
						asset = lba.Asset;
					}

					AssetThumbnail AssetThumbnail1 = (AssetThumbnail) e.Item.FindControl("AssetThumbnail1");
					AssetButtons AssetButtons1 = (AssetButtons) e.Item.FindControl("AssetButtons1");
					PlaceHolder AssetContainer = (PlaceHolder) e.Item.FindControl("AssetContainer");
					PlaceHolder EmptyAssetContainer = (PlaceHolder)e.Item.FindControl("EmptyAssetContainer");

					if (asset.IsNull)
					{
						AssetContainer.Visible = false;
						EmptyAssetContainer.Visible = true;
					}
					else
					{
						AssetContainer.Visible = true;
						EmptyAssetContainer.Visible = false;

						AssetButtons1.LightboxId = SelectedLightboxId;

						AssetThumbnail1.Initialise(asset);
						AssetButtons1.Initialise(asset);

						AssetButtons1.LightboxButtonClicked += new AssetButtonEventHandler(AssetButtons1_LightboxButtonClicked);
						AssetButtons1.CartButtonClicked += new AssetButtonEventHandler(AssetButtons1_CartButtonClicked);
					}

					break;
			}
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Open the specified panel
		/// </summary>
		/// <param name="mode">cart OR lightbox</param>
		/// <param name="state">open OR closed</param>
		private void OpenPanel(string mode, string state)
		{
			mode = mode.ToLower();
			state = state.ToLower();

			string panel = string.Empty;

			switch (state)
			{
				case "open":

					switch (mode)
					{
						case "cart":
							panel = "CartOpen";
							break;

						case "lightbox":
							panel = "LightboxOpen";
							break;
					}

					break;

				case "close":

					switch (mode)
					{
						case "cart":
							panel = "CartClosed";
							break;

						case "lightbox":
							panel = "LightboxClosed";
							break;
					}

					break;
			}

			OpenPanel(panel);
		}

		private void OpenPanel(string controlName)
		{
			if (String.IsNullOrEmpty(controlName))
				throw new ArgumentException("ControlName cannot be empty");

			// Close all panels
			LightboxOpen.Visible = false;
			LightboxClosed.Visible = false;
			CartOpen.Visible = false;
			CartClosed.Visible = false;

			// Open panel
			Control placeHolder = SiteUtils.FindControlRecursive(this, controlName);

			if (placeHolder == null)
				throw new NullReferenceException(string.Format("Control with ID: {0} not found", controlName));

			// Make the control visible
			placeHolder.Visible = true;

			switch (controlName)
			{
				case "LightboxOpen":
					PersistentLightboxCartInfo.PersistentCartLightboxMode = PersistentCartLightboxMode.Lightbox;
					PersistentLightboxCartInfo.PersistentCartLightboxState = PersistentCartLightboxState.Open;
					break;

				case "LightboxClosed":
					PersistentLightboxCartInfo.PersistentCartLightboxMode = PersistentCartLightboxMode.Lightbox;
					PersistentLightboxCartInfo.PersistentCartLightboxState = PersistentCartLightboxState.Closed;
					break;

				case "CartOpen":
					PersistentLightboxCartInfo.PersistentCartLightboxMode = PersistentCartLightboxMode.Cart;
					PersistentLightboxCartInfo.PersistentCartLightboxState = PersistentCartLightboxState.Open;
					break;

				case "CartClosed":
					PersistentLightboxCartInfo.PersistentCartLightboxMode = PersistentCartLightboxMode.Cart;
					PersistentLightboxCartInfo.PersistentCartLightboxState = PersistentCartLightboxState.Closed;
					break;
			}

			CookieManager.SetValue("PersistentCartLightboxMode", PersistentLightboxCartInfo.PersistentCartLightboxMode);
			CookieManager.SetValue("PersistentCartLightboxState", PersistentLightboxCartInfo.PersistentCartLightboxState);

			// Update the dropdowns in all the panels
			CartOpen_ModeDropDownList.SafeSelectValue(PersistentLightboxCartInfo.PersistentCartLightboxMode);
			CartClosed_ModeDropDownList.SafeSelectValue(PersistentLightboxCartInfo.PersistentCartLightboxMode);
			LightboxOpen_ModeDropDownList.SafeSelectValue(PersistentLightboxCartInfo.PersistentCartLightboxMode);
			LightboxClosed_ModeDropDownList.SafeSelectValue(PersistentLightboxCartInfo.PersistentCartLightboxMode);

			// Mode has changed, so rebind
			Bind();
		}

		protected void ArrowImageButton_Command(object sender, CommandEventArgs e)
		{
			string commandName = e.CommandName;
			int offset = Convert.ToInt32(e.CommandArgument);

			switch (commandName)
			{
				case "Lightbox":
					PersistentLightboxCartInfo.LightboxOffSet = offset;
					break;
				
				case "Cart":
					PersistentLightboxCartInfo.CartOffSet = offset;
					break;
			}

			Bind();
		}

		public void Bind()
		{
			var mode = PersistentLightboxCartInfo.PersistentCartLightboxMode;
			var state = PersistentLightboxCartInfo.PersistentCartLightboxState;

			if (mode == PersistentCartLightboxMode.Cart)
			{
				var items = ContextInfo.CartManager.CartList;
				int count = items.Count;

				CartItemCount1.Text = count.ToString();
				CartItemCount2.Text = count.ToString();

				if (state == PersistentCartLightboxState.Open)
				{
					int offset = PersistentLightboxCartInfo.CartOffSet;

					if (count <= NumberOfAssetsToDisplay)
					{
						offset = 0;
					}
					else if (offset > count - NumberOfAssetsToDisplay)
					{
						offset = count - NumberOfAssetsToDisplay;
					}

					if (items.Count == 0)
					{
						offset = 0;
						items.Add(Cart.Empty);
					}

					items = ContextInfo.CartManager.CartList.Skip(offset).Take(NumberOfAssetsToDisplay).ToList();

					CartRepeater.DataSource = items;
					CartRepeater.DataBind();

					UpdatePagingControls(CartRepeater, offset, count);
				}
			}

			if (mode == PersistentCartLightboxMode.Lightbox)
			{
				var lbaList = ContextInfo.LightboxManager.GetLightboxById(SelectedLightboxId).GetLightboxAssetList();

				var items = (from item in lbaList
				             where (EntitySecurityManager.CanViewAssetInfo(CurrentUser, item.Asset))
				             orderby item.LightboxAssetId ascending
				             select item).ToList();

				int count = items.Count;
				LightboxItemCount1.Text = count.ToString();
				LightboxItemCount2.Text = LightboxItemCount1.Text;

				NewLightboxHyperLink.NavigateUrl = string.Format("~/MyAccount/ViewLightbox.aspx?lightboxId={0}&action=new", SelectedLightboxId);
				SendLightboxHyperLink.NavigateUrl = string.Format("~/MyAccount/ViewLightbox.aspx?lightboxId={0}&action=send", SelectedLightboxId);
				ViewLightboxHyperLink.NavigateUrl = string.Format("~/MyAccount/ViewLightbox.aspx?lightboxId={0}", SelectedLightboxId);

				if (state == PersistentCartLightboxState.Open)
				{
					int offset = PersistentLightboxCartInfo.LightboxOffSet;

					if (count <= NumberOfAssetsToDisplay)
					{
						offset = 0;
					}
					else if (offset > count - NumberOfAssetsToDisplay)
					{
						offset = count - NumberOfAssetsToDisplay;
					}

					if (items.Count == 0)
					{
						offset = 0;
						items.Add(LightboxAsset.Empty);
					}

					LightboxRepeater.DataSource = items.Skip(offset).Take(NumberOfAssetsToDisplay);
					LightboxRepeater.DataBind();

					UpdatePagingControls(LightboxRepeater, offset, count);
				}
			}
		}

		private void UpdatePagingControls(Repeater repeater, int offset, int total)
		{
			string prefix = (repeater.ID == LightboxRepeater.ID) ? "Lightbox" : "Cart";

			string leftArrowImageButtonId = string.Concat(prefix, "LeftArrowImageButton");
			string rightArrowImageButtonId = string.Concat(prefix, "RightArrowImageButton");

			ImageButton LeftArrowImageButton = (ImageButton)SiteUtils.FindControlRecursive(repeater, leftArrowImageButtonId);
			ImageButton RightArrowImageButton = (ImageButton)SiteUtils.FindControlRecursive(repeater, rightArrowImageButtonId);

			LeftArrowImageButton.CommandArgument = Convert.ToString(offset - 1);
			RightArrowImageButton.CommandArgument = Convert.ToString(offset + 1);

			LeftArrowImageButton.Visible = (total != 0 && offset > 0);
			RightArrowImageButton.Visible = (total != 0 && offset + NumberOfAssetsToDisplay < total);
		}

		#endregion
	}
}