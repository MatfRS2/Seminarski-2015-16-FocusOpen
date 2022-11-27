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
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class NestedDataCategoryDropDownList : AbstractNestedDataDropDownList<Category>
	{
		#region Constructor

		public NestedDataCategoryDropDownList()
		{
			BrandId = 0;
		}

		#endregion

		#region AbstractNestedDropDownList Implementation

        public override sealed NestedDataControlHelper<Category> NestedDataHelper
        {
            get
            {
                return new NestedDataControlHelper<Category>(GetParentList,GetChildren, GetEntityName, GetEntityId, " ");
            }
        }

	    public override IEnumerable<Category> GetParentList()
		{
			if (BrandId > 0)
				return new[] {CategoryCache.Instance.GetRootCategory(BrandId)};

			return (from brand in BrandCache.Instance.GetList()
			        where SessionInfo.Current.User.CanAccessBrand(brand.BrandId.GetValueOrDefault())
			        select CategoryCache.Instance.GetRootCategory(brand.BrandId.GetValueOrDefault())).OrderBy(c => c.Brand.Name).ThenBy(c => c.CategoryOrder).ThenBy(c => c.Name);
		}

        public override IEnumerable<Category> GetChildren(Category entity)
		{
			return entity.SubCategories;
		}

        public override string GetEntityName(Category entity)
		{
			string name = entity.Name;

			if (BrandId <= 0 && entity.IsRootCategory)
				name = entity.Brand.Name;

			return string.Concat(name, " (", entity.FullAssetCount, ")");
		}

        public override int GetEntityId(Category entity)
		{
			return entity.CategoryId.GetValueOrDefault();
		}

		#endregion
	}
}