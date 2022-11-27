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
	public partial class AssetMetadata : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetMetadataId = null;
		
		// Table variables
		protected int m_assetId = 0;
		protected int m_metadataId = 0;
		
		// View Variables
		protected Nullable <Int32> m_brandId = null;
		protected Nullable <Int32> m_groupNumber = null;
		protected string m_name = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetMetadata object.
		/// </summary>
		public Nullable <Int32> AssetMetadataId
		{
			get
			{
				return m_assetMetadataId;
			}
			set 
			{
				if (value != m_assetMetadataId)
				{
					m_assetMetadataId = value;
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
				return (m_assetMetadataId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetId of the AssetMetadata object.
		/// </summary>
		public virtual int AssetId
		{
			get
			{
				return m_assetId;
			}
			set 
			{ 
				if (value != m_assetId)
				{
					m_ChangedProperties["AssetId"] = new ChangedProperty("AssetId", m_assetId, value);
					
					m_assetId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the MetadataId of the AssetMetadata object.
		/// </summary>
		public virtual int MetadataId
		{
			get
			{
				return m_metadataId;
			}
			set 
			{ 
				if (value != m_metadataId)
				{
					m_ChangedProperties["MetadataId"] = new ChangedProperty("MetadataId", m_metadataId, value);
					
					m_metadataId = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual Nullable <Int32> BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{ 
				m_brandId = value; 
			}
		}
		public virtual Nullable <Int32> GroupNumber
		{
			get
			{
				return m_groupNumber;
			}
			set 
			{ 
				m_groupNumber = value; 
			}
		}
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
		
		#endregion
		
		public enum Columns
		{
			AssetMetadataId,
			AssetId,
			MetadataId
		}
	}
}

