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
	public partial class AuditAssetSearchResultFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditAssetSearchResultId = null;
		
		// Table columns
		protected int m_auditAssetSearchId = 0;
		protected int m_assetId = 0;
		protected DateTime m_date = DateTime.MinValue;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_auditAssetSearchResultIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AuditAssetSearchResult object.
		/// </summary>
		public Nullable <Int32> AuditAssetSearchResultId
		{
			get
			{
				return m_auditAssetSearchResultId;
			}
			set
			{
				if (value != m_auditAssetSearchResultId)
				{
					m_auditAssetSearchResultId = value;
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

		public int AssetId
		{
			get
			{
				return m_assetId;
			}
			set
			{
				if (value != m_assetId)
				{
					m_assetId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime Date
		{
			get
			{
				return m_date;
			}
			set
			{
				if (value != m_date)
				{
					m_date = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AuditAssetSearchResultIdList
		{
			get
			{
				return m_auditAssetSearchResultIdList;
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
				return "[AuditAssetSearchResult]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AuditAssetSearchResultIdList != null && AuditAssetSearchResultIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AuditAssetSearchResultIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AuditAssetSearchResult.Columns.AuditAssetSearchResultId));
			}
			
			if (AuditAssetSearchResultId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@auditAssetSearchResultId", AuditAssetSearchResult.Columns.AuditAssetSearchResultId));
				sb.AddDataParameter("@auditAssetSearchResultId", AuditAssetSearchResultId.Value);
			}
			
			if (AuditAssetSearchId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@auditAssetSearchId", AuditAssetSearchResult.Columns.AuditAssetSearchId));
				sb.AddDataParameter("@auditAssetSearchId", AuditAssetSearchId);
			}
	
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AuditAssetSearchResult.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (Date != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@date", AuditAssetSearchResult.Columns.Date));
				sb.AddDataParameter("@date", Date);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}