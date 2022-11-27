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
	/// This object represents the properties and methods of a Lightbox.
	/// </summary>
	[Serializable]
	public partial class Lightbox : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxId = null;
		
		// Table variables
		protected int m_userId = 0;
		protected string m_name = String.Empty;
		protected string m_summary = String.Empty;
		protected string m_notes = String.Empty;
		protected bool m_isPublic = false;
		protected bool m_isDefault = false;
		protected DateTime m_createDate = DateTime.MinValue;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Lightbox object.
		/// </summary>
		public Nullable <Int32> LightboxId
		{
			get
			{
				return m_lightboxId;
			}
			set 
			{
				if (value != m_lightboxId)
				{
					m_lightboxId = value;
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
				return (m_lightboxId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the UserId of the Lightbox object.
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
		/// Sets or Returns the Name of the Lightbox object.
		/// </summary>
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set 
			{ 
				if ((value != m_name))
				{
					m_ChangedProperties["Name"] = new ChangedProperty("Name", m_name, value);
					
					m_name = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Summary of the Lightbox object.
		/// </summary>
		public virtual string Summary
		{
			get
			{
				return m_summary;
			}
			set 
			{ 
				if ((value != m_summary))
				{
					m_ChangedProperties["Summary"] = new ChangedProperty("Summary", m_summary, value);
					
					m_summary = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Notes of the Lightbox object.
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
		/// Sets or Returns the IsPublic of the Lightbox object.
		/// </summary>
		public virtual bool IsPublic
		{
			get
			{
				return m_isPublic;
			}
			set 
			{ 
				if ((value != m_isPublic))
				{
					m_ChangedProperties["IsPublic"] = new ChangedProperty("IsPublic", m_isPublic, value);
					
					m_isPublic = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDefault of the Lightbox object.
		/// </summary>
		public virtual bool IsDefault
		{
			get
			{
				return m_isDefault;
			}
			set 
			{ 
				if ((value != m_isDefault))
				{
					m_ChangedProperties["IsDefault"] = new ChangedProperty("IsDefault", m_isDefault, value);
					
					m_isDefault = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreateDate of the Lightbox object.
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
		
		
		public enum Columns
		{
			LightboxId,
			UserId,
			Name,
			Summary,
			Notes,
			IsPublic,
			IsDefault,
			CreateDate
		}
	}
}

