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
using System.Linq;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public sealed class BrandCheckBoxList : CheckBoxList
	{
		/// <summary>
		/// Boolean value specifying whether all brands should be displayed
		/// or only those accessible to the currently logged in user.
		/// </summary>
		public bool Restrict { get; set; }

		public BrandCheckBoxList()
		{
			RepeatDirection = RepeatDirection.Vertical;
		}

		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);

			DataSource = GetBrandList();
			DataTextField = Brand.Columns.Name.ToString();
			DataValueField = Brand.Columns.BrandId.ToString();
			DataBind();
		}

		private List<Brand> GetBrandList()
		{
			List<Brand> list = new List<Brand>();
			list.AddRange(BrandCache.Instance.GetList());

			if (Restrict)
			{
				// List is restricted so remove brands that the user cannot access
				list.RemoveAll(b => !SessionInfo.Current.User.CanAccessBrand(b.BrandId.GetValueOrDefault()));
			}

			list.Sort((a, b) => a.Name.CompareTo(b.Name));
			return list;
		}

		internal void SelectItem(int brandId)
		{
			ListItem item = Items.FindByValue(brandId.ToString());

			if (item != null)
				item.Selected = true;
		}

		internal List<int> SelectedItems
		{
			get
			{
				return (from ListItem item in Items
				        where item.Selected
				        select NumericUtils.ParseInt32(item.Value, 0)).ToList();
			}
		}
	}
}