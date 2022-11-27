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
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class AdjustAsset : BaseAdminPage
	{
		#region Private variables

		private List<String> m_AllowedPreviewExtensions = new List<String>();
		private readonly List<String> m_AllowedThumbnailExtensions = new List<string>();

		private Asset m_Asset;
		private int m_AssetId;
        private bool m_ForcePreviewFormat = false;

		#endregion

		#region Accessors

		private Asset Asset
		{
			get
			{
				if (m_Asset == null)
					m_Asset = Asset.Get(m_AssetId);

				return m_Asset;
			}
		}


		protected override ContextType AssetContext
		{
			get
			{
				return ContextType.AdjustAsset;
			}
		}

		#endregion

		#region Constructor

		protected AdjustAsset()
		{
			m_AllowedPreviewExtensions.Add("swf");
			m_AllowedPreviewExtensions.Add("flv");
			m_AllowedPreviewExtensions.Add("jpg");
			m_AllowedPreviewExtensions.Add("gif");
			m_AllowedPreviewExtensions.Add("png");

			m_AllowedThumbnailExtensions.Add("gif");
			m_AllowedThumbnailExtensions.Add("jpg");
			m_AllowedThumbnailExtensions.Add("png");
		}

		#endregion

		#region Handled Page Events

		protected void Page_Load(object sender, EventArgs e)
		{
			// Ensure we have asset id
			m_AssetId = GetRequiredQueryStringParameter("AssetId", "~/Admin/Assets/AssetList.aspx");

			// Setup return link
			SiteUtils.SetHyperLink("Assets.AssetForm", string.Format("?AssetId={0}", m_AssetId), ReturnToAssetHyperLink1);
			SiteUtils.SetHyperLink("Assets.AssetForm", string.Format("?AssetId={0}", m_AssetId), ReturnToAssetHyperLink2);

			if (!Page.IsPostBack)
			{
				//Setup preview settings option
				AssetFileLabel.Text = Asset.Title + " (" + Asset.FileExtension + ")";

				ProcessingStatusFalseRadioButton.Checked = (!Asset.IsProcessed);
				ProcessingStatusTrueRadioButton.Checked = Asset.IsProcessed;

				//set the default plugin
				PreviewPluginDropDownList.RefreshFromDataSource();

				//check if asset uses non-default plugin
				bool usingCustomPlugin = false;
				if (Asset.Plugin != Guid.Empty)
				{
					Plugin customPlugin = ContextInfo.PluginManager.GetPluginForAsset(Asset);
                    if (customPlugin.RegistrationKey == Asset.Plugin)
					{
                        PreviewPluginDropDownList.SelectItem(customPlugin.PluginId.GetValueOrDefault(),0);
						usingCustomPlugin = true;
					}
				}

				//sets the selected plugin to the default
				if (!usingCustomPlugin)
				{
                    PreviewPluginDropDownList.SelectItem(0,-1);
				}

				// Initialise the asset thumbnail and preview
				AssetThumbnail1.Initialise(Asset);
				AssetPreview1.Asset = Asset;
			}

            ConfigurePreviewSettings();
        }

		#endregion

		#region Handled events related to preview settings

		protected void SavePreviewSettingsButton_Click(object sender, EventArgs e)
		{
			Asset.IsProcessed = ProcessingStatusTrueRadioButton.Checked;

			//check preview plugin not the default plugin
            if (PreviewPluginDropDownList.SelectedId > 0)
			{
				Asset.Plugin = PluginManager.GetRegistrationKey(PreviewPluginDropDownList.SelectedId);
			}
			else
			{
				Asset.Plugin = Guid.Empty;
			}

			Asset.Update(Asset);

			//update the display
			AssetThumbnail1.Initialise(Asset);
			AssetPreview1.Asset = Asset;
            ConfigurePreviewSettings();
			FeedbackLabel2.SetSuccessMessage("Asset preview settings updated successfully");
		}

		protected void PreviewPluginDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConfigurePreviewSettings();
		}

		#endregion

		#region Handled events related to previews

		protected void SavePreviewButton_Click(object sender, EventArgs e)
		{
			BinaryFile file = new BinaryFile(PreviewFileUpload.PostedFile);

			if (file.IsEmpty)
			{
				FeedbackLabel1.SetErrorMessage("No file uploaded");
				return;
			}

            bool feedbackSet = false;
			if (!m_AllowedPreviewExtensions.Contains(file.FileExtension))
			{
                string acceptExtensions = string.Join(",", m_AllowedPreviewExtensions.ToArray());
                if (m_ForcePreviewFormat)
                {
                    //allow upload but display warning                  
                    FeedbackLabel1.SetErrorMessage(String.Format("You have supplied a file with the extension {0}, but only files with {1} extensions are supported by this viewer.  Your file may not display correctly.", file.FileExtension, acceptExtensions));
                    feedbackSet = true;
                }
                else
                {
                    //prevent upload and display warning
                    FeedbackLabel1.SetErrorMessage(String.Format("You have supplied a file with the extension {0}, but only files with {1} extensions are supported by this viewer.  Please choose another.",file.FileExtension,acceptExtensions));
                    return;
                }
			}

			try
			{
				// Save the preview
				AssetFileManager.SaveAssetFile(Asset, file, AssetFileType.AssetPreview);

				// Mark asset as processed
				Asset.IsProcessed = true;
				Asset.Update(Asset);

				// Update preview
				AssetPreview1.Asset = Asset;

				// Update UI
                if (!feedbackSet)
                {
                    FeedbackLabel1.SetSuccessMessage("Preview updated successfully");
                }
                ConfigurePreviewSettings();
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, "Error saving preview");
				FeedbackLabel1.SetErrorMessage("Error saving preview: " + ex.Message);
			}
		}

		protected void RegeneratePreview_Click(object sender, EventArgs e)
		{
			try
			{
				if (APSGateway.Instance.ProcessFile(Asset, false, FileOutputs.Preview))
				{
					// Mark asset as unprocessed
					Asset.IsProcessed = false;
					Asset.Update(Asset);

					// Update preview
					AssetPreview1.Asset = Asset;

					// Update UI
					FeedbackLabel1.SetSuccessMessage("New preview will be generated shortly");
                    PreviewExtensionLabel.Text = String.Empty;
				}
				else
				{
					FeedbackLabel1.SetErrorMessage("An error occurred when submitting asset to processing service. Preview will not be regenerated.");
				}
			}
			catch (InvalidAssetException iaex)
			{
				FeedbackLabel1.SetErrorMessage(iaex.Message);
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, "Error regenerating preview");
				FeedbackLabel1.SetErrorMessage("Error regenerating preview: " + ex.Message);
			}
		}

		protected void RegenerateAssetBitmapsButton_Click(object sender, EventArgs e)
		{
			try
			{
				AssetBitmapGroupManager.Generate(Asset);
				FeedbackLabel1.SetSuccessMessage("New asset bitmaps will be generated shortly");
			}
			catch (Exception ex)
			{
				FeedbackLabel1.SetErrorMessage("Unable to regenerate asset bitmaps: " + ex.Message);
			}
		}

		protected void DeletePreview_Click(object sender, EventArgs e)
		{
			AssetPreviewInfo info = new AssetPreviewInfo(Asset);

			if (info.FileExists)
			{
				try
				{
					File.Delete(info.FilePath);
					AssetPreview1.Asset = Asset;
					FeedbackLabel1.SetSuccessMessage("Preview deleted successfully");
                    ConfigurePreviewSettings();
                    PreviewExtensionLabel.Text = String.Empty;
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex, "Error deleting preview");
					FeedbackLabel1.SetErrorMessage("Error deleting preview: " + ex.Message);
				}
			}
			else
			{
				FeedbackLabel1.SetErrorMessage("Preview file does not exist");
			}
		}

		#endregion

		#region Handled events related to thumbnails

		protected void SaveThumbnailButton_Click(object sender, EventArgs e)
		{
			BinaryFile file = new BinaryFile(ThumbnailFileUpload.PostedFile);

			if (file.IsEmpty)
			{
				FeedbackLabel1.SetErrorMessage("No file uploaded");
				return;
			}

			if (!m_AllowedThumbnailExtensions.Contains(file.FileExtension))
			{
				FeedbackLabel1.SetErrorMessage("Invalid file uploaded");
				return;
			}

			try
			{
				// Save the thumbnail
				AssetFileManager.SaveAssetFile(Asset, file, AssetFileType.AssetThumbnail);

				// Mark asset as processed
				Asset.IsProcessed = true;
				Asset.Update(Asset);

				// Update thumbnail display
				AssetThumbnail1.Initialise(Asset);

				// Update UI
				FeedbackLabel1.SetSuccessMessage("Thumbnail updated successfully");
			}
			catch (Exception ex)
			{
				FeedbackLabel1.SetErrorMessage("Error saving thumbnail: " + ex.Message);
				ExceptionHandler.HandleException(ex, "Error saving thumbnail");
			}
		}

		protected void RegenerateThumbnail_Click(object sender, EventArgs e)
		{
			try
			{
				if (APSGateway.Instance.ProcessFile(Asset, false, FileOutputs.Thumbnail))
				{
					// Mark asset as unprocessed
					Asset.IsProcessed = false;
					Asset.Update(Asset);

					// Update the thumbnail
					AssetThumbnail1.Initialise(Asset);

					// Update the UI
					FeedbackLabel1.SetSuccessMessage("New thumbnail will be generated shortly");
				}
				else
				{
					FeedbackLabel1.SetErrorMessage("An error occurred when submitting asset to processing service. Thumbnail will not be regenerated.");
				}
			}
			catch (InvalidAssetException iaex)
			{
				FeedbackLabel1.SetErrorMessage(iaex.Message);
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, "Error regenerating thumbnail");
				FeedbackLabel1.SetErrorMessage("Error regenerating thuumbnail: " + ex.Message);
			}
		}

		protected void DeleteThumbnail_Click(object sender, EventArgs e)
		{
			AssetThumbnailInfo info = new AssetThumbnailInfo(Asset);

			if (info.FileExists)
			{
				try
				{
					File.Delete(info.FilePath);
					AssetThumbnail1.Initialise(Asset);
					FeedbackLabel1.SetSuccessMessage("Thumbnail deleted successfully");
				}
				catch (Exception ex)
				{
					ExceptionHandler.HandleException(ex, "Error deleting thumbnail");
					FeedbackLabel1.SetErrorMessage("Error deleting thumbnail: " + ex.Message);
				}
			}
			else
			{
				FeedbackLabel1.SetErrorMessage("Thumbnail file does not exist");
			}
		}

		protected void RegeneratePreviewAndThumbnailLinkButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (APSGateway.Instance.ProcessFile(Asset, false, FileOutputs.All))
				{
					Asset.IsProcessed = false;
					Asset.Update(Asset);
                    PreviewExtensionLabel.Text = String.Empty;
					FeedbackLabel1.SetSuccessMessage("New preview and thumbnail wil be regenerated shortly.");
				}
				else
				{
					FeedbackLabel1.SetErrorMessage("An error occurred when submitting asset to processing service. Preview and thumbnail will not be regenerated.");
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, "Error regenerating thumbnail and preview");
				FeedbackLabel1.SetErrorMessage("Error regenerating thumbnail and preview: " + ex.Message);
			}
		}

		#endregion

		#region Private helper methods


        private void ConfigurePreviewSettings()
        {
            Plugin currentPlugin = GetPreviewPlugin();

            if (currentPlugin != null && currentPlugin.HasPluginFile)
            {
                m_AllowedPreviewExtensions.Clear();
                foreach (string extension in currentPlugin.PluginFile.FileExtensions)
                {
                    m_AllowedPreviewExtensions.Add(extension);
                }

                m_ForcePreviewFormat = currentPlugin.PluginFile.ForcePreviewFormat;
            }

            SetPreviewSettingsLabels();
        }


        private Plugin GetPreviewPlugin()
        {
            Plugin currentPlugin;

            if (Asset.Plugin != Guid.Empty)
            {
                //get plugin by id
                currentPlugin = ContextInfo.PluginManager.GetValidPluginByKey(Asset.Plugin);
            }
            else
            {
                //get default plugin
                currentPlugin = ContextInfo.PluginManager.GetDefaultPluginForAsset(Asset);
            }

            return currentPlugin;

        }



		private void SetPreviewSettingsLabels()
		{
			if (IsSelectedPluginNonDefault())
			{
				PluginStatusLabel.SetErrorMessage("User selected preview, this is not guaranteed to be compatible with this asset");
			}
			else
			{
				PluginStatusLabel.SetSuccessMessage("Default Preview Plug-in for this file type");
			}

            //preview file extension label
            AssetPreviewInfo info = new AssetPreviewInfo(Asset);
            string fileExtension = StringUtils.GetFileExtension(info.FilePath);

            PreviewExtensionLabel.Text = String.Format("Preview file type: {0}", fileExtension);

            //labels for accepted extensions
            string acceptExtensions = string.Join(",", m_AllowedPreviewExtensions.ToArray());	
		    AcceptedPreviewFormatLabel.Text  = String.Format("Accepted Preview File Formats: {0}", acceptExtensions);
			AcceptedPreviewFormatThumbnailLabel.Text = String.Format("Accepted Thumbnail File Formats: {0}", "JPEG");
		}

		private bool IsSelectedPluginNonDefault()
		{
			Plugin defaultPlugin = ContextInfo.PluginManager.GetDefaultPluginForAsset(Asset);

			if (PreviewPluginDropDownList.SelectedId > 0 && PreviewPluginDropDownList.SelectedId != defaultPlugin.PluginId)
				return true;

			return false;
		}

		#endregion
	}
}