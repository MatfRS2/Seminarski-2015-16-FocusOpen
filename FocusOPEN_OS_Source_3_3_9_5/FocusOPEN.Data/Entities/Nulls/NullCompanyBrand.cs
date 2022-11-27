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
	/// This object represents a null CompanyBrand.
	/// </summary>
	[Serializable]
	public class NullCompanyBrand : CompanyBrand
	{
		#region Singleton implementation

		private NullCompanyBrand()
		{
		}

		private static readonly NullCompanyBrand m_instance = new NullCompanyBrand();

		public static NullCompanyBrand Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the CompanyId of the CompanyBrand object.
		/// </summary>
		public override int CompanyId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the BrandId of the CompanyBrand object.
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

