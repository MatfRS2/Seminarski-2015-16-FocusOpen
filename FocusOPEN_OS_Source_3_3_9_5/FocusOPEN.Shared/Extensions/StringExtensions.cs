using System;
using System.Diagnostics;

namespace FocusOPEN.Shared
{
	public static class StringExtensions
	{
		public static T To<T>(this IConvertible obj, T defVal)
		{
			Type t = typeof(T);
			Type u = Nullable.GetUnderlyingType(t);

			if (u != null)
			{
				if (obj == null)
					return default(T);

				try
				{
					return (T) Convert.ChangeType(obj, u);
				}
				catch
				{
					return defVal;
				}
			}

			try
			{
				return (T) Convert.ChangeType(obj, t);
			}
			catch
			{
				return defVal;
			}
		}

		public static string ToBase64(this string str)
		{
			byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
			return Convert.ToBase64String(encbuff);
		}

		public static string FromBase64(this string str)
		{
			try
			{
				byte[] decbuff = Convert.FromBase64String(str);
				return System.Text.Encoding.UTF8.GetString(decbuff);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(string.Format("Error converting string from base64: {0}. Error: {1}.", str, ex.Message));
				return string.Empty;
			}
		}

		/// <summary>
		/// Ensures the string ends with the end string.  If the input string is null or empty, the endstring will be returned.
		/// </summary>
		/// <param name="str">Input string</param>
		/// <param name="endstring">The string it must end with</param>
		/// <returns>String ending with endstring</returns>
		public static string EnsureEndsWith(this string str, string endstring)
		{
			if (string.IsNullOrEmpty(str))
				return endstring;

			if (!str.EndsWith(endstring))
				str += endstring;

			return str;
		}
	}
}