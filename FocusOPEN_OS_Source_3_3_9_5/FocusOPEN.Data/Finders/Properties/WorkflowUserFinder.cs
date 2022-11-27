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
	public partial class WorkflowUserFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_workflowUserId = null;
		
		// Table columns
		protected int m_workflowId = 0;
		protected int m_userId = 0;
		protected int m_position = 0;
		protected DateTime m_dateAdded = DateTime.MinValue;
		
		// View Variables
		protected string m_userFullName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_workflowUserIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the WorkflowUser object.
		/// </summary>
		public Nullable <Int32> WorkflowUserId
		{
			get
			{
				return m_workflowUserId;
			}
			set
			{
				if (value != m_workflowUserId)
				{
					m_workflowUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int WorkflowId
		{
			get
			{
				return m_workflowId;
			}
			set
			{
				if (value != m_workflowId)
				{
					m_workflowId = value;
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

		public int Position
		{
			get
			{
				return m_position;
			}
			set
			{
				if (value != m_position)
				{
					m_position = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime DateAdded
		{
			get
			{
				return m_dateAdded;
			}
			set
			{
				if (value != m_dateAdded)
				{
					m_dateAdded = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> WorkflowUserIdList
		{
			get
			{
				return m_workflowUserIdList;
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
		
		public virtual string UserFullName
		{
			get
			{
				return m_userFullName;
			}
			set 
			{ 
				m_userFullName = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_WorkflowUser]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (WorkflowUserIdList != null && WorkflowUserIdList.Count > 0)
			{
				JoinableList list = new JoinableList(WorkflowUserIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", WorkflowUser.Columns.WorkflowUserId));
			}
			
			if (WorkflowUserId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@workflowUserId", WorkflowUser.Columns.WorkflowUserId));
				sb.AddDataParameter("@workflowUserId", WorkflowUserId.Value);
			}
			
			if (WorkflowId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@workflowId", WorkflowUser.Columns.WorkflowId));
				sb.AddDataParameter("@workflowId", WorkflowId);
			}
	
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", WorkflowUser.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (Position != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@position", WorkflowUser.Columns.Position));
				sb.AddDataParameter("@position", Position);
			}
	
			if (DateAdded != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@dateAdded", WorkflowUser.Columns.DateAdded));
				sb.AddDataParameter("@dateAdded", DateAdded);
			}
	
			if (UserFullName != String.Empty)
			{
				sb.Criteria.Add("UserFullName=@userFullName");
				sb.AddDataParameter("@userFullName", UserFullName);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}