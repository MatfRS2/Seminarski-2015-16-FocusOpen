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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public partial class AdminNavigation : UserControl
	{
		#region Private variables

		private const string m_BreadcrumbsSpacer = "&nbsp;&gt;&nbsp;";
		private bool m_Initialised = false;
		private DropDownMenuItem m_CurrentItem;
		private string m_CurrentPageId = string.Empty;

		#endregion

		#region Accessors

		public static string BreadcrumbsSpacer
		{
			get
			{
				return m_BreadcrumbsSpacer;
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			InitialiseMenu();
		}

		#region Private Helper Methods

		private void InitialiseMenu()
		{
			// Don't bother initialising again if we've done it already
			if (m_Initialised)
				return;

			// Assume we're initialised now, as we're going to
			// run the initialise process below to do stuff.
			m_Initialised = true;

			// Add the basic admin pages.  These are available to all users
			// so we don't bother to include them in the Admin Navigation XML
			// file, as this is not required.
			DropDownMenu1.Items.Add(new DropDownMenuItem("&laquo; Catalogue", "~/SearchResults.aspx"));
			DropDownMenu1.Items.Add(new DropDownMenuItem("Admin Home", "~/Admin/Default.aspx"));

			// Get the parent page, which has the ID of the selected navigation item
			BaseAdminPage parentPage = (BaseAdminPage)Page;
			m_CurrentPageId = parentPage.PageId.ToLower();

			// Get a list of all the admin sections available to the current user
			IEnumerable<AdminSection> adminSections = AdminNavigationManager.GetAdminSectionsForUser(SessionInfo.Current.User);

			foreach (AdminSection adminSection in adminSections)
			{
				// Create the root level menu item.  This will correspond to
				// each AdminSection specified in the Admin Navigation XML file
				DropDownMenuItem rootMenuItem = new DropDownMenuItem { Text = adminSection.Name, Visible = adminSection.VisibleOnNavigation};
				
				// Now iterate through all of the pages in the admin section and add
				// these to the item.  This will also add any child pages.
				foreach (AdminSectionPage adminSectionPage in adminSection.AdminSectionPages)
					AddItems(rootMenuItem, adminSectionPage);

				var visibleCount = (from item in rootMenuItem.Items
				                    where (item.Visible)
				                    select item).Count();

				if (visibleCount > 0)
					DropDownMenu1.Items.Add(rootMenuItem);
			}

			// Assume we're on the homepage if no page ID is specified
			if (parentPage.PageId == string.Empty)
				m_CurrentItem = DropDownMenu1.Items[1];
		}

		/// <summary>
		/// Adds the page and any of its child pages to the menu item
		/// </summary>
		private void AddItems(DropDownMenuItem parentMenuItem, AdminSectionPage page)
		{
			DropDownMenuItem menuItem = new DropDownMenuItem { Text = page.Name, NavigateUrl = page.Url, Visible = page.VisibleOnNavigation};
			parentMenuItem.Items.Add(menuItem);

			if (page.Id.ToLower() == m_CurrentPageId)
			{
				parentMenuItem.CssClass = "focused";
				menuItem.CssClass = "focused";
				m_CurrentItem = menuItem;
			}

			// Now add all of the subpages via recursion
			// and build up the menu tree
			foreach (AdminSectionPage subPage in page.Pages)
				AddItems(menuItem, subPage);
		}

		#endregion

		#region Breadcrumbs Methods

		public string GetBreadCrumbs()
		{
			InitialiseMenu();

			IList pathList = new ArrayList();

			if (m_CurrentItem != null)
			{
				pathList.Add(m_CurrentItem);

				if (!(m_CurrentItem.Owner is DropDownMenu))
				{
					IDropDownMenuItemContainer currentParent = m_CurrentItem.Owner;

					while (!(currentParent is DropDownMenu) && currentParent != null)
					{
						pathList.Add(currentParent);
						currentParent = currentParent.Owner;
					}
				}
			}

			if (pathList.Count <= 1)
				return string.Empty;

			StringBuilder breadCrumbBuilder = new StringBuilder();

			for (int i = (pathList.Count - 1); i >= 0; i--)
			{
				breadCrumbBuilder.Append(((DropDownMenuItem)pathList[i]).Text);

				if (i > 0)
					breadCrumbBuilder.Append(BreadcrumbsSpacer);
			}

			return breadCrumbBuilder.ToString();
		}

		#endregion
	}
}