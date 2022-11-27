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
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a AssetCategory.
	/// </summary>
	public partial class AssetCategory
	{
		#region Constructor
		
		protected AssetCategory()
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
		
		public static AssetCategory New ()
		{
			return new AssetCategory() ;
		}

		public static AssetCategory Empty
		{
			get { return NullAssetCategory.Instance; }
		}

		public static AssetCategory Get (Nullable <Int32> AssetCategoryId)
		{
			AssetCategory AssetCategory = AssetCategoryMapper.Instance.Get (AssetCategoryId);
			return AssetCategory ?? Empty;
		}

		public static AssetCategory Update (AssetCategory assetCategory)
		{
			return AssetCategoryMapper.Instance.Update(assetCategory) ;
		}

		public static void Delete (Nullable <Int32> AssetCategoryId)
		{
			AssetCategoryMapper.Instance.Delete (AssetCategoryId);
		}

		public static EntityList <AssetCategory> FindMany (AssetCategoryFinder finder, int Page, int PageSize)
		{
			return AssetCategoryMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetCategory> FindMany (AssetCategoryFinder finder)
		{
			return AssetCategoryMapper.Instance.FindMany (finder);
		}

		public static AssetCategory FindOne (AssetCategoryFinder finder)
		{
			AssetCategory AssetCategory = AssetCategoryMapper.Instance.FindOne(finder);
			return AssetCategory ?? Empty;
		}

		public static int GetCount (AssetCategoryFinder finder)
		{
			return AssetCategoryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}