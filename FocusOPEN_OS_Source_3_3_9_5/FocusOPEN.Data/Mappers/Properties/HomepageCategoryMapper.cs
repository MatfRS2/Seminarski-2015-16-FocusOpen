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
	/// <summary>
	/// This object maps data between the database and a HomepageCategory object.
	/// </summary>
	internal partial class HomepageCategoryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			HomepageCategory homepageCategory = HomepageCategory.New();

			// Table Fields
			homepageCategory.HomepageCategoryId = reader.GetInt32("HomepageCategoryId"); 
			homepageCategory.HomepageId = reader.GetInt32("HomepageId");
			homepageCategory.CategoryId = reader.GetInt32("CategoryId");
			homepageCategory.OrderBy = reader.GetInt32("OrderBy");
			

			homepageCategory.IsDirty = false;
			homepageCategory.ChangedProperties.Clear();
			
			return homepageCategory;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <HomepageCategory>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual HomepageCategory Update (HomepageCategory homepageCategory)
		{
 			if (!homepageCategory.IsDirty || homepageCategory.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return homepageCategory;
			}
			
			IDbCommand command = CreateCommand();
			
			if (homepageCategory.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [HomepageCategory] ([HomepageId], [CategoryId], [OrderBy]) VALUES (@homepageId, @categoryId, @orderBy) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [HomepageCategory] SET [HomepageId] = @homepageId, [CategoryId] = @categoryId, [OrderBy] = @orderBy WHERE HomepageCategoryId = @homepageCategoryId"; 
			}
			
			command.Parameters.Add (CreateParameter("@homepageId", homepageCategory.HomepageId));
			command.Parameters.Add (CreateParameter("@categoryId", homepageCategory.CategoryId));
			command.Parameters.Add (CreateParameter("@orderBy", homepageCategory.OrderBy));

			if (homepageCategory.IsNew) 
			{
				homepageCategory.HomepageCategoryId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@homepageCategoryId", homepageCategory.HomepageCategoryId));
				ExecuteCommand (command);
			}
			
			homepageCategory.IsDirty = false;
			homepageCategory.ChangedProperties.Clear();
			
			return homepageCategory;
		}

		public virtual void Delete (Nullable <Int32> homepageCategoryId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [HomepageCategory] WHERE HomepageCategoryId = @homepageCategoryId";
			command.Parameters.Add(CreateParameter("@homepageCategoryId", homepageCategoryId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single HomepageCategory object by homepageCategoryId
		/// </Summary>
		public virtual HomepageCategory Get (Nullable <Int32> homepageCategoryId)
		{
			IDbCommand command = GetGetCommand (homepageCategoryId);
			return (HomepageCategory) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual HomepageCategory FindOne (HomepageCategoryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? HomepageCategory.Empty : entity as HomepageCategory;
		}
		
		public virtual EntityList <HomepageCategory> FindMany (HomepageCategoryFinder finder)
		{
			return (EntityList <HomepageCategory>) (base.FindMany(finder));
		}

		public virtual EntityList <HomepageCategory> FindMany (HomepageCategoryFinder finder, int Page, int PageSize)
		{
			return (EntityList <HomepageCategory>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> homepageCategoryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [HomepageCategory] WHERE HomepageCategoryId = @homepageCategoryId";
			command.Parameters.Add(CreateParameter("@homepageCategoryId", homepageCategoryId)); 
			
			return command;
		}
	}
}

