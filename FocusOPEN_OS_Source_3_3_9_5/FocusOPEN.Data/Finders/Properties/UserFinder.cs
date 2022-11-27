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
using System.Collections.Generic;
using Daydream.Data;
using FocusOPEN.Shared;
using SearchBuilder=Daydream.Data.SearchBuilder;

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class UserFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_userId = null;
		
		// Table columns
		protected string m_firstName = String.Empty;
		protected string m_lastName = String.Empty;
		protected string m_email = String.Empty;
		protected string m_password = String.Empty;
		protected string m_passwordSalt = String.Empty;
		protected Nullable <DateTime> m_passwordExpiryDate = null;
		protected Nullable <Boolean> m_isPasswordNonExpiring = null;
		protected Nullable <Boolean> m_isEmployee = null;
		protected Nullable <Boolean> m_isEngineer = null;
		protected string m_companyName = String.Empty;
		protected int m_primaryBrandId = 0;
		protected Nullable <Int32> m_countryId = null;
		protected string m_phoneNumber = String.Empty;
		protected string m_mobileNumber = String.Empty;
		protected DateTime m_registerDate = DateTime.MinValue;
		protected Nullable <DateTime> m_lastLoginDate = null;
		protected Nullable <DateTime> m_accountExpiryDate = null;
		protected Nullable <Boolean> m_isAccountNonExpiring = null;
		protected Nullable <Boolean> m_isSuspended = null;
		protected int m_userRoleId = 0;
		protected int m_userStatusId = 0;
		protected Nullable <Boolean> m_useWorkflow = null;
		protected Nullable <Int32> m_workflowId = null;
		protected string m_notes = String.Empty;
		protected Nullable <Boolean> m_isAllowedExternalAccess = null;
		protected Nullable <Int32> m_approverUserId = null;
		protected string m_catalogueAssetIdList = String.Empty;
		protected Nullable <Boolean> m_isDeleted = null;
		protected string m_guid = String.Empty;
		protected Nullable <Boolean> m_enableFilePathIngestion = null;
		protected string m_userAPIToken = String.Empty;
		protected string m_sessionAPIToken = String.Empty;
		protected Nullable <DateTime> m_lastAPIOperation = null;
		
		// View Variables
		protected string m_fullName = String.Empty;
		protected Nullable <Boolean> m_isPasswordExpired = null;
		protected Nullable <Boolean> m_isAccountExpired = null;
		protected string m_brandName = String.Empty;
		protected string m_userStatusName = String.Empty;
		protected string m_userRoleName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_userIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the User object.
		/// </summary>
		public Nullable <Int32> UserId
		{
			get
			{
				return m_userId;
			}
			set
			{
				if (value != m_userId)
				{
					m_userId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string FirstName
		{
			get
			{
				return m_firstName;
			}
			set
			{
				if (value != m_firstName)
				{
					m_firstName = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string LastName
		{
			get
			{
				return m_lastName;
			}
			set
			{
				if (value != m_lastName)
				{
					m_lastName = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Email
		{
			get
			{
				return m_email;
			}
			set
			{
				if (value != m_email)
				{
					m_email = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Password
		{
			get
			{
				return m_password;
			}
			set
			{
				if (value != m_password)
				{
					m_password = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string PasswordSalt
		{
			get
			{
				return m_passwordSalt;
			}
			set
			{
				if (value != m_passwordSalt)
				{
					m_passwordSalt = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> PasswordExpiryDate
		{
			get
			{
				return m_passwordExpiryDate;
			}
			set
			{
				if (value != m_passwordExpiryDate)
				{
					m_passwordExpiryDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsPasswordNonExpiring
		{
			get
			{
				return m_isPasswordNonExpiring;
			}
			set
			{
				if (value != m_isPasswordNonExpiring)
				{
					m_isPasswordNonExpiring = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsEmployee
		{
			get
			{
				return m_isEmployee;
			}
			set
			{
				if (value != m_isEmployee)
				{
					m_isEmployee = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsEngineer
		{
			get
			{
				return m_isEngineer;
			}
			set
			{
				if (value != m_isEngineer)
				{
					m_isEngineer = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string CompanyName
		{
			get
			{
				return m_companyName;
			}
			set
			{
				if (value != m_companyName)
				{
					m_companyName = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int PrimaryBrandId
		{
			get
			{
				return m_primaryBrandId;
			}
			set
			{
				if (value != m_primaryBrandId)
				{
					m_primaryBrandId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> CountryId
		{
			get
			{
				return m_countryId;
			}
			set
			{
				if (value != m_countryId)
				{
					m_countryId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string PhoneNumber
		{
			get
			{
				return m_phoneNumber;
			}
			set
			{
				if (value != m_phoneNumber)
				{
					m_phoneNumber = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string MobileNumber
		{
			get
			{
				return m_mobileNumber;
			}
			set
			{
				if (value != m_mobileNumber)
				{
					m_mobileNumber = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime RegisterDate
		{
			get
			{
				return m_registerDate;
			}
			set
			{
				if (value != m_registerDate)
				{
					m_registerDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> LastLoginDate
		{
			get
			{
				return m_lastLoginDate;
			}
			set
			{
				if (value != m_lastLoginDate)
				{
					m_lastLoginDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> AccountExpiryDate
		{
			get
			{
				return m_accountExpiryDate;
			}
			set
			{
				if (value != m_accountExpiryDate)
				{
					m_accountExpiryDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsAccountNonExpiring
		{
			get
			{
				return m_isAccountNonExpiring;
			}
			set
			{
				if (value != m_isAccountNonExpiring)
				{
					m_isAccountNonExpiring = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsSuspended
		{
			get
			{
				return m_isSuspended;
			}
			set
			{
				if (value != m_isSuspended)
				{
					m_isSuspended = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int UserRoleId
		{
			get
			{
				return m_userRoleId;
			}
			set
			{
				if (value != m_userRoleId)
				{
					m_userRoleId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int UserStatusId
		{
			get
			{
				return m_userStatusId;
			}
			set
			{
				if (value != m_userStatusId)
				{
					m_userStatusId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> UseWorkflow
		{
			get
			{
				return m_useWorkflow;
			}
			set
			{
				if (value != m_useWorkflow)
				{
					m_useWorkflow = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> WorkflowId
		{
			get
			{
				return m_workflowId;
			}
			set
			{
				if (value != m_workflowId)
				{
					m_workflowId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Notes
		{
			get
			{
				return m_notes;
			}
			set
			{
				if (value != m_notes)
				{
					m_notes = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsAllowedExternalAccess
		{
			get
			{
				return m_isAllowedExternalAccess;
			}
			set
			{
				if (value != m_isAllowedExternalAccess)
				{
					m_isAllowedExternalAccess = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> ApproverUserId
		{
			get
			{
				return m_approverUserId;
			}
			set
			{
				if (value != m_approverUserId)
				{
					m_approverUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string CatalogueAssetIdList
		{
			get
			{
				return m_catalogueAssetIdList;
			}
			set
			{
				if (value != m_catalogueAssetIdList)
				{
					m_catalogueAssetIdList = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set
			{
				if (value != m_isDeleted)
				{
					m_isDeleted = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Guid
		{
			get
			{
				return m_guid;
			}
			set
			{
				if (value != m_guid)
				{
					m_guid = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> EnableFilePathIngestion
		{
			get
			{
				return m_enableFilePathIngestion;
			}
			set
			{
				if (value != m_enableFilePathIngestion)
				{
					m_enableFilePathIngestion = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string UserAPIToken
		{
			get
			{
				return m_userAPIToken;
			}
			set
			{
				if (value != m_userAPIToken)
				{
					m_userAPIToken = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string SessionAPIToken
		{
			get
			{
				return m_sessionAPIToken;
			}
			set
			{
				if (value != m_sessionAPIToken)
				{
					m_sessionAPIToken = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> LastAPIOperation
		{
			get
			{
				return m_lastAPIOperation;
			}
			set
			{
				if (value != m_lastAPIOperation)
				{
					m_lastAPIOperation = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> UserIdList
		{
			get
			{
				return m_userIdList;
			}
		}
		
		public int FindCriteriaCount
		{
			get
			{
				return m_FindCriteriaCount;
			}
		}
		
		#endregion
		
		#region View Accessors
		
		public virtual string FullName
		{
			get
			{
				return m_fullName;
			}
			set 
			{ 
				m_fullName = value; 
			}
		}
		public virtual Nullable <Boolean> IsPasswordExpired
		{
			get
			{
				return m_isPasswordExpired;
			}
			set 
			{ 
				m_isPasswordExpired = value; 
			}
		}
		public virtual Nullable <Boolean> IsAccountExpired
		{
			get
			{
				return m_isAccountExpired;
			}
			set 
			{ 
				m_isAccountExpired = value; 
			}
		}
		public virtual string BrandName
		{
			get
			{
				return m_brandName;
			}
			set 
			{ 
				m_brandName = value; 
			}
		}
		public virtual string UserStatusName
		{
			get
			{
				return m_userStatusName;
			}
			set 
			{ 
				m_userStatusName = value; 
			}
		}
		public virtual string UserRoleName
		{
			get
			{
				return m_userRoleName;
			}
			set 
			{ 
				m_userRoleName = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_User]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (UserIdList != null && UserIdList.Count > 0)
			{
				JoinableList list = new JoinableList(UserIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", User.Columns.UserId));
			}
			
			if (UserId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", User.Columns.UserId));
				sb.AddDataParameter("@userId", UserId.Value);
			}
			
			if (FirstName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@firstName", User.Columns.FirstName));
				sb.AddDataParameter("@firstName", FirstName);
			}						
	
			if (LastName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@lastName", User.Columns.LastName));
				sb.AddDataParameter("@lastName", LastName);
			}						
	
			if (Email != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@email", User.Columns.Email));
				sb.AddDataParameter("@email", Email);
			}						
	
			if (Password != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@password", User.Columns.Password));
				sb.AddDataParameter("@password", Password);
			}						
	
			if (PasswordSalt != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@passwordSalt", User.Columns.PasswordSalt));
				sb.AddDataParameter("@passwordSalt", PasswordSalt);
			}						
	
			if (PasswordExpiryDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@passwordExpiryDate", User.Columns.PasswordExpiryDate));
				sb.AddDataParameter("@passwordExpiryDate", PasswordExpiryDate.Value);
			}
	
			if (IsPasswordNonExpiring.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isPasswordNonExpiring", User.Columns.IsPasswordNonExpiring));
				sb.AddDataParameter("@isPasswordNonExpiring", SqlUtils.BitValue(IsPasswordNonExpiring.Value));
			}
	
			if (IsEmployee.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isEmployee", User.Columns.IsEmployee));
				sb.AddDataParameter("@isEmployee", SqlUtils.BitValue(IsEmployee.Value));
			}
	
			if (IsEngineer.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isEngineer", User.Columns.IsEngineer));
				sb.AddDataParameter("@isEngineer", SqlUtils.BitValue(IsEngineer.Value));
			}
	
			if (CompanyName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@companyName", User.Columns.CompanyName));
				sb.AddDataParameter("@companyName", CompanyName);
			}						
	
			if (PrimaryBrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@primaryBrandId", User.Columns.PrimaryBrandId));
				sb.AddDataParameter("@primaryBrandId", PrimaryBrandId);
			}
	
			if (CountryId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@countryId", User.Columns.CountryId));
				sb.AddDataParameter("@countryId", CountryId.Value);
			}
	
			if (PhoneNumber != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@phoneNumber", User.Columns.PhoneNumber));
				sb.AddDataParameter("@phoneNumber", PhoneNumber);
			}						
	
			if (MobileNumber != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@mobileNumber", User.Columns.MobileNumber));
				sb.AddDataParameter("@mobileNumber", MobileNumber);
			}						
	
			if (RegisterDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@registerDate", User.Columns.RegisterDate));
				sb.AddDataParameter("@registerDate", RegisterDate);
			}
	
			if (LastLoginDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@lastLoginDate", User.Columns.LastLoginDate));
				sb.AddDataParameter("@lastLoginDate", LastLoginDate.Value);
			}
	
			if (AccountExpiryDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@accountExpiryDate", User.Columns.AccountExpiryDate));
				sb.AddDataParameter("@accountExpiryDate", AccountExpiryDate.Value);
			}
	
			if (IsAccountNonExpiring.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isAccountNonExpiring", User.Columns.IsAccountNonExpiring));
				sb.AddDataParameter("@isAccountNonExpiring", SqlUtils.BitValue(IsAccountNonExpiring.Value));
			}
	
			if (IsSuspended.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isSuspended", User.Columns.IsSuspended));
				sb.AddDataParameter("@isSuspended", SqlUtils.BitValue(IsSuspended.Value));
			}
	
			if (UserRoleId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userRoleId", User.Columns.UserRoleId));
				sb.AddDataParameter("@userRoleId", UserRoleId);
			}
	
			if (UserStatusId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userStatusId", User.Columns.UserStatusId));
				sb.AddDataParameter("@userStatusId", UserStatusId);
			}
	
			if (UseWorkflow.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@useWorkflow", User.Columns.UseWorkflow));
				sb.AddDataParameter("@useWorkflow", SqlUtils.BitValue(UseWorkflow.Value));
			}
	
			if (WorkflowId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@workflowId", User.Columns.WorkflowId));
				sb.AddDataParameter("@workflowId", WorkflowId.Value);
			}
	
			if (Notes != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@notes", User.Columns.Notes));
				sb.AddDataParameter("@notes", Notes);
			}						
	
			if (IsAllowedExternalAccess.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isAllowedExternalAccess", User.Columns.IsAllowedExternalAccess));
				sb.AddDataParameter("@isAllowedExternalAccess", SqlUtils.BitValue(IsAllowedExternalAccess.Value));
			}
	
			if (ApproverUserId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@approverUserId", User.Columns.ApproverUserId));
				sb.AddDataParameter("@approverUserId", ApproverUserId.Value);
			}
	
			if (CatalogueAssetIdList != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@catalogueAssetIdList", User.Columns.CatalogueAssetIdList));
				sb.AddDataParameter("@catalogueAssetIdList", CatalogueAssetIdList);
			}						
	
			if (IsDeleted.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDeleted", User.Columns.IsDeleted));
				sb.AddDataParameter("@isDeleted", SqlUtils.BitValue(IsDeleted.Value));
			}
	
			if (Guid != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@guid", User.Columns.Guid));
				sb.AddDataParameter("@guid", Guid);
			}						
	
			if (EnableFilePathIngestion.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@enableFilePathIngestion", User.Columns.EnableFilePathIngestion));
				sb.AddDataParameter("@enableFilePathIngestion", SqlUtils.BitValue(EnableFilePathIngestion.Value));
			}
	
			if (UserAPIToken != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@userAPIToken", User.Columns.UserAPIToken));
				sb.AddDataParameter("@userAPIToken", UserAPIToken);
			}						
	
			if (SessionAPIToken != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@sessionAPIToken", User.Columns.SessionAPIToken));
				sb.AddDataParameter("@sessionAPIToken", SessionAPIToken);
			}						
	
			if (LastAPIOperation != null)
			{
				sb.Criteria.Add(string.Format("{0}=@lastAPIOperation", User.Columns.LastAPIOperation));
				sb.AddDataParameter("@lastAPIOperation", LastAPIOperation.Value);
			}
	
			if (FullName != String.Empty)
			{
				sb.Criteria.Add("FullName=@fullName");
				sb.AddDataParameter("@fullName", FullName);
			}						

			if (IsPasswordExpired.HasValue)
			{
				sb.Criteria.Add("IsPasswordExpired=@isPasswordExpired");
				sb.AddDataParameter("@isPasswordExpired", SqlUtils.BitValue(IsPasswordExpired.Value));
			}

			if (IsAccountExpired.HasValue)
			{
				sb.Criteria.Add("IsAccountExpired=@isAccountExpired");
				sb.AddDataParameter("@isAccountExpired", SqlUtils.BitValue(IsAccountExpired.Value));
			}

			if (BrandName != String.Empty)
			{
				sb.Criteria.Add("BrandName=@brandName");
				sb.AddDataParameter("@brandName", BrandName);
			}						

			if (UserStatusName != String.Empty)
			{
				sb.Criteria.Add("UserStatusName=@userStatusName");
				sb.AddDataParameter("@userStatusName", UserStatusName);
			}						

			if (UserRoleName != String.Empty)
			{
				sb.Criteria.Add("UserRoleName=@userRoleName");
				sb.AddDataParameter("@userRoleName", UserRoleName);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}