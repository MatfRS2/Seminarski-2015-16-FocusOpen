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
	public partial class Order : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_orderId = null;
		
		// Table variables
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
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
					m_isDirty = true;
				}
			}
		}
		
		public Dictionary<String, ChangedProperty> ChangedProperties
		{
			get
			{
				return m_ChangedProperties;
			}
		}

		public override bool IsNew
		{
			get
			{
				return (m_orderId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the UserId of the Order object.
		/// </summary>
		public virtual int UserId
		{
			get
			{
				return m_userId;
			}
			set 
			{ 
				if ((value != m_userId))
				{
					m_ChangedProperties["UserId"] = new ChangedProperty("UserId", m_userId, value);
					
					m_userId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OrderDate of the Order object.
		/// </summary>
		public virtual DateTime OrderDate
		{
			get
			{
				return m_orderDate;
			}
			set 
			{ 
				if ((value != m_orderDate))
				{
					m_ChangedProperties["OrderDate"] = new ChangedProperty("OrderDate", m_orderDate, value);
					
					m_orderDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CompletionDate of the Order object.
		/// </summary>
		public virtual Nullable <DateTime> CompletionDate
		{
			get
			{
				return m_completionDate;
			}
			set 
			{ 
				if ((value != m_completionDate))
				{
					m_ChangedProperties["CompletionDate"] = new ChangedProperty("CompletionDate", m_completionDate, value);
					
					m_completionDate = value; 
					m_isDirty = true;
				}
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
		
		public enum Columns
		{
			OrderId,
			UserId,
			OrderDate,
			CompletionDate
		}
	}
}

