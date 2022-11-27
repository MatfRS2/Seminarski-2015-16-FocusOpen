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
using System.Data;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;
using InfoSoftGlobal;

namespace FocusOPEN.Website.Admin
{
	public partial class AssetStats : BaseAdminPage
    {
        #region Accessors

        protected override ContextType AssetContext
        {
            get
            {
                return ContextType.AssetStatistics;
            }
        }

        #endregion



        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int assetId = GetRequiredQueryStringParameter("AssetId", "../Assets/AssetList.aspx");

				// Get the asset.  No need to check permissions
				// as the security module will have already checked it
				Asset asset = GetAsset(assetId);

				if (asset.IsNull)
				{
					Response.Redirect("../Assets/AssetList.aspx", false);
					return;
				}

				// Setup the audit trail link
				SiteUtils.SetHyperLink(CurrentUser, "Reports.AssetAuditTrail", string.Format("?AssetId={0}&Source=AssetStats", asset.AssetId), ViewAuditTrailHyperlink);

				// Update popularity rank
				AssetReportMapper.Instance.UpdateAssetPopularityRank(assetId);

				// Populate asset preview with this asset
				AssetPreview1.Asset = asset;

				// Populate asset info
				AssetReferenceLabel.Text = asset.AssetId.ToString();
				AssetTypeLabel.Text = asset.AssetType.Name;
				TitleLabel.Text = asset.Title;
				BrandLabel.Text = asset.Brand.Name;
				RestrictedDownloadLabel.Text = EntitySecurityManager.IsAssetRestricted(CurrentUser, asset) ? "Yes" : "No";
				UsageRestrictionsLabel.Text = asset.UsageRestrictions;
				UploadUserHyperLink.Text = asset.UploadedByUserName;
				UploadUserHyperLink.NavigateUrl = "~/Admin/Users/UserForm.aspx?UserId=" + asset.UploadedByUserId;
				DateUploadedLabel.Text = asset.UploadDate.ToString(Global.DateFormat);
				DatePublishedLabel.Text = asset.PublishDate.ToString(Global.DateFormat);
				DateLastEditedLabel.Text = asset.LastUpdate.ToString(Global.DateFormat);

				// Ensure empty cells have at least non-breaking space
				SiteUtils.PopulateBlankControl(TitleLabel);
				SiteUtils.PopulateBlankControl(UsageRestrictionsLabel);

				// Get total number of search hits
				AuditAssetSearchResultFinder finder1 = new AuditAssetSearchResultFinder();
				finder1.AssetId = asset.AssetId.GetValueOrDefault();
				TotalNumberOfSearchHitsLabel.Text = AuditAssetSearchResult.GetCount(finder1).ToString();

				// Get total number of views
				AuditAssetHistoryFinder finder2 = new AuditAssetHistoryFinder();
				finder2.AssetId = asset.AssetId.GetValueOrDefault();
				finder2.AuditAssetAction = AuditAssetAction.ViewedAssetDetail;
				TotalNumberOfViewsLabel.Text = AuditAssetHistory.GetCount(finder2).ToString();

				// Get total number of added to lightbox
				AuditAssetHistoryFinder finder3 = new AuditAssetHistoryFinder();
				finder3.AssetId = asset.AssetId.GetValueOrDefault();
				finder3.AuditAssetAction = AuditAssetAction.AddedToLightbox;
				TotalNumberAddedToLightboxLabel.Text = AuditAssetHistory.GetCount(finder3).ToString();

				// Get total number of added to cart
				AuditAssetHistoryFinder finder4 = new AuditAssetHistoryFinder();
				finder4.AssetId = asset.AssetId.GetValueOrDefault();
				finder4.AuditAssetAction = AuditAssetAction.AddedToCart;
				TotalNumberAddedToCartLabel.Text = AuditAssetHistory.GetCount(finder4).ToString();

				// Get total number of order requests
				OrderItemFinder finder5 = new OrderItemFinder();
				finder5.AssetId = asset.AssetId.GetValueOrDefault();
				TotalNumberOfOrderRequestsLabel.Text = OrderItem.GetCount(finder5).ToString();

				// Get total number of downloads
				AuditAssetHistoryFinder finder6 = new AuditAssetHistoryFinder();
				finder6.AssetId = asset.AssetId.GetValueOrDefault();
				finder6.AuditAssetAction = AuditAssetAction.DownloadedAssetFile;
				TotalNumberOfDownloadsLabel.Text = AuditAssetHistory.GetCount(finder6).ToString();

				// Get total assets
				AssetFinder finder7 = new AssetFinder();
				int assetCount = Asset.GetCount(finder7);

