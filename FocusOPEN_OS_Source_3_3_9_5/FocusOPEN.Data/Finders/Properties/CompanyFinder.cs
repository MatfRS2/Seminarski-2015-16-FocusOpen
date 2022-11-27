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
	public partial class CompanyFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_companyId = null;
		
		// Table columns
		protected string m_name = String.Empty;
		protected string m_brands = String.Empty;
		protected string m_domain = String.Empty;
		protected Nullable <Boolean> m_isInternal = null;
		protected int m_createdByUserId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_companyIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Company object.
		/// </summary>
		public Nullable <Int32> CompanyId
		{
			get
			{
				return m_companyId;
			}
			set
			{
				if (value != m_companyId)
				{
					m_companyId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				if (value != m_name)
				{
					m_name = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Brands
		{
			get
			{
				return m_brands;
			}
			set
			{
				if (value != m_brands)
				{
					m_brands = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Domain
		{
			get
			{
				return m_domain;
			}
			set
			{
				if (value != m_domain)
				{
					m_domain = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsInternal
		{
			get
			{
				return m_isInternal;
			}
			set
			{
				if (value != m_isInternal)
				{
					m_isInternal = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int CreatedByUserId
		{
			get
			{
				return m_createdByUserId;
			}
			set
			{
				if (value != m_createdByUserId)
				{
					m_createdByUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set
			{
				if (value != m_createDate)
				{
					m_createDate = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> CompanyIdList
		{
			get
			{
				return m_companyIdList;
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
		
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_Company]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (CompanyIdList != null && CompanyIdList.Count > 0)
			{
				JoinableList list = new JoinableList(CompanyIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Company.Columns.CompanyId));
			}
			
			if (CompanyId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@companyId", Company.Columns.CompanyId));
				sb.AddDataParameter("@companyId", CompanyId.Value);
			}
			
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Company.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (Brands != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@brands", Company.Columns.Brands));
				sb.AddDataParameter("@brands", Brands);
			}						
	
			if (Domain != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@domain", Company.Columns.Domain));
				sb.AddDataParameter("@domain", Domain);
			}						
	
			if (IsInternal.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isInternal", Company.Columns.IsInternal));
				sb.AddDataParameter("@isInternal", SqlUtils.BitValue(IsInternal.Value));
			}
	
			if (CreatedByUserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@createdByUserId", Company.Columns.CreatedByUserId));
				sb.AddDataParameter("@createdByUserId", CreatedByUserId);
			}
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", Company.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}