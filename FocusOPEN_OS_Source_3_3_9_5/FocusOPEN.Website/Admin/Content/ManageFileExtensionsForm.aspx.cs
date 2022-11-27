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
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin.Content
{
	public partial class ManageFileExtensionsForm : BaseAdminPage
	{
		protected ManageFileExtensionsForm()
		{
			PageId = "Content.ManageFileExtensions";
		}

		private int AssetTypeFileExtensionId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "AssetTypeFileExtensionId", 0);
			}
			set
			{
				ViewState["AssetTypeFileExtensionId"] = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int assetTypeId = WebUtils.GetIntRequestParam("AssetTypeId", 0);
				AssetTypeDropDownList.SafeSelectValue(assetTypeId);

				int id = WebUtils.GetIntRequestParam("AssetTypeFileExtensionId", 0);
				string ext = WebUtils.GetRequestParam("Extension", string.Empty);

				AssetTypeFileExtension atfe = AssetTypeFileExtensionCache.Instance.GetById(id);

				if (atfe.IsNull && !StringUtils.IsBlank(ext))
					atfe = AssetTypeFileExtensionCache.Instance.GetByExtension(ext);

				if (!atfe.IsNull)
				{
					AssetTypeFileExtensionId = atfe.AssetTypeFileExtensionId.GetValueOrDefault();

					// Populate UI
					ExtensionTextBox.Text = atfe.Extension;
					FileTypeNameTextBox.Text = atfe.Name;
					IconImage.ImageUrl = SiteUtils.GetFileTypeImageUrl(atfe.Extension);
					AssetTypeDropDownList.SafeSelectValue(atfe.AssetTypeId);
					IsVisibleCheckBox.Checked = atfe.IsVisible;
					IconImagePanel.Visible = true;
					DeleteButton.Visible = true;


                    string generatedExtension = APSGateway.Instance.GeneratesFileExtension(atfe.Extension);
                    PreviewPluginDropDownList.Extension = generatedExtension;
                    PreviewFileFormatLabel.Text = generatedExtension;
                    PreviewPluginDropDownList.ShowSupportedOnly = ShowSupportedPluginsCheckbox.Checked;
                    PreviewPluginDropDownList.RefreshFromDataSource();

                    if (atfe.Plugin != Guid.Empty)
                    {
                        //try and select the file extensions plugin
                        PreviewPluginDropDownList.SafeSelectValue(PluginManager.GetPluginId(atfe.Plugin));

                        if (PreviewPluginDropDownList.SelectedValue == PreviewPluginDropDownList.BlankValue)
                        {
                            //not found in supported list so try finding the plugin in the unsupported list
                            ShowSupportedPluginsCheckbox.Checked = false;
                            PreviewPluginDropDownList.ShowSupportedOnly = false;
                            PreviewPluginDropDownList.RefreshFromDataSource();
                            PreviewPluginDropDownList.SafeSelectValue(PluginManager.GetPluginId(atfe.Plugin));
                        }
                    }
             
					ProcessingSupportPlaceHolder.Visible = true;
					ProcessingSupportLabel.Text = (APSGateway.Instance.CanProcess(atfe.Extension) ? "Yes" : "No");

					// Duplicate, clear extension
					if (WebUtils.GetRequestParam("Action", string.Empty) == "Duplicate")
						ExtensionTextBox.Text = string.Empty;
				}
			}
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			AssetTypeFileExtension atfe = SaveATFE();

			if (!atfe.IsNull)
				AuditLogManager.LogUserAction(SessionInfo.Current.User, AuditUserAction.ModifyFileExtension, string.Format("File Extension: {0}, ID: {1}", atfe.Extension, atfe.AssetTypeFileExtensionId));
		}

		protected void DuplicateButton_Click(object sender, EventArgs e)
		{
			AssetTypeFileExtension atfe = SaveATFE();

			if (!atfe.IsNull)
			{
				string ext = atfe.Extension;
				AssetTypeFileExtensionManager.Duplicate(atfe);
				AuditLogManager.LogUserAction(SessionInfo.Current.User, AuditUserAction.DuplicateFileExtension, string.Format("File Extension: {0} (temp ext: {1}), ID: {2}", ext, atfe.Extension, atfe.AssetTypeFileExtensionId));
				Response.Redirect("ManageFileExtensionsForm.aspx?Action=Duplicate&AssetTypeFileExtensionId=" + atfe.AssetTypeFileExtensionId);
			}
		}

		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			// Get the ID of the extension to be deleted
			int id = WebUtils.GetIntRequestParam("AssetTypeFileExtensionId", 0);

			// Get the file extension to be deleted
			AssetTypeFileExtension atfe = AssetTypeFileExtension.Get(id);

			// Now actually delete it
			AssetTypeFileExtensionManager.Delete(atfe);

			// Update audit log
			AuditLogManager.LogUserAction(SessionInfo.Current.User, AuditUserAction.DeleteFileExtension, string.Format("File Extension: {0}, ID: {1}", atfe.Extension, atfe.AssetTypeFileExtensionId));

			// Update UI
			FormPanel.Visible = false;
			SuccessPanel.Visible = true;
			MessageLabel2.SetSuccessMessage("File extension deleted successfully");
		}

        protected void ShowSupportedPluginsCheckbox_CheckChanged(object sender, EventArgs e)
        {
            PreviewPluginDropDownList.ShowSupportedOnly = ShowSupportedPluginsCheckbox.Checked;
            PreviewPluginDropDownList.RefreshFromDataSource();
        }


		private AssetTypeFileExtension SaveATFE()
		{
			AssetTypeFileExtension atfe = GetATFEFromForm();
			BinaryFile file = new BinaryFile(IconFileUpload.PostedFile);

			try
			{
				AssetTypeFileExtensionManager.Save(atfe, file);
				FormPanel.Visible = false;
				SuccessPanel.Visible = true;

				return atfe;
			}
			catch (AssetTypeFileExtensionException ex)
			{
				MessageLabel1.SetErrorMessage("Error saving file extension", ex.Errors);
			}

			return AssetTypeFileExtension.Empty;
		}

		private AssetTypeFileExtension GetATFEFromForm()
		{
			AssetTypeFileExtension atfe = AssetTypeFileExtension.Get(AssetTypeFileExtensionId);

			if (atfe.IsNull)
				atfe = AssetTypeFileExtension.New();

			atfe.Extension = ExtensionTextBox.Text.Trim().ToLower();
			atfe.Name = FileTypeNameTextBox.Text.Trim();
			atfe.AssetTypeId = AssetTypeDropDownList.SelectedId;
			atfe.IsVisible = IsVisibleCheckBox.Checked;
            atfe.Plugin = PluginManager.GetRegistrationKey(PreviewPluginDropDownList.SelectedId);

			return atfe;
		}
	}
}