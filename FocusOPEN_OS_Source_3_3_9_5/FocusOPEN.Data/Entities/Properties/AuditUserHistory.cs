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
	/// This object represents the properties and methods of a AuditUserHistory.
	/// </summary>
	[Serializable]
	public partial class AuditUserHistory : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditUserHistoryId = null;
		
		// Table variables
		protected string m_sessionId = String.Empty;
		protected string m_ipAddress = String.Empty;
		protected int m_auditUserActionId = 0;
		protected int m_userId = 0;
		protected string m_notes = String.Empty;
		protected DateTime m_date = DateTime.MinValue;
		
		// View Variables
		protected string m_description = String.Empty;
		protected string m_userName = String.Empty;
		protected string m_userEmail = String.Empty;
		protected Nullable <Int32> m_primaryBrandId = null;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AuditUserHistory object.
		/// </summary>
		public Nullable <Int32> AuditUserHistoryId
		{
			get
			{
				return m_auditUserHistoryId;
			}
			set 
			{
				if (value != m_auditUserHistoryId)
				{
					m_auditUserHistoryId = value;
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
				return (m_auditUserHistoryId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the SessionId of the AuditUserHistory object.
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
		/// Sets or Returns the IpAddress of the AuditUserHistory object.
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
		/// Sets or Returns the AuditUserActionId of the AuditUserHistory object.
		/// </summary>
		public virtual int AuditUserActionId
		{
			get
			{
				return m_auditUserActionId;
			}
			set 
			{ 
				if ((value != m_auditUserActionId))
				{
					m_ChangedProperties["AuditUserActionId"] = new ChangedProperty("AuditUserActionId", m_auditUserActionId, value);
					
					m_auditUserActionId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserId of the AuditUserHistory object.
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
		/// Sets or Returns the Notes of the AuditUserHistory object.
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
		/// Sets or Returns the Date of the AuditUserHistory object.
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
		public virtual Nullable <Int32> PrimaryBrandId
		{
			get
			{
				return m_primaryBrandId;
			}
			set 
			{ 
				m_primaryBrandId = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			AuditUserHistoryId,
			SessionId,
			IpAddress,
			AuditUserActionId,
			UserId,
			Notes,
			Date
		}
	}
}

