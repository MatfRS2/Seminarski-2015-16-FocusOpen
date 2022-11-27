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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class UserList : BaseAdminPageableEntityListPage<UserFinder, User>
	{
		#region BaseAdminAdvancedPageablePage Implementation

		protected override BaseSavedAdminSearch<UserFinder> SavedSearch
		{
			get
			{
				return SessionInfo.Current.AdminSessionInfo.AdminSavedUserSearch;
			}
		}

		protected override ISortableControl DataControl
		{
			get
			{
				return UsersRepeater;
			}
		}


		protected override EntityList<User> GetData(UserFinder finder, int page)
		{
			CookieManager.SetValue("AdminSavedUserSearchPageSize", PageSize);
			return Data.User.FindMany(finder, page, PageSize);
		}

		protected override UserFinder GetFinderFromForm()
		{
			UserFinder finder = new UserFinder();

			if (CurrentUser.UserRole == UserRole.BrandAdministrator)
			{
				finder.PrimaryBrandId = CurrentUser.PrimaryBrandId;
				finder.BrandAdministratorId = CurrentUser.UserId.GetValueOrDefault(-1);
			}

			finder.SortExpressions.AddRange(UsersRepeater.GetSortExpressions());

			if (EmailTextBox.Text.Trim() != string.Empty)
				finder.FuzzyEmail = EmailTextBox.Text.Trim();

			finder.ExpiredAndSuspendedAccountsOnly = ExpiredAndSuspendedAccountsOnlyCheckBox.Checked;

			if (BrandRow.Visible)
				finder.PrimaryBrandId = BrandIdDropdown.SelectedId;

			return (finder);
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			UserFinder finder = SavedSearch.Finder;

			EmailTextBox.Text = finder.FuzzyEmail;
			BrandIdDropdown.SafeSelectValue(finder.PrimaryBrandId);
			ExpiredAndSuspendedAccountsOnlyCheckBox.Checked = finder.ExpiredAndSuspendedAccountsOnly;

			DataControl.SortExpression = SavedSearch.SortExpression;
			DataControl.SortAscending = SavedSearch.SortAscending;

			int page = SavedSearch.Page;
			PerformSearchAndBindGrid(finder, page);
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteUtils.TieButton(EmailTextBox, SearchButton);
			
			// Only show the brand row for superadmins and where there is more than one brand
			BrandRow.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);
		}

		protected void UsersRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Header):

					HtmlTableCell HeaderCell5 = (HtmlTableCell) e.Item.FindControl("HeaderCell5");
					HeaderCell5.Visible = (BrandManager.IsMultipleBrandMode);

					break;

				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					User user = (User) e.Item.DataItem;

					// Toggle brand name column
					HtmlTableCell BrandNameRow = (HtmlTableCell) e.Item.FindControl("BrandNameRow");
					BrandNameRow.Visible = (BrandManager.IsMultipleBrandMode);

					// Get controls
					HyperLink EditUserHyperLink = (HyperLink) e.Item.FindControl("EditUserHyperLink");
					Label UserTypeNameLabel = (Label) e.Item.FindControl("UserTypeNameLabel");
					HtmlTableCell StatsEditCell = (HtmlTableCell) e.Item.FindControl("StatsEditCell");
					Label BrandNameLabel = (Label) e.Item.FindControl("BrandNameLabel");
					Label IsExpiredLabel = (Label) e.Item.FindControl("IsExpiredLabel");
					Label IsSuspendedLabel = (Label) e.Item.FindControl("IsSuspendedLabel");
					Label LastLoginDateLabel = (Label) e.Item.FindControl("LastLoginDateLabel");
					EnhancedHyperLink StatsHyperLink = (EnhancedHyperLink) e.Item.FindControl("StatsHyperLink");

					EditUserHyperLink.Text = user.FullName;
					EditUserHyperLink.NavigateUrl = "UserForm.aspx?userId=" + user.UserId;
					UserTypeNameLabel.Text = EnumUtils.GetDescription(user.UserRole);
					BrandNameLabel.Text = SiteUtils.GetBrandName(user);
					IsExpiredLabel.Text = (user.GetAccountExpiryDate() < DateTime.Now) ? "Y" : "N";
					IsSuspendedLabel.Text = (user.IsSuspended) ? "Y" : "N";
					LastLoginDateLabel.Text = user.LastLoginDate.HasValue ? user.LastLoginDate.Value.ToString(Global.DateFormat) : "Never";

					// Only show the stats link if the current user has access to the audit trail page
					SiteUtils.SetHyperLink(CurrentUser, "Reports.AuditTrail", string.Format("?email={0}", user.Email), StatsHyperLink);

					if (IsExpiredLabel.Text == "Y")
						IsExpiredLabel.CssClass = "bold";

					if (IsSuspendedLabel.Text == "Y")
						IsSuspendedLabel.CssClass = "bold";

					if (!EntitySecurityManager.CanManageUser(CurrentUser, user))
					{
						EditUserHyperLink.NavigateUrl = string.Empty;
						StatsEditCell.InnerHtml = "&nbsp;";
					}

					break;
			}
		}

		#endregion
	}
}