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
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Xml;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components
{
	public static class AdminNavigationManager
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Private Accessors

		private static IEnumerable<AdminSection> AdminSections
		{
			get
			{
				List<AdminSection> adminSections = HttpContext.Current.Cache["AdminSections"] as List<AdminSection>;

				if (adminSections == null)
				{
					adminSections = new List<AdminSection>();

					string filename = HttpContext.Current.Server.MapPath("~/Config/AdminNavigation.Config");
					m_Logger.InfoFormat("AdminSections cache is null.  Reloading nodes from '{0}'", filename);

					XmlDocument doc = new XmlDocument();
					doc.Load(filename);
					XmlNodeList sections = doc.SelectNodes("//Section");

					if (sections == null)
						throw new SystemException("No admin sections found in config file");

					m_Logger.InfoFormat("Found {0} admin sections", sections.Count);

					foreach (XmlNode sectionNode in sections)
					{
						AdminSection section = new AdminSection(sectionNode);
						m_Logger.InfoFormat("- {0}", section.Name);

						foreach (XmlNode pageNode in sectionNode.ChildNodes)
						{
							AdminSectionPage adminSectionPage = new AdminSectionPage(pageNode);
							section.AdminSectionPages.Add(adminSectionPage);
							m_Logger.InfoFormat("-- {0}", adminSectionPage.Name);
						}

						adminSections.Add(section);
					}

					m_Logger.InfoFormat("Done loading admin sections");

					CacheDependency dependency = new CacheDependency(filename);
					HttpContext.Current.Cache.Insert("AdminSections", adminSections, dependency);

					m_Logger.DebugFormat("Added admin sections to cache");
				}

				return (adminSections);
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets the AdminSectionPage with the specified id.  If it does not exist, returns null.
		/// </summary>
		public static AdminSectionPage GetAdminSectionPageById(string pageId)
		{
			return AdminSections.SelectMany(adminSection => adminSection.AdminSectionPages).FirstOrDefault(adminSectionPage => StringUtils.IgnoreCaseCompare(adminSectionPage.Id, pageId));
		}

		/// <summary>
		/// Gets the AdminSectionPage with the specified url.  If it does not exist, returns null.
		/// </summary>
		public static AdminSectionPage GetAdminSectionPageByUrl(string url)
		{
			return (from adminSection in AdminSections
			        from adminSectionPage in adminSection.AdminSectionPages
			        select Search(adminSectionPage, url)).FirstOrDefault(page => page != null);
		}

		public static IEnumerable<AdminSection> GetAdminSectionsForUser(User user)
		{
			List<AdminSection> list = new List<AdminSection>();

			foreach (AdminSection adminSection in AdminSections)
			{
				// Get admin section pages in this section that are accessible by the specified user
				List<AdminSectionPage> adminSectionPages = GetAdminSectionPages(adminSection, user);

				// Only go ahead and add the section if the user can access pages inside it.  Otherwise,
				// there's no point in adding it (as the section will be empty to the user).
				if (adminSectionPages.Count > 0)
				{
					// Get a copy of the admin section object
					// This will only return the basic object without child pages, so we don't need to clear the child pages first.
					// Getting some strange behaviour with the MemberwiseClone method.  See comments in Clone method for more info.
					AdminSection adminSectionClone = (AdminSection) adminSection.Clone();

					// Add back in the pages accessible in this section to the specified user
					adminSectionClone.AdminSectionPages.AddRange(adminSectionPages);

					// Add the admin section to the list
					list.Add(adminSectionClone);
				}
			}

			return (list);
		}

		#endregion

		#region Private Helper Methods

		/// <summary>
		/// Searches the AdminSectionPage and all its subpages for a page
		/// matching the specified url.  Returns page if found. Otherwise, null.
		/// </summary>
		private static AdminSectionPage Search(AdminSectionPage page, string url)
		{
			if (StringUtils.IgnoreCaseCompare(page.Url, url))
				return page;

			return page.Pages.Select(subPage => Search(subPage, url)).FirstOrDefault(p => p != null);
		}

		/// <summary>
		/// Gets the list of admin section pages for the specified section, that are
		/// accessible by the specified user
		/// </summary>
		private static List<AdminSectionPage> GetAdminSectionPages(AdminSection adminSection, User user)
		{
			return (from page in adminSection.AdminSectionPages
			        where SecurityManager.UserHasAccess(user, page)
			        select page).ToList();
		}

		#endregion
	}

	#region Related classes

	/// <summary>
	/// Represents an admin navigation base object, containing
	/// properties common to admin navigation section AND pages.
	/// </summary>
	public abstract class BaseAdminSection
	{
		public readonly string Id;
		public readonly string Name;
		public readonly bool VisibleOnNavigation;
		public readonly bool RestrictToEngineers;

		protected BaseAdminSection(string id, string name, bool visible, bool engineersOnly)
		{
			Id = id;
			Name = name;
			VisibleOnNavigation = visible;
			RestrictToEngineers = engineersOnly;
		}

		protected BaseAdminSection(XmlNode node)
		{
			if (node.Attributes == null || node.Attributes.Count == 0)
				throw new SystemException("Navigation XML Node does not contain any attributes");

			Id = node.Attributes.GetNamedItem("Id").Value;
			Name = node.Attributes.GetNamedItem("Name").Value;

			XmlNode visibleOnNavigation = node.Attributes.GetNamedItem("VisibleOnNavigation");
			VisibleOnNavigation = (visibleOnNavigation == null) ? true : Convert.ToBoolean(visibleOnNavigation.Value);

			XmlNode restrictToEngineers = node.Attributes.GetNamedItem("RestrictToEngineers");
			RestrictToEngineers = (restrictToEngineers == null) ? false : Convert.ToBoolean(restrictToEngineers.Value);
		}
	}

	/// <summary>
	/// Represents an admin section
	/// </summary>
	public class AdminSection : BaseAdminSection, ICloneable
	{
		/// <summary>
		/// List of admin section pages in this section
		/// </summary>
		internal readonly List<AdminSectionPage> AdminSectionPages = new List<AdminSectionPage>();

		private AdminSection(string id, string name, bool visible, bool engineersOnly) : base(id, name, visible, engineersOnly)
		{
		}

		public AdminSection(XmlNode node) : base(node)
		{
		}

		#region ICloneable Implementation

		public object Clone()
		{
			// For some odd reason, MemberwiseClone() doesn't seem to
			// return a copy of the object here, so we're doing it manually.
			// Only the Id and Name is copied, not the admin section page list
			// as we don't need this for our purposes.

			return new AdminSection(Id, Name, VisibleOnNavigation, RestrictToEngineers);
		}

		#endregion
	}

	/// <summary>
	/// Represents an admin section page
	/// </summary>
	public class AdminSectionPage : BaseAdminSection
	{
		public readonly List<UserRole> UserRoleList = new List<UserRole>();
		public readonly List<AdminSectionPage> Pages = new List<AdminSectionPage>();
		public readonly string Url;

		public AdminSectionPage(XmlNode node) : base(node)
		{
			if (node.Attributes == null)
				throw new SystemException("Node does not have any attributes");

			// Get the page URL, using the ID if no URL is specified
			XmlNode urlNode = node.Attributes.GetNamedItem("Url");
			Url = (urlNode == null) ? string.Format("~/Admin/{0}.aspx", Id.Replace('.', '/')) : urlNode.Value;

			// Get the minimum role and/or allowed roles to access this page
			XmlNode minimumRoleNode = node.Attributes.GetNamedItem("MinimumRole");
			XmlNode allowedRolesNode = node.Attributes.GetNamedItem(("AllowedRoles"));

			// If no minimum role or allowed roles are specified, then search through
			// the parents until we find this information.
			if (minimumRoleNode == null && allowedRolesNode == null)
			{
				// Get the parent node.  This will be another <Page> node or a <Section> node
				XmlNode parentNode = node.ParentNode;

				while (parentNode != null && parentNode.Attributes != null)
				{
					// Get the nodes
					minimumRoleNode = parentNode.Attributes.GetNamedItem("MinimumRole");
					allowedRolesNode = parentNode.Attributes.GetNamedItem(("AllowedRoles"));

					// Drop out if we found valid nodes.  Otherwise, keep searching upwards
					if (minimumRoleNode != null || allowedRolesNode != null)
						break;

					// Set parent to this parent and keep searching
					parentNode = parentNode.ParentNode;
				}
			}

			if (minimumRoleNode == null)
			{
				UserRoleList.Add(UserRole.SuperAdministrator);
			}
			else
			{
				foreach (UserRole userRole in Enum.GetValues(typeof(UserRole)))
				{
					int userRoleId = (int) userRole;
					int minimumRoleId = (int) EnumUtils.GetEnumFromValue<UserRole>(minimumRoleNode.Value);

					if (userRoleId >= minimumRoleId)
					{
						UserRoleList.Add(userRole);
					}
				}
			}

			if (allowedRolesNode != null)
			{
				string[] allowedRoles = allowedRolesNode.Value.Split(',');

				foreach (string role in allowedRoles)
				{
					UserRole userRole = EnumUtils.GetEnumFromValue<UserRole>(role);

					if (userRole != 0 && !UserRoleList.Contains(userRole))
						UserRoleList.Add(userRole);
				}
			}

			XmlNodeList pages = node.SelectNodes("Page");

			if (pages != null)
			{
				foreach (XmlNode pageNode in pages)
				{
					AdminSectionPage page = new AdminSectionPage(pageNode);
					Pages.Add(page);
				}
			}

			// Add all user roles from children pages to this too
			// (Otherwise, this node won't show up in navigation
			// and the children won't be accessible).
			UserRoleList.AddRange(GetUserRoleList());
		}

		private IEnumerable<UserRole> GetUserRoleList()
		{
			List<UserRole> list = new List<UserRole>();

			list.AddRange(UserRoleList);

			foreach (AdminSectionPage page in Pages)
				list.AddRange(page.GetUserRoleList());

			return list.Distinct().ToList();
		}
	}

	#endregion
}