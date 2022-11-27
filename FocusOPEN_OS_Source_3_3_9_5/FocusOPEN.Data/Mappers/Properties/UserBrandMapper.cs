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
	/// This object maps data between the database and a UserBrand object.
	/// </summary>
	internal partial class UserBrandMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			UserBrand userBrand = UserBrand.New();

			// Table Fields
			userBrand.UserBrandId = reader.GetInt32("UserBrandId"); 
			userBrand.UserId = reader.GetInt32("UserId");
			userBrand.BrandId = reader.GetInt32("BrandId");
			

			userBrand.IsDirty = false;
			userBrand.ChangedProperties.Clear();
			
			return userBrand;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <UserBrand>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual UserBrand Update (UserBrand userBrand)
		{
 			if (!userBrand.IsDirty || userBrand.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return userBrand;
			}
			
			IDbCommand command = CreateCommand();
			
			if (userBrand.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [UserBrand] ([UserId], [BrandId]) VALUES (@userId, @brandId) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [UserBrand] SET [UserId] = @userId, [BrandId] = @brandId WHERE UserBrandId = @userBrandId"; 
			}
			
			command.Parameters.Add (CreateParameter("@userId", userBrand.UserId));
			command.Parameters.Add (CreateParameter("@brandId", userBrand.BrandId));

			if (userBrand.IsNew) 
			{
				userBrand.UserBrandId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@userBrandId", userBrand.UserBrandId));
				ExecuteCommand (command);
			}
			
			userBrand.IsDirty = false;
			userBrand.ChangedProperties.Clear();
			
			return userBrand;
		}

		public virtual void Delete (Nullable <Int32> userBrandId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [UserBrand] WHERE UserBrandId = @userBrandId";
			command.Parameters.Add(CreateParameter("@userBrandId", userBrandId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single UserBrand object by userBrandId
		/// </Summary>
		public virtual UserBrand Get (Nullable <Int32> userBrandId)
		{
			IDbCommand command = GetGetCommand (userBrandId);
			return (UserBrand) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual UserBrand FindOne (UserBrandFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? UserBrand.Empty : entity as UserBrand;
		}
		
		public virtual EntityList <UserBrand> FindMany (UserBrandFinder finder)
		{
			return (EntityList <UserBrand>) (base.FindMany(finder));
		}

		public virtual EntityList <UserBrand> FindMany (UserBrandFinder finder, int Page, int PageSize)
		{
			return (EntityList <UserBrand>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> userBrandId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [UserBrand] WHERE UserBrandId = @userBrandId";
			command.Parameters.Add(CreateParameter("@userBrandId", userBrandId)); 
			
			return command;
		}
	}
}

