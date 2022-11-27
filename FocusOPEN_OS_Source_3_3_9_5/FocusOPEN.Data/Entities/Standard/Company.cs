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
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Company.
	/// </summary>
	public partial class Company
	{
		#region Constructor
		
		protected Company()
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
		
		public static Company New ()
		{
			return new Company() ;
		}

		public static Company Empty
		{
			get { return NullCompany.Instance; }
		}

		public static Company Get (Nullable <Int32> CompanyId)
		{
			Company Company = CompanyMapper.Instance.Get (CompanyId);
			return Company ?? Empty;
		}

		public static Company Update (Company company)
		{
			if (company.BrandListLoaded)
			{
				JoinableList jList = new JoinableList();
				company.BrandList.ForEach(b => jList.Add(b.Name));
				company.Brands = jList.ToString();
			}

			CompanyMapper.Instance.Update(company);

			if (company.BrandListLoaded)
			{
				CompanyBrandMapper.Instance.DeleteBrands(company.CompanyId);
				
				foreach (Brand brand in company.BrandList)
				{
					CompanyBrand cb = CompanyBrand.New();
					cb.CompanyId = company.CompanyId.GetValueOrDefault();
					cb.BrandId = brand.BrandId.GetValueOrDefault();
					CompanyBrand.Update(cb);
				}
			}

			return company ;
		}

		public static void Delete (Nullable <Int32> CompanyId)
		{
			CompanyMapper.Instance.Delete (CompanyId);
		}

		public static EntityList <Company> FindMany (CompanyFinder finder, int Page, int PageSize)
		{
			return CompanyMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Company> FindMany (CompanyFinder finder)
		{
			return CompanyMapper.Instance.FindMany (finder);
		}

		public static Company FindOne (CompanyFinder finder)
		{
			Company Company = CompanyMapper.Instance.FindOne(finder);
			return Company ?? Empty;
		}

		public static int GetCount (CompanyFinder finder)
		{
			return CompanyMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
