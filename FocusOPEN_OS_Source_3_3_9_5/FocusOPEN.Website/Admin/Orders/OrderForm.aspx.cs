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
	public partial class OrderForm : BaseAdminPageableEntityListPage<OrderItemFinder, OrderItem>
	{
		#region Private Helper Methods

		private Order GetOrder(int orderId)
		{
			Order o = Context.Items["Order"] as Order;

			if (o == null || o.OrderId.GetValueOrDefault(0) != orderId)
				o = Order.Get(orderId);

			return o;
		}

		private void HideProcessButtonIfComplete(Order order)
		{
			// Only show process button if order is not complete
			ProcessButton.Visible = !order.CompletionDate.HasValue;
		}

		#endregion

		#region Accessors

		private int OrderId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "OrderId", -1);
			}
			set
			{
				ViewState["OrderId"] = value;
			}
		}


		#endregion

		#region BaseAdminPageablePage Implementation

		protected override ISortableControl DataControl
		{
			get
			{
				return OrderItemsRepeater;
			}
		}

		protected override BaseSavedAdminSearch<OrderItemFinder> SavedSearch
		{
			get
			{
				return new BaseSavedAdminSearch<OrderItemFinder>();
			}
		}

		protected override OrderItemFinder GetFinderFromForm()
		{
			OrderItemFinder finder = new OrderItemFinder { OrderId = OrderId };
			finder.SortExpressions.Add(new AscendingSort(OrderItem.Columns.OrderItemId));

			if (CurrentUser.UserRole == UserRole.BrandAdministrator)
				finder.AssetBrandId = CurrentUser.PrimaryBrandId;

			if (CurrentUser.UserRole == UserRole.UploadUser)
				finder.AssetUploadUserId = CurrentUser.UserId.GetValueOrDefault();

			return finder;
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			throw new NotImplementedException();
		}

		protected override EntityList<OrderItem> GetData(OrderItemFinder finder, int page)
		{
			return OrderItem.FindMany(finder, page, PageSize);
		}

		protected override void TogglePanels(bool isEmptyResultSet)
		{
			ButtonPanel.Visible = (!isEmptyResultSet);
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int orderId = GetRequiredQueryStringParameter("OrderId", "OrderList.aspx");

				// Get the order.  No need to check permissions
				// as the security module will have already checked it
				Order order = GetOrder(orderId);

				if (order.IsNull)
				{
					Response.Redirect("OrderList.aspx", false);
					return;
				}

				// Put order ID in viewstate
				OrderId = orderId;

				OrderRefLabel.Text = order.OrderId.ToString();
				PlacedByUserNameLabel.Text = order.User.FullName;
				PlacedByUserBrandName.Text = order.User.PrimaryBrand.Name;
				PlacedByUserCompanyName.Text = order.User.CompanyName;

				HideProcessButtonIfComplete(order);
			}
		}

		protected void OrderItemsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					OrderItem orderItem = (OrderItem) e.Item.DataItem;
					Asset asset = orderItem.Asset;

					HiddenField OrderItemIdHiddenField = (HiddenField) e.Item.FindControl("OrderItemIdHiddenField");
					OrderItemIdHiddenField.Value = orderItem.OrderItemId.ToString();

					AssetThumbnail AssetThumbnail1 = (AssetThumbnail) e.Item.FindControl("AssetThumbnail1");
					AssetThumbnail1.Initialise(asset);

					AssetButtons AssetButtons1 = (AssetButtons) e.Item.FindControl("AssetButtons1");
					AssetButtons1.Initialise(asset);

					Label AssetIdLabel = (Label) e.Item.FindControl("AssetIdLabel");
					AssetIdLabel.Text = asset.AssetId.ToString();

					Label AssetDescriptionLabel = (Label) e.Item.FindControl("AssetDescriptionLabel");
					AssetDescriptionLabel.Text = SiteUtils.ConvertTextToHtml(asset.Description);

					Label BrandNameLabel = (Label) e.Item.FindControl("BrandNameLabel");
					BrandNameLabel.Text = asset.Brand.Name;

					Label AssetTypeLabel = (Label) e.Item.FindControl("AssetTypeLabel");
					AssetTypeLabel.Text = asset.AssetType.Name;

					Label AssetUsageRestrictionsLabel = (Label) e.Item.FindControl("AssetUsageRestrictionsLabel");
					AssetUsageRestrictionsLabel.Text = asset.UsageRestrictions;

					Label DeadlineDatelabel = (Label)e.Item.FindControl("DeadlineDateLabel");
					
					if (orderItem.RequiredByDate.HasValue)
						DeadlineDatelabel.Text = orderItem.RequiredByDate.Value.ToString(Global.DateFormat);

					Label UserNotesLabel = (Label)e.Item.FindControl("UserNotesLabel");
					UserNotesLabel.Text = orderItem.Notes;

					Label NoConversationLabel = (Label)e.Item.FindControl("NoConversationLabel");
					HtmlTableRow AddCommentRow = (HtmlTableRow)e.Item.FindControl("AddCommentRow");

					RadioButtonList OrderItemStatusRadioButtonList = (RadioButtonList) e.Item.FindControl("OrderItemStatusRadioButtonList");
					OrderItemStatusRadioButtonList.SelectedValue = orderItem.OrderItemStatusId.ToString();

					FeedbackLabel OrderItemStatusMessageLabel = (FeedbackLabel) e.Item.FindControl("OrderItemStatusMessageLabel");

					switch (orderItem.OrderItemStatus)
					{
						case OrderItemStatus.Preapproved:

							OrderItemStatusRadioButtonList.Visible = false;
							OrderItemStatusMessageLabel.SetSuccessMessage("Asset is preapproved");

							break;

						case OrderItemStatus.AwaitingApproval:
							{
								HtmlTableRow AssignedToRow = (HtmlTableRow)e.Item.FindControl("AssignedToRow");
								AssignedToRow.Visible = true;

								Label AssignedToLabel = (Label)e.Item.FindControl("AssignedToLabel");
								AssignedToLabel.Text = orderItem.AssignedToUser.FullName;
								AssignedToLabel.ToolTip = orderItem.AssignedToUser.UserRoleName;

								if (CurrentUser.UserRole == UserRole.SuperAdministrator)
								{
									// Do nothing.  Super-admins can action any order item
									// Keep the radio buttons visible.
								}
								else if (orderItem.AssignedToUserId == CurrentUser.UserId)
								{
									// Do nothing.  When an order item is assigned to the user viewing it,
									// they are able to action it as desired. Keep the radio buttons visible.
								}
								else
								{
									// Show who the order item is awaiting approval from.
									OrderItemStatusMessageLabel.SetErrorMessage("Awaiting approval from " + orderItem.AssignedToUser.FullName);

									// Hide the action radio buttons
									OrderItemStatusRadioButtonList.Visible = false;
								}
							}

							break;

						case OrderItemStatus.Approved:

							OrderItemStatusRadioButtonList.Visible = false;
							string message = string.Format("Approved by {0} on {1}", orderItem.StatusChangedByUser.FullName, orderItem.OrderItemStatusDate.GetValueOrDefault().ToString(Global.DateFormat));
							OrderItemStatusMessageLabel.SetSuccessMessage(message);

							break;

						case OrderItemStatus.Rejected:

							OrderItemStatusRadioButtonList.Visible = false;
							message = string.Format("Rejected by {0} on {1}", orderItem.StatusChangedByUser.FullName, orderItem.OrderItemStatusDate.GetValueOrDefault().ToString(Global.DateFormat));
							OrderItemStatusMessageLabel.SetErrorMessage(message);

							break;
					}

					if (GeneralUtils.ValueIsInList(orderItem.OrderItemStatus, OrderItemStatus.Preapproved, OrderItemStatus.Approved, OrderItemStatus.Rejected))
					{
						if (orderItem.OrderItemCommentList.Count == 0)
						{
							NoConversationLabel.Text = "(none)";
							NoConversationLabel.Font.Italic = true;
						}

						AddCommentRow.Visible = false;
						OrderItemStatusRadioButtonList.Visible = false;
					}

					if (StringUtils.IsBlank(DeadlineDatelabel.Text))
					{
						DeadlineDatelabel.Text = "(none)";
						DeadlineDatelabel.CssClass = "Italics";
					}

					if (StringUtils.IsBlank(AssetDescriptionLabel.Text))
					{
						AssetDescriptionLabel.Text = "(none)";
						AssetDescriptionLabel.Font.Italic = true;
					}

					if (StringUtils.IsBlank(UserNotesLabel.Text))
					{
						UserNotesLabel.Text = "(none)";
						UserNotesLabel.Font.Italic = true;
					}

					NoConversationLabel.Visible = (orderItem.OrderItemCommentList.Count == 0);

					Repeater ConversationRepeater = (Repeater)e.Item.FindControl("ConversationRepeater");
					ConversationRepeater.DataSource = orderItem.OrderItemCommentList;
					ConversationRepeater.DataBind();

					break;
			}
		}

		protected void ConversationRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:

					OrderItemComment orderItemComment = (OrderItemComment)e.Item.DataItem;

					Label UserNameLabel = (Label)e.Item.FindControl("UserNameLabel");
					UserNameLabel.Text = orderItemComment.UserFullName;

					Label DateLabel = (Label)e.Item.FindControl("DateLabel");
					DateLabel.Text = orderItemComment.CommentDate.ToString(Global.DateTimeFormat);

					Label CommentTextLabel = (Label)e.Item.FindControl("CommentTextLabel");
					CommentTextLabel.Text = SiteUtils.ConvertTextToHtml(orderItemComment.CommentText, e.Item.ItemType == ListItemType.Item ? "PosTxt" : "BodyTxt");

					break;
			}
		}

		protected void ProcessButton_Click(object sender, EventArgs e)
		{
			EntityList<OrderItem> orderItems = new EntityList<OrderItem>();

			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						// Get the controls
						HiddenField OrderItemIdHiddenField = (HiddenField)ri.FindControl("OrderItemIdHiddenField");
						TextBox CommentsTextBox = (TextBox)ri.FindControl("CommentsTextBox");
						RadioButtonList OrderItemStatusRadioButtonList = (RadioButtonList)ri.FindControl("OrderItemStatusRadioButtonList");

						// Get the values
						int orderItemId = NumericUtils.ParseInt32(OrderItemIdHiddenField.Value, 0);
						string comments = CommentsTextBox.Text;
						int orderItemStatusId = NumericUtils.ParseInt32(OrderItemStatusRadioButtonList.SelectedValue, 0);

						// Skip if the order item or status is missing
						if (orderItemId == 0 || orderItemStatusId == 0)
							continue;

						// Get the order item
						OrderItem orderItem = OrderItem.Get(orderItemId);

						// Assume we're not processing it
						bool process = false;

						// Check if a comment was entered
						if (!StringUtils.IsBlank(comments))
						{
							process = true;
							orderItem.AddComment(CurrentUser.UserId.GetValueOrDefault(), comments);
						}

						// .. Or the status has been changed
						if (orderItem.OrderItemStatusId != orderItemStatusId)
						{
							process = true;
							orderItem.OrderItemStatusId = orderItemStatusId;
							orderItem.OrderItemStatusDate = DateTime.Now;
							orderItem.OrderItemStatusUserId = CurrentUser.UserId;
						}

						// If something was changed, then add this order item to the list
						// of order items that we will submit for processing.
						if (process)
							orderItems.Add(orderItem);

						break;
				}
			}

			try
			{
				OrderManager.ProcessOrderItems(orderItems, CurrentUser);
				MessageLabel1.SetSuccessMessage("Selected items processed successfully");
				PerformSearchAndBindGrid(CurrentPage);

				// We know there's at least one order item or the OrderManager.ProcessOrderItems() method
				// will throw an InvalidOrderItemException which will be caught below.
				Order order = orderItems[0].Order;

				HideProcessButtonIfComplete(order);
			}
			catch (InvalidOrderItemException ioex)
			{
				MessageLabel1.SetErrorMessage(ioex.Message);
			}
		}

		#endregion
	}
}