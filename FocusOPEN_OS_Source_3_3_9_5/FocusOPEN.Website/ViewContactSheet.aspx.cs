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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Controls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website
{
	public partial class ViewContactSheet : Page
    {
        #region Private Variables

		private LightboxSent m_lightboxSent = null;

        #endregion


        #region Handled Events

        protected void Page_Load(object sender, EventArgs e)
		{
			// Get the data from the querystring, to be used to get the sent lightbox
			int lightboxSentId = WebUtils.GetIntRequestParam("lsid", 0);
			int senderId = WebUtils.GetIntRequestParam("suid", 0);
			string ticks = WebUtils.GetRequestParam("dst", string.Empty);

			// Ensure that we have all required querystring data
			if (lightboxSentId == 0 || senderId == 0 || ticks == string.Empty)
			{
				Response.Redirect("~/Login.aspx?error=ContactSheetMissingData", false);
				return;
			}

			// Get the lightbox
            m_lightboxSent = LightboxSent.Get(lightboxSentId);

			// Check posted data - ensure that the sender id and ticks match (ie. to ensure user is not messing with the querystring)
            if (!m_lightboxSent.SenderId.Equals(senderId) || ticks.Length < 6 || !m_lightboxSent.DateSent.Ticks.ToString().Substring(0, 6).Equals(ticks.Substring(0, 6)))
			{
				Response.Redirect("~/Login.aspx?error=ContactSheetInvalidData", false);
				return;
			}

			// Ensure the lightbox has not expired
            if (m_lightboxSent.ExpiryDate.HasValue && m_lightboxSent.ExpiryDate < DateTime.Now)
			{
				Response.Redirect("~/Login.aspx?error=ContactSheetExpired", false);
				return;
			}

			// Get the lightbox
            Lightbox lightbox = m_lightboxSent.Lightbox;

			// Update the UI with the lightbox information
			LightboxTitleLabel.Text = lightbox.Name;
            LightboxSenderName.Text = m_lightboxSent.Sender.FullName;
            LightboxSenderEmail.EmailAddress = m_lightboxSent.Sender.Email;
            LightboxDateSentLabel.Text = m_lightboxSent.DateSent.ToString("HH:mm, dd MMMM yyyy");
            LightboxDateExpiresLabel.Text = m_lightboxSent.ExpiryDate.HasValue ? m_lightboxSent.ExpiryDate.Value.ToString("HH:mm, dd MMMM yyyy") : "Never";
			LightboxNotesLabel.Text = lightbox.Notes;

			// Get the lightbox assets
			EntityList<LightboxAsset> lightboxAssetList = lightbox.GetLightboxAssetList();

			// Ensure we have at least one row
			while (lightboxAssetList.Count < LightboxDataList.RepeatColumns)
				lightboxAssetList.Add(LightboxAsset.Empty);

			// Bind the datalist to the lightbox assets
			LightboxDataList.DataSource = lightboxAssetList;
			LightboxDataList.DataBind();
		}

		protected void LightboxDataList_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Panel LightboxAssetInfoPanel = (Panel) e.Item.FindControl("LightboxAssetInfoPanel");
					LightboxAsset lightboxAsset = (LightboxAsset) e.Item.DataItem;

					if (lightboxAsset.IsNull)
					{
						LightboxAssetInfoPanel.Visible = false;

						// Clear the CSS class so the background doesnt show
						WebControl wc = (WebControl)LightboxAssetInfoPanel.Parent;
						wc.CssClass = string.Empty;
					}
					else
					{
						Asset asset = lightboxAsset.Asset;

						AssetThumbnail AssetThumbnail1 = (AssetThumbnail) e.Item.FindControl("AssetThumbnail1");
						HtmlGenericControl AssetIdLabel = (HtmlGenericControl) e.Item.FindControl("AssetIdLabel");
						HtmlGenericControl AssetTypeLabel = (HtmlGenericControl) e.Item.FindControl("AssetTypeLabel");
						Image NotesImage = (Image) e.Item.FindControl("NotesImage");
                        HyperLink DownloadHyperLink = (HyperLink)e.Item.FindControl("DownloadHyperLink");

						AssetThumbnail1.HoverCaption = asset.Title;
						AssetThumbnail1.HoverText = GeneralUtils.GetNonEmptyString(lightboxAsset.Notes, "[Sender has not entered any notes]");
						AssetThumbnail1.Initialise(asset);

						AssetIdLabel.InnerText = asset.AssetId.ToString();
						AssetTypeLabel.InnerText = "(" + asset.AssetType.Name + ")";

						NotesImage.AlternateText = Server.HtmlEncode(AssetThumbnail1.HoverText);
						NotesImage.ToolTip = Server.HtmlEncode(AssetThumbnail1.HoverText);

                        //check if light box was sent with download links enabled
                        //and double check that the sender was super admin
                        if (m_lightboxSent.DownloadLinks.GetValueOrDefault(false) &&
                            m_lightboxSent.Sender.UserRole == UserRole.SuperAdministrator)
                        {
                            DownloadHyperLink.Visible = true;
                            DownloadHyperLink.NavigateUrl = AssetFileUrlHelper.GetExternalDownloadUrl(asset.AssetId, m_lightboxSent.LightboxSentId, m_lightboxSent.SenderId,m_lightboxSent.DateSent.Ticks);
                            DownloadHyperLink.ImageUrl = SiteUtils.GetIconPath("download0.gif");
                        }
                        else
                        {
                            DownloadHyperLink.Visible = false; 
                        }
					}

					break;
			}
		}

		#endregion
	}
}