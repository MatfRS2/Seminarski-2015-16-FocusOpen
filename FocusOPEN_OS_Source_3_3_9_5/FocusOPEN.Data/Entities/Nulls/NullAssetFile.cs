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

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullAssetFile : AssetFile
	{
		#region Singleton implementation

		private NullAssetFile()
		{
		}

		private static readonly NullAssetFile m_instance = new NullAssetFile();

		public static NullAssetFile Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the AssetId of the AssetFile object.
		/// </summary>
		public override int AssetId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the FileContent of the AssetFile object.
		/// </summary>
		public override byte[] FileContent
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the Filename of the AssetFile object.
		/// </summary>
		public override string Filename
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the FileExtension of the AssetFile object.
		/// </summary>
		public override string FileExtension
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetFileTypeId of the AssetFile object.
		/// </summary>
		public override int AssetFileTypeId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastUpdate of the AssetFile object.
		/// </summary>
		public override DateTime LastUpdate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

