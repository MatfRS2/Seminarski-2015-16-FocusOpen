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
	/// Forms the base of all admin pages which have paging functionality for entities
	/// (ie, those pages which have search forms and save search criteria in session)
	/// </summary>
	/// <typeparam name="FinderType">Type of finder to use for retrieving data</typeparam>
	/// <typeparam name="EntityType">Type of entity the page is being used to display</typeparam>
	public abstract class BaseAdminPageableEntityListPage<FinderType, EntityType> : BaseAdminPageablePage
		where FinderType : AbstractFinder, new()
		where EntityType : AbstractEntity
	{
		#region Private Variables

		protected bool m_BindOnLoad = true;

		#endregion

		#region Accessors

		/// <summary>
		/// Boolean value specifying whether the resultsgrid should be automatically bound when the page loads.
		/// </summary>
		protected bool BindOnLoad
		{
			get
			{
				return m_BindOnLoad;
			}
			set
			{
				m_BindOnLoad = value;
			}
		}

		#endregion

		#region Abstract Stuff

		protected abstract ISortableControl DataControl { get; }
		protected abstract BaseSavedAdminSearch<FinderType> SavedSearch { get; }
		protected abstract FinderType GetFinderFromForm();
		protected abstract void RetrieveSearchAndBindGrid();
		protected abstract EntityList<EntityType> GetData(FinderType finder, int page);

		#endregion

		#region Overrides

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Page.IsPostBack)
			{
				PageSizeDropDownList PageSizeDropDownList1 = (PageSizeDropDownList) SiteUtils.FindControlRecursive(this, "PageSizeDropDownList1");
				PageSizeDropDownList1.SafeSelectValue(SavedSearch.PageSize);

				if (SavedSearch.HasSearch)
				{
					RetrieveSearchAndBindGrid();
				}
				else if (BindOnLoad)
				{
					PerformSearchAndBindGrid(1);
				}
			}
		}

		#endregion

		#region Handled Events

		protected void SearchButton_Click(object sender, EventArgs e)
		{
			PerformSearchAndBindGrid(1);
		}

		protected void ResetButton_Click(object sender, EventArgs e)
		{
			SavedSearch.Reset();
			Response.Redirect(Request.Path);
		}

		protected void SimplePager_OnCommand(object source, CommandEventArgs e)
		{
			int pg = Convert.ToInt32(e.CommandArgument);
			PerformSearchAndBindGrid(pg);
		}

		protected void PageSizeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			PerformSearchAndBindGrid(1);
		}

		protected void SortListLinkButton_Command(object sender, CommandEventArgs e)
		{
			DataControl.UpdateSortExpression(e.CommandArgument.ToString());
			PerformSearchAndBindGrid(1);
		}

		#endregion

		#region Protected Helper Methods

		/// <summary>
		/// Performs the search and binds the grid
		/// </summary>
		/// <param name="finder">Finder to perform search</param>
		/// <param name="page">Page number - 1 based index</param>
		protected virtual void PerformSearchAndBindGrid(FinderType finder, int page)
		{
			if (page > 0) page--;

			EntityList<EntityType> data = GetData(finder, page);

			BindGrid(data);

			bool isEmptyResultSet = (data.Count == 0);

			Panel NoResultsPanel = (Panel)SiteUtils.FindControlRecursive(this, "NoResultsPanel");
			DataControl.Visible = (!isEmptyResultSet);
			NoResultsPanel.Visible = (isEmptyResultSet);

			TogglePanels(isEmptyResultSet);

			UpdateSavedSearch(finder, page);
			UpdatePagingControls(data.PagingInfo);
		}

		protected virtual void BindGrid(EntityList<EntityType> data)
		{
			DataControl.DataSource = data;
			DataControl.DataBind();
		}

		protected virtual void TogglePanels(bool isEmptyResultSet)
		{
		}

		#endregion

		#region Private Helper Methods

		private void UpdateSavedSearch(FinderType finder, int page)
		{
			SavedSearch.Finder = finder;
			SavedSearch.Page = page;
			SavedSearch.PageSize = PageSize;
			SavedSearch.SortExpression = DataControl.SortExpression;
			SavedSearch.SortAscending = DataControl.SortAscending;
		}

		protected void PerformSearchAndBindGrid(int page)
		{
			FinderType finder = GetFinderFromForm();
			PerformSearchAndBindGrid(finder, page);
		}

		#endregion
	}
}