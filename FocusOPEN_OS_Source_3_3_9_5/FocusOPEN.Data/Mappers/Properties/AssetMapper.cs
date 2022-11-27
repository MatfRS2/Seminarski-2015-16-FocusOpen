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
using System.Data;
using Daydream.Data;

namespace FocusOPEN.Data
{
	internal partial class AssetMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Asset asset = Asset.New();

			// Table Fields
			asset.AssetId = reader.GetInt32("AssetId"); 
			asset.AssetTypeId = reader.GetInt32("AssetTypeId");
			asset.UploadDate = reader.GetDateTime("UploadDate");
			asset.Filename = reader.GetString("Filename");
			asset.FileSize = reader.GetInt64("FileSize");
			asset.FileHash = reader.GetString("FileHash");
			asset.TemplateAssetId = reader.GetNullableInt32("TemplateAssetId");
			asset.WorkflowId = reader.GetNullableInt32("WorkflowId");
			asset.Title = reader.GetString("Title");
			asset.ProjectCode = reader.GetString("ProjectCode");
			asset.Originator = reader.GetString("Originator");
			asset.Description = reader.GetString("Description");
			asset.Keywords = reader.GetString("Keywords");
			asset.ContactEmail = reader.GetString("ContactEmail");
			asset.ProductionDay = reader.GetNullableInt32("ProductionDay");
			asset.ProductionMonth = reader.GetNullableInt32("ProductionMonth");
			asset.ProductionYear = reader.GetNullableInt32("ProductionYear");
			asset.BrandId = reader.GetInt32("BrandId");
			asset.CopyrightOwner = reader.GetString("CopyrightOwner");
			asset.UsageRestrictions = reader.GetString("UsageRestrictions");
			asset.RestrictAttachedFiles = reader.GetBoolean("RestrictAttachedFiles");
			asset.WatermarkPreview = reader.GetBoolean("WatermarkPreview");
			asset.InternalUsers_DownloadApprovalRequired = reader.GetBoolean("InternalUsers_DownloadApprovalRequired");
			asset.InternalUsers_HideFromUsers = reader.GetBoolean("InternalUsers_HideFromUsers");
			asset.ExternalUsers_DownloadApprovalRequired = reader.GetBoolean("ExternalUsers_DownloadApprovalRequired");
			asset.ExternalUsers_HideFromUsers = reader.GetBoolean("ExternalUsers_HideFromUsers");
			asset.AssetFilePathId = reader.GetInt32("AssetFilePathId");
			asset.PublishDate = reader.GetDateTime("PublishDate");
			asset.ExpiryDate = reader.GetDateTime("ExpiryDate");
			asset.IsProcessed = reader.GetBoolean("IsProcessed");
			asset.AssetPublishStatusId = reader.GetInt32("AssetPublishStatusId");
			asset.UploadedByUserId = reader.GetInt32("UploadedByUserId");
			asset.CreateDate = reader.GetDateTime("CreateDate");
			asset.LastUpdate = reader.GetDateTime("LastUpdate");
			asset.DownloadCount = reader.GetInt32("DownloadCount");
			asset.PopularityRank = reader.GetInt32("PopularityRank");
			asset.IsDeleted = reader.GetBoolean("IsDeleted");
			asset.AssetCategories = reader.GetString("AssetCategories");
			asset.AssetMetadataVals = reader.GetString("AssetMetadataVals");
			asset.MetadataXml = reader.GetString("MetadataXml");
			asset.Height = reader.GetNullableInt32("Height");
			asset.Width = reader.GetNullableInt32("Width");
			asset.Duration = reader.GetNullableInt32("Duration");
			asset.MetadataSearchVals = reader.GetString("MetadataSearchVals");
			asset.Plugin = reader.GetGuid("Plugin");
			
			// View Fields
			asset.AssetTypeName = reader.GetString("AssetTypeName");
			asset.FileExtension = reader.GetString("FileExtension");
			asset.ShadowDate = reader.GetNullableDateTime("ShadowDate");
			asset.AssetPublishStatusName = reader.GetString("AssetPublishStatusName");
			asset.UploadedByUserName = reader.GetString("UploadedByUserName");
			asset.BrandName = reader.GetString("BrandName");
			asset.SearchableData = reader.GetString("SearchableData");

			asset.IsDirty = false;
			asset.ChangedProperties.Clear();
			
