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
using System.Data;
using System.Linq;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a User.
	/// </summary>
	public partial class User
	{
		#region Static methods

		/// <summary>
		/// Gets a user by their email address.
		/// </summary>
		/// <param name="email">The email address.</param>
		/// <returns></returns>
		public static User GetByEmail(string email)
		{
			if (StringUtils.IsBlank(email))
				return Empty;

			UserFinder finder = new UserFinder {Email = email};
			return FindOne(finder);
		}

        /// <summary>
        /// Gets a user by their user api token.
        /// </summary>
        public static User GetByUserAPIToken(string userAPIToken)
        {
            if (StringUtils.IsBlank(userAPIToken))
                return Empty;

            UserFinder finder = new UserFinder { UserAPIToken = userAPIToken };
            return FindOne(finder);
        }

        /// <summary>
        /// Gets a user by their session api token.
        /// </summary>
        public static User GetBySessionAPIToken(string sessionAPIToken)
        {
            if (StringUtils.IsBlank(sessionAPIToken))
                return Empty;

            UserFinder finder = new UserFinder { SessionAPIToken = sessionAPIToken };
            return FindOne(finder);
        }

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the user role.
		/// </summary>
		/// <value>The user role.</value>
		public UserRole UserRole
		{
			get
			{
				return EnumUtils.GetEnumFromValue<UserRole>(UserRoleId);
			}
			set
			{
				UserRoleId = Convert.ToInt32(value);
			}
		}

		/// <summary>
		/// Gets or sets the user status.
		/// </summary>
		/// <value>The user status.</value>
		public UserStatus UserStatus
		{
			get
			{
				return EnumUtils.GetEnumFromValue<UserStatus>(UserStatusId);
			}
			set
			{
				UserStatusId = Convert.ToInt32(value);
			}
		}

		/// <summary>
		/// Gets a value indicating whether [password changed].
		/// </summary>
		/// <value><c>true</c> if [password changed]; otherwise, <c>false</c>.</value>
		public bool PasswordChanged
		{
			get
			{
				return ChangedProperties.ContainsKey(Columns.Password.ToString());
			}
		}

		/// <summary>
		/// Gets this user's lightboxes (not lazy loaded)
		/// </summary>
		public List<Lightbox> Lightboxes
		{
			get
			{
				LightboxFinder finder = new LightboxFinder { UserId = UserId.GetValueOrDefault(-1) };
				return Lightbox.FindMany(finder);
			}
		}

		#endregion

		#region Password Stuff

		private string m_UnencryptedPassword = string.Empty;
		private string m_UnencryptedConfirmPassword = string.Empty;

		public string UnencryptedPassword
		{
			get
			{
				return m_UnencryptedPassword;
			}
		}

		public string UnencryptedConfirmPassword
		{
			get
			{
				return m_UnencryptedConfirmPassword;
			}
		}

		/// <summary>
		/// Sets the EncryptedPassword property to a hashed version of the specified value
		/// </summary>
		public virtual void SetPassword(string password)
		{
			m_UnencryptedPassword = password.Trim();
			Password = StringHasher.HashString(m_UnencryptedPassword + PasswordSalt);
		}

		/// <summary>
		/// Sets the confirmation password
		/// </summary>
		public virtual void SetConfirmPassword(string password)
		{
			m_UnencryptedConfirmPassword = password.Trim();
		}

		/// <summary>
		/// Checks if the specified password matches the user's password
		/// </summary>
		public virtual bool CheckPassword(string password)
		{
			return StringHasher.VerifyHash(Password, password + PasswordSalt);
		}

		#endregion

		#region Lazy Loads

		private List<Brand> m_BrandList = null;

		/// <summary>
		/// Gets the country.
		/// </summary>
		/// <value>The country.</value>
		public Country Country
		{
			get
			{
				return CountryCache.Instance.GetById(m_countryId.GetValueOrDefault());
			}
		}

		/// <summary>
		/// Gets the primary brand.
		/// </summary>
		/// <value>The primary brand.</value>
		public Brand PrimaryBrand
		{
			get
			{
				return BrandCache.Instance.GetById(PrimaryBrandId);
			}
		}

		/// <summary>
		/// Gets the list of brands to which this user has access
		/// </summary>
		public List<Brand> Brands
		{
			get
			{
				if (m_BrandList == null)
				{
					m_BrandList = new List<Brand>();

					if (!IsNew)
					{
						UserBrandFinder finder = new UserBrandFinder {UserId = UserId.GetValueOrDefault()};
						List<UserBrand> list = UserBrand.FindMany(finder);

						foreach (Brand brand in BrandCache.Instance.GetList())
							if (UserRole == UserRole.SuperAdministrator || brand.BrandId == PrimaryBrandId || list.Any(ub => ub.BrandId.Equals(brand.BrandId)))
								m_BrandList.Add(brand);
					}
				}

				return m_BrandList;
			}
		}

		private bool BrandsLoaded
		{
			get
			{
				return (m_BrandList != null);
			}
		}

		#endregion

		public bool CanAccessBrand(int brandId)
		{
			// User can always access their primary brand
			if (brandId == PrimaryBrandId)
				return true;

			return Brands.Any(b => b.BrandId.Equals(brandId));
		}

		/// <summary>
		/// Gets the User ID of the user responsible for approving order items
		/// for this asset, where this user is the upload user
		/// </summary>
		/// <returns></returns>
		public int GetOrderItemApproverUserId()
		{
			if (ApproverUserId.HasValue)
				return ApproverUserId.Value;

			return UserId.GetValueOrDefault();
		}

		/// <summary>
		/// Gets the account expiry date
		/// </summary>
		/// <returns>DateTime.MaxValue if account is non-expiring, DateTime.Now if no date is specified.  Otherwise ExpiryDate</returns>
		public DateTime GetAccountExpiryDate()
		{
			if (IsAccountNonExpiring)
				return DateTime.MaxValue;

			if (!AccountExpiryDate.HasValue)
				return DateTime.Now;

			return AccountExpiryDate.Value;
		}

		/// <summary>
		/// Gets the password expiry date.
		/// </summary>
		/// <returns></returns>
		public DateTime GetPasswordExpiryDate()
		{
			if (IsPasswordNonExpiring)
				return DateTime.MaxValue;

			if (!PasswordExpiryDate.HasValue)
				return DateTime.Now;

			return PasswordExpiryDate.Value;
		}

		/// <summary>
		/// Determines whether this user is active.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </returns>
		public bool IsActive()
		{
			return (!IsNull && !IsSuspended && !IsAccountExpired.GetValueOrDefault());
		}

		/// <summary>
		/// Gets a list of asset ID's that need to be catalogued by this user.
		/// </summary>
		public IEnumerable<int> GetCatalogueAssetIdList()
		{
            return (from x in CatalogueAssetIdList.Split(',')
                    let id = NumericUtils.ParseInt32(x, 0)
                    where (id > 0)
                    select id).ToList();
		}

		/// <summary>
		/// Gets the bad login info.
		/// </summary>
		/// <param name="minutes">The minutes.</param>
		/// <returns></returns>
		public BadLoginInfo GetBadLoginInfo(int minutes)
		{
			DataRow row = UserMapper.Instance.GetBadLoginInfo(UserId.GetValueOrDefault(), minutes);

			int badLoginCount = Convert.ToInt32(row["BadLoginCount"]);
			DateTime? lastBadLoginDate = null;

			if (row["LastBadLoginDate"] != DBNull.Value)
				lastBadLoginDate = Convert.ToDateTime(row["LastBadLoginDate"]);

			return new BadLoginInfo(badLoginCount, lastBadLoginDate);
		}


        /// <summary>
        /// regenerates the UserAPIToken for the user
        /// </summary>
        public void RegenerateUserAPIToken()
        {
            UserAPIToken = UserMapper.Instance.GenerateUserAPIToken(Email);
            Update(this);         
        }

	}

	public struct BadLoginInfo
	{
		public int BadLoginCount;
		public DateTime? LastBadLoginDate;

		public BadLoginInfo(int badLoginCount, DateTime? lastBadLoginDate)
		{
			BadLoginCount = badLoginCount;
			LastBadLoginDate = lastBadLoginDate;
		}
	}
}