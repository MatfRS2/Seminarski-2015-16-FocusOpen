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

namespace FocusOPEN.Shared
{
	public static class PasswordGenerator
	{
		#region Properties

		/// <summary>
		/// Specifies whether a single validation error should be displayed for all password validation errors
		/// </summary>
		public static bool UseSingleValidationError { get; set; }

		/// <summary>
		/// Specifies whether special characters should be used in generated passwords
		/// </summary>
		public static bool UseSpecialChars { get; set; }

		#endregion

		#region Constructor

		static PasswordGenerator()
		{
			UseSingleValidationError = true;
			UseSpecialChars = false;
		}

		#endregion

		#region Private Variables

		private const string m_SingleErrorMessage = "Password must be at least {0} characters, containing both uppercase and lowercase letters, and numbers. It must not be one you have used before.";
		private const int m_MinimumLength = 8;
		private const string m_LowercaseChars = "abcdefghijklmnopqrstuvqxyz";
		private const string m_UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private const string m_NumericChars = "1234567890";
		private const string m_SpecialChars = "~?/@#!�$%^&*+-_.=|";

		#endregion

		#region Public Methods

		/// <summary>
		/// Generates password of the minimum length
		/// </summary>
		public static string GeneratePassword()
		{
			return GeneratePassword(m_MinimumLength);
		}

		/// <summary>
		/// Generates a password of the specified length
		/// </summary>
		/// <param name="length">The number of characters to generate</param>
		public static string GeneratePassword(int length)
		{
			return GeneratePassword(length, Environment.TickCount);
		}

		public static ErrorList ValidatePassword(string password)
		{
			ErrorList e = new ErrorList();

			if (password == null)
				password = string.Empty;

			if (password.Length < m_MinimumLength)
			{
				if (UseSingleValidationError)
				{
					e.Add(GetSingleValidationErrorMessage());
					return e;
				}

				e.Add(string.Format("Password must be at least {0} characters", m_MinimumLength));
			}

			bool containsLowercaseChar = false;
			bool containsUppercaseChar = false;
			bool containsNumericChar = false;

			foreach (char c in password)
			{
				if (m_LowercaseChars.IndexOf(c) != -1)
					containsLowercaseChar = true;

				if (m_UppercaseChars.IndexOf(c) != -1)
					containsUppercaseChar = true;

				if (m_NumericChars.IndexOf(c) != -1)
					containsNumericChar = true;
			}

			if (!containsLowercaseChar)
			{
				if (UseSingleValidationError)
				{
					e.Add(GetSingleValidationErrorMessage());
					return e;
				}

				e.Add("Password must contain at least one lowercase letter");
			}

			if (!containsUppercaseChar)
			{
				if (UseSingleValidationError)
				{
					e.Add(GetSingleValidationErrorMessage());
					return e;
				}

				e.Add("Password must contain at least one uppercase letter");
			}

			if (!containsNumericChar)
			{
				if (UseSingleValidationError)
				{
					e.Add(GetSingleValidationErrorMessage());
					return e;
				}

				e.Add("Password must contain at least one number");
			}

			return (e);
		}

		#endregion

		#region Private Methods

		private static string GetSingleValidationErrorMessage()
		{
			return string.Format(m_SingleErrorMessage, m_MinimumLength);
		}

		/// <summary>
		/// Generates a password of the specified length using the specified seed
		/// </summary>
		private static string GeneratePassword(int length, int seed)
		{
			// Passwords must contain at least 3 of the following character groups: uppercase letters, lowercase letters
			// numerals, and special characters (!, #, $, �, etc)

			// The password must be at least 4 characters  so that we can add a single character from each group.
			if (length < 4) throw new ArgumentException("Password must be at least 4 characters");

			StringBuilder SB = new StringBuilder();

			Random rand = new Random(seed);

			// Ensure that we add all of the required groups first
			SB.Append(GetRandomCharacter(m_LowercaseChars, rand));
			SB.Append(GetRandomCharacter(m_UppercaseChars, rand));
			SB.Append(GetRandomCharacter(m_NumericChars, rand));

			if (UseSpecialChars)
				SB.Append(GetRandomCharacter(m_SpecialChars, rand));

			// Now add random characters up to the end of the string
			while (SB.Length < length)
			{
				SB.Append(GetRandomCharacter(GetRandomString(rand), rand));
			}

			string GeneratedPassword = SB.ToString();
			return GeneratedPassword;
		}

		#endregion

		#region Helper Methods

		private static string GetRandomString(Random rand)
		{
			int a = rand.Next(3);
			switch (a)
			{
				case 1:
					return m_UppercaseChars;
				case 2:
					return m_NumericChars;
				case 3:
					return (UseSpecialChars) ? m_SpecialChars : m_LowercaseChars;
				default:
					return m_LowercaseChars;
			}
		}

		private static char GetRandomCharacter(string s, Random rand)
		{
			int x = rand.Next(s.Length);

			string a = s.Substring(x, 1);
			char b = Convert.ToChar(a);

			return (b);
		}

		#endregion
	}
}