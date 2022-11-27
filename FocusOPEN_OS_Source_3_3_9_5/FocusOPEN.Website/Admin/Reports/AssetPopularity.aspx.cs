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

namespace FocusOPEN.Website.Admin
{
	public partial class AssetPopularity : BaseAdminPage
	{
		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				AssetReportMapper.Instance.UpdateAllAssetsPopularityRank();
			}
		}

		protected void GenerateReportButton_Click(object sender, EventArgs e)
		{
			BindRepeater();
		}

		protected void SortListLinkButton_Command(object sender, CommandEventArgs e)
		{
			AssetRepeater.UpdateSortExpression(e.CommandArgument.ToString());
			BindRepeater();
		}

		protected void TopRecordsDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindRepeater();
		}

		protected void AssetRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					// Get data item
					DataRowView row = (DataRowView) e.Item.DataItem;

					// Get controls
					HtmlTableCell PopularityRankCell = (HtmlTableCell) e.Item.FindControl("PopularityRankCell");
					HyperLink AssetIdHyperLink = (HyperLink) e.Item.FindControl("AssetIdHyperLink");
					HtmlTableCell AssetTypeNameCell = (HtmlTableCell) e.Item.FindControl("AssetTypeNameCell");
					HtmlTableCell UploadedByUserNameCell = (HtmlTableCell) e.Item.FindControl("UploadedByUserNameCell");
					HtmlTableCell TitleCell = (HtmlTableCell) e.Item.FindControl("TitleCell");
					HtmlTableCell DownloadCountCell = (HtmlTableCell) e.Item.FindControl("DownloadCountCell");
					HyperLink StatsHyperLink = (HyperLink) e.Item.FindControl("StatsHyperLink");

					// Get common values
					int assetId = Convert.ToInt32(row["AssetId"]);
					string assetStatsLink = "~/Admin/Reports/AssetStats.aspx?assetId=" + assetId;

					// Populate controls
					PopularityRankCell.InnerText = row["PopularityRankInTimeFrame"].ToString();
					AssetIdHyperLink.Text = assetId.ToString();
					AssetIdHyperLink.NavigateUrl = assetStatsLink;
					AssetTypeNameCell.InnerText = row["AssetTypeName"].ToString();
					UploadedByUserNameCell.InnerText = row["UploadedByUserName"].ToString();
					TitleCell.InnerText = row["Title"].ToString();
					DownloadCountCell.InnerText = row["DownloadCountInTimeFrame"].ToString();
					StatsHyperLink.NavigateUrl = assetStatsLink;

					// Populate blank cells
					SiteUtils.PopulateBlankControl(TitleCell);

					break;
			}
		}

		protected void DownloadCSVButton_Click(object sender, EventArgs e)
		{
			ExportToCSV();
		}

		#endregion

		#region Helper Methods

		private void BindRepeater()
		{
			DataTable dt = GetDataTable();

			AssetRepeater.DataSource = dt;
			AssetRepeater.DataBind();

			AssetRepeater.Visible = (dt.Rows.Count > 0);
			NoResultsPanel.Visible = (dt.Rows.Count == 0);

			InitialPanel.Visible = false;
			ResultsPanel.Visible = true;

			InitialFooterPanel.Visible = false;
			ResultsFooterPanel.Visible = true;
		}

		private DataTable GetDataTable()
		{
			int top = TopRecordsDropDownList1.SelectedId;
			int assetTypeId = AssetTypeDropDownList1.SelectedId;
			int brandId = BrandDropDownList.SelectedId;

			TimeFrame timeframe = EnumUtils.GetEnumFromValue<TimeFrame>(TimeFrameDropDownList1.SelectedId);
			DateRange dateRange = SiteUtils.GetDateRangeFromTimeFrame(timeframe);

			DataTable dt = AssetReportMapper.Instance.GetPopularAssetList(top, assetTypeId, brandId, dateRange);
			dt.DefaultView.Sort = AssetRepeater.GetSortExpressions()[0].ToString();

			return dt;
		}

		private void ExportToCSV()
		{
			DataTable dt = GetDataTable();
			var fileGen = new DataTableCsvExporter(dt);

			fileGen.AddFieldMapping("PopularityRankInTimeFrame", "Rank");
			fileGen.AddFieldMapping("AssetId", "Ref");
			fileGen.AddFieldMapping("AssetTypeName", "Type");
			fileGen.AddFieldMapping("UploadedByUserName", "Upload User");
			fileGen.AddFieldMapping("Title", "Title");
			fileGen.AddFieldMapping("DownloadCountInTimeFrame", "Downloads");

		    var exporter = new Exporter("AssetPopularity.csv", fileGen);

		    exporter.Export();
		}

		#endregion
	}
}