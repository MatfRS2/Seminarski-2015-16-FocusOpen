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

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class User : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_userId = null;
		
		// Table variables
		protected string m_firstName = String.Empty;
		protected string m_lastName = String.Empty;
		protected string m_email = String.Empty;
		protected string m_password = String.Empty;
		protected string m_passwordSalt = String.Empty;
		protected Nullable <DateTime> m_passwordExpiryDate = null;
		protected bool m_isPasswordNonExpiring = false;
		protected bool m_isEmployee = false;
		protected bool m_isEngineer = false;
		protected string m_companyName = String.Empty;
		protected int m_primaryBrandId = 0;
		protected Nullable <Int32> m_countryId = null;
		protected string m_phoneNumber = String.Empty;
		protected string m_mobileNumber = String.Empty;
		protected DateTime m_registerDate = DateTime.MinValue;
		protected Nullable <DateTime> m_lastLoginDate = null;
		protected Nullable <DateTime> m_accountExpiryDate = null;
		protected bool m_isAccountNonExpiring = false;
		protected bool m_isSuspended = false;
		protected int m_userRoleId = 0;
		protected int m_userStatusId = 0;
		protected bool m_useWorkflow = false;
		protected Nullable <Int32> m_workflowId = null;
		protected string m_notes = String.Empty;
		protected bool m_isAllowedExternalAccess = false;
		protected Nullable <Int32> m_approverUserId = null;
		protected string m_catalogueAssetIdList = String.Empty;
		protected bool m_isDeleted = false;
		protected string m_guid = String.Empty;
		protected bool m_enableFilePathIngestion = false;
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
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
					m_isDirty = true;
				}
			}
		}
		
		public Dictionary<String, ChangedProperty> ChangedProperties
		{
			get
			{
				return m_ChangedProperties;
			}
		}

		public override bool IsNew
		{
			get
			{
				return (m_userId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the FirstName of the User object.
		/// </summary>
		public virtual string FirstName
		{
			get
			{
				return m_firstName;
			}
			set 
			{ 
				if (value != m_firstName)
				{
					m_ChangedProperties["FirstName"] = new ChangedProperty("FirstName", m_firstName, value);
					
					m_firstName = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastName of the User object.
		/// </summary>
		public virtual string LastName
		{
			get
			{
				return m_lastName;
			}
			set 
			{ 
				if (value != m_lastName)
				{
					m_ChangedProperties["LastName"] = new ChangedProperty("LastName", m_lastName, value);
					
					m_lastName = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Email of the User object.
		/// </summary>
		public virtual string Email
		{
			get
			{
				return m_email;
			}
			set 
			{ 
				if (value != m_email)
				{
					m_ChangedProperties["Email"] = new ChangedProperty("Email", m_email, value);
					
					m_email = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Password of the User object.
		/// </summary>
		public virtual string Password
		{
			get
			{
				return m_password;
			}
			set 
			{ 
				if (value != m_password)
				{
					m_ChangedProperties["Password"] = new ChangedProperty("Password", m_password, value);
					
					m_password = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PasswordSalt of the User object.
		/// </summary>
		public virtual string PasswordSalt
		{
			get
			{
				return m_passwordSalt;
			}
			set 
			{ 
				if (value != m_passwordSalt)
				{
					m_ChangedProperties["PasswordSalt"] = new ChangedProperty("PasswordSalt", m_passwordSalt, value);
					
					m_passwordSalt = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PasswordExpiryDate of the User object.
		/// </summary>
		public virtual Nullable <DateTime> PasswordExpiryDate
		{
			get
			{
				return m_passwordExpiryDate;
			}
			set 
			{ 
				if (value != m_passwordExpiryDate)
				{
					m_ChangedProperties["PasswordExpiryDate"] = new ChangedProperty("PasswordExpiryDate", m_passwordExpiryDate, value);
					
					m_passwordExpiryDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsPasswordNonExpiring of the User object.
		/// </summary>
		public virtual bool IsPasswordNonExpiring
		{
			get
			{
				return m_isPasswordNonExpiring;
			}
			set 
			{ 
				if (value != m_isPasswordNonExpiring)
				{
					m_ChangedProperties["IsPasswordNonExpiring"] = new ChangedProperty("IsPasswordNonExpiring", m_isPasswordNonExpiring, value);
					
					m_isPasswordNonExpiring = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsEmployee of the User object.
		/// </summary>
		public virtual bool IsEmployee
		{
			get
			{
				return m_isEmployee;
			}
			set 
			{ 
				if (value != m_isEmployee)
				{
					m_ChangedProperties["IsEmployee"] = new ChangedProperty("IsEmployee", m_isEmployee, value);
					
					m_isEmployee = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsEngineer of the User object.
		/// </summary>
		public virtual bool IsEngineer
		{
			get
			{
				return m_isEngineer;
			}
			set 
			{ 
				if (value != m_isEngineer)
				{
					m_ChangedProperties["IsEngineer"] = new ChangedProperty("IsEngineer", m_isEngineer, value);
					
					m_isEngineer = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CompanyName of the User object.
		/// </summary>
		public virtual string CompanyName
		{
			get
			{
				return m_companyName;
			}
			set 
			{ 
				if (value != m_companyName)
				{
					m_ChangedProperties["CompanyName"] = new ChangedProperty("CompanyName", m_companyName, value);
					
					m_companyName = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PrimaryBrandId of the User object.
		/// </summary>
		public virtual int PrimaryBrandId
		{
			get
			{
				return m_primaryBrandId;
			}
			set 
			{ 
				if (value != m_primaryBrandId)
				{
					m_ChangedProperties["PrimaryBrandId"] = new ChangedProperty("PrimaryBrandId", m_primaryBrandId, value);
					
					m_primaryBrandId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CountryId of the User object.
		/// </summary>
		public virtual Nullable <Int32> CountryId
		{
			get
			{
				return m_countryId;
			}
			set 
			{ 
				if (value != m_countryId)
				{
					m_ChangedProperties["CountryId"] = new ChangedProperty("CountryId", m_countryId, value);
					
					m_countryId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PhoneNumber of the User object.
		/// </summary>
		public virtual string PhoneNumber
		{
			get
			{
				return m_phoneNumber;
			}
			set 
			{ 
				if (value != m_phoneNumber)
				{
					m_ChangedProperties["PhoneNumber"] = new ChangedProperty("PhoneNumber", m_phoneNumber, value);
					
					m_phoneNumber = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the MobileNumber of the User object.
		/// </summary>
		public virtual string MobileNumber
		{
			get
			{
				return m_mobileNumber;
			}
			set 
			{ 
				if (value != m_mobileNumber)
				{
					m_ChangedProperties["MobileNumber"] = new ChangedProperty("MobileNumber", m_mobileNumber, value);
					
					m_mobileNumber = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the RegisterDate of the User object.
		/// </summary>
		public virtual DateTime RegisterDate
		{
			get
			{
				return m_registerDate;
			}
			set 
			{ 
				if (value != m_registerDate)
				{
					m_ChangedProperties["RegisterDate"] = new ChangedProperty("RegisterDate", m_registerDate, value);
					
					m_registerDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastLoginDate of the User object.
		/// </summary>
		public virtual Nullable <DateTime> LastLoginDate
		{
			get
			{
				return m_lastLoginDate;
			}
			set 
			{ 
				if (value != m_lastLoginDate)
				{
					m_ChangedProperties["LastLoginDate"] = new ChangedProperty("LastLoginDate", m_lastLoginDate, value);
					
					m_lastLoginDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AccountExpiryDate of the User object.
		/// </summary>
		public virtual Nullable <DateTime> AccountExpiryDate
		{
			get
			{
				return m_accountExpiryDate;
			}
			set 
			{ 
				if (value != m_accountExpiryDate)
				{
					m_ChangedProperties["AccountExpiryDate"] = new ChangedProperty("AccountExpiryDate", m_accountExpiryDate, value);
					
					m_accountExpiryDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsAccountNonExpiring of the User object.
		/// </summary>
		public virtual bool IsAccountNonExpiring
		{
			get
			{
				return m_isAccountNonExpiring;
			}
			set 
			{ 
				if (value != m_isAccountNonExpiring)
				{
					m_ChangedProperties["IsAccountNonExpiring"] = new ChangedProperty("IsAccountNonExpiring", m_isAccountNonExpiring, value);
					
					m_isAccountNonExpiring = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsSuspended of the User object.
		/// </summary>
		public virtual bool IsSuspended
		{
			get
			{
				return m_isSuspended;
			}
			set 
			{ 
				if (value != m_isSuspended)
				{
					m_ChangedProperties["IsSuspended"] = new ChangedProperty("IsSuspended", m_isSuspended, value);
					
					m_isSuspended = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserRoleId of the User object.
		/// </summary>
		public virtual int UserRoleId
		{
			get
			{
				return m_userRoleId;
			}
			set 
			{ 
				if (value != m_userRoleId)
				{
					m_ChangedProperties["UserRoleId"] = new ChangedProperty("UserRoleId", m_userRoleId, value);
					
					m_userRoleId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserStatusId of the User object.
		/// </summary>
		public virtual int UserStatusId
		{
			get
			{
				return m_userStatusId;
			}
			set 
			{ 
				if (value != m_userStatusId)
				{
					m_ChangedProperties["UserStatusId"] = new ChangedProperty("UserStatusId", m_userStatusId, value);
					
					m_userStatusId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UseWorkflow of the User object.
		/// </summary>
		public virtual bool UseWorkflow
		{
			get
			{
				return m_useWorkflow;
			}
			set 
			{ 
				if (value != m_useWorkflow)
				{
					m_ChangedProperties["UseWorkflow"] = new ChangedProperty("UseWorkflow", m_useWorkflow, value);
					
					m_useWorkflow = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the WorkflowId of the User object.
		/// </summary>
		public virtual Nullable <Int32> WorkflowId
		{
			get
			{
				return m_workflowId;
			}
			set 
			{ 
				if (value != m_workflowId)
				{
					m_ChangedProperties["WorkflowId"] = new ChangedProperty("WorkflowId", m_workflowId, value);
					
					m_workflowId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Notes of the User object.
		/// </summary>
		public virtual string Notes
		{
			get
			{
				return m_notes;
			}
			set 
			{ 
				if (value != m_notes)
				{
					m_ChangedProperties["Notes"] = new ChangedProperty("Notes", m_notes, value);
					
					m_notes = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsAllowedExternalAccess of the User object.
		/// </summary>
		public virtual bool IsAllowedExternalAccess
		{
			get
			{
				return m_isAllowedExternalAccess;
			}
			set 
			{ 
				if (value != m_isAllowedExternalAccess)
				{
					m_ChangedProperties["IsAllowedExternalAccess"] = new ChangedProperty("IsAllowedExternalAccess", m_isAllowedExternalAccess, value);
					
					m_isAllowedExternalAccess = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ApproverUserId of the User object.
		/// </summary>
		public virtual Nullable <Int32> ApproverUserId
		{
			get
			{
				return m_approverUserId;
			}
			set 
			{ 
				if (value != m_approverUserId)
				{
					m_ChangedProperties["ApproverUserId"] = new ChangedProperty("ApproverUserId", m_approverUserId, value);
					
					m_approverUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CatalogueAssetIdList of the User object.
		/// </summary>
		public virtual string CatalogueAssetIdList
		{
			get
			{
				return m_catalogueAssetIdList;
			}
			set 
			{ 
				if (value != m_catalogueAssetIdList)
				{
					m_ChangedProperties["CatalogueAssetIdList"] = new ChangedProperty("CatalogueAssetIdList", m_catalogueAssetIdList, value);
					
					m_catalogueAssetIdList = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDeleted of the User object.
		/// </summary>
		public virtual bool IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set 
			{ 
				if (value != m_isDeleted)
				{
					m_ChangedProperties["IsDeleted"] = new ChangedProperty("IsDeleted", m_isDeleted, value);
					
					m_isDeleted = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Guid of the User object.
		/// </summary>
		public virtual string Guid
		{
			get
			{
				return m_guid;
			}
			set 
			{ 
				if (value != m_guid)
				{
					m_ChangedProperties["Guid"] = new ChangedProperty("Guid", m_guid, value);
					
					m_guid = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the EnableFilePathIngestion of the User object.
		/// </summary>
		public virtual bool EnableFilePathIngestion
		{
			get
			{
				return m_enableFilePathIngestion;
			}
			set 
			{ 
				if (value != m_enableFilePathIngestion)
				{
					m_ChangedProperties["EnableFilePathIngestion"] = new ChangedProperty("EnableFilePathIngestion", m_enableFilePathIngestion, value);
					
					m_enableFilePathIngestion = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserAPIToken of the User object.
		/// </summary>
		public virtual string UserAPIToken
		{
			get
			{
				return m_userAPIToken;
			}
			set 
			{ 
				if (value != m_userAPIToken)
				{
					m_ChangedProperties["UserAPIToken"] = new ChangedProperty("UserAPIToken", m_userAPIToken, value);
					
					m_userAPIToken = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the SessionAPIToken of the User object.
		/// </summary>
		public virtual string SessionAPIToken
		{
			get
			{
				return m_sessionAPIToken;
			}
			set 
			{ 
				if (value != m_sessionAPIToken)
				{
					m_ChangedProperties["SessionAPIToken"] = new ChangedProperty("SessionAPIToken", m_sessionAPIToken, value);
					
					m_sessionAPIToken = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastAPIOperation of the User object.
		/// </summary>
		public virtual Nullable <DateTime> LastAPIOperation
		{
			get
			{
				return m_lastAPIOperation;
			}
			set 
			{ 
				if (value != m_lastAPIOperation)
				{
					m_ChangedProperties["LastAPIOperation"] = new ChangedProperty("LastAPIOperation", m_lastAPIOperation, value);
					
					m_lastAPIOperation = value; 
					m_isDirty = true;
				}
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
		
		public enum Columns
		{
			UserId,
			FirstName,
			LastName,
			Email,
			Password,
			PasswordSalt,
			PasswordExpiryDate,
			IsPasswordNonExpiring,
			IsEmployee,
			IsEngineer,
			CompanyName,
			PrimaryBrandId,
			CountryId,
			PhoneNumber,
			MobileNumber,
			RegisterDate,
			LastLoginDate,
			AccountExpiryDate,
			IsAccountNonExpiring,
			IsSuspended,
			UserRoleId,
			UserStatusId,
			UseWorkflow,
			WorkflowId,
			Notes,
			IsAllowedExternalAccess,
			ApproverUserId,
			CatalogueAssetIdList,
			IsDeleted,
			Guid,
			EnableFilePathIngestion,
			UserAPIToken,
			SessionAPIToken,
			LastAPIOperation
		}
	}
}

