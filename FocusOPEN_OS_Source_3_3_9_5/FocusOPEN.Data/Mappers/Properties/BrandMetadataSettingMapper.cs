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
	internal partial class BrandMetadataSettingMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			BrandMetadataSetting brandMetadataSetting = BrandMetadataSetting.New();

			// Table Fields
			brandMetadataSetting.BrandMetadataSettingId = reader.GetInt32("BrandMetadataSettingId"); 
			brandMetadataSetting.BrandId = reader.GetInt32("BrandId");
			brandMetadataSetting.FieldId = reader.GetString("FieldId");
			brandMetadataSetting.FieldName = reader.GetString("FieldName");
			brandMetadataSetting.IsRequired = reader.GetBoolean("IsRequired");
			brandMetadataSetting.OnAssetForm = reader.GetBoolean("OnAssetForm");
			brandMetadataSetting.OnAssetDetail = reader.GetBoolean("OnAssetDetail");
			brandMetadataSetting.AdditionalCopy = reader.GetString("AdditionalCopy");
			brandMetadataSetting.ToolTip = reader.GetString("ToolTip");
            brandMetadataSetting.UiControlType = reader.GetInt32("UiControlType");
            brandMetadataSetting.IsCustom = reader.GetBoolean("IsCustom");
            brandMetadataSetting.CatalogueOrderNum = reader.GetInt32("CatalogueOrderNum");
            brandMetadataSetting.AssetDetailOrderNum = reader.GetInt32("AssetDetailOrderNum");

			brandMetadataSetting.IsDirty = false;
			brandMetadataSetting.ChangedProperties.Clear();
			
			return brandMetadataSetting;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <BrandMetadataSetting>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual BrandMetadataSetting Update (BrandMetadataSetting brandMetadataSetting)
		{
 			if (!brandMetadataSetting.IsDirty || brandMetadataSetting.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return brandMetadataSetting;
			}
			
			IDbCommand command = CreateCommand();
			
			if (brandMetadataSetting.IsNew) 
			{
				// Adding
                command.CommandText = "INSERT INTO [BrandMetadataSetting] ([BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip],[UiControlType],[IsCustom],[CatalogueOrderNum],[AssetDetailOrderNum]  ) VALUES (@brandId, @fieldId, @fieldName, @isRequired, @onAssetForm, @onAssetDetail, @additionalCopy, @toolTip, @uiControlType, @isCustom, @catalogueOrderNum, @assetDetailOrderNum) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
                command.CommandText = "UPDATE [BrandMetadataSetting] SET [BrandId] = @brandId, [FieldId] = @fieldId, [FieldName] = @fieldName, [IsRequired] = @isRequired, [OnAssetForm] = @onAssetForm, [OnAssetDetail] = @onAssetDetail, [AdditionalCopy] = @additionalCopy, [ToolTip] = @toolTip, [UiControlType] = @uiControlType, [IsCustom] = @isCustom, [CatalogueOrderNum] = @catalogueOrderNum, [AssetDetailOrderNum] = @assetDetailOrderNum  WHERE BrandMetadataSettingId = @brandMetadataSettingId"; 
			}
			
			command.Parameters.Add (CreateParameter("@brandId", brandMetadataSetting.BrandId));
			command.Parameters.Add (CreateParameter("@fieldId", brandMetadataSetting.FieldId));
			command.Parameters.Add (CreateParameter("@fieldName", brandMetadataSetting.FieldName));
			command.Parameters.Add (CreateParameter("@isRequired", brandMetadataSetting.IsRequired));
			command.Parameters.Add (CreateParameter("@onAssetForm", brandMetadataSetting.OnAssetForm));
			command.Parameters.Add (CreateParameter("@onAssetDetail", brandMetadataSetting.OnAssetDetail));
			command.Parameters.Add (CreateParameter("@additionalCopy", brandMetadataSetting.AdditionalCopy));
			command.Parameters.Add (CreateParameter("@toolTip", brandMetadataSetting.ToolTip));
//			command.Parameters.Add (CreateParameter("@allowMultiple", brandMetadataSetting.AllowMultiple));
            command.Parameters.Add( CreateParameter("@uiControlType", brandMetadataSetting.UiControlType));
            command.Parameters.Add( CreateParameter("@isCustom", brandMetadataSetting.IsCustom));
            command.Parameters.Add(CreateParameter("@catalogueOrderNum", brandMetadataSetting.CatalogueOrderNum));
            command.Parameters.Add(CreateParameter("@assetDetailOrderNum", brandMetadataSetting.AssetDetailOrderNum));

			if (brandMetadataSetting.IsNew) 
			{
				brandMetadataSetting.BrandMetadataSettingId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@brandMetadataSettingId", brandMetadataSetting.BrandMetadataSettingId));
				ExecuteCommand (command);
			}
			
			brandMetadataSetting.IsDirty = false;
			brandMetadataSetting.ChangedProperties.Clear();
			
			return brandMetadataSetting;
		}

		public virtual void Delete (Nullable <Int32> brandMetadataSettingId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [BrandMetadataSetting] WHERE BrandMetadataSettingId = @brandMetadataSettingId";
			command.Parameters.Add(CreateParameter("@brandMetadataSettingId", brandMetadataSettingId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single BrandMetadataSetting object by brandMetadataSettingId
		/// </Summary>
		public virtual BrandMetadataSetting Get (Nullable <Int32> brandMetadataSettingId)
		{
			IDbCommand command = GetGetCommand (brandMetadataSettingId);
			return (BrandMetadataSetting) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual BrandMetadataSetting FindOne (BrandMetadataSettingFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? BrandMetadataSetting.Empty : entity as BrandMetadataSetting;
		}
		
		public virtual EntityList <BrandMetadataSetting> FindMany (BrandMetadataSettingFinder finder)
		{
			return (EntityList <BrandMetadataSetting>) (base.FindMany(finder));
		}

		public virtual EntityList <BrandMetadataSetting> FindMany (BrandMetadataSettingFinder finder, int Page, int PageSize)
		{
			return (EntityList <BrandMetadataSetting>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> brandMetadataSettingId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [BrandMetadataSetting] WHERE BrandMetadataSettingId = @brandMetadataSettingId";
			command.Parameters.Add(CreateParameter("@brandMetadataSettingId", brandMetadataSettingId)); 
			
			return command;
		}
	}
}

