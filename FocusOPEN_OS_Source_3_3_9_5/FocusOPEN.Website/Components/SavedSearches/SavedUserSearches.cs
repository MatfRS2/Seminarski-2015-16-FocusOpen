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
using FocusOPEN.Business;
using FocusOPEN.Data;

namespace FocusOPEN.Website.Components
{
	[Serializable]
	public class SavedUserAssetSearch
	{
		#region Private variables

		private AssetFinder m_AssetFinder;

		#endregion

		#region Constructor

		public SavedUserAssetSearch()
		{
			AuditAssetSearchId = 0;
			CurrentCategoryId = -1;
			CategoriesOpen = false;
			FilterOpen = false;
			HasSearch = false;
			SearchResultsView = SearchResultsView.Thumbnails;
			PageSize = 0;
			Page = 1;
		}

		#endregion

		#region Accessors

		public AssetFinder AssetFinder
		{
			get
			{
				if (m_AssetFinder == null)
				{
					m_AssetFinder = SearchManager.GetBaseAssetFinder(SessionInfo.Current.User);
					m_AssetFinder.BrandId = WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();
				}

				return m_AssetFinder;
			}
			set
			{
				m_AssetFinder = value;
				HasSearch = true;
			}
		}

		public int Page { get; set; }

		public int PageSize { get; set; }

		public int AuditAssetSearchId { get; set; }

		public SearchResultsView SearchResultsView { get; set; }

		public bool HasSearch { get; private set; }

		public bool FilterOpen { get; set; }

		public bool CategoriesOpen { get; set; }

		public int CurrentCategoryId { get; set; }

		#endregion

		/// <summary>
		/// Gets the current category.
		/// </summary>
		public Category CurrentCategory
		{
			get
			{
				return CategoryCache.Instance.GetById(CurrentCategoryId);
			}
		}

		/// <summary>
		/// Selects the category.  Sets the current category ID, and updates the asset finder to search this category
		/// and all of it's child categories.
		/// </summary>
		public void SelectCategory(int categoryId)
		{
			CurrentCategoryId = categoryId;
			AssetFinder.CategoryIdList.Clear();

			if (categoryId > 0)
				AssetFinder.CategoryIdList.AddRange(CategoryManager.GetCategoryIdsForSearch(categoryId));
		}
	}
}