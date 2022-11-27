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
	public partial class OrderItemCommentFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_orderItemCommentId = null;
		
		// Table columns
		protected int m_orderItemId = 0;
		protected int m_userId = 0;
		protected string m_commentText = String.Empty;
		protected DateTime m_commentDate = DateTime.MinValue;
		
		// View Variables
		protected string m_userFullName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_orderItemCommentIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the OrderItemComment object.
		/// </summary>
		public Nullable <Int32> OrderItemCommentId
		{
			get
			{
				return m_orderItemCommentId;
			}
			set
			{
				if (value != m_orderItemCommentId)
				{
					m_orderItemCommentId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int OrderItemId
		{
			get
			{
				return m_orderItemId;
			}
			set
			{
				if (value != m_orderItemId)
				{
					m_orderItemId = value;
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

		public string CommentText
		{
			get
			{
				return m_commentText;
			}
			set
			{
				if (value != m_commentText)
				{
					m_commentText = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime CommentDate
		{
			get
			{
				return m_commentDate;
			}
			set
			{
				if (value != m_commentDate)
				{
					m_commentDate = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> OrderItemCommentIdList
		{
			get
			{
				return m_orderItemCommentIdList;
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
				return "[v_OrderItemComment]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (OrderItemCommentIdList != null && OrderItemCommentIdList.Count > 0)
			{
				JoinableList list = new JoinableList(OrderItemCommentIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", OrderItemComment.Columns.OrderItemCommentId));
			}
			
			if (OrderItemCommentId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@orderItemCommentId", OrderItemComment.Columns.OrderItemCommentId));
				sb.AddDataParameter("@orderItemCommentId", OrderItemCommentId.Value);
			}
			
			if (OrderItemId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@orderItemId", OrderItemComment.Columns.OrderItemId));
				sb.AddDataParameter("@orderItemId", OrderItemId);
			}
	
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", OrderItemComment.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (CommentText != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@commentText", OrderItemComment.Columns.CommentText));
				sb.AddDataParameter("@commentText", CommentText);
			}						
	
			if (CommentDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@commentDate", OrderItemComment.Columns.CommentDate));
				sb.AddDataParameter("@commentDate", CommentDate);
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