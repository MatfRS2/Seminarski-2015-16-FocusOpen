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
	/// This object represents the properties and methods of a AuditAssetSearchKeyword.
	/// </summary>
	[Serializable]
	public partial class AuditAssetSearchKeyword : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_auditAssetSearchKeywordId = null;
		
		// Table variables
		protected int m_auditAssetSearchId = 0;
		protected string m_searchKeyword = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AuditAssetSearchKeyword object.
		/// </summary>
		public Nullable <Int32> AuditAssetSearchKeywordId
		{
			get
			{
				return m_auditAssetSearchKeywordId;
			}
			set 
			{
				if (value != m_auditAssetSearchKeywordId)
				{
					m_auditAssetSearchKeywordId = value;
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
				return (m_auditAssetSearchKeywordId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AuditAssetSearchId of the AuditAssetSearchKeyword object.
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
		/// Sets or Returns the SearchKeyword of the AuditAssetSearchKeyword object.
		/// </summary>
		public virtual string SearchKeyword
		{
			get
			{
				return m_searchKeyword;
			}
			set 
			{ 
				if ((value != m_searchKeyword))
				{
					m_ChangedProperties["SearchKeyword"] = new ChangedProperty("SearchKeyword", m_searchKeyword, value);
					
					m_searchKeyword = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			AuditAssetSearchKeywordId,
			AuditAssetSearchId,
			SearchKeyword
		}
	}
}

