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
	/// This object represents the properties and methods of a Company.
	/// </summary>
	[Serializable]
	public partial class Company : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_companyId = null;
		
		// Table variables
		protected string m_name = String.Empty;
		protected string m_brands = String.Empty;
		protected string m_domain = String.Empty;
		protected bool m_isInternal = false;
		protected int m_createdByUserId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		
		// View Variables
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Company object.
		/// </summary>
		public Nullable <Int32> CompanyId
		{
			get
			{
				return m_companyId;
			}
			set 
			{
				if (value != m_companyId)
				{
					m_companyId = value;
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
				return (m_companyId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Name of the Company object.
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
		/// Sets or Returns the Brands of the Company object.
		/// </summary>
		public virtual string Brands
		{
			get
			{
				return m_brands;
			}
			set 
			{ 
				if ((value != m_brands))
				{
					m_ChangedProperties["Brands"] = new ChangedProperty("Brands", m_brands, value);
					
					m_brands = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Domain of the Company object.
		/// </summary>
		public virtual string Domain
		{
			get
			{
				return m_domain;
			}
			set 
			{ 
				if ((value != m_domain))
				{
					m_ChangedProperties["Domain"] = new ChangedProperty("Domain", m_domain, value);
					
					m_domain = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsInternal of the Company object.
		/// </summary>
		public virtual bool IsInternal
		{
			get
			{
				return m_isInternal;
			}
			set 
			{ 
				if ((value != m_isInternal))
				{
					m_ChangedProperties["IsInternal"] = new ChangedProperty("IsInternal", m_isInternal, value);
					
					m_isInternal = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreatedByUserId of the Company object.
		/// </summary>
		public virtual int CreatedByUserId
		{
			get
			{
				return m_createdByUserId;
			}
			set 
			{ 
				if ((value != m_createdByUserId))
				{
					m_ChangedProperties["CreatedByUserId"] = new ChangedProperty("CreatedByUserId", m_createdByUserId, value);
					
					m_createdByUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreateDate of the Company object.
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
		
		
		#endregion
		
		public enum Columns
		{
			CompanyId,
			Name,
			Brands,
			Domain,
			IsInternal,
			CreatedByUserId,
			CreateDate
		}
	}
}

