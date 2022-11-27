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
	internal partial class AssetTypeMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetType assetType = AssetType.New();

			// Table Fields
			assetType.AssetTypeId = reader.GetInt32("AssetTypeId"); 
			assetType.Name = reader.GetString("Name");
			assetType.IsVisible = reader.GetBoolean("IsVisible");
			assetType.IsDeleted = reader.GetBoolean("IsDeleted");
			
			// View Fields

			assetType.IsDirty = false;
			assetType.ChangedProperties.Clear();
			
			return assetType;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetType>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetType Update (AssetType assetType)
		{
 			if (!assetType.IsDirty || assetType.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetType;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetType.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetType] ([Name], [IsVisible], [IsDeleted]) VALUES (@name, @isVisible, @isDeleted) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetType] SET [Name] = @name, [IsVisible] = @isVisible, [IsDeleted] = @isDeleted WHERE AssetTypeId = @assetTypeId"; 
			}
			
			command.Parameters.Add (CreateParameter("@name", assetType.Name));
			command.Parameters.Add (CreateParameter("@isVisible", assetType.IsVisible));
			command.Parameters.Add (CreateParameter("@isDeleted", assetType.IsDeleted));

			if (assetType.IsNew) 
			{
				assetType.AssetTypeId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetTypeId", assetType.AssetTypeId));
				ExecuteCommand (command);
			}
			
			assetType.IsDirty = false;
			assetType.ChangedProperties.Clear();
			
			return assetType;
		}

		public virtual void Delete (Nullable <Int32> assetTypeId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetType] WHERE AssetTypeId = @assetTypeId";
			command.Parameters.Add(CreateParameter("@assetTypeId", assetTypeId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetType object by assetTypeId
		/// </Summary>
		public virtual AssetType Get (Nullable <Int32> assetTypeId)
		{
			IDbCommand command = GetGetCommand (assetTypeId);
			return (AssetType) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetType FindOne (AssetTypeFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetType.Empty : entity as AssetType;
		}
		
		public virtual EntityList <AssetType> FindMany (AssetTypeFinder finder)
		{
			return (EntityList <AssetType>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetType> FindMany (AssetTypeFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetType>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetTypeId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AssetType] WHERE AssetTypeId = @assetTypeId";
			command.Parameters.Add(CreateParameter("@assetTypeId", assetTypeId)); 
			
			return command;
		}
	}
}

