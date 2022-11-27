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
	internal partial class AssetMetadataMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetMetadata assetMetadata = AssetMetadata.New();

			// Table Fields
			assetMetadata.AssetMetadataId = reader.GetInt32("AssetMetadataId"); 
			assetMetadata.AssetId = reader.GetInt32("AssetId");
			assetMetadata.MetadataId = reader.GetInt32("MetadataId");
			
			// View Fields
			assetMetadata.BrandId = reader.GetNullableInt32("BrandId");
			assetMetadata.GroupNumber = reader.GetInt32("GroupNumber");
			assetMetadata.Name = reader.GetString("Name");

			assetMetadata.IsDirty = false;
			assetMetadata.ChangedProperties.Clear();
			
			return assetMetadata;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetMetadata>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetMetadata Update (AssetMetadata assetMetadata)
		{
 			if (!assetMetadata.IsDirty || assetMetadata.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetMetadata;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetMetadata.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetMetadata] ([AssetId], [MetadataId]) VALUES (@assetId, @metadataId) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetMetadata] SET [AssetId] = @assetId, [MetadataId] = @metadataId WHERE AssetMetadataId = @assetMetadataId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetId", assetMetadata.AssetId));
			command.Parameters.Add (CreateParameter("@metadataId", assetMetadata.MetadataId));

			if (assetMetadata.IsNew) 
			{
				assetMetadata.AssetMetadataId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetMetadataId", assetMetadata.AssetMetadataId));
				ExecuteCommand (command);
			}
			
			assetMetadata.IsDirty = false;
			assetMetadata.ChangedProperties.Clear();
			
			return assetMetadata;
		}

		public virtual void Delete (Nullable <Int32> assetMetadataId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetMetadata] WHERE AssetMetadataId = @assetMetadataId";
			command.Parameters.Add(CreateParameter("@assetMetadataId", assetMetadataId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetMetadata object by assetMetadataId
		/// </Summary>
		public virtual AssetMetadata Get (Nullable <Int32> assetMetadataId)
		{
			IDbCommand command = GetGetCommand (assetMetadataId);
			return (AssetMetadata) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetMetadata FindOne (AssetMetadataFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetMetadata.Empty : entity as AssetMetadata;
		}
		
		public virtual EntityList <AssetMetadata> FindMany (AssetMetadataFinder finder)
		{
			return (EntityList <AssetMetadata>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetMetadata> FindMany (AssetMetadataFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetMetadata>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetMetadataId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AssetMetadata] WHERE AssetMetadataId = @assetMetadataId";
			command.Parameters.Add(CreateParameter("@assetMetadataId", assetMetadataId)); 
			
			return command;
		}
	}
}