			return asset;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Asset>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Asset Update (Asset asset)
		{
 			if (!asset.IsDirty || asset.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return asset;
			}
			
			IDbCommand command = CreateCommand();
			
			if (asset.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Asset] ([AssetTypeId], [UploadDate], [Filename], [FileSize], [FileHash], [TemplateAssetId], [WorkflowId], [Title], [ProjectCode], [Originator], [Description], [Keywords], [ContactEmail], [ProductionDay], [ProductionMonth], [ProductionYear], [BrandId], [CopyrightOwner], [UsageRestrictions], [RestrictAttachedFiles], [WatermarkPreview], [InternalUsers_DownloadApprovalRequired], [InternalUsers_HideFromUsers], [ExternalUsers_DownloadApprovalRequired], [ExternalUsers_HideFromUsers], [AssetFilePathId], [PublishDate], [ExpiryDate], [IsProcessed], [AssetPublishStatusId], [UploadedByUserId], [CreateDate], [LastUpdate], [DownloadCount], [PopularityRank], [IsDeleted], [AssetCategories], [AssetMetadataVals], [MetadataXml], [Height], [Width], [Duration], [MetadataSearchVals], [Plugin]) VALUES (@assetTypeId, @uploadDate, @filename, @fileSize, @fileHash, @templateAssetId, @workflowId, @title, @projectCode, @originator, @description, @keywords, @contactEmail, @productionDay, @productionMonth, @productionYear, @brandId, @copyrightOwner, @usageRestrictions, @restrictAttachedFiles, @watermarkPreview, @internalUsers_DownloadApprovalRequired, @internalUsers_HideFromUsers, @externalUsers_DownloadApprovalRequired, @externalUsers_HideFromUsers, @assetFilePathId, @publishDate, @expiryDate, @isProcessed, @assetPublishStatusId, @uploadedByUserId, @createDate, @lastUpdate, @downloadCount, @popularityRank, @isDeleted, @assetCategories, @assetMetadataVals, @metadataXml, @height, @width, @duration, @metadataSearchVals, @plugin) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Asset] SET [AssetTypeId] = @assetTypeId, [UploadDate] = @uploadDate, [Filename] = @filename, [FileSize] = @fileSize, [FileHash] = @fileHash, [TemplateAssetId] = @templateAssetId, [WorkflowId] = @workflowId, [Title] = @title, [ProjectCode] = @projectCode, [Originator] = @originator, [Description] = @description, [Keywords] = @keywords, [ContactEmail] = @contactEmail, [ProductionDay] = @productionDay, [ProductionMonth] = @productionMonth, [ProductionYear] = @productionYear, [BrandId] = @brandId, [CopyrightOwner] = @copyrightOwner, [UsageRestrictions] = @usageRestrictions, [RestrictAttachedFiles] = @restrictAttachedFiles, [WatermarkPreview] = @watermarkPreview, [InternalUsers_DownloadApprovalRequired] = @internalUsers_DownloadApprovalRequired, [InternalUsers_HideFromUsers] = @internalUsers_HideFromUsers, [ExternalUsers_DownloadApprovalRequired] = @externalUsers_DownloadApprovalRequired, [ExternalUsers_HideFromUsers] = @externalUsers_HideFromUsers, [AssetFilePathId] = @assetFilePathId, [PublishDate] = @publishDate, [ExpiryDate] = @expiryDate, [IsProcessed] = @isProcessed, [AssetPublishStatusId] = @assetPublishStatusId, [UploadedByUserId] = @uploadedByUserId, [CreateDate] = @createDate, [LastUpdate] = @lastUpdate, [DownloadCount] = @downloadCount, [PopularityRank] = @popularityRank, [IsDeleted] = @isDeleted, [AssetCategories] = @assetCategories, [AssetMetadataVals] = @assetMetadataVals, [MetadataXml] = @metadataXml, [Height] = @height, [Width] = @width, [Duration] = @duration, [MetadataSearchVals] = @metadataSearchVals, [Plugin] = @plugin WHERE AssetId = @assetId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetTypeId", asset.AssetTypeId));
			command.Parameters.Add (CreateParameter("@uploadDate", asset.UploadDate));
			command.Parameters.Add (CreateParameter("@filename", asset.Filename));
			command.Parameters.Add (CreateParameter("@fileSize", asset.FileSize));
			command.Parameters.Add (CreateParameter("@fileHash", asset.FileHash));
			command.Parameters.Add (CreateParameter("@templateAssetId", asset.TemplateAssetId));
			command.Parameters.Add (CreateParameter("@workflowId", asset.WorkflowId));
			command.Parameters.Add (CreateParameter("@title", asset.Title));
			command.Parameters.Add (CreateParameter("@projectCode", asset.ProjectCode));
			command.Parameters.Add (CreateParameter("@originator", asset.Originator));
			command.Parameters.Add (CreateParameter("@description", asset.Description));
			command.Parameters.Add (CreateParameter("@keywords", asset.Keywords));
			command.Parameters.Add (CreateParameter("@contactEmail", asset.ContactEmail));
			command.Parameters.Add (CreateParameter("@productionDay", asset.ProductionDay));
			command.Parameters.Add (CreateParameter("@productionMonth", asset.ProductionMonth));
			command.Parameters.Add (CreateParameter("@productionYear", asset.ProductionYear));
			command.Parameters.Add (CreateParameter("@brandId", asset.BrandId));
			command.Parameters.Add (CreateParameter("@copyrightOwner", asset.CopyrightOwner));
			command.Parameters.Add (CreateParameter("@usageRestrictions", asset.UsageRestrictions));
			command.Parameters.Add (CreateParameter("@restrictAttachedFiles", asset.RestrictAttachedFiles));
			command.Parameters.Add (CreateParameter("@watermarkPreview", asset.WatermarkPreview));
			command.Parameters.Add (CreateParameter("@internalUsers_DownloadApprovalRequired", asset.InternalUsers_DownloadApprovalRequired));
			command.Parameters.Add (CreateParameter("@internalUsers_HideFromUsers", asset.InternalUsers_HideFromUsers));
			command.Parameters.Add (CreateParameter("@externalUsers_DownloadApprovalRequired", asset.ExternalUsers_DownloadApprovalRequired));
			command.Parameters.Add (CreateParameter("@externalUsers_HideFromUsers", asset.ExternalUsers_HideFromUsers));
			command.Parameters.Add (CreateParameter("@assetFilePathId", asset.AssetFilePathId));
			command.Parameters.Add (CreateParameter("@publishDate", asset.PublishDate));
			command.Parameters.Add (CreateParameter("@expiryDate", asset.ExpiryDate));
			command.Parameters.Add (CreateParameter("@isProcessed", asset.IsProcessed));
			command.Parameters.Add (CreateParameter("@assetPublishStatusId", asset.AssetPublishStatusId));
			command.Parameters.Add (CreateParameter("@uploadedByUserId", asset.UploadedByUserId));
			command.Parameters.Add (CreateParameter("@createDate", asset.CreateDate));
			command.Parameters.Add (CreateParameter("@lastUpdate", asset.LastUpdate));
			command.Parameters.Add (CreateParameter("@downloadCount", asset.DownloadCount));
			command.Parameters.Add (CreateParameter("@popularityRank", asset.PopularityRank));
			command.Parameters.Add (CreateParameter("@isDeleted", asset.IsDeleted));
			command.Parameters.Add (CreateParameter("@assetCategories", asset.AssetCategories));
			command.Parameters.Add (CreateParameter("@assetMetadataVals", asset.AssetMetadataVals));
			command.Parameters.Add (CreateParameter("@metadataXml", asset.MetadataXml));
			command.Parameters.Add (CreateParameter("@height", asset.Height));
			command.Parameters.Add (CreateParameter("@width", asset.Width));
			command.Parameters.Add (CreateParameter("@duration", asset.Duration));
			command.Parameters.Add (CreateParameter("@metadataSearchVals", asset.MetadataSearchVals));
			command.Parameters.Add (CreateParameter("@plugin", asset.Plugin));

			if (asset.IsNew) 
			{
				asset.AssetId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetId", asset.AssetId));
				ExecuteCommand (command);
			}
			
			asset.IsDirty = false;
			asset.ChangedProperties.Clear();
			
			return asset;
		}

		public virtual void Delete (Nullable <Int32> assetId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Asset] WHERE AssetId = @assetId";
			command.Parameters.Add(CreateParameter("@assetId", assetId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Asset object by assetId
		// </Summary>
		public virtual Asset Get (Nullable <Int32> assetId)
		{
			IDbCommand command = GetGetCommand (assetId);
			return (Asset) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Asset FindOne (AssetFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Asset.Empty : entity as Asset;
		}
		
		public virtual EntityList <Asset> FindMany (AssetFinder finder)
		{
			return (EntityList <Asset>) (base.FindMany(finder));
		}

		public virtual EntityList <Asset> FindMany (AssetFinder finder, int Page, int PageSize)
		{
			return (EntityList <Asset>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Asset] WHERE AssetId = @assetId";
			command.Parameters.Add(CreateParameter("@assetId", assetId)); 
			
			return command;
		}
	}
}

