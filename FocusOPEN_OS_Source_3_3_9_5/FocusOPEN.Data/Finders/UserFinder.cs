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
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public partial class UserFinder
	{
		#region Private variables

		private int m_MinimumUserRoleId = 0;
		private int m_MaximumUserRoleId = 0;
		private bool m_ExpiredAndSuspendedAccountsOnly = false;
		private int m_BrandAdministratorId = 0;
		private string m_FuzzyEmail = string.Empty;
		private DateRange m_RegisterDateRange = DateRange.Empty;

		#endregion

		#region Accessors

		public int MinimumUserRoleId
		{
			get
			{
				return m_MinimumUserRoleId;
			}
			set
			{
				m_MinimumUserRoleId = value;
			}
		}

		public int MaximumUserRoleId
		{
			get
			{
				return m_MaximumUserRoleId;
			}
			set
			{
				m_MaximumUserRoleId = value;
			}
		}

		public bool ExpiredAndSuspendedAccountsOnly
		{
			get
			{
				return m_ExpiredAndSuspendedAccountsOnly;
			}
			set
			{
				m_ExpiredAndSuspendedAccountsOnly = value;
			}
		}

		public int BrandAdministratorId
		{
			get
			{
				return m_BrandAdministratorId;
			}
			set
			{
				m_BrandAdministratorId = value;
			}
		}

		public string FuzzyEmail
		{
			get
			{
				return m_FuzzyEmail;
			}
			set
			{
				m_FuzzyEmail = value;
			}
		}

		/// <summary>
		/// Finds users created between the specified start and end date
		/// </summary>
		public DateRange RegisterDateRange
		{
			get
			{
				return m_RegisterDateRange;
			}
			set
			{
				m_RegisterDateRange = value;
			}
		}

		public UserRole UserRole
		{
			get
			{
				return EnumUtils.GetEnumFromValue<UserRole>(UserRoleId);
			}
			set
			{
				UserRoleId = Convert.ToInt32(value);
			}
		}

		#endregion

		protected void SetCustomSearchCriteria(ref SearchBuilder sb)
		{
			sb.Table = "[v_User]";

			if (CompanyName.ToLower() == "all")
				sb.Criteria.Remove(string.Format("{0}=@companyName", User.Columns.CompanyName));

			if (MinimumUserRoleId > 0)
			{
				string sql = string.Format("{0} >= {1}", User.Columns.UserRoleId, MinimumUserRoleId);
				sb.Criteria.Add(sql);
			}

			if (MaximumUserRoleId > 0)
			{
				string sql = string.Format("{0} <= {1}", User.Columns.UserRoleId, MaximumUserRoleId);
				sb.Criteria.Add(sql);
			}

			if (FuzzyEmail != string.Empty)
			{
				string sql = string.Format("{0} LIKE '%{1}%'", User.Columns.Email, FuzzyEmail.Replace("'", "''"));
				sb.Criteria.Add(sql);
			}

			if (ExpiredAndSuspendedAccountsOnly)
			{
				sb.Criteria.Add("((IsAccountExpired = 1) OR (IsSuspended = 1))");
			}

			if (PrimaryBrandId == Int32.MinValue)
			{
				// -------------------------------------------------------------
				// Criteria checks
				// -------------------------------------------------------------
				// null				= ignore	(automatic)
				// int				= int		(automatic)
				// Int32.MinValue	= IS NULL	(handled below)
				// -------------------------------------------------------------

				sb.Criteria.Remove(string.Format("{0}=@primaryBrandId", User.Columns.PrimaryBrandId));
				sb.Criteria.Add(string.Format("{0} IS NULL", User.Columns.PrimaryBrandId));
			}

			if (!RegisterDateRange.IsNull)
			{
				if (RegisterDateRange.StartDate.HasValue)
				{
					string criteria = string.Format("{0} >= @RegisterDateRangeStartDate", User.Columns.RegisterDate);
					sb.Criteria.Add(criteria);
					sb.AddDataParameter("@RegisterDateRangeStartDate", RegisterDateRange.StartDate);
				}

				if (RegisterDateRange.EndDate.HasValue)
				{
					string criteria = string.Format("{0} <= @RegisterDateRangeEndDate", User.Columns.RegisterDate);
					sb.Criteria.Add(criteria);
					sb.AddDataParameter("@RegisterDateRangeEndDate", RegisterDateRange.EndDate);
				}
			}

			//--------------------------------------------------------------------
			// Brand Admins are restricted to seeing users in their own BU
			// or from companies they have created, so we modify the where clause
			// accordingly to reflect this.
			//--------------------------------------------------------------------
			if (BrandAdministratorId != 0)
			{
				// Ensure that the primary brand ID is specified
				if (!sb.Criteria.Contains(string.Format("{0}=@primaryBrandId", User.Columns.PrimaryBrandId)))
					throw new SystemException("PrimaryBrandId must be specified when BrandAdministratorId is specified");

				// Create list to hold criteria
				JoinableList jList = new JoinableList(" AND ");
				
				// Remove primary brand ID from SQL
				sb.Criteria.Remove(string.Format("{0}=@primaryBrandId", User.Columns.PrimaryBrandId));

				// Update with correct syntax
				jList.Add(PrimaryBrandId == 0 ? string.Format("{0} IS NULL", User.Columns.PrimaryBrandId) : string.Format("{0}={1}", User.Columns.PrimaryBrandId, PrimaryBrandId));

				// Add the criteria
				sb.Criteria.Add(jList.ToString());
			}
		}
	}
}