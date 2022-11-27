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
	public partial class AuditAssetHistory : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditAssetHistoryId = null;
		
		// Table variables
		protected string m_sessionId = String.Empty;
		protected string m_ipAddress = String.Empty;
		protected int m_auditAssetActionId = 0;
		protected int m_userId = 0;
		protected int m_assetId = 0;
		protected string m_notes = String.Empty;
		protected DateTime m_date = DateTime.MinValue;
		
		// View Variables
		protected string m_description = String.Empty;
		protected Nullable <Int32> m_userPrimaryBrandId = null;
		protected Nullable <Int32> m_assetBrandId = null;
		protected string m_userName = String.Empty;
		protected string m_userEmail = String.Empty;
		protected string m_title = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AuditAssetHistory object.
		/// </summary>
		public Nullable <Int32> AuditAssetHistoryId
		{
			get
			{
				return m_auditAssetHistoryId;
			}
			set 
			{
				if (value != m_auditAssetHistoryId)
				{
					m_auditAssetHistoryId = value;
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
				return (m_auditAssetHistoryId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the SessionId of the AuditAssetHistory object.
		/// </summary>
		public virtual string SessionId
		{
			get
			{
				return m_sessionId;
			}
			set 
			{ 
				if ((value != m_sessionId))
				{
					m_ChangedProperties["SessionId"] = new ChangedProperty("SessionId", m_sessionId, value);
					
					m_sessionId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IpAddress of the AuditAssetHistory object.
		/// </summary>
		public virtual string IpAddress
		{
			get
			{
				return m_ipAddress;
			}
			set 
			{ 
				if ((value != m_ipAddress))
				{
					m_ChangedProperties["IpAddress"] = new ChangedProperty("IpAddress", m_ipAddress, value);
					
					m_ipAddress = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AuditAssetActionId of the AuditAssetHistory object.
		/// </summary>
		public virtual int AuditAssetActionId
		{
			get
			{
				return m_auditAssetActionId;
			}
			set 
			{ 
				if ((value != m_auditAssetActionId))
				{
					m_ChangedProperties["AuditAssetActionId"] = new ChangedProperty("AuditAssetActionId", m_auditAssetActionId, value);
					
					m_auditAssetActionId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserId of the AuditAssetHistory object.
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
		/// Sets or Returns the AssetId of the AuditAssetHistory object.
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
		/// Sets or Returns the Notes of the AuditAssetHistory object.
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
		/// Sets or Returns the Date of the AuditAssetHistory object.
		/// </summary>
		public virtual DateTime Date
		{
			get
			{
				return m_date;
			}
			set 
			{ 
				if ((value != m_date))
				{
					m_ChangedProperties["Date"] = new ChangedProperty("Date", m_date, value);
					
					m_date = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual string Description
		{
			get
			{
				return m_description;
			}
			set 
			{ 
				m_description = value; 
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
		public virtual string Title
		{
			get
			{
				return m_title;
			}
			set 
			{ 
				m_title = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			AuditAssetHistoryId,
			SessionId,
			IpAddress,
			AuditAssetActionId,
			UserId,
			AssetId,
			Notes,
			Date
		}
	}
}

