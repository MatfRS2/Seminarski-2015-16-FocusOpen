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
	/// This object represents the properties and methods of a AssetMetadataTextArea.
	/// </summary>
	public partial class AssetMetadataTextArea
	{
		#region Constructor
		
		protected AssetMetadataTextArea()
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
		
		public static AssetMetadataTextArea New ()
		{
			return new AssetMetadataTextArea() ;
		}

		public static AssetMetadataTextArea Empty
		{
			get { return NullAssetMetadataTextArea.Instance; }
		}

		public static AssetMetadataTextArea Get (Nullable <Int32> AssetMetadataTextAreaId)
		{
			AssetMetadataTextArea AssetMetadataTextArea = AssetMetadataTextAreaMapper.Instance.Get (AssetMetadataTextAreaId);
			return AssetMetadataTextArea ?? Empty;
		}

		public static AssetMetadataTextArea Update (AssetMetadataTextArea assetMetadataTextArea)
		{
			return AssetMetadataTextAreaMapper.Instance.Update(assetMetadataTextArea) ;
		}

		public static void Delete (Nullable <Int32> AssetMetadataTextAreaId)
		{
			AssetMetadataTextAreaMapper.Instance.Delete (AssetMetadataTextAreaId);
		}

		public static EntityList <AssetMetadataTextArea> FindMany (AssetMetadataTextAreaFinder finder, int Page, int PageSize)
		{
			return AssetMetadataTextAreaMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetMetadataTextArea> FindMany (AssetMetadataTextAreaFinder finder)
		{
			return AssetMetadataTextAreaMapper.Instance.FindMany (finder);
		}

		public static AssetMetadataTextArea FindOne (AssetMetadataTextAreaFinder finder)
		{
			AssetMetadataTextArea AssetMetadataTextArea = AssetMetadataTextAreaMapper.Instance.FindOne(finder);
			return AssetMetadataTextArea ?? Empty;
		}

		public static int GetCount (AssetMetadataTextAreaFinder finder)
		{
			return AssetMetadataTextAreaMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
