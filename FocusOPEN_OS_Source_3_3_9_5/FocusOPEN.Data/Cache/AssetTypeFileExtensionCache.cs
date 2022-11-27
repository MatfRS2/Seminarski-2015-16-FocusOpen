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
using System.Linq;
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public class AssetTypeFileExtensionCache : BaseEntityCache<AssetTypeFileExtension>
	{
		#region Singleton

		private AssetTypeFileExtensionCache()
		{
		}

		private static readonly AssetTypeFileExtensionCache m_Instance = new AssetTypeFileExtensionCache();

		public static AssetTypeFileExtensionCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<AssetTypeFileExtension> GetData()
		{
			AssetTypeFileExtensionFinder finder = new AssetTypeFileExtensionFinder();
			finder.SortExpressions.Add(new AscendingSort(AssetTypeFileExtension.Columns.Name));
			return AssetTypeFileExtension.FindMany(finder);
		}

		protected override bool Compare(AssetTypeFileExtension entity, int id)
		{
			return entity.AssetTypeFileExtensionId.GetValueOrDefault() == id;
		}

		protected override AssetTypeFileExtension Empty
		{
			get
			{
				return AssetTypeFileExtension.Empty;
			}
		}
		
		#endregion
		
		public AssetTypeFileExtension GetByExtension(string ext)
		{
			return (from x in Instance.GetList()
			        where StringUtils.IgnoreCaseCompare(x.Extension, ext)
			        select x).FirstOrDefault() ?? AssetTypeFileExtension.Empty;
		}

		public List<AssetTypeFileExtension> GetByAssetType(int assetTypeId)
		{
			return (from x in Instance.GetList()
					where x.AssetTypeId == assetTypeId
					select x).ToList();
		}
	}
}