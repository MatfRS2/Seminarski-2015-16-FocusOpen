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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Components.Exporting;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class AssetActivity : BaseAdminPageableEntityListPage<AssetFinder, Asset>
	{
		#region Constructor

		public AssetActivity()
		{
			BindOnLoad = false;
		}

		#endregion

		#region BaseAdminPageableEntityListPage Implementation

		protected override ISortableControl DataControl
		{
			get
			{
				return AssetRepeater;
			}
		}

		protected override BaseSavedAdminSearch<AssetFinder> SavedSearch
		{
			get
			{
				return new BaseSavedAdminSearch<AssetFinder>();
			}
		}

		protected override AssetFinder GetFinderFromForm()
		{
			AuditAssetHistoryFinder finder = new AuditAssetHistoryFinder();

			if (UploadedCheckBox.Checked)
				finder.IncludeActionsList.Add(AuditAssetAction.UploadedAsset);

			if (DownloadedCheckBox.Checked)
				finder.IncludeActionsList.Add(AuditAssetAction.DownloadedAssetFile);

			if (OrderRequestsCheckBox.Checked)
				finder.IncludeActionsList.Add(AuditAssetAction.Ordered);

			if (ViewsCheckBox.Checked)
				finder.IncludeActionsList.Add(AuditAssetAction.ViewedAssetDetail);

			if (SearcHitsCheckBox.Checked)
				finder.IncludeActionsList.Add(AuditAssetAction.ReturnedInSearch);

			if (CartCheckBox.Checked)
				finder.IncludeActionsList.Add(AuditAssetAction.AddedToCart);

			if (LightboxCheckBox.Checked)
				finder.IncludeActionsList.Add(AuditAssetAction.AddedToLightbox);

			finder.BrandId = BrandDropDownList.SelectedId;

			TimeFrame timeframe = EnumUtils.GetEnumFromValue<TimeFrame>(TimeFrameDropDownList1.SelectedId);
			DateRange dateRange = SiteUtils.GetDateRangeFromTimeFrame(timeframe);

			finder.StartDate = dateRange.StartDate;
			finder.EndDate = dateRange.EndDate;

			// Initialise the asset finder
			AssetFinder assetFinder = new AssetFinder();
			assetFinder.SortExpressions.AddRange(AssetRepeater.GetSortExpressions());
			
			// Give the asset finder, so it only finds assets in the
			// asset audit history matching the specified criteria
			// Caution: If the AuditAssetHistoryFinder is using any values
			// causing the constructed SQL query to use the parameter list
			// (eg. by setting @properties), this will fail.
			assetFinder.AuditAssetHistoryFinder = finder;
			
			return assetFinder;
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			throw new NotImplementedException();
		}

		protected override EntityList<Asset> GetData(AssetFinder finder, int page)
		{
			return Asset.FindMany(finder, page, PageSize);
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				AssetReportMapper.Instance.UpdateAllAssetsPopularityRank();
			}
		}

		protected void AssetRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					// Get data item
					Asset asset = (Asset )e.Item.DataItem;

					// Get controls
					HtmlTableCell PopularityRankCell = (HtmlTableCell)e.Item.FindControl("PopularityRankCell");
					HyperLink AssetIdHyperLink = (HyperLink)e.Item.FindControl("AssetIdHyperLink");
					HtmlTableCell AssetTypeNameCell = (HtmlTableCell)e.Item.FindControl("AssetTypeNameCell");
					HtmlTableCell UploadedByUserNameCell = (HtmlTableCell)e.Item.FindControl("UploadedByUserNameCell");
					HtmlTableCell TitleCell = (HtmlTableCell)e.Item.FindControl("TitleCell");
					HyperLink StatsHyperLink = (HyperLink)e.Item.FindControl("StatsHyperLink");

					// Link to asset stats
					string assetStatsLink = "~/Admin/Reports/AssetStats.aspx?assetId=" + asset.AssetId;

					// Populate cells
					PopularityRankCell.InnerText = asset.PopularityRank.ToString();
					AssetIdHyperLink.NavigateUrl = assetStatsLink;
					AssetIdHyperLink.Text = asset.AssetId.ToString();
					AssetTypeNameCell.InnerText = asset.AssetType.Name;
					UploadedByUserNameCell.InnerText = asset.UploadedByUserName;
					TitleCell.InnerText = asset.Title;
					StatsHyperLink.NavigateUrl = assetStatsLink;

					// Populate blank cells
					SiteUtils.PopulateBlankControl(TitleCell);

					break;
			}
		}

		protected void GenerateReportButton_Click(object sender, EventArgs e)
		{
			// Ensure at least one activity is checked
			bool anyChecked = (UploadedCheckBox.Checked || DownloadedCheckBox.Checked || OrderRequestsCheckBox.Checked || ViewsCheckBox.Checked || SearcHitsCheckBox.Checked || CartCheckBox.Checked || LightboxCheckBox.Checked);

			if (!anyChecked)
			{
				FeedbackLabel1.SetErrorMessage("the following errors occured", "at least one activity must be selected");
				return;
			}

			InitialPanel.Visible = false;
			ResultsPanel.Visible = true;
			ResultsHeaderPanel.Visible = true;

			InitialFooterPanel.Visible = false;
			ResultsFooterPanel.Visible = true;

			string panel = AssetListPanel.Visible ? "assetlist" : "activitysummary";
			DisplayPanel(panel);
		}

		protected void DownloadCSVButton_Click(object sender, EventArgs e)
		{
			ExportToCSV();
		}

		protected void ToggleViewLinkButton_Command(object sender, CommandEventArgs e)
		{
			string panel = e.CommandArgument.ToString();
			DisplayPanel(panel);
		}

		#endregion

		#region Helper Methods

		private void PopulateActivitySummary()
		{
			//------------------------------------
			// Also populate the activity summary
			//------------------------------------
			TimeFrame timeframe = EnumUtils.GetEnumFromValue<TimeFrame>(TimeFrameDropDownList1.SelectedId);
			DateRange dateRange = SiteUtils.GetDateRangeFromTimeFrame(timeframe);
			DataRow row = AssetReportMapper.Instance.GetAssetReportSummary(BrandDropDownList.SelectedId, dateRange);

			//------------------------------------
			// Get data from row
			//------------------------------------
			int uploads = Convert.ToInt32(row["Uploads"]);
			int totalUploads = Convert.ToInt32(row["TotalUploads"]);
			int downloads = Convert.ToInt32(row["Downloads"]);
			int totalDownloads = Convert.ToInt32(row["TotalDownloads"]);
			int orderRequests = Convert.ToInt32(row["OrderRequests"]);
			int totalOrderRequests = Convert.ToInt32(row["TotalOrderRequests"]);
			int views = Convert.ToInt32(row["Views"]);
			int totalViews = Convert.ToInt32(row["TotalViews"]);
			int searchHits = Convert.ToInt32(row["SearchHits"]);
			int totalSearchHits = Convert.ToInt32(row["TotalSearchHits"]);
			int addedToCart = Convert.ToInt32(row["AddedToCart"]);
			int totalAddedToCart = Convert.ToInt32(row["TotalAddedToCart"]);
			int addedToLightbox = Convert.ToInt32(row["AddedToLightbox"]);
			int totalAddedToLightbox = Convert.ToInt32(row["TotalAddedToLightbox"]);

			//------------------------------------
			// Update the UI
			//------------------------------------
			SiteUtils.PopulateCell(UploadsCell, uploads, totalUploads);
			SiteUtils.PopulateCell(DownloadsCell, downloads, totalDownloads);
			SiteUtils.PopulateCell(OrderRequestsCell, orderRequests, totalOrderRequests);
			SiteUtils.PopulateCell(ViewsCell, views, totalViews);
			SiteUtils.PopulateCell(SearchHitsCell, searchHits, totalSearchHits);
			SiteUtils.PopulateCell(AddToCartCell, addedToCart, totalAddedToCart);
			SiteUtils.PopulateCell(AddToLightboxCell, addedToLightbox, totalAddedToLightbox);

			//------------------------------------
			// Update the chart headers
			//------------------------------------
			ChartBrandHeaderLabel.Text = (BrandDropDownList.SelectedId > 0) ? BrandDropDownList.SelectedItem.Text : "All Brands";
		}

		private void DisplayPanel(string panel)
		{
			ViewAllAssetsLinkButton1.Enabled = true;
			ViewAllAssetsLinkButton2.Enabled = true;
			ViewActivitySummaryLinkButton1.Enabled = true;
			ViewActivitySummaryLinkButton2.Enabled = true;
			
			switch (panel)
			{
				case "assetlist":

					PerformSearchAndBindGrid(1);

					ViewAllAssetsLinkButton1.Enabled = false;
					ViewAllAssetsLinkButton2.Enabled = false;

					AssetListPanel.Visible = true;
					ActivitySummaryPanel.Visible = false;

					TopPageControlsPanel.Visible = true;
					BottomPageControlsPanel.Visible = true;

					break;

				case "activitysummary":

					PopulateActivitySummary();

					ViewActivitySummaryLinkButton1.Enabled = false;
					ViewActivitySummaryLinkButton2.Enabled = false;

					AssetListPanel.Visible = false;
					ActivitySummaryPanel.Visible = true;

					TopPageControlsPanel.Visible = false;
					BottomPageControlsPanel.Visible = false;

					break;

				default:

					throw new ArgumentException("Unknown argument", panel);
			}
		}

		private void ExportToCSV()
		{
			AssetFinder finder = GetFinderFromForm();
			EntityList<Asset> assetList = Asset.FindMany(finder);

			var fileGen = new EntityListCsvExporter(assetList);

			fileGen.AddFieldMapping("PopularityRank", "Rank");
			fileGen.AddFieldMapping("AssetId", "Ref");
			fileGen.AddFieldMapping("AssetTypeName", "Type");
			fileGen.AddFieldMapping("UploadedByUserName", "Upload User");
			fileGen.AddFieldMapping("Title", "Title");

		    var exporter = new Exporter("AuditAssetHistory.csv", fileGen);
		    
            exporter.Export();
		}

		#endregion
	}
}