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
    /// This object represents the properties and methods of a LightboxSent.
    /// </summary>
	[Serializable]
	public partial class LightboxSent : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxSentId = null;
		
		// Table variables
		protected int m_lightboxId = 0;
		protected Nullable <Int32> m_createdLightboxId = null;
		protected int m_senderId = 0;
		protected string m_recipientEmail = String.Empty;
		protected string m_subject = String.Empty;
		protected string m_message = String.Empty;
		protected DateTime m_dateSent = DateTime.MinValue;
		protected Nullable <DateTime> m_expiryDate = null;
		protected Nullable <Boolean> m_downloadLinks = null;
		protected Nullable <Int32> m_lightboxLinkedId = null;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxSent object.
		/// </summary>
		public Nullable <Int32> LightboxSentId
		{
			get
			{
				return m_lightboxSentId;
			}
			set 
			{
				if (value != m_lightboxSentId)
				{
					m_lightboxSentId = value;
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
				return (m_lightboxSentId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the LightboxId of the LightboxSent object.
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
		/// Sets or Returns the CreatedLightboxId of the LightboxSent object.
		/// </summary>
		public virtual Nullable <Int32> CreatedLightboxId
		{
			get
			{
				return m_createdLightboxId;
			}
			set 
			{ 
				if (value != m_createdLightboxId)
				{
					m_ChangedProperties["CreatedLightboxId"] = new ChangedProperty("CreatedLightboxId", m_createdLightboxId, value);
					
					m_createdLightboxId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the SenderId of the LightboxSent object.
		/// </summary>
		public virtual int SenderId
		{
			get
			{
				return m_senderId;
			}
			set 
			{ 
				if (value != m_senderId)
				{
					m_ChangedProperties["SenderId"] = new ChangedProperty("SenderId", m_senderId, value);
					
					m_senderId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the RecipientEmail of the LightboxSent object.
		/// </summary>
		public virtual string RecipientEmail
		{
			get
			{
				return m_recipientEmail;
			}
			set 
			{ 
				if (value != m_recipientEmail)
				{
					m_ChangedProperties["RecipientEmail"] = new ChangedProperty("RecipientEmail", m_recipientEmail, value);
					
					m_recipientEmail = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Subject of the LightboxSent object.
		/// </summary>
		public virtual string Subject
		{
			get
			{
				return m_subject;
			}
			set 
			{ 
				if (value != m_subject)
				{
					m_ChangedProperties["Subject"] = new ChangedProperty("Subject", m_subject, value);
					
					m_subject = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Message of the LightboxSent object.
		/// </summary>
		public virtual string Message
		{
			get
			{
				return m_message;
			}
			set 
			{ 
				if (value != m_message)
				{
					m_ChangedProperties["Message"] = new ChangedProperty("Message", m_message, value);
					
					m_message = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DateSent of the LightboxSent object.
		/// </summary>
		public virtual DateTime DateSent
		{
			get
			{
				return m_dateSent;
			}
			set 
			{ 
				if (value != m_dateSent)
				{
					m_ChangedProperties["DateSent"] = new ChangedProperty("DateSent", m_dateSent, value);
					
					m_dateSent = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ExpiryDate of the LightboxSent object.
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
		/// Sets or Returns the DownloadLinks of the LightboxSent object.
		/// </summary>
		public virtual Nullable <Boolean> DownloadLinks
		{
			get
			{
				return m_downloadLinks;
			}
			set 
			{ 
				if (value != m_downloadLinks)
				{
					m_ChangedProperties["DownloadLinks"] = new ChangedProperty("DownloadLinks", m_downloadLinks, value);
					
					m_downloadLinks = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LightboxLinkedId of the LightboxSent object.
		/// </summary>
		public virtual Nullable <Int32> LightboxLinkedId
		{
			get
			{
				return m_lightboxLinkedId;
			}
			set 
			{ 
				if (value != m_lightboxLinkedId)
				{
					m_ChangedProperties["LightboxLinkedId"] = new ChangedProperty("LightboxLinkedId", m_lightboxLinkedId, value);
					
					m_lightboxLinkedId = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			LightboxSentId,
			LightboxId,
			CreatedLightboxId,
			SenderId,
			RecipientEmail,
			Subject,
			Message,
			DateSent,
			ExpiryDate,
			DownloadLinks,
			LightboxLinkedId
		}
	}
}

