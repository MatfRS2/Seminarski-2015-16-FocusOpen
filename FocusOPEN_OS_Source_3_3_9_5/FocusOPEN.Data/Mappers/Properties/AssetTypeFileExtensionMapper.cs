/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
	internal partial class AssetTypeFileExtensionMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetTypeFileExtension assetTypeFileExtension = AssetTypeFileExtension.New();

			// Table Fields
			assetTypeFileExtension.AssetTypeFileExtensionId = reader.GetInt32("AssetTypeFileExtensionId"); 
			assetTypeFileExtension.Extension = reader.GetString("Extension");
			assetTypeFileExtension.Name = reader.GetString("Name");
			assetTypeFileExtension.AssetTypeId = reader.GetInt32("AssetTypeId");
			assetTypeFileExtension.IconImage = reader.GetBytes("IconImage");
			assetTypeFileExtension.IconFilename = reader.GetString("IconFilename");
			assetTypeFileExtension.IsVisible = reader.GetBoolean("IsVisible");
			assetTypeFileExtension.Plugin = reader.GetGuid("Plugin");
			
			// View Fields
			assetTypeFileExtension.AssetTypeName = reader.GetString("AssetTypeName");

			assetTypeFileExtension.IsDirty = false;
			assetTypeFileExtension.ChangedProperties.Clear();
			
			return assetTypeFileExtension;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetTypeFileExtension>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetTypeFileExtension Update (AssetTypeFileExtension assetTypeFileExtension)
		{
 			if (!assetTypeFileExtension.IsDirty || assetTypeFileExtension.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetTypeFileExtension;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetTypeFileExtension.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetTypeFileExtension] ([Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (@extension, @name, @assetTypeId, @iconImage, @iconFilename, @isVisible, @plugin) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetTypeFileExtension] SET [Extension] = @extension, [Name] = @name, [AssetTypeId] = @assetTypeId, [IconImage] = @iconImage, [IconFilename] = @iconFilename, [IsVisible] = @isVisible, [Plugin] = @plugin WHERE AssetTypeFileExtensionId = @assetTypeFileExtensionId"; 
			}

            // HACK: Need to deal with this generated code in a better way
            IDbDataParameter param = CreateParameter("@iconImage", assetTypeFileExtension.IconImage);
            param.DbType = DbType.Binary;
            command.Parameters.Add(param);

			command.Parameters.Add (CreateParameter("@extension", assetTypeFileExtension.Extension));
			command.Parameters.Add (CreateParameter("@name", assetTypeFileExtension.Name));
			command.Parameters.Add (CreateParameter("@assetTypeId", assetTypeFileExtension.AssetTypeId));
			command.Parameters.Add (CreateParameter("@iconFilename", assetTypeFileExtension.IconFilename));
			command.Parameters.Add (CreateParameter("@isVisible", assetTypeFileExtension.IsVisible));
			command.Parameters.Add (CreateParameter("@plugin", assetTypeFileExtension.Plugin));

			if (assetTypeFileExtension.IsNew) 
			{
				assetTypeFileExtension.AssetTypeFileExtensionId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetTypeFileExtensionId", assetTypeFileExtension.AssetTypeFileExtensionId));
				ExecuteCommand (command);
			}
			
			assetTypeFileExtension.IsDirty = false;
			assetTypeFileExtension.ChangedProperties.Clear();
			
			return assetTypeFileExtension;
		}

		public virtual void Delete (Nullable <Int32> assetTypeFileExtensionId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetTypeFileExtension] WHERE AssetTypeFileExtensionId = @assetTypeFileExtensionId";
			command.Parameters.Add(CreateParameter("@assetTypeFileExtensionId", assetTypeFileExtensionId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetTypeFileExtension object by assetTypeFileExtensionId
		// </Summary>
		public virtual AssetTypeFileExtension Get (Nullable <Int32> assetTypeFileExtensionId)
		{
			IDbCommand command = GetGetCommand (assetTypeFileExtensionId);
			return (AssetTypeFileExtension) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetTypeFileExtension FindOne (AssetTypeFileExtensionFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetTypeFileExtension.Empty : entity as AssetTypeFileExtension;
		}
		
		public virtual EntityList <AssetTypeFileExtension> FindMany (AssetTypeFileExtensionFinder finder)
		{
			return (EntityList <AssetTypeFileExtension>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetTypeFileExtension> FindMany (AssetTypeFileExtensionFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetTypeFileExtension>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetTypeFileExtensionId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AssetTypeFileExtension] WHERE AssetTypeFileExtensionId = @assetTypeFileExtensionId";
			command.Parameters.Add(CreateParameter("@assetTypeFileExtensionId", assetTypeFileExtensionId)); 
			
			return command;
		}
	}
}

