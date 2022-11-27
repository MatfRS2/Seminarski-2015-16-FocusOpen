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
	/// This object represents the properties and methods of a AuditAssetSearchResult.
	/// </summary>
	[Serializable]
	public partial class AuditAssetSearchResult : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditAssetSearchResultId = null;
		
		// Table variables
		protected int m_auditAssetSearchId = 0;
		protected int m_assetId = 0;
		protected DateTime m_date = DateTime.MinValue;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AuditAssetSearchResult object.
		/// </summary>
		public Nullable <Int32> AuditAssetSearchResultId
		{
			get
			{
				return m_auditAssetSearchResultId;
			}
			set 
			{
				if (value != m_auditAssetSearchResultId)
				{
					m_auditAssetSearchResultId = value;
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
				return (m_auditAssetSearchResultId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AuditAssetSearchId of the AuditAssetSearchResult object.
		/// </summary>
		public virtual int AuditAssetSearchId
		{
			get
			{
				return m_auditAssetSearchId;
			}
			set 
			{ 
				if ((value != m_auditAssetSearchId))
				{
					m_ChangedProperties["AuditAssetSearchId"] = new ChangedProperty("AuditAssetSearchId", m_auditAssetSearchId, value);
					
					m_auditAssetSearchId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetId of the AuditAssetSearchResult object.
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
		/// Sets or Returns the Date of the AuditAssetSearchResult object.
		/// </summary>
		public virtual DateTime Date
		{
			get
			{
				return m_date;
			}
			set 
			{ 
				if ((value != m_date))
				{
					m_ChangedProperties["Date"] = new ChangedProperty("Date", m_date, value);
					
					m_date = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			AuditAssetSearchResultId,
			AuditAssetSearchId,
			AssetId,
			Date
		}
	}
}

