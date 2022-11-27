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
	/// This object represents a null AssetImageSize.
	/// </summary>
	[Serializable]
	public class NullAssetImageSize : AssetImageSize
	{
		#region Singleton implementation

		private NullAssetImageSize()
		{
		}

		private static readonly NullAssetImageSize m_instance = new NullAssetImageSize();

		public static NullAssetImageSize Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Description of the AssetImageSize object.
		/// </summary>
		public override string Description
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Height of the AssetImageSize object.
		/// </summary>
		public override int Height
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Width of the AssetImageSize object.
		/// </summary>
		public override int Width
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the DotsPerInch of the AssetImageSize object.
		/// </summary>
		public override int DotsPerInch
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the FileSuffix of the AssetImageSize object.
		/// </summary>
		public override string FileSuffix
		{
			get { return String.Empty; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

