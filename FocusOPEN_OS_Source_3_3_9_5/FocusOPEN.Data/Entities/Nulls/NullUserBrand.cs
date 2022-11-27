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
	/// This object represents a null UserBrand.
	/// </summary>
	[Serializable]
	public class NullUserBrand : UserBrand
	{
		#region Singleton implementation

		private NullUserBrand()
		{
		}

		private static readonly NullUserBrand m_instance = new NullUserBrand();

		public static NullUserBrand Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the UserId of the UserBrand object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the BrandId of the UserBrand object.
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

