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
	public class NullAssetMetadataTextArea : AssetMetadataTextArea
	{
		#region Singleton implementation

		private NullAssetMetadataTextArea()
		{
		}

		private static readonly NullAssetMetadataTextArea m_instance = new NullAssetMetadataTextArea();

		public static NullAssetMetadataTextArea Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the AssetId of the AssetMetadataTextArea object.
		/// </summary>
		public override int AssetId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the GroupNumber of the AssetMetadataTextArea object.
		/// </summary>
		public override int GroupNumber
		{
			get { return 0; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

