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
	/// This object represents the properties and methods of a AssetType.
	/// </summary>
	public partial class AssetType
	{
		#region Constructor
		
		protected AssetType()
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
		
		public static AssetType New ()
		{
			return new AssetType() ;
		}

		public static AssetType Empty
		{
			get { return NullAssetType.Instance; }
		}

		public static AssetType Get (Nullable <Int32> AssetTypeId)
		{
			AssetType AssetType = AssetTypeMapper.Instance.Get (AssetTypeId);
			return AssetType ?? Empty;
		}

		public static AssetType Update (AssetType assetType)
		{
			return AssetTypeMapper.Instance.Update(assetType) ;
		}

		public static void Delete (Nullable <Int32> AssetTypeId)
		{
			AssetTypeMapper.Instance.Delete (AssetTypeId);
		}

		public static EntityList <AssetType> FindMany (AssetTypeFinder finder, int Page, int PageSize)
		{
			return AssetTypeMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetType> FindMany (AssetTypeFinder finder)
		{
			return AssetTypeMapper.Instance.FindMany (finder);
		}

		public static AssetType FindOne (AssetTypeFinder finder)
		{
			AssetType AssetType = AssetTypeMapper.Instance.FindOne(finder);
			return AssetType ?? Empty;
		}

		public static int GetCount (AssetTypeFinder finder)
		{
			return AssetTypeMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
