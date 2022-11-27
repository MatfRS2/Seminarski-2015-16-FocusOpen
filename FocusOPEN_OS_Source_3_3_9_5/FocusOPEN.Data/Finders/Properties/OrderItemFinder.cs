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
	public partial class OrderItemFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_orderItemId = null;
		
		// Table columns
		protected int m_orderId = 0;
		protected int m_assetId = 0;
		protected string m_notes = String.Empty;
		protected Nullable <DateTime> m_requiredByDate = null;
		protected int m_orderItemStatusId = 0;
		protected Nullable <DateTime> m_orderItemStatusDate = null;
		protected Nullable <Int32> m_orderItemStatusUserId = null;
		protected Nullable <Int32> m_assignedToUserId = null;
		protected DateTime m_createDate = DateTime.MinValue;
		
		// View Variables
		protected Nullable <Int32> m_userId = null;
		protected Nullable <Int32> m_assetUploadUserId = null;
		protected Nullable <Int32> m_assetBrandId = null;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_orderItemIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the OrderItem object.
		/// </summary>
		public Nullable <Int32> OrderItemId
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

		public int OrderId
		{
			get
			{
				return m_orderId;
			}
			set
			{
				if (value != m_orderId)
				{
					m_orderId = value;
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

		public Nullable <DateTime> RequiredByDate
		{
			get
			{
				return m_requiredByDate;
			}
			set
			{
				if (value != m_requiredByDate)
				{
					m_requiredByDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int OrderItemStatusId
		{
			get
			{
				return m_orderItemStatusId;
			}
			set
			{
				if (value != m_orderItemStatusId)
				{
					m_orderItemStatusId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> OrderItemStatusDate
		{
			get
			{
				return m_orderItemStatusDate;
			}
			set
			{
				if (value != m_orderItemStatusDate)
				{
					m_orderItemStatusDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> OrderItemStatusUserId
		{
			get
			{
				return m_orderItemStatusUserId;
			}
			set
			{
				if (value != m_orderItemStatusUserId)
				{
					m_orderItemStatusUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> AssignedToUserId
		{
			get
			{
				return m_assignedToUserId;
			}
			set
			{
				if (value != m_assignedToUserId)
				{
					m_assignedToUserId = value;
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
		
		public List<Int32> OrderItemIdList
		{
			get
			{
				return m_orderItemIdList;
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
		
		public virtual Nullable <Int32> UserId
		{
			get
			{
				return m_userId;
			}
			set 
			{ 
				m_userId = value; 
			}
		}
		public virtual Nullable <Int32> AssetUploadUserId
		{
			get
			{
				return m_assetUploadUserId;
			}
			set 
			{ 
				m_assetUploadUserId = value; 
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
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_OrderItem]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (OrderItemIdList != null && OrderItemIdList.Count > 0)
			{
				JoinableList list = new JoinableList(OrderItemIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", OrderItem.Columns.OrderItemId));
			}
			
			if (OrderItemId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@orderItemId", OrderItem.Columns.OrderItemId));
				sb.AddDataParameter("@orderItemId", OrderItemId.Value);
			}
			
			if (OrderId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@orderId", OrderItem.Columns.OrderId));
				sb.AddDataParameter("@orderId", OrderId);
			}
	
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", OrderItem.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (Notes != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@notes", OrderItem.Columns.Notes));
				sb.AddDataParameter("@notes", Notes);
			}						
	
			if (RequiredByDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@requiredByDate", OrderItem.Columns.RequiredByDate));
				sb.AddDataParameter("@requiredByDate", RequiredByDate.Value);
			}
	
			if (OrderItemStatusId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@orderItemStatusId", OrderItem.Columns.OrderItemStatusId));
				sb.AddDataParameter("@orderItemStatusId", OrderItemStatusId);
			}
	
			if (OrderItemStatusDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@orderItemStatusDate", OrderItem.Columns.OrderItemStatusDate));
				sb.AddDataParameter("@orderItemStatusDate", OrderItemStatusDate.Value);
			}
	
			if (OrderItemStatusUserId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@orderItemStatusUserId", OrderItem.Columns.OrderItemStatusUserId));
				sb.AddDataParameter("@orderItemStatusUserId", OrderItemStatusUserId.Value);
			}
	
			if (AssignedToUserId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@assignedToUserId", OrderItem.Columns.AssignedToUserId));
				sb.AddDataParameter("@assignedToUserId", AssignedToUserId.Value);
			}
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", OrderItem.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			if (UserId.HasValue)
			{
				if (UserId.Value == 0)
				{
					sb.Criteria.Add("UserId IS NULL");
				}
				else
				{
					sb.Criteria.Add("UserId=@userId");
					sb.AddDataParameter("@userId", UserId.Value);
				}
			}

			if (AssetUploadUserId.HasValue)
			{
				if (AssetUploadUserId.Value == 0)
				{
					sb.Criteria.Add("AssetUploadUserId IS NULL");
				}
				else
				{
					sb.Criteria.Add("AssetUploadUserId=@assetUploadUserId");
					sb.AddDataParameter("@assetUploadUserId", AssetUploadUserId.Value);
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

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}