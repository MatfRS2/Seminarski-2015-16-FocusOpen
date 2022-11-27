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
	public partial class AuditAssetSearchKeywordFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditAssetSearchKeywordId = null;
		
		// Table columns
		protected int m_auditAssetSearchId = 0;
		protected string m_searchKeyword = String.Empty;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_auditAssetSearchKeywordIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AuditAssetSearchKeyword object.
		/// </summary>
		public Nullable <Int32> AuditAssetSearchKeywordId
		{
			get
			{
				return m_auditAssetSearchKeywordId;
			}
			set
			{
				if (value != m_auditAssetSearchKeywordId)
				{
					m_auditAssetSearchKeywordId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AuditAssetSearchId
		{
			get
			{
				return m_auditAssetSearchId;
			}
			set
			{
				if (value != m_auditAssetSearchId)
				{
					m_auditAssetSearchId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string SearchKeyword
		{
			get
			{
				return m_searchKeyword;
			}
			set
			{
				if (value != m_searchKeyword)
				{
					m_searchKeyword = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AuditAssetSearchKeywordIdList
		{
			get
			{
				return m_auditAssetSearchKeywordIdList;
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
				return "[AuditAssetSearchKeyword]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AuditAssetSearchKeywordIdList != null && AuditAssetSearchKeywordIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AuditAssetSearchKeywordIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AuditAssetSearchKeyword.Columns.AuditAssetSearchKeywordId));
			}
			
			if (AuditAssetSearchKeywordId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@auditAssetSearchKeywordId", AuditAssetSearchKeyword.Columns.AuditAssetSearchKeywordId));
				sb.AddDataParameter("@auditAssetSearchKeywordId", AuditAssetSearchKeywordId.Value);
			}
			
			if (AuditAssetSearchId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@auditAssetSearchId", AuditAssetSearchKeyword.Columns.AuditAssetSearchId));
				sb.AddDataParameter("@auditAssetSearchId", AuditAssetSearchId);
			}
	
			if (SearchKeyword != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@searchKeyword", AuditAssetSearchKeyword.Columns.SearchKeyword));
				sb.AddDataParameter("@searchKeyword", SearchKeyword);
			}						
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}