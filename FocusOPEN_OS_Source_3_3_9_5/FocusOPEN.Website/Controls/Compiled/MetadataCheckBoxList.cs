using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls.Compiled
{
    public class MetadataCheckBoxList : CheckBoxList, INestedDataSelectable<Metadata>
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
        public SelectableMetadataOrderType OrderType
        {
            set
            {
                RepeatDirection = value == SelectableMetadataOrderType.ByRow
                                      ? RepeatDirection.Horizontal
                                      : RepeatDirection.Vertical;
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
        public int BrandId
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "BrandId", 0);
            }
            set
            {
                ViewState["BrandId"] = value;
            }
        }

		#endregion

		#region Constructor

        public MetadataCheckBoxList()
		{
			GroupNumber = 0;
		}

		#endregion

		#region AbstractNestedDropDownList Implementation

        public NestedDataControlHelper<Metadata> NestedDataHelper
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

        public List<int> SelectedIds
        {
            get
            {
                var list = new List<int>();

                foreach (ListItem item in Items)
                {
                    if (item.Selected)
                    {
                        int value = Convert.ToInt32(item.Value);

                        list.Add(value);
                    }
                }

                return list;
            }
        }

        #endregion

        private object GetDataSource()
        {
            return NestedDataHelper.GetDataSource();
        }

        public void RefreshFromDataSource()
        {
            DataSource = GetDataSource();
            DataTextField = NestedDataHelper.GetDataTextField();
            DataValueField = NestedDataHelper.GetDataValueField();
            DataBind();
        }

        public void RefreshFromBrandAndSelect(int brandId, IEnumerable<int> idList)
        {
            BrandId = brandId;
            RefreshFromDataSource();

            foreach (int id in idList)
                SafeSelectValue(id);
        }

        /// <summary>
        /// Selects the list item with the specified value
        /// Does not throw an error if item does not exist
        /// </summary>
        public void SafeSelectValue(int? id)
        {
            ListItem li = Items.FindByValue(id.ToString());
            if (li != null)
            {
                li.Selected = true;
            }
        }
    }
}
