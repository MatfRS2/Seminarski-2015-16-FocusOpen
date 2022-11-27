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
using System.Reflection;
using System.Security;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Components.Exporting;
using FocusOPEN.Website.Controls;
using log4net;

namespace FocusOPEN.Website.Admin
{
	public partial class AssetList : BaseAdminPageableEntityListPage<AssetFinder, Asset>
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region BaseAdminPageablePage Implementation

		protected override BaseSavedAdminSearch<AssetFinder> SavedSearch
		{
			get
			{
				return SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch;
			}
		}

		protected override ISortableControl DataControl
		{
			get
			{
				return AssetList1.Repeater;
			}
		}

		protected override EntityList<Asset> GetData(AssetFinder finder, int page)
		{
			AssetList1.ShowThumbnails = SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch.ShowThumbnails;
			CookieManager.SetValue("AdminSavedAssetSearchPageSize", PageSize);
			SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch.AssetPublicationStatus = EnumUtils.GetEnumFromValue<AssetPublicationStatus>(AssetPublicationStatusDropDownList1.SelectedId);

			return Asset.FindMany(finder, page, PageSize);
		}

		protected override AssetFinder GetFinderFromForm()
		{
			AssetFinder finder = GetBaseFinder();
			finder.GeneralKeyword = SearchTermTextBox.Text.Trim();

			finder.OrderBy = OrderBy.Custom;
			finder.SortExpressions.AddRange(AssetList1.Repeater.GetSortExpressions());

			if (AssetTypeDropDownList1.SelectedId != 0)
				finder.AssetTypeId = AssetTypeDropDownList1.SelectedId;

			if (BrandSelectorWrapper.Visible)
				finder.BrandId = BrandIdDropdown.SelectedId;

			if (CategoryDropDownList.SelectedId > 0)
				finder.CategoryIdList.AddRange(CategoryManager.GetCategoryIdsForSearch(CategoryDropDownList.SelectedId));

			if (UploadUserSelectorWrapper.Visible && UploadUserDropDownList.SelectedId != 0)
				finder.UploadedByUserId = UploadUserDropDownList.SelectedId;

			SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch.ShowThumbnails = ShowThumbnailsCheckBox.Checked;

			AssetPublicationStatus aps = EnumUtils.GetEnumFromValue<AssetPublicationStatus>(AssetPublicationStatusDropDownList1.SelectedId);

			switch (aps)
			{
				case (AssetPublicationStatus.AllAssets):
					break;

				case (AssetPublicationStatus.PrepublicationDateAssets):
					finder.IsBeforePublicationDate = true;
					finder.IsPublished = true;
					break;

				case (AssetPublicationStatus.PendingAssets):
					finder.AssetPublishStatusId = (int) AssetPublishStatus.PendingApproval;
					break;

				case (AssetPublicationStatus.PublishedAssets):
					finder.IsPublished = true;
					break;

				case (AssetPublicationStatus.ExpiredAssets):
					finder.IsExpired = true;
					break;

				case (AssetPublicationStatus.UnpublishedAssets):
					finder.IsPublished = false;
					break;
			}

			return (finder);
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			AssetFinder finder = SavedSearch.Finder;

			SearchTermTextBox.Text = finder.GeneralKeyword.Trim();
			AssetTypeDropDownList1.SelectedValue = finder.AssetTypeId.ToString();
			BrandIdDropdown.SafeSelectValue(finder.BrandId);

			if (BrandIdDropdown.SelectedId > 0)
				CategoryDropDownList.RefreshFromBrand(BrandIdDropdown.SelectedId);

			if (finder.CategoryIdList.Count > 0)
				CategoryDropDownList.SafeSelectValue(finder.CategoryIdList[0]);

			UploadUserDropDownList.SafeSelectValue(finder.UploadedByUserId);
			AssetPublicationStatusDropDownList1.SelectedValue = Convert.ToInt32(SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch.AssetPublicationStatus).ToString();
			ShowThumbnailsCheckBox.Checked = SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch.ShowThumbnails;

			DataControl.SortExpression = SavedSearch.SortExpression;
			DataControl.SortAscending = SavedSearch.SortAscending;

			int page = SavedSearch.Page;
			PerformSearchAndBindGrid(finder, page);
		}

		protected override void TogglePanels(bool isEmptyResultSet)
		{
			GridPanel.Visible = !isEmptyResultSet;
		}

		#endregion

        #region Accessors

        protected override ContextType AssetContext
        {
            get
            {
                return ContextType.ViewAssetList;
            }
        }

        #endregion

        #region Handled Events

