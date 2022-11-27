/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections;
using System.Web;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Some values in querystrings cause strange behavour (such as ampersands - &)
	/// This class is used to replace unsafe values with safe values, so that redirects
	/// work correctly.
	/// </summary>
	public static class UrlEncoder
	{
		#region Private Variables

		// Hashtable of keys and the values they are to be replaced with
		private static readonly Hashtable m_ReplaceList = new Hashtable();

		#endregion

		#region Constructors

		static UrlEncoder()
		{
			m_ReplaceList["&"] = "|";
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Encodes the specified url, replacing unsafe values
		/// with safe values and UrlEncoding it.
		/// </summary>
		public static string Encode(string url)
		{
			string newUrl = url;

			foreach (string key in m_ReplaceList.Keys)
			{
				string val = m_ReplaceList[key].ToString();
				newUrl = newUrl.Replace(key, val);
			}

			newUrl = HttpUtility.UrlEncode(newUrl);

			return newUrl;
		}

		/// <summary>
		/// Decodes the specified url, replacing safe values
		/// with unsafe values and UrlDecoding it
		/// (This usually needs to be done before an encoded url can be used for redirection)
		/// </summary>
		public static string Decode(string url)
		{
			string newUrl = url;

			foreach (string key in m_ReplaceList.Keys)
			{
				string val = m_ReplaceList[key].ToString();
				newUrl = newUrl.Replace(val, key);
			}

			newUrl = HttpUtility.UrlDecode(newUrl);

			return newUrl;
		}

		#endregion
	}
}