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
	public partial class AssetWorkflowFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetWorkflowId = null;
		
		// Table columns
		protected int m_assetId = 0;
		protected int m_submittedByUserId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		protected Nullable <Boolean> m_isComplete = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetWorkflowIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetWorkflow object.
		/// </summary>
		public Nullable <Int32> AssetWorkflowId
		{
			get
			{
				return m_assetWorkflowId;
			}
			set
			{
				if (value != m_assetWorkflowId)
				{
					m_assetWorkflowId = value;
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

		public int SubmittedByUserId
		{
			get
			{
				return m_submittedByUserId;
			}
			set
			{
				if (value != m_submittedByUserId)
				{
					m_submittedByUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set
			{
				if (value != m_createDate)
				{
					m_createDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsComplete
		{
			get
			{
				return m_isComplete;
			}
			set
			{
				if (value != m_isComplete)
				{
					m_isComplete = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetWorkflowIdList
		{
			get
			{
				return m_assetWorkflowIdList;
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
				return "[AssetWorkflow]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetWorkflowIdList != null && AssetWorkflowIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetWorkflowIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetWorkflow.Columns.AssetWorkflowId));
			}
			
			if (AssetWorkflowId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetWorkflowId", AssetWorkflow.Columns.AssetWorkflowId));
				sb.AddDataParameter("@assetWorkflowId", AssetWorkflowId.Value);
			}
			
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AssetWorkflow.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (SubmittedByUserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@submittedByUserId", AssetWorkflow.Columns.SubmittedByUserId));
				sb.AddDataParameter("@submittedByUserId", SubmittedByUserId);
			}
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", AssetWorkflow.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			if (IsComplete.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isComplete", AssetWorkflow.Columns.IsComplete));
				sb.AddDataParameter("@isComplete", SqlUtils.BitValue(IsComplete.Value));
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}