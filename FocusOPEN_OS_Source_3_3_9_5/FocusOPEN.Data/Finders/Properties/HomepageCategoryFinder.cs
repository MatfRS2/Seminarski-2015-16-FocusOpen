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
	public partial class HomepageCategoryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_homepageCategoryId = null;
		
		// Table columns
		protected int m_homepageId = 0;
		protected int m_categoryId = 0;
		protected int m_orderBy = 0;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_homepageCategoryIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the HomepageCategory object.
		/// </summary>
		public Nullable <Int32> HomepageCategoryId
		{
			get
			{
				return m_homepageCategoryId;
			}
			set
			{
				if (value != m_homepageCategoryId)
				{
					m_homepageCategoryId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int HomepageId
		{
			get
			{
				return m_homepageId;
			}
			set
			{
				if (value != m_homepageId)
				{
					m_homepageId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int CategoryId
		{
			get
			{
				return m_categoryId;
			}
			set
			{
				if (value != m_categoryId)
				{
					m_categoryId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int OrderBy
		{
			get
			{
				return m_orderBy;
			}
			set
			{
				if (value != m_orderBy)
				{
					m_orderBy = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> HomepageCategoryIdList
		{
			get
			{
				return m_homepageCategoryIdList;
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
				return "[HomepageCategory]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (HomepageCategoryIdList != null && HomepageCategoryIdList.Count > 0)
			{
				JoinableList list = new JoinableList(HomepageCategoryIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", HomepageCategory.Columns.HomepageCategoryId));
			}
			
			if (HomepageCategoryId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@homepageCategoryId", HomepageCategory.Columns.HomepageCategoryId));
				sb.AddDataParameter("@homepageCategoryId", HomepageCategoryId.Value);
			}
			
			if (HomepageId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@homepageId", HomepageCategory.Columns.HomepageId));
				sb.AddDataParameter("@homepageId", HomepageId);
			}
	
			if (CategoryId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@categoryId", HomepageCategory.Columns.CategoryId));
				sb.AddDataParameter("@categoryId", CategoryId);
			}
	
			if (OrderBy != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@orderBy", HomepageCategory.Columns.OrderBy));
				sb.AddDataParameter("@orderBy", OrderBy);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}