/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Company.
	/// </summary>
	public partial class Company
	{
		#region Private variables

		private List<CompanyBrand> m_CompanyBrandList = null;
		private List<Brand> m_BrandList = null;                                        
		private User m_CreatedByUser = null;

		#endregion

		#region Lazy Loads

		public List<CompanyBrand> CompanyBrandList
		{
			get
			{
				if (m_CompanyBrandList == null)
				{
					if (IsNew)
					{
						m_CompanyBrandList = new List<CompanyBrand>();
					}
					else
					{
						CompanyBrandFinder finder = new CompanyBrandFinder {CompanyId = CompanyId.GetValueOrDefault()};
						m_CompanyBrandList = CompanyBrand.FindMany(finder);
					}
				}
				return m_CompanyBrandList;
			}
		}

		public List<Brand> BrandList
		{
			get
			{
				if (m_BrandList == null)
				{
					if (IsNew)
					{
						m_BrandList = new List<Brand>();
					}
					else
					{
						BrandFinder finder = new BrandFinder();
						finder.BrandIdList.Add(0);
						CompanyBrandList.ForEach(cb => finder.BrandIdList.Add(cb.BrandId));
						m_BrandList = Brand.FindMany(finder);
					}
				}
				return m_BrandList;
			}
		}

		public User CreatedByUser
		{
			get
			{
				if (m_CreatedByUser == null)
					m_CreatedByUser = User.Get(CreatedByUserId);

				return m_CreatedByUser;
			}
		}

		#endregion

		public bool BrandListLoaded
		{
			get
			{
				return (m_BrandList != null);
			}
		}
	}
}