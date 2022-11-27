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
	public partial class BrandMetadataSelectableSetting : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_brandMetadataSelectableSettingId = null;
		
		// Table variables
        protected int m_brandMetadataSettingId = 0;
        protected int m_selectableType = 0;
        protected int m_depth = 0;
        protected bool m_isLinear = false;
        protected int m_sortType = 0;
        protected bool m_allowMultiple = false;
        protected int m_orderType = 0;
        protected int m_columnCount = 0;
        protected int m_filterGroup = 0;
        protected int m_filterSelectableType = 0;
        protected int m_filterDepth = 0;

		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the BrandMetadataSelectableSetting object.
		/// </summary>
		public Nullable <Int32> BrandMetadataSelectableSettingId
		{
			get
			{
				return m_brandMetadataSelectableSettingId;
			}
			set 
			{
				if (value != m_brandMetadataSelectableSettingId)
				{
					m_brandMetadataSelectableSettingId = value;
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
				return (m_brandMetadataSelectableSettingId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors

        public virtual int BrandMetadataSettingId { get { return m_brandMetadataSettingId; } set { if (value != m_brandMetadataSettingId) { m_ChangedProperties["BrandMetadataSettingId"] = new ChangedProperty("BrandMetadataSettingId", m_brandMetadataSettingId, value); m_brandMetadataSettingId = value; m_isDirty = true; } } }
        public virtual int SelectableType { get { return m_selectableType; } set { if (value != m_selectableType) { m_ChangedProperties["SelectableType"] = new ChangedProperty("SelectableType", m_selectableType, value); m_selectableType = value; m_isDirty = true; } } }
        public virtual int Depth { get { return m_depth; } set { if (value != m_depth) { m_ChangedProperties["Depth"] = new ChangedProperty("Depth", m_depth, value); m_depth = value; m_isDirty = true; } } }
        public virtual bool IsLinear { get { return m_isLinear; } set { if (value != m_isLinear) { m_ChangedProperties["IsLinear"] = new ChangedProperty("IsLinear", m_isLinear, value); m_isLinear = value; m_isDirty = true; } } }
        public virtual int SortType { get { return m_sortType; } set { if (value != m_sortType) { m_ChangedProperties["SortType"] = new ChangedProperty("SortType", m_sortType, value); m_sortType = value; m_isDirty = true; } } }
        public virtual bool AllowMultiple { get { return m_allowMultiple; } set { if (value != m_allowMultiple) { m_ChangedProperties["AllowMultiple"] = new ChangedProperty("AllowMultiple", m_allowMultiple, value); m_allowMultiple = value; m_isDirty = true; } } }
        public virtual int OrderType { get { return m_orderType; } set { if (value != m_orderType) { m_ChangedProperties["OrderType"] = new ChangedProperty("OrderType", m_orderType, value); m_orderType = value; m_isDirty = true; } } }
        public virtual int ColumnCount { get { return m_columnCount; } set { if (value != m_columnCount) { m_ChangedProperties["ColumnCount"] = new ChangedProperty("ColumnCount", m_columnCount, value); m_columnCount = value; m_isDirty = true; } } }
        public virtual int FilterGroup { get { return m_filterGroup; } set { if (value != m_filterGroup) { m_ChangedProperties["FilterGroup"] = new ChangedProperty("FilterGroup", m_filterGroup, value); m_filterGroup = value; m_isDirty = true; } } }
        public virtual int FilterSelectableType { get { return m_filterSelectableType; } set { if (value != m_filterSelectableType) { m_ChangedProperties["FilterSelectableType"] = new ChangedProperty("FilterSelectableType", m_filterSelectableType, value); m_filterSelectableType = value; m_isDirty = true; } } }
        public virtual int FilterDepth { get { return m_filterDepth; } set { if (value != m_filterDepth) { m_ChangedProperties["FilterDepth"] = new ChangedProperty("FilterDepth", m_filterDepth, value); m_filterDepth = value; m_isDirty = true; } } }

	    #endregion
		
		public enum Columns
		{
            BrandMetadataSelectableSettingId,
            BrandMetadataSettingId,
            SelectableType,
            Depth,
            IsLinear,
            SortType,
            AllowMultiple,
            OrderType,
            ColumnCount,
            FilterGroup,
            FilterSelectableType,
            FilterDepth
		}
	}
}

