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
	public class AssetFilePathCache : BaseEntityCache<AssetFilePath>
	{
		#region Singleton

		private AssetFilePathCache()
		{
		}

		private static readonly AssetFilePathCache m_Instance = new AssetFilePathCache();

		public static AssetFilePathCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<AssetFilePath> GetData()
		{
			AssetFilePathFinder finder = new AssetFilePathFinder();
			finder.SortExpressions.Add(new AscendingSort(AssetFilePath.Columns.Path));
			return AssetFilePath.FindMany(finder);
		}

		protected override bool Compare(AssetFilePath entity, int id)
		{
			return entity.AssetFilePathId.GetValueOrDefault() == id;
		}

		protected override AssetFilePath Empty
		{
			get
			{
				return AssetFilePath.Empty;
			}
		}

		#endregion
	}
}
