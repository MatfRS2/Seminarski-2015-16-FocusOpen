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
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;
using System.Linq;

namespace FocusOPEN.Business
{
	public static class AssetManager
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Fired when asset is delegated to another user
		/// </summary>
		public static event AssetDelegatedEventHandler AssetDelegated;

		/// <summary>
		/// Gets the asset status indicating whether the asset is available, withdrawn, or expired
		/// </summary>
		public static AssetStatus GetAssetStatusForUser(Asset asset, User user)
		{
			// All assets are available to super administrators
			if (user.UserRole == UserRole.SuperAdministrator)
				return AssetStatus.Available;

			// Assets in same brand are always accessible to brand admin
			if (user.UserRole == UserRole.BrandAdministrator && asset.BrandId == user.PrimaryBrandId)
				return AssetStatus.Available;

			// Assets are always available to their owners
			if (asset.UploadedByUserId == user.UserId)
				return AssetStatus.Available;

			// Unpublished assets are withdrawn
			if (asset.AssetPublishStatus != AssetPublishStatus.Published)
				return AssetStatus.Withdrawn;

			// Assets outside of the publish date or expiry date are expired
			if (asset.PublishDate > DateTime.Now || asset.ExpiryDate < DateTime.Now)
				return AssetStatus.Expired;
			
			// Otherwise, asset is available
			return AssetStatus.Available;
		}

		/// <summary>
		/// Copies data from the template asset to the child asset.  If the template asset ID and child
		/// asset ID are the same, no action is taken but no error is thrown (as the data will already be
		/// the same, so we don't need to copy anything)
		/// </summary>
		/// <param name="templateAsset">The template asset</param>
		/// <param name="childAssetId">The AssetId of the asset to which data from the template asset should be copied</param>
		public static void MergeForCataloguing(Asset templateAsset, int childAssetId)
		{
			// We don't want to merge the template asset with itself...
			if (templateAsset.AssetId.GetValueOrDefault() == childAssetId)
				return;

			// Get the template and child assets
			Asset childAsset = Asset.Get(childAssetId);

			// Quit if the template or child are not found
			if (templateAsset.IsNull || childAsset.IsNull)
				return;

			// Manually copy fields from the template to the child.
			// Generally, these will be fields not set from the AssetUploader class
            childAsset.AssetTypeId = templateAsset.AssetTypeId;
			childAsset.TemplateAssetId = templateAsset.AssetId.GetValueOrDefault();
			childAsset.Title = templateAsset.Title;
			childAsset.ProjectCode = templateAsset.ProjectCode;
			childAsset.Originator = templateAsset.Originator;
			childAsset.Description = templateAsset.Description;
			childAsset.Keywords = templateAsset.Keywords;
			childAsset.ProductionDay = templateAsset.ProductionDay;
			childAsset.ProductionMonth = templateAsset.ProductionMonth;
			childAsset.ProductionYear = templateAsset.ProductionYear;
			childAsset.UsageRestrictions = templateAsset.UsageRestrictions;
			childAsset.RestrictAttachedFiles = templateAsset.RestrictAttachedFiles;
			childAsset.WatermarkPreview = templateAsset.WatermarkPreview;
			childAsset.InternalUsers_DownloadApprovalRequired = templateAsset.InternalUsers_DownloadApprovalRequired;
			childAsset.InternalUsers_HideFromUsers = templateAsset.InternalUsers_HideFromUsers;
			childAsset.ExternalUsers_DownloadApprovalRequired = templateAsset.ExternalUsers_DownloadApprovalRequired;
			childAsset.ExternalUsers_HideFromUsers = templateAsset.ExternalUsers_HideFromUsers;
			childAsset.PublishDate = templateAsset.PublishDate;
			childAsset.ExpiryDate = templateAsset.ExpiryDate;

			// The fields are set from the asset uploader, but we want to change them
			childAsset.BrandId = templateAsset.BrandId;
			childAsset.CopyrightOwner = templateAsset.CopyrightOwner;

            //clear metadata in case it was loaded before - it will be deleted
            //a bit later and replaced by the template's ones anyway
            childAsset.MetadataList.Clear();
            childAsset.MetadataTextAreasList.Clear();
            childAsset.MetadataTextFieldsList.Clear();

		    // Save Metadata
			foreach (var metadata in templateAsset.MetadataList)
				childAsset.MetadataList.Add(metadata);

            //Save texts metadata
            foreach (var metadata in templateAsset.MetadataTextAreasList)
                childAsset.MetadataTextAreasList.Add(metadata);

            foreach (var metadata in templateAsset.MetadataTextFieldsList)
                childAsset.MetadataTextFieldsList.Add(metadata);

            //make sure all metadata collections are not null so that
            //meta saving will take place later even if one or more are missing
            childAsset.InitAllMetadataLists();

			// Saved linked assets
			foreach (var linkedasset in templateAsset.LinkedAssetList)
				childAsset.LinkedAssetList.Add(linkedasset);

			// Save categories
			foreach (Category category in templateAsset.CategoryList)
				childAsset.CategoryList.Add(category);

			// Set primary category
			childAsset.PrimaryCategoryId = templateAsset.PrimaryCategoryId;

			// Change last update date
			childAsset.LastUpdate = DateTime.Now;

			// Save changes to DB
			Asset.Update(childAsset);
			Asset.SaveAssetMetadata(childAsset);

			// Copy attached files
        	Asset.CopyAttachedFiles(templateAsset.AssetId.GetValueOrDefault(), childAsset.AssetId.GetValueOrDefault());
		}

