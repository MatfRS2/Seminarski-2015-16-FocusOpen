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
	/// This object represents the properties and methods of a User.
	/// </summary>
	public partial class User
	{
		#region Constructor
		
		protected User()
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
		
		public static User New ()
		{
			return new User() ;
		}

		public static User Empty
		{
			get { return NullUser.Instance; }
		}

		public static User Get (Nullable <Int32> UserId)
		{
			User User = UserMapper.Instance.Get (UserId);
			return User ?? Empty;
		}

		public static User Update (User user)
		{
			bool isNew = user.IsNew;
			
			UserMapper.Instance.Update(user);

			if (user.BrandsLoaded)
			{
				if (!isNew)
					UserBrandMapper.Instance.DeleteUserBrands(user.UserId);

				if (user.UserRole != Shared.UserRole.SuperAdministrator)
				{
					foreach (Brand brand in user.Brands)
					{
						UserBrand ub = UserBrand.New();
						ub.UserId = user.UserId.GetValueOrDefault();
						ub.BrandId = brand.BrandId.GetValueOrDefault();
						UserBrand.Update(ub);
					}
				}
			}

			return user;
		}

		public static void Delete (Nullable <Int32> UserId)
		{
			UserMapper.Instance.Delete (UserId);
		}

		public static EntityList <User> FindMany (UserFinder finder, int Page, int PageSize)
		{
			return UserMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <User> FindMany (UserFinder finder)
		{
			return UserMapper.Instance.FindMany (finder);
		}

		public static User FindOne (UserFinder finder)
		{
			User User = UserMapper.Instance.FindOne(finder);
			return User ?? Empty;
		}

		public static int GetCount (UserFinder finder)
		{
			return UserMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
