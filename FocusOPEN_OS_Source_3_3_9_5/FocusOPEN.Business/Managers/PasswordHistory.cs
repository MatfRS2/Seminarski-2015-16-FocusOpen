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
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class PasswordHistory
	{
		/// <summary>
		/// Checks if the user with the specified ID has used the specified password
		/// recently (ie. within the last 24 password changes).
		/// Returns true if they have, otherwise false.
		/// </summary>
		public static bool IsRecentPassword(User user, string password)
		{
			// Get the last 24 passwords
			UserPasswordHistoryFinder finder = new UserPasswordHistoryFinder {UserId = user.UserId.GetValueOrDefault(-1), MaxRecords = 24};
			finder.SortExpressions.Add(new DescendingSort(UserPasswordHistory.Columns.Date));
			EntityList<UserPasswordHistory> passwordHistory = UserPasswordHistory.FindMany(finder);
			return passwordHistory.Any(uph => StringHasher.VerifyHash(uph.Password, password + user.PasswordSalt));
		}

		/// <summary>
		/// Adds the user's current password to their password history.
		/// </summary>
		public static void UpdateUserPasswordHistory(User user)
		{
			UserPasswordHistory uph = UserPasswordHistory.New();
			uph.UserId = user.UserId.GetValueOrDefault();
			uph.Password = user.Password;
			uph.Date = DateTime.Now;
			UserPasswordHistory.Update(uph);
		}
	}
}