        protected void Page_Init(object sender, EventArgs e)
		{
			// Only super admins can bulk delete assets
			BulkDeleteButton1.Visible = BulkDeleteButton2.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator);

			// Wire up asset repeater events
			AssetList1.Delete += new CommandEventHandler(DeleteLinkButton_OnCommand);
			AssetList1.SortChanged += new CommandEventHandler(SortListLinkButton_Command);

			// Add event handlers for brand dropdowns, as we need to change the upload user dropdown accordingly
			BrandIdDropdown.SelectedIndexChanged += new EventHandler(BrandIdDropdown_SelectedIndexChanged);

			if (!Page.IsPostBack)
			{
				// Only show brand selector to super-admins when there is more than one brand available
				BrandSelectorWrapper.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);

				// Only show users who are at least upload users
				UploadUserDropDownList.MinimumUserRoleId = Convert.ToInt32(UserRole.UploadUser);

				switch (CurrentUser.UserRole)
				{
					case (UserRole.SuperAdministrator):

						// Super admins can do everything.  No need to restrict the UI here
						// The UploadUserDropDownList will show all upload users

						break;

					case (UserRole.BrandAdministrator):

						// Brand Administrators should only see upload users or above
						// in their own Brand, so filter the UploadUserDropdown here.

						UploadUserDropDownList.BrandId = CurrentUser.PrimaryBrandId;

						break;

					case (UserRole.UploadUser):

						// Neither the brand or upload user selectors are available
						// to upload users, so we do not need to do anything here

						break;
				}

