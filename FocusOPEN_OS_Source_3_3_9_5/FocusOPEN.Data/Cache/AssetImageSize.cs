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
	public class AssetImageSizeCache : BaseEntityCache<AssetImageSize>
	{
		#region Singleton

		private AssetImageSizeCache()
		{
		}

		private static readonly AssetImageSizeCache m_Instance = new AssetImageSizeCache();

		public static AssetImageSizeCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<AssetImageSize> GetData()
		{
			AssetImageSizeFinder finder = new AssetImageSizeFinder();
			EntityList<AssetImageSize> list = AssetImageSize.FindMany(finder);

			AssetImageSize ais = AssetImageSize.New();
			ais.AssetImageSizeId = 0;
			ais.Description = "Original";
			list.Insert(0, ais);

			return list;
		}

		protected override bool Compare(AssetImageSize entity, int id)
		{
			return entity.AssetImageSizeId.GetValueOrDefault() == id;
		}

		protected override AssetImageSize Empty
		{
			get
			{
				return AssetImageSize.Empty;
			}
		}

		#endregion
	}
}
