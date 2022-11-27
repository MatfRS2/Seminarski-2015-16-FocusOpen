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

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullAsset : Asset
	{
		#region Singleton implementation

		private NullAsset()
		{
		}

		private static readonly NullAsset m_instance = new NullAsset();

		public static NullAsset Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the AssetTypeId of the Asset object.
		/// </summary>
		public override int AssetTypeId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UploadDate of the Asset object.
		/// </summary>
		public override DateTime UploadDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the Filename of the Asset object.
		/// </summary>
		public override string Filename
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the FileSize of the Asset object.
		/// </summary>
		public override long FileSize
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the FileHash of the Asset object.
		/// </summary>
		public override string FileHash
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the TemplateAssetId of the Asset object.
		/// </summary>
		public override Nullable <Int32> TemplateAssetId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the WorkflowId of the Asset object.
		/// </summary>
		public override Nullable <Int32> WorkflowId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the Title of the Asset object.
		/// </summary>
		public override string Title
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ProjectCode of the Asset object.
		/// </summary>
		public override string ProjectCode
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Originator of the Asset object.
		/// </summary>
		public override string Originator
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Description of the Asset object.
		/// </summary>
		public override string Description
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Keywords of the Asset object.
		/// </summary>
		public override string Keywords
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ContactEmail of the Asset object.
		/// </summary>
		public override string ContactEmail
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ProductionDay of the Asset object.
		/// </summary>
		public override Nullable <Int32> ProductionDay
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the ProductionMonth of the Asset object.
		/// </summary>
		public override Nullable <Int32> ProductionMonth
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the ProductionYear of the Asset object.
		/// </summary>
		public override Nullable <Int32> ProductionYear
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the BrandId of the Asset object.
		/// </summary>
		public override int BrandId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CopyrightOwner of the Asset object.
		/// </summary>
		public override string CopyrightOwner
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the UsageRestrictions of the Asset object.
		/// </summary>
		public override string UsageRestrictions
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the RestrictAttachedFiles of the Asset object.
		/// </summary>
		public override bool RestrictAttachedFiles
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the WatermarkPreview of the Asset object.
		/// </summary>
		public override bool WatermarkPreview
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the InternalUsers_DownloadApprovalRequired of the Asset object.
		/// </summary>
		public override bool InternalUsers_DownloadApprovalRequired
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the InternalUsers_HideFromUsers of the Asset object.
		/// </summary>
		public override bool InternalUsers_HideFromUsers
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the ExternalUsers_DownloadApprovalRequired of the Asset object.
		/// </summary>
		public override bool ExternalUsers_DownloadApprovalRequired
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the ExternalUsers_HideFromUsers of the Asset object.
		/// </summary>
		public override bool ExternalUsers_HideFromUsers
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetFilePathId of the Asset object.
		/// </summary>
		public override int AssetFilePathId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the PublishDate of the Asset object.
		/// </summary>
		public override DateTime PublishDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the ExpiryDate of the Asset object.
		/// </summary>
		public override DateTime ExpiryDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsProcessed of the Asset object.
		/// </summary>
		public override bool IsProcessed
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetPublishStatusId of the Asset object.
		/// </summary>
		public override int AssetPublishStatusId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UploadedByUserId of the Asset object.
		/// </summary>
		public override int UploadedByUserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreateDate of the Asset object.
		/// </summary>
		public override DateTime CreateDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastUpdate of the Asset object.
		/// </summary>
		public override DateTime LastUpdate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the DownloadCount of the Asset object.
		/// </summary>
		public override int DownloadCount
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the PopularityRank of the Asset object.
		/// </summary>
		public override int PopularityRank
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDeleted of the Asset object.
		/// </summary>
		public override bool IsDeleted
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetCategories of the Asset object.
		/// </summary>
		public override string AssetCategories
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetMetadataVals of the Asset object.
		/// </summary>
		public override string AssetMetadataVals
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the MetadataXml of the Asset object.
		/// </summary>
		public override string MetadataXml
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Height of the Asset object.
		/// </summary>
		public override Nullable <Int32> Height
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the Width of the Asset object.
		/// </summary>
		public override Nullable <Int32> Width
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the Duration of the Asset object.
		/// </summary>
		public override Nullable <Int32> Duration
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the MetadataSearchVals of the Asset object.
		/// </summary>
		public override string MetadataSearchVals
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Plugin of the Asset object.
		/// </summary>
		public override Guid Plugin
		{
			get { return Guid.Empty; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