				UploadUserDropDownList.RefreshFromDataSource();
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteUtils.TieButton(SearchTermTextBox, SearchButton);
		}

        protected new void SearchButton_Click(object sender, EventArgs e)
        {
            if(DownloadCSVRadioButton.Checked)
            {
                ExportToCSV();
            }else if(DownloadXMLRadioButton.Checked)
            {
                ExportToXML();
            }else //default selection...no need for an if:  if (ViewOnPageRadioButton.Checked)
            {
                base.SearchButton_Click(sender, e);
            }
        }

		protected void BrandIdDropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			CategoryDropDownList.RefreshFromBrand(BrandIdDropdown.SelectedId);

			UploadUserDropDownList.BrandId = BrandIdDropdown.SelectedId;
			UploadUserDropDownList.RefreshFromDataSource();
		}

		private void DeleteLinkButton_OnCommand(object sender, CommandEventArgs e)
		{
			int assetId = Convert.ToInt32(e.CommandArgument);
			AssetManager.DeleteAsset(CurrentUser, assetId);
			PerformSearchAndBindGrid(CurrentPage);
		}

		protected void SelectAllButton_Click(object sender, EventArgs e)
		{
			ToggleCheckBoxes(true);
		}

		protected void DeselectAllButton_Click(object sender, EventArgs e)
		{
			ToggleCheckBoxes(false);
		}

		protected void BulkRenewButton_Click(object sender, EventArgs e)
		{
			RedirectToActionPage("AssetsBulkRenewal.aspx");
		}

		protected void BulkDelegateButton_Click(object sender, EventArgs e)
		{
			RedirectToActionPage("AssetsBulkDelegation.aspx");
		}

		protected void BulkChangeCategoryButton_Click(object sender, EventArgs e)
		{
			List<int> categoryIdList = SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch.Finder.CategoryIdList;

			if (categoryIdList.Count == 0)
			{
				MessageLabel1.SetErrorMessage("Unable to reassign assets; no category selected. Please search for assets assigned to a specific category and try again.");
				return;
			}

			List<int> selectedIdList = GetSelectedAssetIdList();

			if (selectedIdList.Count == 0)
			{
				MessageLabel1.SetErrorMessage("No assets selected");
				return;
			}

			Session["BulkAction_DefaultCategoryId"] = categoryIdList[0];
			Session["BulkAction_DeleteCategory"] = false;

			RedirectToActionPage("AssetsBulkChangeCategory.aspx");
		}

        protected void BulkTemplateCatalogueButton_Click(object sender, EventArgs e)
        { 
            //Check if there are any assets selected.
            List<int> selectedIdList = GetSelectedAssetIdList();

            if (selectedIdList.Count == 0)
            {
                MessageLabel1.SetErrorMessage("No assets selected");
                return;
            }

            //We'll use our new list to add the items that are saved in session. This means that the selected items will be at the 
            //beginning of the list.
            selectedIdList.AddRange(SessionInfo.Current.AdminSessionInfo.UploadedAssetsList);

            SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Clear();
            SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.AddRange(selectedIdList);

            //NOTE: We do not need to check the number of items selected as the column type where the items will be saved is varchar(max).
            SiteUtils.SaveUploadedAssetListToCurrentUser();

            Response.Redirect("AssetsInSession.aspx");
        }

		protected void BulkRegeneratePreviewsButton_Click(object sender, EventArgs e)
		{
			List<int> selectedIdList = GetSelectedAssetIdList();

			if (selectedIdList.Count == 0)
			{
				MessageLabel1.SetErrorMessage("No assets selected");
				return;
			}

			// Get the assets that we need to reprocess
			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(selectedIdList);
			List<Asset> selectedAssets = Asset.FindMany(finder);

			if (selectedAssets.Count == 0)
			{
				MessageLabel1.SetErrorMessage("Selected assets no longer exist");
				return;
			}

			// Initialise error list for problems that might occur as assets are processed
			ErrorList errors = new ErrorList();

			// Counters
			int successCount = 0;
			int failCount = 0;

			foreach (Asset asset in selectedAssets)
			{
				m_Logger.DebugFormat("Re-processing asset ID = {0}", asset.AssetId);

				AssetFileInfo afi = new AssetFileInfo(asset);

				if (!afi.FileExists)
				{
					errors.Add(string.Format("Asset file for asset with ID {0} is missing or unavailable", asset.AssetId));
					m_Logger.Warn("Asset file does not exist.  Cannot reprocess");
					continue;
				}

				try
				{
					if (APSGateway.Instance.ProcessFile(asset, false, FileOutputs.All))
					{
						asset.IsProcessed = false;
						Asset.Update(asset);
						successCount++;

						m_Logger.DebugFormat("Asset {0} resubmitted for processing successfully", asset.AssetId);
					}
					else
					{
						failCount++;
						errors.Add(string.Format("#{0} : An error occured when submitting the asset to the processing service", asset.AssetId));
						m_Logger.DebugFormat("Asset {0} not submitted to the processing service. An error occurred.", asset.AssetId);
					}
				}
				catch (InvalidAssetException iaex)
				{
					failCount++;
					m_Logger.Warn(string.Format("An error occured when processing the asset: {0}", asset.AssetId), iaex);
					errors.Add(string.Format("#{0}: Error submitting asset for processing: {1}", asset.AssetId, iaex.Message));
				}
				catch (Exception ex)
				{
					failCount++;
					m_Logger.Warn(string.Format("An unknown error occured when processing the asset: {0}", asset.AssetId), ex);
					errors.Add(string.Format("#{0}: Error submitting asset for processing", asset.AssetId));

					ExceptionHandler.HandleException(ex, "Error bulk regenerating previews");
				}
			}

			m_Logger.DebugFormat("Asset processing result: {0} success, {1} failures, Errors: {2}", successCount, failCount, errors);

			if (successCount == 0)
			{
				MessageLabel1.SetErrorMessage("The selected assets could not be submitted for processing", errors);
				return;
			}

			if (failCount == 0)
			{
				MessageLabel1.SetSuccessMessage("All selected assets were resubmitted for reprocessing successfully");
				return;
			}

			if (successCount > 0 && failCount > 0)
			{
				MessageLabel1.SetErrorMessage("Some of the selected assets could not be resubmitted for processing", errors);
				return;
			}
		}

		protected void BulkDeleteButton_Click(object sender, EventArgs e)
		{
			List<int> selectedIdList = GetSelectedAssetIdList();

			if (selectedIdList.Count == 0)
			{
				MessageLabel1.SetErrorMessage("No assets selected");
				return;
			}

			if (selectedIdList.Count >= Settings.MaxBulkDeleteAllowed)
			{
				MessageLabel1.SetErrorMessage(string.Format("Only a maximum of {0} assets can be selected for deletion at one time", Settings.MaxBulkDeleteAllowed));
				return;
			}

			Session["BulkAction_AssetIdList"] = selectedIdList;
			Response.Redirect("AssetsBulkDelete.aspx");
		}

		protected void BulkChangePermissionsButton_Click(object sender, EventArgs e)
		{
			List<int> selectedIdList = GetSelectedAssetIdList();

			if (selectedIdList.Count == 0)
			{
				MessageLabel1.SetErrorMessage("No assets selected");
				return;
			}

			Session["BulkAction_AssetIdList"] = selectedIdList;
			Response.Redirect("AssetsBulkChangePermissions.aspx");
		}

		#endregion

		#region Helper Methods

		private static AssetFinder GetBaseFinder()
		{
			AssetFinder finder = new AssetFinder();

			switch (CurrentUser.UserRole)
			{
				case (UserRole.SuperAdministrator):

					// No restrictions

					break;

				case (UserRole.BrandAdministrator):

					// Only assets in this user's brand

					finder.BrandId = CurrentUser.PrimaryBrandId;

					break;

				case (UserRole.UploadUser):

					// Only assets uploaded by this user

					finder.UploadedByUserId = CurrentUser.UserId.GetValueOrDefault(-1);
					break;

				default:

					throw new SecurityException("Access denied");
			}

			return finder;
		}

		private void RedirectToActionPage(string page)
		{
			List<int> selectedIdList = GetSelectedAssetIdList();

			if (selectedIdList.Count == 0)
			{
				MessageLabel1.SetErrorMessage("No assets selected");
				return;
			}

			Session["BulkAction_AssetIdList"] = selectedIdList;
			Response.Redirect(page);
		}

		private List<Int32> GetSelectedAssetIdList()
		{
			List<Int32> list = new List<int>();

			foreach (RepeaterItem ri in AssetList1.Repeater.Items)
			{
				switch (ri.ItemType)
				{
					case ListItemType.Item:
					case ListItemType.AlternatingItem:

						CheckBox AssetCheckBox = (CheckBox)ri.FindControl("AssetCheckBox");

						if (AssetCheckBox.Checked)
						{
							HiddenField AssetIdHiddenField = (HiddenField)ri.FindControl("AssetIdHiddenField");
							int assetId = Convert.ToInt32(AssetIdHiddenField.Value);
							list.Add(assetId);
						}

						break;
				}
			}

			return list;
		}

		private void ToggleCheckBoxes(bool check)
		{
			foreach (RepeaterItem ri in AssetList1.Repeater.Items)
			{
				switch (ri.ItemType)
				{
					case ListItemType.Item:
					case ListItemType.AlternatingItem:

						CheckBox AssetCheckBox = (CheckBox)ri.FindControl("AssetCheckBox");
						AssetCheckBox.Checked = check;

						break;
				}
			}
		}

		private static readonly string[] REPORT_COLUMNS = new[]
		                                                  	{
		                                                  		"AssetId",
		                                                  		"AssetTypeId",
		                                                  		"AssetTypeName",
		                                                  		"UploadDate",
		                                                  		"Filename",
		                                                  		"FileExtension",
		                                                  		"FileSize",
		                                                  		"FileHash",
		                                                  		"TemplateAssetId",
		                                                  		"WorkflowId",
		                                                  		"Title",
		                                                  		"ProjectCode",
		                                                  		"Originator",
		                                                  		"Description",
		                                                  		"Keywords",
		                                                  		"ContactEmail",
		                                                  		"ProductionMonth",
		                                                  		"ProductionYear",
		                                                  		"BrandId",
		                                                  		"CopyrightOwner",
		                                                  		"UsageRestrictions",
		                                                  		"RestrictAttachedFiles",
		                                                  		"DownloadApprovalRequired",
		                                                  		"WatermarkPreview",
		                                                  		"HideFromExternalUsers",
		                                                  		"AssetFilePathId",
		                                                  		"PublishDate",
		                                                  		"ExpiryDate",
		                                                  		"IsProcessed",
		                                                  		"AssetPublishStatusId",
		                                                  		"AssetPublishStatusName",
		                                                  		"UploadedByUserId",
		                                                  		"UploadedByUserName",
		                                                  		"CreateDate",
		                                                  		"LastUpdate",
		                                                  		"IsDeleted",
		                                                  		"AssetCategories",
		                                                  		"DownloadCount",
		                                                  		"PopularityRank",
		                                                  		"Height",
		                                                  		"Width",
		                                                  		"Duration",
		                                                  		"BrandName"
		                                                  	};
        
		private void ExportToXML()
        {
            var finder = GetFinderFromForm();
            IList<Asset> assetList = Asset.FindMany(finder);

            var groupedTreee = AssetManager.GroupAssetsByPrimaryCategory(assetList, REPORT_COLUMNS);

            var fileGen = new EntityListXmlExporter(groupedTreee);

            var exporter = new Exporter("AssetList.xml", fileGen);

            exporter.Export();
        }

        private void ExportToCSV()
        {
            var finder = GetFinderFromForm();
            List<Asset> assetList = Asset.FindMany(finder);

            var fileGen = new EntityListCsvExporter(assetList);

            foreach (var col in REPORT_COLUMNS)
                fileGen.AddFieldMapping(col, GeneralUtils.SplitIntoSentence(col));

            var exporter = new Exporter("AssetList.csv", fileGen);
            
            exporter.Export();
        }

		#endregion
	}
}