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
	public partial class CompanyBrandFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_companyBrandId = null;
		
		// Table columns
		protected int m_companyId = 0;
		protected int m_brandId = 0;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_companyBrandIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the CompanyBrand object.
		/// </summary>
		public Nullable <Int32> CompanyBrandId
		{
			get
			{
				return m_companyBrandId;
			}
			set
			{
				if (value != m_companyBrandId)
				{
					m_companyBrandId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int CompanyId
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

		public int BrandId
		{
			get
			{
				return m_brandId;
			}
			set
			{
				if (value != m_brandId)
				{
					m_brandId = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> CompanyBrandIdList
		{
			get
			{
				return m_companyBrandIdList;
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

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[CompanyBrand]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (CompanyBrandIdList != null && CompanyBrandIdList.Count > 0)
			{
				JoinableList list = new JoinableList(CompanyBrandIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", CompanyBrand.Columns.CompanyBrandId));
			}
			
			if (CompanyBrandId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@companyBrandId", CompanyBrand.Columns.CompanyBrandId));
				sb.AddDataParameter("@companyBrandId", CompanyBrandId.Value);
			}
			
			if (CompanyId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@companyId", CompanyBrand.Columns.CompanyId));
				sb.AddDataParameter("@companyId", CompanyId);
			}
	
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", CompanyBrand.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}