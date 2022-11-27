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
	public partial class WorkflowFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_workflowId = null;
		
		// Table columns
		protected string m_name = String.Empty;
		protected int m_brandId = 0;
		protected Nullable <Boolean> m_isDeleted = null;
		
		// View Variables
		protected string m_brandName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_workflowIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Workflow object.
		/// </summary>
		public Nullable <Int32> WorkflowId
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

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				if (value != m_name)
				{
					m_name = value;
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

		public Nullable <Boolean> IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set
			{
				if (value != m_isDeleted)
				{
					m_isDeleted = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> WorkflowIdList
		{
			get
			{
				return m_workflowIdList;
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
		
		public virtual string BrandName
		{
			get
			{
				return m_brandName;
			}
			set 
			{ 
				m_brandName = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_Workflow]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (WorkflowIdList != null && WorkflowIdList.Count > 0)
			{
				JoinableList list = new JoinableList(WorkflowIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Workflow.Columns.WorkflowId));
			}
			
			if (WorkflowId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@workflowId", Workflow.Columns.WorkflowId));
				sb.AddDataParameter("@workflowId", WorkflowId.Value);
			}
			
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Workflow.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", Workflow.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			if (IsDeleted.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDeleted", Workflow.Columns.IsDeleted));
				sb.AddDataParameter("@isDeleted", SqlUtils.BitValue(IsDeleted.Value));
			}
	
			if (BrandName != String.Empty)
			{
				sb.Criteria.Add("BrandName=@brandName");
				sb.AddDataParameter("@brandName", BrandName);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}