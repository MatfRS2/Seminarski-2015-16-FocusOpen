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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FocusOPEN.Shared
{
	public static class CollectionExtensions
	{
		/// <summary>
		///	Gets the value from the collection or returns an empty string
		/// </summary>
		/// <remarks>
		///	Also allows placeholders.  If a key value is {something}, the value of key "something"
		/// will be returned.  If a value contains ##something##, it will be replaced with the
		/// the value of the something key.
		/// </remarks>
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

		/// <summary>
		/// Gets the value from the collection or returns an empty string
		/// </summary>
		public static int GetIntValue(this NameValueCollection collection, string key, int defaultValue)
		{
			string s = GetValue(collection, key);

			if (string.IsNullOrEmpty(key))
				return defaultValue;

			try
			{
				return Int32.Parse(s);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Gets the value from the collection or returns an empty string
		/// </summary>
		public static bool GetBoolValue(this NameValueCollection collection, string key)
		{
			string s = GetValue(collection, key);

			if (string.IsNullOrEmpty(s))
				return false;

			switch (s.ToLower())
			{
				case "y":
				case "yes":
				case "true":
				case "1":
					return true;

				default:
					return false;
			}
		}

		public static string ToDelimitedList(this IEnumerable collection, char delimiter)
		{
			StringBuilder sb = new StringBuilder();

			foreach (object o in collection)
			{
				string nns = (o == null) ? string.Empty : o.ToString();

				sb.Append(nns);
				sb.Append(delimiter + " ");
			}

			string s = sb.ToString().Trim();

			if (s.EndsWith(delimiter.ToString()))
				s = s.Substring(0, s.Length - 1);

			return s.Trim();
		}

		/// <summary>
		/// Gets a comma delimited list from the collection
		/// </summary>
		public static string ToCommaDelimitedList(this IEnumerable collection)
		{
			return collection.ToDelimitedList(',');
		}

		public static string EmptyValue(this string s, string emptyVal)
		{
			if (string.IsNullOrEmpty(s))
				return emptyVal;

			return s;
		}

		/// <summary>
		/// Checks if the collection contains any of the specified values
		/// </summary>
		public static bool ContainsAny<T>(this IEnumerable<T> collection, params T[] items)
		{
			return items.Any(item => collection.Contains(item));
		}
	}
}