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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using System.Collections.Generic;

namespace FocusOPEN.Website.MyAccount
{
	public partial class ViewOrders : BasePageablePage
	{
		#region Constructor

		protected ViewOrders()
		{
			PageStatePersisterType = PersisterType.ZipCompressed;
		}

		#endregion

		#region Private variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private Order m_CurrentOrder;

		#endregion

		#region Accessors

		private SelectedOrderItemList SelectedOrderItems
		{
			get
			{
				SelectedOrderItemList selectedOrderItemList = ViewState["SelectedOrderItems"] as SelectedOrderItemList;

				if (selectedOrderItemList == null)
				{
					selectedOrderItemList = new SelectedOrderItemList();
					ViewState["SelectedOrderItems"] = selectedOrderItemList;
				}

				return selectedOrderItemList;
			}
		}

		private Order CurrentOrder
		{
			get
			{
				if (m_CurrentOrder == null)
					m_CurrentOrder = Order.Get(CurrentOrderId);

				return m_CurrentOrder;
			}
			set
			{
				m_CurrentOrder = value;
				CurrentOrderId = value.OrderId.GetValueOrDefault();
			}
		}

		public int CurrentOrderId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "CurrentOrderId", 0);
			}
			set
			{
				ViewState["CurrentOrderId"] = value;
			}
		}

		private bool AllAssetsSelected
		{
			get
			{
				return (SelectAllAssets1.Checked || SelectAllAssets2.Checked);
			}
			set
			{
				SelectAllAssets1.Checked = value;
				SelectAllAssets2.Checked = value;
			}
		}


        protected override ContextType AssetContext
        {
            get
            {
                return ContextType.OrderManager;
            }
        }

		#endregion

		#region Zip Download Stuff

		protected void DownloadAsZipLinkButton_Click(object sender, EventArgs e)
		{
			// Ensure assets are selected
			if (!AreAssetsSelected())
				return;

			ZipSelectedAssetsAndSend();
		}

		private void ZipSelectedAssetsAndSend()
		{
			int? currentOrderId = CurrentOrder.OrderId;

			m_Logger.DebugFormat("User: {0} has selected assets to zip from order: {1}", CurrentUser.FullName, currentOrderId);

			// The folder where the zip file should be stored
			string sessionFolder = SessionHelper.GetForCurrentSession().CreateSessionTempFolder();

			// Unique filename strings
			string dateString = DateTime.Now.ToString("yyyyMMddHHmmss");
			string guid = GeneralUtils.GetGuid();

			// The actual filename of the zip file on disk
			string outputFilename = string.Format("assets_from_order_{0}_{1}_{2}.zip", currentOrderId, dateString, guid);

			// The full path to the zip file on disk
			string outputPath = Path.Combine(sessionFolder, outputFilename);
			m_Logger.DebugFormat("Zip file will be stored in: {0}", outputPath);

			using (ZipOutputStream zos = new ZipOutputStream(File.Create(outputPath)))
			{
				try
				{
					zos.SetLevel(9);
					zos.SetComment(string.Format("Selected assets from order {0} on {1}", CurrentOrder.OrderId, CurrentOrder.OrderDate.ToString(Global.DateFormat)));

					foreach (OrderItem orderItem in CurrentOrder.OrderItemList)
					{
						if (Response.IsClientConnected && ShouldDownload(orderItem))
						{
							// Get the asset
							Asset asset = orderItem.Asset;

							// Get the path to the asset file
							AssetFileInfo info = new AssetFileInfo(asset);

							// Get actual path
							string path = info.FilePath;

							// Check if a different image format is being requested
							if (AssetTypeChecker.IsImage(asset.FileExtension))
							{
								SelectedOrderItem soi = SelectedOrderItems.Get(orderItem.OrderItemId.GetValueOrDefault());
								path = AssetImageManager.GetResizedAssetImage(asset, soi.AssetImageSizeId, soi.DownloadFormat, true);
							}

							// Filename
							string filename = string.Concat(Path.GetFileNameWithoutExtension(asset.Filename), "_", asset.FileReference, (Path.GetExtension(path) ?? ".unknown").ToLower());

							// Add the file to the generated zip
							AddFileToZipOutputStream(filename, path, zos);

							// Log this download for reporting
							AuditLogManager.LogAssetAction(asset, CurrentUser, AuditAssetAction.DownloadedAssetFile);
							AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.DownloadAsset, string.Format("Downloaded AssetId: {0} as part of zipped download for OrderId: {1}", asset.AssetId, CurrentOrder.OrderId));
						}
					}
				}
				finally
				{
					zos.Finish();
					zos.Close();
				}
			}

			string downloadUrl = string.Format("~/GetZipDownload.ashx?OrderId={0}&d={1}&guid={2}", currentOrderId, dateString, guid);
			Response.Redirect(downloadUrl);
		}

		private static void AddFileToZipOutputStream(string filename, string assetFilePath, ZipOutputStream zos)
		{
			if (assetFilePath != string.Empty && File.Exists(assetFilePath))
			{
				ZipEntry ze = new ZipEntry(filename);
				zos.PutNextEntry(ze);

				using (FileStream fs = File.OpenRead(assetFilePath))
				{
					int sourceBytes;

					// Buffer to read 10K bytes in chunk:
					byte[] buffer = new byte[10000];

					do
					{
						sourceBytes = fs.Read(buffer, 0, buffer.Length);
						zos.Write(buffer, 0, sourceBytes);
					}
					while (sourceBytes > 0);
				}

				m_Logger.DebugFormat(" - Added file to zip: {0}", assetFilePath);
			}
			else
			{
				m_Logger.DebugFormat(" - Didn't add file to zip: {0}. File path is empty or does not exist.", assetFilePath);
			}
		}

		#endregion

		#region Handled Methods

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				PageSizeDropDownList1.SafeSelectValue(SessionInfo.Current.UserSessionInfo.DownloadManagerPageSize);

				int orderId = WebUtils.GetIntRequestParam("orderId", 0);

				if (orderId == 0)
				{
					DisplayMostRecentOrder();
				}
				else
				{
					OrderFinder finder = new OrderFinder {OrderId = orderId, UserId = CurrentUser.UserId.GetValueOrDefault()};
					Order order = Order.FindOne(finder);

					DisplayOrder(order);
				}
			}
		}

		protected void ToggleApprovalDetailsLinkButton_Click(object sender, EventArgs e)
		{
			LinkButton btn = (LinkButton) sender;

			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						Control itemButton = ri.FindControl(btn.ID);

						// Check the unique ID as the panel is in a repeater so we need to make sure
						// that we're in the right repeater item before we toggle the panel
						if (itemButton != null && itemButton.UniqueID == btn.UniqueID)
						{
							Panel ApprovalDetailsPanel = (Panel) ri.FindControl("ApprovalDetailsPanel");

							if (ApprovalDetailsPanel.Visible)
							{
								ApprovalDetailsPanel.Visible = false;
								btn.Text = "show approval details [+]";
							}
							else
							{
								ApprovalDetailsPanel.Visible = true;
								btn.Text = "hide approval details [-]";
							}
						}

						break;
				}
			}
		}

		protected void PageSizeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Bind(1);
		}

		protected void Page_IndexChanged(object sender, CommandEventArgs e)
		{
			int pg = Convert.ToInt32(e.CommandArgument);
			Bind(pg);
		}

		protected void SelectAllAssets_CheckChanged(object sender, EventArgs e)
		{
			CheckBox o = (CheckBox) sender;
			AllAssetsSelected = o.Checked;

			if (!o.Checked)
				SelectedOrderItems.Clear();

			ToggleSelectedAssets();
		}

		protected void DeselectAllLinkButton_Click(object sender, EventArgs e)
		{
			AllAssetsSelected = false;
			SelectedOrderItems.Clear();
			ToggleSelectedAssets();
		}

		protected void DownloadFormatDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			DownloadFormatDropDownList dropdown = (DownloadFormatDropDownList)sender;

			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						DownloadFormatDropDownList DownloadFormatDropDownList1 = (DownloadFormatDropDownList)ri.FindControl("DownloadFormatDropDownList1");

						if (DownloadFormatDropDownList1.UniqueID == dropdown.UniqueID)
						{
							AssetImageSizeDropDownList AssetImageSizeDropDownList1 = (AssetImageSizeDropDownList)ri.FindControl("AssetImageSizeDropDownList1");
							PlaceHolder ImageOptionsPlaceHolder = (PlaceHolder)ri.FindControl("ImageOptionsPlaceHolder");
							HiddenField OrderItemIdHiddenField = (HiddenField)ri.FindControl("OrderItemIdHiddenField");
							HiddenField AssetIdHiddenField = (HiddenField)ri.FindControl("AssetIdHiddenField");
							FileSizeMessageLabel ImageFileSizeMessageLabel = (FileSizeMessageLabel)ri.FindControl("ImageFileSizeMessageLabel");

							int orderItemId = Convert.ToInt32(OrderItemIdHiddenField.Value);
							int assetImageSizeId = (ImageOptionsPlaceHolder.Visible) ? AssetImageSizeDropDownList1.SelectedId : 0;
							int assetId = Convert.ToInt32(AssetIdHiddenField.Value);
							DownloadFormat downloadFormat = DownloadFormatDropDownList1.SelectedDownloadFormat;

							SelectedOrderItems.AddUpdate(orderItemId, downloadFormat, assetImageSizeId);

							Asset asset = Asset.Get(assetId);
							string filename = AssetImageManager.GetResizedAssetImage(asset, assetImageSizeId, downloadFormat, true);
							long filesize = FileUtils.GetFileSize(filename);

							ImageFileSizeMessageLabel.SetFileSize(filesize);
						}

						break;
				}
			}
		}

		protected void AssetImageSizeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			AssetImageSizeDropDownList dropdown = (AssetImageSizeDropDownList) sender;

			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						AssetImageSizeDropDownList AssetImageSizeDropDownList1 = (AssetImageSizeDropDownList) ri.FindControl("AssetImageSizeDropDownList1");

						if (AssetImageSizeDropDownList1.UniqueID == dropdown.UniqueID)
						{
							DownloadFormatDropDownList DownloadFormatDropDownList1 = (DownloadFormatDropDownList)ri.FindControl("DownloadFormatDropDownList1");
							PlaceHolder ImageOptionsPlaceHolder = (PlaceHolder) ri.FindControl("ImageOptionsPlaceHolder");
							HiddenField OrderItemIdHiddenField = (HiddenField) ri.FindControl("OrderItemIdHiddenField");
							HiddenField AssetIdHiddenField = (HiddenField) ri.FindControl("AssetIdHiddenField");
							FileSizeMessageLabel ImageFileSizeMessageLabel = (FileSizeMessageLabel) ri.FindControl("ImageFileSizeMessageLabel");

							int orderItemId = Convert.ToInt32(OrderItemIdHiddenField.Value);
							int assetImageSizeId = (ImageOptionsPlaceHolder.Visible) ? AssetImageSizeDropDownList1.SelectedId : 0;
							int assetId = Convert.ToInt32(AssetIdHiddenField.Value);
							DownloadFormat downloadFormat = DownloadFormatDropDownList1.SelectedDownloadFormat;

							SelectedOrderItems.AddUpdate(orderItemId, downloadFormat, assetImageSizeId);

							Asset asset = Asset.Get(assetId);
							string filename = AssetImageManager.GetResizedAssetImage(asset, assetImageSizeId, downloadFormat, true);
							long filesize = FileUtils.GetFileSize(filename);

							ImageFileSizeMessageLabel.SetFileSize(filesize);
						}

						break;
				}
			}
		}

		protected void CopyDownloadSettingsToAllButton_Click(object sender, EventArgs e)
		{
			bool found = false;
			Button btn = (Button)sender;

			int sourceAssetId = 0;
			DownloadFormat downloadFormat = DownloadFormat.Original;
			int assetImageSizeId = 0;

			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				if (!GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
					continue;

				Button CopyDownloadSettingsToAllButton = (Button)ri.FindControl("CopyDownloadSettingsToAllButton");

				if (CopyDownloadSettingsToAllButton.UniqueID != btn.UniqueID)
					continue;

				HiddenField AssetIdHiddenField = (HiddenField)ri.FindControl("AssetIdHiddenField");
				DownloadFormatDropDownList DownloadFormatDropDownList1 = (DownloadFormatDropDownList)ri.FindControl("DownloadFormatDropDownList1");
				AssetImageSizeDropDownList AssetImageSizeDropDownList1 = (AssetImageSizeDropDownList)ri.FindControl("AssetImageSizeDropDownList1");

				sourceAssetId = Convert.ToInt32(AssetIdHiddenField.Value);
				downloadFormat = DownloadFormatDropDownList1.SelectedDownloadFormat;
				assetImageSizeId = AssetImageSizeDropDownList1.SelectedId;

				found = true;

				break;
			}

			if (!found)
				return;

			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(CurrentOrder.OrderItemList.Select(o => o.AssetId));
			List<Asset> assetList = Asset.FindMany(finder);

			Asset sourceAsset = assetList.Where(a => a.AssetId == sourceAssetId).FirstOrDefault() ?? Asset.Empty;

			Debug.Assert(!sourceAsset.IsNull);

			foreach (OrderItem oi in CurrentOrder.OrderItemList)
			{
				// Get the asset for the order item
				Asset asset = assetList.Where(a => a.AssetId == oi.AssetId).FirstOrDefault() ?? Asset.Empty;
				
				// Non-images do not have download options so ignore them
				if (!AssetTypeChecker.IsImage(asset.FileExtension))
					continue;

				// Update the selection
				SelectedOrderItems.AddUpdate(oi.OrderItemId.GetValueOrDefault(), downloadFormat, assetImageSizeId);
			}

			// Rebind the list
			Bind(CurrentPage);

			FeedbackLabel1.SetSuccessMessage("Download options updated successfully");
		}

		protected void UserOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListControl lc = (ListControl) sender;
			int orderId = Convert.ToInt32(lc.SelectedValue);

			Order order = ContextInfo.UserOrderManager.GetOrderById(orderId);
			DisplayOrder(order);
		}

		protected void ViewAllOrdersLinkButton_Click(object sender, EventArgs e)
		{
			LimitedOrdersPanel.Visible = false;
			AllOrdersPanel.Visible = true;

			UserOrderListBox1.SafeSelectValue(UserOrderDropDownList1.SelectedId);
		}

		protected void ViewLimitedOrdersLinkButton_Click(object sender, EventArgs e)
		{
			LimitedOrdersPanel.Visible = true;
			AllOrdersPanel.Visible = false;

			foreach (ListItem li in UserOrderDropDownList1.Items)
			{
				if (li.Value == UserOrderListBox1.SelectedValue)
				{
					UserOrderDropDownList1.SelectedValue = li.Value;
					return;
				}
			}
		}

		protected void SelectAssetCheckBox_CheckChanged(object sender, EventArgs e)
		{
			AllAssetsSelected = false;

			CheckBox cb = (CheckBox) sender;

			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						Control control = ri.FindControl(cb.ID);

						if (control != null && control.UniqueID == cb.UniqueID)
						{
							HiddenField OrderItemIdHiddenField = (HiddenField) ri.FindControl("OrderItemIdHiddenField");
							DownloadFormatDropDownList DownloadFormatDropDownList1 = (DownloadFormatDropDownList)ri.FindControl("DownloadFormatDropDownList1");
							AssetImageSizeDropDownList AssetImageSizeDropDownList1 = (AssetImageSizeDropDownList) ri.FindControl("AssetImageSizeDropDownList1");

							int orderItemId = Convert.ToInt32(OrderItemIdHiddenField.Value);
							int assetImageSizeId = AssetImageSizeDropDownList1.SelectedId;
							DownloadFormat downloadFormat = DownloadFormatDropDownList1.SelectedDownloadFormat;

							if (cb.Checked)
							{
								SelectedOrderItems.AddUpdate(orderItemId, downloadFormat, assetImageSizeId);
							}
							else
							{
								SelectedOrderItems.Remove(orderItemId);
							}
						}

						break;
				}
			}
		}

		protected void OrderItemsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					// Get the order item and its corresponding asset
					OrderItem orderItem = (OrderItem) e.Item.DataItem;
					Asset asset = orderItem.Asset;

					// Get all controls we'll be using
					HiddenField OrderItemIdHiddenField = (HiddenField) e.Item.FindControl("OrderItemIdHiddenField");
					HiddenField AssetIdHiddenField = (HiddenField) e.Item.FindControl("AssetIdHiddenField");
					AssetThumbnail AssetThumbnail1 = (AssetThumbnail) e.Item.FindControl("AssetThumbnail1");
					AssetButtons AssetButtons1 = (AssetButtons) e.Item.FindControl("AssetButtons1");
					FileSizeMessageLabel FileSizeLabel = (FileSizeMessageLabel) e.Item.FindControl("FileSizeLabel");
					EmailHyperLink AssetContactHyperlink = (EmailHyperLink) e.Item.FindControl("AssetContactHyperlink");
					HyperLink DownloadHyperlink = (HyperLink) e.Item.FindControl("DownloadHyperlink");
					CheckBox SelectAssetCheckBox = (CheckBox) e.Item.FindControl("SelectAssetCheckBox");
					DownloadFormatDropDownList DownloadFormatDropDownList1 = (DownloadFormatDropDownList)e.Item.FindControl("DownloadFormatDropDownList1");
					AssetImageSizeDropDownList AssetImageSizeDropDownList1 = (AssetImageSizeDropDownList) e.Item.FindControl("AssetImageSizeDropDownList1");
					FeedbackLabel OrderItemMessageLabel = (FeedbackLabel) e.Item.FindControl("OrderItemMessageLabel");
					PlaceHolder ImageOptionsPlaceHolder = (PlaceHolder)e.Item.FindControl("ImageOptionsPlaceHolder");
					HtmlTableRow FileSizeRow = (HtmlTableRow) e.Item.FindControl("FileSizeRow");
					HtmlGenericControl SelectorContainer = (HtmlGenericControl) e.Item.FindControl("SelectorContainer");
					HtmlGenericControl LinkButtonWrapper = (HtmlGenericControl) e.Item.FindControl("LinkButtonWrapper");
					FileSizeMessageLabel ImageFileSizeMessageLabel = (FileSizeMessageLabel) e.Item.FindControl("ImageFileSizeMessageLabel");

					// Populate table cells with basic information about the asset
					SetTableCellText(e.Item, "AssetReferenceCell", asset.AssetId.ToString());
					SetTableCellHtml(e.Item, "DescriptionCell", SiteUtils.ConvertTextToHtml(asset.Description));
					SetTableCellText(e.Item, "BrandCell", asset.Brand.Name);
					SetTableCellText(e.Item, "AssetTypeCell", asset.AssetType.Name);
					SetTableCellHtml(e.Item, "UsageRestrictionsCell", asset.UsageRestrictions);
					SetTableCellText(e.Item, "DateRequestedByCell", orderItem.RequiredByDate.HasValue ? orderItem.RequiredByDate.Value.ToString(Global.DateFormat) : "None");
					SetTableCellText(e.Item, "DateOfDecisionCell", orderItem.OrderItemStatusDate.HasValue ? orderItem.OrderItemStatusDate.Value.ToString(Global.DateFormat) : "None");
					SetTableCellText(e.Item, "NotesCell", orderItem.Notes);

					// Populate hidden fields as we'll need these for other stuff
					OrderItemIdHiddenField.Value = orderItem.OrderItemId.ToString();
					AssetIdHiddenField.Value = asset.AssetId.ToString();

					// Get/Initialise other objects we'll need here
					AssetStatus assetStatus = AssetManager.GetAssetStatusForUser(asset, CurrentUser);
					SelectedOrderItem soi = SelectedOrderItems.Get(orderItem.OrderItemId.GetValueOrDefault());

					// Check if user can download this asset
					bool canDownload = CanDownload(orderItem);

					// Initialise the thumbnail and buttons
					AssetThumbnail1.Initialise(asset);
					AssetButtons1.Initialise(asset);

					// Populate other controls which are not dependent on the asset type
					AssetContactHyperlink.SetNameEmail(asset.UploadedByUser.FullName, asset.UploadedByUser.Email);
					SelectorContainer.Visible = canDownload;
					SelectAssetCheckBox.Checked = IsOrderItemSelected(orderItem.OrderItemId.GetValueOrDefault());
					DownloadHyperlink.NavigateUrl = string.Format("javascript:downloadAsset('{0}','{1}','{2}', '{3}', '{4}')", orderItem.AssetId, orderItem.OrderId, orderItem.OrderItemId, DownloadFormatDropDownList1.ClientID, AssetImageSizeDropDownList1.ClientID);
					
					// Do processing dependent on asset type
					if (AssetTypeChecker.IsImage(asset.FileExtension))
					{
						// Only show download resolution row if user can download
						ImageOptionsPlaceHolder.Visible = canDownload;

						// Only do the image size stuff if the download resolution row is visible
						if (ImageOptionsPlaceHolder.Visible)
						{
							// Populate the download format dropdown with the previously selected value (if any)
							DownloadFormatDropDownList1.SafeSelectValue(soi.DownloadFormat);

							// Populate the asset image size dropdown with the previously selected value (if any)
							AssetImageSizeDropDownList1.SafeSelectValue(soi.AssetImageSizeId);

							// Get the filename to the image *if it exists already*
							string filename = AssetImageManager.GetResizedAssetImage(asset, AssetImageSizeDropDownList1.SelectedId, DownloadFormatDropDownList1.SelectedDownloadFormat, false);

							// Only set the filesize if the scaled image already exists, as it will be too heavy to create a scaled image
							// of each asset as the page loads. Maybe we should do this when assets are uploaded...
							if (filename != string.Empty)
								ImageFileSizeMessageLabel.SetFileSize(FileUtils.GetFileSize(filename));
						}
					}
					
					// Only show file size row if download resolution row is hidden and user can download this asset
					// (No point showing them the filesize for an asset they can't download)
					if (ImageOptionsPlaceHolder.Visible)
					{
						FileSizeRow.Visible = false;
					}
					else
					{
						FileSizeRow.Visible = canDownload;
						FileSizeLabel.SetFileSize(asset.FileSize);
					}

					// Only show the conversation row if we have a conversation
					HtmlTableRow ConversationRow = (HtmlTableRow)e.Item.FindControl("ConversationRow");
					ConversationRow.Visible = (orderItem.OrderItemCommentList.Count > 0);

					// Bind conversation
					Repeater ConversationRepeater = (Repeater)e.Item.FindControl("ConversationRepeater");
					ConversationRepeater.DataSource = orderItem.OrderItemCommentList;
					ConversationRepeater.DataBind();

					// Hide the row to add notes, as this is only visible whilst an order item is awaiting approval
					HtmlTableRow AddNotesRow = (HtmlTableRow)e.Item.FindControl("AddNotesRow");
					AddNotesRow.Visible = false;

					if (assetStatus == AssetStatus.Available)
					{
						switch (orderItem.OrderItemStatus)
						{
							case (OrderItemStatus.Preapproved):

								OrderItemMessageLabel.SetSuccessMessage("no approval required", "Approval is not required to download and use this asset.");
								LinkButtonWrapper.Visible = false;

								break;

							case (OrderItemStatus.Approved):

								OrderItemMessageLabel.SetSuccessMessage("approved", "Approval to use this asset has been granted. Refer to the response given in the approval details for any further information.");
								LinkButtonWrapper.Visible = true;

								break;

							case (OrderItemStatus.AwaitingApproval):

								OrderItemMessageLabel.MessageType = BaseMessageLabel.MessageTypes.Pending;
								OrderItemMessageLabel.Header = "approval pending";
								OrderItemMessageLabel.Text = "Approval to use this asset is pending a decision. An email will be sent to you when a decision is made.";
								LinkButtonWrapper.Visible = true;
								AddNotesRow.Visible = true;

								break;

							case (OrderItemStatus.Rejected):

								OrderItemMessageLabel.SetErrorMessage("approval rejected", "Approval to use this asset has been rejected. Refer to the response given in the approval notes for further information.");
								LinkButtonWrapper.Visible = true;

								break;
						}
					}
					else
					{
						OrderItemMessageLabel.MessageType = BaseMessageLabel.MessageTypes.Withdrawn;
						OrderItemMessageLabel.Header = "asset withdrawn";
						OrderItemMessageLabel.Text = "This asset has been withdrawn from the system and is no longer available to download.";
						LinkButtonWrapper.Visible = false;
					}

					if (WebUtils.GetIntRequestParam("OrderItemId", 0) == orderItem.OrderItemId)
					{
						Panel ApprovalDetailsPanel = (Panel)e.Item.FindControl("ApprovalDetailsPanel");
						ApprovalDetailsPanel.Visible = true;

						LinkButton ToggleApprovalDetailsLinkButton = (LinkButton)e.Item.FindControl("ToggleApprovalDetailsLinkButton");
						ToggleApprovalDetailsLinkButton.Text = "hide approval details [-]";
					}

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

		protected void DownloadAsFtpLinkButton_Click(object sender, EventArgs e)
		{
			// Ensure assets are selected
			if (!AreAssetsSelected())
				return;

			// Copy the selected order into the session
			Session["SelectedOrder"] = CurrentOrder;

			// Copy the selected order items into the session
			Session["SelectedOrderItems"] = SelectedOrderItems;

			// Redirect to the FTP details page so user can enter details 
			Response.Redirect("DownloadToFtp.aspx");
		}

		protected void SaveNotesButton_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;

			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						Button SaveNotesButton = (Button)ri.FindControl("SaveNotesButton");

						if (SaveNotesButton.UniqueID == button.UniqueID)
						{
							TextBox CommentsTextBox = (TextBox)ri.FindControl("CommentsTextBox");
							string comments = CommentsTextBox.Text;

							if (!StringUtils.IsBlank(comments))
							{
								// Get the OrderItem ID
								HiddenField OrderItemIdHiddenField = (HiddenField)ri.FindControl("OrderItemIdHiddenField");
								int orderItemId = Convert.ToInt32(OrderItemIdHiddenField.Value);

								// Save the comment
								OrderManager.AddOrderItemComment(CurrentOrder, orderItemId, CurrentUser.UserId.GetValueOrDefault(), comments);

								// Clear the textbox
								CommentsTextBox.Text = string.Empty;

								// Refresh the conversation list
								Repeater ConversationRepeater = (Repeater)ri.FindControl("ConversationRepeater");
								ConversationRepeater.DataSource = OrderItem.Get(orderItemId).OrderItemCommentList;
								ConversationRepeater.DataBind();

								// Show the table row
								HtmlTableRow ConversationRow = (HtmlTableRow) ri.FindControl("ConversationRow");
								ConversationRow.Visible = true;
							}
						}

						break;
				}
			}
		}

		#endregion

		#region Public Static Methods

		public static bool CanDownload(OrderItem orderItem)
		{
			if (AssetManager.GetAssetStatusForUser(orderItem.Asset, CurrentUser) == AssetStatus.Available)
			{
				switch (orderItem.OrderItemStatus)
				{
					case (OrderItemStatus.Preapproved):
					case (OrderItemStatus.Approved):
						return true;

					case OrderItemStatus.AwaitingApproval:
					case OrderItemStatus.Rejected:
						return false;
				}
			}

			return false;
		}

		#endregion

		#region Private Helper Methods

		private bool AreAssetsSelected()
		{
			int count = 0;

			// Count how many order items should be downloaded
			if (CurrentOrder.OrderItemList.Any(ShouldDownload))
				count++;

			// Ensure there's at least one
			if (count == 0)
			{
				FeedbackLabel1.SetErrorMessage("no assets have been selected");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks if the order item with the specified ID is currently selected on the page
		/// </summary>
		private bool IsOrderItemSelectedOnPage(int orderItemId)
		{
			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						HiddenField OrderItemIdHiddenField = (HiddenField) ri.FindControl("OrderItemIdHiddenField");
						int currentOrderItemId = Convert.ToInt32(OrderItemIdHiddenField.Value);

						if (currentOrderItemId == orderItemId)
						{
							CheckBox SelectAssetCheckBox = (CheckBox) ri.FindControl("SelectAssetCheckBox");
							return (SelectAssetCheckBox.Checked);
						}

						break;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if the order item with the specified ID is selected
		/// </summary>
		private bool IsOrderItemSelected(int orderItemId)
		{
			return (AllAssetsSelected || SelectedOrderItems.Contains(orderItemId) || IsOrderItemSelectedOnPage(orderItemId));
		}

		/// <summary>
		/// Checks if the order item can and should be downloaded
		/// (ie. is it available and selected)
		/// </summary>
		private bool ShouldDownload(OrderItem orderItem)
		{
			return (CanDownload(orderItem) && IsOrderItemSelected(orderItem.OrderItemId.GetValueOrDefault()));
		}

		private static void SetTableCellText(Control ri, string id, string text)
		{
			HtmlTableCell cell = (HtmlTableCell) ri.FindControl(id);
			cell.InnerText = text;
		}

		private static void SetTableCellHtml(Control ri, string id, string text)
		{
			HtmlTableCell cell = (HtmlTableCell) ri.FindControl(id);
			cell.InnerHtml = text;
		}

		private void ToggleSelectedAssets()
		{
			foreach (RepeaterItem ri in OrderItemsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						HiddenField OrderItemIdHiddenField = (HiddenField) ri.FindControl("OrderItemIdHiddenField");
						CheckBox SelectAssetCheckBox = (CheckBox) ri.FindControl("SelectAssetCheckBox");

						SelectAssetCheckBox.Checked = AllAssetsSelected;

						int orderItemId = Convert.ToInt32(OrderItemIdHiddenField.Value);

						if (SelectAssetCheckBox.Checked)
						{
							SelectedOrderItems.AddUpdate(orderItemId);
						}
						else
						{
							SelectedOrderItems.Remove(orderItemId);
						}

						break;
				}
			}
		}

		private void DisplayMostRecentOrder()
		{
			OrderFinder finder = new OrderFinder {UserId = CurrentUser.UserId.GetValueOrDefault()};
			finder.SortExpressions.Add(new DescendingSort(Order.Columns.OrderId));
			Order order = Order.FindOne(finder);

			if (order.IsNull)
			{
				// The 'no orders' page is a little complicated as there are controls
				// scattered around, so rather than creating more complexity by showing
				// and hiding controls on this page, we just transfer to a new page.
				Server.Transfer("ViewOrdersEmpty.aspx");
			}

			DisplayOrder(order);
		}

		private void DisplayOrder(Order order)
		{
			if (order.IsNull)
				DisplayMostRecentOrder();

			CurrentOrder = order;

			OrderIdLabel.Text = order.OrderId.ToString();
			OrderDateLabel.Text = order.OrderDate.ToString(Global.DateFormat);

			// Order has changed so no assets selected
			AllAssetsSelected = false;

			// Clear selected assets list
			SelectedOrderItems.Clear();

			Bind(1);
		}

		private void Bind(int page)
		{
			// First get all of the items in the order
			OrderItemFinder finder = new OrderItemFinder {OrderId = CurrentOrderId};
			List<OrderItem> items = OrderItem.FindMany(finder);

			// Filter out those assets that are not accessible by the current user
			var validItems = (from item in items
			                  where EntitySecurityManager.CanViewAssetInfo(CurrentUser, item.Asset)
			                  select item);

			// Get the asset count
			int total = validItems.Count();

			// Set the page size
			SessionInfo.Current.UserSessionInfo.DownloadManagerPageSize = PageSize;

			// Bind the asset list to relevant page
			OrderItemsRepeater.DataSource = items;
			OrderItemsRepeater.DataBind();

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
			OrderDetailsPanel.Visible = (items.Count > 0);
			EmptyOrderPanel.Visible = (items.Count == 0);
		}

		#endregion
	}
}