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
	/// <summary>
	/// This object represents a null AssetFilePath.
	/// </summary>
	[Serializable]
	public class NullAssetFilePath : AssetFilePath
	{
		#region Singleton implementation

		private NullAssetFilePath()
		{
		}

		private static readonly NullAssetFilePath m_instance = new NullAssetFilePath();

		public static NullAssetFilePath Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Path of the AssetFilePath object.
		/// </summary>
		public override string Path
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDefault of the AssetFilePath object.
		/// </summary>
		public override bool IsDefault
		{
			get { return false; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

