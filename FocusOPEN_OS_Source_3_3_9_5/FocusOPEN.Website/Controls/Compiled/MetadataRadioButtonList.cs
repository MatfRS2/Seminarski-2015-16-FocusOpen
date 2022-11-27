using System;
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls.Compiled
{
    public class MetadataRadioButtonList : RadioButtonList, INestedDataSelectable<Metadata>
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
        public virtual int SelectedId
        {
            get
            {
                try
                {
                    return Convert.ToInt32(SelectedItem.Value);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

		#endregion

		#region Constructor

        public MetadataRadioButtonList()
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

        public void RefreshFromBrandAndSelect(int brandId, int metaId)
        {
            RefreshFromDataSource();
            SelectedValue = metaId.ToString();
        }
    }
}
