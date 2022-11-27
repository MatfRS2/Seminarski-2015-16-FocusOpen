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
	/// This object represents the properties and methods of a Homepage.
	/// </summary>
	[Serializable]
	public partial class Homepage : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_homepageId = null;
		
		// Table variables
		protected int m_brandId = 0;
		protected string m_introText = String.Empty;
		protected string m_url1 = String.Empty;
		protected string m_url2 = String.Empty;
		protected string m_url3 = String.Empty;
		protected string m_url4 = String.Empty;
		protected string m_bumperPageHtml = String.Empty;
		protected bool m_bumperPageSkip = false;
		protected string m_customHtml = String.Empty;
		protected int m_homepageTypeId = 0;
		protected bool m_isPublished = false;
		protected int m_lastModifiedByUserId = 0;
		protected DateTime m_lastModifiedDate = DateTime.MinValue;
		
		// View Variables
		protected string m_brandName = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Homepage object.
		/// </summary>
		public Nullable <Int32> HomepageId
		{
			get
			{
				return m_homepageId;
			}
			set 
			{
				if (value != m_homepageId)
				{
					m_homepageId = value;
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
				return (m_homepageId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the BrandId of the Homepage object.
		/// </summary>
		public virtual int BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{ 
				if ((value != m_brandId))
				{
					m_ChangedProperties["BrandId"] = new ChangedProperty("BrandId", m_brandId, value);
					
					m_brandId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IntroText of the Homepage object.
		/// </summary>
		public virtual string IntroText
		{
			get
			{
				return m_introText;
			}
			set 
			{ 
				if ((value != m_introText))
				{
					m_ChangedProperties["IntroText"] = new ChangedProperty("IntroText", m_introText, value);
					
					m_introText = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Url1 of the Homepage object.
		/// </summary>
		public virtual string Url1
		{
			get
			{
				return m_url1;
			}
			set 
			{ 
				if ((value != m_url1))
				{
					m_ChangedProperties["Url1"] = new ChangedProperty("Url1", m_url1, value);
					
					m_url1 = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Url2 of the Homepage object.
		/// </summary>
		public virtual string Url2
		{
			get
			{
				return m_url2;
			}
			set 
			{ 
				if ((value != m_url2))
				{
					m_ChangedProperties["Url2"] = new ChangedProperty("Url2", m_url2, value);
					
					m_url2 = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Url3 of the Homepage object.
		/// </summary>
		public virtual string Url3
		{
			get
			{
				return m_url3;
			}
			set 
			{ 
				if ((value != m_url3))
				{
					m_ChangedProperties["Url3"] = new ChangedProperty("Url3", m_url3, value);
					
					m_url3 = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Url4 of the Homepage object.
		/// </summary>
		public virtual string Url4
		{
			get
			{
				return m_url4;
			}
			set 
			{ 
				if ((value != m_url4))
				{
					m_ChangedProperties["Url4"] = new ChangedProperty("Url4", m_url4, value);
					
					m_url4 = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the BumperPageHtml of the Homepage object.
		/// </summary>
		public virtual string BumperPageHtml
		{
			get
			{
				return m_bumperPageHtml;
			}
			set 
			{ 
				if ((value != m_bumperPageHtml))
				{
					m_ChangedProperties["BumperPageHtml"] = new ChangedProperty("BumperPageHtml", m_bumperPageHtml, value);
					
					m_bumperPageHtml = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the BumperPageSkip of the Homepage object.
		/// </summary>
		public virtual bool BumperPageSkip
		{
			get
			{
				return m_bumperPageSkip;
			}
			set 
			{ 
				if ((value != m_bumperPageSkip))
				{
					m_ChangedProperties["BumperPageSkip"] = new ChangedProperty("BumperPageSkip", m_bumperPageSkip, value);
					
					m_bumperPageSkip = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CustomHtml of the Homepage object.
		/// </summary>
		public virtual string CustomHtml
		{
			get
			{
				return m_customHtml;
			}
			set 
			{ 
				if ((value != m_customHtml))
				{
					m_ChangedProperties["CustomHtml"] = new ChangedProperty("CustomHtml", m_customHtml, value);
					
					m_customHtml = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the HomepageTypeId of the Homepage object.
		/// </summary>
		public virtual int HomepageTypeId
		{
			get
			{
				return m_homepageTypeId;
			}
			set 
			{ 
				if ((value != m_homepageTypeId))
				{
					m_ChangedProperties["HomepageTypeId"] = new ChangedProperty("HomepageTypeId", m_homepageTypeId, value);
					
					m_homepageTypeId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsPublished of the Homepage object.
		/// </summary>
		public virtual bool IsPublished
		{
			get
			{
				return m_isPublished;
			}
			set 
			{ 
				if ((value != m_isPublished))
				{
					m_ChangedProperties["IsPublished"] = new ChangedProperty("IsPublished", m_isPublished, value);
					
					m_isPublished = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastModifiedByUserId of the Homepage object.
		/// </summary>
		public virtual int LastModifiedByUserId
		{
			get
			{
				return m_lastModifiedByUserId;
			}
			set 
			{ 
				if ((value != m_lastModifiedByUserId))
				{
					m_ChangedProperties["LastModifiedByUserId"] = new ChangedProperty("LastModifiedByUserId", m_lastModifiedByUserId, value);
					
					m_lastModifiedByUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastModifiedDate of the Homepage object.
		/// </summary>
		public virtual DateTime LastModifiedDate
		{
			get
			{
				return m_lastModifiedDate;
			}
			set 
			{ 
				if ((value != m_lastModifiedDate))
				{
					m_ChangedProperties["LastModifiedDate"] = new ChangedProperty("LastModifiedDate", m_lastModifiedDate, value);
					
					m_lastModifiedDate = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual string BrandName
		{
			get
			{
				return m_brandName;
			}
			set 
			{ 
				m_brandName = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			HomepageId,
			BrandId,
			IntroText,
			Url1,
			Url2,
			Url3,
			Url4,
			BumperPageHtml,
			BumperPageSkip,
			CustomHtml,
			HomepageTypeId,
			IsPublished,
			LastModifiedByUserId,
			LastModifiedDate
		}
	}
}

