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
	/// <summary>
	/// This object represents the properties and methods of a OrderItem.
	/// </summary>
	public partial class OrderItem
	{
		#region Private Variables

		private readonly List<OrderItemComment> m_NewOrderItemCommentList = new List<OrderItemComment>();

		#endregion

		#region Accessors

		public OrderItemStatus OrderItemStatus
		{
			get
			{
				return EnumUtils.GetEnumFromValue<OrderItemStatus>(OrderItemStatusId);
			}
			set
			{
				OrderItemStatusId = Convert.ToInt32(value);
			}
		}

		public List<OrderItemComment> NewOrderItemCommentList
		{
			get
			{
				return m_NewOrderItemCommentList;
			}
		}

		public string LastComment
		{
			get
			{
				if (OrderItemCommentList.Count == 0)
					return string.Empty;

				return OrderItemCommentList[OrderItemCommentList.Count - 1].CommentText;
			}
		}

		#endregion

		#region Lazy loads

		private Asset m_Asset = null;
		private User m_AssignedToUser = null;
		private Order m_Order = null;
		private User m_StatusChangedByUser = null;
		private List<OrderItemComment> m_OrderItemCommentList = null;

		public Order Order
		{
			get
			{
				if (m_Order == null)
					m_Order = Order.Get(OrderId);

				return m_Order;
			}
		}

		public Asset Asset
		{
			get
			{
				if (m_Asset == null)
					m_Asset = Asset.Get(AssetId);

				return m_Asset;
			}
		}

		/// <summary>
		/// Gets the user who last changed the status of this order item
		/// </summary>
		public User StatusChangedByUser
		{
			get
			{
				if (m_StatusChangedByUser == null)
					m_StatusChangedByUser = User.Get(OrderItemStatusUserId.GetValueOrDefault());

				return m_StatusChangedByUser;
			}
		}

		/// <summary>
		/// Gets the user assigned to approving this asset
		/// </summary>
		public User AssignedToUser
		{
			get
			{
				if (m_AssignedToUser == null)
					m_AssignedToUser = User.Get(AssignedToUserId);

				return m_AssignedToUser;
			}
		}

		public List<OrderItemComment> OrderItemCommentList
		{
			get
			{
				if (m_OrderItemCommentList == null)
				{
					OrderItemCommentFinder finder = new OrderItemCommentFinder {OrderItemId = OrderItemId.GetValueOrDefault()};
					finder.SortExpressions.Add(new AscendingSort(OrderItemComment.Columns.CommentDate));
					finder.SortExpressions.Add(new AscendingSort(OrderItemComment.Columns.OrderItemCommentId));
					m_OrderItemCommentList = OrderItemComment.FindMany(finder);
				}
				return (m_OrderItemCommentList);
			}
		}

		#endregion

		public void AddComment(int userId, string comment)
		{
			if (StringUtils.IsBlank(comment))
				throw new ArgumentException("Comment cannot be blank");

			OrderItemComment orderItemComment = OrderItemComment.New();
			orderItemComment.OrderItemId = OrderItemId.GetValueOrDefault();
			orderItemComment.UserId = userId;
			orderItemComment.CommentText = comment;
			orderItemComment.CommentDate = DateTime.Now;
			NewOrderItemCommentList.Add(orderItemComment);
		}
	}
}