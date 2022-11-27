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
	/// This object represents a null LightboxBrand.
	/// </summary>
	[Serializable]
	public class NullLightboxBrand : LightboxBrand
	{
		#region Singleton implementation

		private NullLightboxBrand()
		{
		}

		private static readonly NullLightboxBrand m_instance = new NullLightboxBrand();

		public static NullLightboxBrand Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the LightboxId of the LightboxBrand object.
		/// </summary>
		public override int LightboxId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the BrandId of the LightboxBrand object.
		/// </summary>
		public override int BrandId
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

