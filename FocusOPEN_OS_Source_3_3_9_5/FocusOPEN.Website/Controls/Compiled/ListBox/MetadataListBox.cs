using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
    public class MetadataListBox : AbstractNestedDataListBox<Metadata>
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

        public MetadataListBox()
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

        public void RefreshFromBrandAndSelect(int brandId, IEnumerable<int> idList)
        {
            BrandId = brandId;
            BindList();

            foreach (int id in idList)
                SafeSelectValue(id);
        }
    }
}