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
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace FocusOPEN.APS
{
	/// <summary>
	/// Various extension methods
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Wraps the string in quotes if it isn't already
		/// </summary>
		public static string WrapInQuotes(this string s)
		{
			if (!s.StartsWith("\"") && !s.EndsWith("\""))
				s = string.Format("\"{0}\"", s);

			return s;
		}

		public static T GetAttribute<T>(this XElement element, string name, T defaultVal)
		{
			XAttribute attribute = element.Attribute(name);

			if (attribute == null)
				return defaultVal;

			string val = attribute.Value;

			try
			{
				return (T)Convert.ChangeType(val, typeof(T));
			}
			catch (Exception)
			{
				return defaultVal;
			}
		}

		public static bool IsSet(this Enum input, Enum matchTo)
		{
			return (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;
		}

		public static bool ContainsKey (this NameValueCollection collection, string key)
		{
			return (collection[key] != null);
		}

		public static string GetValue(this NameValueCollection collection, string key)
		{
			// First check the collection
			string value = collection[key];

			// Return empty string if nothing found
			if (value == null)
				return string.Empty;

			// Check if this is a placeholder, and if so, get the value
			if (value.StartsWith("{") && value.EndsWith("}"))
			{
				string newKey = value.Substring(1, value.Length - 2);
				value = GetValue(collection, newKey);
			}

			// Check if we need to replace any placeholders
			Regex regex = new Regex(@"##(?<Key>(?:(?!##).)*)##");
			Match match = regex.Match(value);

			if (match.Success)
			{
				string insertVal = GetValue(collection, match.Groups["Key"].Value);
				value = value.Replace(match.Value, insertVal);
			}

			return value;
		}
	}
}
