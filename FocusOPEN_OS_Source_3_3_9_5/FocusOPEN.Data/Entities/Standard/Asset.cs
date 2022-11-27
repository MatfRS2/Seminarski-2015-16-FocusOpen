/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Asset.
	/// </summary>
	public partial class Asset
	{
		#region Constructor

		protected Asset()
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

		public static Asset New()
		{
			return new Asset();
		}

		public static Asset Empty
		{
			get
			{
				return NullAsset.Instance;
			}
		}

		public static Asset Get(int? AssetId)
		{
			Asset Asset = AssetMapper.Instance.Get(AssetId);
			return Asset ?? Empty;
		}

		public static void Update(Asset asset)
		{
			AssetMapper.Instance.Update(asset);
		}

		public static void Delete(int? AssetId)
		{
			AssetMapper.Instance.Delete(AssetId);
		}

		public static EntityList<Asset> FindMany(AssetFinder finder, int Page, int PageSize)
		{
			return AssetMapper.Instance.FindMany(finder, Page, PageSize);
		}

		public static EntityList<Asset> FindMany(AssetFinder finder)
		{
			return AssetMapper.Instance.FindMany(finder);
		}

		public static Asset FindOne(AssetFinder finder)
		{
			Asset Asset = AssetMapper.Instance.FindOne(finder);
			return Asset ?? Empty;
		}

		public static int GetCount(AssetFinder finder)
		{
			return AssetMapper.Instance.GetCount(finder);
		}

		#endregion
	}
}