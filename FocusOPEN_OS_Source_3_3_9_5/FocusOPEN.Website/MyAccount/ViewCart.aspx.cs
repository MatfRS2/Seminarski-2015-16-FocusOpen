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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.MyAccount
{
	public partial class ViewCart : BasePageablePage
	{
		#region Constructor

		protected ViewCart()
		{
			PageStatePersisterType = PersisterType.ZipCompressed;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Returns the ID's of selected assets
		/// </summary>
		private List<Int32> SelectedCartItemIdList
		{
			get
			{
				List<Int32> list = ViewState["SelectedCartItemIdList"] as List<Int32>;

				if (list == null)
				{
					list = new List<Int32>();
					ViewState["SelectedCartItemIdList"] = list;
				}

				return list;
			}
		}

		private bool AllAssetsSelected
		{
			get
			{
				return (SelectAllAssetsInCart1.Checked || SelectAllAssetsInCart2.Checked);
			}
			set
			{
				SelectAllAssetsInCart1.Checked = value;
				SelectAllAssetsInCart2.Checked = value;

				if (value)
				{
					foreach (Cart cart in ContextInfo.CartManager.CartList)
					{
						int cartId = cart.CartId.GetValueOrDefault();

						if (!SelectedCartItemIdList.Contains(cartId))
							SelectedCartItemIdList.Add(cartId);
					}
				}
			}
		}

        protected override ContextType AssetContext
        {
            get
            {
                return ContextType.Cart;
            }
        }


		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				PageSizeDropDownList1.SafeSelectValue(SessionInfo.Current.UserSessionInfo.CartPageSize);
				Bind(1);
			}
		}

		protected void SelectAssetCheckBox_CheckChanged(object sender, EventArgs e)
		{
			CheckBox SelectAssetCheckBox = (CheckBox) sender;

			foreach (RepeaterItem ri in CartRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						Control cb = ri.FindControl(SelectAssetCheckBox.ID);

						if (cb != null && cb.UniqueID == SelectAssetCheckBox.UniqueID)
						{
							HiddenField CartIdHiddenField = (HiddenField) ri.FindControl("CartIdHiddenField");
							int cartId = Convert.ToInt32(CartIdHiddenField.Value);

							if (SelectAssetCheckBox.Checked)
							{
								if (!SelectedCartItemIdList.Contains(cartId))
									SelectedCartItemIdList.Add(cartId);
							}
							else
							{
								if (SelectedCartItemIdList.Contains(cartId))
									SelectedCartItemIdList.Remove(cartId);

								AllAssetsSelected = false;
							}
						}

						break;
				}
			}
		}

		protected void CartRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Cart cart = (Cart) e.Item.DataItem;
					Asset asset = cart.Asset;

					HiddenField AssetIdHiddenField = (HiddenField) e.Item.FindControl("AssetIdHiddenField");
					HiddenField CartIdHiddenField = (HiddenField) e.Item.FindControl("CartIdHiddenField");

					AssetIdHiddenField.Value = asset.AssetId.ToString();
					CartIdHiddenField.Value = cart.CartId.ToString();

					AssetThumbnail AssetThumbnail1 = (AssetThumbnail) e.Item.FindControl("AssetThumbnail1");
					AssetButtons AssetButtons1 = (AssetButtons) e.Item.FindControl("AssetButtons1");

					AssetThumbnail1.Initialise(asset);
					AssetButtons1.Initialise(asset);

					HtmlTableCell AssetIdCell = (HtmlTableCell) e.Item.FindControl("AssetIdCell");
					HtmlTableCell AssetShortDescriptionCell = (HtmlTableCell) e.Item.FindControl("AssetShortDescriptionCell");
					HtmlTableCell AssetBrandNameCell = (HtmlTableCell)e.Item.FindControl("AssetBrandNameCell");
					HtmlTableCell AssetTypeCell = (HtmlTableCell) e.Item.FindControl("AssetTypeCell");
					HtmlTableCell UsageRestrictionsCell = (HtmlTableCell) e.Item.FindControl("UsageRestrictionsCell");
					EmailHyperLink AssetContactEmailHyperLink = (EmailHyperLink) e.Item.FindControl("AssetContactEmailHyperLink");
					DatePicker DateRequiredPicker = (DatePicker) e.Item.FindControl("DateRequiredPicker");
					TextBox NotesTextBox = (TextBox) e.Item.FindControl("NotesTextBox");
					CheckBox SelectAssetCheckBox = (CheckBox) e.Item.FindControl("SelectAssetCheckBox");
					FeedbackLabel CartItemMessageLabel = (FeedbackLabel) e.Item.FindControl("CartItemMessageLabel");
					HtmlTableRow IntendedUseRow = (HtmlTableRow) e.Item.FindControl("IntendedUseRow");
					PlaceHolder SaveNotesButtonPlaceHolder = (PlaceHolder) e.Item.FindControl("SaveNotesButtonPlaceHolder");

					AssetIdCell.InnerText = asset.AssetId.ToString();
					AssetShortDescriptionCell.InnerHtml = SiteUtils.ConvertTextToHtml(asset.Description);
					AssetBrandNameCell.InnerText = asset.Brand.Name;
					AssetTypeCell.InnerText = asset.AssetType.Name;
					UsageRestrictionsCell.InnerHtml = asset.UsageRestrictions;
					AssetContactEmailHyperLink.EmailAddress = asset.ContactEmail;
					DateRequiredPicker.SelectedDate = cart.RequiredByDate;
					NotesTextBox.Text = cart.Notes;
					SelectAssetCheckBox.Checked = IsCartItemSelected(cart.CartId.GetValueOrDefault());

					// Ensure blank cells have content
					SiteUtils.PopulateBlankControl(AssetShortDescriptionCell);
					SiteUtils.PopulateBlankControl(UsageRestrictionsCell);

					AssetStatus assetStatus = AssetManager.GetAssetStatusForUser(asset, CurrentUser);
					bool isRestricted = EntitySecurityManager.IsAssetRestricted(CurrentUser, asset);

					switch (assetStatus)
					{
						case AssetStatus.Withdrawn:

							CartItemMessageLabel.MessageType = BaseMessageLabel.MessageTypes.Withdrawn;
							CartItemMessageLabel.Header = "this asset is not available for ordering";
							IntendedUseRow.Visible = false;
							SaveNotesButtonPlaceHolder.Visible = false;
							break;

						case AssetStatus.Expired:

							CartItemMessageLabel.MessageType = BaseMessageLabel.MessageTypes.Withdrawn;
							CartItemMessageLabel.Header = "this asset is not available for ordering";
							IntendedUseRow.Visible = false;
							SaveNotesButtonPlaceHolder.Visible = false;
							break;

						case AssetStatus.Available:

							if (isRestricted)
							{
								CartItemMessageLabel.MessageType = BaseMessageLabel.MessageTypes.Negative;
								CartItemMessageLabel.Header = "approval required";
								CartItemMessageLabel.Text = "Approval is required to use this asset. State intended usage in the notes (you may be asked for further information).";
								IntendedUseRow.Visible = true;
								SaveNotesButtonPlaceHolder.Visible = true;
							}
							else
							{
								CartItemMessageLabel.MessageType = BaseMessageLabel.MessageTypes.Positive;
								CartItemMessageLabel.Header = "no approval required";
								CartItemMessageLabel.Text = "Approval is not required to download and use this asset.";
								IntendedUseRow.Visible = false;
								SaveNotesButtonPlaceHolder.Visible = false;
							}

							break;
					}

					// Only show the date required selector if the asset is available but restricted
					HtmlTableRow DateRequiredRow = (HtmlTableRow) e.Item.FindControl("DateRequiredRow");
					DateRequiredRow.Visible = (assetStatus == AssetStatus.Available && isRestricted);

					break;
			}
		}

		protected void PageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			Bind(1);
		}

		protected void Page_IndexChanged(object sender, CommandEventArgs e)
		{
			int pg = Convert.ToInt32(e.CommandArgument);
			Bind(pg);
		}

		protected void SelectAllAssetsInCart_CheckChanged(object sender, EventArgs e)
		{
			CheckBox o = (CheckBox) sender;
			AllAssetsSelected = o.Checked;
			ToggleSelectedAssets();
		}

		protected void RemoveSelectedAssetsLinkButton_Click(object sender, EventArgs e)
		{
			if (SelectedCartItemIdList.Count == 0)
			{
				FeedbackLabel1.SetErrorMessage("no assets selected");
				return;
			}

			foreach (int cartId in SelectedCartItemIdList)
				ContextInfo.CartManager.RemoveCartItemFromCart(cartId);

			SelectedCartItemIdList.Clear();
			FeedbackLabel1.SetSuccessMessage("selected assets removed from cart");
			Bind(CurrentPage);
		}

		protected void DeselectAllLinkButton_Click(object sender, EventArgs e)
		{
			AllAssetsSelected = false;
			SelectedCartItemIdList.Clear();
			ToggleSelectedAssets();
		}

		protected void ProcessCartButton_Click(object sender, EventArgs e)
		{
			if (!AgreeTermsConditionsCheckBox.Checked)
			{
				FeedbackLabel2.SetErrorMessage("you must agree to Terms & Conditions of Use before your order can be processed");
				return;
			}

			try
			{
				SaveNotes();
				Order order = OrderManager.CreateOrderFromCart(CurrentUser);
				Response.Redirect("ViewOrders.aspx?orderId=" + order.OrderId);
			}
			catch (InvalidOrderException ioex)
			{
				FeedbackLabel1.SetErrorMessage(ioex.Message);
			}
		}

		protected void AssetButtons1_CartButtonClicked(object sender, EventArgs e)
		{
			Bind(CurrentPage);
		}

		protected void SaveNotesButton_Click(object sender, EventArgs e)
		{
			SaveNotes();
		}

		protected void CopyNotesToAllButton_Click(object sender, EventArgs e)
		{
			bool found = false;

			DateTime? dt = null;
			string notes = string.Empty;

			Button btn = (Button)sender;

			foreach (RepeaterItem ri in CartRepeater.Items)
			{
				if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
				{
					Button CopyNotesToAllButton = (Button)ri.FindControl("CopyNotesToAllButton");

					if (CopyNotesToAllButton.UniqueID == btn.UniqueID)
					{
						DatePicker DateRequiredPicker = (DatePicker)ri.FindControl("DateRequiredPicker");
						TextArea NotesTextBox = (TextArea)ri.FindControl("NotesTextBox");

						dt = DateRequiredPicker.SelectedDate;
						notes = NotesTextBox.Text;

						found = true;

						break;
					}
				}
			}

			if (found)
			{
				foreach (Cart cart in ContextInfo.CartManager.CartList)
				{
					cart.RequiredByDate = dt;
					cart.Notes = notes;
					Cart.Update(cart);
				}
			}

			Bind(CurrentPage);
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Checks if the order item with the specified ID is currently selected on the page
		/// </summary>
		private bool IsCartItemSelectedOnPage(int cartId)
		{
			foreach (RepeaterItem ri in CartRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						HiddenField CartIdHiddenField = (HiddenField)ri.FindControl("CartIdHiddenField");
						int currentCartId = Convert.ToInt32(CartIdHiddenField.Value);

						if (currentCartId == cartId)
						{
							CheckBox SelectAssetCheckBox = (CheckBox)ri.FindControl("SelectAssetCheckBox");
							return (SelectAssetCheckBox.Checked);
						}

						break;
				}
			}

			return false;
		}

		private bool IsCartItemSelected(int cartId)
		{
			return (AllAssetsSelected || SelectedCartItemIdList.Contains(cartId) || IsCartItemSelectedOnPage(cartId));
		}

		private void ToggleSelectedAssets()
		{
			foreach (RepeaterItem ri in CartRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						CheckBox SelectAssetCheckBox = (CheckBox) ri.FindControl("SelectAssetCheckBox");
						SelectAssetCheckBox.Checked = AllAssetsSelected;

						break;
				}
			}
		}

		private void SaveNotes()
		{
			foreach (RepeaterItem ri in CartRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						HiddenField CartIdHiddenField = (HiddenField) ri.FindControl("CartIdHiddenField");
						int cartId = Convert.ToInt32(CartIdHiddenField.Value);

						TextBox NotesTextBox = (TextBox) ri.FindControl("NotesTextBox");
						string notes = NotesTextBox.Text.Trim();

						DatePicker DateRequiredPicker = (DatePicker) ri.FindControl("DateRequiredPicker");
						DateTime? requiredByDate = DateRequiredPicker.SelectedDate;

						Cart cart = ContextInfo.CartManager.GetCartById(cartId);
						cart.Notes = notes;
						cart.RequiredByDate = requiredByDate;
						Cart.Update(cart);

						FeedbackLabel1.SetSuccessMessage("notes updated");

						break;
				}
			}
		}

		private void Bind(int page)
		{
			// First get all of the assets in the cart
			CartFinder finder = new CartFinder {UserId = CurrentUser.UserId.GetValueOrDefault()};
			finder.SortExpressions.Add(new DescendingSort(Cart.Columns.DateAdded.ToString()));
			finder.SortExpressions.Add(new DescendingSort(Cart.Columns.CartId.ToString()));
			List<Cart> items = Cart.FindMany(finder);

			// Filter out those assets that are not accessible by the current user
			var validItems = (from item in items
			                  where EntitySecurityManager.CanViewAssetInfo(CurrentUser, item.Asset)
			                  select item);

			// Get the asset count
			int total = validItems.Count();

			// Set the page size
			SessionInfo.Current.UserSessionInfo.CartPageSize = PageSize;

			// Bind the asset list to relevant page
			CartRepeater.DataSource = items;
			CartRepeater.DataBind();

			// Setup the top pager
			TopPager.CurrentPage = page;
			TopPager.PageSize = PageSize;
			TopPager.ItemCount = total;
			TopPager.CalculatePageCount();

			// Setup the bottom pager
			BottomPager.CurrentPage = TopPager.CurrentPage;
			BottomPager.PageSize = TopPager.PageSize;
			BottomPager.ItemCount = TopPager.ItemCount;
			BottomPager.CalculatePageCount();

			// Setup page variables
			CurrentPage = page;
			AssetCount = total;
			TotalPages = TopPager.PageCount;

			// Toggle no results panel
			CartResultsPanel.Visible = (items.Count > 0);
			CartNoResultsPanel.Visible = (items.Count == 0);
		}

		#endregion
	}
}