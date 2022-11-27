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
using System.Diagnostics;
using System.Linq;
using System.Web;
using FocusOPEN.Data;

namespace FocusOPEN.Website.Components
{
	public static class WebsiteBrandManager
	{
		#region Public Methods

		/// <summary>
		/// Gets the brand info.  First tries to use the current user's primary brand, then the URL
		/// If both of those fail, the master brand is returned as a last resort.
		/// </summary>
		public static Brand GetBrand()
		{
			Brand brand = null;

			// First try and get brand info by current user's primary brand, if this is all they have access to
			// However, some users have access to more than one brand, so we'll leave those alone and let it drop through
			// to the check below where the brand is retrieved by URL.
			if (!SessionInfo.Current.User.IsNull && SessionInfo.Current.User.Brands.Count == 1 && SessionInfo.Current.User.Brands[0].BrandId == SessionInfo.Current.User.PrimaryBrandId)
				brand = GetBrandById(SessionInfo.Current.User.PrimaryBrandId);

			// Then try and get it using the current URL
			if (brand == null)
				brand = GetBrandByCurrentUrl();

			// Finally, resort to master brand
			if (brand == null)
				brand = GetMasterBrand();

			// All done.
			return brand;
		}

		/// <summary>
		/// Gets the BrandInfo for the specified brand Id.  Returns null if no brand found
		/// </summary>
		public static Brand GetBrandById(int brandId)
		{
			Brand brand = BrandCache.Instance.GetById(brandId);

			if (brand.IsNull)
				brand = null;

			return brand;
		}

		/// <summary>
		/// Gets the brand info using the URL of the current web request
		/// If no brand info is found, the master brand is returned
		/// </summary>
		public static Brand GetBrandByCurrentUrl()
		{
			string url = HttpContext.Current.Request.Url.AbsoluteUri;
			return GetBrandByUrl(url);
		}

		/// <summary>
		/// Gets the master brand
		/// </summary>
		/// <returns></returns>
		public static Brand GetMasterBrand()
		{
			return BrandCache.Instance.GetMasterBrand();
		}

		#endregion

		#region Private Methods

		private static Brand GetBrandByUrl(string url)
		{
			Uri uri = new Uri(url);
			string host = uri.Host.ToLower();
			return GetBrandByHost(host);
		}

		/// <summary>
		/// Gets the brand info for the specified host. Returns null if not found.
		/// </summary>
		private static Brand GetBrandByHost(string host)
		{
			try
			{
				// If multiple brands are using the same URL, then prioritize the master brand.

				return (from b in BrandCache.Instance.GetList()
							 let BrandUri = new Uri(b.WebsiteUrl)
							 let BrandHost = BrandUri.Host.ToLower()
							 where (BrandHost == host)
				             orderby b.IsMasterBrand descending, b.BrandId ascending 
				             select b).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("Error getting brand info using host: {0}. Error: {1}", host, ex.Message));
			}

			return null;
		}

		#endregion
	}
}