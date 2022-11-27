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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FocusOPEN.Shared
{
	public static class StringUtils
	{
		/// <summary>
		/// Gets the file extension, lowercased and without preceding dot
		/// </summary>
		/// <param name="path">The path.</param>
		public static string GetFileExtension(string path)
		{
			if (!path.Contains("."))
				return path;

			string extension = Path.GetExtension(path) ?? string.Empty;

			extension = extension.Trim().ToLower();

			if (extension.StartsWith("."))
				extension = extension.Substring(1);

			return extension;
		}

		/// <summary>
		/// Check that the supplied string represents a valid email address
		/// </summary>
		public static bool IsEmail(string s)
		{
			if (String.IsNullOrEmpty(s))
				return false;

			const string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
			return Regex.Match(s, pattern, RegexOptions.IgnoreCase).Success;
		}


		/// <summary>
		/// Splits the supplied string into an array of valid email addresses,
		/// using commas or semi-colons as the delimiter. Returns false if input 
		/// contains invalid addresses otherwise returns true.
		/// </summary>
		public static bool SplitEmails(string s, out string[] emailAddresses)
		{
			//split string by delimiters
			string[] checkEmails = s.Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries);

			List<string> emails = new List<string>();

			const string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
			Regex rgx = new Regex(pattern);

			//loop through array and parse out valid email addresses
			bool fAllValid = true;

			foreach (string address in checkEmails)
			{
				Match m = rgx.Match(address);

				if (m.Success)
				{
					emails.Add(m.Value.ToLower());
				}
				else
				{
					fAllValid = false;
				}
			}

			emailAddresses = emails.ToArray();
			return fAllValid;
		}

		/// <summary>
		/// Check whether the supplied string is blank
		/// </summary>
		/// <param name="s">String to check.  Can be null.</param>
		public static bool IsBlank(string s)
		{
			return (TrimWhiteSpace(s) == String.Empty);
		}

		/// <summary>
		/// Trim all whitespace from a string.  If supplied string is null
		/// empty string is returned.
		/// </summary>
		public static string TrimWhiteSpace(string s)
		{
			if (s == null)
				return String.Empty;

			char[] trimchars = new[] {' ', '\r', '\n', '\t'};
			return s.Trim(trimchars);
		}

		/// <summary>
		/// Return at most [maxLen] characters from a string, ensuring that the
		/// string does not 
		/// end with an incomplete word. A suffix, such as an ellipsis, 
		/// will also be returned if supplied
		/// </summary>
		public static string BreakAt(string s, int maxLen, string suffix)
		{
			if (IsBlank(s) || maxLen <= 1)
				return string.Empty;

			string local = s.Trim();

			if (string.IsNullOrEmpty(suffix))
				suffix = string.Empty;

			if (local.Length <= maxLen)
				return local;

			if (local.Substring(maxLen, 1) == " ")
				return local.Substring(0, maxLen) + suffix;

			local = local.Substring(0, maxLen);

			return local.Substring(0, local.LastIndexOf(" ")) + suffix;
		}

		/// <summary>
		/// Return a copy of the input string with all non-numeric characters 
		/// stripped out 
		/// </summary>
		public static string StripNonDigits(IEnumerable<char> input)
		{
			return input.Where(Char.IsNumber).Aggregate(String.Empty, (m_Current, c) => m_Current + c.ToString());
		}

		/// <summary>Compare two strings, ignoring Case</summary>
		/// <returns>True if the strings are the same, False otherwise</returns>
		public static bool IgnoreCaseCompare(string str1, string str2)
		{
			string localStr1 = str1;
			string localStr2 = str2;

			if (localStr1 != null)
				localStr1 = localStr1.ToLower();

			if (localStr2 != null)
				localStr2 = localStr2.ToLower();

			return (localStr1 == localStr2);
		}

		/// <summary>
		/// Splits a string at capital letters e.g. "HelloMagoo" becomes "Hello Magoo"
		/// </summary>
		public static string SplitAtCapitalLetters(string str)
		{
			Regex r = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
			return r.Replace(str, " ");
		}
	}
}