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
using System.Linq;
using System.Text.RegularExpressions;

namespace FocusOPEN.Shared
{
	public static class GeneralUtils
	{
		/// <summary>
		/// Parses the string to the specified enum.  If no match is made, the default value is returned
		/// </summary>
		/// <typeparam name="T">Enum type</typeparam>
		/// <param name="s">string to parse</param>
		/// <param name="defaultVal">Default enum value to return if string cannot be parsed</param>
		/// <returns>Enum value of parsed string, or default value if string could not be parsed</returns>
		public static T ParseEnum<T>(string s, T defaultVal) where T : struct
		{
			try
			{
				return (T) Enum.Parse(typeof (T), s, true);
			}
			catch
			{
				return defaultVal;
			}
		}

		/// <summary>
		/// Checks if the first value is in any of the subsequent values
		/// </summary>
		/// <param name="val">The value to check</param>
		/// <param name="list">The list of values to check for the source value</param>
		public static bool ValueIsInList<T>(T val, params T[] list)
		{
			return list.Any(listVal => val.ToString() == listVal.ToString());
		}

		/// <summary>
		/// Gets a GUID, all lowercase and without dashes
		/// </summary>
		public static string GetGuid()
		{
			return Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();
		}

		/// <summary>
		/// Splits a string into a sentence.  Eg. AssetNotFound becomes Asset Not Found
		/// </summary>
		public static string SplitIntoSentence(string s)
		{
			if (string.IsNullOrEmpty(s))
				return string.Empty;

			return Regex.Replace(s, "([a-z]{0,})([A-Z])([a-z]+)", "$2$3 ", RegexOptions.Singleline).Trim();
		}

		/// <summary>
		/// Returns the first non-empty string.  If all are empty, returns empty string
		/// </summary>
		public static string GetNonEmptyString(params string[] strings)
		{
			foreach (string s in strings.Where(s => !StringUtils.IsBlank(s)))
				return s;

			return string.Empty;
		}
	}
}