		public static void Save(Asset asset, BinaryFile file)
		{
			// Check if asset is delegated. Do not inline.
			bool isDelegated = asset.IsPropertyChanged(Asset.Columns.ContactEmail);

			// Validate the asset
			ErrorList errors = ValidateAsset(asset, file);

			// No point continuing if we have errors
			if (errors.HasErrors)
				throw new InvalidAssetException(errors, asset);

			// Delegate asset if required
			if (isDelegated)
				DelegateAsset(asset);

			// Truncate asset description to 400 chars
			if (asset.Description.Length > 400)
				asset.Description = asset.Description.Substring(0, 400);

			// Update the asset record
			asset.LastUpdate = DateTime.Now;
			
			Asset.Update(asset);
			Asset.SaveAssetMetadata(asset);

			// Save the asset file
			AssetFileManager.SaveAssetFile(asset, file, false);
		}

		public static void ReplaceAssetFile(Asset asset, BinaryFile file, bool notify, User uploadUser)
		{
			m_Logger.DebugFormat("ReplaceAssetFile() - AssetId: {0}", asset.AssetId);

			// Will save the asset file, increment version, etc
			AssetFileManager.SaveAssetFile(asset, file, notify);

			if (asset.AssetPublishStatus == AssetPublishStatus.PendingApproval)
			{
				// The asset is still in a workflow, which we need to cancel and re-submit
				// Get the most recent workflow and perform relevant actions on it

				if (asset.AssetWorkflowList.Count > 0)
				{
					// Get the most recent workflow
					AssetWorkflow assetWorkflow = asset.AssetWorkflowList[0];

					// Cancel it
					WorkflowManager.CancelWorkflow(assetWorkflow);
					m_Logger.DebugFormat("Cancelled AssetWorkflow - AssetWorkflowId: {0}", assetWorkflow.AssetWorkflowId);

					// Resubmit it
					WorkflowManager.SubmitAssetToWorkflow(asset, uploadUser);
					m_Logger.DebugFormat("Resubmitted asset to workflow.  AssetId: {0}", asset.AssetId);
				}
			}

			AuditLogManager.LogAssetAction(asset, uploadUser, AuditAssetAction.ReplacedAssetFile);
			AuditLogManager.LogUserAction(uploadUser, AuditUserAction.ReplacedAssetFile, string.Format("Replaced asset file of AssetId: {0}", asset.AssetId));
		}

