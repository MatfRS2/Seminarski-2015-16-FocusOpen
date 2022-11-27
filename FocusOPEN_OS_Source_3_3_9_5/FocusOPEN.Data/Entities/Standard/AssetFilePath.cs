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
	/// This object represents the properties and methods of a AssetFilePath.
	/// </summary>
	public partial class AssetFilePath
	{
		#region Constructor
		
		protected AssetFilePath()
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
		
		public static AssetFilePath New ()
		{
			return new AssetFilePath() ;
		}

		public static AssetFilePath Empty
		{
			get { return NullAssetFilePath.Instance; }
		}

		public static AssetFilePath Get (Nullable <Int32> AssetFilePathId)
		{
			AssetFilePath AssetFilePath = AssetFilePathMapper.Instance.Get (AssetFilePathId);
			return AssetFilePath ?? Empty;
		}

		public static AssetFilePath Update (AssetFilePath assetFilePath)
		{
			return AssetFilePathMapper.Instance.Update(assetFilePath) ;
		}

		public static void Delete (Nullable <Int32> AssetFilePathId)
		{
			AssetFilePathMapper.Instance.Delete (AssetFilePathId);
		}

		public static EntityList <AssetFilePath> FindMany (AssetFilePathFinder finder, int Page, int PageSize)
		{
			return AssetFilePathMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetFilePath> FindMany (AssetFilePathFinder finder)
		{
			return AssetFilePathMapper.Instance.FindMany (finder);
		}

		public static AssetFilePath FindOne (AssetFilePathFinder finder)
		{
			AssetFilePath AssetFilePath = AssetFilePathMapper.Instance.FindOne(finder);
			return AssetFilePath ?? Empty;
		}

		public static int GetCount (AssetFilePathFinder finder)
		{
			return AssetFilePathMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
