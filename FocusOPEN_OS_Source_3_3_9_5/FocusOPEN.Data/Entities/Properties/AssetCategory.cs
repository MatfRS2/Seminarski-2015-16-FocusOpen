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
	public partial class AssetCategory : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetCategoryId = null;
		
		// Table variables
		protected int m_assetId = 0;
		protected int m_categoryId = 0;
		protected bool m_isPrimary = false;
		
		// View Variables
		protected string m_name = String.Empty;
		protected Nullable <Int32> m_parentCategoryId = null;
		protected string m_synonyms = String.Empty;
		protected Nullable <Int32> m_categoryOrder = null;
		protected string m_nameAndSynonyms = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetCategory object.
		/// </summary>
		public Nullable <Int32> AssetCategoryId
		{
			get
			{
				return m_assetCategoryId;
			}
			set 
			{
				if (value != m_assetCategoryId)
				{
					m_assetCategoryId = value;
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
				return (m_assetCategoryId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetId of the AssetCategory object.
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
		/// Sets or Returns the CategoryId of the AssetCategory object.
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
		/// Sets or Returns the IsPrimary of the AssetCategory object.
		/// </summary>
		public virtual bool IsPrimary
		{
			get
			{
				return m_isPrimary;
			}
			set 
			{ 
				if ((value != m_isPrimary))
				{
					m_ChangedProperties["IsPrimary"] = new ChangedProperty("IsPrimary", m_isPrimary, value);
					
					m_isPrimary = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set 
			{ 
				m_name = value; 
			}
		}
		public virtual Nullable <Int32> ParentCategoryId
		{
			get
			{
				return m_parentCategoryId;
			}
			set 
			{ 
				m_parentCategoryId = value; 
			}
		}
		public virtual string Synonyms
		{
			get
			{
				return m_synonyms;
			}
			set 
			{ 
				m_synonyms = value; 
			}
		}
		public virtual Nullable <Int32> CategoryOrder
		{
			get
			{
				return m_categoryOrder;
			}
			set 
			{ 
				m_categoryOrder = value; 
			}
		}
		public virtual string NameAndSynonyms
		{
			get
			{
				return m_nameAndSynonyms;
			}
			set 
			{ 
				m_nameAndSynonyms = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			AssetCategoryId,
			AssetId,
			CategoryId,
			IsPrimary
		}
	}
}