		private static ErrorList ValidateAsset(Asset asset, BinaryFile file)
		{
			ErrorList errors = new ErrorList();

            bool hasFullProductionDate = asset.ProductionYear.HasValue && asset.ProductionMonth.HasValue && asset.ProductionDay.HasValue;
            if (hasFullProductionDate)
            {
                string productionDate = String.Format("{0}/{1}/{2}", asset.ProductionYear.Value, asset.ProductionMonth.Value, asset.ProductionDay.Value);
                DateTime dt;
                if ( !DateTime.TryParse(productionDate, out dt) )
                    errors.Add(string.Format("{0} is not a valid date", productionDate));
            }
            
            if (file != null && !file.IsEmpty)
			{
				// We also use a client-side javascript check to ensure that only valid files are uploaded, but this check
				// is just to make absolutely sure that the file is correct (ie. in case the client has javascript disabled)
				if (!asset.AssetType.FileExtensionList.Contains(file.FileExtension.ToLower()))
					errors.Add(string.Format("{0} is not a valid {1} file", file.FileName, asset.AssetType.Name.ToLower()));
			}

			if (asset.BrandId == 0)
				errors.Add(asset.Brand.GetMetadataSetting(BrandMetadataSettings.BRAND).FieldName + " is required");

			var publicationDateField = asset.Brand.GetMetadataSetting(BrandMetadataSettings.PUBLICATION_DATE);
			var expiryDateField = asset.Brand.GetMetadataSetting(BrandMetadataSettings.EXPIRY_DATE);

			if (publicationDateField.IsRequired && asset.PublishDate == DateTime.MinValue)
				errors.Add(publicationDateField.FieldName + " is required");

			if (expiryDateField.IsRequired && asset.ExpiryDate == DateTime.MinValue)
				errors.Add(expiryDateField.FieldName + " is required");

			if (publicationDateField.IsRequired &&
				expiryDateField.IsRequired &&
				asset.ExpiryDate < asset.PublishDate)
				errors.Add(string.Format("{0} must be after {1}", publicationDateField.FieldName, expiryDateField.FieldName));

            // We don't care about validation until asset is published or submitted to a workflow
			if (asset.AssetPublishStatus == AssetPublishStatus.Published || asset.AssetPublishStatus == AssetPublishStatus.PendingApproval)
			{
				// Assets submitted to a workflow must have a workflow specified
				if (asset.AssetPublishStatus == AssetPublishStatus.PendingApproval && asset.WorkflowId.GetValueOrDefault() == 0)
					errors.Add("Workflow is required");

				if (asset.CategoryList.Count == 0)
					errors.Add("At least one category must be selected");

				if (asset.PrimaryCategoryId <= 0)
					errors.Add(string.Format("{0} is required", asset.Brand.GetMetadataSetting(BrandMetadataSettings.PRIMARY_CATEGORY).FieldName));

                //validate custom metadata - do it only if the asset is to be published though!
                // Get all of the metadata custom settings for this brand
                var customMetas = BrandMetadataSettingManager.GetCustomMetadataSettings(asset.BrandId);
                foreach (var setting in customMetas)
                {
                    if (!setting.IsRequired)
						continue;

                    var notOk = false;

                    if (setting.UiControlType == (int)BrandMetadataUiControlType.Select)
                    {
                        var selectableSetting = setting.SelectableSetting;
                        var zeroSelected = asset.GetMetadataForGroup(setting.GroupNumber).Count == 0;
                        var selType = (SelectableMetadataType)selectableSetting.SelectableType;

                        if (selectableSetting.AllowMultiple &&
                           (selType == SelectableMetadataType.ComboBox || selType == SelectableMetadataType.Checkboxes) &&
                            zeroSelected)
                        {
                            notOk = true;
                        }

                        if (selType == SelectableMetadataType.RadioButtons && zeroSelected)
                        {
                            notOk = true;
                        }

                        if (selType == SelectableMetadataType.PresetTextArea)
                        {
                            notOk = string.IsNullOrEmpty(asset.GetStringValForGroup(setting.GroupNumber));
                        }
                    }
                    else
                    {
                        notOk = string.IsNullOrEmpty(asset.GetStringValForGroup(setting.GroupNumber));
                    }

                    if (notOk)
                        errors.Add(string.Format("{0} is required", setting.FieldName));
                }

				// Validate fields
				ValidateTextField(asset, errors, BrandMetadataSettings.TITLE, asset.Title);
				ValidateTextField(asset, errors, BrandMetadataSettings.PROJECT_CODE, asset.ProjectCode);
				ValidateTextField(asset, errors, BrandMetadataSettings.DESCRIPTION, asset.Description);
				ValidateTextField(asset, errors, BrandMetadataSettings.ORIGINATOR, asset.Originator);
				ValidateTextField(asset, errors, BrandMetadataSettings.ADDITIONAL_KEYWORDS, asset.Keywords);
				ValidateTextField(asset, errors, BrandMetadataSettings.COPYRIGHT_OWNER, asset.CopyrightOwner);
				ValidateTextField(asset, errors, BrandMetadataSettings.USAGE_RESTRICTIONS, asset.UsageRestrictions);

				// Ensure contact email has been specified if required
				if (asset.Brand.GetMetadataSetting(BrandMetadataSettings.CONTACT_EMAIL).IsRequired)
				{
					if (StringUtils.IsBlank(asset.ContactEmail))
					{
						errors.Add(asset.Brand.GetMetadataSetting(BrandMetadataSettings.CONTACT_EMAIL).FieldName + " is required");
					}
					else if (!StringUtils.IsEmail(asset.ContactEmail))
					{
						errors.Add(asset.Brand.GetMetadataSetting(BrandMetadataSettings.CONTACT_EMAIL).FieldName + " is invalid");
					}
				}

				// Ensure production date has been entered if required
				if (asset.Brand.GetMetadataSetting(BrandMetadataSettings.DATE_PRODUCED).IsRequired && asset.ProductionYear == 0)
					errors.Add(asset.Brand.GetMetadataSetting(BrandMetadataSettings.DATE_PRODUCED).FieldName + " is required");

				if (asset.Brand.GetMetadataSetting(BrandMetadataSettings.PUBLICATION_DATE).IsRequired)
				{
					if (asset.PublishDate == DateTime.MinValue)
					{
						errors.Add("Publication date is required");
					}
				}

				if (asset.Brand.GetMetadataSetting(BrandMetadataSettings.EXPIRY_DATE).IsRequired)
				{
					if (asset.ExpiryDate == DateTime.MinValue)
					{
						errors.Add("Expiry date is required");
					}
					else if (asset.ExpiryDate.Date < asset.PublishDate.Date)
					{
						errors.Add("Expiry date must be after publication date");
					}
				}
			}

			return errors;
		}

