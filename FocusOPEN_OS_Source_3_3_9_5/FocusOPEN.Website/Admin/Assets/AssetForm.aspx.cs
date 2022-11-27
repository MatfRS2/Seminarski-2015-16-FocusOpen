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
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Brettle.Web.NeatUpload;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;
using log4net;

namespace FocusOPEN.Website.Admin
{
    public partial class AssetForm : BaseAdminPage
    {
        #region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        //only set this one when you need the client side sorting logic to be executed - for example upon postbacks
        //that render and return the asset form
        protected bool metadataSortingOn;

        #endregion
        
		#region Accessors

		/// <summary>
		/// Gets the asset currently being edited
		/// </summary>
		private Asset Asset
		{
			get
			{
				Asset asset = ViewState["Asset"] as Asset ??
							  Context.Items["Asset"] as Asset ??
							  Asset.Get(WebUtils.GetIntRequestParam("AssetId", 0));

				return asset;
			}
		}

		/// <summary>
		/// Temporary list linked assets, stored in viewstate until asset is saved back to the database.
		/// </summary>
		private List<Asset> LinkedAssetList
		{
			get
			{
				List<Asset> list = ViewState["LinkedAssetList"] as List<Asset>;

				if (list == null)
				{
					list = new List<Asset>();
					ViewState["LinkedAssetList"] = list;
				}

				return list;
			}
		}

		/// <summary>
		/// Gets or sets the primary category id.
		/// </summary>
    	private int PrimaryCategoryId
    	{
    		get
    		{
    			return NumericUtils.ParseInt32(PrimaryCategoryIdHiddenField.Value, 0);
    		}
			set
			{
				PrimaryCategoryIdHiddenField.Value = value.ToString();
			}
    	}

		private bool AssetIsBeingCataloged
		{
			get
			{
				return SessionInfo.Current.AdminSessionInfo.AssetPager.AssetIdList.Contains(Asset.AssetId.GetValueOrDefault());
			}
		}

        protected override ContextType AssetContext
        {
            get
            {
                return ContextType.CatalogueAsset;
            }
        }


		#endregion

        #region Handled Events

		protected void Page_Init(object sender, EventArgs e)
		{
			CopyrightOwnerRadioButtonList.Items[0].Text = WebsiteBrandManager.GetBrand().OrganisationName;
			
			// Only show the brand selector if there is more than one brand available
			BrandSelectorWrapper.Visible = (BrandManager.IsMultipleBrandMode);
		}

        protected void Page_Load(object sender, EventArgs e)
        {
			// Only show exteranal publish wrapper if enabled and current user is superadmin
			ExternalPublishWrapper.Visible = (CurrentUser.UserRole.Equals(UserRole.SuperAdministrator) && ExternalPublisher.Instance.IsEnabled());

        	// Only show file hash to superadmins
        	FileHashRow.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator);

            // Update UI when copyright owner is changed
			AttachCopyrightOwnerJavascript();

