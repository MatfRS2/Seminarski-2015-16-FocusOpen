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
	public class NullAssetCategory : AssetCategory
	{
		#region Singleton implementation

		private NullAssetCategory()
		{
		}

		private static readonly NullAssetCategory m_instance = new NullAssetCategory();

		public static NullAssetCategory Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the AssetId of the AssetCategory object.
		/// </summary>
		public override int AssetId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CategoryId of the AssetCategory object.
		/// </summary>
		public override int CategoryId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsPrimary of the AssetCategory object.
		/// </summary>
		public override bool IsPrimary
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