		private static void ValidateTextField(Asset asset, ErrorList errors, string fieldId, string val)
		{
			BrandMetadataSetting setting = asset.Brand.GetMetadataSetting(fieldId);

			if (setting.IsRequired && StringUtils.IsBlank(val))
				errors.Add(setting.FieldName + " is required");
		}

		/// <summary>
		/// Delete asset and update audit log
		/// </summary>
		/// <param name="user">The user deleting the asset</param>
		/// <param name="assetId">The ID of the asset to be deleted</param>
		public static void DeleteAsset(User user, int assetId)
		{
			m_Logger.DebugFormat("DeleteAsset: {0}", assetId);

			Asset asset = Asset.Get(assetId);

			if (asset.IsNull)
				return;

			asset.IsDeleted = true;
			Asset.Update(asset);
			m_Logger.Debug(" -Flagged asset as deleted");

			// Update the audit log
			AuditLogManager.LogAssetAction(asset, user, AuditAssetAction.DeleteAsset);
			AuditLogManager.LogUserAction(user, AuditUserAction.DeleteAsset, string.Format("Deleted Asset with AssetId: {0}", asset.AssetId));
			m_Logger.Debug(" -Updated audit log");

			// Delete asset files
			DeleteAssetFiles(asset, asset.AssetFilePath.Path);

			// Delete asset bitmaps folder
			AssetBitmapGroupManager.DeleteAssetBitmapsFolder(asset);

            // Delete all asset categories
			AssetCategory.DeleteAllByAssetId(assetId);
		}

		/// <summary>
		/// Deletes the asset files including any versioned files.
		/// </summary>
		private static void DeleteAssetFiles(Asset asset, string path)
		{
			// Delete all asset files and any versions
			foreach (string file in Directory.GetFiles(path))
			{
				// Get the filename only, without the extension
				string filenameOnly = (Path.GetFileNameWithoutExtension(file) ?? string.Empty).ToLower();

				// Delete asset files and versions
				if (filenameOnly == asset.FileReference || filenameOnly.StartsWith(asset.FileReference + "_"))
				{
					File.Delete(file);
					m_Logger.DebugFormat(" -Deleted file: {0}", file);
				}
			}

			// Recurse into any subdirectories
			foreach (string dir in Directory.GetDirectories(path))
				DeleteAssetFiles(asset, dir);
		}

