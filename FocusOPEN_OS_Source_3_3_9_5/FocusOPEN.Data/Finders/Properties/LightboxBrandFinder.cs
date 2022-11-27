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
	public partial class LightboxBrandFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxBrandId = null;
		
		// Table columns
		protected int m_lightboxId = 0;
		protected int m_brandId = 0;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_lightboxBrandIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxBrand object.
		/// </summary>
		public Nullable <Int32> LightboxBrandId
		{
			get
			{
				return m_lightboxBrandId;
			}
			set
			{
				if (value != m_lightboxBrandId)
				{
					m_lightboxBrandId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int LightboxId
		{
			get
			{
				return m_lightboxId;
			}
			set
			{
				if (value != m_lightboxId)
				{
					m_lightboxId = value;
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
		
		public List<Int32> LightboxBrandIdList
		{
			get
			{
				return m_lightboxBrandIdList;
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
				return "[LightboxBrand]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (LightboxBrandIdList != null && LightboxBrandIdList.Count > 0)
			{
				JoinableList list = new JoinableList(LightboxBrandIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", LightboxBrand.Columns.LightboxBrandId));
			}
			
			if (LightboxBrandId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxBrandId", LightboxBrand.Columns.LightboxBrandId));
				sb.AddDataParameter("@lightboxBrandId", LightboxBrandId.Value);
			}
			
			if (LightboxId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxId", LightboxBrand.Columns.LightboxId));
				sb.AddDataParameter("@lightboxId", LightboxId);
			}
	
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", LightboxBrand.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}