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
using Daydream.Data;
using FocusOPEN.Shared;
using SearchBuilder=Daydream.Data.SearchBuilder;

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class AssetFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetId = null;
		
		// Table columns
		protected int m_assetTypeId = 0;
		protected DateTime m_uploadDate = DateTime.MinValue;
		protected string m_filename = String.Empty;
		protected long m_fileSize = 0;
		protected string m_fileHash = String.Empty;
		protected Nullable <Int32> m_templateAssetId = null;
		protected Nullable <Int32> m_workflowId = null;
		protected string m_title = String.Empty;
		protected string m_projectCode = String.Empty;
		protected string m_originator = String.Empty;
		protected string m_description = String.Empty;
		protected string m_keywords = String.Empty;
		protected string m_contactEmail = String.Empty;
		protected Nullable <Int32> m_productionDay = null;
		protected Nullable <Int32> m_productionMonth = null;
		protected Nullable <Int32> m_productionYear = null;
		protected int m_brandId = 0;
		protected string m_copyrightOwner = String.Empty;
		protected string m_usageRestrictions = String.Empty;
		protected Nullable <Boolean> m_restrictAttachedFiles = null;
		protected Nullable <Boolean> m_watermarkPreview = null;
		protected Nullable <Boolean> m_internalUsers_DownloadApprovalRequired = null;
		protected Nullable <Boolean> m_internalUsers_HideFromUsers = null;
		protected Nullable <Boolean> m_externalUsers_DownloadApprovalRequired = null;
		protected Nullable <Boolean> m_externalUsers_HideFromUsers = null;
		protected int m_assetFilePathId = 0;
		protected DateTime m_publishDate = DateTime.MinValue;
		protected DateTime m_expiryDate = DateTime.MinValue;
		protected Nullable <Boolean> m_isProcessed = null;
		protected int m_assetPublishStatusId = 0;
		protected int m_uploadedByUserId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		protected int m_downloadCount = 0;
		protected int m_popularityRank = 0;
		protected Nullable <Boolean> m_isDeleted = null;
		protected string m_assetCategories = String.Empty;
		protected string m_assetMetadataVals = String.Empty;
		protected string m_metadataXml = String.Empty;
		protected Nullable <Int32> m_height = null;
		protected Nullable <Int32> m_width = null;
		protected Nullable <Int32> m_duration = null;
		protected string m_metadataSearchVals = String.Empty;
		protected Guid m_plugin = Guid.Empty;
		
		// View Variables
		protected string m_assetTypeName = String.Empty;
		protected string m_fileExtension = String.Empty;
		protected Nullable <DateTime> m_shadowDate = null;
		protected string m_assetPublishStatusName = String.Empty;
		protected string m_uploadedByUserName = String.Empty;
		protected string m_brandName = String.Empty;
		protected string m_searchableData = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Asset object.
		/// </summary>
		public Nullable <Int32> AssetId
		{
			get
			{
				return m_assetId;
			}
			set
			{
				if (value != m_assetId)
				{
					m_assetId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AssetTypeId
		{
			get
			{
				return m_assetTypeId;
			}
			set
			{
				if (value != m_assetTypeId)
				{
					m_assetTypeId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime UploadDate
		{
			get
			{
				return m_uploadDate;
			}
			set
			{
				if (value != m_uploadDate)
				{
					m_uploadDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Filename
		{
			get
			{
				return m_filename;
			}
			set
			{
				if (value != m_filename)
				{
					m_filename = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public long FileSize
		{
			get
			{
				return m_fileSize;
			}
			set
			{
				if (value != m_fileSize)
				{
					m_fileSize = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string FileHash
		{
			get
			{
				return m_fileHash;
			}
			set
			{
				if (value != m_fileHash)
				{
					m_fileHash = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> TemplateAssetId
		{
			get
			{
				return m_templateAssetId;
			}
			set
			{
				if (value != m_templateAssetId)
				{
					m_templateAssetId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> WorkflowId
		{
			get
			{
				return m_workflowId;
			}
			set
			{
				if (value != m_workflowId)
				{
					m_workflowId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Title
		{
			get
			{
				return m_title;
			}
			set
			{
				if (value != m_title)
				{
					m_title = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string ProjectCode
		{
			get
			{
				return m_projectCode;
			}
			set
			{
				if (value != m_projectCode)
				{
					m_projectCode = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Originator
		{
			get
			{
				return m_originator;
			}
			set
			{
				if (value != m_originator)
				{
					m_originator = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Description
		{
			get
			{
				return m_description;
			}
			set
			{
				if (value != m_description)
				{
					m_description = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Keywords
		{
			get
			{
				return m_keywords;
			}
			set
			{
				if (value != m_keywords)
				{
					m_keywords = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string ContactEmail
		{
			get
			{
				return m_contactEmail;
			}
			set
			{
				if (value != m_contactEmail)
				{
					m_contactEmail = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> ProductionDay
		{
			get
			{
				return m_productionDay;
			}
			set
			{
				if (value != m_productionDay)
				{
					m_productionDay = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> ProductionMonth
		{
			get
			{
				return m_productionMonth;
			}
			set
			{
				if (value != m_productionMonth)
				{
					m_productionMonth = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> ProductionYear
		{
			get
			{
				return m_productionYear;
			}
			set
			{
				if (value != m_productionYear)
				{
					m_productionYear = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int BrandId
		{
			get
			{
				return m_brandId;
			}
			set
			{
				if (value != m_brandId)
				{
					m_brandId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string CopyrightOwner
		{
			get
			{
				return m_copyrightOwner;
			}
			set
			{
				if (value != m_copyrightOwner)
				{
					m_copyrightOwner = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string UsageRestrictions
		{
			get
			{
				return m_usageRestrictions;
			}
			set
			{
				if (value != m_usageRestrictions)
				{
					m_usageRestrictions = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> RestrictAttachedFiles
		{
			get
			{
				return m_restrictAttachedFiles;
			}
			set
			{
				if (value != m_restrictAttachedFiles)
				{
					m_restrictAttachedFiles = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> WatermarkPreview
		{
			get
			{
				return m_watermarkPreview;
			}
			set
			{
				if (value != m_watermarkPreview)
				{
					m_watermarkPreview = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> InternalUsers_DownloadApprovalRequired
		{
			get
			{
				return m_internalUsers_DownloadApprovalRequired;
			}
			set
			{
				if (value != m_internalUsers_DownloadApprovalRequired)
				{
					m_internalUsers_DownloadApprovalRequired = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> InternalUsers_HideFromUsers
		{
			get
			{
				return m_internalUsers_HideFromUsers;
			}
			set
			{
				if (value != m_internalUsers_HideFromUsers)
				{
					m_internalUsers_HideFromUsers = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> ExternalUsers_DownloadApprovalRequired
		{
			get
			{
				return m_externalUsers_DownloadApprovalRequired;
			}
			set
			{
				if (value != m_externalUsers_DownloadApprovalRequired)
				{
					m_externalUsers_DownloadApprovalRequired = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> ExternalUsers_HideFromUsers
		{
			get
			{
				return m_externalUsers_HideFromUsers;
			}
			set
			{
				if (value != m_externalUsers_HideFromUsers)
				{
					m_externalUsers_HideFromUsers = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AssetFilePathId
		{
			get
			{
				return m_assetFilePathId;
			}
			set
			{
				if (value != m_assetFilePathId)
				{
					m_assetFilePathId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime PublishDate
		{
			get
			{
				return m_publishDate;
			}
			set
			{
				if (value != m_publishDate)
				{
					m_publishDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime ExpiryDate
		{
			get
			{
				return m_expiryDate;
			}
			set
			{
				if (value != m_expiryDate)
				{
					m_expiryDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsProcessed
		{
			get
			{
				return m_isProcessed;
			}
			set
			{
				if (value != m_isProcessed)
				{
					m_isProcessed = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AssetPublishStatusId
		{
			get
			{
				return m_assetPublishStatusId;
			}
			set
			{
				if (value != m_assetPublishStatusId)
				{
					m_assetPublishStatusId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int UploadedByUserId
		{
			get
			{
				return m_uploadedByUserId;
			}
			set
			{
				if (value != m_uploadedByUserId)
				{
					m_uploadedByUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set
			{
				if (value != m_createDate)
				{
					m_createDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime LastUpdate
		{
			get
			{
				return m_lastUpdate;
			}
			set
			{
				if (value != m_lastUpdate)
				{
					m_lastUpdate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int DownloadCount
		{
			get
			{
				return m_downloadCount;
			}
			set
			{
				if (value != m_downloadCount)
				{
					m_downloadCount = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int PopularityRank
		{
			get
			{
				return m_popularityRank;
			}
			set
			{
				if (value != m_popularityRank)
				{
					m_popularityRank = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set
			{
				if (value != m_isDeleted)
				{
					m_isDeleted = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string AssetCategories
		{
			get
			{
				return m_assetCategories;
			}
			set
			{
				if (value != m_assetCategories)
				{
					m_assetCategories = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string AssetMetadataVals
		{
			get
			{
				return m_assetMetadataVals;
			}
			set
			{
				if (value != m_assetMetadataVals)
				{
					m_assetMetadataVals = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string MetadataXml
		{
			get
			{
				return m_metadataXml;
			}
			set
			{
				if (value != m_metadataXml)
				{
					m_metadataXml = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> Height
		{
			get
			{
				return m_height;
			}
			set
			{
				if (value != m_height)
				{
					m_height = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> Width
		{
			get
			{
				return m_width;
			}
			set
			{
				if (value != m_width)
				{
					m_width = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> Duration
		{
			get
			{
				return m_duration;
			}
			set
			{
				if (value != m_duration)
				{
					m_duration = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string MetadataSearchVals
		{
			get
			{
				return m_metadataSearchVals;
			}
			set
			{
				if (value != m_metadataSearchVals)
				{
					m_metadataSearchVals = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Guid Plugin
		{
			get
			{
				return m_plugin;
			}
			set
			{
				if (value != m_plugin)
				{
					m_plugin = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetIdList
		{
			get
			{
				return m_assetIdList;
			}
		}
		
		public int FindCriteriaCount
		{
			get
			{
				return m_FindCriteriaCount;
			}
		}
		
		#endregion
		
		#region View Accessors
		
		public virtual string AssetTypeName
		{
			get
			{
				return m_assetTypeName;
			}
			set 
			{ 
				m_assetTypeName = value; 
			}
		}
		public virtual string FileExtension
		{
			get
			{
				return m_fileExtension;
			}
			set 
			{ 
				m_fileExtension = value; 
			}
		}
		public virtual Nullable <DateTime> ShadowDate
		{
			get
			{
				return m_shadowDate;
			}
			set 
			{ 
				m_shadowDate = value; 
			}
		}
		public virtual string AssetPublishStatusName
		{
			get
			{
				return m_assetPublishStatusName;
			}
			set 
			{ 
				m_assetPublishStatusName = value; 
			}
		}
		public virtual string UploadedByUserName
		{
			get
			{
				return m_uploadedByUserName;
			}
			set 
			{ 
				m_uploadedByUserName = value; 
			}
		}
		public virtual string BrandName
		{
			get
			{
				return m_brandName;
			}
			set 
			{ 
				m_brandName = value; 
			}
		}
		public virtual string SearchableData
		{
			get
			{
				return m_searchableData;
			}
			set 
			{ 
				m_searchableData = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_Asset]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetIdList != null && AssetIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Asset.Columns.AssetId));
			}
			
			if (AssetId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", Asset.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId.Value);
			}
			
			if (AssetTypeId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetTypeId", Asset.Columns.AssetTypeId));
				sb.AddDataParameter("@assetTypeId", AssetTypeId);
			}
	
			if (UploadDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@uploadDate", Asset.Columns.UploadDate));
				sb.AddDataParameter("@uploadDate", UploadDate);
			}
	
			if (Filename != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@filename", Asset.Columns.Filename));
				sb.AddDataParameter("@filename", Filename);
			}						
	
			if (FileSize != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@fileSize", Asset.Columns.FileSize));
				sb.AddDataParameter("@fileSize", FileSize);
			}						
	
			if (FileHash != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@fileHash", Asset.Columns.FileHash));
				sb.AddDataParameter("@fileHash", FileHash);
			}						
	
			if (TemplateAssetId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@templateAssetId", Asset.Columns.TemplateAssetId));
				sb.AddDataParameter("@templateAssetId", TemplateAssetId.Value);
			}
	
			if (WorkflowId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@workflowId", Asset.Columns.WorkflowId));
				sb.AddDataParameter("@workflowId", WorkflowId.Value);
			}
	
			if (Title != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@title", Asset.Columns.Title));
				sb.AddDataParameter("@title", Title);
			}						
	
			if (ProjectCode != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@projectCode", Asset.Columns.ProjectCode));
				sb.AddDataParameter("@projectCode", ProjectCode);
			}						
	
			if (Originator != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@originator", Asset.Columns.Originator));
				sb.AddDataParameter("@originator", Originator);
			}						
	
			if (Description != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@description", Asset.Columns.Description));
				sb.AddDataParameter("@description", Description);
			}						
	
			if (Keywords != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@keywords", Asset.Columns.Keywords));
				sb.AddDataParameter("@keywords", Keywords);
			}						
	
			if (ContactEmail != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@contactEmail", Asset.Columns.ContactEmail));
				sb.AddDataParameter("@contactEmail", ContactEmail);
			}						
	
			if (ProductionDay != null)
			{
				sb.Criteria.Add(string.Format("{0}=@productionDay", Asset.Columns.ProductionDay));
				sb.AddDataParameter("@productionDay", ProductionDay.Value);
			}
	
			if (ProductionMonth != null)
			{
				sb.Criteria.Add(string.Format("{0}=@productionMonth", Asset.Columns.ProductionMonth));
				sb.AddDataParameter("@productionMonth", ProductionMonth.Value);
			}
	
			if (ProductionYear != null)
			{
				sb.Criteria.Add(string.Format("{0}=@productionYear", Asset.Columns.ProductionYear));
				sb.AddDataParameter("@productionYear", ProductionYear.Value);
			}
	
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", Asset.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			if (CopyrightOwner != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@copyrightOwner", Asset.Columns.CopyrightOwner));
				sb.AddDataParameter("@copyrightOwner", CopyrightOwner);
			}						
	
			if (UsageRestrictions != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@usageRestrictions", Asset.Columns.UsageRestrictions));
				sb.AddDataParameter("@usageRestrictions", UsageRestrictions);
			}						
	
			if (RestrictAttachedFiles.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@restrictAttachedFiles", Asset.Columns.RestrictAttachedFiles));
				sb.AddDataParameter("@restrictAttachedFiles", SqlUtils.BitValue(RestrictAttachedFiles.Value));
			}
	
			if (WatermarkPreview.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@watermarkPreview", Asset.Columns.WatermarkPreview));
				sb.AddDataParameter("@watermarkPreview", SqlUtils.BitValue(WatermarkPreview.Value));
			}
	
			if (InternalUsers_DownloadApprovalRequired.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@internalUsers_DownloadApprovalRequired", Asset.Columns.InternalUsers_DownloadApprovalRequired));
				sb.AddDataParameter("@internalUsers_DownloadApprovalRequired", SqlUtils.BitValue(InternalUsers_DownloadApprovalRequired.Value));
			}
	
			if (InternalUsers_HideFromUsers.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@internalUsers_HideFromUsers", Asset.Columns.InternalUsers_HideFromUsers));
				sb.AddDataParameter("@internalUsers_HideFromUsers", SqlUtils.BitValue(InternalUsers_HideFromUsers.Value));
			}
	
			if (ExternalUsers_DownloadApprovalRequired.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@externalUsers_DownloadApprovalRequired", Asset.Columns.ExternalUsers_DownloadApprovalRequired));
				sb.AddDataParameter("@externalUsers_DownloadApprovalRequired", SqlUtils.BitValue(ExternalUsers_DownloadApprovalRequired.Value));
			}
	
			if (ExternalUsers_HideFromUsers.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@externalUsers_HideFromUsers", Asset.Columns.ExternalUsers_HideFromUsers));
				sb.AddDataParameter("@externalUsers_HideFromUsers", SqlUtils.BitValue(ExternalUsers_HideFromUsers.Value));
			}
	
			if (AssetFilePathId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetFilePathId", Asset.Columns.AssetFilePathId));
				sb.AddDataParameter("@assetFilePathId", AssetFilePathId);
			}
	
			if (PublishDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@publishDate", Asset.Columns.PublishDate));
				sb.AddDataParameter("@publishDate", PublishDate);
			}
	
			if (ExpiryDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@expiryDate", Asset.Columns.ExpiryDate));
				sb.AddDataParameter("@expiryDate", ExpiryDate);
			}
	
			if (IsProcessed.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isProcessed", Asset.Columns.IsProcessed));
				sb.AddDataParameter("@isProcessed", SqlUtils.BitValue(IsProcessed.Value));
			}
	
			if (AssetPublishStatusId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetPublishStatusId", Asset.Columns.AssetPublishStatusId));
				sb.AddDataParameter("@assetPublishStatusId", AssetPublishStatusId);
			}
	
			if (UploadedByUserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@uploadedByUserId", Asset.Columns.UploadedByUserId));
				sb.AddDataParameter("@uploadedByUserId", UploadedByUserId);
			}
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", Asset.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			if (LastUpdate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lastUpdate", Asset.Columns.LastUpdate));
				sb.AddDataParameter("@lastUpdate", LastUpdate);
			}
	
			if (DownloadCount != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@downloadCount", Asset.Columns.DownloadCount));
				sb.AddDataParameter("@downloadCount", DownloadCount);
			}
	
			if (PopularityRank != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@popularityRank", Asset.Columns.PopularityRank));
				sb.AddDataParameter("@popularityRank", PopularityRank);
			}
	
			if (IsDeleted.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDeleted", Asset.Columns.IsDeleted));
				sb.AddDataParameter("@isDeleted", SqlUtils.BitValue(IsDeleted.Value));
			}
	
			if (AssetCategories != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@assetCategories", Asset.Columns.AssetCategories));
				sb.AddDataParameter("@assetCategories", AssetCategories);
			}						
	
			if (AssetMetadataVals != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@assetMetadataVals", Asset.Columns.AssetMetadataVals));
				sb.AddDataParameter("@assetMetadataVals", AssetMetadataVals);
			}						
	
			if (MetadataXml != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@metadataXml", Asset.Columns.MetadataXml));
				sb.AddDataParameter("@metadataXml", MetadataXml);
			}						
	
			if (Height != null)
			{
				sb.Criteria.Add(string.Format("{0}=@height", Asset.Columns.Height));
				sb.AddDataParameter("@height", Height.Value);
			}
	
			if (Width != null)
			{
				sb.Criteria.Add(string.Format("{0}=@width", Asset.Columns.Width));
				sb.AddDataParameter("@width", Width.Value);
			}
	
			if (Duration != null)
			{
				sb.Criteria.Add(string.Format("{0}=@duration", Asset.Columns.Duration));
				sb.AddDataParameter("@duration", Duration.Value);
			}
	
			if (MetadataSearchVals != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@metadataSearchVals", Asset.Columns.MetadataSearchVals));
				sb.AddDataParameter("@metadataSearchVals", MetadataSearchVals);
			}						
	
			if (Plugin != Guid.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@plugin", Asset.Columns.Plugin));
				sb.AddDataParameter("@plugin", Plugin);
			}						
	
			if (AssetTypeName != String.Empty)
			{
				sb.Criteria.Add("AssetTypeName=@assetTypeName");
				sb.AddDataParameter("@assetTypeName", AssetTypeName);
			}						

			if (FileExtension != String.Empty)
			{
				sb.Criteria.Add("FileExtension=@fileExtension");
				sb.AddDataParameter("@fileExtension", FileExtension);
			}						

			if (ShadowDate != null)
			{
				sb.Criteria.Add("ShadowDate=@shadowDate");
				sb.AddDataParameter("@shadowDate", ShadowDate.Value);
			}

			if (AssetPublishStatusName != String.Empty)
			{
				sb.Criteria.Add("AssetPublishStatusName=@assetPublishStatusName");
				sb.AddDataParameter("@assetPublishStatusName", AssetPublishStatusName);
			}						

			if (UploadedByUserName != String.Empty)
			{
				sb.Criteria.Add("UploadedByUserName=@uploadedByUserName");
				sb.AddDataParameter("@uploadedByUserName", UploadedByUserName);
			}						

			if (BrandName != String.Empty)
			{
				sb.Criteria.Add("BrandName=@brandName");
				sb.AddDataParameter("@brandName", BrandName);
			}						

			if (SearchableData != String.Empty)
			{
				sb.Criteria.Add("SearchableData=@searchableData");
				sb.AddDataParameter("@searchableData", SearchableData);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}