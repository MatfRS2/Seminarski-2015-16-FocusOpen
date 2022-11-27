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
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class OrderList : BaseAdminPageableEntityListPage<OrderFinder, Order>
	{
		#region BaseAdminPageableEntityListPage Implementation

		protected override ISortableControl DataControl
		{
			get
			{
				return OrdersRepeater;
			}
		}

		protected override BaseSavedAdminSearch<OrderFinder> SavedSearch
		{
			get
			{
				return SessionInfo.Current.AdminSessionInfo.AdminSavedOrderSearch;
			}
		}

		protected override OrderFinder GetFinderFromForm()
		{
			OrderFinder finder = GetBaseFinder();
			finder.SortExpressions.AddRange(OrdersRepeater.GetSortExpressions());

			if (!StringUtils.IsBlank(GeneralKeywordTextBox.Text))
				finder.GeneralKeyword = GeneralKeywordTextBox.Text;

			if (BrandSelectorPanel.Visible)
			{
				if (BrandIdDropdown.SelectedId > 0)
					finder.UserPrimaryBrandId = BrandIdDropdown.SelectedId;
			}
			else
			{
				finder.UserPrimaryBrandId = CurrentUser.PrimaryBrandId;
			}

			if (ApplicantSelectorPanel.Visible && ApplicantDropDownList.GetSelectedId() != 0)
				finder.UserId = ApplicantDropDownList.GetSelectedId();

			if (PendingOrdersOnlyCheckBox.Checked)
				finder.PendingOrdersOnly = true;

			return (finder);
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			OrderFinder finder = SavedSearch.Finder;

			GeneralKeywordTextBox.Text = finder.GeneralKeyword;
			BrandIdDropdown.SafeSelectValue(finder.ContainingAssetsFromBrandId);
			ApplicantDropDownList.SafeSelectValue(finder.UserId);
			PendingOrdersOnlyCheckBox.Checked = (finder.PendingOrdersOnly.HasValue && finder.PendingOrdersOnly.Value);

			DataControl.SortExpression = SavedSearch.SortExpression;
			DataControl.SortAscending = SavedSearch.SortAscending;

			int page = SavedSearch.Page;
			PerformSearchAndBindGrid(finder, page);
		}

		protected override EntityList<Order> GetData(OrderFinder finder, int page)
		{
			CookieManager.SetValue("AdminSavedOrderSearchPageSize", PageSize);
			return Order.FindMany(finder, page, PageSize);
		}

		#endregion

		#region Handled Events

		protected void Page_Init(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// Only show brand selector to super-admins when there is more than one brand available
				BrandSelectorPanel.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);

				// Populate applicant dropdown
				BindApplicantDropDownList();
			}
		}

		protected void BrandIdDropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindApplicantDropDownList();
		}

		protected void OrdersRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Order order = (Order) e.Item.DataItem;

					HyperLink OrderIdHyperLink = (HyperLink) e.Item.FindControl("OrderIdHyperLink");
					OrderIdHyperLink.Text = order.OrderId.ToString();
					OrderIdHyperLink.NavigateUrl = "OrderForm.aspx?orderId=" + order.OrderId;

					Label AssetCountLabel = (Label) e.Item.FindControl("AssetCountLabel");
					AssetCountLabel.Text = order.OrderItemList.Count.ToString();

					Label UserNameLabel = (Label) e.Item.FindControl("UserNameLabel");
					UserNameLabel.Text = order.User.FullName;

					Label UserBrandLabel = (Label) e.Item.FindControl("UserBrandLabel");
					UserBrandLabel.Text = order.User.PrimaryBrand.Name;

					Label IsPendingLabel = (Label) e.Item.FindControl("IsPendingLabel");
					IsPendingLabel.Text = (order.CompletionDate.HasValue) ? "N" : "Y";

					if (IsPendingLabel.Text == "Y")
						IsPendingLabel.CssClass = "Bold";

					Label RequestDateLabel = (Label) e.Item.FindControl("RequestDateLabel");
					RequestDateLabel.Text = order.OrderDate.ToString(Global.DateFormat);

					Label DeadlineDateLabel = (Label) e.Item.FindControl("DeadlineDateLabel");
					DeadlineDateLabel.Text = order.DeadlineDate.HasValue ? order.DeadlineDate.Value.ToString(Global.DateFormat) : "None";

					if (DeadlineDateLabel.Text == "None")
						DeadlineDateLabel.CssClass = "Italics";

					break;
			}
		}

		#endregion

		#region Helper Methods

		private void BindApplicantDropDownList()
		{
			OrderFinder finder = GetBaseFinder();
			finder.PendingOrdersOnly = true;

			if (BrandIdDropdown.SelectedId > 0)
				finder.UserPrimaryBrandId = BrandIdDropdown.SelectedId;

			finder.SortExpressions.Add(new AscendingSort("UserName"));
			List<Order> orders = Order.FindMany(finder);

			ApplicantDropDownList.Items.Clear();
			ApplicantDropDownList.Items.Add(new ListItem("All", "0"));

			var users = (from o in orders
			             select new { o.UserId, o.UserName}).Distinct();

			foreach (var u in users)
				ApplicantDropDownList.Items.Add(new ListItem(u.UserName, u.UserId.ToString()));
		}

		private static OrderFinder GetBaseFinder()
		{
			OrderFinder finder = new OrderFinder();

			switch (CurrentUser.UserRole)
			{
				case (UserRole.SuperAdministrator):

					// Do nothing
					break;

				case (UserRole.BrandAdministrator):

					// Do nothing
					break;

				case (UserRole.UploadUser):

					finder.ContainingAssetsUploadedByUserId = CurrentUser.UserId.GetValueOrDefault();
					break;
			}

			return finder;
		}

		#endregion
	}
}