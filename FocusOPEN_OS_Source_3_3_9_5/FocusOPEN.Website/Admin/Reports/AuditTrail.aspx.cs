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
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Components.Exporting;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class AuditTrail : BaseAdminPageableEntityListPage<AuditUserHistoryFinder, AuditUserHistory>
	{
		#region Constructor

		protected AuditTrail()
		{
			BindOnLoad = false;
		}

		#endregion

		#region BaseAdminPageableEntityListPage Implementation

		protected override ISortableControl DataControl
		{
			get
			{
				return AuditUserHistoryRepeater;
			}
		}

		protected override BaseSavedAdminSearch<AuditUserHistoryFinder> SavedSearch
		{
			get
			{
				return SessionInfo.Current.AdminSessionInfo.AdminSavedAuditUserHistorySearch;
			}
		}

		protected override AuditUserHistoryFinder GetFinderFromForm()
		{
			AuditUserHistoryFinder finder = new AuditUserHistoryFinder();
			finder.SortExpressions.AddRange(AuditUserHistoryRepeater.GetSortExpressions());

			finder.Keyword = KeywordTextBox.Text.Trim();
			finder.UserEmail = UserEmailTextBox.Text.Trim();
			finder.AuditUserActionId = AuditUserActionDropDownList1.SelectedId;
			finder.IpAddress = IpAddressTextBox.Text.Trim();
			finder.StartDate = StartDatePicker.SelectedDate;
			finder.EndDate = EndDatePicker.SelectedDate;

			if (BrandSelectorWrapper.Visible)
			{
				finder.PrimaryBrandId = null;

				if (BrandDropDownList.SelectedId > 0)
					finder.PrimaryBrandId = BrandDropDownList.SelectedId;
			}
			else
			{
				finder.PrimaryBrandId = CurrentUser.PrimaryBrandId;
			}

			return finder;
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			AuditUserHistoryFinder finder = SavedSearch.Finder;

			KeywordTextBox.Text = finder.Keyword;
			UserEmailTextBox.Text = finder.UserEmail;
			AuditUserActionDropDownList1.SafeSelectValue(finder.AuditUserActionId);
			IpAddressTextBox.Text = finder.IpAddress;
			StartDatePicker.SelectedDate = finder.StartDate;
			EndDatePicker.SelectedDate = finder.EndDate;

			DataControl.SortExpression = SavedSearch.SortExpression;
			DataControl.SortAscending = SavedSearch.SortAscending;

			int page = SavedSearch.Page;
			PerformSearchAndBindGrid(finder, page);
		}

		protected override EntityList<AuditUserHistory> GetData(AuditUserHistoryFinder finder, int page)
		{
			return AuditUserHistory.FindMany(finder, page, PageSize);
		}

		protected override void PerformSearchAndBindGrid(AuditUserHistoryFinder finder, int page)
		{
			InitialPanel.Visible = false;
			ResultsPanel.Visible = true;

			InitialFooterPanel.Visible = false;
			ResultsFooterPanel.Visible = true;

			base.PerformSearchAndBindGrid(finder, page);
		}

		#endregion

		#region Handled Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// Only show brand selector to super-admins when there is more than one brand available
				BrandSelectorWrapper.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);

				if (WebUtils.GetRequestParam("email", string.Empty) != string.Empty)
				{
					SavedSearch.Reset();
					UserEmailTextBox.Text = WebUtils.GetRequestParam("email", string.Empty);
					PerformSearchAndBindGrid(1);
				}
			}
		}

		protected void GenerateReportButton_Click(object sender, EventArgs e)
		{
			if (ViewOnPageRadioButton.Checked)
			{
				SearchButton_Click(sender, e);
			}
			else if (DownloadRadioButton.Checked)
			{
				ExportToCSV();
			}
		}

		protected void AuditUserHistoryRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					// Get data item
					AuditUserHistory auditUserHistory = (AuditUserHistory) e.Item.DataItem;

					// Get controls
					HyperLink UserNameHyperLink = (HyperLink)e.Item.FindControl("UserNameHyperLink");
					Label EventDescriptionLabel = (Label) e.Item.FindControl("EventDescriptionLabel");
					Label IpAddressLabel = (Label) e.Item.FindControl("IpAddressLabel");
					Label DateTimeLabel = (Label) e.Item.FindControl("DateTimeLabel");
					Label NotesLabel = (Label) e.Item.FindControl("NotesLabel");

					// Populate them
					UserNameHyperLink.Text = auditUserHistory.UserName;
					UserNameHyperLink.NavigateUrl = "~/Admin/Users/UserForm.aspx?UserId=" + auditUserHistory.UserId;
					UserNameHyperLink.ToolTip = auditUserHistory.UserEmail;
					EventDescriptionLabel.Text = auditUserHistory.Description;
					IpAddressLabel.Text = auditUserHistory.IpAddress;
					DateTimeLabel.Text = auditUserHistory.Date.ToString(Global.DateTimeFormat);
					NotesLabel.Text = auditUserHistory.Notes;

					// Populate blank
					SiteUtils.PopulateBlankControl(NotesLabel);

					break;
			}
		}

		protected void DownloadCSVButton_Click(object sender, EventArgs e)
		{
			ExportToCSV();
		}

		#endregion

		#region Helper Methods

		private void ExportToCSV()
		{
			AuditUserHistoryFinder finder = GetFinderFromForm();
			EntityList<AuditUserHistory> auditUserHistory = AuditUserHistory.FindMany(finder);

			var fileGen = new EntityListCsvExporter(auditUserHistory);

			fileGen.AddFieldMapping("UserEmail", "User");
			fileGen.AddFieldMapping("Description", "Event");
			fileGen.AddFieldMapping("IpAddress", "IpAddress");
			fileGen.AddFieldMapping("Date", "Date & Time");
			fileGen.AddFieldMapping("Notes", "Notes");

		    var exporter = new Exporter("AuditUserHistory.csv", fileGen);

            exporter.Export();
		}

		#endregion
	}
}