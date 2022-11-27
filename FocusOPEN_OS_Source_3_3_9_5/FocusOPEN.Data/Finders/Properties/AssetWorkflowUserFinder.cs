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
	public partial class AssetWorkflowUserFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetWorkflowUserId = null;
		
		// Table columns
		protected int m_assetWorkflowId = 0;
		protected int m_userId = 0;
		protected int m_position = 0;
		protected string m_comments = String.Empty;
		protected int m_assetWorkflowUserStatusId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		protected Nullable <Boolean> m_isDeleted = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetWorkflowUserIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetWorkflowUser object.
		/// </summary>
		public Nullable <Int32> AssetWorkflowUserId
		{
			get
			{
				return m_assetWorkflowUserId;
			}
			set
			{
				if (value != m_assetWorkflowUserId)
				{
					m_assetWorkflowUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AssetWorkflowId
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

		public string Comments
		{
			get
			{
				return m_comments;
			}
			set
			{
				if (value != m_comments)
				{
					m_comments = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AssetWorkflowUserStatusId
		{
			get
			{
				return m_assetWorkflowUserStatusId;
			}
			set
			{
				if (value != m_assetWorkflowUserStatusId)
				{
					m_assetWorkflowUserStatusId = value;
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

		public DateTime LastUpdate
		{
			get
			{
				return m_lastUpdate;
			}
			set
			{
				if (value != m_lastUpdate)
				{
					m_lastUpdate = value;
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
		
		public List<Int32> AssetWorkflowUserIdList
		{
			get
			{
				return m_assetWorkflowUserIdList;
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
				return "[AssetWorkflowUser]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetWorkflowUserIdList != null && AssetWorkflowUserIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetWorkflowUserIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetWorkflowUser.Columns.AssetWorkflowUserId));
			}
			
			if (AssetWorkflowUserId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetWorkflowUserId", AssetWorkflowUser.Columns.AssetWorkflowUserId));
				sb.AddDataParameter("@assetWorkflowUserId", AssetWorkflowUserId.Value);
			}
			
			if (AssetWorkflowId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetWorkflowId", AssetWorkflowUser.Columns.AssetWorkflowId));
				sb.AddDataParameter("@assetWorkflowId", AssetWorkflowId);
			}
	
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", AssetWorkflowUser.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (Position != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@position", AssetWorkflowUser.Columns.Position));
				sb.AddDataParameter("@position", Position);
			}
	
			if (Comments != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@comments", AssetWorkflowUser.Columns.Comments));
				sb.AddDataParameter("@comments", Comments);
			}						
	
			if (AssetWorkflowUserStatusId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetWorkflowUserStatusId", AssetWorkflowUser.Columns.AssetWorkflowUserStatusId));
				sb.AddDataParameter("@assetWorkflowUserStatusId", AssetWorkflowUserStatusId);
			}
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", AssetWorkflowUser.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			if (LastUpdate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lastUpdate", AssetWorkflowUser.Columns.LastUpdate));
				sb.AddDataParameter("@lastUpdate", LastUpdate);
			}
	
			if (IsDeleted.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDeleted", AssetWorkflowUser.Columns.IsDeleted));
				sb.AddDataParameter("@isDeleted", SqlUtils.BitValue(IsDeleted.Value));
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}