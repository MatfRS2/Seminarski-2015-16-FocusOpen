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
	/// This object represents the properties and methods of a LightboxAsset.
	/// </summary>
	public partial class LightboxAsset
	{
		#region Constructor
		
		protected LightboxAsset()
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
		
		public static LightboxAsset New ()
		{
			return new LightboxAsset() ;
		}

		public static LightboxAsset Empty
		{
			get { return NullLightboxAsset.Instance; }
		}

		public static LightboxAsset Get (Nullable <Int32> LightboxAssetId)
		{
			LightboxAsset LightboxAsset = LightboxAssetMapper.Instance.Get (LightboxAssetId);
			return LightboxAsset ?? Empty;
		}

		public static LightboxAsset Update (LightboxAsset lightboxAsset)
		{
			return LightboxAssetMapper.Instance.Update(lightboxAsset) ;
		}

		public static void Delete (Nullable <Int32> LightboxAssetId)
		{
			LightboxAssetMapper.Instance.Delete (LightboxAssetId);
		}

		public static EntityList <LightboxAsset> FindMany (LightboxAssetFinder finder, int Page, int PageSize)
		{
			return LightboxAssetMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <LightboxAsset> FindMany (LightboxAssetFinder finder)
		{
			return LightboxAssetMapper.Instance.FindMany (finder);
		}

		public static LightboxAsset FindOne (LightboxAssetFinder finder)
		{
			LightboxAsset LightboxAsset = LightboxAssetMapper.Instance.FindOne(finder);
			return LightboxAsset ?? Empty;
		}

		public static int GetCount (LightboxAssetFinder finder)
		{
			return LightboxAssetMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
