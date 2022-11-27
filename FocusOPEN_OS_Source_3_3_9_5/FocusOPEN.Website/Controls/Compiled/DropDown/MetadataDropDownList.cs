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
using System.Linq;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class MetadataDropDownList : AbstractNestedDataDropDownList<Metadata>
	{
		#region Accessors

        public SelectableMetadataSortType SortType
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "SortType", SelectableMetadataSortType.AlphaNumeric);
            }
            set
            {
                ViewState["SortType"] = value;
            }
        }
        public bool LinearOutlay
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "LinearOutlay", false);
            }
            set
            {
                ViewState["LinearOutlay"] = value;
            }
        }
        public int GroupNumber
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "GroupNumber", 0);
            }
            set
            {
                ViewState["GroupNumber"] = value;
            }
        }

		#endregion

		#region Constructor

		public MetadataDropDownList()
		{
			BrandId = 0;
			GroupNumber = 0;
		}

		#endregion

		#region AbstractNestedDropDownList Implementation

        public override sealed NestedDataControlHelper<Metadata> NestedDataHelper
        {
            get
            {
                return new MetadataNestedDataControlHelper(BrandId, GroupNumber, SortType, LinearOutlay);
                //	            if(m_NestedDataControlHelper==null)
                //                    m_NestedDataControlHelper = new MetadataNestedDataControlHelper(BrandId, GroupNumber, SortType, LinearOutlay);
                //
                //	            return m_NestedDataControlHelper;
            }
        }

	    public override IEnumerable<Metadata> GetParentList()
		{
            throw new NotImplementedException("the NestedDataControlHelper should take care of this logic");
//			var entries = (from m in MetadataCache.Instance.GetList(BrandId, GroupNumber)
//			                where !m.ParentMetadataId.HasValue
//			                select m)
//                                .ToList();
//
//            if (SortType == SelectableMetadataSortType.AlphaNumeric)
//                entries = entries.OrderBy(e => e.Name).ToList();
//
//		    return entries;
		}

        public override IEnumerable<Metadata> GetChildren(Metadata entity)
		{
            throw new NotImplementedException("the NestedDataControlHelper should take care of this logic");
//		    var entries = entity.Children;
//
//            if (SortType == SelectableMetadataSortType.AlphaNumeric)
//                entries = entries.OrderBy(e => e.Name).ToList();
//
//            return entries;
		}

        public override string GetEntityName(Metadata entity)
		{
            throw new NotImplementedException("the NestedDataControlHelper should take care of this logic");
//			return entity.Name;
		}

        public override int GetEntityId(Metadata entity)
		{
            throw new NotImplementedException("the NestedDataControlHelper should take care of this logic");
//			return entity.MetadataId.GetValueOrDefault();
		}

		#endregion

	}
}