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
using Daydream.Data;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Forms the base of all admin pages using paging
	/// </summary>
	public class BaseAdminPageablePage : BaseAdminPage
	{
		#region Accessors

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

		public int RecordCount
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "RecordCount", 0);
			}
			set
			{
				ViewState["RecordCount"] = value;
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

		protected void UpdatePagingControls(PagingInfo pagingInfo)
		{
			// Get each pager
			SimplePager TopPager = (SimplePager) SiteUtils.FindControlRecursive(this, "TopPager");
			SimplePager BottomPager = (SimplePager) SiteUtils.FindControlRecursive(this, "BottomPager");

			// Update the top pager
			TopPager.CurrentPage = pagingInfo.CurrentPage + 1;
			TopPager.PageSize = pagingInfo.PageSize;
			TopPager.PageCount = pagingInfo.TotalPages;
			TopPager.ItemCount = pagingInfo.TotalRecords;

			// Update the bottom pager
			BottomPager.CurrentPage = TopPager.CurrentPage;
			BottomPager.PageSize = TopPager.PageSize;
			BottomPager.PageCount = TopPager.PageCount;
			BottomPager.ItemCount = TopPager.ItemCount;

			// Set page-based paging info
			CurrentPage = TopPager.CurrentPage;
			TotalPages = pagingInfo.TotalPages;
			RecordCount = pagingInfo.TotalRecords;
		}
	}
}