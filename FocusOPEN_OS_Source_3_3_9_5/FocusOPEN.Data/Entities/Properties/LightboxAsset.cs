/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
	public partial class LightboxAsset : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxAssetId = null;
		
		// Table variables
		protected int m_lightboxId = 0;
		protected int m_assetId = 0;
		protected string m_notes = String.Empty;
		protected DateTime m_createDate = DateTime.MinValue;
		protected Nullable <Int32> m_orderNumber = null;
		
		// View Variables
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxAsset object.
		/// </summary>
		public Nullable <Int32> LightboxAssetId
		{
			get
			{
				return m_lightboxAssetId;
			}
			set 
			{
				if (value != m_lightboxAssetId)
				{
					m_lightboxAssetId = value;
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
				return (m_lightboxAssetId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the LightboxId of the LightboxAsset object.
		/// </summary>
		public virtual int LightboxId
		{
			get
			{
				return m_lightboxId;
			}
			set 
			{ 
				if (value != m_lightboxId)
				{
					m_ChangedProperties["LightboxId"] = new ChangedProperty("LightboxId", m_lightboxId, value);
					
					m_lightboxId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetId of the LightboxAsset object.
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
		/// Sets or Returns the Notes of the LightboxAsset object.
		/// </summary>
		public virtual string Notes
		{
			get
			{
				return m_notes;
			}
			set 
			{ 
				if (value != m_notes)
				{
					m_ChangedProperties["Notes"] = new ChangedProperty("Notes", m_notes, value);
					
					m_notes = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreateDate of the LightboxAsset object.
		/// </summary>
		public virtual DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set 
			{ 
				if (value != m_createDate)
				{
					m_ChangedProperties["CreateDate"] = new ChangedProperty("CreateDate", m_createDate, value);
					
					m_createDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OrderNumber of the LightboxAsset object.
		/// </summary>
		public virtual Nullable <Int32> OrderNumber
		{
			get
			{
				return m_orderNumber;
			}
			set 
			{ 
				if (value != m_orderNumber)
				{
					m_ChangedProperties["OrderNumber"] = new ChangedProperty("OrderNumber", m_orderNumber, value);
					
					m_orderNumber = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		
		#endregion
		
		public enum Columns
		{
			LightboxAssetId,
			LightboxId,
			AssetId,
			Notes,
			CreateDate,
			OrderNumber
		}
	}
}

