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

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullUser : User
	{
		#region Singleton implementation

		private NullUser()
		{
		}

		private static readonly NullUser m_instance = new NullUser();

		public static NullUser Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the FirstName of the User object.
		/// </summary>
		public override string FirstName
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastName of the User object.
		/// </summary>
		public override string LastName
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Email of the User object.
		/// </summary>
		public override string Email
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Password of the User object.
		/// </summary>
		public override string Password
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the PasswordSalt of the User object.
		/// </summary>
		public override string PasswordSalt
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the PasswordExpiryDate of the User object.
		/// </summary>
		public override Nullable <DateTime> PasswordExpiryDate
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsPasswordNonExpiring of the User object.
		/// </summary>
		public override bool IsPasswordNonExpiring
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsEmployee of the User object.
		/// </summary>
		public override bool IsEmployee
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsEngineer of the User object.
		/// </summary>
		public override bool IsEngineer
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the CompanyName of the User object.
		/// </summary>
		public override string CompanyName
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the PrimaryBrandId of the User object.
		/// </summary>
		public override int PrimaryBrandId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CountryId of the User object.
		/// </summary>
		public override Nullable <Int32> CountryId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the PhoneNumber of the User object.
		/// </summary>
		public override string PhoneNumber
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the MobileNumber of the User object.
		/// </summary>
		public override string MobileNumber
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the RegisterDate of the User object.
		/// </summary>
		public override DateTime RegisterDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastLoginDate of the User object.
		/// </summary>
		public override Nullable <DateTime> LastLoginDate
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the AccountExpiryDate of the User object.
		/// </summary>
		public override Nullable <DateTime> AccountExpiryDate
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsAccountNonExpiring of the User object.
		/// </summary>
		public override bool IsAccountNonExpiring
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsSuspended of the User object.
		/// </summary>
		public override bool IsSuspended
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserRoleId of the User object.
		/// </summary>
		public override int UserRoleId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserStatusId of the User object.
		/// </summary>
		public override int UserStatusId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UseWorkflow of the User object.
		/// </summary>
		public override bool UseWorkflow
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the WorkflowId of the User object.
		/// </summary>
		public override Nullable <Int32> WorkflowId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the Notes of the User object.
		/// </summary>
		public override string Notes
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsAllowedExternalAccess of the User object.
		/// </summary>
		public override bool IsAllowedExternalAccess
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the ApproverUserId of the User object.
		/// </summary>
		public override Nullable <Int32> ApproverUserId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the CatalogueAssetIdList of the User object.
		/// </summary>
		public override string CatalogueAssetIdList
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDeleted of the User object.
		/// </summary>
		public override bool IsDeleted
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the Guid of the User object.
		/// </summary>
		public override string Guid
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the EnableFilePathIngestion of the User object.
		/// </summary>
		public override bool EnableFilePathIngestion
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserAPIToken of the User object.
		/// </summary>
		public override string UserAPIToken
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the SessionAPIToken of the User object.
		/// </summary>
		public override string SessionAPIToken
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastAPIOperation of the User object.
		/// </summary>
		public override Nullable <DateTime> LastAPIOperation
		{
			get { return null; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

