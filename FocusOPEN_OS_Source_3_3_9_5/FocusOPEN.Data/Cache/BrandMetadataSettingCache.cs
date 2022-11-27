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
using System.Linq;
using Daydream.Data;

namespace FocusOPEN.Data
{
	public class BrandMetadataSettingCache : BaseEntityCache<BrandMetadataSetting>
	{
		#region Singleton

		private BrandMetadataSettingCache()
		{
		}

		private static readonly BrandMetadataSettingCache m_Instance = new BrandMetadataSettingCache();

		public static BrandMetadataSettingCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<BrandMetadataSetting> GetData()
		{
			BrandMetadataSettingFinder finder = new BrandMetadataSettingFinder();
			finder.SortExpressions.Add(new AscendingSort(BrandMetadataSetting.Columns.FieldName));
			return BrandMetadataSetting.FindMany(finder);
		}

		protected override bool Compare(BrandMetadataSetting entity, int id)
		{
			return entity.BrandMetadataSettingId.GetValueOrDefault() == id;
		}

		protected override BrandMetadataSetting Empty
		{
			get
			{
				return BrandMetadataSetting.Empty;
			}
		}
		
		#endregion
		
		public IEnumerable<BrandMetadataSetting> GetByBrand(int brandId)
		{
			return (from b in GetList()
			        where b.BrandId == brandId
			        orderby b.FieldName
			        select b).ToList();
		}
	}
}