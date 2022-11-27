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
using System.Web.UI.WebControls;
using FocusOPEN.Data;

namespace FocusOPEN.Website.Controls
{
	public class CategoriesTreeView : BaseTreeView<Category>
	{
		public CategoriesTreeView()
		{
			SingleRootNodeCheckable = true;
		}

		#region BaseTreeView Implementation

		protected override IEnumerable<Category> GetRootList()
		{
			return new [] { CategoryCache.Instance.GetRootCategory(BrandId) };
		}

		protected override IEnumerable<Category> GetSubEntitities(Category entity)
		{
			return entity.SubCategories;
		}

		protected override TreeNode GetNodeFromEntity(Category entity)
		{
			return new TreeNode
			                	{
			                		Text = entity.Name,
									Value = entity.CategoryId.ToString()
			                	};
		}

		#endregion
	}
}