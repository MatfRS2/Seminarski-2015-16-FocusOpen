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
	public partial class AssetWorkflowCommenterFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetWorkflowCommenterId = null;
		
		// Table columns
		protected int m_userId = 0;
		protected int m_invitingAssetWorkflowUserId = 0;
		protected string m_invitingUserMessage = String.Empty;
		protected string m_comments = String.Empty;
		protected DateTime m_createDate = DateTime.MinValue;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		
		// View Variables
		protected Nullable <Int32> m_invitingUserId = null;
		protected Nullable <Int32> m_assetWorkflowId = null;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetWorkflowCommenterIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetWorkflowCommenter object.
		/// </summary>
		public Nullable <Int32> AssetWorkflowCommenterId
		{
			get
			{
				return m_assetWorkflowCommenterId;
			}
			set
			{
				if (value != m_assetWorkflowCommenterId)
				{
					m_assetWorkflowCommenterId = value;
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

		public int InvitingAssetWorkflowUserId
		{
			get
			{
				return m_invitingAssetWorkflowUserId;
			}
			set
			{
				if (value != m_invitingAssetWorkflowUserId)
				{
					m_invitingAssetWorkflowUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string InvitingUserMessage
		{
			get
			{
				return m_invitingUserMessage;
			}
			set
			{
				if (value != m_invitingUserMessage)
				{
					m_invitingUserMessage = value;
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
		
		public List<Int32> AssetWorkflowCommenterIdList
		{
			get
			{
				return m_assetWorkflowCommenterIdList;
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
		
		public virtual Nullable <Int32> InvitingUserId
		{
			get
			{
				return m_invitingUserId;
			}
			set 
			{ 
				m_invitingUserId = value; 
			}
		}
		public virtual Nullable <Int32> AssetWorkflowId
		{
			get
			{
				return m_assetWorkflowId;
			}
			set 
			{ 
				m_assetWorkflowId = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AssetWorkflowCommenter]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetWorkflowCommenterIdList != null && AssetWorkflowCommenterIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetWorkflowCommenterIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetWorkflowCommenter.Columns.AssetWorkflowCommenterId));
			}
			
			if (AssetWorkflowCommenterId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetWorkflowCommenterId", AssetWorkflowCommenter.Columns.AssetWorkflowCommenterId));
				sb.AddDataParameter("@assetWorkflowCommenterId", AssetWorkflowCommenterId.Value);
			}
			
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", AssetWorkflowCommenter.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (InvitingAssetWorkflowUserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@invitingAssetWorkflowUserId", AssetWorkflowCommenter.Columns.InvitingAssetWorkflowUserId));
				sb.AddDataParameter("@invitingAssetWorkflowUserId", InvitingAssetWorkflowUserId);
			}
	
			if (InvitingUserMessage != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@invitingUserMessage", AssetWorkflowCommenter.Columns.InvitingUserMessage));
				sb.AddDataParameter("@invitingUserMessage", InvitingUserMessage);
			}						
	
			if (Comments != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@comments", AssetWorkflowCommenter.Columns.Comments));
				sb.AddDataParameter("@comments", Comments);
			}						
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", AssetWorkflowCommenter.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			if (LastUpdate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lastUpdate", AssetWorkflowCommenter.Columns.LastUpdate));
				sb.AddDataParameter("@lastUpdate", LastUpdate);
			}
	
			if (InvitingUserId.HasValue)
			{
				if (InvitingUserId.Value == 0)
				{
					sb.Criteria.Add("InvitingUserId IS NULL");
				}
				else
				{
					sb.Criteria.Add("InvitingUserId=@invitingUserId");
					sb.AddDataParameter("@invitingUserId", InvitingUserId.Value);
				}
			}

			if (AssetWorkflowId.HasValue)
			{
				if (AssetWorkflowId.Value == 0)
				{
					sb.Criteria.Add("AssetWorkflowId IS NULL");
				}
				else
				{
					sb.Criteria.Add("AssetWorkflowId=@assetWorkflowId");
					sb.AddDataParameter("@assetWorkflowId", AssetWorkflowId.Value);
				}
			}

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}