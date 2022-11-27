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

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class Asset : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetId = null;
		
		// Table variables
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
		protected bool m_restrictAttachedFiles = false;
		protected bool m_watermarkPreview = false;
		protected bool m_internalUsers_DownloadApprovalRequired = false;
		protected bool m_internalUsers_HideFromUsers = false;
		protected bool m_externalUsers_DownloadApprovalRequired = false;
		protected bool m_externalUsers_HideFromUsers = false;
		protected int m_assetFilePathId = 0;
		protected DateTime m_publishDate = DateTime.MinValue;
		protected DateTime m_expiryDate = DateTime.MinValue;
		protected bool m_isProcessed = false;
		protected int m_assetPublishStatusId = 0;
		protected int m_uploadedByUserId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		protected int m_downloadCount = 0;
		protected int m_popularityRank = 0;
		protected bool m_isDeleted = false;
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
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
					m_isDirty = true;
				}
			}
		}
		
		public Dictionary<String, ChangedProperty> ChangedProperties
		{
			get
			{
				return m_ChangedProperties;
			}
		}

		public override bool IsNew
		{
			get
			{
				return (m_assetId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetTypeId of the Asset object.
		/// </summary>
		public virtual int AssetTypeId
		{
			get
			{
				return m_assetTypeId;
			}
			set 
			{ 
				if (value != m_assetTypeId)
				{
					m_ChangedProperties["AssetTypeId"] = new ChangedProperty("AssetTypeId", m_assetTypeId, value);
					
					m_assetTypeId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UploadDate of the Asset object.
		/// </summary>
		public virtual DateTime UploadDate
		{
			get
			{
				return m_uploadDate;
			}
			set 
			{ 
				if (value != m_uploadDate)
				{
					m_ChangedProperties["UploadDate"] = new ChangedProperty("UploadDate", m_uploadDate, value);
					
					m_uploadDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Filename of the Asset object.
		/// </summary>
		public virtual string Filename
		{
			get
			{
				return m_filename;
			}
			set 
			{ 
				if (value != m_filename)
				{
					m_ChangedProperties["Filename"] = new ChangedProperty("Filename", m_filename, value);
					
					m_filename = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FileSize of the Asset object.
		/// </summary>
		public virtual long FileSize
		{
			get
			{
				return m_fileSize;
			}
			set 
			{ 
				if (value != m_fileSize)
				{
					m_ChangedProperties["FileSize"] = new ChangedProperty("FileSize", m_fileSize, value);
					
					m_fileSize = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FileHash of the Asset object.
		/// </summary>
		public virtual string FileHash
		{
			get
			{
				return m_fileHash;
			}
			set 
			{ 
				if (value != m_fileHash)
				{
					m_ChangedProperties["FileHash"] = new ChangedProperty("FileHash", m_fileHash, value);
					
					m_fileHash = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the TemplateAssetId of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> TemplateAssetId
		{
			get
			{
				return m_templateAssetId;
			}
			set 
			{ 
				if (value != m_templateAssetId)
				{
					m_ChangedProperties["TemplateAssetId"] = new ChangedProperty("TemplateAssetId", m_templateAssetId, value);
					
					m_templateAssetId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the WorkflowId of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> WorkflowId
		{
			get
			{
				return m_workflowId;
			}
			set 
			{ 
				if (value != m_workflowId)
				{
					m_ChangedProperties["WorkflowId"] = new ChangedProperty("WorkflowId", m_workflowId, value);
					
					m_workflowId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Title of the Asset object.
		/// </summary>
		public virtual string Title
		{
			get
			{
				return m_title;
			}
			set 
			{ 
				if (value != m_title)
				{
					m_ChangedProperties["Title"] = new ChangedProperty("Title", m_title, value);
					
					m_title = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ProjectCode of the Asset object.
		/// </summary>
		public virtual string ProjectCode
		{
			get
			{
				return m_projectCode;
			}
			set 
			{ 
				if (value != m_projectCode)
				{
					m_ChangedProperties["ProjectCode"] = new ChangedProperty("ProjectCode", m_projectCode, value);
					
					m_projectCode = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Originator of the Asset object.
		/// </summary>
		public virtual string Originator
		{
			get
			{
				return m_originator;
			}
			set 
			{ 
				if (value != m_originator)
				{
					m_ChangedProperties["Originator"] = new ChangedProperty("Originator", m_originator, value);
					
					m_originator = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Description of the Asset object.
		/// </summary>
		public virtual string Description
		{
			get
			{
				return m_description;
			}
			set 
			{ 
				if (value != m_description)
				{
					m_ChangedProperties["Description"] = new ChangedProperty("Description", m_description, value);
					
					m_description = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Keywords of the Asset object.
		/// </summary>
		public virtual string Keywords
		{
			get
			{
				return m_keywords;
			}
			set 
			{ 
				if (value != m_keywords)
				{
					m_ChangedProperties["Keywords"] = new ChangedProperty("Keywords", m_keywords, value);
					
					m_keywords = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ContactEmail of the Asset object.
		/// </summary>
		public virtual string ContactEmail
		{
			get
			{
				return m_contactEmail;
			}
			set 
			{ 
				if (value != m_contactEmail)
				{
					m_ChangedProperties["ContactEmail"] = new ChangedProperty("ContactEmail", m_contactEmail, value);
					
					m_contactEmail = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ProductionDay of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> ProductionDay
		{
			get
			{
				return m_productionDay;
			}
			set 
			{ 
				if (value != m_productionDay)
				{
					m_ChangedProperties["ProductionDay"] = new ChangedProperty("ProductionDay", m_productionDay, value);
					
					m_productionDay = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ProductionMonth of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> ProductionMonth
		{
			get
			{
				return m_productionMonth;
			}
			set 
			{ 
				if (value != m_productionMonth)
				{
					m_ChangedProperties["ProductionMonth"] = new ChangedProperty("ProductionMonth", m_productionMonth, value);
					
					m_productionMonth = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ProductionYear of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> ProductionYear
		{
			get
			{
				return m_productionYear;
			}
			set 
			{ 
				if (value != m_productionYear)
				{
					m_ChangedProperties["ProductionYear"] = new ChangedProperty("ProductionYear", m_productionYear, value);
					
					m_productionYear = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the BrandId of the Asset object.
		/// </summary>
		public virtual int BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{ 
				if (value != m_brandId)
				{
					m_ChangedProperties["BrandId"] = new ChangedProperty("BrandId", m_brandId, value);
					
					m_brandId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CopyrightOwner of the Asset object.
		/// </summary>
		public virtual string CopyrightOwner
		{
			get
			{
				return m_copyrightOwner;
			}
			set 
			{ 
				if (value != m_copyrightOwner)
				{
					m_ChangedProperties["CopyrightOwner"] = new ChangedProperty("CopyrightOwner", m_copyrightOwner, value);
					
					m_copyrightOwner = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UsageRestrictions of the Asset object.
		/// </summary>
		public virtual string UsageRestrictions
		{
			get
			{
				return m_usageRestrictions;
			}
			set 
			{ 
				if (value != m_usageRestrictions)
				{
					m_ChangedProperties["UsageRestrictions"] = new ChangedProperty("UsageRestrictions", m_usageRestrictions, value);
					
					m_usageRestrictions = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the RestrictAttachedFiles of the Asset object.
		/// </summary>
		public virtual bool RestrictAttachedFiles
		{
			get
			{
				return m_restrictAttachedFiles;
			}
			set 
			{ 
				if (value != m_restrictAttachedFiles)
				{
					m_ChangedProperties["RestrictAttachedFiles"] = new ChangedProperty("RestrictAttachedFiles", m_restrictAttachedFiles, value);
					
					m_restrictAttachedFiles = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the WatermarkPreview of the Asset object.
		/// </summary>
		public virtual bool WatermarkPreview
		{
			get
			{
				return m_watermarkPreview;
			}
			set 
			{ 
				if (value != m_watermarkPreview)
				{
					m_ChangedProperties["WatermarkPreview"] = new ChangedProperty("WatermarkPreview", m_watermarkPreview, value);
					
					m_watermarkPreview = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the InternalUsers_DownloadApprovalRequired of the Asset object.
		/// </summary>
		public virtual bool InternalUsers_DownloadApprovalRequired
		{
			get
			{
				return m_internalUsers_DownloadApprovalRequired;
			}
			set 
			{ 
				if (value != m_internalUsers_DownloadApprovalRequired)
				{
					m_ChangedProperties["InternalUsers_DownloadApprovalRequired"] = new ChangedProperty("InternalUsers_DownloadApprovalRequired", m_internalUsers_DownloadApprovalRequired, value);
					
					m_internalUsers_DownloadApprovalRequired = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the InternalUsers_HideFromUsers of the Asset object.
		/// </summary>
		public virtual bool InternalUsers_HideFromUsers
		{
			get
			{
				return m_internalUsers_HideFromUsers;
			}
			set 
			{ 
				if (value != m_internalUsers_HideFromUsers)
				{
					m_ChangedProperties["InternalUsers_HideFromUsers"] = new ChangedProperty("InternalUsers_HideFromUsers", m_internalUsers_HideFromUsers, value);
					
					m_internalUsers_HideFromUsers = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ExternalUsers_DownloadApprovalRequired of the Asset object.
		/// </summary>
		public virtual bool ExternalUsers_DownloadApprovalRequired
		{
			get
			{
				return m_externalUsers_DownloadApprovalRequired;
			}
			set 
			{ 
				if (value != m_externalUsers_DownloadApprovalRequired)
				{
					m_ChangedProperties["ExternalUsers_DownloadApprovalRequired"] = new ChangedProperty("ExternalUsers_DownloadApprovalRequired", m_externalUsers_DownloadApprovalRequired, value);
					
					m_externalUsers_DownloadApprovalRequired = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ExternalUsers_HideFromUsers of the Asset object.
		/// </summary>
		public virtual bool ExternalUsers_HideFromUsers
		{
			get
			{
				return m_externalUsers_HideFromUsers;
			}
			set 
			{ 
				if (value != m_externalUsers_HideFromUsers)
				{
					m_ChangedProperties["ExternalUsers_HideFromUsers"] = new ChangedProperty("ExternalUsers_HideFromUsers", m_externalUsers_HideFromUsers, value);
					
					m_externalUsers_HideFromUsers = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetFilePathId of the Asset object.
		/// </summary>
		public virtual int AssetFilePathId
		{
			get
			{
				return m_assetFilePathId;
			}
			set 
			{ 
				if (value != m_assetFilePathId)
				{
					m_ChangedProperties["AssetFilePathId"] = new ChangedProperty("AssetFilePathId", m_assetFilePathId, value);
					
					m_assetFilePathId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PublishDate of the Asset object.
		/// </summary>
		public virtual DateTime PublishDate
		{
			get
			{
				return m_publishDate;
			}
			set 
			{ 
				if (value != m_publishDate)
				{
					m_ChangedProperties["PublishDate"] = new ChangedProperty("PublishDate", m_publishDate, value);
					
					m_publishDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ExpiryDate of the Asset object.
		/// </summary>
		public virtual DateTime ExpiryDate
		{
			get
			{
				return m_expiryDate;
			}
			set 
			{ 
				if (value != m_expiryDate)
				{
					m_ChangedProperties["ExpiryDate"] = new ChangedProperty("ExpiryDate", m_expiryDate, value);
					
					m_expiryDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsProcessed of the Asset object.
		/// </summary>
		public virtual bool IsProcessed
		{
			get
			{
				return m_isProcessed;
			}
			set 
			{ 
				if (value != m_isProcessed)
				{
					m_ChangedProperties["IsProcessed"] = new ChangedProperty("IsProcessed", m_isProcessed, value);
					
					m_isProcessed = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetPublishStatusId of the Asset object.
		/// </summary>
		public virtual int AssetPublishStatusId
		{
			get
			{
				return m_assetPublishStatusId;
			}
			set 
			{ 
				if (value != m_assetPublishStatusId)
				{
					m_ChangedProperties["AssetPublishStatusId"] = new ChangedProperty("AssetPublishStatusId", m_assetPublishStatusId, value);
					
					m_assetPublishStatusId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UploadedByUserId of the Asset object.
		/// </summary>
		public virtual int UploadedByUserId
		{
			get
			{
				return m_uploadedByUserId;
			}
			set 
			{ 
				if (value != m_uploadedByUserId)
				{
					m_ChangedProperties["UploadedByUserId"] = new ChangedProperty("UploadedByUserId", m_uploadedByUserId, value);
					
					m_uploadedByUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreateDate of the Asset object.
		/// </summary>
		public virtual DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set 
			{ 
				if (value != m_createDate)
				{
					m_ChangedProperties["CreateDate"] = new ChangedProperty("CreateDate", m_createDate, value);
					
					m_createDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastUpdate of the Asset object.
		/// </summary>
		public virtual DateTime LastUpdate
		{
			get
			{
				return m_lastUpdate;
			}
			set 
			{ 
				if (value != m_lastUpdate)
				{
					m_ChangedProperties["LastUpdate"] = new ChangedProperty("LastUpdate", m_lastUpdate, value);
					
					m_lastUpdate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DownloadCount of the Asset object.
		/// </summary>
		public virtual int DownloadCount
		{
			get
			{
				return m_downloadCount;
			}
			set 
			{ 
				if (value != m_downloadCount)
				{
					m_ChangedProperties["DownloadCount"] = new ChangedProperty("DownloadCount", m_downloadCount, value);
					
					m_downloadCount = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PopularityRank of the Asset object.
		/// </summary>
		public virtual int PopularityRank
		{
			get
			{
				return m_popularityRank;
			}
			set 
			{ 
				if (value != m_popularityRank)
				{
					m_ChangedProperties["PopularityRank"] = new ChangedProperty("PopularityRank", m_popularityRank, value);
					
					m_popularityRank = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDeleted of the Asset object.
		/// </summary>
		public virtual bool IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set 
			{ 
				if (value != m_isDeleted)
				{
					m_ChangedProperties["IsDeleted"] = new ChangedProperty("IsDeleted", m_isDeleted, value);
					
					m_isDeleted = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetCategories of the Asset object.
		/// </summary>
		public virtual string AssetCategories
		{
			get
			{
				return m_assetCategories;
			}
			set 
			{ 
				if (value != m_assetCategories)
				{
					m_ChangedProperties["AssetCategories"] = new ChangedProperty("AssetCategories", m_assetCategories, value);
					
					m_assetCategories = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetMetadataVals of the Asset object.
		/// </summary>
		public virtual string AssetMetadataVals
		{
			get
			{
				return m_assetMetadataVals;
			}
			set 
			{ 
				if (value != m_assetMetadataVals)
				{
					m_ChangedProperties["AssetMetadataVals"] = new ChangedProperty("AssetMetadataVals", m_assetMetadataVals, value);
					
					m_assetMetadataVals = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the MetadataXml of the Asset object.
		/// </summary>
		public virtual string MetadataXml
		{
			get
			{
				return m_metadataXml;
			}
			set 
			{ 
				if (value != m_metadataXml)
				{
					m_ChangedProperties["MetadataXml"] = new ChangedProperty("MetadataXml", m_metadataXml, value);
					
					m_metadataXml = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Height of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> Height
		{
			get
			{
				return m_height;
			}
			set 
			{ 
				if (value != m_height)
				{
					m_ChangedProperties["Height"] = new ChangedProperty("Height", m_height, value);
					
					m_height = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Width of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> Width
		{
			get
			{
				return m_width;
			}
			set 
			{ 
				if (value != m_width)
				{
					m_ChangedProperties["Width"] = new ChangedProperty("Width", m_width, value);
					
					m_width = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Duration of the Asset object.
		/// </summary>
		public virtual Nullable <Int32> Duration
		{
			get
			{
				return m_duration;
			}
			set 
			{ 
				if (value != m_duration)
				{
					m_ChangedProperties["Duration"] = new ChangedProperty("Duration", m_duration, value);
					
					m_duration = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the MetadataSearchVals of the Asset object.
		/// </summary>
		public virtual string MetadataSearchVals
		{
			get
			{
				return m_metadataSearchVals;
			}
			set 
			{ 
				if (value != m_metadataSearchVals)
				{
					m_ChangedProperties["MetadataSearchVals"] = new ChangedProperty("MetadataSearchVals", m_metadataSearchVals, value);
					
					m_metadataSearchVals = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Plugin of the Asset object.
		/// </summary>
		public virtual Guid Plugin
		{
			get
			{
				return m_plugin;
			}
			set 
			{ 
				if (value != m_plugin)
				{
					m_ChangedProperties["Plugin"] = new ChangedProperty("Plugin", m_plugin, value);
					
					m_plugin = value; 
					m_isDirty = true;
				}
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
		
		public enum Columns
		{
			AssetId,
			AssetTypeId,
			UploadDate,
			Filename,
			FileSize,
			FileHash,
			TemplateAssetId,
			WorkflowId,
			Title,
			ProjectCode,
			Originator,
			Description,
			Keywords,
			ContactEmail,
			ProductionDay,
			ProductionMonth,
			ProductionYear,
			BrandId,
			CopyrightOwner,
			UsageRestrictions,
			RestrictAttachedFiles,
			WatermarkPreview,
			InternalUsers_DownloadApprovalRequired,
			InternalUsers_HideFromUsers,
			ExternalUsers_DownloadApprovalRequired,
			ExternalUsers_HideFromUsers,
			AssetFilePathId,
			PublishDate,
			ExpiryDate,
			IsProcessed,
			AssetPublishStatusId,
			UploadedByUserId,
			CreateDate,
			LastUpdate,
			DownloadCount,
			PopularityRank,
			IsDeleted,
			AssetCategories,
			AssetMetadataVals,
			MetadataXml,
			Height,
			Width,
			Duration,
			MetadataSearchVals,
			Plugin
		}
	}
}

