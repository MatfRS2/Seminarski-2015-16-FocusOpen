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
	public class BrandCache : BaseEntityCache<Brand>
	{
		#region Singleton

		private BrandCache()
		{
		}

		private static readonly BrandCache m_Instance = new BrandCache();

		public static BrandCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<Brand> GetData()
		{
			BrandFinder finder = new BrandFinder();
			finder.SortExpressions.Add(new AscendingSort(Brand.Columns.Name));
			return Brand.FindMany(finder);
		}

		protected override bool Compare(Brand entity, int id)
		{
			return entity.BrandId.GetValueOrDefault() == id;
		}

		protected override Brand Empty
		{
			get
			{
				return Brand.Empty;
			}
		}

		#endregion
		
		public Brand GetMasterBrand()
		{
			return GetList().Find(b => b.IsMasterBrand);
		}

		public int GetMasterBrandId()
		{
			Brand brand = GetMasterBrand();

			if (brand != null)
				return brand.BrandId.GetValueOrDefault();

			return -1;
		}

		public override void InvalidateCache()
		{
			// Invalidate the brand cache
			base.InvalidateCache();

			// Also invalidate the brand metadata settings cache
			BrandMetadataSettingCache.Instance.InvalidateCache();
		}
	}
}