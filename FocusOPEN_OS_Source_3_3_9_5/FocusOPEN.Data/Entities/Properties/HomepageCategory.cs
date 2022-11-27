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
	/// This object represents the properties and methods of a HomepageCategory.
	/// </summary>
	[Serializable]
	public partial class HomepageCategory : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_homepageCategoryId = null;
		
		// Table variables
		protected int m_homepageId = 0;
		protected int m_categoryId = 0;
		protected int m_orderBy = 0;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the HomepageCategory object.
		/// </summary>
		public Nullable <Int32> HomepageCategoryId
		{
			get
			{
				return m_homepageCategoryId;
			}
			set 
			{
				if (value != m_homepageCategoryId)
				{
					m_homepageCategoryId = value;
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
				return (m_homepageCategoryId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the HomepageId of the HomepageCategory object.
		/// </summary>
		public virtual int HomepageId
		{
			get
			{
				return m_homepageId;
			}
			set 
			{ 
				if ((value != m_homepageId))
				{
					m_ChangedProperties["HomepageId"] = new ChangedProperty("HomepageId", m_homepageId, value);
					
					m_homepageId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CategoryId of the HomepageCategory object.
		/// </summary>
		public virtual int CategoryId
		{
			get
			{
				return m_categoryId;
			}
			set 
			{ 
				if ((value != m_categoryId))
				{
					m_ChangedProperties["CategoryId"] = new ChangedProperty("CategoryId", m_categoryId, value);
					
					m_categoryId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OrderBy of the HomepageCategory object.
		/// </summary>
		public virtual int OrderBy
		{
			get
			{
				return m_orderBy;
			}
			set 
			{ 
				if ((value != m_orderBy))
				{
					m_ChangedProperties["OrderBy"] = new ChangedProperty("OrderBy", m_orderBy, value);
					
					m_orderBy = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			HomepageCategoryId,
			HomepageId,
			CategoryId,
			OrderBy
		}
	}
}

