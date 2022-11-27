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
	/// This object represents the properties and methods of a AssetFile.
	/// </summary>
	public partial class AssetFile
	{
		#region Constructor
		
		protected AssetFile()
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
		
		public static AssetFile New ()
		{
			return new AssetFile() ;
		}

		public static AssetFile Empty
		{
			get { return NullAssetFile.Instance; }
		}

		public static AssetFile Get (Nullable <Int32> AssetFileId)
		{
			AssetFile AssetFile = AssetFileMapper.Instance.Get (AssetFileId);
			return AssetFile ?? Empty;
		}

		public static AssetFile Update (AssetFile assetFile)
		{
			return AssetFileMapper.Instance.Update(assetFile) ;
		}

		public static void Delete (Nullable <Int32> AssetFileId)
		{
			AssetFileMapper.Instance.Delete (AssetFileId);
		}

		public static EntityList <AssetFile> FindMany (AssetFileFinder finder, int Page, int PageSize)
		{
			return AssetFileMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetFile> FindMany (AssetFileFinder finder)
		{
			return AssetFileMapper.Instance.FindMany (finder);
		}

		public static AssetFile FindOne (AssetFileFinder finder)
		{
			AssetFile AssetFile = AssetFileMapper.Instance.FindOne(finder);
			return AssetFile ?? Empty;
		}

		public static int GetCount (AssetFileFinder finder)
		{
			return AssetFileMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
