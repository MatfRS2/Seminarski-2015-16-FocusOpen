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
using SearchBuilder=Daydream.Data.SearchBuilder;

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class BrandMetadataSelectableSettingFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_brandMetadataSelectableSettingId = null;
		
		// Table columns
        protected int m_brandMetadataSettingId = 0;
        protected int m_selectableType = 0;
        protected int m_depth = 0;
        protected Nullable<Boolean> m_isLinear = null;
        protected int m_sortType = 0;
        protected Nullable<Boolean> m_allowMultiple = null;
        protected int m_orderType = 0;
        protected int m_columnCount = 0;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_brandMetadataSelectableSettingIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
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
					m_FindCriteriaCount++;
				}
			}
		}


        public Int32 BrandMetadataSettingId { get { return m_brandMetadataSettingId; } set { if (value != m_brandMetadataSettingId) { m_brandMetadataSettingId = value; m_FindCriteriaCount++; } } }
        public Int32 SelectableType { get { return m_selectableType; } set { if (value != m_selectableType) { m_selectableType = value; m_FindCriteriaCount++; } } }
        public Int32 Depth { get { return m_depth; } set { if (value != m_depth) { m_depth = value; m_FindCriteriaCount++; } } }
        public Nullable<bool> IsLinear { get { return m_isLinear; } set { if (value != m_isLinear) { m_isLinear = value; m_FindCriteriaCount++; } } }
        public Int32 SortType { get { return m_sortType; } set { if (value != m_sortType) { m_sortType = value; m_FindCriteriaCount++; } } }
        public Nullable<bool> AllowMultiple { get { return m_allowMultiple; } set { if (value != m_allowMultiple) { m_allowMultiple = value; m_FindCriteriaCount++; } } }
        public Int32 OrderType { get { return m_orderType; } set { if (value != m_orderType) { m_orderType = value; m_FindCriteriaCount++; } } }
        public Int32 ColumnCount { get { return m_columnCount; } set { if (value != m_columnCount) { m_columnCount = value; m_FindCriteriaCount++; } } }

		public int FindCriteriaCount
		{
			get
			{
				return m_FindCriteriaCount;
			}
		}
		
		#endregion
		

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[BrandMetadataSelectableSetting]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (BrandMetadataSelectableSettingId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@brandMetadataSelectableSettingId", BrandMetadataSelectableSetting.Columns.BrandMetadataSelectableSettingId));
				sb.AddDataParameter("@brandMetadataSelectableSettingId", BrandMetadataSelectableSettingId.Value);
			}

            if (BrandMetadataSettingId != 0) { sb.Criteria.Add(string.Format("{0}=@brandMetadataSettingId", BrandMetadataSelectableSetting.Columns.BrandMetadataSettingId)); sb.AddDataParameter("@brandMetadataSettingId", BrandMetadataSettingId); }
            if (SelectableType != 0) { sb.Criteria.Add(string.Format("{0}=@selectableType", BrandMetadataSelectableSetting.Columns.SelectableType)); sb.AddDataParameter("@selectableType", SelectableType); }
            if (Depth != 0) { sb.Criteria.Add(string.Format("{0}=@depth", BrandMetadataSelectableSetting.Columns.Depth)); sb.AddDataParameter("@depth", Depth); }
            if (IsLinear.HasValue) { sb.Criteria.Add(string.Format("{0}=@isLinear", BrandMetadataSelectableSetting.Columns.IsLinear)); sb.AddDataParameter("@isLinear", IsLinear); }
            if (SortType != 0) { sb.Criteria.Add(string.Format("{0}=@sortType", BrandMetadataSelectableSetting.Columns.SortType)); sb.AddDataParameter("@sortType", SortType); }
            if (AllowMultiple.HasValue) { sb.Criteria.Add(string.Format("{0}=@allowMultiple", BrandMetadataSelectableSetting.Columns.AllowMultiple)); sb.AddDataParameter("@allowMultiple", AllowMultiple); }
            if (OrderType != 0) { sb.Criteria.Add(string.Format("{0}=@orderType", BrandMetadataSelectableSetting.Columns.OrderType)); sb.AddDataParameter("@orderType", OrderType); }
            if (ColumnCount != 0) { sb.Criteria.Add(string.Format("{0}=@columnCount", BrandMetadataSelectableSetting.Columns.ColumnCount)); sb.AddDataParameter("@columnCount", ColumnCount); }

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}