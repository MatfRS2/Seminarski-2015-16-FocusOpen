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
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public partial class CategoryBreadcrumb : UserControl
	{
		#region Events

		public event RepeaterCommandEventHandler SelectCategory;

		#endregion

		#region Accessors

		public int CategoryId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "CategoryId", 0);
			}
			set
			{
				ViewState["CategoryId"] = value;
			}
		}

		private Category CurrentCategory
		{
			get
			{
				return CategoryCache.Instance.GetById(CategoryId);
			}
		}

		private Stack Breadcrumb
		{
			get
			{
				if (ViewState["Breadcrumb"] == null)
					ViewState["Breadcrumb"] = new Stack();

				return (Stack) ViewState["Breadcrumb"];
			}
			set
			{
				ViewState["Breadcrumb"] = value;
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				CreateBreadcrumb();
			}
			else
			{
				Bind();
			}
		}

		protected void BreadcrumbRepeater_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			if (e.CommandName.Equals("SelectCategory"))
			{
				SelectCategory(sender, e);
			}
		}

		#endregion

		#region Private Methods

		private void CreateBreadcrumb()
		{
			if (!CurrentCategory.IsNull)
				Breadcrumb = CategoryBreadcrumbNode.BuildBreadCrumbs(CurrentCategory);

			Bind();
		}

		private void Bind()
		{
			BreadcrumbRepeater.DataSource = Breadcrumb;
			BreadcrumbRepeater.DataBind();
		}

		#endregion
	}

	[Serializable]
	public class CategoryBreadcrumbNode
	{
		#region Properties

		public int CategoryId { get; set; }
		public string Name { get; set; }
		public int AssetsCount { get; set; }

		#endregion

		#region Constructor

		private CategoryBreadcrumbNode(int categoryId, string name, int assetsCount)
		{
			CategoryId = categoryId;
			Name = name;
			AssetsCount = assetsCount;
		}

		#endregion

		public static Stack BuildBreadCrumbs(Category category)
		{
			Stack breadcrumb = new Stack();

			// Add all children categories
			while (category.ParentCategoryId.HasValue)
			{
				int categoryId = category.CategoryId.GetValueOrDefault();
				string name = category.Name;
				int assetCount = category.AvailableAssetCount;

				breadcrumb.Push(new CategoryBreadcrumbNode(categoryId, name, assetCount));

				category = category.ParentCategory;
			}

			// Add parent category
			if (!category.IsNull && category.IsRootCategory)
			{
				int categoryId = category.CategoryId.GetValueOrDefault();
				string name = (SessionInfo.Current.User.Brands.Count > 1) ? category.Brand.Name : category.Name;
				int assetCount = category.AvailableAssetCount;
				
				breadcrumb.Push(new CategoryBreadcrumbNode(categoryId, name, assetCount));
			}

			// Add "all categories" option
			if (SessionInfo.Current.User.Brands.Count > 1)
				breadcrumb.Push(new CategoryBreadcrumbNode(-1, "All Categories", GetAllCategoriesAssetCount()));

			return breadcrumb;
		}

		private static int GetAllCategoriesAssetCount()
		{
			AssetFinder finder = SearchManager.GetBaseAssetFinder(SessionInfo.Current.User);
			return Asset.GetCount(finder);
		}
	}
}