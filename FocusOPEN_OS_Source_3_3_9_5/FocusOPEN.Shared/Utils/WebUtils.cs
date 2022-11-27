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
using System.Text;
using System.Web;
using System.IO;

namespace FocusOPEN.Shared
{
	public static class WebUtils
	{
		#region Private Accessors

		private static HttpRequest Request
		{
			get
			{
				return HttpContext.Current.Request;
			}
		}

		#endregion

		public static string GetRequestParam(string key)
		{
			string ret = Request.QueryString[key] ?? Request.Form[key];
			return ret;
		}

		public static string GetRequestParam(string key, string defaultValue)
		{
			string ret = Request.QueryString[key] ?? Request.Form[key];
			return ret ?? defaultValue;
		}

		public static Int32 GetIntRequestParam(string key, int defaultValue)
		{
			string val = GetRequestParam(key, defaultValue.ToString());
			try
			{
				return Int32.Parse(val);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static bool? GetNullableBoolRequestParam(string key)
		{
			string value = GetRequestParam(key);

			if (value == null)
				return null;

			switch (value.ToLower())
			{
				case "1":
				case "true":
				case "yes":
					return true;

				case "0":
				case "false":
				case "no":
					return false;

				default:
					return null;
			}
		}

		public static string GetBaseUrl()
		{
			StringBuilder url = new StringBuilder();
			url.Append(Request.Url.Scheme);
			url.Append("://");
			url.Append(Request.Url.Host);
			
			if (Request.Url.Port != 80)
			{
				url.Append(":");
				url.Append(Request.Url.Port);
			}

			url.Append(Request.ApplicationPath);

			return url.ToString().EnsureEndsWith("/");
		}

		public static string GetApplicationPath()
		{
			return (Request.ApplicationPath ?? string.Empty).EnsureEndsWith("/");
		}

        /// <summary>
        /// Gets the full path to the web applications subfolder.
        /// </summary>
        public static string GetSubFolderPath(string subFolder)
        {
            if (!Path.IsPathRooted(subFolder))
            {
                //make sure path is specified relative to the web app root
                if (!subFolder.StartsWith("~"))
                {
                    if (!subFolder.StartsWith("/")) { subFolder = "/" + subFolder; }
                    subFolder = "~" + subFolder;
                }

                subFolder = HttpContext.Current.Server.MapPath(subFolder);
            }
            return subFolder;
        }
	}
}