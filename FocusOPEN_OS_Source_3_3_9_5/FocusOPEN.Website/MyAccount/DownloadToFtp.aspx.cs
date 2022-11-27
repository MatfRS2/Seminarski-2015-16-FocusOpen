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
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;

namespace FocusOPEN.Website.MyAccount
{
	public partial class DownloadToFtp : BaseSecuredPage
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Private Accessors

		private Order SelectedOrder
		{
			get
			{
				return ViewState["SelectedOrder"] as Order;
			}
			set
			{
				ViewState["SelectedOrder"] = value;
			}
		}

		private SelectedOrderItemList SelectedOrderItems
		{
			get
			{
				return ViewState["SelectedOrderItems"] as SelectedOrderItemList;
			}
			set
			{
				ViewState["SelectedOrderItems"] = value;
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// Get the selected order from the session.  This should've been set in the last page
				Order selectedOrder = Session["SelectedOrder"] as Order;

				// Get the selected order items from the session.  This should've been set in the last page
				SelectedOrderItemList selectedOrderItems = Session["SelectedOrderItems"] as SelectedOrderItemList;

				// Ensure we have an order and order items, or redirect back to the download manager
				if (selectedOrder == null || selectedOrderItems == null)
					Response.Redirect("ViewOrders.aspx?error=MissingOrderOrNoItems&source=DownloadToFtp");

				// Copy the order into this page's viewstate
				SelectedOrder = selectedOrder;

				// Copy the selected order items into this page's viewstate
				SelectedOrderItems = selectedOrderItems;

				// Remove the selected order items from the session
				Session.Remove("SelectedOrderItems");

				// Remove the order from the session
				Session.Remove("SelectedOrder");

				// Populate form from cookie if user has been here before
				FtpHostTextBox.Text = CookieManager.GetValue("FtpHost");
				FtpPortTextBox.Text = CookieManager.GetValue("FtpPort");
				FtpPassiveModeCheckBox.Checked = (CookieManager.GetValue("FtpPassiveMode", "0") == "1");
				FtpUsernameTextBox.Text = CookieManager.GetValue("FtpUsername");
				FtpRemoteFolderTextBox.Text = CookieManager.GetValue("FtpRemoteFolder");

				// Default values
				if (StringUtils.IsBlank(FtpPortTextBox.Text))
					FtpPortTextBox.Text = "21";
			}
		}

