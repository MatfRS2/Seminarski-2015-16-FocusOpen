/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
	public partial class LightboxLinked : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxLinkedId = null;
		
		// Table variables
		protected int m_lightboxId = 0;
		protected int m_userId = 0;
		protected Nullable <Boolean> m_isEditable = null;
		protected Nullable <DateTime> m_expiryDate = null;
		protected Nullable <Boolean> m_disabled = null;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxLinked object.
		/// </summary>
		public Nullable <Int32> LightboxLinkedId
		{
			get
			{
				return m_lightboxLinkedId;
			}
			set 
			{
				if (value != m_lightboxLinkedId)
				{
					m_lightboxLinkedId = value;
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
				return (m_lightboxLinkedId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the LightboxId of the LightboxLinked object.
		/// </summary>
		public virtual int LightboxId
		{
			get
			{
				return m_lightboxId;
			}
			set 
			{ 
				if (value != m_lightboxId)
				{
					m_ChangedProperties["LightboxId"] = new ChangedProperty("LightboxId", m_lightboxId, value);
					
					m_lightboxId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserId of the LightboxLinked object.
		/// </summary>
		public virtual int UserId
		{
			get
			{
				return m_userId;
			}
			set 
			{ 
				if (value != m_userId)
				{
					m_ChangedProperties["UserId"] = new ChangedProperty("UserId", m_userId, value);
					
					m_userId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsEditable of the LightboxLinked object.
		/// </summary>
		public virtual Nullable <Boolean> IsEditable
		{
			get
			{
				return m_isEditable;
			}
			set 
			{ 
				if (value != m_isEditable)
				{
					m_ChangedProperties["IsEditable"] = new ChangedProperty("IsEditable", m_isEditable, value);
					
					m_isEditable = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ExpiryDate of the LightboxLinked object.
		/// </summary>
		public virtual Nullable <DateTime> ExpiryDate
		{
			get
			{
				return m_expiryDate;
			}
			set 
			{ 
				if (value != m_expiryDate)
				{
					m_ChangedProperties["ExpiryDate"] = new ChangedProperty("ExpiryDate", m_expiryDate, value);
					
					m_expiryDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Disabled of the LightboxLinked object.
		/// </summary>
		public virtual Nullable <Boolean> Disabled
		{
			get
			{
				return m_disabled;
			}
			set 
			{ 
				if (value != m_disabled)
				{
					m_ChangedProperties["Disabled"] = new ChangedProperty("Disabled", m_disabled, value);
					
					m_disabled = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			LightboxLinkedId,
			LightboxId,
			UserId,
			IsEditable,
			ExpiryDate,
			Disabled
		}
	}
}

