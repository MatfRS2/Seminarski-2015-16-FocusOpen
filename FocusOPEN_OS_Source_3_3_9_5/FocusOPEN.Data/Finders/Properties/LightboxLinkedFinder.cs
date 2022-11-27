/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
	public partial class LightboxLinkedFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxLinkedId = null;
		
		// Table columns
		protected int m_lightboxId = 0;
		protected int m_userId = 0;
		protected Nullable <Boolean> m_isEditable = null;
		protected Nullable <DateTime> m_expiryDate = null;
		protected Nullable <Boolean> m_disabled = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_lightboxLinkedIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxLinked object.
		/// </summary>
		public Nullable <Int32> LightboxLinkedId
		{
			get
			{
				return m_lightboxLinkedId;
			}
			set
			{
				if (value != m_lightboxLinkedId)
				{
					m_lightboxLinkedId = value;
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

		public Nullable <Boolean> IsEditable
		{
			get
			{
				return m_isEditable;
			}
			set
			{
				if (value != m_isEditable)
				{
					m_isEditable = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> ExpiryDate
		{
			get
			{
				return m_expiryDate;
			}
			set
			{
				if (value != m_expiryDate)
				{
					m_expiryDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> Disabled
		{
			get
			{
				return m_disabled;
			}
			set
			{
				if (value != m_disabled)
				{
					m_disabled = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> LightboxLinkedIdList
		{
			get
			{
				return m_lightboxLinkedIdList;
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
				return "[LightboxLinked]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (LightboxLinkedIdList != null && LightboxLinkedIdList.Count > 0)
			{
				JoinableList list = new JoinableList(LightboxLinkedIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", LightboxLinked.Columns.LightboxLinkedId));
			}
			
			if (LightboxLinkedId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxLinkedId", LightboxLinked.Columns.LightboxLinkedId));
				sb.AddDataParameter("@lightboxLinkedId", LightboxLinkedId.Value);
			}
			
			if (LightboxId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxId", LightboxLinked.Columns.LightboxId));
				sb.AddDataParameter("@lightboxId", LightboxId);
			}
	
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", LightboxLinked.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (IsEditable.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isEditable", LightboxLinked.Columns.IsEditable));
				sb.AddDataParameter("@isEditable", SqlUtils.BitValue(IsEditable.Value));
			}
	
			if (ExpiryDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@expiryDate", LightboxLinked.Columns.ExpiryDate));
				sb.AddDataParameter("@expiryDate", ExpiryDate.Value);
			}
	
			if (Disabled.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@disabled", LightboxLinked.Columns.Disabled));
				sb.AddDataParameter("@disabled", SqlUtils.BitValue(Disabled.Value));
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}