		protected void NextButton_Click(object sender, EventArgs e)
		{
			ErrorList errors = new ErrorList();

			if (StringUtils.IsBlank(FtpHostTextBox.Text))
				errors.Add("Ftp host is required");

			if (NumericUtils.ParseInt32(FtpPortTextBox.Text, 0) == 0)
				errors.Add("Ftp host port must be a number");

			if (StringUtils.IsBlank(FtpUsernameTextBox.Text))
				errors.Add("Username is required");

			if (errors.Count > 0)
			{
				FeedbackLabel1.SetErrorMessage("Please check the following and try again:", errors);
				return;
			}

			// List of files to upload
			List<FtpFile> assetFilePaths = new List<FtpFile>();

			// Get all of the order items in the current order
			foreach (OrderItem orderItem in SelectedOrder.OrderItemList)
			{
				// Get the order item id
				int orderItemId = orderItem.OrderItemId.GetValueOrDefault();

				// Check if the order item can be downloaded and is in the list of selected order items
				if (ViewOrders.CanDownload(orderItem) && SelectedOrderItems.Contains(orderItemId))
				{
					// Get the asset
					Asset asset = orderItem.Asset;

					// Get the selected order item
					SelectedOrderItem soi = SelectedOrderItems.Get(orderItemId);

					// Initialise path and filename to FTP
					string path, filename;

					// Check if zip asset files is enabled and we're not doing any kind of transcoding.
					// If so, we want to FTP the zipped asset file instead, so set the path and filename accordingly.
					// However, if the zip file doesn't exist, then we want to FTP the original asset instead.

					if (AssetFileManager.ZipAssetFiles && soi.AssetImageSizeId == 0)
					{
						// First get the path to the zipped asset file
						ZippedAssetFileInfo zippedFileInfo = new ZippedAssetFileInfo(asset);

						if (zippedFileInfo.FileExists)
						{
							// Ensure that a path was returned, and if so, set the filename accordingly
							path = zippedFileInfo.FilePath;
							filename = Path.GetFileNameWithoutExtension(asset.Filename) + ".zip";
						}
						else
						{
							// Otherwise, the zip file doesn't exist, so get the path to the original
							// asset file, and set the filename to the asset filename

							AssetFileInfo info = new AssetFileInfo(asset);

							path = info.FilePath;
							filename = asset.Filename;
						}
					}
					else
					{
						// Get the file path to the asset
						AssetFileInfo info = new AssetFileInfo(asset);
						path = info.FilePath;

						// For images, get the filepath to the resized image
						if (AssetTypeChecker.IsImage(asset.FileExtension))
							path = AssetImageManager.GetResizedAssetImage(asset, soi.AssetImageSizeId, soi.DownloadFormat, true);

						// Construct the asset filename
						filename = Path.GetFileNameWithoutExtension(asset.Filename) + Path.GetExtension(path);
					}

					// Add the file path to the list
					FtpFile file = new FtpFile(path, filename);
					assetFilePaths.Add(file);
				}
			}

			try
			{
				// Instantiate FTP downloader
				FtpDownloader ftpDownloader = new FtpDownloader
				                              	{
				                              		BackgroundTransfer = true,
													Host = FtpHostTextBox.Text.Trim(),
													Port = NumericUtils.ParseInt32(FtpPortTextBox.Text, 0),
													PassiveMode = FtpPassiveModeCheckBox.Checked,
													Username = FtpUsernameTextBox.Text.Trim(),
													Password = FtpPasswordTextBox.Text.Trim(),
													RemoteFolder = FtpRemoteFolderTextBox.Text.Trim(),
													User = CurrentUser
				                              	};

				// Specify files to send to FTP server
				foreach (FtpFile file in assetFilePaths)
					ftpDownloader.Files.Add(file);

				// Wire up events
				ftpDownloader.UploadComplete += new FtpDownloadCompleteEventHandler(NotifyEngine.FtpDownloadComplete);

				// Go do it!
				ftpDownloader.Go();

				// Log the assets as downloaded
				foreach (SelectedOrderItem soi in SelectedOrderItems)
				{
					OrderItem orderItem = OrderItem.Get(soi.OrderItemId);
					Asset asset = orderItem.Asset;

					AuditLogManager.LogAssetAction(asset, CurrentUser, AuditAssetAction.DownloadedAssetFile);
					AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.DownloadAsset, string.Format("Downloaded AssetId: {0} as part of FTP download to: {1} for OrderId: {2}", asset.AssetId, ftpDownloader.Host, orderItem.OrderId));
				}

				// Lets cookie the settings as well, so we can pre-populate
				// the form when the user returns to download more assets
				CookieManager.SetValue("FtpHost", ftpDownloader.Host);
				CookieManager.SetValue("FtpPort", ftpDownloader.Port);
				CookieManager.SetValue("FtpPassiveMode", FtpPassiveModeCheckBox.Checked ? "1" : "0");
				CookieManager.SetValue("FtpUsername", ftpDownloader.Username);

				// Update UI
				FormPanel.Visible = false;
				SuccessPanel.Visible = true;
			}
			catch (FtpDownloadException fdex)
			{
				// Remove the error code from the start of the error message
				string message = Regex.Replace(fdex.Message, @"(\d+\s-\s)", string.Empty);

				// Display the error to the user
				FeedbackLabel1.SetErrorMessage("Error downloading assets", message);

				// Log the error
				m_Logger.Warn(string.Format("Error downloading files to FTP. User: {0}, Host: {1}. Error: {2}", CurrentUser.FullName, FtpHostTextBox.Text, message), fdex);
			}
		}
	}
}