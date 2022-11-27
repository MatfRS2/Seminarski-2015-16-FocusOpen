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
	/// This object represents the properties and methods of a UserBrand.
	/// </summary>
	public partial class UserBrand
	{
		#region Constructor
		
		protected UserBrand()
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
		
		public static UserBrand New ()
		{
			return new UserBrand() ;
		}

		public static UserBrand Empty
		{
			get { return NullUserBrand.Instance; }
		}

		public static UserBrand Get (Nullable <Int32> UserBrandId)
		{
			UserBrand UserBrand = UserBrandMapper.Instance.Get (UserBrandId);
			return UserBrand ?? Empty;
		}

		public static UserBrand Update (UserBrand userBrand)
		{
			return UserBrandMapper.Instance.Update(userBrand) ;
		}

		public static void Delete (Nullable <Int32> UserBrandId)
		{
			UserBrandMapper.Instance.Delete (UserBrandId);
		}

		public static EntityList <UserBrand> FindMany (UserBrandFinder finder, int Page, int PageSize)
		{
			return UserBrandMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <UserBrand> FindMany (UserBrandFinder finder)
		{
			return UserBrandMapper.Instance.FindMany (finder);
		}

		public static UserBrand FindOne (UserBrandFinder finder)
		{
			UserBrand UserBrand = UserBrandMapper.Instance.FindOne(finder);
			return UserBrand ?? Empty;
		}

		public static int GetCount (UserBrandFinder finder)
		{
			return UserBrandMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
