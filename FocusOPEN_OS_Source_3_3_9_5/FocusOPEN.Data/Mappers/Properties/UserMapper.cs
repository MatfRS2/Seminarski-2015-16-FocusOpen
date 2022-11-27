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
	internal partial class UserMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			User user = User.New();

			// Table Fields
			user.UserId = reader.GetInt32("UserId"); 
			user.FirstName = reader.GetString("FirstName");
			user.LastName = reader.GetString("LastName");
			user.Email = reader.GetString("Email");
			user.Password = reader.GetString("Password");
			user.PasswordSalt = reader.GetString("PasswordSalt");
			user.PasswordExpiryDate = reader.GetNullableDateTime("PasswordExpiryDate");
			user.IsPasswordNonExpiring = reader.GetBoolean("IsPasswordNonExpiring");
			user.IsEmployee = reader.GetBoolean("IsEmployee");
			user.IsEngineer = reader.GetBoolean("IsEngineer");
			user.CompanyName = reader.GetString("CompanyName");
			user.PrimaryBrandId = reader.GetInt32("PrimaryBrandId");
			user.CountryId = reader.GetNullableInt32("CountryId");
			user.PhoneNumber = reader.GetString("PhoneNumber");
			user.MobileNumber = reader.GetString("MobileNumber");
			user.RegisterDate = reader.GetDateTime("RegisterDate");
			user.LastLoginDate = reader.GetNullableDateTime("LastLoginDate");
			user.AccountExpiryDate = reader.GetNullableDateTime("AccountExpiryDate");
			user.IsAccountNonExpiring = reader.GetBoolean("IsAccountNonExpiring");
			user.IsSuspended = reader.GetBoolean("IsSuspended");
			user.UserRoleId = reader.GetInt32("UserRoleId");
			user.UserStatusId = reader.GetInt32("UserStatusId");
			user.UseWorkflow = reader.GetBoolean("UseWorkflow");
			user.WorkflowId = reader.GetNullableInt32("WorkflowId");
			user.Notes = reader.GetString("Notes");
			user.IsAllowedExternalAccess = reader.GetBoolean("IsAllowedExternalAccess");
			user.ApproverUserId = reader.GetNullableInt32("ApproverUserId");
			user.CatalogueAssetIdList = reader.GetString("CatalogueAssetIdList");
			user.IsDeleted = reader.GetBoolean("IsDeleted");
			user.Guid = reader.GetString("Guid");
			user.EnableFilePathIngestion = reader.GetBoolean("EnableFilePathIngestion");
			user.UserAPIToken = reader.GetString("UserAPIToken");
			user.SessionAPIToken = reader.GetString("SessionAPIToken");
			user.LastAPIOperation = reader.GetNullableDateTime("LastAPIOperation");
			
			// View Fields
			user.FullName = reader.GetString("FullName");
			user.IsPasswordExpired = reader.GetNullableBoolean("IsPasswordExpired");
			user.IsAccountExpired = reader.GetNullableBoolean("IsAccountExpired");
			user.BrandName = reader.GetString("BrandName");
			user.UserStatusName = reader.GetString("UserStatusName");
			user.UserRoleName = reader.GetString("UserRoleName");

			user.IsDirty = false;
			user.ChangedProperties.Clear();
			
			return user;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <User>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual User Update (User user)
		{
 			if (!user.IsDirty || user.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return user;
			}
			
			IDbCommand command = CreateCommand();
			
			if (user.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [User] ([FirstName], [LastName], [Email], [Password], [PasswordSalt], [PasswordExpiryDate], [IsPasswordNonExpiring], [IsEmployee], [IsEngineer], [CompanyName], [PrimaryBrandId], [CountryId], [PhoneNumber], [MobileNumber], [RegisterDate], [LastLoginDate], [AccountExpiryDate], [IsAccountNonExpiring], [IsSuspended], [UserRoleId], [UserStatusId], [UseWorkflow], [WorkflowId], [Notes], [IsAllowedExternalAccess], [ApproverUserId], [CatalogueAssetIdList], [IsDeleted], [Guid], [EnableFilePathIngestion], [UserAPIToken], [SessionAPIToken], [LastAPIOperation]) VALUES (@firstName, @lastName, @email, @password, @passwordSalt, @passwordExpiryDate, @isPasswordNonExpiring, @isEmployee, @isEngineer, @companyName, @primaryBrandId, @countryId, @phoneNumber, @mobileNumber, @registerDate, @lastLoginDate, @accountExpiryDate, @isAccountNonExpiring, @isSuspended, @userRoleId, @userStatusId, @useWorkflow, @workflowId, @notes, @isAllowedExternalAccess, @approverUserId, @catalogueAssetIdList, @isDeleted, @guid, @enableFilePathIngestion, @userAPIToken, @sessionAPIToken, @lastAPIOperation) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [User] SET [FirstName] = @firstName, [LastName] = @lastName, [Email] = @email, [Password] = @password, [PasswordSalt] = @passwordSalt, [PasswordExpiryDate] = @passwordExpiryDate, [IsPasswordNonExpiring] = @isPasswordNonExpiring, [IsEmployee] = @isEmployee, [IsEngineer] = @isEngineer, [CompanyName] = @companyName, [PrimaryBrandId] = @primaryBrandId, [CountryId] = @countryId, [PhoneNumber] = @phoneNumber, [MobileNumber] = @mobileNumber, [RegisterDate] = @registerDate, [LastLoginDate] = @lastLoginDate, [AccountExpiryDate] = @accountExpiryDate, [IsAccountNonExpiring] = @isAccountNonExpiring, [IsSuspended] = @isSuspended, [UserRoleId] = @userRoleId, [UserStatusId] = @userStatusId, [UseWorkflow] = @useWorkflow, [WorkflowId] = @workflowId, [Notes] = @notes, [IsAllowedExternalAccess] = @isAllowedExternalAccess, [ApproverUserId] = @approverUserId, [CatalogueAssetIdList] = @catalogueAssetIdList, [IsDeleted] = @isDeleted, [Guid] = @guid, [EnableFilePathIngestion] = @enableFilePathIngestion, [UserAPIToken] = @userAPIToken, [SessionAPIToken] = @sessionAPIToken, [LastAPIOperation] = @lastAPIOperation WHERE UserId = @userId"; 
			}
			
			command.Parameters.Add (CreateParameter("@firstName", user.FirstName));
			command.Parameters.Add (CreateParameter("@lastName", user.LastName));
			command.Parameters.Add (CreateParameter("@email", user.Email));
			command.Parameters.Add (CreateParameter("@password", user.Password));
			command.Parameters.Add (CreateParameter("@passwordSalt", user.PasswordSalt));
			command.Parameters.Add (CreateParameter("@passwordExpiryDate", user.PasswordExpiryDate));
			command.Parameters.Add (CreateParameter("@isPasswordNonExpiring", user.IsPasswordNonExpiring));
			command.Parameters.Add (CreateParameter("@isEmployee", user.IsEmployee));
			command.Parameters.Add (CreateParameter("@isEngineer", user.IsEngineer));
			command.Parameters.Add (CreateParameter("@companyName", user.CompanyName));
			command.Parameters.Add (CreateParameter("@primaryBrandId", user.PrimaryBrandId));
			command.Parameters.Add (CreateParameter("@countryId", user.CountryId));
			command.Parameters.Add (CreateParameter("@phoneNumber", user.PhoneNumber));
			command.Parameters.Add (CreateParameter("@mobileNumber", user.MobileNumber));
			command.Parameters.Add (CreateParameter("@registerDate", user.RegisterDate));
			command.Parameters.Add (CreateParameter("@lastLoginDate", user.LastLoginDate));
			command.Parameters.Add (CreateParameter("@accountExpiryDate", user.AccountExpiryDate));
			command.Parameters.Add (CreateParameter("@isAccountNonExpiring", user.IsAccountNonExpiring));
			command.Parameters.Add (CreateParameter("@isSuspended", user.IsSuspended));
			command.Parameters.Add (CreateParameter("@userRoleId", user.UserRoleId));
			command.Parameters.Add (CreateParameter("@userStatusId", user.UserStatusId));
			command.Parameters.Add (CreateParameter("@useWorkflow", user.UseWorkflow));
			command.Parameters.Add (CreateParameter("@workflowId", user.WorkflowId));
			command.Parameters.Add (CreateParameter("@notes", user.Notes));
			command.Parameters.Add (CreateParameter("@isAllowedExternalAccess", user.IsAllowedExternalAccess));
			command.Parameters.Add (CreateParameter("@approverUserId", user.ApproverUserId));
			command.Parameters.Add (CreateParameter("@catalogueAssetIdList", user.CatalogueAssetIdList));
			command.Parameters.Add (CreateParameter("@isDeleted", user.IsDeleted));
			command.Parameters.Add (CreateParameter("@guid", user.Guid));
			command.Parameters.Add (CreateParameter("@enableFilePathIngestion", user.EnableFilePathIngestion));
			command.Parameters.Add (CreateParameter("@userAPIToken", user.UserAPIToken));
			command.Parameters.Add (CreateParameter("@sessionAPIToken", user.SessionAPIToken));
			command.Parameters.Add (CreateParameter("@lastAPIOperation", user.LastAPIOperation));

			if (user.IsNew) 
			{
				user.UserId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@userId", user.UserId));
				ExecuteCommand (command);
			}
			
			user.IsDirty = false;
			user.ChangedProperties.Clear();
			
			return user;
		}

		public virtual void Delete (Nullable <Int32> userId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [User] WHERE UserId = @userId";
			command.Parameters.Add(CreateParameter("@userId", userId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single User object by userId
		// </Summary>
		public virtual User Get (Nullable <Int32> userId)
		{
			IDbCommand command = GetGetCommand (userId);
			return (User) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual User FindOne (UserFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? User.Empty : entity as User;
		}
		
		public virtual EntityList <User> FindMany (UserFinder finder)
		{
			return (EntityList <User>) (base.FindMany(finder));
		}

		public virtual EntityList <User> FindMany (UserFinder finder, int Page, int PageSize)
		{
			return (EntityList <User>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> userId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_User] WHERE UserId = @userId";
			command.Parameters.Add(CreateParameter("@userId", userId)); 
			
			return command;
		}
	}
}

