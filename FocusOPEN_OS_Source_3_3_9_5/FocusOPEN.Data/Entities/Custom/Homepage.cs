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
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Homepage.
	/// </summary>
	public partial class Homepage
	{
		#region Private Variables

		private User m_LastModifiedByUser;
		private List<HomepageCategory> m_HomepageCategoryList;
		private List<Category> m_CategoryList;

		#endregion

		#region Accessors

		public bool CategoriesLoaded
		{
			get
			{
				return (m_HomepageCategoryList != null);
			}
		}

		public List<HomepageCategory> HomepageCategoryList
		{
			get
			{
				if (m_HomepageCategoryList == null)
				{
					HomepageCategoryFinder finder = new HomepageCategoryFinder {HomepageId = HomepageId.GetValueOrDefault()};
					finder.SortExpressions.Add(new AscendingSort(HomepageCategory.Columns.OrderBy));
					m_HomepageCategoryList = HomepageCategory.FindMany(finder);
				}

				return m_HomepageCategoryList;
			}
		}

		public List<Category> CategoryList
		{
			get
			{
				if (m_CategoryList == null)
				{
					HomepageCategoryFinder finder = new HomepageCategoryFinder {HomepageId = HomepageId.GetValueOrDefault()};
					finder.SortExpressions.Add(new AscendingSort(HomepageCategory.Columns.OrderBy));
					List<HomepageCategory> list = HomepageCategory.FindMany(finder);

					m_CategoryList = new List<Category>();

					foreach (HomepageCategory hc in list)
					{
						Category category = CategoryCache.Instance.GetById(hc.CategoryId);
						m_CategoryList.Add(category);
					}
				}
				return m_CategoryList;
			}
		}

		public User LastModifiedByUser
		{
			get
			{
				if (m_LastModifiedByUser == null)
					m_LastModifiedByUser = User.Get(LastModifiedByUserId);

				return m_LastModifiedByUser;
			}
		}

		#endregion

		public HomepageType HomepageType
		{
			get
			{
				return HomepageTypeCache.Instance.GetById(HomepageTypeId);
			}
		}

		public bool HasBumperPage
		{
			get
			{
				return (!StringUtils.IsBlank(BumperPageHtml));
			}
		}
	}
}