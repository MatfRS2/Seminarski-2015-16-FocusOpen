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
	internal partial class AssetCategoryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetCategory assetCategory = AssetCategory.New();

			// Table Fields
			assetCategory.AssetCategoryId = reader.GetInt32("AssetCategoryId"); 
			assetCategory.AssetId = reader.GetInt32("AssetId");
			assetCategory.CategoryId = reader.GetInt32("CategoryId");
			assetCategory.IsPrimary = reader.GetBoolean("IsPrimary");
			
			// View Fields
			assetCategory.Name = reader.GetString("Name");
			assetCategory.ParentCategoryId = reader.GetNullableInt32("ParentCategoryId");
			assetCategory.Synonyms = reader.GetString("Synonyms");
			assetCategory.CategoryOrder = reader.GetInt32("CategoryOrder");
			assetCategory.NameAndSynonyms = reader.GetString("NameAndSynonyms");

			assetCategory.IsDirty = false;
			assetCategory.ChangedProperties.Clear();
			
			return assetCategory;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetCategory>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetCategory Update (AssetCategory assetCategory)
		{
 			if (!assetCategory.IsDirty || assetCategory.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetCategory;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetCategory.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetCategory] ([AssetId], [CategoryId], [IsPrimary]) VALUES (@assetId, @categoryId, @isPrimary) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetCategory] SET [AssetId] = @assetId, [CategoryId] = @categoryId, [IsPrimary] = @isPrimary WHERE AssetCategoryId = @assetCategoryId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetId", assetCategory.AssetId));
			command.Parameters.Add (CreateParameter("@categoryId", assetCategory.CategoryId));
			command.Parameters.Add (CreateParameter("@isPrimary", assetCategory.IsPrimary));

			if (assetCategory.IsNew) 
			{
				assetCategory.AssetCategoryId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetCategoryId", assetCategory.AssetCategoryId));
				ExecuteCommand (command);
			}
			
			assetCategory.IsDirty = false;
			assetCategory.ChangedProperties.Clear();
			
			return assetCategory;
		}

		public virtual void Delete (Nullable <Int32> assetCategoryId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetCategory] WHERE AssetCategoryId = @assetCategoryId";
			command.Parameters.Add(CreateParameter("@assetCategoryId", assetCategoryId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetCategory object by assetCategoryId
		/// </Summary>
		public virtual AssetCategory Get (Nullable <Int32> assetCategoryId)
		{
			IDbCommand command = GetGetCommand (assetCategoryId);
			return (AssetCategory) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetCategory FindOne (AssetCategoryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetCategory.Empty : entity as AssetCategory;
		}
		
		public virtual EntityList <AssetCategory> FindMany (AssetCategoryFinder finder)
		{
			return (EntityList <AssetCategory>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetCategory> FindMany (AssetCategoryFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetCategory>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetCategoryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AssetCategory] WHERE AssetCategoryId = @assetCategoryId";
			command.Parameters.Add(CreateParameter("@assetCategoryId", assetCategoryId)); 
			
			return command;
		}
	}
}

