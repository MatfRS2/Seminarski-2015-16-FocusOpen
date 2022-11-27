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
	public partial class OrderItem : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_orderItemId = null;
		
		// Table variables
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
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
				return (m_orderItemId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the OrderId of the OrderItem object.
		/// </summary>
		public virtual int OrderId
		{
			get
			{
				return m_orderId;
			}
			set 
			{ 
				if ((value != m_orderId))
				{
					m_ChangedProperties["OrderId"] = new ChangedProperty("OrderId", m_orderId, value);
					
					m_orderId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetId of the OrderItem object.
		/// </summary>
		public virtual int AssetId
		{
			get
			{
				return m_assetId;
			}
			set 
			{ 
				if ((value != m_assetId))
				{
					m_ChangedProperties["AssetId"] = new ChangedProperty("AssetId", m_assetId, value);
					
					m_assetId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Notes of the OrderItem object.
		/// </summary>
		public virtual string Notes
		{
			get
			{
				return m_notes;
			}
			set 
			{ 
				if ((value != m_notes))
				{
					m_ChangedProperties["Notes"] = new ChangedProperty("Notes", m_notes, value);
					
					m_notes = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the RequiredByDate of the OrderItem object.
		/// </summary>
		public virtual Nullable <DateTime> RequiredByDate
		{
			get
			{
				return m_requiredByDate;
			}
			set 
			{ 
				if ((value != m_requiredByDate))
				{
					m_ChangedProperties["RequiredByDate"] = new ChangedProperty("RequiredByDate", m_requiredByDate, value);
					
					m_requiredByDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OrderItemStatusId of the OrderItem object.
		/// </summary>
		public virtual int OrderItemStatusId
		{
			get
			{
				return m_orderItemStatusId;
			}
			set 
			{ 
				if ((value != m_orderItemStatusId))
				{
					m_ChangedProperties["OrderItemStatusId"] = new ChangedProperty("OrderItemStatusId", m_orderItemStatusId, value);
					
					m_orderItemStatusId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OrderItemStatusDate of the OrderItem object.
		/// </summary>
		public virtual Nullable <DateTime> OrderItemStatusDate
		{
			get
			{
				return m_orderItemStatusDate;
			}
			set 
			{ 
				if ((value != m_orderItemStatusDate))
				{
					m_ChangedProperties["OrderItemStatusDate"] = new ChangedProperty("OrderItemStatusDate", m_orderItemStatusDate, value);
					
					m_orderItemStatusDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OrderItemStatusUserId of the OrderItem object.
		/// </summary>
		public virtual Nullable <Int32> OrderItemStatusUserId
		{
			get
			{
				return m_orderItemStatusUserId;
			}
			set 
			{ 
				if ((value != m_orderItemStatusUserId))
				{
					m_ChangedProperties["OrderItemStatusUserId"] = new ChangedProperty("OrderItemStatusUserId", m_orderItemStatusUserId, value);
					
					m_orderItemStatusUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssignedToUserId of the OrderItem object.
		/// </summary>
		public virtual Nullable <Int32> AssignedToUserId
		{
			get
			{
				return m_assignedToUserId;
			}
			set 
			{ 
				if ((value != m_assignedToUserId))
				{
					m_ChangedProperties["AssignedToUserId"] = new ChangedProperty("AssignedToUserId", m_assignedToUserId, value);
					
					m_assignedToUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreateDate of the OrderItem object.
		/// </summary>
		public virtual DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set 
			{ 
				if ((value != m_createDate))
				{
					m_ChangedProperties["CreateDate"] = new ChangedProperty("CreateDate", m_createDate, value);
					
					m_createDate = value; 
					m_isDirty = true;
				}
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
		
		public enum Columns
		{
			OrderItemId,
			OrderId,
			AssetId,
			Notes,
			RequiredByDate,
			OrderItemStatusId,
			OrderItemStatusDate,
			OrderItemStatusUserId,
			AssignedToUserId,
			CreateDate
		}
	}
}

