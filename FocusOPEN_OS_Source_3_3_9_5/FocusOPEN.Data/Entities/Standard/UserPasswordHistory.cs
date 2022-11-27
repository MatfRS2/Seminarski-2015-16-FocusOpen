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
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a UserPasswordHistory.
	/// </summary>
	public partial class UserPasswordHistory
	{
		#region Constructor
		
		protected UserPasswordHistory()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static UserPasswordHistory New ()
		{
			return new UserPasswordHistory() ;
		}

		public static UserPasswordHistory Empty
		{
			get { return NullUserPasswordHistory.Instance; }
		}

		public static UserPasswordHistory Get (Nullable <Int32> UserPasswordHistoryId)
		{
			UserPasswordHistory UserPasswordHistory = UserPasswordHistoryMapper.Instance.Get (UserPasswordHistoryId);
			return UserPasswordHistory ?? Empty;
		}

		public static UserPasswordHistory Update (UserPasswordHistory userPasswordHistory)
		{
			return UserPasswordHistoryMapper.Instance.Update(userPasswordHistory) ;
		}

		public static void Delete (Nullable <Int32> UserPasswordHistoryId)
		{
			UserPasswordHistoryMapper.Instance.Delete (UserPasswordHistoryId);
		}

		public static EntityList <UserPasswordHistory> FindMany (UserPasswordHistoryFinder finder, int Page, int PageSize)
		{
			return UserPasswordHistoryMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <UserPasswordHistory> FindMany (UserPasswordHistoryFinder finder)
		{
			return UserPasswordHistoryMapper.Instance.FindMany (finder);
		}

		public static UserPasswordHistory FindOne (UserPasswordHistoryFinder finder)
		{
			UserPasswordHistory UserPasswordHistory = UserPasswordHistoryMapper.Instance.FindOne(finder);
			return UserPasswordHistory ?? Empty;
		}

		public static int GetCount (UserPasswordHistoryFinder finder)
		{
			return UserPasswordHistoryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
