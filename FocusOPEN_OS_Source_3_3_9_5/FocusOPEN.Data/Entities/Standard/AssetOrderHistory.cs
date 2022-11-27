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
    /// This object represents the properties and methods of a AssetOrderHistory.
	/// </summary>
	public partial class AssetOrderHistory
	{
		#region Constructor
		
		protected AssetOrderHistory()
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
		
		public static AssetOrderHistory New ()
		{
            return new AssetOrderHistory();
		}

        public static AssetOrderHistory Empty
		{
            get { return NullAssetOrderHistory.Instance; }
		}

        public static AssetOrderHistory Get(Nullable<Int32> AssetOrderHistoryId)
		{
            AssetOrderHistory AssetOrderHistory = AssetOrderHistoryMapper.Instance.Get(AssetOrderHistoryId);
            return AssetOrderHistory ?? Empty;
		}

        public static AssetOrderHistory Update(AssetOrderHistory assetOrderHistory)
		{
            return AssetOrderHistoryMapper.Instance.Update(assetOrderHistory) ;
		}

		public static void Delete (Nullable <Int32> AssetOrderHistoryId)
		{
            AssetOrderHistoryMapper.Instance.Delete(AssetOrderHistoryId);
		}

        public static EntityList<AssetOrderHistory> FindMany(AssetOrderHistoryFinder finder, int Page, int PageSize)
		{
            return AssetOrderHistoryMapper.Instance.FindMany(finder, Page, PageSize);
		}

        public static EntityList<AssetOrderHistory> FindMany(AssetOrderHistoryFinder finder)
		{
            return AssetOrderHistoryMapper.Instance.FindMany(finder);
		}

        public static AssetOrderHistory FindOne(AssetOrderHistoryFinder finder)
		{
            AssetOrderHistory AssetOrderHistory = AssetOrderHistoryMapper.Instance.FindOne(finder);
            return AssetOrderHistory ?? Empty;
		}

        public static int GetCount(AssetOrderHistoryFinder finder)
		{
            return AssetOrderHistoryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
