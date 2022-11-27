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
	public partial class UserBrandFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_userBrandId = null;
		
		// Table columns
		protected int m_userId = 0;
		protected int m_brandId = 0;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_userBrandIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the UserBrand object.
		/// </summary>
		public Nullable <Int32> UserBrandId
		{
			get
			{
				return m_userBrandId;
			}
			set
			{
				if (value != m_userBrandId)
				{
					m_userBrandId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int UserId
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
		
		public List<Int32> UserBrandIdList
		{
			get
			{
				return m_userBrandIdList;
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
				return "[UserBrand]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (UserBrandIdList != null && UserBrandIdList.Count > 0)
			{
				JoinableList list = new JoinableList(UserBrandIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", UserBrand.Columns.UserBrandId));
			}
			
			if (UserBrandId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@userBrandId", UserBrand.Columns.UserBrandId));
				sb.AddDataParameter("@userBrandId", UserBrandId.Value);
			}
			
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", UserBrand.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", UserBrand.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}