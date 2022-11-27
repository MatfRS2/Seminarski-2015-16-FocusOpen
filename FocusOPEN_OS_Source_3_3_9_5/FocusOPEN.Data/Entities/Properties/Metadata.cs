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
	public partial class Metadata : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_metadataId = null;
		
		// Table variables
		protected Nullable <Int32> m_brandId = null;
		protected Nullable <Int32> m_parentMetadataId = null;
		protected string m_name = String.Empty;
		protected string m_externalRef = String.Empty;
		protected string m_synonyms = String.Empty;
		protected int m_groupNumber = 0;
        protected int m_metadataOrder = 0;
		protected bool m_isDeleted = false;
		
		// View Variables
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Metadata object.
		/// </summary>
		public Nullable <Int32> MetadataId
		{
			get
			{
				return m_metadataId;
			}
			set 
			{
				if (value != m_metadataId)
				{
					m_metadataId = value;
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
				return (m_metadataId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the BrandId of the Metadata object.
		/// </summary>
		public virtual Nullable <Int32> BrandId
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
		/// Sets or Returns the ParentMetadataId of the Metadata object.
		/// </summary>
		public virtual Nullable <Int32> ParentMetadataId
		{
			get
			{
				return m_parentMetadataId;
			}
			set 
			{ 
				if (value != m_parentMetadataId)
				{
					m_ChangedProperties["ParentMetadataId"] = new ChangedProperty("ParentMetadataId", m_parentMetadataId, value);
					
					m_parentMetadataId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Name of the Metadata object.
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
		/// Sets or Returns the ExternalRef of the Metadata object.
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
		/// Sets or Returns the Synonyms of the Metadata object.
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
		/// Sets or Returns the GroupNumber of the Metadata object.
		/// </summary>
		public virtual int GroupNumber
		{
			get
			{
				return m_groupNumber;
			}
			set 
			{ 
				if (value != m_groupNumber)
				{
					m_ChangedProperties["GroupNumber"] = new ChangedProperty("GroupNumber", m_groupNumber, value);
					
					m_groupNumber = value; 
					m_isDirty = true;
				}
			}
		}
        
        /// <summary>
        /// Sets or Returns the MetadataOrder of the Metadata object.
		/// </summary>
        public virtual int MetadataOrder
		{
			get
			{
				return m_metadataOrder;
			}
			set 
			{ 
				if (value != m_metadataOrder)
				{
                    m_ChangedProperties["MetadataOrder"] = new ChangedProperty("MetadataOrder", m_metadataOrder, value);
					
					m_metadataOrder = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDeleted of the Metadata object.
		/// </summary>
		public virtual bool IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set 
			{ 
				if (value != m_isDeleted)
				{
					m_ChangedProperties["IsDeleted"] = new ChangedProperty("IsDeleted", m_isDeleted, value);
					
					m_isDeleted = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		
		#endregion
		
		public enum Columns
		{
			MetadataId,
			BrandId,
			ParentMetadataId,
			Name,
			ExternalRef,
			Synonyms,
			GroupNumber,
			IsDeleted,
            MetadataOrder
		}
	}
}

