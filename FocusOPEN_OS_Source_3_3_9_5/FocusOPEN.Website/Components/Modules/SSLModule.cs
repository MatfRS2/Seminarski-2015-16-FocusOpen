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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	public class SSLModule : IHttpModule
	{
		#region Private variables

		private HttpApplication m_HttpApplication;

		#endregion

		#region Accessors

		public static bool UseSSL { get; set; }

		#endregion
		
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

		#region Handled Events in Http Pipeline

		private void OnPreRequestHandlerExecute(object sender, EventArgs e)
		{
			string path = GetRelativePath();

			// We only want to check ASPX pages
			// (Don't really care about checking ashx/axd/etc pages as they don't need to be secured)
			if (StringUtils.GetFileExtension(path) == "aspx")
			{
				SecureSiteCheck();
			}
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets a list of all the paths to be secured
		/// </summary>
		private List<String> SecurePages
		{
			get
			{
				List<String> list = Context.Cache.Get("SSLModule_SecurePages") as List<String>;

				if (list == null)
				{
					// Create empty list
					list = new List<string>();

					// Path to XML file so we can load doc and create a cache dependency for this file later
					string securePagesXmlPath = Context.Server.MapPath("~/Config/SecurePages.Config");

					if (File.Exists(securePagesXmlPath))
					{
						// Open doc and get nodes
						XmlDocument doc = new XmlDocument();
						doc.Load(securePagesXmlPath);
						XmlNodeList nodes = doc.SelectNodes("/SSL/Paths/Path");

						// Ensure we have nodes
						if (nodes != null)
						{
							// Add the nodes to the list
							list.AddRange(from XmlNode node in nodes
							              select node.InnerText.ToLower());

							// Cache the list with file dependency to force reload if file changes
							CacheDependency cacheDependency = new CacheDependency(securePagesXmlPath);
							Context.Cache.Insert("SSLModule_SecurePages", securePagesXmlPath, cacheDependency);
						}
					}
				}

				return list;
			}
		}

		/// <summary>
		/// Gets a list of all the exceptions which do not need to be
		/// secured, even if they appear in the secure pages list
		/// </summary>
		private List<String> Exceptions
		{
			get
			{
				List<String> list = Context.Cache.Get("SSLModule_Exceptions") as List<String>;

				if (list == null)
				{
					// Create empty list
					list = new List<string>();

					// Path to XML file so we can load doc and create a cache dependency for this file later
					string securePagesXmlPath = Context.Server.MapPath("~/Config/SecurePages.Config");

					if (File.Exists(securePagesXmlPath))
					{
						// Open doc and get nodes
						XmlDocument doc = new XmlDocument();
						doc.Load(securePagesXmlPath);
						XmlNodeList nodes = doc.SelectNodes("/SSL/Exceptions/Exception");

						// Ensure we have nodes
						if (nodes != null)
						{
							// Add the nodes to the list
							list.AddRange(from XmlNode node in nodes
							              select node.InnerText.ToLower());

							// Cache the list with file dependency to force reload if file changes
							CacheDependency cacheDependency = new CacheDependency(securePagesXmlPath);
							Context.Cache.Insert("SSLModule_Exceptions", securePagesXmlPath, cacheDependency);
						}
					}
				}

				return list;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets the current path converted to a relative path and lowercased
		/// </summary>
		private string GetRelativePath()
		{
			return VirtualPathUtility.ToAppRelative(Request.Url.AbsolutePath).ToLower();
		}

		private void SecureSiteCheck()
		{
			if (UseSSL && SecurePages.Count > 0)
			{
				string path = GetRelativePath();

				if (IsException(path))
					return;

				if (PathRequiresSecureConnection(path) && !Request.IsSecureConnection)
				{
					string uri = Request.Url.ToString().ToLower().Replace("http://", "https://");
					Response.Redirect(uri);
				}

				if (!PathRequiresSecureConnection(path) && Request.IsSecureConnection)
				{
					string uri = Request.Url.ToString().ToLower().Replace("https://", "http://");
					Response.Redirect(uri);
				}
			}
		}

		private bool IsException(string path)
		{
			// Check if this exact path is listed as an exception
			if (Exceptions.Contains(path))
				return true;

			// Get list of all exception paths (ending with a *)
			List<String> exceptionPaths = Exceptions.FindAll(page => page.EndsWith("*"));

			// Iterate through the exceptions
			foreach (string page in exceptionPaths)
			{
				// Get the path without the preceding star
				string pg = page.Substring(0, page.Length - 1);

				// Check for a match
				if (path.StartsWith(pg, true, CultureInfo.InvariantCulture))
					return true;
			}

			// Not an exception
			return false;
		}

		/// <summary>
		/// Checks if the specified path is configured as a secure path which required SSL access.
		/// </summary>
		private bool PathRequiresSecureConnection(string path)
		{
			// Check if this exact path is listed as a secure path
			if (SecurePages.Contains(path))
				return true;

			// Get all secure paths (these are stored as secure pages, but end with *)
			// Eg. to secure the admin section, we'd have ~/admin/*
			List<String> securePaths = SecurePages.FindAll(page => page.EndsWith("*"));

			// Iterate through the secure paths
			foreach (string page in securePaths)
			{
				// Get the path without the preceding star
				// Eg ~/admin/* => ~/admin/
				string pg = page.Substring(0, page.Length - 1);

				// Check if the current path starts with this
				if (path.StartsWith(pg, true, CultureInfo.InvariantCulture))
					return true;
			}

			// None matched so return false
			return false;
		}

		#endregion
	}
}