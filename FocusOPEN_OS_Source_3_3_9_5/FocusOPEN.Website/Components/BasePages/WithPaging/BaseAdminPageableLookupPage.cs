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
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Components
{
	public abstract class BaseAdminPageableLookupPage : BaseAdminPageablePage
	{
		protected abstract SortableDataGrid Grid { get; }

		protected abstract void BindGrid(int page);
		protected abstract void Grid_ItemDataBound(object sender, DataGridItemEventArgs e);
		protected abstract void Grid_UpdateCommand(object source, DataGridCommandEventArgs e);
		protected abstract void Grid_DeleteCommand(object source, DataGridCommandEventArgs e);
		protected abstract void Grid_ItemCommand(object source, DataGridCommandEventArgs e);

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			PageSizeDropDownList PageSizeDropDownList1 = (PageSizeDropDownList) SiteUtils.FindControlRecursive(this, "PageSizeDropDownList1");
			PageSizeDropDownList1.AutoPostBack = true;
			PageSizeDropDownList1.SelectedIndexChanged += new EventHandler(PageSizeDropDownList1_SelectedIndexChanged);

			Grid.SortCommand += new DataGridSortCommandEventHandler(Grid_SortCommand);
			Grid.EditCommand += new DataGridCommandEventHandler(Grid_EditCommand);
			Grid.CancelCommand += new DataGridCommandEventHandler(Grid_CancelCommand);
			Grid.ItemDataBound += new DataGridItemEventHandler(Grid_ItemDataBound);
			Grid.UpdateCommand += new DataGridCommandEventHandler(Grid_UpdateCommand);
			Grid.DeleteCommand += new DataGridCommandEventHandler(Grid_DeleteCommand);
			Grid.ItemCommand += new DataGridCommandEventHandler(Grid_ItemCommand);
		}

		protected void PageSizeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindGrid(1);
		}

		// This method is not attached from OnInit here as doing so causes some strange bugs
		// (Possibly because of something within the SimplePager control).
		protected void SimplePager_OnCommand(object sender, CommandEventArgs e)
		{
			int pg = Convert.ToInt32(e.CommandArgument);
			BindGrid(pg);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Page.IsPostBack)
				BindGrid(1);
		}

		/// <summary>
		/// Resets the edit item index and rebinds the grid to the current page
		/// </summary>
		protected void ResetGridState()
		{
			Grid.EditItemIndex = -1;
			BindGrid(CurrentPage);
			Grid.ShowFooter = true;
		}

		#region Grid Events

		protected void Grid_SortCommand(object source, DataGridSortCommandEventArgs e)
		{
			ResetGridState();
			BindGrid(1);
		}

		protected void Grid_EditCommand(object source, DataGridCommandEventArgs e)
		{
			Grid.EditItemIndex = e.Item.ItemIndex;
			BindGrid(CurrentPage);
			Grid.ShowFooter = false;
		}

		protected void Grid_CancelCommand(object source, DataGridCommandEventArgs e)
		{
			ResetGridState();
		}

		#endregion
	}
}