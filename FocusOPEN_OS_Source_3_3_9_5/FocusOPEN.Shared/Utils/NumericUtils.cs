using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FocusOPEN.Shared
{
	public static class NumericUtils
	{
		/// <summary>
		/// Check whether the supplied string is an integer
		/// </summary>
		public static bool IsInt32(string s)
		{
			try
			{
				Int32.Parse(s);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Return a parsed integer from the supplied expression, after 
		/// stripping all non-numeric characters from the expression. If 
		/// the remaining expression does not parse as an int, return the
		/// supplied default value 
		/// </summary>
		public static Int32 ParseInt32(string expression, int defaultValue)
		{
			string input = expression.ToLower();

			// Change 15K to 15000
			if (input.EndsWith("k"))
			{
				input = input.Substring(0, input.Length - 1) + "000";
			}

			try
			{
				// We're not interested in decimal amounts, so...
				input = LoseDecimalPortion(input);

				// But check it isn't a -ve number.
				if (input.StartsWith("-"))
					return Int32.Parse("-" + StringUtils.StripNonDigits(input.Substring(1)));

				return Int32.Parse(StringUtils.StripNonDigits(input));
			}
			catch
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// parses '1,2,4' into a list and '1' into a list again but with a single value
		/// </summary>
		public static List<int> ParseIntList(string input, string delimiter)
		{
			var result = new List<int>();

			if (string.IsNullOrEmpty(input)) return result;

			try
			{
				var parsedVals = input
					.Split(delimiter.ToCharArray())
					.Select(s => int.Parse(s))
					.ToList();

				result.AddRange(parsedVals);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error parsing value into int list: " + ex.Message);
			}

			return result;
		}

		/// <summary>
		/// Remove the decimal portion of the number
		/// </summary>
		private static string LoseDecimalPortion(string input)
		{
			if (input.IndexOf(".") > -1)
				input = input.Substring(0, input.IndexOf("."));

			return input;
		}

		/// <summary>
		/// Gets the underlying value from a nullable int
		/// </summary>
		public static int GetIntValue(int? val, int nullValue)
		{
			if (!val.HasValue)
				return nullValue;

			return val.Value;
		}
	}
}