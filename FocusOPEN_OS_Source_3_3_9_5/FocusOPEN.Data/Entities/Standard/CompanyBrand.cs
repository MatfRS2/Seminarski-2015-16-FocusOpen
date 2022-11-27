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
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a CompanyBrand.
	/// </summary>
	public partial class CompanyBrand
	{
		#region Constructor
		
		protected CompanyBrand()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static CompanyBrand New ()
		{
			return new CompanyBrand() ;
		}

		public static CompanyBrand Empty
		{
			get { return NullCompanyBrand.Instance; }
		}

		public static CompanyBrand Get (Nullable <Int32> CompanyBrandId)
		{
			CompanyBrand CompanyBrand = CompanyBrandMapper.Instance.Get (CompanyBrandId);
			return CompanyBrand ?? Empty;
		}

		public static CompanyBrand Update (CompanyBrand companyBrand)
		{
			return CompanyBrandMapper.Instance.Update(companyBrand) ;
		}

		public static void Delete (Nullable <Int32> CompanyBrandId)
		{
			CompanyBrandMapper.Instance.Delete (CompanyBrandId);
		}

		public static EntityList <CompanyBrand> FindMany (CompanyBrandFinder finder, int Page, int PageSize)
		{
			return CompanyBrandMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <CompanyBrand> FindMany (CompanyBrandFinder finder)
		{
			return CompanyBrandMapper.Instance.FindMany (finder);
		}

		public static CompanyBrand FindOne (CompanyBrandFinder finder)
		{
			CompanyBrand CompanyBrand = CompanyBrandMapper.Instance.FindOne(finder);
			return CompanyBrand ?? Empty;
		}

		public static int GetCount (CompanyBrandFinder finder)
		{
			return CompanyBrandMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