        public static XmlTree GroupAssetsByPrimaryCategory(IList<Asset> assets, IEnumerable<string> columnsToDisplay)
        {
            //root element
            var root = new XmlTree("assets");
            
            //if no items were supplied return an empty xml structure
            if (assets == null || assets.Count == 0) return root;

            //group assets by their primary category id - assets without a primary category
            //id go on top of the list and respectively on top of the XML
            var grouped = assets
                            .GroupBy(a => a.PrimaryCategoryId)
                            .OrderByDescending(a=>a.Key == 0);
            
            //prepare columns filter
            var colsLowered = columnsToDisplay == null ? new List<string>() : columnsToDisplay.Select(s => s.ToLower()).ToList();

            foreach (var grp in grouped)
            {
                //set the primary category node
                var primCatId = grp.Key;
                var cat = new XmlTree("category");

                cat.Attributes["id"] = primCatId.ToString();

				if (primCatId == 0)
				{
					//if asset doesn't have a category defined yet - set a default name value
					cat.Attributes["name"] = "no primary category";
				}
				else
				{
					var primCat = grp.First().CategoryList.First(c => c.CategoryId == primCatId);
					cat.Attributes["name"] = primCat.Name;
					cat.Attributes["external_reference"] = primCat.ExternalRef;
				}

                //add the assets to the primary category node
                foreach (var asset in grp)
                {
                    var assetNode = new XmlTree("asset");
                    var propInfo = asset.GetType().GetProperties();
                    var propsToDisplay = colsLowered.Count == 0 ? new List<PropertyInfo>(propInfo) : propInfo.Where(p => colsLowered.Contains(p.Name.ToLower())).ToList();

                    foreach (var p in propsToDisplay)
                    {
                        var val = (p.GetValue(asset, null) ?? "NULL");
                        assetNode.Children.Add(new XmlTree(p.Name, val.ToString()));
                    }
                    
					//add another asset node to primary category node
                    cat.Children.Add(assetNode);
                }
                
				//add another primary category node to root node
                root.Children.Add(cat);
            }
            
            return root;
        }

		#region Delegate Asset Functionality

		/// <summary>
		/// Delegates the asset to the user with the email address set as the ContactEmail
		/// </summary>
		private static void DelegateAsset(Asset asset)
		{
			DelegateAsset(asset, asset.ContactEmail);
		}

		/// <summary>
		/// Delegates an asset to another user
		/// </summary>
		/// <param name="asset">The asset to be delegated</param>
		/// <param name="newContactEmail">The email address of the user to which the asset should be delegated. This must be an upload user or above.</param>
		/// <exception cref="InvalidAssetDelegationException">Thrown if the specified email address does not belong to a valid user, or if belongs to a user without upload permissions</exception>
		private static void DelegateAsset(Asset asset, string newContactEmail)
		{
			if (StringUtils.IsBlank(newContactEmail))
				throw new InvalidAssetDelegationException("System Error: Contact Email is blank", asset, User.Empty);

			// First get the user we are delegating to
			User user = User.GetByEmail(newContactEmail);

			// Ensure we have a valid user
			if (user.IsNull)
				throw new InvalidAssetDelegationException("Email address does not belong to a valid user", asset, user);

			// Ensure the user is at least an upload user
			if (user.UserRoleId < Convert.ToInt32(UserRole.UploadUser))
				throw new InvalidAssetDelegationException("Email address does not belong to a user with upload permissions", asset, user);

			// Otherwise, user is valid, so re-assign the asset
			asset.UploadedByUserId = user.UserId.GetValueOrDefault();
			asset.ContactEmail = user.Email;

			// Save it
			Asset.Update(asset);

			// Log it
			AuditLogManager.LogAssetAction(asset, user, AuditAssetAction.SavedAsset, "Asset was delegated to this user");
			m_Logger.DebugFormat("Asset with AssetId: {0} delegated to user: {1}", asset.AssetId, user.FullName);

			// Fire email notification
			if (AssetDelegated != null)
			{
				AssetDelegatedEventArgs e = new AssetDelegatedEventArgs(asset, user);
				AssetDelegated(null, e);
			}
		}

		#endregion
    }
}