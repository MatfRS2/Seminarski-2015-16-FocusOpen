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
	/// This object maps data between the database and a UserPasswordHistory object.
	/// </summary>
	internal partial class UserPasswordHistoryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			UserPasswordHistory userPasswordHistory = UserPasswordHistory.New();

			// Table Fields
			userPasswordHistory.UserPasswordHistoryId = reader.GetInt32("UserPasswordHistoryId"); 
			userPasswordHistory.UserId = reader.GetInt32("UserId");
			userPasswordHistory.Password = reader.GetString("Password");
			userPasswordHistory.Date = reader.GetDateTime("Date");
			

			userPasswordHistory.IsDirty = false;
			userPasswordHistory.ChangedProperties.Clear();
			
			return userPasswordHistory;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <UserPasswordHistory>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual UserPasswordHistory Update (UserPasswordHistory userPasswordHistory)
		{
 			if (!userPasswordHistory.IsDirty || userPasswordHistory.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return userPasswordHistory;
			}
			
			IDbCommand command = CreateCommand();
			
			if (userPasswordHistory.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [UserPasswordHistory] ([UserId], [Password], [Date]) VALUES (@userId, @password, @date) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [UserPasswordHistory] SET [UserId] = @userId, [Password] = @password, [Date] = @date WHERE UserPasswordHistoryId = @userPasswordHistoryId"; 
			}
			
			command.Parameters.Add (CreateParameter("@userId", userPasswordHistory.UserId));
			command.Parameters.Add (CreateParameter("@password", userPasswordHistory.Password));
			command.Parameters.Add (CreateParameter("@date", userPasswordHistory.Date));

			if (userPasswordHistory.IsNew) 
			{
				userPasswordHistory.UserPasswordHistoryId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@userPasswordHistoryId", userPasswordHistory.UserPasswordHistoryId));
				ExecuteCommand (command);
			}
			
			userPasswordHistory.IsDirty = false;
			userPasswordHistory.ChangedProperties.Clear();
			
			return userPasswordHistory;
		}

		public virtual void Delete (Nullable <Int32> userPasswordHistoryId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [UserPasswordHistory] WHERE UserPasswordHistoryId = @userPasswordHistoryId";
			command.Parameters.Add(CreateParameter("@userPasswordHistoryId", userPasswordHistoryId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single UserPasswordHistory object by userPasswordHistoryId
		/// </Summary>
		public virtual UserPasswordHistory Get (Nullable <Int32> userPasswordHistoryId)
		{
			IDbCommand command = GetGetCommand (userPasswordHistoryId);
			return (UserPasswordHistory) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual UserPasswordHistory FindOne (UserPasswordHistoryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? UserPasswordHistory.Empty : entity as UserPasswordHistory;
		}
		
		public virtual EntityList <UserPasswordHistory> FindMany (UserPasswordHistoryFinder finder)
		{
			return (EntityList <UserPasswordHistory>) (base.FindMany(finder));
		}

		public virtual EntityList <UserPasswordHistory> FindMany (UserPasswordHistoryFinder finder, int Page, int PageSize)
		{
			return (EntityList <UserPasswordHistory>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> userPasswordHistoryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [UserPasswordHistory] WHERE UserPasswordHistoryId = @userPasswordHistoryId";
			command.Parameters.Add(CreateParameter("@userPasswordHistoryId", userPasswordHistoryId)); 
			
			return command;
		}
	}
}

