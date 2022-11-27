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
	/// This object represents the properties and methods of a LightboxBrand.
	/// </summary>
	[Serializable]
	public partial class LightboxBrand : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxBrandId = null;
		
		// Table variables
		protected int m_lightboxId = 0;
		protected int m_brandId = 0;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxBrand object.
		/// </summary>
		public Nullable <Int32> LightboxBrandId
		{
			get
			{
				return m_lightboxBrandId;
			}
			set 
			{
				if (value != m_lightboxBrandId)
				{
					m_lightboxBrandId = value;
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
				return (m_lightboxBrandId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the LightboxId of the LightboxBrand object.
		/// </summary>
		public virtual int LightboxId
		{
			get
			{
				return m_lightboxId;
			}
			set 
			{ 
				if ((value != m_lightboxId))
				{
					m_ChangedProperties["LightboxId"] = new ChangedProperty("LightboxId", m_lightboxId, value);
					
					m_lightboxId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the BrandId of the LightboxBrand object.
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
			LightboxBrandId,
			LightboxId,
			BrandId
		}
	}
}