				// Display popularity based on downloads
				PopularityBasedOnDownloadsLabel.Text = string.Format("{0} / {1} assets", asset.PopularityRank, assetCount);

				// Open the panel specified in the querystring, or default to activity summary
				string panel = WebUtils.GetRequestParam("panel", Panels.ActivitySummary.ToString());
				TogglePanel(panel);
			}
		}

		protected void TimeFrameDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			TogglePanel(Panels.ActivitySummary);
		}

		protected void DisplayPanel_Click(object sender, CommandEventArgs e)
		{
			string panelName = e.CommandArgument.ToString();
			TogglePanel(panelName);
		}

		protected void LastUsersToDownloadRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					DataRowView row = (DataRowView) e.Item.DataItem;

					EmailHyperLink EmailHyperLink1 = (EmailHyperLink) e.Item.FindControl("EmailHyperLink1");
					Label DateLabel = (Label) e.Item.FindControl("DateLabel");

					EmailHyperLink1.EmailAddress = row["Email"].ToString();
					DateLabel.Text = Convert.ToDateTime(row["Date"]).ToString(Global.DateTimeFormat);

					break;
			}
		}

		#region Private Helper Methods

		private enum Panels
		{
			ActivitySummary,
			TopSearchTerms,
			LastUsersToDownload
		}

		private void TogglePanel(string panelName)
		{
			Panels panel = GeneralUtils.ParseEnum(panelName, Panels.ActivitySummary);
			TogglePanel(panel);
		}

		private void TogglePanel(Panels panel)
		{
			// Enable all of them
			ActivitySummaryLinkButton.Enabled = true;
			TopSearchTermsLinkButton.Enabled = true;
			LastUsersToDownloadLinkButton.Enabled = true;

			// Hide all the panels
			ActivitySummaryPanel.Visible = false;
			TopSearchTermsPanel.Visible = false;
			LastUsersToDownloadPanel.Visible = false;

			// Get the asset id
			int assetId = WebUtils.GetIntRequestParam("AssetId", 0);

			switch (panel)
			{
				case (Panels.ActivitySummary):

					ActivitySummaryLinkButton.Enabled = false;
					ActivitySummaryPanel.Visible = true;

					Asset asset = GetAsset(assetId);
					DisplayActivitySummary(asset);

					break;

				case (Panels.TopSearchTerms):

					TopSearchTermsLinkButton.Enabled = false;
					TopSearchTermsPanel.Visible = true;
					DisplayTopSearchTerms(assetId);

					break;

				case (Panels.LastUsersToDownload):

					LastUsersToDownloadLinkButton.Enabled = false;
					LastUsersToDownloadPanel.Visible = true;
					DisplayLastUsersToDownload(assetId);

					break;
			}
		}

		private Asset GetAsset(int assetId)
		{
			Asset asset = Context.Items["Asset"] as Asset;

			if (asset == null || asset.AssetId.GetValueOrDefault(0) != assetId)
				asset = Asset.Get(assetId);

			return asset;
		}

		private void DisplayLastUsersToDownload(int assetId)
		{
			DataTable dt = AssetReportMapper.Instance.GetLastUsersToDownload(assetId);

			LastUsersToDownloadRepeater.Visible = (dt.Rows.Count > 0);
			NoDownloadsPanel.Visible = (!LastUsersToDownloadRepeater.Visible);

			LastUsersToDownloadRepeater.DataSource = dt;
			LastUsersToDownloadRepeater.DataBind();
		}

		private void DisplayTopSearchTerms(int assetId)
		{
			DataTable dt = AssetReportMapper.Instance.GetTopSearchTerms(assetId);

			TopSearchTermsRepeater.Visible = (dt.Rows.Count > 0);
			NoSearchTermsPanel.Visible = (!TopSearchTermsRepeater.Visible);

			TopSearchTermsRepeater.DataSource = dt;
			TopSearchTermsRepeater.DataBind();
		}

		private void DisplayActivitySummary(Asset asset)
		{
			DateRange timeFrame = SiteUtils.GetDateRangeFromTimeFrame(EnumUtils.GetEnumFromValue<TimeFrame>(TimeFrameDropDownList1.SelectedId));
			DataRow row = AssetReportMapper.Instance.GetAssetStats(asset.AssetId.GetValueOrDefault(), timeFrame);

			int assetDownloads = Convert.ToInt32(row["AssetDownloads"]);
			int totalDownloads = Convert.ToInt32(row["TotalDownloads"]);
			int assetOrderRequests = Convert.ToInt32(row["AssetOrderRequests"]);
			int totalOrderRequests = Convert.ToInt32(row["TotalOrderRequests"]);
			int assetViews = Convert.ToInt32(row["AssetViews"]);
			int totalViews = Convert.ToInt32(row["TotalViews"]);
			int assetSearchHits = Convert.ToInt32(row["AssetSearchHits"]);
			int totalSearchHits = Convert.ToInt32(row["TotalSearchHits"]);
			int assetAddedToCart = Convert.ToInt32(row["AssetAddedToCart"]);
			int totalAddedToCart = Convert.ToInt32(row["TotalAddedToCart"]);
			int assetAddedToLightbox = Convert.ToInt32(row["AssetAddedToLightbox"]);
			int totalAddedToLightbox = Convert.ToInt32(row["TotalAddedToLightbox"]);

			SiteUtils.PopulateCell(DownloadsCell, assetDownloads, totalDownloads);
			SiteUtils.PopulateCell(OrderRequestsCell, assetOrderRequests, totalOrderRequests);
			SiteUtils.PopulateCell(ViewsCell, assetViews, totalViews);
			SiteUtils.PopulateCell(SearchHitsCell, assetSearchHits, totalSearchHits);
			SiteUtils.PopulateCell(AddToCartCell, assetAddedToCart, totalAddedToCart);
			SiteUtils.PopulateCell(AddToLightboxCell, assetAddedToLightbox, totalAddedToLightbox);

			// Register the FusionCharts javascript
			if (!Page.ClientScript.IsClientScriptIncludeRegistered("FusionCharts"))
				Page.ClientScript.RegisterClientScriptInclude("FusionCharts", ResolveUrl("~/FusionCharts/FusionCharts.js"));

			// Setup the XML for the chart
			XElement xml = new XElement("graph",
				new XAttribute("xaxisname", string.Empty),
				new XAttribute("yaxisname", "Count"),
				new XAttribute("hovercapbg", "dedebe"),
				new XAttribute("hovercapborder", "413189"),
				new XAttribute("rotateNames", 0),
				new XAttribute("numdivlines", 9),
				new XAttribute("canvasBaseDepth", 2),
				new XAttribute("canvasBgDepth", 2),
				new XAttribute("divLineColor", "cccccc"),
				new XAttribute("divLineAlpha", 80),
				new XAttribute("divLineThickness", 1),
				new XAttribute("decimalPrecision", 0),
				new XAttribute("showAlternateHGridColor", 1),
				new XAttribute("AlternateHGridAlpha", 30),
				new XAttribute("AlternateHGridColor", "cccccc"),
				new XAttribute("zeroPlaneShowBorder", 1),
				new XElement("categories",
					new XAttribute("font", "Arial"),
					new XAttribute("fontSize", 11),
					new XAttribute("fontColor", "000000"),
					new XElement("category", new XAttribute("name", "Downloads")),
					new XElement("category", new XAttribute("name", "   Order&#10;Requests"), new XAttribute("hoverText", "Order Requests")),
					new XElement("category", new XAttribute("name", "Views")),
					new XElement("category", new XAttribute("name", "Search&#10;  Hits"), new XAttribute("hoverText", "Search Hits")),
					new XElement("category", new XAttribute("name", "Added&#10;   To&#10; Cart"), new XAttribute("hoverText", "Added To Cart")),
					new XElement("category", new XAttribute("name", " Added&#10;   To&#10;Lightbox"), new XAttribute("hoverText", "Added To Lightbox"))
				),
				new XElement("dataset",
					new XAttribute("seriesname", "This Asset"),
					new XAttribute("color", "413189"),
					new XElement("set", new XAttribute("value", assetDownloads)),
					new XElement("set", new XAttribute("value", assetOrderRequests)),
					new XElement("set", new XAttribute("value", assetViews)),
					new XElement("set", new XAttribute("value", assetSearchHits)),
					new XElement("set", new XAttribute("value", assetAddedToCart)),
					new XElement("set", new XAttribute("value", assetAddedToLightbox))
				),
				new XElement("dataset",
					new XAttribute("seriesname", "All Assets"),
					new XAttribute("color", "bcbbcb"),
					new XElement("set", new XAttribute("value", totalDownloads)),
					new XElement("set", new XAttribute("value", totalOrderRequests)),
					new XElement("set", new XAttribute("value", totalViews)),
					new XElement("set", new XAttribute("value", totalSearchHits)),
					new XElement("set", new XAttribute("value", totalAddedToCart)),
					new XElement("set", new XAttribute("value", totalAddedToLightbox))
				)
			);

			// Setup the chart
			ChartLiteral.Text = FusionCharts.RenderChart(ResolveUrl("~/FusionCharts/FCF_MSColumn3D.swf"), string.Empty, xml.ToUnformattedXmlForFusionCharts(), "AssetInfoChart_" + ChartLiteral.ClientID, "550", "350", false, false);
		}

		#endregion
	}
}