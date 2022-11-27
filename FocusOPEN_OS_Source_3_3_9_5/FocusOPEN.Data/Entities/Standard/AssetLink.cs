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
	public partial class AssetLink
	{
		#region Constructor
		
		protected AssetLink()
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
		
		public static AssetLink New ()
		{
			return new AssetLink() ;
		}

		public static AssetLink Empty
		{
			get { return NullAssetLink.Instance; }
		}

		public static AssetLink Get (Nullable <Int32> AssetLinkId)
		{
			AssetLink AssetLink = AssetLinkMapper.Instance.Get (AssetLinkId);
			return AssetLink ?? Empty;
		}

		public static AssetLink Update (AssetLink assetLink)
		{
			return AssetLinkMapper.Instance.Update(assetLink) ;
		}

		public static void Delete (Nullable <Int32> AssetLinkId)
		{
			AssetLinkMapper.Instance.Delete (AssetLinkId);
		}

		public static EntityList <AssetLink> FindMany (AssetLinkFinder finder, int Page, int PageSize)
		{
			return AssetLinkMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetLink> FindMany (AssetLinkFinder finder)
		{
			return AssetLinkMapper.Instance.FindMany (finder);
		}

		public static AssetLink FindOne (AssetLinkFinder finder)
		{
			AssetLink AssetLink = AssetLinkMapper.Instance.FindOne(finder);
			return AssetLink ?? Empty;
		}

		public static int GetCount (AssetLinkFinder finder)
		{
			return AssetLinkMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
