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
	/// This object represents the properties and methods of a AssetMetadataTextField.
	/// </summary>
	public partial class AssetMetadataTextField
	{
		#region Constructor
		
		protected AssetMetadataTextField()
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
		
		public static AssetMetadataTextField New ()
		{
			return new AssetMetadataTextField() ;
		}

		public static AssetMetadataTextField Empty
		{
			get { return NullAssetMetadataTextField.Instance; }
		}

		public static AssetMetadataTextField Get (Nullable <Int32> AssetMetadataTextFieldId)
		{
			AssetMetadataTextField AssetMetadataTextField = AssetMetadataTextFieldMapper.Instance.Get (AssetMetadataTextFieldId);
			return AssetMetadataTextField ?? Empty;
		}

		public static AssetMetadataTextField Update (AssetMetadataTextField assetMetadataTextField)
		{
			return AssetMetadataTextFieldMapper.Instance.Update(assetMetadataTextField) ;
		}

		public static void Delete (Nullable <Int32> AssetMetadataTextFieldId)
		{
			AssetMetadataTextFieldMapper.Instance.Delete (AssetMetadataTextFieldId);
		}

		public static EntityList <AssetMetadataTextField> FindMany (AssetMetadataTextFieldFinder finder, int Page, int PageSize)
		{
			return AssetMetadataTextFieldMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AssetMetadataTextField> FindMany (AssetMetadataTextFieldFinder finder)
		{
			return AssetMetadataTextFieldMapper.Instance.FindMany (finder);
		}

		public static AssetMetadataTextField FindOne (AssetMetadataTextFieldFinder finder)
		{
			AssetMetadataTextField AssetMetadataTextField = AssetMetadataTextFieldMapper.Instance.FindOne(finder);
			return AssetMetadataTextField ?? Empty;
		}

		public static int GetCount (AssetMetadataTextFieldFinder finder)
		{
			return AssetMetadataTextFieldMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
