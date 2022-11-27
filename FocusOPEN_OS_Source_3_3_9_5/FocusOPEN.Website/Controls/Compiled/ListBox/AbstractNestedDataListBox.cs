using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
    public abstract class AbstractNestedDataListBox<T> : AbstractListBox, INestedDataSelectable<T>
    {
        
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
		    return NestedDataHelper.GetDataSource();
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
