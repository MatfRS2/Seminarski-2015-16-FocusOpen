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
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	public class SecurityModule : IHttpModule
	{
		private HttpApplication m_HttpApplication;

		#region Convenience Wrappers

		private HttpContext Context
		{
			get
			{
				return m_HttpApplication.Context;
			}
		}

		private HttpRequest Request
		{
			get
			{
				return Context.Request;
			}
		}

		private HttpResponse Response
		{
			get
			{
				return Context.Response;
			}
		}

		#endregion

		#region IHttpModule Implementation

		public void Init(HttpApplication context)
		{
			m_HttpApplication = context;
			m_HttpApplication.PreRequestHandlerExecute += new EventHandler(OnPreRequestHandlerExecute);
		}

		public void Dispose()
		{
			// Do Nothing
		}

		#endregion

		private void OnPreRequestHandlerExecute(object sender, EventArgs e)
		{
			string filename = Path.GetFileName(Request.Path) ?? string.Empty;

			// Secure all other ASPX pages
			if (StringUtils.GetFileExtension(Request.Path) == "aspx")
			{
				// Check if the user can access the requested page
				bool userHasAccess = SecurityManager.UserHasAccess(SessionInfo.Current.User, Request.Url);

				// Redirect to the login page if user does not have access
				if (!userHasAccess)
					RedirectToLoginPage();
			}
			else if (Regex.IsMatch(filename, @"assetfile.\d+.ashx", RegexOptions.IgnoreCase) && SessionInfo.Current.User.IsNull)
			{
				// If the user is requesting an asset file but is not logged in
				// then redirect to the login page.

				RedirectToLoginPage();
			}
		}

		#region Private Helper Methods

		private void RedirectToLoginPage()
		{
			// The base url to the login page
			string loginPath = "~/Login.aspx?check=1";

			// A non-null user means that the user got caught trying to access a page
			// to which they do not have acess due to an insufficient user role
			if (!SessionInfo.Current.User.IsNull)
				loginPath += "&message=AccessDenied";

			// Append the current url so we know where to return to
			loginPath += "&returnUrl=" + UrlEncoder.Encode(Request.RawUrl);

			// Redirect to the login page
			Response.Redirect(loginPath, true);
		}

		#endregion

	}
}