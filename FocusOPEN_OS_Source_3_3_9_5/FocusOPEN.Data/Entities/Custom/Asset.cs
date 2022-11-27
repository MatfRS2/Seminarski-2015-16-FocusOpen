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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public partial class Asset
	{
		#region Lazy Loads

		private int? m_PrimaryCategoryId;
		private User m_UploadedByUser;
		private List<AssetWorkflow> m_AssetWorkflowList;
		private List<AssetOrderHistory> m_AssetOrderHistory;
		private List<AssetMetadata> m_AssetMetadataList;
        private List<AssetMetadataTextField> m_AssetMetadataTextFieldList;
        private List<AssetMetadataTextArea> m_AssetMetadataTextAreaList;
		private List<Metadata> m_MetadataList;
		private List<AssetCategory> m_AssetCategoryList;
		private List<Category> m_CategoryList;
		private List<AssetLink> m_AssetLinkList;
		private List<Asset> m_LinkedAssetList;
		private List<Asset> m_ReciprocalLinkedAssetList;

        public List<AssetOrderHistory> OrderHistory
        {
            get
            {
                if (m_AssetOrderHistory == null)
                {
                    AssetOrderHistoryFinder finder = new AssetOrderHistoryFinder { AssetId = AssetId };
                    m_AssetOrderHistory = AssetOrderHistory.FindMany(finder);
                }
                return m_AssetOrderHistory;
            }
        }

		public List<Asset> LinkedAssetList
		{
			get
			{
				if (m_LinkedAssetList == null)
				{
					AssetFinder finder = new AssetFinder();
					finder.AssetIdList.Add(0);
					AssetLinkList.ForEach(o => finder.AssetIdList.Add(o.LinkedAssetId));
					m_LinkedAssetList = FindMany(finder);

					List<int> idList = AssetLinkList.Select(x => x.LinkedAssetId).ToList();
					m_LinkedAssetList = m_LinkedAssetList.OrderBy(asset => idList.IndexOf(asset.AssetId.GetValueOrDefault())).ToList();
				}
				return m_LinkedAssetList;
			}
		}

		public List<Asset> ReciprocalLinkedAssetList
		{
			get
			{
				if (m_ReciprocalLinkedAssetList == null)
				{
					AssetLinkFinder finder1 = new AssetLinkFinder { AssetId = AssetId.GetValueOrDefault() };
					List<AssetLink> assetList1 = AssetLink.FindMany(finder1);

					AssetLinkFinder finder2 = new AssetLinkFinder { LinkedAssetId = AssetId.GetValueOrDefault() };
					List<AssetLink> assetList2 = AssetLink.FindMany(finder2);

					AssetFinder assetFinder = new AssetFinder();
					assetFinder.AssetIdList.Add(0);
					assetList1.ForEach(la => assetFinder.AssetIdList.Add(la.LinkedAssetId));
					assetList2.ForEach(la => assetFinder.AssetIdList.Add(la.AssetId));

					m_ReciprocalLinkedAssetList = FindMany(assetFinder);
				}

				return m_ReciprocalLinkedAssetList;
			}
		}

		public int PrimaryCategoryId
		{
			get
			{
				if (!m_PrimaryCategoryId.HasValue)
				{
					m_PrimaryCategoryId = 0;

					foreach (AssetCategory ac in AssetCategoryList)
					{
						if (ac.IsPrimary)
						{
							m_PrimaryCategoryId = ac.CategoryId;
							break;
						}
					}
				}

				return m_PrimaryCategoryId.Value;
			}
			set
			{
				m_PrimaryCategoryId = value;
			}
		}

		public List<Category> CategoryList
		{
			get
			{
				if (m_CategoryList == null)
					m_CategoryList = (from c in CategoryCache.Instance.GetList()
									  where AssetCategoryList.Any(ac => ac.CategoryId == c.CategoryId)
									  select c).ToList();

				return m_CategoryList;
			}
		}

		public List<Metadata> MetadataList
		{
			get
			{
				if (m_MetadataList == null)
					m_MetadataList = (from m in MetadataCache.Instance.GetList()
									  where AssetMetadataList.Any(am => am.MetadataId == m.MetadataId)
									  select m).ToList();

				return m_MetadataList;
			}
		}

        public List<AssetMetadataTextField> MetadataTextFieldsList
        {
            get
            {
                if (m_AssetMetadataTextFieldList == null)
                {
                    var finder = new AssetMetadataTextFieldFinder { AssetId = AssetId.GetValueOrDefault() };
                    m_AssetMetadataTextFieldList = AssetMetadataTextField.FindMany(finder);
                }

                return m_AssetMetadataTextFieldList;
            }

        }

        public List<AssetMetadataTextArea> MetadataTextAreasList
        {
            get
            {
                if (m_AssetMetadataTextAreaList == null)
                {
                    var finder = new AssetMetadataTextAreaFinder { AssetId = AssetId.GetValueOrDefault() };
                    m_AssetMetadataTextAreaList = AssetMetadataTextArea.FindMany(finder);
                }

                return m_AssetMetadataTextAreaList;
            }

        }

		public IList<AssetWorkflow> AssetWorkflowList
		{
			get
			{
				if (m_AssetWorkflowList == null)
				{
					AssetWorkflowFinder finder = new AssetWorkflowFinder { AssetId = AssetId.GetValueOrDefault(-1) };
					finder.SortExpressions.Add(new DescendingSort(AssetWorkflow.Columns.AssetWorkflowId));
					finder.SortExpressions.Add(new DescendingSort(AssetWorkflow.Columns.CreateDate));
					m_AssetWorkflowList = AssetWorkflow.FindMany(finder);
				}

				return m_AssetWorkflowList;
			}
		}

		public User UploadedByUser
		{
			get
			{
				if (m_UploadedByUser == null)
					m_UploadedByUser = User.Get(UploadedByUserId);

				return (m_UploadedByUser);
			}
		}

		#endregion

		#region Lazy Load Checks

		/// <summary>
		/// Checks if the asset categories are loaded
		/// </summary>
		private bool AssetCategoriesLoaded()
		{
			return (m_CategoryList != null);
		}

		/// <summary>
		/// Checks if the asset metadata is loaded
		/// </summary>
		private bool AssetMetadataLoaded()
		{
            return (m_MetadataList != null && m_AssetMetadataTextFieldList != null && m_AssetMetadataTextAreaList!= null);
		}

        public void InitAllMetadataLists()
        {
            m_MetadataList = m_MetadataList ?? new List<Metadata>();
            m_AssetMetadataTextFieldList = m_AssetMetadataTextFieldList ?? new List<AssetMetadataTextField>();
            m_AssetMetadataTextAreaList = m_AssetMetadataTextAreaList ?? new List<AssetMetadataTextArea>();
        }

		/// <summary>
		/// Checks if linked asset list is loaded
		/// </summary>
		private bool LinkedAssetsLoaded()
		{
			return (m_LinkedAssetList != null);
		}

		#endregion

		#region Cached Accessors

		/// <summary>
		/// Gets the brand.
		/// </summary>
		/// <value>The brand.</value>
		public Brand Brand
		{
			get
			{
				return BrandCache.Instance.GetById(BrandId);
			}
		}

		/// <summary>
		/// Gets the asset type.
		/// </summary>
		public AssetType AssetType
		{
			get
			{
				return AssetTypeCache.Instance.GetById(AssetTypeId);
			}
		}

		/// <summary>
		/// Gets the asset file path.
		/// </summary>
		/// <value>The asset file path.</value>
		public AssetFilePath AssetFilePath
		{
			get
			{
				return AssetFilePathCache.Instance.GetById(AssetFilePathId);
			}
		}


		#endregion

		#region Public Methods

		public bool IsPropertyChanged(Columns column)
		{
			return ChangedProperties.ContainsKey(column.ToString());
		}

		public List<Metadata> GetMetadataForGroup(int groupNumber)
		{
			return (from m in MetadataList
			        where m.GroupNumber == groupNumber
			        select m).ToList();
		}
        /// <summary>
        /// Returns a string from either the TextArea or TextField custom metadata. If none is find a null string
        /// is returned
        /// </summary>
        /// <param name="groupNum">The custom metadata group number</param>
        /// <returns></returns>
        public string GetStringValForGroup(int groupNum)
        {
            var allSettings = BrandMetadataSetting.FindMany(new BrandMetadataSettingFinder() {});
            var setting = allSettings.FirstOrDefault(s => s.GroupNumber == groupNum);

            //if this metadata is either not found or is of type selectable i.e. there is no textual field then return null
            if(setting == null) 
                return null;

            if(setting.UiControlType == (int)BrandMetadataUiControlType.TextArea ||
               setting.UiControlType == (int)BrandMetadataUiControlType.Select)
            {
                var aa = MetadataTextAreasList.FirstOrDefault(a => a.GroupNumber == groupNum);
                return aa != null ? aa.TextAreaValue : null;
            }
            
            if(setting.UiControlType == (int)BrandMetadataUiControlType.TextField)
            {
                var aa = MetadataTextFieldsList.FirstOrDefault(a => a.GroupNumber == groupNum);
                return aa != null ? aa.TextFieldValue : null;
            }

            return null;
        }

		/// <summary>
		/// Determines whether this instance is published and available by verifying that it is published, start date is before now
		/// and that the expiry date is after now.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is visible; otherwise, <c>false</c>.
		/// </returns>
		public bool IsPublishedAndAvailable()
		{
			return (AssetPublishStatus == AssetPublishStatus.Published && PublishDate < DateTime.Now && ExpiryDate > DateTime.Now);
		}

		/// <summary>
		/// Gets the production date.
		/// </summary>
		public string GetProductionDate()
		{
            if (!ProductionYear.HasValue) return "Undefined";

            string dayString = String.Empty;
            bool hasDay = false;
            int productionDay = ProductionDay.GetValueOrDefault();
            if (productionDay >= 1 && productionDay <= 31)
            {
                dayString = productionDay.ToString();
                hasDay = true;
            }
            
            string monthName = string.Empty;
            bool hasMonth = false;
			int productionMonth = ProductionMonth.GetValueOrDefault();
            if (productionMonth >= 1 && productionMonth <= 12)
            {
                monthName = Thread.CurrentThread.CurrentCulture.DateTimeFormat.MonthNames[productionMonth - 1];
                hasMonth = true;
            }

			string displayDate = string.Format("{0} {1} {2}", dayString, monthName, ProductionYear);
            
            // check if date is real
            DateTime foo;
            if (hasDay && hasMonth && !DateTime.TryParse(displayDate, out foo))
                displayDate = "Undefined";

			return displayDate.Trim();
		}

		/// <summary>
		/// Gets the orientation.
		/// </summary>
		/// <returns>Unknown, Portrait, Landscape, or Square</returns>
		public string GetOrientation()
		{
			int height = Height.GetValueOrDefault();
			int width = Width.GetValueOrDefault();

			if (height == 0 && width == 0)
				return "Unknown";

			if (height > width)
				return "Portrait";

			if (width > height)
				return "Landscape";

			if (width == height)
				return "Square";

			return "Unknown";
		}

		/// <summary>
		/// Gets friendly representation of asset dimensions. eg. 10 (h) x 5 (w)
		/// </summary>
		/// <returns></returns>
		public string GetDimensions()
		{
			if (!IsProcessed)
				return "being processed...";

			int height = Height.GetValueOrDefault();
			int width = Width.GetValueOrDefault();

			if (height == 0 && width == 0)
				return "unknown";

			return string.Format("{0} (h) x {1} (w)", height, width);
		}

		/// <summary>
		/// Gets the attached files.
		/// </summary>
		public List<AssetFile> GetAttachedFiles()
		{
			AssetFileFinder finder = new AssetFileFinder {AssetId = AssetId.GetValueOrDefault(), AssetFileType = AssetFileType.AttachedFile};
			return AssetFile.FindMany(finder);
		}

		/// <summary>
		/// Gets the file metadata.
		/// </summary>
		/// <param name="filter">Boolean value specifying whether potentially sensitive values should be filtered</param>
		public List<KeyValuePair<string,string>> GetFileMetadata(bool filter)
		{
			string[] excludeMetadataFields = new string[] {};

			if (filter)
				excludeMetadataFields = new[]
				                        	{
				                        		"ExifTool Version Number",
				                        		"Directory",
				                        		"Filename",
				                        		"File Permissions",
				                        		"Photoshop Thumbnail",
				                        		"Thumbnail Image"
				                        	};

			try
			{
				return (from item in XDocument.Parse(MetadataXml).XPathSelectElements("/Metadata/SimpleMetadata/Item")
				        let n1 = item.Element("Name")
				        let n2 = item.Element("Value")
				        let name = (n1 == null) ? string.Empty : n1.Value
				        let value = (n2 == null) ? string.Empty : n2.Value
				        where (!excludeMetadataFields.Contains(name))
				        select new KeyValuePair<string, string>(name, value)).ToList();
			}
			catch (Exception)
			{
				return new List<KeyValuePair<string, string>>();
			}
		}


		/// <summary>
		/// Saves the asset metadata.
		/// </summary>
		public static void SaveAssetMetadata(Asset asset)
		{
			if (asset.IsNull)
			{
				Debug.WriteLine("Skipped saving asset metadata, asset is null");
				return;
			}

			if (asset.IsNew)
			{
				Debug.WriteLine("Skipped saving asset metadata, asset is new");
				return;
			}

			if (asset.AssetMetadataLoaded())
			{
				AssetMetadataMapper.Instance.DeleteAllAssetMetadata(asset.AssetId.GetValueOrDefault());

				StringBuilder sb = new StringBuilder();

				foreach (var metadata in asset.MetadataList.Distinct())
				{
					if (metadata.IsNull || metadata.MetadataId == 0)
						continue;

					AssetMetadata am = AssetMetadata.New();
					am.AssetId = asset.AssetId.GetValueOrDefault();
					am.MetadataId = metadata.MetadataId.GetValueOrDefault();
					AssetMetadata.Update(am);

					sb.AppendFormat(" {0}", metadata.Name);

					if (!StringUtils.IsBlank(metadata.ExternalRef))
						sb.AppendFormat(", {0}", metadata.ExternalRef);

					if (!StringUtils.IsBlank(metadata.Synonyms))
						sb.AppendFormat(", {0}", metadata.Synonyms);
				}

                foreach (var area in asset.MetadataTextAreasList)
                {
                    if (area.IsNull || StringUtils.IsBlank(area.TextAreaValue))
                        continue;

                    area.AssetMetadataTextAreaId = null;
                    area.AssetId = asset.AssetId.GetValueOrDefault();
                    AssetMetadataTextArea.Update(area);

                    sb.AppendFormat(" {0}", area.TextAreaValue.Length > 600 ? area.TextAreaValue.Substring(0, 600) : area.TextAreaValue);
                }

                foreach (var field in asset.MetadataTextFieldsList)
                {
                    if (field.IsNull || StringUtils.IsBlank(field.TextFieldValue))
                        continue;

                    field.AssetMetadataTextFieldId = null;
                    field.AssetId = asset.AssetId.GetValueOrDefault();
                    AssetMetadataTextField.Update(field);

                    sb.AppendFormat(" {0}", field.TextFieldValue);
                }

			    asset.AssetMetadataVals = sb.ToString().Trim();
				Update(asset);
			}

			if (asset.AssetCategoriesLoaded())
			{
				AssetCategoryMapper.Instance.DeleteAssetCategories(asset.AssetId.GetValueOrDefault());

				StringBuilder sb = new StringBuilder();

				foreach (Category category in asset.CategoryList)
				{
					if (category.IsNull)
						continue;

					AssetCategory ac = AssetCategory.New();
					ac.AssetId = asset.AssetId.GetValueOrDefault();
					ac.CategoryId = category.CategoryId.GetValueOrDefault();
					ac.IsPrimary = (asset.PrimaryCategoryId == ac.CategoryId);
					AssetCategory.Update(ac);

					sb.Append(category.Name);

					//if (!StringUtils.IsBlank(category.Message))
					//	sb.AppendFormat(", {0}", category.Message);

					if (!StringUtils.IsBlank(category.ExternalRef))
						sb.AppendFormat(", {0}", category.ExternalRef);

					if (!StringUtils.IsBlank(category.Synonyms))
						sb.AppendFormat(", {0}", category.Synonyms);
				}

				asset.AssetCategories = sb.ToString();
				Update(asset);
			}

			if (asset.LinkedAssetsLoaded())
			{
				AssetLinkMapper.Instance.DeleteLinkedAssets(asset.AssetId.GetValueOrDefault());

				foreach (Asset linkedAsset in asset.LinkedAssetList)
				{
					if (linkedAsset.IsNull)
						continue;

					AssetLink assetLink = AssetLink.New();
					assetLink.AssetId = asset.AssetId.GetValueOrDefault();
					assetLink.LinkedAssetId = linkedAsset.AssetId.GetValueOrDefault();
					assetLink.LinkedAssetTitle = linkedAsset.Title; // Not really required, controlled by the view anyway
					AssetLink.Update(assetLink);
				}
			}
		}

		/// <summary>
		/// Adds or updates file contents.
		/// </summary>
		/// <param name="assetId">The asset id.</param>
		/// <param name="filename">The filename.</param>
		/// <param name="assetFileType">Type of the asset file.</param>
		/// <param name="byteArray">The byte array of file content.</param>
		/// <param name="forceAdd">if set to <c>true</c> file will be added.  Otherwise, file with specified type will be updated.</param>
		public static void AddUpdateFileContents(int assetId, string filename, AssetFileType assetFileType, ByteArray byteArray, bool forceAdd)
		{
			AssetFileMapper.Instance.AddUpdateFileContents(assetId, filename, assetFileType, byteArray, forceAdd);
		}

		/// <summary>
		/// Copies the attached files from the template asset to the child asset.
		/// </summary>
		public static void CopyAttachedFiles(int templateAssetId, int childAssetId)
		{
			AssetFileMapper.Instance.CopyAttachedFiles(templateAssetId, childAssetId);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the file reference.  This is the Asset ID padded to 7 characters with 0's.
		/// </summary>
		public string FileReference
		{
			get
			{
				return AssetId.ToString().PadLeft(7, '0');
			}
		}

		/// <summary>
		/// Gets or sets the asset publish status.
		/// </summary>
		public AssetPublishStatus AssetPublishStatus
		{
			get
			{
				return EnumUtils.GetEnumFromValue<AssetPublishStatus>(AssetPublishStatusId);
			}
			set
			{
				AssetPublishStatusId = Convert.ToInt32(value);
			}
		}

		#endregion

		#region Private Properties

		private IEnumerable<AssetMetadata> AssetMetadataList
		{
			get
			{
				if (m_AssetMetadataList == null)
				{
					AssetMetadataFinder finder = new AssetMetadataFinder { AssetId = AssetId.GetValueOrDefault() };
					m_AssetMetadataList = AssetMetadata.FindMany(finder);
				}

				return m_AssetMetadataList;
			}
		}

		private IEnumerable<AssetCategory> AssetCategoryList
		{
			get
			{
				if (m_AssetCategoryList == null)
				{
					AssetCategoryFinder finder = new AssetCategoryFinder { AssetId = AssetId.GetValueOrDefault() };
					m_AssetCategoryList = AssetCategory.FindMany(finder);
				}
				return m_AssetCategoryList;
			}
		}

		private List<AssetLink> AssetLinkList
		{
			get
			{
				if (m_AssetLinkList == null)
				{
					AssetLinkFinder finder = new AssetLinkFinder { AssetId = AssetId.GetValueOrDefault(-1) };
					m_AssetLinkList = AssetLink.FindMany(finder);
				}
				return m_AssetLinkList;
			}
		}

	    

	    #endregion

	    
	}
}