            if (!Page.IsPostBack)
            {
                //only apply meta li containers sorting logic upon initial rendering of the page
                //or upon validation errors
                metadataSortingOn = true;

				// Get the asset being edited
            	Asset asset = Asset;

				// Ensure that the asset is valid.
				// If not, redirect back to the asset list.
                if (asset.IsNull)
                {
                    Response.Redirect("AssetList.aspx", false);
                    return;
                }

				// Get the asset ID
				int assetId = asset.AssetId.GetValueOrDefault();

            	// Get the publish status for this asset (published, not published, pending, etc)
                AssetPublishStatus assetPublishStatus = GetAssetPublishStatus(asset);

				// Add client side checker to replace asset button to ensure that only file with correct
				// extenson can be uploaded, and that upload cannot start with incorrect files
				bool isPending = (assetPublishStatus == AssetPublishStatus.PendingApproval);
				SiteUtils.AddValidateUploadClientSideCheck(ReplaceFileButton, NewAssetFileUpload, asset.AssetType, isPending);

                // Initialise the asset preview
				AssetPreview1.Asset = asset;

				// Set the download link to the direct download URL
                DownloadAssetFileHyperLink.NavigateUrl = AssetFileUrlHelper.GetDownloadUrl(asset.AssetId) + "?original=1";

                // Update the links to the adjust asset page
            	SiteUtils.SetHyperLink("Assets.AdjustAsset", string.Format("?AssetId={0}", assetId), AdjustAssetHyperLink1);
				SiteUtils.SetHyperLink("Assets.AdjustAsset", string.Format("?AssetId={0}", assetId), AdjustAssetHyperLink2);

				// Set the audit log link
				SiteUtils.SetHyperLink("Reports.AssetAuditTrail", string.Format("?AssetId={0}", assetId), ViewAssetAuditLogHyperLink);

                // Show the adjust asset warning if the asset type cannot be automatically processed
            	AdjustAssetWarningContainer.Visible = (!APSGateway.Instance.CanProcess(asset.FileExtension));

				// Populate the UI with standard asset values
                AssetIdLabel.Text = asset.AssetId.ToString();
                AssetTypeDropDownList1.SafeSelectValue(asset.AssetTypeId);
                UploadDateLabel.Text = asset.UploadDate.ToString(Global.DateFormat);
            	UploadDateLabel.ToolTip = asset.UploadDate.ToString(Global.DateTimeFormat);
                TemplateIdLabel.Text = (asset.TemplateAssetId.HasValue) ? asset.TemplateAssetId.ToString() : "This asset is the template";
                WorkflowDropDownList.SafeSelectValue(asset.WorkflowId);
                TitleTextBox.Text = asset.Title;
                ProjectCodeTextBox.Text = asset.ProjectCode;
				OriginatorTextBox.Text = asset.Originator;
				ShortDescriptionTextBox.Text = asset.Description;
				KeywordsTextBox.Text = asset.Keywords;
				ContactEmailTextBox.Text = asset.ContactEmail;
                ProductionDayDropDownList.SafeSelectValue(asset.ProductionDay.GetValueOrDefault().ToString());
                ProductionMonthDropDownList.SafeSelectValue(asset.ProductionMonth.GetValueOrDefault().ToString());
                ProductionYearDropDownList.SafeSelectValue(asset.ProductionYear.GetValueOrDefault().ToString());
				UsageRestrictionsTextBox.Text = asset.UsageRestrictions;
				WatermarkPreviewCheckBox.Checked = asset.WatermarkPreview;
				WatermarkPreviewCheckBox.Enabled = APSGateway.Instance.CanProcess(asset.FileExtension);
				PublicationDatePicker.SelectedDate = asset.PublishDate;

				InternalUsersRestrictionsRadioButtonList.SelectedValue = (asset.InternalUsers_HideFromUsers) ? "NotVisible" : "None";
				if (asset.InternalUsers_DownloadApprovalRequired)
					InternalUsersRestrictionsRadioButtonList.SelectedValue = "ApprovalRequired";

				ExternalUsersRestrictionsRadioButtonList.SelectedValue = (asset.ExternalUsers_HideFromUsers) ? "NotVisible" : "None";
				if (asset.ExternalUsers_DownloadApprovalRequired)
					ExternalUsersRestrictionsRadioButtonList.SelectedValue = "ApprovalRequired";

                //set the ordering of metadata
                LiAssetType.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.ASSET_TYPE).CatalogueOrderNum.ToString();
                LiDateProduced.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.DATE_PRODUCED).CatalogueOrderNum.ToString();
                LiOriginator.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.ORIGINATOR).CatalogueOrderNum.ToString();
                LiTitle.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.TITLE).CatalogueOrderNum.ToString();
                LiProjectCode.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.PROJECT_CODE).CatalogueOrderNum.ToString();
                LiBrand.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.BRAND).CatalogueOrderNum.ToString();
                LiCats.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.CATEGORIES).CatalogueOrderNum.ToString();
                LiShortDescription.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.DESCRIPTION).CatalogueOrderNum.ToString();
                LiAdditionalKeywords.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.ADDITIONAL_KEYWORDS).CatalogueOrderNum.ToString();
                LiCopyrightOwner.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.COPYRIGHT_OWNER).CatalogueOrderNum.ToString();
                LiUsageRestrictions.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.USAGE_RESTRICTIONS).CatalogueOrderNum.ToString();
                LiAttachedFiles.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.ATTACHED_FILES).CatalogueOrderNum.ToString();
                LiLinkedAssets.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.LINKED_ASSETS).CatalogueOrderNum.ToString();
                LiDownloadRestrictions.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.DOWNLOAD_RESTRICTIONS).CatalogueOrderNum.ToString();
                LiContactEmail.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.CONTACT_EMAIL).CatalogueOrderNum.ToString();
                LiPublicationDate.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.PUBLICATION_DATE).CatalogueOrderNum.ToString();
                LiExpiryDate.Attributes["class"] = asset.Brand.GetMetadataSetting(BrandMetadataSettings.EXPIRY_DATE).CatalogueOrderNum.ToString();
                
                if (asset.ExpiryDate == Settings.MAX_DATETIME)
				{
					ExpiryDatePicker.Enabled = false;
					ExpiryDatePicker.SelectedDate = null;
					AssetExpiresRadioButtonList.SelectedValue = "false";
				}
				else
				{
					ExpiryDatePicker.Enabled = true;
					ExpiryDatePicker.SelectedDate = asset.ExpiryDate;
					AssetExpiresRadioButtonList.SelectedValue = "true";
				}

            	// Populate asset file metadata
				BindAssetFileMetadataRepeater(asset);

            	// Get the asset type info, which specifies which options
				// are available to this type of asset
                AssetTypeInfo assetTypeInfo = AssetTypeInfo.Get(asset.FileExtension);

				// Toggle UI elements absed on the asset type
                OrientationRow.Visible = assetTypeInfo.HasOrientation;
                DurationRow.Visible = assetTypeInfo.HasDuration;
                DimensionsRow.Visible = assetTypeInfo.HasDimensions;

				// Set the copyright owner checkbox based on who the copyright owner is
				// We compare this against the company name to figure this out.
				if (StringUtils.IgnoreCaseCompare(asset.CopyrightOwner, WebsiteBrandManager.GetBrand().OrganisationName))
				{
					CopyrightOwnerRadioButtonList.SelectedIndex = 0;
					CopyrightOwnerTextBox.Attributes["disabled"] = "disabled";
				}
				else
				{
					CopyrightOwnerRadioButtonList.SelectedIndex = 1;
					CopyrightOwnerTextBox.Text = asset.CopyrightOwner;
				}

				//-----------------------------------------------------------------------------------------------------
				// Other UI setup stuff
				//-----------------------------------------------------------------------------------------------------
				DisplayAssetFileInformation(asset);
				
				//-----------------------------------------------------------------------------------------------------
				// Attached Files Stuff
				//-----------------------------------------------------------------------------------------------------
				BindAttachedFilesRepeater();
            	RestrictAttachedFilesCheckBox.Checked = asset.RestrictAttachedFiles;

				//-----------------------------------------------------------------------------------------------------
				// Linked Assets Stuff
				//-----------------------------------------------------------------------------------------------------
				BindLinkedAssetsRepeater();

				//-----------------------------------------------------------------------------------------------------
				// Populate Asset Metadata
				//-----------------------------------------------------------------------------------------------------
            	BrandDropDownList1.SafeSelectValue(asset.BrandId);
				BindAssetCustomMetadata();
				SetAndDisplayPrimaryCategory(asset.PrimaryCategoryId);
				//-----------------------------------------------------------------------------------------------------

                switch (assetPublishStatus)
                {
                    case (AssetPublishStatus.Approved):
                    case (AssetPublishStatus.NotPublished):
                    case (AssetPublishStatus.Published):

                        // Asset can be published

                        WorkflowSelectorRow.Visible = false;
                        IsPublishedCheckBox.Checked = (assetPublishStatus == AssetPublishStatus.Published);
                        PublishWrapper.Visible = true;
                        SubmitToWorkflowWrapper.Visible = false;

                        break;

                    case (AssetPublishStatus.NotApproved):

						// Check if workflows are available
                		bool workflowsAreAvailable = ((from li in WorkflowDropDownList.Items.Cast<ListItem>()
                		                               where (NumericUtils.ParseInt32(li.Value, 0) > 0)
                		                               select li).Count() > 0);

						// Display the workflow selector and submit to workflow controls
						// when the user has to use workflow and there are workflows available.
						if (CurrentUser.UseWorkflow && workflowsAreAvailable)
						{
							// Workflow selector is displayed when a workflow has not been assigned
							// to the user.  This is so they can select which workflow to submit to.
							WorkflowSelectorRow.Visible = (!CurrentUser.WorkflowId.HasValue);

							// The publish wrapper is hidden and the workflow submit wrapper is displayed
							// When the user cannot choose the workflow, it is submitted to the one chosen for them.
							PublishWrapper.Visible = false;
							SubmitToWorkflowWrapper.Visible = true;
						}
						else
						{
							// When the user has not been set to use workflow or there are no
							// workflows available, we allow direct publishing of the asset.

							PublishWrapper.Visible = true;
							SubmitToWorkflowWrapper.Visible = false;
						}

                        break;

                    case (AssetPublishStatus.PendingApproval):

                        // Asset has been submitted to the workflow
                        // Show the workflow selector, but disable the dropdown
                        // Don't show the publish box.  Show the workflow row and checkbox,
                        // check it (as we're in a workflow process) and disable it

                        WorkflowSelectorRow.Visible = true;
                        WorkflowDropDownList.Enabled = false;
                        PublishWrapper.Visible = false;
                        SubmitToWorkflowWrapper.Visible = true;
                        SubmitToWorkflowCheckbox.Checked = true;
                        SubmitToWorkflowCheckbox.Enabled = false;

                        break;
                }

				if (ExternalPublishWrapper.Visible)
				{
					if (asset.IsProcessed)
					{
						if (ExternalPublisher.Instance.IsPublished(assetId))
						{
							IsAssetPublishedToExternalFolderCheckBox.Checked = true;
							IsAssetPublishedInfoLabel.Text = "Reference to use asset: " + ExternalPublisher.Instance.GetHash(assetId);
						}
						else
						{
							IsAssetPublishedToExternalFolderCheckBox.Checked = false;
							IsAssetPublishedInfoLabel.Text = "Asset is not available on external access area";
						}
					}
					else
					{
						IsAssetPublishedToExternalFolderCheckBox.Enabled = false;
						IsAssetPublishedInfoLabel.Text = "Asset cannot be published until processing is complete";
					}
				}

            	// Only show the workflow info wrapper if the asset has used a workflow and at least one asset workflow
                WorkflowInfoWrapper.Visible = (asset.WorkflowId.HasValue && asset.AssetWorkflowList.Count > 0);

                // Only bind the repeater if the workflow info wrapper is visible
                if (WorkflowInfoWrapper.Visible)
                    AssetWorkflowLog.Bind(asset);

                if (WebUtils.GetRequestParam("source", string.Empty) == "assetlist")
                {
                    SaveAndPreviousButton.Visible = false;
                    SaveAndNextAssetButton.Visible = false;
                }
                else
                {
					if (AssetIsBeingCataloged)
                    {
						SessionInfo.Current.AdminSessionInfo.AssetPager.CurrentAssetId = assetId;

                        string previousAssetId = SessionInfo.Current.AdminSessionInfo.AssetPager.PreviousAssetId().ToString();
                        string nextAssetId = SessionInfo.Current.AdminSessionInfo.AssetPager.NextAssetId().ToString();

                        SaveAndPreviousButton.Text = SaveAndPreviousButton.Text.Replace("AssetId", previousAssetId);
                        SaveAndNextAssetButton.Text = SaveAndNextAssetButton.Text.Replace("AssetId", nextAssetId);

                        SaveAndPreviousButton.CommandArgument = previousAssetId;
                        SaveAndNextAssetButton.CommandArgument = nextAssetId;

                        SaveAndPreviousButton.ToolTip = string.Format("Go to {0}", SaveAndPreviousButton.CommandArgument);
                        SaveAndNextAssetButton.ToolTip = string.Format("Go to {0}", SaveAndNextAssetButton.CommandArgument);

						EditAssetHeaderPlaceHolder.Visible = false;
                        CatalogueAssetHeaderPlaceHolder.Visible = true;

						SetPageTitle("Catalogue Assets: Step Two");
						HeaderTextDiv.InnerText = "Catalogue Assets: Step Two";

                        Label BreadCrumbsLabel = (Label)SiteUtils.FindControlRecursive(Page.Master, "BreadCrumbsLabel");
                        BreadCrumbsLabel.Text = string.Format("Manage Assets{0}Catalogue Assets: Step Two", AdminNavigation.BreadcrumbsSpacer);

						if (SessionInfo.Current.AdminSessionInfo.AssetPager.UseFirstAssetAsTemplate && assetId == SessionInfo.Current.AdminSessionInfo.AssetPager.FirstAssetId())
                        {
                            const string templatingWarningMessage = "alert('Note:\\nAsset details will be saved and replicated to other assets selected for cataloguing.\\n\\nHowever, this is a once only action, and returning to this asset and making further changes will NOT be propogated to other assets.')";
                            SaveAndExitButton.Attributes["onClick"] = templatingWarningMessage;
                            SaveAndNextAssetButton.Attributes["onClick"] = templatingWarningMessage;
                        }
                    }
                    else
                    {
                        SessionInfo.Current.AdminSessionInfo.AssetPager = new AssetPager();
                    }

                    SaveAndPreviousButton.Visible = !SessionInfo.Current.AdminSessionInfo.AssetPager.IsFirst() && (SessionInfo.Current.AdminSessionInfo.AssetPager.PreviousAssetId() != 0);
                    SaveAndNextAssetButton.Visible = !SessionInfo.Current.AdminSessionInfo.AssetPager.IsLast() && (SessionInfo.Current.AdminSessionInfo.AssetPager.NextAssetId() != 0);
                }
            }
        }

    	private void BindAssetFileMetadataRepeater(Asset asset)
    	{
    		var metadataList = asset.GetFileMetadata(false);

			if (metadataList.Count == 0)
			{
				NoAssetFileMetadataWrapper.Visible = true;
				AssetFileMetadataPanel.Visible = false;
			}
			else
			{
				NoAssetFileMetadataWrapper.Visible = false;
				AssetFileMetadataPanel.Visible = true;
				AssetFileMetadataRepeater.DataSource = metadataList;
				AssetFileMetadataRepeater.DataBind();
			}
    	}

    	protected void ReplaceAssetLinkButton_Click(object sender, EventArgs e)
        {
            ReplaceFilePanel.Visible = true;
        }

        protected void CancelFileUploadButton_Click(object sender, EventArgs e)
        {
            ReplaceFilePanel.Visible = false;
        }

        protected void ReplaceFileButton_Click(object sender, EventArgs e)
        {
			if (!NewAssetFileUpload.HasFile)
			{
				ReplaceFileMessageLabel.SetErrorMessage("File is required");
				return;
			}

			// Save the uploaded file to disk
			string uploadedFilePath = Path.Combine(SessionHelper.GetForCurrentSession().CreateSessionTempFolder(), NewAssetFileUpload.FileName);
			NewAssetFileUpload.MoveTo(uploadedFilePath, MoveToOptions.Overwrite);

			// Wrap the uploaded file in a generic BinaryFile class for use in the business layer.
			BinaryFile file = new BinaryFile(uploadedFilePath, BinaryFile.SaveMode.Move);

            try
            {
                AssetManager.ReplaceAssetFile(Asset, file, NotifyWhenProcessedCheckBox.Checked, CurrentUser);

                AssetPreview1.Asset = Asset;
                ReplaceFilePanel.Visible = false;
                DisplayAssetFileInformation(Asset);
            }
            catch (InvalidAssetFileException iafex)
            {
                ReplaceFileMessageLabel.SetErrorMessage(iafex.Message);
            }
        }

		protected void BrandDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindAssetCustomMetadata();
		}

        protected void SaveButton_Command(object sender, CommandEventArgs e)
        {
			// Get the asset to be saved
        	Asset asset = Asset;

			// Get the asset ID
			int assetId = asset.AssetId.GetValueOrDefault();

			// Save the metadata
            asset.AssetTypeId = AssetTypeDropDownList1.SelectedId;

			// Add selected metadata 
        	asset.MetadataList.Clear();
            asset.MetadataTextAreasList.Clear();
            asset.MetadataTextFieldsList.Clear();

            AddCustomMetadata(asset);

//            for (int i = 1; i <= Settings.NumberOfMetadataFields; i++)
//			{
//				MetadataInputWrapper input = SiteUtils.FindControlRecursive(Page, "MetadataInput" + i) as MetadataInputWrapper;
//
//				if (input == null)
//					continue;
//			}

			// Clear the linked assets, they will be re-added
			asset.LinkedAssetList.Clear();
			asset.LinkedAssetList.AddRange(LinkedAssetList.FindAll(la => la.AssetId.GetValueOrDefault() > 0));

			// Save the primary category
			asset.PrimaryCategoryId = NumericUtils.ParseInt32(PrimaryCategoryIdHiddenField.Value, 0);

			// Clear the categories, they will be re-added
			asset.CategoryList.Clear();

			// Add each selected category to the list
			foreach (TreeNode node in CategoriesTreeView.GetSelectedNodes())
			{
				int id = NumericUtils.ParseInt32(node.Value, 0);

				if (!asset.CategoryList.Any(c => c.CategoryId == id))
				{
					Category category = CategoryCache.Instance.GetById(id);
					asset.CategoryList.Add(category);
				}
			}

			// Set the primary category ID
        	asset.PrimaryCategoryId = NumericUtils.ParseInt32(PrimaryCategoryIdHiddenField.Value, 0);

			// Set the primary brand
			asset.BrandId = BrandDropDownList1.SelectedId;
			
        	// Set relevant asset options, if enabled
        	asset.Title = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.TITLE).OnAssetForm) ? TitleTextBox.Text.Trim() : string.Empty;
            asset.ProjectCode = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.PROJECT_CODE).OnAssetForm) ? ProjectCodeTextBox.Text.Trim() : string.Empty;
            asset.Originator = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.ORIGINATOR).OnAssetForm) ? OriginatorTextBox.Text.Trim() : string.Empty;
            asset.Description = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.DESCRIPTION).OnAssetForm) ? ShortDescriptionTextBox.Text.Trim() : string.Empty;
            asset.Keywords = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.ADDITIONAL_KEYWORDS).OnAssetForm) ? KeywordsTextBox.Text.Trim() : string.Empty;
            asset.ContactEmail = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.CONTACT_EMAIL).OnAssetForm) ? ContactEmailTextBox.Text.Trim() : string.Empty;


			// Set the production month and year if the production date setting is required
			// The month can be null, but the year must be specified.  If this option is not enabled
			// we just set both of these to null.
			if (asset.Brand.GetMetadataSetting(BrandMetadataSettings.DATE_PRODUCED).OnAssetForm)
			{
				// Reset production date
                asset.ProductionDay = null;
                asset.ProductionMonth = null;
                asset.ProductionYear = null;

				// Only set if values were selected
                if (ProductionYearDropDownList.SelectedValue != "0")
                {
                    asset.ProductionYear = Convert.ToInt32(ProductionYearDropDownList.SelectedValue);

                    if (ProductionMonthDropDownList.SelectedValue != "0")
                    {
                        asset.ProductionMonth = Convert.ToInt32(ProductionMonthDropDownList.SelectedValue);

                        if (ProductionDayDropDownList.SelectedValue != "0")
                            asset.ProductionDay = Convert.ToInt32(ProductionDayDropDownList.SelectedValue);
                    }
                }
            }
			else
			{
                asset.ProductionDay = null;
                asset.ProductionMonth = null;
                asset.ProductionYear = null;
            }
        	
			// Set Copyright Owner, if enabled
			asset.CopyrightOwner = (CopyrightOwnerRadioButtonList.SelectedIndex == 0) ?  WebsiteBrandManager.GetBrand().OrganisationName : CopyrightOwnerTextBox.Text.Trim();

			// Set usage restrictions, if enabled
            asset.UsageRestrictions = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.USAGE_RESTRICTIONS).OnAssetForm) ? UsageRestrictionsTextBox.Text.Trim() : string.Empty;

			// Only set asset file restriction if attached files are enabled
			asset.RestrictAttachedFiles = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.ATTACHED_FILES).OnAssetForm) ? RestrictAttachedFilesCheckBox.Checked : false;

			// Watermark preview?
			asset.WatermarkPreview = WatermarkPreviewCheckBox.Checked;

			// Set download restrictions if enabled.  Otherwise, just set all of these to false
			// as the UI elements aren't visible, and we don't want to inadvertently restrict
			// the asset without the user knowing about it (or being able to change it).
			if (asset.Brand.GetMetadataSetting(BrandMetadataSettings.DOWNLOAD_RESTRICTIONS).OnAssetForm)
			{
				asset.InternalUsers_HideFromUsers = (InternalUsersRestrictionsRadioButtonList.SelectedValue == "NotVisible");
				asset.InternalUsers_DownloadApprovalRequired = (InternalUsersRestrictionsRadioButtonList.SelectedValue == "ApprovalRequired");

				asset.ExternalUsers_HideFromUsers = (ExternalUsersRestrictionsRadioButtonList.SelectedValue == "NotVisible");
				asset.ExternalUsers_DownloadApprovalRequired = (ExternalUsersRestrictionsRadioButtonList.SelectedValue == "ApprovalRequired");
			}
			else
			{
				asset.InternalUsers_HideFromUsers = false;
				asset.InternalUsers_DownloadApprovalRequired = false;

				asset.ExternalUsers_HideFromUsers = false;
				asset.ExternalUsers_HideFromUsers = false;
			}

			// Set publish date if required.  Otherwise, default it to SQL's minimum date
        	asset.PublishDate = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.PUBLICATION_DATE).OnAssetForm) ? PublicationDatePicker.SelectedDate.GetValueOrDefault() : Settings.MIN_DATETIME;

			// Set the expiry date if required.  Otherwise, default it to SQL's maximum date
            asset.ExpiryDate = (asset.Brand.GetMetadataSetting(BrandMetadataSettings.USAGE_RESTRICTIONS).OnAssetForm && AssetExpiresRadioButtonList.SelectedValue.ToLower() == "true") ? ExpiryDatePicker.SelectedDate.GetValueOrDefault() : Settings.MAX_DATETIME;

			// Setup workflow stuff if required
			bool submitToWorkflow = SetWorkflowAndPublishStatus(asset);
        	
            try
            {
				// Initialise with empty file
            	BinaryFile file = BinaryFile.Empty;

				// Use the uploaded file, if it exists (ie. if a file was uploaded)
				if (NewAssetFileUpload.HasFile)
				{
					// Save the uploaded file to disk
					string uploadedFilePath = Path.Combine(SessionHelper.GetForCurrentSession().CreateSessionTempFolder(), NewAssetFileUpload.FileName);
					NewAssetFileUpload.MoveTo(uploadedFilePath, MoveToOptions.Overwrite);

					// Wrap the uploaded file in a generic BinaryFile class for use in the business layer.
					file = new BinaryFile(uploadedFilePath, BinaryFile.SaveMode.Move);
				}

            	// NB. Do not inline
                bool watermarkPreviewSettingChanged = asset.IsPropertyChanged(Asset.Columns.WatermarkPreview);

				// Change the processed setting so that the updated watermark is used
				if (watermarkPreviewSettingChanged)
					asset.IsProcessed = false;

                // Save the asset.  If it contains validation errors, an InvalidAssetException is thrown
                // which will be caught by the exception handler below and displayed to the user accordingly
                AssetManager.Save(asset, file);

				// Save asset to external folder if required
            	if (IsAssetPublishedToExternalFolderCheckBox.Checked)
				{
					ExternalPublisher.Instance.Publish(assetId);
				}
				else
				{
					ExternalPublisher.Instance.Unpublish(assetId);
				}

				// Send the asset to the workflow if it needs approval
                if (submitToWorkflow)
                    WorkflowManager.SubmitAssetToWorkflow(asset, CurrentUser);

				// Save additional metadata files
				if (asset.Brand.GetMetadataSetting(BrandMetadataSettings.ATTACHED_FILES).OnAssetForm)
            		SaveAttachedFiles(asset);

                // Update the watermark preview if the setting has changed
				if (watermarkPreviewSettingChanged && APSGateway.Instance.CanProcess(Asset.FileExtension))
					APSGateway.Instance.ProcessFile(Asset, NotifyWhenProcessedCheckBox.Checked, FileOutputs.Preview);

            	// Check if this asset is in the list of upload assets
                bool isUploadAsset = SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Contains(assetId);

                // Check if this is the first time we are cataloguing this asset
                if (isUploadAsset)
                {
                    AuditLogManager.LogAssetAction(asset, CurrentUser, AuditAssetAction.CataloguedAsset);
                    AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.CatalogueAsset, string.Format("Catalogued asset with AssetId: {0}.", asset.AssetId));
                }
                else
                {
                    AuditLogManager.LogAssetAction(asset, CurrentUser, AuditAssetAction.SavedAsset);
                    AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.EditAsset, string.Format("Saved asset with AssetId: {0}", asset.AssetId));
                }

                // Check if we want to use the first asset as the template, and this is it
                // If so, loop through the assets and copy data from this asset to them
				if (SessionInfo.Current.AdminSessionInfo.AssetPager.UseFirstAssetAsTemplate && assetId == SessionInfo.Current.AdminSessionInfo.AssetPager.FirstAssetId())
                {
                    // Copy data from this asset to all of those selected for cataloging
                    foreach (int id in SessionInfo.Current.AdminSessionInfo.AssetPager.AssetIdList)
                        AssetManager.MergeForCataloguing(asset, id);

                    // Clear the flag so we don't change the assets if the template is edited again
                    SessionInfo.Current.AdminSessionInfo.AssetPager.UseFirstAssetAsTemplate = false;
                }

                // Asset has been saved so remove it from the upload list
				RemoveAssetIdFromUploadList(assetId);

                switch (e.CommandName)
                {
                    case "Previous":
                    case "Next":

                        string nextAssetId = e.CommandArgument.ToString();
                        string redirectUrl = string.Format("AssetForm.aspx?assetId={0}", nextAssetId);
                        Response.Redirect(redirectUrl, false);

                        break;

                    case "Exit":

                        AssetFormPlaceHolder.Visible = false;
                        SuccessPlaceHolder.Visible = true;

						ExternalPublishInfoWrapper.Visible = (ExternalPublisher.Instance.IsPublished(assetId));
						AssetExternalReferenceLabel.Text = ExternalPublisher.Instance.GetHash(assetId);

                        string message = (isUploadAsset) ? "Asset catalogued successfully" : "Asset updated successfully";
                        AssetSavedFeedbackLabel.SetSuccessMessage(message);

                        break;
                }
            }
            catch (InvalidAssetException iaex)
            {
                metadataSortingOn = true;
                MessageLabel1.SetErrorMessage("the following errors occured:", iaex.Errors);
            	ResetScrollPosition();
            }
            catch (InvalidAssetDelegationException iade)
            {
                metadataSortingOn = true;
                MessageLabel1.SetErrorMessage("unable to delegate asset due to the following errors:", iade.Message);
				ResetScrollPosition();
            }
            catch (Exception ex)
            {
                metadataSortingOn = true;
                m_Logger.Fatal(string.Format("Error editing asset with id: {0}", asset.AssetId), ex);
                MessageLabel1.SetErrorMessage("An unhandled error occured", ex.ToString());
				ResetScrollPosition();
            }
        }

		/// <summary>
		/// Resets the scroll position, overriding the MaintainScrollPositionOnPostback setting
		/// This is required if we need to display any validation error messages at the top
		/// of the page, which will not be seen if the page automatically scrolls down.
		/// </summary>
    	private void ResetScrollPosition()
		{
			if (!ClientScript.IsClientScriptBlockRegistered("CreateResetScrollPosition"))
			{
				ClientScript.RegisterClientScriptBlock(GetType(), "CreateResetScrollPosition", "function ResetScrollPosition() {" + Environment.NewLine + " var scrollX = document.getElementById('__SCROLLPOSITIONX');" + Environment.NewLine + " var scrollY = document.getElementById('__SCROLLPOSITIONY');" + Environment.NewLine + " if (scrollX && scrollY) {" + Environment.NewLine + "scrollX.value = 0;" + Environment.NewLine + "scrollY.value = 0;" + Environment.NewLine + " }" + Environment.NewLine + "}", true);
				ClientScript.RegisterStartupScript(GetType(), "CallResetScrollPosition", "ResetScrollPosition();", true);
			}
		}

		/// <summary>
		/// Saves any posted files with the asset
		/// </summary>
		private void SaveAttachedFiles(Asset asset)
		{
			int count = 0;

			foreach (string key in Request.Files.AllKeys)
			{
				if (key.StartsWith("AttachedFile_"))
				{
					HttpPostedFile file = Request.Files[key];

					if (file == null)
						continue;

					AssetFileManager.SaveAttachedFile(asset, new BinaryFile(file));
					count++;
				}
			}

			if (count > 0)
				AssetFileZipper.CreateZip(asset);
		}
		
		protected void AttachedFileRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
			{
				AssetFile file = (AssetFile)e.Item.DataItem;

				Label filenameLabel = (Label)e.Item.FindControl("FilenameLabel");
				LinkButton deleteAttachedFileLinkButton = (LinkButton)e.Item.FindControl("DeleteAttachedFileLinkButton");
				LinkButton downloadAttachedFileLinkButton = (LinkButton)e.Item.FindControl("DownloadAttachedFileLinkButton");

				filenameLabel.Text = FileUtils.GetTruncatedFilename(file.Filename, 35);
				filenameLabel.ToolTip = file.Filename;
				deleteAttachedFileLinkButton.CommandArgument = file.AssetFileId.ToString();
				downloadAttachedFileLinkButton.CommandArgument = file.AssetFileId.ToString();
			}
		}

		protected void DeleteAttachedFileLinkButton_Command(object sender, CommandEventArgs e)
		{
			int assetFileId = NumericUtils.ParseInt32(e.CommandArgument.ToString(), 0);
			AssetFile.Delete(assetFileId);
			AssetFileZipper.CreateZip(Asset);
			BindAttachedFilesRepeater();
		}

		protected void DownloadAttachedFileLinkButton_Command(object sender, CommandEventArgs e)
		{
			int assetFileId = NumericUtils.ParseInt32(e.CommandArgument.ToString(), 0);
			SiteUtils.SendAttachedFile(assetFileId);
		}

		protected void AssetExpiresRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{

			if (AssetExpiresRadioButtonList.SelectedValue.ToLower() == "true")
			{
				ExpiryDatePicker.Enabled = true;
				ExpiryDatePicker.SelectedDate = (Asset.ExpiryDate == Settings.MAX_DATETIME) ? DateTime.Now.AddYears(2) : Asset.ExpiryDate;
			}
			else
			{
				ExpiryDatePicker.Enabled = false;
				ExpiryDatePicker.SelectedDate = null;
			}
		}

		protected void AssetFileMetadataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
			{
				KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)e.Item.DataItem;

				Literal MetadataNameLiteral = (Literal)e.Item.FindControl("MetadataNameLiteral");
				MetadataNameLiteral.Text = kvp.Key;

				Literal MetadataValueLiteral = (Literal)e.Item.FindControl("MetadataValueLiteral");
				MetadataValueLiteral.Text = kvp.Value;
			}
		}

		protected void DeleteAssetLinkButton_Click(object sender, EventArgs e)
		{
			string redirectUrl = (AssetIsBeingCataloged) ? "AssetsInSession.aspx" : "AssetList.aspx?Message=AssetDeleted";
			AssetManager.DeleteAsset(CurrentUser, Asset.AssetId.GetValueOrDefault());
			Response.Redirect(redirectUrl);
		}

        #endregion

        #region Private Methods

		private void AddCustomMetadata(Asset asset)
		{
            foreach (RepeaterItem ri in MetadataRepeater.Items)
		    {
                if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
                {
                    var metadataInput = (MetadataInputWrapper)ri.FindControl("MetadataInput");
                    var fieldIdHidden = (HiddenField)ri.FindControl("FieldIdHidden");

                    var metaFieldName = fieldIdHidden.Value;
                    var metaSetting = asset.Brand.GetMetadataSetting(metaFieldName);
                    if (metaSetting.OnAssetForm)
                    {
                        IEnumerable<int> ids;
                        string stringVal;

                        metadataInput.GetSelection(out ids, out stringVal);

                        if(ids!=null)
                        {
                            var metadataList = (from m in MetadataCache.Instance.GetList()
                                                join x in ids on m.MetadataId equals x
                                                select m);

                            asset.MetadataList.AddRange(metadataList);    
                        }
                        
                        if(!string.IsNullOrEmpty(stringVal))
                        {
                            if(metaSetting.UiControlType == (int)BrandMetadataUiControlType.TextField)
                            {
                                var a = AssetMetadataTextField.New();
                                a.TextFieldValue = stringVal;
                                a.GroupNumber = metaSetting.GroupNumber;
                                asset.MetadataTextFieldsList.Add(a);
                            }
                            if(metaSetting.UiControlType == (int)BrandMetadataUiControlType.TextArea)
                            {
                                var a = AssetMetadataTextArea.New();
                                a.TextAreaValue = stringVal;
                                a.GroupNumber = metaSetting.GroupNumber;
                                asset.MetadataTextAreasList.Add(a);
                            }
                            if (metaSetting.UiControlType == (int)BrandMetadataUiControlType.Select && 
                                metaSetting.SelectableSetting.SelectableType == (int)SelectableMetadataType.PresetTextArea)
                            {
                                var a = AssetMetadataTextArea.New();
                                a.TextAreaValue = stringVal;
                                a.GroupNumber = metaSetting.GroupNumber;
                                asset.MetadataTextAreasList.Add(a);
                            }
                        }
                    }
                }
		    }
		}

		private void BindAttachedFilesRepeater()
		{
			AssetFileFinder finder = new AssetFileFinder { AssetId = Asset.AssetId.GetValueOrDefault(), AssetFileType = AssetFileType.AttachedFile};
			List<AssetFile> files = AssetFile.FindMany(finder);
			AttachedFileRepeater.DataSource = files;
			AttachedFileRepeater.DataBind();
			AttachedFileWrapper.Visible = (files.Count < 5);
		}

		private static string GetDuration(Asset asset)
		{
			if (!asset.IsProcessed)
				return "being processed...";

			return SiteUtils.FriendlyDuration(asset.Duration.GetValueOrDefault(), "unknown");
		}

		private void AttachCopyrightOwnerJavascript()
		{
			if (!Page.ClientScript.IsClientScriptBlockRegistered("ToggleCopyrightOwnerScript"))
			{
				string script = @"function toggleCopyrightOwner(isOwnerOrg)
								{
									var e = document.getElementById('" + CopyrightOwnerTextBox.ClientID + @"');
									if (isOwnerOrg)
									{
										e.value = '';
										e.disabled = true;
									}
									else
									{
										e.disabled = false;
									}
								}";

				Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ToggleCopyrightOwnerScript", script, true);
			}

			CopyrightOwnerRadioButtonList.Items[0].Attributes["OnClick"] = "toggleCopyrightOwner(true)";
			CopyrightOwnerRadioButtonList.Items[1].Attributes["OnClick"] = "toggleCopyrightOwner(false)";
		}

		private void BindAssetCustomMetadata()
		{
            // Get all of the metadata custom settings for this brand
		    var settings = BrandMetadataSettingManager.GetCustomMetadataSettings(BrandDropDownList1.SelectedId);
            
            // order settings
		    var ordered = settings.OrderBy(s => s.CatalogueOrderNum).ToList();

            // Bind repeater to the settings list
            MetadataRepeater.DataSource = ordered;
            MetadataRepeater.DataBind();

			// Refresh the categories list for the selected brand
			CategoriesTreeView.RefreshFromBrandAndSelect(BrandDropDownList1.SelectedId, Asset.CategoryList.Select(c => c.CategoryId.GetValueOrDefault()));
			CategoriesTreeView.CheckNode(PrimaryCategoryId);
			UpdateCategoriesListBox();
		}

        private static AssetPublishStatus GetAssetPublishStatus(Asset asset)
        {
            // Get the asset publish status
            AssetPublishStatus assetPublishStatus = asset.AssetPublishStatus;

            // Super Admins don't need to submit to workflow so we set the
            // asset publish status of approved regardless of what is really is
            // unless the asset has already been submitted to a workflow

            bool isRoleAboveUploadUser = (CurrentUser.UserRoleId > Convert.ToInt32(UserRole.UploadUser));
            bool isAssetNotApproved = (assetPublishStatus == AssetPublishStatus.NotApproved);

            if (isRoleAboveUploadUser && isAssetNotApproved)
                assetPublishStatus = AssetPublishStatus.Approved;

            // All done.  Return value
            return assetPublishStatus;
        }

        private static void RemoveAssetIdFromUploadList(int assetId)
        {
            // Remove from the session list
            SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Remove(assetId);

            // Save the uploaded asset list to the logged in user
            SiteUtils.SaveUploadedAssetListToCurrentUser();
        }

        private void DisplayAssetFileInformation(Asset asset)
        {
            DimensionsLabel.Text = asset.GetDimensions();
            OrientationLabel.Text = asset.GetOrientation();
            DurationLabel.Text = GetDuration(asset);
            FilenameLabel.Text = asset.Filename;
        	FileHashLabel.Text = GeneralUtils.GetNonEmptyString(asset.FileHash, "[not available]");
            SizeLabel.Text = FileUtils.FriendlyFileSize(asset.FileSize);

            if (!asset.IsProcessed)
            {
                DimensionsLabel.CssClass = "FbkNegTxt";
                DurationLabel.CssClass = "FbkNegTxt";
            }
        }

        #endregion

    	#region Categories Functionality

		protected void CategoriesTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
		{
			int id = NumericUtils.ParseInt32(e.Node.Value, 0);
			CategoriesTreeView.ToggleNode(id, e.Node.Checked);
			UpdateCategoriesListBox();
		}

    	protected void MakePrimaryCategoryButton_Click(object sender, EventArgs e)
    	{
    		if (CategoriesListBox.SelectedIndex == -1)
    			return;

    		SetAndDisplayPrimaryCategory(NumericUtils.ParseInt32(CategoriesListBox.SelectedValue, 0));
    	}

    	protected void RemoveCategoryButton_Click(object sender, EventArgs e)
    	{
    		// Exit if no category was selected
    		if (CategoriesListBox.SelectedIndex == -1)
    			return;

    		// Get the ID of the selected item, and uncheck it
    		int id = NumericUtils.ParseInt32(CategoriesListBox.SelectedValue, 0);
    		CategoriesTreeView.UncheckNode(id);

    		// Also update the listbox to remove the item
    		UpdateCategoriesListBox();
    	}

    	private void UpdateCategoriesListBox()
		{
			CategoriesListBox.Items.Clear();
			bool primaryCategorySelected = false;

			foreach (TreeNode node in CategoriesTreeView.GetSelectedNodes())
			{
				int id = NumericUtils.ParseInt32(node.Value, 0);
				Category entity = CategoryCache.Instance.GetById(id);
				string fullname = SiteUtils.GetFullCategoryName(entity);

				if (id == PrimaryCategoryId)
					primaryCategorySelected = true;

				CategoriesListBox.Items.Add(new ListItem(fullname, entity.CategoryId.ToString()));
			}

			if (!primaryCategorySelected)
			{
				PrimaryCategoryId = 0;
				PrimaryCategoryTextBox.Text = string.Empty;

				if (CategoriesListBox.Items.Count > 0)
				{
					int id = NumericUtils.ParseInt32(CategoriesListBox.Items[0].Value, 0);
					PrimaryCategoryId = id;
					primaryCategorySelected = true;
				}
			}

			if (!primaryCategorySelected)
				PrimaryCategoryId = 0;

			DisplayPrimaryCategory(PrimaryCategoryId);
		}

    	private void SetAndDisplayPrimaryCategory(int categoryId)
		{
			PrimaryCategoryId = categoryId;
    		DisplayPrimaryCategory(categoryId);
    	}

    	private void DisplayPrimaryCategory(int categoryId)
    	{
    		Category category = CategoryCache.Instance.GetById(categoryId);
    		PrimaryCategoryTextBox.Text = SiteUtils.GetFullCategoryName(category);
    	}

    	#endregion

    	#region Brand & Metadata Related Functionality

		private bool SetWorkflowAndPublishStatus(Asset asset)
		{
			if (PublishWrapper.Visible)
			{
				// Asset can be published, no workflow needed.

				asset.AssetPublishStatus = (IsPublishedCheckBox.Checked) ? AssetPublishStatus.Published : AssetPublishStatus.NotPublished;
				return false;
			}

			if (WorkflowSelectorRow.Visible && !WorkflowDropDownList.Enabled)
			{
				// Asset is already pending approval, no action needed.

				return false;
			}

			if (WorkflowSelectorRow.Visible && WorkflowDropDownList.Enabled)
			{
				// Workflow selector is visible and dropdown is enabled
				// A workflow to submit the asset to must be selected
				// But the workflow will only be used when the asset
				// is actually submitted for approval

				asset.WorkflowId = null;

				if (WorkflowDropDownList.SelectedId > 0)
					asset.WorkflowId = WorkflowDropDownList.SelectedId;

				if (SubmitToWorkflowCheckbox.Enabled && SubmitToWorkflowCheckbox.Checked)
				{
					asset.AssetPublishStatus = AssetPublishStatus.PendingApproval;
					return true;
				}
				
				return false;
			}

			if (!WorkflowSelectorRow.Visible && CurrentUser.UseWorkflow)
			{
				// Workflow selector is not visible, but the current user is
				// set to use a workflow.  In this case, we need to set the
				// workflow to the one assigned to the user, but it is only
				// actually used when the asset is submitted for approval

				asset.WorkflowId = CurrentUser.WorkflowId;

				if (SubmitToWorkflowCheckbox.Enabled && SubmitToWorkflowCheckbox.Checked)
				{
					asset.AssetPublishStatus = AssetPublishStatus.PendingApproval;
					return true;
				}

				return false;
			}

			throw new SystemException("Unable to set workflow and publish status (might be UI mismatch)");
		}

    	#endregion

		#region Linked Assets

		private void BindLinkedAssetsRepeater()
		{
			// Store the current link assets in the viewstate
			if (!Page.IsPostBack)
				LinkedAssetList.AddRange(Asset.LinkedAssetList);

			// Add an empty asset to the list so that our binding works
			// as this is needed to add new assets
			if (LinkedAssetList.Count == 0)
				LinkedAssetList.Add(Asset.New());

			LinkedAssetsRepeater.DataSource = LinkedAssetList;
			LinkedAssetsRepeater.DataBind();
		}

		protected void LinkedAssetIdTextBox_TextChanged(object sender, EventArgs e)
		{
			ReloadLinkedAssets();
			BindLinkedAssetsRepeater();
		}

		private void ReloadLinkedAssets()
		{
			LinkedAssetList.Clear();

			foreach (RepeaterItem item in LinkedAssetsRepeater.Items)
			{
				TextBox LinkedAssetIdTextBox = (TextBox)item.FindControl("LinkedAssetIdTextBox");
				Asset linkedAsset = GetLinkedAsset(LinkedAssetIdTextBox.Text);
				LinkedAssetList.Add(linkedAsset);
			}
		}

		private Asset GetLinkedAsset(string assetIdText)
		{
			Asset linkedAsset = Asset.New();
			int assetId;

			// Validate user input and check if the asset exists or if it wasn't already added.
			if (int.TryParse(assetIdText, out assetId))
			{
				if (assetId == Asset.AssetId)
				{
					linkedAsset.Title = "asset cannot be linked to itself";
				}
				else
				{
					if (LinkedAssetList.Find(a => a.AssetId == assetId) != null)
					{
						linkedAsset.Title = "asset already linked";
					}
					else
					{
						linkedAsset = Asset.Get(assetId);

						if (linkedAsset.IsNull)
						{
							linkedAsset = Asset.New();
							linkedAsset.Title = "asset not found";
						}
					}
				}
			}
			else
			{
				if (assetIdText != string.Empty)
					linkedAsset.Title = "invalid asset id";
			}

			return linkedAsset;
		}

		protected void AddLinkedAssetButton_Click(object sender, EventArgs e)
		{
			LinkedAssetList.Add(Asset.New());
			BindLinkedAssetsRepeater();
		}

		protected void LinkedAssetDeleteLinkButton_Command(object sender, CommandEventArgs e)
		{
			//  Delete by index as there might be some invalid assets, so won't
			//  be able to remove them taking the id.  

			int index = NumericUtils.ParseInt32(e.CommandArgument.ToString(), 0);
			LinkedAssetList.RemoveAt(index);

			BindLinkedAssetsRepeater();

			LinkedAssetsRepeaterUpdatePanel.Update();
		}

        protected void MetadataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var setting = (BrandMetadataSetting) e.Item.DataItem;

                var metadataVisibilityWrapper = (MetadataVisibilityWrapper)e.Item.FindControl("MetadataVisibilityWrapper");
                var metadataInput = (MetadataInputWrapper)e.Item.FindControl("MetadataInput");
                var fieldIdHidden = (HiddenField)e.Item.FindControl("FieldIdHidden");
                var brandAdminMetadataLabel = (BrandAdminMetadataLabel)e.Item.FindControl("BrandAdminMetadataLabel");
                var liMetaRepeater = (HtmlGenericControl)e.Item.FindControl("LiMetaRepeater");

                //set field id for the wrapper and label
                metadataVisibilityWrapper.FieldName = brandAdminMetadataLabel.FieldName = fieldIdHidden.Value = setting.FieldId;

                //set the order number so that meta section is sorted according to it
                liMetaRepeater.Attributes["class"] = setting.CatalogueOrderNum.ToString();

                //init the input
                var groupNum = setting.GroupNumber;
                metadataInput.GroupNumber = groupNum;
                metadataInput.BrandId = BrandDropDownList1.SelectedId;
                metadataInput.ResetState();
                metadataInput.InitInput(true);
                metadataInput.RefreshFromBrandAndSelet(
                                        BrandDropDownList1.SelectedId, 
                                        Asset.GetMetadataForGroup(groupNum).Select(m => m.MetadataId.GetValueOrDefault()),
                                        Asset.GetStringValForGroup(groupNum)
                                                      );
            }
        }

        protected void LinkedAssetsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				// Get the asset being linked
				Asset linkedAsset = (Asset)e.Item.DataItem;

				// Get the controls
				PlaceHolder LinkedAssetActionLinks = (PlaceHolder) e.Item.FindControl("LinkedAssetActionLinks");
				HyperLink LinkedAssetNameLinkButton = (HyperLink)e.Item.FindControl("LinkedAssetNameLinkButton");
				HyperLink LinkedAssetViewHyperLink = (HyperLink)e.Item.FindControl("LinkedAssetViewHyperLink");
				LinkButton LinkedAssetDeleteLinkButton = (LinkButton)e.Item.FindControl("LinkedAssetDeleteLinkButton");
				FeedbackLabel InvalidAssetIdMessage = (FeedbackLabel)e.Item.FindControl("InvalidAssetIdMessage");
				TextBox LinkedAssetIdTextBox = (TextBox)e.Item.FindControl("LinkedAssetIdTextBox");

				if (linkedAsset.IsNew)
				{
					// This is a new asset, so hide the action links
					LinkedAssetActionLinks.Visible = false;

					// Check the asset title, as this is being used as a temp store
					// for error messages, so if contains an error, then display it.
					if (linkedAsset.Title != string.Empty)
					{
						InvalidAssetIdMessage.SetErrorMessage(linkedAsset.Title);
						InvalidAssetIdMessage.Visible = true;
					}

					return;
				}

				// Otherwise, this is not a new asset, so populate the controls accordingly
				// Is a valid linked asset, so show the action buttons
				LinkedAssetActionLinks.Visible = true;

				// Populate the textbox with the asset id
				LinkedAssetIdTextBox.Text = linkedAsset.AssetId.ToString();

				// Get the URL for the popup.
				string infoUrl = SiteUtils.GetShowAssetInfoUrl(linkedAsset.AssetId.GetValueOrDefault());

				// Display the asset title, limit this to 50 chars.
				LinkedAssetNameLinkButton.Text = StringUtils.BreakAt(linkedAsset.Title, 50, "...");
				LinkedAssetNameLinkButton.Visible = true;
				LinkedAssetNameLinkButton.NavigateUrl = infoUrl;
				
				// Display the button to view the asset info
				LinkedAssetViewHyperLink.Visible = true;
				LinkedAssetViewHyperLink.NavigateUrl = infoUrl;

				// Ensure title is visible
				if (StringUtils.IsBlank(LinkedAssetNameLinkButton.Text))
					LinkedAssetNameLinkButton.Text = "(No Title)";

				// If linked asset is not visible (eg. not published or expired) then display an error
				if (!linkedAsset.IsPublishedAndAvailable())
				{
					LinkedAssetNameLinkButton.NavigateUrl = "javascript:void(null)";
					LinkedAssetViewHyperLink.NavigateUrl = "javascript:void(null)";

					InvalidAssetIdMessage.SetErrorMessage("asset not published");
					InvalidAssetIdMessage.Visible = true;
				}

				// Update action button command args
				LinkedAssetDeleteLinkButton.CommandArgument = e.Item.ItemIndex.ToString();
			}
		}

		#endregion
    }
}