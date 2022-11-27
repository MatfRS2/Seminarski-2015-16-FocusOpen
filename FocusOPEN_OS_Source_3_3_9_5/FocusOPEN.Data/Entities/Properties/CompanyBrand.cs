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
	/// This object represents the properties and methods of a CompanyBrand.
	/// </summary>
	[Serializable]
	public partial class CompanyBrand : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_companyBrandId = null;
		
		// Table variables
		protected int m_companyId = 0;
		protected int m_brandId = 0;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the CompanyBrand object.
		/// </summary>
		public Nullable <Int32> CompanyBrandId
		{
			get
			{
				return m_companyBrandId;
			}
			set 
			{
				if (value != m_companyBrandId)
				{
					m_companyBrandId = value;
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
				return (m_companyBrandId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the CompanyId of the CompanyBrand object.
		/// </summary>
		public virtual int CompanyId
		{
			get
			{
				return m_companyId;
			}
			set 
			{ 
				if ((value != m_companyId))
				{
					m_ChangedProperties["CompanyId"] = new ChangedProperty("CompanyId", m_companyId, value);
					
					m_companyId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the BrandId of the CompanyBrand object.
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

		#endregion
		
		
		public enum Columns
		{
			CompanyBrandId,
			CompanyId,
			BrandId
		}
	}
}

