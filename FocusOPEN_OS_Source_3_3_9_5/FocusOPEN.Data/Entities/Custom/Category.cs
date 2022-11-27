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
using System.Linq;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Category.
	/// </summary>
	public partial class Category
	{
		public virtual bool IsRootCategory
		{
			get
			{
				return (!ParentCategoryId.HasValue);
			}
		}

		public virtual Brand Brand
		{
			get
			{
				return BrandCache.Instance.GetById(BrandId);
			}
		}

		public virtual Category ParentCategory
        {
            get
            {
                return CategoryCache.Instance.GetById(ParentCategoryId.GetValueOrDefault(-1));
            }
        }

		public virtual List<Category> SubCategories
        {
            get
            {
            	return (from c in CategoryCache.Instance.GetList()
            	        where c.ParentCategoryId.HasValue && c.ParentCategoryId == CategoryId
            	        orderby c.CategoryOrder, c.Name
            	        select c).ToList();
            }
        }
	}
}
