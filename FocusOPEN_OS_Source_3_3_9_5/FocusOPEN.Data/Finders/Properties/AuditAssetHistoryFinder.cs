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
	public partial class AuditAssetHistoryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditAssetHistoryId = null;
		
		// Table columns
		protected string m_sessionId = String.Empty;
		protected string m_ipAddress = String.Empty;
		protected int m_auditAssetActionId = 0;
		protected int m_userId = 0;
		protected int m_assetId = 0;
		protected string m_notes = String.Empty;
		protected DateTime m_date = DateTime.MinValue;
		
		// View Variables
		protected string m_description = String.Empty;
		protected Nullable <Int32> m_userPrimaryBrandId = null;
		protected Nullable <Int32> m_assetBrandId = null;
		protected string m_userName = String.Empty;
		protected string m_userEmail = String.Empty;
		protected string m_title = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_auditAssetHistoryIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AuditAssetHistory object.
		/// </summary>
		public Nullable <Int32> AuditAssetHistoryId
		{
			get
			{
				return m_auditAssetHistoryId;
			}
			set
			{
				if (value != m_auditAssetHistoryId)
				{
					m_auditAssetHistoryId = value;
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

		public int AuditAssetActionId
		{
			get
			{
				return m_auditAssetActionId;
			}
			set
			{
				if (value != m_auditAssetActionId)
				{
					m_auditAssetActionId = value;
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

		public string Notes
		{
			get
			{
				return m_notes;
			}
			set
			{
				if (value != m_notes)
				{
					m_notes = value;
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
		
		public List<Int32> AuditAssetHistoryIdList
		{
			get
			{
				return m_auditAssetHistoryIdList;
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
		
		public virtual string Description
		{
			get
			{
				return m_description;
			}
			set 
			{ 
				m_description = value; 
			}
		}
		public virtual Nullable <Int32> UserPrimaryBrandId
		{
			get
			{
				return m_userPrimaryBrandId;
			}
			set 
			{ 
				m_userPrimaryBrandId = value; 
			}
		}
		public virtual Nullable <Int32> AssetBrandId
		{
			get
			{
				return m_assetBrandId;
			}
			set 
			{ 
				m_assetBrandId = value; 
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
		public virtual string UserEmail
		{
			get
			{
				return m_userEmail;
			}
			set 
			{ 
				m_userEmail = value; 
			}
		}
		public virtual string Title
		{
			get
			{
				return m_title;
			}
			set 
			{ 
				m_title = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AuditAssetHistory]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AuditAssetHistoryIdList != null && AuditAssetHistoryIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AuditAssetHistoryIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AuditAssetHistory.Columns.AuditAssetHistoryId));
			}
			
			if (AuditAssetHistoryId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@auditAssetHistoryId", AuditAssetHistory.Columns.AuditAssetHistoryId));
				sb.AddDataParameter("@auditAssetHistoryId", AuditAssetHistoryId.Value);
			}
			
			if (SessionId != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@sessionId", AuditAssetHistory.Columns.SessionId));
				sb.AddDataParameter("@sessionId", SessionId);
			}						
	
			if (IpAddress != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@ipAddress", AuditAssetHistory.Columns.IpAddress));
				sb.AddDataParameter("@ipAddress", IpAddress);
			}						
	
			if (AuditAssetActionId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@auditAssetActionId", AuditAssetHistory.Columns.AuditAssetActionId));
				sb.AddDataParameter("@auditAssetActionId", AuditAssetActionId);
			}
	
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", AuditAssetHistory.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AuditAssetHistory.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (Notes != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@notes", AuditAssetHistory.Columns.Notes));
				sb.AddDataParameter("@notes", Notes);
			}						
	
			if (Date != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@date", AuditAssetHistory.Columns.Date));
				sb.AddDataParameter("@date", Date);
			}
	
			if (Description != String.Empty)
			{
				sb.Criteria.Add("Description=@description");
				sb.AddDataParameter("@description", Description);
			}						

			if (UserPrimaryBrandId.HasValue)
			{
				if (UserPrimaryBrandId.Value == 0)
				{
					sb.Criteria.Add("UserPrimaryBrandId IS NULL");
				}
				else
				{
					sb.Criteria.Add("UserPrimaryBrandId=@userPrimaryBrandId");
					sb.AddDataParameter("@userPrimaryBrandId", UserPrimaryBrandId.Value);
				}
			}

			if (AssetBrandId.HasValue)
			{
				if (AssetBrandId.Value == 0)
				{
					sb.Criteria.Add("AssetBrandId IS NULL");
				}
				else
				{
					sb.Criteria.Add("AssetBrandId=@assetBrandId");
					sb.AddDataParameter("@assetBrandId", AssetBrandId.Value);
				}
			}

			if (UserName != String.Empty)
			{
				sb.Criteria.Add("UserName=@userName");
				sb.AddDataParameter("@userName", UserName);
			}						

			if (UserEmail != String.Empty)
			{
				sb.Criteria.Add("UserEmail=@userEmail");
				sb.AddDataParameter("@userEmail", UserEmail);
			}						

			if (Title != String.Empty)
			{
				sb.Criteria.Add("Title=@title");
				sb.AddDataParameter("@title", Title);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}