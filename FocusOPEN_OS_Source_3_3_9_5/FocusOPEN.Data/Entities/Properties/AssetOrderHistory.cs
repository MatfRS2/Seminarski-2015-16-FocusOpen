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
	public partial class AssetOrderHistory : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected int? m_AssetOrderHistoryId = null;

		// View Variables
		protected int? m_assetId = null;
        protected int? m_orderId = null;
        protected DateTime? m_orderDate = null;
        protected DateTime? m_deadlineDate = null;
        protected string m_userEmail = String.Empty;
		protected int? m_userId = null;
		protected string m_userName = String.Empty;
		protected string m_notes = String.Empty;
        protected DateTime? m_orderItemStatusDate = null;
        protected int? m_orderItemStatusId = null;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetOrderHistory object.
		/// </summary>
		public int? AssetOrderHistoryId
		{
			get
			{
				return m_AssetOrderHistoryId;
			}
			set 
			{
				if (value != m_AssetOrderHistoryId)
				{
					m_AssetOrderHistoryId = value;
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
				return (m_AssetOrderHistoryId.HasValue == false);
			}
		}
		
		#endregion

        #region View Accessors

        /// <summary>
		/// Sets or Returns the AssetId of the AssetOrderHistory object.
		/// </summary>
		public virtual int? AssetId
		{
			get
			{
				return m_assetId;
			}
			set 
			{ 
				m_assetId = value; 
			}
		}


        /// <summary>
		/// Sets or Returns the OrderId of the AssetOrderHistory object.
		/// </summary>
		public virtual int? OrderId
		{
			get
			{
				return m_orderId;
			}
			set 
			{ 
				m_orderId = value; 
			}
		}

   		/// <summary>
		/// Sets or Returns the OrderDate of the AssetOrderHistory object.
		/// </summary>
		public virtual DateTime? OrderDate
		{
			get
			{
				return m_orderDate;
			}
			set 
			{ 
				m_orderDate = value; 
			}
		}


        /// <summary>
		/// Sets or Returns the DeadlineDate of the AssetOrderHistory object.
		/// </summary>
		public virtual DateTime? DeadlineDate
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

        /// <summary>
		/// Sets or Returns the UserId of the AssetOrderHistory object.
		/// </summary>
		public virtual int? UserId
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
		
        /// <summary>
		/// Sets or Returns the Notes of the AssetOrderHistory object.
		/// </summary>
		public virtual string Notes
		{
			get
			{
				return m_notes;
			}
			set 
			{ 
				m_notes = value; 
			}
		}

        public virtual DateTime? OrderItemStatusDate
        {
            get
            {
                return m_orderItemStatusDate;
            }
            set
            {
                m_orderItemStatusDate = value;
            }
        }

        public virtual int? OrderItemStatusId
        {
            get
            {
                return m_orderItemStatusId;
            }
            set
            {
                m_orderItemStatusId = value;
            }
        }
		
		
		#endregion
		
		public enum Columns
		{
			AssetOrderHistoryId,
            AssetId,
            OrderId,
            OrderDate,
            DeadlineDate,
            UserEmail,
            UserId,
            UserName,
            Notes,
            OrderItemStatusDate,
            OrderItemStatusId
		}
	}
}

