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
	/// This object represents the properties and methods of a AssetImageSize.
	/// </summary>
	public partial class AssetImageSize
	{
		#region Constructor
		
		protected AssetImageSize()
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
		
		public static AssetImageSize New ()
		{
			return new AssetImageSize() ;
		}

		public static AssetImageSize Empty
		{
			get { return NullAssetImageSize.Instance; }
		}

		public static AssetImageSize Get (Nullable <Int32> AssetImageSizeId)
		{
			AssetImageSize AssetImageSize = AssetImageSizeMapper.Instance.Get (AssetImageSizeId);
			return AssetImageSize ?? Empty;
		}

		public static AssetImageSize Update (AssetImageSize assetImageSize)
		{
			return AssetImageSizeMapper.Instance.Update(assetImageSize) ;
		}

		public static void Delete (Nullable <Int32> AssetImageSizeId)
		{
			AssetImageSizeMapper.Instance.Delete (AssetImageSizeId);
		}

		public static EntityList <AssetImageSize> FindMany (AssetImageSizeFinder finder, int Page, int PageSize)
		{
			return AssetImageSizeMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetImageSize> FindMany (AssetImageSizeFinder finder)
		{
			return AssetImageSizeMapper.Instance.FindMany (finder);
		}

		public static AssetImageSize FindOne (AssetImageSizeFinder finder)
		{
			AssetImageSize AssetImageSize = AssetImageSizeMapper.Instance.FindOne(finder);
			return AssetImageSize ?? Empty;
		}

		public static int GetCount (AssetImageSizeFinder finder)
		{
			return AssetImageSizeMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
