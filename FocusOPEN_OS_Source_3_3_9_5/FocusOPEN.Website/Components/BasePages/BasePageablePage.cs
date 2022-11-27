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
using System.Web.UI.WebControls;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Base class for secured front-end pages with paging.
	/// Contains easy accessors for storing current page, total page,
	/// and asset count in the viewstate so that we can manage
	/// these across postbacks and show this info on ASPX pages.
	/// </summary>
	public abstract class BasePageablePage : BaseSecuredPage
	{
		#region Accessors

		/// <summary>
		/// Returns a 1-based index of the current page
		/// </summary>
		public int CurrentPage
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "CurrentPage", 0);
			}
			set
			{
				ViewState["CurrentPage"] = (value <= 0) ? 1 : value;
			}
		}

		public int TotalPages
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "TotalPages", 0);
			}
			set
			{
				ViewState["TotalPages"] = (value <= 0) ? 1 : value;
			}
		}

		public int AssetCount
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "AssetCount", 0);
			}
			set
			{
				ViewState["AssetCount"] = value;
			}
		}

		protected int PageSize
		{
			get
			{
				DropDownList dd = (DropDownList) SiteUtils.FindControlRecursive(this, "PageSizeDropDownList1");
				return Convert.ToInt32(dd.SelectedValue);
			}
		}

		#endregion
	}
}