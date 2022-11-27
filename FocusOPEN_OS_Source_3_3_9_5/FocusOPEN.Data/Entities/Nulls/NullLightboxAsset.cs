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
	public class NullLightboxAsset : LightboxAsset
	{
		#region Singleton implementation

		private NullLightboxAsset()
		{
		}

		private static readonly NullLightboxAsset m_instance = new NullLightboxAsset();

		public static NullLightboxAsset Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the LightboxId of the LightboxAsset object.
		/// </summary>
		public override int LightboxId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetId of the LightboxAsset object.
		/// </summary>
		public override int AssetId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Notes of the LightboxAsset object.
		/// </summary>
		public override string Notes
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreateDate of the LightboxAsset object.
		/// </summary>
		public override DateTime CreateDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the OrderNumber of the LightboxAsset object.
		/// </summary>
		public override Nullable <Int32> OrderNumber
		{
			get { return null; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

