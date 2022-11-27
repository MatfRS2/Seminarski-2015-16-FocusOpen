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
	public partial class OrderFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_orderId = null;
		
		// Table columns
		protected int m_userId = 0;
		protected DateTime m_orderDate = DateTime.MinValue;
		protected Nullable <DateTime> m_completionDate = null;
		
		// View Variables
		protected string m_userName = String.Empty;
		protected string m_userEmail = String.Empty;
		protected Nullable <Int32> m_userPrimaryBrandId = null;
		protected string m_userPrimaryBrandName = String.Empty;
		protected Nullable <Int32> m_assetCount = null;
		protected Nullable <Boolean> m_isPending = null;
		protected Nullable <DateTime> m_deadlineDate = null;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_orderIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Order object.
		/// </summary>
		public Nullable <Int32> OrderId
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

		public DateTime OrderDate
		{
			get
			{
				return m_orderDate;
			}
			set
			{
				if (value != m_orderDate)
				{
					m_orderDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> CompletionDate
		{
			get
			{
				return m_completionDate;
			}
			set
			{
				if (value != m_completionDate)
				{
					m_completionDate = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> OrderIdList
		{
			get
			{
				return m_orderIdList;
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
		public virtual string UserPrimaryBrandName
		{
			get
			{
				return m_userPrimaryBrandName;
			}
			set 
			{ 
				m_userPrimaryBrandName = value; 
			}
		}
		public virtual Nullable <Int32> AssetCount
		{
			get
			{
				return m_assetCount;
			}
			set 
			{ 
				m_assetCount = value; 
			}
		}
		public virtual Nullable <Boolean> IsPending
		{
			get
			{
				return m_isPending;
			}
			set 
			{ 
				m_isPending = value; 
			}
		}
		public virtual Nullable <DateTime> DeadlineDate
		{
			get
			{
				return m_deadlineDate;
			}
			set 
			{ 
				m_deadlineDate = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_Order]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (OrderIdList != null && OrderIdList.Count > 0)
			{
				JoinableList list = new JoinableList(OrderIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Order.Columns.OrderId));
			}
			
			if (OrderId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@orderId", Order.Columns.OrderId));
				sb.AddDataParameter("@orderId", OrderId.Value);
			}
			
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", Order.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (OrderDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@orderDate", Order.Columns.OrderDate));
				sb.AddDataParameter("@orderDate", OrderDate);
			}
	
			if (CompletionDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@completionDate", Order.Columns.CompletionDate));
				sb.AddDataParameter("@completionDate", CompletionDate.Value);
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

			if (UserPrimaryBrandName != String.Empty)
			{
				sb.Criteria.Add("UserPrimaryBrandName=@userPrimaryBrandName");
				sb.AddDataParameter("@userPrimaryBrandName", UserPrimaryBrandName);
			}						

			if (AssetCount.HasValue)
			{
				if (AssetCount.Value == 0)
				{
					sb.Criteria.Add("AssetCount IS NULL");
				}
				else
				{
					sb.Criteria.Add("AssetCount=@assetCount");
					sb.AddDataParameter("@assetCount", AssetCount.Value);
				}
			}

			if (IsPending.HasValue)
			{
				sb.Criteria.Add("IsPending=@isPending");
				sb.AddDataParameter("@isPending", SqlUtils.BitValue(IsPending.Value));
			}

			if (DeadlineDate != null)
			{
				sb.Criteria.Add("DeadlineDate=@deadlineDate");
				sb.AddDataParameter("@deadlineDate", DeadlineDate.Value);
			}

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}