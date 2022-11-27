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
	public partial class AuditUserHistoryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditUserHistoryId = null;
		
		// Table columns
		protected string m_sessionId = String.Empty;
		protected string m_ipAddress = String.Empty;
		protected int m_auditUserActionId = 0;
		protected int m_userId = 0;
		protected string m_notes = String.Empty;
		protected DateTime m_date = DateTime.MinValue;
		
		// View Variables
		protected string m_description = String.Empty;
		protected string m_userName = String.Empty;
		protected string m_userEmail = String.Empty;
		protected Nullable <Int32> m_primaryBrandId = null;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_auditUserHistoryIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AuditUserHistory object.
		/// </summary>
		public Nullable <Int32> AuditUserHistoryId
		{
			get
			{
				return m_auditUserHistoryId;
			}
			set
			{
				if (value != m_auditUserHistoryId)
				{
					m_auditUserHistoryId = value;
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

		public int AuditUserActionId
		{
			get
			{
				return m_auditUserActionId;
			}
			set
			{
				if (value != m_auditUserActionId)
				{
					m_auditUserActionId = value;
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
		
		public List<Int32> AuditUserHistoryIdList
		{
			get
			{
				return m_auditUserHistoryIdList;
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
		public virtual Nullable <Int32> PrimaryBrandId
		{
			get
			{
				return m_primaryBrandId;
			}
			set 
			{ 
				m_primaryBrandId = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AuditUserHistory]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AuditUserHistoryIdList != null && AuditUserHistoryIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AuditUserHistoryIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AuditUserHistory.Columns.AuditUserHistoryId));
			}
			
			if (AuditUserHistoryId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@auditUserHistoryId", AuditUserHistory.Columns.AuditUserHistoryId));
				sb.AddDataParameter("@auditUserHistoryId", AuditUserHistoryId.Value);
			}
			
			if (SessionId != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@sessionId", AuditUserHistory.Columns.SessionId));
				sb.AddDataParameter("@sessionId", SessionId);
			}						
	
			if (IpAddress != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@ipAddress", AuditUserHistory.Columns.IpAddress));
				sb.AddDataParameter("@ipAddress", IpAddress);
			}						
	
			if (AuditUserActionId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@auditUserActionId", AuditUserHistory.Columns.AuditUserActionId));
				sb.AddDataParameter("@auditUserActionId", AuditUserActionId);
			}
	
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", AuditUserHistory.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (Notes != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@notes", AuditUserHistory.Columns.Notes));
				sb.AddDataParameter("@notes", Notes);
			}						
	
			if (Date != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@date", AuditUserHistory.Columns.Date));
				sb.AddDataParameter("@date", Date);
			}
	
			if (Description != String.Empty)
			{
				sb.Criteria.Add("Description=@description");
				sb.AddDataParameter("@description", Description);
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

			if (PrimaryBrandId.HasValue)
			{
				if (PrimaryBrandId.Value == 0)
				{
					sb.Criteria.Add("PrimaryBrandId IS NULL");
				}
				else
				{
					sb.Criteria.Add("PrimaryBrandId=@primaryBrandId");
					sb.AddDataParameter("@primaryBrandId", PrimaryBrandId.Value);
				}
			}

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}