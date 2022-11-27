/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Lightbox.
	/// </summary>
	public partial class Lightbox
	{
		#region Private variables

		private User m_User = null;
		private List<Brand> m_Brands = null;

		#endregion

		#region Lazy Loads

		public User User
		{
			get
			{
				if (m_User == null)
					m_User = User.Get(UserId);

				return (m_User);
			}
		}

		public List<Brand> Brands
		{
			get
			{
				if (m_Brands == null)
				{
					LightboxBrandFinder finder = new LightboxBrandFinder {LightboxId = LightboxId.GetValueOrDefault()};
					List<LightboxBrand> list = LightboxBrand.FindMany(finder);

					m_Brands = new List<Brand>();
					list.ForEach(lb => m_Brands.Add(BrandCache.Instance.GetById(lb.BrandId)));
				}

				return m_Brands;
			}
		}

		#endregion

        #region Properties

        public bool IsEditable { get; set; }

        public bool IsLinked { get; set; }

        #endregion


        public EntityList<LightboxAsset> GetLightboxAssetList()
		{
			LightboxAssetFinder finder = new LightboxAssetFinder {LightboxId = LightboxId.GetValueOrDefault(-1)};
			return LightboxAsset.FindMany(finder);
		}

		public EntityList<Asset> GetAssetList()
		{
			AssetFinder assetFinder = new AssetFinder();
			assetFinder.AssetIdList.Add(0);

			foreach (LightboxAsset lightboxAsset in GetLightboxAssetList())
			{
				assetFinder.AssetIdList.Add(lightboxAsset.AssetId);
			}

			return Asset.FindMany(assetFinder);
		}
	}
}