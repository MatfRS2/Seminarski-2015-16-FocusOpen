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
	/// This object represents the properties and methods of a AssetMetadata.
	/// </summary>
	public partial class AssetMetadata
	{
		#region Constructor
		
		protected AssetMetadata()
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
		
		public static AssetMetadata New ()
		{
			return new AssetMetadata() ;
		}

		public static AssetMetadata Empty
		{
			get { return NullAssetMetadata.Instance; }
		}

		public static AssetMetadata Get (Nullable <Int32> AssetMetadataId)
		{
			AssetMetadata AssetMetadata = AssetMetadataMapper.Instance.Get (AssetMetadataId);
			return AssetMetadata ?? Empty;
		}

		public static AssetMetadata Update (AssetMetadata assetMetadata)
		{
			return AssetMetadataMapper.Instance.Update(assetMetadata) ;
		}

		public static void Delete (Nullable <Int32> AssetMetadataId)
		{
			AssetMetadataMapper.Instance.Delete (AssetMetadataId);
		}

		public static EntityList <AssetMetadata> FindMany (AssetMetadataFinder finder, int Page, int PageSize)
		{
			return AssetMetadataMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetMetadata> FindMany (AssetMetadataFinder finder)
		{
			return AssetMetadataMapper.Instance.FindMany (finder);
		}

		public static AssetMetadata FindOne (AssetMetadataFinder finder)
		{
			AssetMetadata AssetMetadata = AssetMetadataMapper.Instance.FindOne(finder);
			return AssetMetadata ?? Empty;
		}

		public static int GetCount (AssetMetadataFinder finder)
		{
			return AssetMetadataMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
