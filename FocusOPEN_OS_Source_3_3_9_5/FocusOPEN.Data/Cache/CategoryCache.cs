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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Daydream.Data;

namespace FocusOPEN.Data
{
	public class CategoryCache : BaseEntityCache<Category>
	{
		#region Singleton

		private CategoryCache()
		{
		}

		private static readonly CategoryCache m_Instance = new CategoryCache();

		public static CategoryCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<Category> GetData()
		{
			CategoryFinder finder = new CategoryFinder();
			finder.SortExpressions.Add(new AscendingSort(Category.Columns.Name));
			return Category.FindMany(finder);
		}

		protected override bool Compare(Category entity, int id)
		{
			return entity.CategoryId.GetValueOrDefault() == id;
		}

		protected override Category Empty
		{
			get
			{
				return Category.Empty;
			}
		}

		#endregion

		public Category GetRootCategory(int brandId)
		{
			return (from c in GetList()
			        where c.BrandId == brandId && c.IsRootCategory
			        orderby c.Name
			        select c).FirstOrDefault() ?? Category.Empty;
		}
		
		protected override void AddToCache(List<Category> list)
		{
			HttpRuntime.Cache.Insert(CacheKey, list, null, DateTime.Now.AddMinutes(15), TimeSpan.Zero);
		}
	}
}