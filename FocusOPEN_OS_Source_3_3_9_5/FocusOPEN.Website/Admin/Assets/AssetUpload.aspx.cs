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
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using Brettle.Web.NeatUpload;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;
using ICSharpCode.SharpZipLib.Zip;
using log4net;

namespace FocusOPEN.Website.Admin
{
	public partial class AssetUpload : BaseAdminPage
	{
		#region Private Variables

		private readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// Initialise the extended options
				SetExtendedOptionsVisibility(SessionInfo.Current.AdminSessionInfo.ShowUploadExtendedOptions);
				AssetTypeDropDownList.SafeSelectValue(Int32.MinValue);
				AssetSourcePathDropDownList.SelectedIndex = 0;
				CategoryDropDownList.SelectedIndex = 0;
				MoveOrCopyButtonList.Visible = false;
				AssetSourceSubfolderTextBox.Visible = false;
				SubFolderCheckBox.Checked = false;

				// Set visibility of AssetFileSourceDiv
				AssetFileSourceDiv.Visible = CurrentUser.EnableFilePathIngestion;
			}

			string js = string.Format("return validateUpload('{0}', '', '{1}', '', 0);", AssetFileUpload.ClientID, AssetSourcePathDropDownList.ClientID);
			UploadButton.Attributes["onClick"] = js;
		}

		protected void AssetSourcePathDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (AssetSourcePathDropDownList.SelectedValue != "#BROWSERUPLOAD#")
			{
				MoveOrCopyButtonList.Visible = true;
				AssetSourceSubfolderTextBox.Visible = true;
			}
			else
			{
				MoveOrCopyButtonList.Visible = false;
				AssetSourceSubfolderTextBox.Visible = false;
			}
		}

		protected void UploadButton_Click(object sender, EventArgs e)
		{
			List<string> files;

			if (!AssetFileUpload.HasFile && AssetSourcePathDropDownList.SelectedValue == "#BROWSERUPLOAD#")
			{
				MessageLabel1.SetErrorMessage("Please select a file to upload");
				return;
			}

			if (AssetSourceSubfolderTextBox.Text.Contains("..\\"))
			{
				MessageLabel1.SetErrorMessage("Invalid source path sub-folder specified");
				return;
			}

			if (!AssetFileUpload.HasFile)
			{
				// Must be scanning Asset source paths
				// Check that we have the specified folder
				string path = Path.Combine(AssetSourcePathDropDownList.SelectedValue, AssetSourceSubfolderTextBox.Text);
				if (!Directory.Exists(path))
				{
					MessageLabel1.SetErrorMessage("Source path specified does not exit");
					return;
				}

				files = AssetSourcePathManager.GetAssetFiles(path);
				if (files.Count() == 0)
				{
					MessageLabel1.SetErrorMessage("No files exist on path '" + AssetSourcePathDropDownList.SelectedValue + "'");
					return;
				}
				// Check the radio button to see whether we are moving or copying.
				BinaryFile.SaveMode saveMode = (MoveOrCopyButtonList.SelectedValue == "M" ? BinaryFile.SaveMode.Move : BinaryFile.SaveMode.Copy);
				ProcessUpload(files, saveMode, false, path);
			}
			else
			{
				// Uploading a single file through the browser
				try
				{
					// Save the uploaded file to disk
					string uploadedFilePath = Path.Combine(SessionHelper.GetForCurrentSession().CreateSessionTempFolder(), AssetFileUpload.FileName);
					AssetFileUpload.MoveTo(uploadedFilePath, MoveToOptions.Overwrite);

					// Wrap the uploaded file in a generic BinaryFile class for use in the business layer.
					BinaryFile file = new BinaryFile(uploadedFilePath, BinaryFile.SaveMode.Move);

					// Ensure basic form values are valid
					if (!PageIsValid(file))
						return;

					files = new List<string> { uploadedFilePath };

					// Alway move the file when processing a browser uploaded file because
					// at this stage we are playing with the temporary file.
					ProcessUpload(files, BinaryFile.SaveMode.Move, true, string.Empty);
				}
				catch (Exception ex)
				{
					const string message = "Unknown error occured when uploading asset(s)";
					string error = (CurrentUser.UserRole == UserRole.SuperAdministrator) ? ex.ToString() : ex.Message;

					MessageLabel1.SetErrorMessage(message, error);
					ExceptionHandler.HandleException(ex, message);
				}
			}
		}

		/// <summary>
		/// Processes a list of files and passes them to the MultiAssetUploader class
		/// Files that come via the browser will not cause sub-categories to be created
		/// unless the file is a zip file in which case the paths in the zip file can be
		/// used to create sub-categories.
		/// </summary>
		/// <param name="filesToUpload">List of filenames to upload</param>
		/// <param name="saveMode">Move or Copy</param>
		/// <param name="IsBrowserUpload">Flag to indicate weather the file came via the browser</param>
		/// <param name="rootPath">The root path for directory scanning</param>
		private void ProcessUpload(IEnumerable<string> filesToUpload, BinaryFile.SaveMode saveMode, bool IsBrowserUpload, string rootPath)
		{
			ErrorList errors = new ErrorList();

			List<UploadedAssetResult> uploadResultList = null;

			rootPath = rootPath.ToLower();

			foreach (string fileName in filesToUpload)
			{
				// At this point we have a list of files containing one or more files to process.
				try
				{
					// Wrap the uploaded file in a generic BinaryFile class for use in the business layer.
					BinaryFile file = new BinaryFile(fileName, saveMode);

					m_Logger.DebugFormat("Uploading file: {0}", file.FileName);

					// Get the directory name where we're uploading from
					string relativePath = new FileInfo(fileName).DirectoryName.ToLower();

					// Strip out the root path if required; we don't need it for processing
					if (!string.IsNullOrEmpty(relativePath) && !string.IsNullOrEmpty(rootPath))
						rootPath = rootPath.Replace(rootPath, string.Empty);

					// Initialize the asset uploader
					MultiAssetUploader assetUploader = new MultiAssetUploader
					{
						BinaryFile = file,
						SourcePath = IsBrowserUpload ? string.Empty : relativePath,
						AssetTypeId = AssetTypeDropDownList.GetSelectedId(),
						TargetCategoryId = CategoryDropDownList.GetSelectedId(),
						CreateCategorySubFolders = SubFolderCheckBox.Checked,
						UploadedBy = CurrentUser,
						SendEmailOnCompletion = NotifyWhenProcessedCheckBox.Checked,
						PreserveZipFile = PreserveZipFileCheckBox.Checked,
						DoNotProcessForPreview = DoNotSubmitForPreviewCheckBox.Checked
					};

					assetUploader.BeforeSave += delegate(object aus, AssetEventArgs aue)
					{
						aue.Asset.Title = Path.GetFileNameWithoutExtension(aue.Asset.Filename);
						aue.Asset.Description = "No description available";
						aue.Asset.UsageRestrictions = string.Concat("(c) ", CurrentUser.CompanyName, " ", DateTime.Now.Year);
					};

					// Do the upload
					assetUploader.Upload();

					m_Logger.DebugFormat("Upload completed successfully");

					// Get Processed Asset ID's
					var processedAssetList = from uar in assetUploader.UploadedAssetResultList
																	 where (uar.FileStatus == FileStatus.Processed || (CurrentUser.UserRole == UserRole.SuperAdministrator && uar.FileStatus == FileStatus.DuplicateHash))
																	 select uar.Asset.AssetId.GetValueOrDefault();

					// Add the assets to the session
					foreach (int assetId in processedAssetList)
						SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Add(assetId);

					// Save the uploaded asset list to the logged in user
					SiteUtils.SaveUploadedAssetListToCurrentUser();

					// Toggle panels
					FormPanel.Visible = false;
					SuccessPanel.Visible = true;

					// Append the asset ID too, so we can jump straight to it
					if (processedAssetList.LongCount() >= 1)
						CatalogueAssetsHyperLink.NavigateUrl += "&AssetId=" + processedAssetList.First();

					if (uploadResultList == null)
					{
						uploadResultList = assetUploader.UploadedAssetResultList;
					}
					else
					{
						uploadResultList.AddRange(assetUploader.UploadedAssetResultList);
					}
				}
				catch (UnauthorizedAccessException uaEx)
				{
					errors.Add(string.Format("Error uploading asset '{0}', {1}", fileName, uaEx.Message));
				}
				catch (DuplicateHashException dhex)
				{
					// This exception will only be thrown for single asset uploads.

					if (EntitySecurityManager.IsAssetRestricted(CurrentUser, dhex.Asset))
					{
						IList<Asset> list = new List<Asset> { dhex.Asset };
						NotifyEngine.AssetUploadDuplicateHash(CurrentUser, list);
					}

					errors.Add(string.Format("Error uploading asset '{0}', {1}", fileName, dhex.Message));
				}
				catch (AssetUploadException auex)
				{
					errors.Add(string.Format("Error uploading asset '{0}', {1}", fileName, auex.Message));
				}
				catch (InvalidAssetException iex)
				{
					if (iex.Errors.Count > 0)
					{
						errors.Add(string.Format("Error uploading asset '{0}'", fileName));
						errors.AddRange(iex.Errors);
					}
					else
					{
						errors.Add(string.Format("Error uploading asset '{0}', {1}", fileName, iex.Message));
					}
				}
				catch (InvalidAssetFileException ifex)
				{
					errors.Add(string.Format("Error saving asset '{0}', {1}", fileName, ifex.Message));
				}
				catch (ZipException zex)
				{
					const string message = "An error occured";

					string error = "It appears that the zip file is corrupt.  Please try re-creating the zip file and uploading again.";

					if (CurrentUser.UserRole == UserRole.SuperAdministrator)
						error += "<br /><br />" + zex;

					errors.Add(string.Format("An error ocurred with file '{0}', {1}", fileName, error));
					ExceptionHandler.HandleException(zex, message);
				}
				catch (Exception ex)
				{
					const string message = "Unknown error occured when uploading asset(s)";
					string error = (CurrentUser.UserRole == UserRole.SuperAdministrator) ? ex.ToString() : ex.Message;

					errors.Add(string.Format("Unknown error occured when uploading asset '{0}', {1}", fileName, error));
					ExceptionHandler.HandleException(ex, message);
				}
			}

			// Display errors if we have any
			if (errors.Count > 0)
				MessageLabel1.SetErrorMessage("Errors occurred while uploading assets", errors);

			if (uploadResultList != null && uploadResultList.Count > 0)
			{
				// Get the groups of files
				var statusList = (from result in uploadResultList
													select result.FileStatus).Distinct();

				// Sort the groups, push processed group to the top as this is the most important
				var sortedStatusList = from status in statusList
															 let SortOrder = (status == FileStatus.Processed) ? 1 : 0
															 orderby SortOrder
															 select status;

				// Populate the groups with files
				var fileGroupList = from status in sortedStatusList
														select new FileGroup(status, from result in uploadResultList
																												 where (result.FileStatus == status)
																												 orderby result.Filename
																												 select result.Filename);

				// Notify admins about any assets uploaded that were duplicates of an 
				// existing asset and the upload user does not have access to that asset.
				var duplicateAssets = from result in uploadResultList
															where (result.FileStatus == FileStatus.DuplicateHash) && (EntitySecurityManager.IsAssetRestricted(CurrentUser, result.Asset))
															orderby result.Asset.AssetId
															select result.Asset;

				// Send the notification
				if (duplicateAssets.LongCount() > 0)
					NotifyEngine.AssetUploadDuplicateHash(CurrentUser, duplicateAssets);

				// Bind the list
				UploadedFileListRepeater.DataSource = fileGroupList;
				UploadedFileListRepeater.DataBind();
			}
		}

		protected void UploadedFileListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
			{
				FileGroup fileGroup = (FileGroup)e.Item.DataItem;
				FeedbackLabel UploadedFileInfoFeedbackLabel = (FeedbackLabel)e.Item.FindControl("UploadedFileInfoFeedbackLabel");

				ErrorList fileList = new ErrorList();

				foreach (string f in fileGroup.Files)
					fileList.Add(f);

				string header = GetFileGroupHeader(fileGroup.FileStatus, fileList.Count);
				bool isError = (fileGroup.FileStatus != FileStatus.Processed);

				UploadedFileInfoFeedbackLabel.SetMessage(header, fileList, isError);
			}
		}

		protected void ToggleExtendedOptionsLinkButton_Click(object sender, EventArgs e)
		{
			SetExtendedOptionsVisibility(!ExtendedOptionsPanel.Visible);
		}

		#endregion

		#region Helper Methods

		private bool PageIsValid(BinaryFile file)
		{
			ErrorList errors = new ErrorList();

			if (file.IsEmpty)
				errors.Add("No file uploaded");

			if (errors.Count > 0)
			{
				MessageLabel1.SetErrorMessage("Error uploading asset(s)", errors);
				return false;
			}

			return true;
		}

		private static string GetFileGroupHeader(FileStatus status, int fileCount)
		{
			switch (status)
			{
				case FileStatus.Processed:
					return string.Format("Successfully uploaded {0} assets", fileCount);

				case FileStatus.DuplicateHash:
					{
						// Super admins can upload duplicates
						if (CurrentUser.UserRole == UserRole.SuperAdministrator)
							return string.Format("Successfully uploaded {0} files, though there were duplicates", fileCount);

						// Otherwise, they are ignored
						return string.Format("Ignored {0} files (files have already been uploaded)", fileCount);
					}

				case FileStatus.IgnoredFile:
					return string.Format("Ignored {0} files (unknown type)", fileCount);

				case FileStatus.IgnoredPath:
					return string.Format("Ignored {0} files (files inside archive are inside invalid folder)", fileCount);

				case FileStatus.InvalidFile:
					return string.Format("Skipped {0} files (incorrect asset type)", fileCount);
			}

			return status.ToString();
		}

		private void SetExtendedOptionsVisibility(bool visible)
		{
			ExtendedOptionsPanel.Visible = visible;
			ToggleExtendedOptionsLinkButton.Text = (visible) ? "[-] hide extended upload options" : "[+] show extended upload options";

			SessionInfo.Current.AdminSessionInfo.ShowUploadExtendedOptions = visible;
		}

		#endregion

		private struct FileGroup
		{
			public FileStatus FileStatus { get; private set; }
			public IEnumerable<string> Files { get; private set; }

			public FileGroup(FileStatus status, IEnumerable<string> files)
				: this()
			{
				FileStatus = status;
				Files = files;
			}
		}
	}
}