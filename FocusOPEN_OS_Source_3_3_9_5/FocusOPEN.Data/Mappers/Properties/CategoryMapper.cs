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
	internal partial class CategoryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Category category = Category.New();

			// Table Fields
			category.CategoryId = reader.GetInt32("CategoryId"); 
			category.ParentCategoryId = reader.GetNullableInt32("ParentCategoryId");
			category.BrandId = reader.GetInt32("BrandId");
			category.Name = reader.GetString("Name");
			category.ExternalRef = reader.GetString("ExternalRef");
			category.Message = reader.GetString("Message");
			category.Synonyms = reader.GetString("Synonyms");
			category.OwnerUserId = reader.GetInt32("OwnerUserId");
			category.CategoryOrder = reader.GetInt32("CategoryOrder");
			category.FullAssetCount = reader.GetInt32("FullAssetCount");
			category.AvailableAssetCount = reader.GetInt32("AvailableAssetCount");
			
			// View Fields
			category.OwnerEmail = reader.GetString("OwnerEmail");
			category.OwnerName = reader.GetString("OwnerName");

			category.IsDirty = false;
			category.ChangedProperties.Clear();
			
			return category;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Category>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Category Update (Category category)
		{
 			if (!category.IsDirty || category.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return category;
			}
			
			IDbCommand command = CreateCommand();
			
			if (category.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Category] ([ParentCategoryId], [BrandId], [Name], [ExternalRef], [Message], [Synonyms], [OwnerUserId], [CategoryOrder], [FullAssetCount], [AvailableAssetCount]) VALUES (@parentCategoryId, @brandId, @name, @externalRef, @message, @synonyms, @ownerUserId, @categoryOrder, @fullAssetCount, @availableAssetCount) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Category] SET [ParentCategoryId] = @parentCategoryId, [BrandId] = @brandId, [Name] = @name, [ExternalRef] = @externalRef, [Message] = @message, [Synonyms] = @synonyms, [OwnerUserId] = @ownerUserId, [CategoryOrder] = @categoryOrder, [FullAssetCount] = @fullAssetCount, [AvailableAssetCount] = @availableAssetCount WHERE CategoryId = @categoryId"; 
			}
			
			command.Parameters.Add (CreateParameter("@parentCategoryId", category.ParentCategoryId));
			command.Parameters.Add (CreateParameter("@brandId", category.BrandId));
			command.Parameters.Add (CreateParameter("@name", category.Name));
			command.Parameters.Add (CreateParameter("@externalRef", category.ExternalRef));
			command.Parameters.Add (CreateParameter("@message", category.Message));
			command.Parameters.Add (CreateParameter("@synonyms", category.Synonyms));
			command.Parameters.Add (CreateParameter("@ownerUserId", category.OwnerUserId));
			command.Parameters.Add (CreateParameter("@categoryOrder", category.CategoryOrder));
			command.Parameters.Add (CreateParameter("@fullAssetCount", category.FullAssetCount));
			command.Parameters.Add (CreateParameter("@availableAssetCount", category.AvailableAssetCount));

			if (category.IsNew) 
			{
				category.CategoryId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@categoryId", category.CategoryId));
				ExecuteCommand (command);
			}
			
			category.IsDirty = false;
			category.ChangedProperties.Clear();
			
			return category;
		}

		public virtual void Delete (Nullable <Int32> categoryId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Category] WHERE CategoryId = @categoryId";
			command.Parameters.Add(CreateParameter("@categoryId", categoryId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Category object by categoryId
		/// </Summary>
		public virtual Category Get (Nullable <Int32> categoryId)
		{
			IDbCommand command = GetGetCommand (categoryId);
			return (Category) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Category FindOne (CategoryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Category.Empty : entity as Category;
		}
		
		public virtual EntityList <Category> FindMany (CategoryFinder finder)
		{
			return (EntityList <Category>) (base.FindMany(finder));
		}

		public virtual EntityList <Category> FindMany (CategoryFinder finder, int Page, int PageSize)
		{
			return (EntityList <Category>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> categoryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Category] WHERE CategoryId = @categoryId";
			command.Parameters.Add(CreateParameter("@categoryId", categoryId)); 
			
			return command;
		}
	}
}

