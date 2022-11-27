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
	public partial class Category : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_categoryId = null;
		
		// Table variables
		protected Nullable <Int32> m_parentCategoryId = null;
		protected int m_brandId = 0;
		protected string m_name = String.Empty;
		protected string m_externalRef = String.Empty;
		protected string m_message = String.Empty;
		protected string m_synonyms = String.Empty;
		protected int m_ownerUserId = 0;
		protected int m_categoryOrder = 0;
		protected int m_fullAssetCount = 0;
		protected int m_availableAssetCount = 0;
		
		// View Variables
		protected string m_ownerEmail = String.Empty;
		protected string m_ownerName = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Category object.
		/// </summary>
		public Nullable <Int32> CategoryId
		{
			get
			{
				return m_categoryId;
			}
			set 
			{
				if (value != m_categoryId)
				{
					m_categoryId = value;
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
				return (m_categoryId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the ParentCategoryId of the Category object.
		/// </summary>
		public virtual Nullable <Int32> ParentCategoryId
		{
			get
			{
				return m_parentCategoryId;
			}
			set 
			{ 
				if (value != m_parentCategoryId)
				{
					m_ChangedProperties["ParentCategoryId"] = new ChangedProperty("ParentCategoryId", m_parentCategoryId, value);
					
					m_parentCategoryId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the BrandId of the Category object.
		/// </summary>
		public virtual int BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{ 
				if (value != m_brandId)
				{
					m_ChangedProperties["BrandId"] = new ChangedProperty("BrandId", m_brandId, value);
					
					m_brandId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Name of the Category object.
		/// </summary>
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set 
			{ 
				if (value != m_name)
				{
					m_ChangedProperties["Name"] = new ChangedProperty("Name", m_name, value);
					
					m_name = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ExternalRef of the Category object.
		/// </summary>
		public virtual string ExternalRef
		{
			get
			{
				return m_externalRef;
			}
			set 
			{ 
				if (value != m_externalRef)
				{
					m_ChangedProperties["ExternalRef"] = new ChangedProperty("ExternalRef", m_externalRef, value);
					
					m_externalRef = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Message of the Category object.
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
		/// Sets or Returns the Synonyms of the Category object.
		/// </summary>
		public virtual string Synonyms
		{
			get
			{
				return m_synonyms;
			}
			set 
			{ 
				if (value != m_synonyms)
				{
					m_ChangedProperties["Synonyms"] = new ChangedProperty("Synonyms", m_synonyms, value);
					
					m_synonyms = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OwnerUserId of the Category object.
		/// </summary>
		public virtual int OwnerUserId
		{
			get
			{
				return m_ownerUserId;
			}
			set 
			{ 
				if (value != m_ownerUserId)
				{
					m_ChangedProperties["OwnerUserId"] = new ChangedProperty("OwnerUserId", m_ownerUserId, value);
					
					m_ownerUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CategoryOrder of the Category object.
		/// </summary>
		public virtual int CategoryOrder
		{
			get
			{
				return m_categoryOrder;
			}
			set 
			{ 
				if (value != m_categoryOrder)
				{
					m_ChangedProperties["CategoryOrder"] = new ChangedProperty("CategoryOrder", m_categoryOrder, value);
					
					m_categoryOrder = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FullAssetCount of the Category object.
		/// </summary>
		public virtual int FullAssetCount
		{
			get
			{
				return m_fullAssetCount;
			}
			set 
			{ 
				if (value != m_fullAssetCount)
				{
					m_ChangedProperties["FullAssetCount"] = new ChangedProperty("FullAssetCount", m_fullAssetCount, value);
					
					m_fullAssetCount = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AvailableAssetCount of the Category object.
		/// </summary>
		public virtual int AvailableAssetCount
		{
			get
			{
				return m_availableAssetCount;
			}
			set 
			{ 
				if (value != m_availableAssetCount)
				{
					m_ChangedProperties["AvailableAssetCount"] = new ChangedProperty("AvailableAssetCount", m_availableAssetCount, value);
					
					m_availableAssetCount = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual string OwnerEmail
		{
			get
			{
				return m_ownerEmail;
			}
			set 
			{ 
				m_ownerEmail = value; 
			}
		}
		public virtual string OwnerName
		{
			get
			{
				return m_ownerName;
			}
			set 
			{ 
				m_ownerName = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			CategoryId,
			ParentCategoryId,
			BrandId,
			Name,
			ExternalRef,
			Message,
			Synonyms,
			OwnerUserId,
			CategoryOrder,
			FullAssetCount,
			AvailableAssetCount
		}
	}
}

