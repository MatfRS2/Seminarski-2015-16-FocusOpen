/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web.Security;

namespace FocusOPEN.Shared
{
	/// <summary>
	/// Utility class for hashing strings and comparing hashed strings
	/// (eg. for cases where encrypted data needs to be stored)
	/// </summary>
	public static class StringHasher
	{
		#region Methods

		/// <summary>
		/// Returns a hashed version of the string using the MD5 algorithm
		/// </summary>
		public static string HashString(string originalString)
		{
			string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(originalString, "md5") ?? string.Empty;
			return hash.ToLower();
		}

		/// <summary>
		/// Checks if the specified hashed string matches the original hashed string
		/// </summary>
		/// <param name="hash">Hashed string</param>
		/// <param name="original">Unhashed original string to compare against</param>
		public static bool VerifyHash(string hash, string original)
		{
			string hashedOriginal = HashString(original);
			return (hash == hashedOriginal);
		}

		#endregion
	}
}