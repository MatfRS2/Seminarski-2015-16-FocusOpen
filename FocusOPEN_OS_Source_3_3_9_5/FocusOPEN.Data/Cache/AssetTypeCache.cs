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
	public class AssetTypeCache : BaseEntityCache<AssetType>
	{
		#region Singleton

		private AssetTypeCache()
		{
		}

		private static readonly AssetTypeCache m_Instance = new AssetTypeCache();

		public static AssetTypeCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<AssetType> GetData()
		{
			AssetTypeFinder finder = new AssetTypeFinder();
			finder.SortExpressions.Add(new AscendingSort(AssetType.Columns.Name));
			return AssetType.FindMany(finder);
		}

		protected override bool Compare(AssetType entity, int id)
		{
			return entity.AssetTypeId.GetValueOrDefault() == id;
		}

		protected override AssetType Empty
		{
			get
			{
				return AssetType.Empty;
			}
		}

		#endregion

		public IList<AssetType> GetVisibleAssetTypeList()
		{
			return (from at in GetList()
			        where at.IsVisible
					orderby at.Name
			        select at).ToList();
		}
	}
}
