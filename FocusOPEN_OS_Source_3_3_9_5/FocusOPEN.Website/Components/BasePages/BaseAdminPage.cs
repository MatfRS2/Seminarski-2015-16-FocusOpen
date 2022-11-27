/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Text.RegularExpressions;
using System.Web;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Forms the base of all admin pages.
	/// </summary>
	public abstract class BaseAdminPage : BaseSecuredPage
	{
		#region Private Variables

		private string m_PageId = string.Empty;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the page ID, used to determine whether the item
		/// should be higlighted on the admin navigation bar.
		/// </summary>
		public string PageId
		{
			get { return m_PageId; }
			set { m_PageId = value; }
		}

		#endregion

		#region Constructor

		public BaseAdminPage()
		{
			PageId = GetPageId();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Calculates the page ID based on folder and filename
		/// </summary>
		/// <remarks>
		/// This is a short-cut way of doing things, and avoids having to specify the page ID in
		/// every single page.  However, for cases where the page ID does need to be manually
		/// specified (eg. because the folder and filename don't match), simply set the PageId
		/// property in the relevant page constructor.
		/// 
		/// For automatic pageId calculated, we assume a structure of '/admin/[sectionId]/[pageId].aspx'
		/// which gets converted into (and returned as) a page ID of [sectionId].[pageId]
		/// 
		/// Eg. The page '/admin/Users/ViewUserList.aspx' would return 'Users.ViewUserList'
		/// </remarks>
		private static string GetPageId()
		{
			string returnVal = string.Empty;
			string url = HttpContext.Current.Request.Path;

			const string pattern = @"/admin/(?<sectionId>[^/]+)/(?<pageId>[^.]+).aspx";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			Match match = regex.Match(url);

			if (match.Groups.Count == 3)
			{
				string sectionId = match.Groups["sectionId"].Value;
				string pageId = match.Groups["pageId"].Value;

				returnVal = string.Format("{0}.{1}", sectionId, pageId);
			}

			return (returnVal);
		}

		#endregion

		/// <summary>
		/// Gets the required value from the querystring.
		/// If it doesn't exist, redirects to redirectUrl.
		/// </summary>
		protected int GetRequiredQueryStringParameter(string key, string redirectUrl)
		{
			int val = WebUtils.GetIntRequestParam(key, -1);

			if (val == -1)
			{
				Page.Response.Redirect(redirectUrl, false);
				return 0;
			}

			return val;
		}

		protected void SetPageTitle(string title)
		{
			Page.Title = string.Concat(title, " [ADMIN]");
		}
	}
}