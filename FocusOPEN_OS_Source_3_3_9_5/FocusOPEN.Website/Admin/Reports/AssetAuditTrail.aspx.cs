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
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class AssetAuditTrail : BaseAdminPageableEntityListPage<AuditAssetHistoryFinder, AuditAssetHistory>
	{
		#region Constructor

		public AssetAuditTrail()
		{
			BindOnLoad = false;
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				ReturnToAssetListWrapper.Visible = (WebUtils.GetRequestParam("Source", string.Empty).ToLower() == "assetlist");

				if (WebUtils.GetIntRequestParam("AssetId", 0) != 0)
				{
					SavedSearch.Reset();
					AssetIdTextBox.Text = WebUtils.GetIntRequestParam("AssetId", 0).ToString();
					PerformSearchAndBindGrid(1);
				}
			}
		}

		protected void AuditAssetHistoryRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					// Get data item
					AuditAssetHistory auditAssetHistory = (AuditAssetHistory)e.Item.DataItem;

					// Get controls
					HyperLink AssetHyperlink = (HyperLink)e.Item.FindControl("AssetHyperlink");
					EmailHyperLink UserEmailHyperLink = (EmailHyperLink)e.Item.FindControl("UserEmailHyperLink");
					Label EventDescriptionLabel = (Label)e.Item.FindControl("EventDescriptionLabel");
					Label IpAddressLabel = (Label)e.Item.FindControl("IpAddressLabel");
					Label DateTimeLabel = (Label)e.Item.FindControl("DateTimeLabel");
					Label NotesLabel = (Label)e.Item.FindControl("NotesLabel");

					// Populate them
					AssetHyperlink.Text = auditAssetHistory.AssetId.ToString();
					AssetHyperlink.NavigateUrl = string.Format("~/Admin/Assets/AssetForm.aspx?AssetId={0}", auditAssetHistory.AssetId);
					UserEmailHyperLink.EmailAddress = auditAssetHistory.UserEmail;
					EventDescriptionLabel.Text = auditAssetHistory.Description;
					IpAddressLabel.Text = auditAssetHistory.IpAddress;
					DateTimeLabel.Text = auditAssetHistory.Date.ToString(Global.DateTimeFormat);
					NotesLabel.Text = auditAssetHistory.Notes;

					// Populate blank
					SiteUtils.PopulateBlankControl(NotesLabel);

					break;
			}
		}

		#endregion

		#region BaseAdminPageableEntityListPage Implementation

		protected override ISortableControl DataControl
		{
			get
			{
				return AuditAssetHistoryRepeater;
			}
		}

		protected override BaseSavedAdminSearch<AuditAssetHistoryFinder> SavedSearch
		{
			get
			{
				return new BaseSavedAdminSearch<AuditAssetHistoryFinder>();
			}
		}

		protected override AuditAssetHistoryFinder GetFinderFromForm()
		{
			AuditAssetHistoryFinder finder = new AuditAssetHistoryFinder();
			finder.SortExpressions.AddRange(AuditAssetHistoryRepeater.GetSortExpressions());

			// Brand admins can only see assets in their own brand
			if (CurrentUser.UserRole == UserRole.BrandAdministrator)
				finder.BrandId = CurrentUser.PrimaryBrandId;

			finder.AssetId = NumericUtils.ParseInt32(AssetIdTextBox.Text, 0);
			finder.UserEmail = UserEmailTextBox.Text.Trim();
			finder.AuditAssetActionId = AuditAssetActionDropDownList1.SelectedId;
			finder.IpAddress = IpAddressTextBox.Text.Trim();
			finder.StartDate = StartDatePicker.SelectedDate;
			finder.EndDate = EndDatePicker.SelectedDate;

			return finder;
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			throw new NotImplementedException();
		}

		protected override EntityList<AuditAssetHistory> GetData(AuditAssetHistoryFinder finder, int page)
		{
			return AuditAssetHistory.FindMany(finder, page, PageSize);
		}

		protected override void PerformSearchAndBindGrid(AuditAssetHistoryFinder finder, int page)
		{
			InitialPanel.Visible = false;
			ResultsPanel.Visible = true;

			InitialFooterPanel.Visible = false;
			ResultsFooterPanel.Visible = true;

			base.PerformSearchAndBindGrid(finder, page);
		}

		#endregion
	}
}