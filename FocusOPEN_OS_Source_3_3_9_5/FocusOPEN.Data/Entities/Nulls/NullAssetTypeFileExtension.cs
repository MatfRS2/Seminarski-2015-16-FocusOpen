/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullAssetTypeFileExtension : AssetTypeFileExtension
	{
		#region Singleton implementation

		private NullAssetTypeFileExtension()
		{
		}

		private static readonly NullAssetTypeFileExtension m_instance = new NullAssetTypeFileExtension();

		public static NullAssetTypeFileExtension Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Extension of the AssetTypeFileExtension object.
		/// </summary>
		public override string Extension
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Name of the AssetTypeFileExtension object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetTypeId of the AssetTypeFileExtension object.
		/// </summary>
		public override int AssetTypeId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IconImage of the AssetTypeFileExtension object.
		/// </summary>
		public override byte[] IconImage
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the IconFilename of the AssetTypeFileExtension object.
		/// </summary>
		public override string IconFilename
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsVisible of the AssetTypeFileExtension object.
		/// </summary>
		public override bool IsVisible
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the Plugin of the AssetTypeFileExtension object.
		/// </summary>
		public override Guid Plugin
		{
			get { return Guid.Empty; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

