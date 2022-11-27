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
using System.Collections.Specialized;
using System.Web;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Static class for managing cookies with web applications
	/// </summary>
	public static class CookieManager
	{
		#region Constructor

		static CookieManager()
		{
			CookieName = "Cookie";
			ExpirationHours = 14;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the name of the cookie
		/// </summary>
		public static string CookieName { get; set;}

		/// <summary>
		/// Gets or sets the number of hours that the cookie should be saved for
		/// </summary>
		public static int ExpirationHours { get; set; }

		/// <summary>
		/// Gets the current web context
		/// </summary>
		private static HttpContext Context
		{
			get
			{
				return (HttpContext.Current);
			}
		}

		#endregion

		#region Private helper methods

		private static HttpCookie GetCookie()
		{
			HttpCookie cookie = Context.Request.Cookies.Get(CookieName);

			if (cookie == null)
				cookie = new HttpCookie(CookieName);

			return (cookie);
		}

		private static void SaveCookie(HttpCookie cookie)
		{
			cookie.Expires = DateTime.Now.AddHours(ExpirationHours);
			Context.Response.AppendCookie(cookie);
		}

		#endregion

		#region Public Methods

		public static bool ContainsValue(string key)
		{
			HttpCookie cookie = GetCookie();
			return (cookie.Values.Get(key) != null);
		}

		public static string GetValue(string key)
		{
			string val = string.Empty;

			HttpCookie cookie = GetCookie();

			if (cookie.Values.Get(key) != null)
				val = cookie.Values.Get(key);

			return (val);
		}

		public static T GetValue<T>(string key, T defaultValue)
		{
			string val = GetValue(key);

			try
			{
				return (T)Convert.ChangeType(val, typeof(T));
			}
			catch
			{
				return defaultValue;
			}
		}

		public static NameValueCollection GetValues()
		{
			HttpCookie cookie = GetCookie();
			return cookie.Values;
		}

		public static void SetValue(string key, object value)
		{
			HttpCookie cookie = GetCookie();
			cookie[key] = value.ToString();
			SaveCookie(cookie);
		}

		public static void ClearValue(string key)
		{
			HttpCookie cookie = GetCookie();
			cookie.Values.Remove(key);
			SaveCookie(cookie);
		}

		public static void DeleteCookie()
		{
			Context.Response.Cookies.Remove(CookieName);
		}

		public static void RefreshCookie()
		{
			HttpCookie cookie = GetCookie();
			SaveCookie(cookie);
		}

		#endregion
	}
}