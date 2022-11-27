/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public abstract class AbstractNestedDropDownList<T> : AbstractDropDownList, IBrandControl
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

		protected abstract IEnumerable<T> GetParentList();
		protected abstract IEnumerable<T> GetChildren(T entity);
		protected abstract string GetName(T entity);
		protected abstract int GetId(T entity);

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			IEnumerable<T> parentList = GetParentList();
			List<LookupItem> lookupList = new List<LookupItem>();

			foreach (T entity in parentList)
				AddEntity(entity, 0, lookupList);

            if (IncludeSpecifyDuringCataloguing)
                lookupList.Insert(0, new LookupItem(int.MinValue, "Specify during cataloguing"));

			return lookupList;
		}

		public override string GetDataTextField()
		{
			return "Name";
		}

		public override string GetDataValueField()
		{
			return "Id";
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

		#region Private Helper Methods

		private void AddEntity(IEnumerable<T> entities, int level, List<LookupItem> data)
		{
			foreach (T entity in entities)
				AddEntity(entity, level, data);
		}

		private void AddEntity(T entity, int level, List<LookupItem> data)
		{
			AddSingleEntity(entity, level, data);
			AddEntity(GetChildren(entity), level + 1, data);
		}

		private void AddSingleEntity(T entity, int level, ICollection<LookupItem> data)
		{
			string prefix = GetString(" ", level * 3);
			string name = prefix + GetName(entity);
			int id = GetId(entity);

			LookupItem li = new LookupItem(id, name);
			data.Add(li);
		}

		private static string GetString(string s, int count)
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < count; i++)
				sb.Append(s);

			return sb.ToString();
		}

		#endregion

		#region Nested Class: LookupItem

		private class LookupItem
		{
			public int Id { get; set; }
			public string Name { get; set; }

			public LookupItem(int id, string name)
			{
				Id = id;
				Name = name;
			}
		}

		#endregion
	}
}
