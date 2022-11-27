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
	public partial class AuditAssetSearchFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditAssetSearchId = null;
		
		// Table columns
		protected string m_sessionId = String.Empty;
		protected string m_ipAddress = String.Empty;
		protected int m_userId = 0;
		protected DateTime m_date = DateTime.MinValue;
		
		// View Variables
		protected string m_searchKeyword = String.Empty;
		protected Nullable <Int32> m_assetId = null;
		protected string m_userName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_auditAssetSearchIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AuditAssetSearch object.
		/// </summary>
		public Nullable <Int32> AuditAssetSearchId
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

		public string SessionId
		{
			get
			{
				return m_sessionId;
			}
			set
			{
				if (value != m_sessionId)
				{
					m_sessionId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string IpAddress
		{
			get
			{
				return m_ipAddress;
			}
			set
			{
				if (value != m_ipAddress)
				{
					m_ipAddress = value;
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
		
		public List<Int32> AuditAssetSearchIdList
		{
			get
			{
				return m_auditAssetSearchIdList;
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
		
		public virtual string SearchKeyword
		{
			get
			{
				return m_searchKeyword;
			}
			set 
			{ 
				m_searchKeyword = value; 
			}
		}
		public virtual Nullable <Int32> AssetId
		{
			get
			{
				return m_assetId;
			}
			set 
			{ 
				m_assetId = value; 
			}
		}
		public virtual string UserName
		{
			get
			{
				return m_userName;
			}
			set 
			{ 
				m_userName = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AuditAssetSearch]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AuditAssetSearchIdList != null && AuditAssetSearchIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AuditAssetSearchIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AuditAssetSearch.Columns.AuditAssetSearchId));
			}
			
			if (AuditAssetSearchId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@auditAssetSearchId", AuditAssetSearch.Columns.AuditAssetSearchId));
				sb.AddDataParameter("@auditAssetSearchId", AuditAssetSearchId.Value);
			}
			
			if (SessionId != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@sessionId", AuditAssetSearch.Columns.SessionId));
				sb.AddDataParameter("@sessionId", SessionId);
			}						
	
			if (IpAddress != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@ipAddress", AuditAssetSearch.Columns.IpAddress));
				sb.AddDataParameter("@ipAddress", IpAddress);
			}						
	
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", AuditAssetSearch.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (Date != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@date", AuditAssetSearch.Columns.Date));
				sb.AddDataParameter("@date", Date);
			}
	
			if (SearchKeyword != String.Empty)
			{
				sb.Criteria.Add("SearchKeyword=@searchKeyword");
				sb.AddDataParameter("@searchKeyword", SearchKeyword);
			}						

			if (AssetId.HasValue)
			{
				if (AssetId.Value == 0)
				{
					sb.Criteria.Add("AssetId IS NULL");
				}
				else
				{
					sb.Criteria.Add("AssetId=@assetId");
					sb.AddDataParameter("@assetId", AssetId.Value);
				}
			}

			if (UserName != String.Empty)
			{
				sb.Criteria.Add("UserName=@userName");
				sb.AddDataParameter("@userName", UserName);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}