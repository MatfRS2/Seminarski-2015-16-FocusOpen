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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
    public abstract class AbstractNestedDataDropDownList<T> : AbstractDropDownList, IBrandControl, INestedDataSelectable<T>
    {
		public bool AutoSetBrand { get; set; }
        public bool IncludeSpecifyDuringCataloguing { get; set; }

		#region IBrandControl Implementation

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

		public void RefreshFromBrand(int brandId)
		{
			BrandId = brandId;
			RefreshFromDataSource();
		}

		public void RefreshFromBrandAndSelect(int brandId, object val)
		{
			RefreshFromBrand(brandId);
			SafeSelectValue(val);
		}

		#endregion

		#region Abstract Methods

        public abstract NestedDataControlHelper<T> NestedDataHelper { get; }
        public abstract IEnumerable<T> GetParentList();
        public abstract IEnumerable<T> GetChildren(T entity);
		public abstract string GetEntityName(T entity);
		public abstract int GetEntityId(T entity);

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
            var lookupList = (List<LookupItem>)NestedDataHelper.GetDataSource();

            if (IncludeSpecifyDuringCataloguing)
                lookupList.Insert(0, new LookupItem(int.MinValue, "Specify during cataloguing"));

		    return lookupList;

//			IEnumerable<T> parentList = GetParentList();
//			List<LookupItem> lookupList = new List<LookupItem>();
//
//			foreach (T entity in parentList)
//                NestedDataHelper.AddEntity(entity, 0, lookupList);
//
//            if (IncludeSpecifyDuringCataloguing)
//                lookupList.Insert(0, new LookupItem(int.MinValue, "Specify during cataloguing"));
//
//			return lookupList;
		}

        public override string GetDataTextField()
        {
            return NestedDataHelper.GetDataTextField();
            //			return "Name";
        }

        public override string GetDataValueField()
        {
            return NestedDataHelper.GetDataValueField();
            //			return "Id";
        }

		#endregion

		#region Overrides

		protected override void OnInit(System.EventArgs e)
		{
			if (AutoSetBrand)
				BrandId = WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();

			base.OnInit(e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			foreach (ListItem item in Items)
				item.Text = item.Text.Replace(" ", HttpUtility.HtmlDecode("&nbsp;"));

			base.Render(writer);
		}

		#endregion
    }
}
