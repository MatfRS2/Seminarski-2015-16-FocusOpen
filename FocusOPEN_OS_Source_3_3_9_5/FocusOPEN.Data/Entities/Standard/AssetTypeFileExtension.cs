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
	/// This object represents the properties and methods of a AssetTypeFileExtension.
	/// </summary>
	public partial class AssetTypeFileExtension
	{
		#region Constructor
		
		protected AssetTypeFileExtension()
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
		
		public static AssetTypeFileExtension New ()
		{
			return new AssetTypeFileExtension() ;
		}

		public static AssetTypeFileExtension Empty
		{
			get { return NullAssetTypeFileExtension.Instance; }
		}

		public static AssetTypeFileExtension Get (Nullable <Int32> AssetTypeFileExtensionId)
		{
			AssetTypeFileExtension AssetTypeFileExtension = AssetTypeFileExtensionMapper.Instance.Get (AssetTypeFileExtensionId);
			return AssetTypeFileExtension ?? Empty;
		}

		public static AssetTypeFileExtension Update (AssetTypeFileExtension assetTypeFileExtension)
		{
			return AssetTypeFileExtensionMapper.Instance.Update(assetTypeFileExtension) ;
		}

		public static void Delete (Nullable <Int32> AssetTypeFileExtensionId)
		{
			AssetTypeFileExtensionMapper.Instance.Delete (AssetTypeFileExtensionId);
		}

		public static EntityList <AssetTypeFileExtension> FindMany (AssetTypeFileExtensionFinder finder, int Page, int PageSize)
		{
			return AssetTypeFileExtensionMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetTypeFileExtension> FindMany (AssetTypeFileExtensionFinder finder)
		{
			return AssetTypeFileExtensionMapper.Instance.FindMany (finder);
		}

		public static AssetTypeFileExtension FindOne (AssetTypeFileExtensionFinder finder)
		{
			AssetTypeFileExtension AssetTypeFileExtension = AssetTypeFileExtensionMapper.Instance.FindOne(finder);
			return AssetTypeFileExtension ?? Empty;
		}

		public static int GetCount (AssetTypeFileExtensionFinder finder)
		{
			return AssetTypeFileExtensionMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
