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
using System.Web;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public partial class PageHeader : BaseUserControl
	{
		#region Accessors

		private static SavedUserAssetSearch SavedUserAssetSearch
		{
			get
			{
				return SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch;
			}
			set
			{
				SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch = value;
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Init(object sender, EventArgs e)
		{
			CategorySearchButtonWrapper.Visible = (!WebsiteBrandManager.GetBrand().HideCategorySearch);
			FilterSearchButtonWrapper.Visible = (!WebsiteBrandManager.GetBrand().HideFilterSearch);

            MetadataFilters.FilterSearchClick += (MetadataFilters_FilterSearchClick);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteUtils.TieButton(KeywordsTextBox, SearchButton1);

			if (SavedUserAssetSearch == null || SavedUserAssetSearch.AssetFinder == null)
				SavedUserAssetSearch = new SavedUserAssetSearch();

			AssetFinder finder = SavedUserAssetSearch.AssetFinder;

			if (!Page.IsPostBack)
			{
				string path = VirtualPathUtility.ToAppRelative(Request.Url.AbsolutePath).ToLower();

				if (path.StartsWith("~/searchresults.aspx"))
				{
					// If browse option is selected
					if (Request.QueryString["showCategories"] != null)
					{
						SavedUserAssetSearch.CategoriesOpen = true;
						SavedUserAssetSearch.FilterOpen = false;
					}

					// Setup UI
					KeywordsTextBox.Text = finder.GeneralKeyword;
					BrandDropDownList1.SafeSelectValue(finder.BrandId);
					AssetTypeDropDownList1.SafeSelectValue(finder.AssetTypeId);

					// Only open the filter and populate the dropdowns if the filter is open
					if (SavedUserAssetSearch.CategoriesOpen || SavedUserAssetSearch.FilterOpen || WebUtils.GetIntRequestParam("openfilter", 0) == 1)
					{
						ToggleSearchVisibility(SavedUserAssetSearch.FilterOpen, SavedUserAssetSearch.CategoriesOpen);
					}
				}

				CategoryId = SavedUserAssetSearch.CurrentCategoryId;
			}

			SetupCategories();
		}

		protected void AssetTypeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
		    ToggleAssetTypeFilters();
		}

        /// <summary>
        /// handles the click event from the metadata filters search button
        /// </summary>
        void MetadataFilters_FilterSearchClick(object sender, EventArgs e)
        {
            SearchButton_Click(sender, e);
        }

		protected void SearchButton_Click(object sender, EventArgs e)
		{
			// Get base finder
			AssetFinder finder = SearchManager.GetBaseAssetFinder(CurrentUser);

			// Reset the category list if all brands are selected, or if the brand selector has changed
			if (BrandDropDownList1.SelectedId == 0 || finder.BrandId != BrandDropDownList1.SelectedId)
			{
				finder.CategoryIdList.Clear();
				SavedUserAssetSearch.CurrentCategoryId = -1;
			}

			// Set basic criteria
			finder.GeneralKeyword = KeywordsTextBox.Text.Trim();
			finder.BrandId = BrandDropDownList1.SelectedId;
			finder.AssetTypeId = AssetTypeDropDownList1.SelectedId;

            MetadataFilters.AddAdvancedSearchCriteria(ref finder);
			AddCategorySearchCriteria();

			// Log the search
			AuditAssetSearch aas = AuditLogManager.LogSearch(finder, CurrentUser);

			// Store the finder and clear the audit ID, as we dont want to audit advanced searches
			SavedUserAssetSearch.AssetFinder = finder;
			SavedUserAssetSearch.AuditAssetSearchId = aas.AuditAssetSearchId.GetValueOrDefault();
			SavedUserAssetSearch.Page = 1;

			// Redirect to the search results
			// It will pick up the finder from the session to display results
			Response.Redirect("~/SearchResults.aspx", false);
		}

		protected void ResetSearchButton_Click(object sender, EventArgs e)
		{
			SavedUserAssetSearch.AssetFinder = SearchManager.GetBaseAssetFinder(CurrentUser);
			SavedUserAssetSearch.AssetFinder.BrandId = WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();
			SavedUserAssetSearch.SelectCategory(0);
			Response.Redirect("~/SearchResults.aspx", false);
		}

		protected void FilterResultsLinkButton_Click(object sender, EventArgs e)
		{
            RebindMetadataFilter();

			FiltersPanel.Visible = (!FiltersPanel.Visible);
			TogglePanelButtonText();
			ToggleSearchVisibility();
		}

		protected void CategoriesLinkButton_Click(object sender, EventArgs e)
		{
			CategoryNavigationPanel.Visible = (!CategoryNavigationPanel.Visible);
			TogglePanelButtonText();
			ToggleSearchVisibility();
		}

		protected void BrandDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// When the brand dropdown is changed, all of the metadata dropdowns
			// also need to be updated to display brand specific values
		    MetadataFilters.BrandId = BrandDropDownList1.SelectedId == 0 ? CurrentUser.PrimaryBrandId : BrandDropDownList1.SelectedId;
            MetadataFilters.LoadFilter();

			CategoryId = -1;
			SetupCategories();
		}

		#endregion

		#region Public Methods

		private void ToggleSearchVisibility()
		{
			ToggleSearchVisibility(FiltersPanel.Visible, CategoryNavigationPanel.Visible, true);
		}

		public void ToggleSearchVisibility(bool filtersVisible, bool categoriesVisible)
		{
			ToggleSearchVisibility(filtersVisible, categoriesVisible, true);
		}

		public void ToggleSearchVisibility(bool filtersVisible, bool categoriesVisible, bool updateSavedSearch)
		{
			if (WebsiteBrandManager.GetBrand().HideFilterSearch)
				filtersVisible = false;

			if (WebsiteBrandManager.GetBrand().HideCategorySearch)
				categoriesVisible = false;

			FiltersPanel.Visible = filtersVisible;
			CategoryNavigationPanel.Visible = categoriesVisible;

			if (updateSavedSearch)
			{
				SavedUserAssetSearch.FilterOpen = filtersVisible;
				SavedUserAssetSearch.CategoriesOpen = categoriesVisible;
			}

			if (categoriesVisible)
			{
				// Show the "view all assets" link in the category panel if we have a search keyword
				// This will allow us to clear the text but retain the category, so that all assets
				// in the selected category are displayed.
				ViewAllAssetsLinkButtonWrapper.Visible = (!StringUtils.IsBlank(SavedUserAssetSearch.AssetFinder.GeneralKeyword));
			}

			if (filtersVisible)
			{
				// Get the finder from the session
				var finder = SavedUserAssetSearch.AssetFinder;

                //load meta lists and dropdowns
                RebindMetadataFilter();

                // load metadata filter
                MetadataFilters.LoadFilterFromStoredValues(finder);

				// No need to rebind these
				// BrandDropDownList1.SafeSelectValue(finder.BrandId);
				// AssetTypeDropDownList1.SafeSelectValue(finder.AssetTypeId);
                
                
                
			    ToggleAssetTypeFilters();
			}

			TogglePanelButtonText();
		}

		#endregion

		#region Private Methods

		private void RebindMetadataFilter()
		{
			int brandId = BrandDropDownList1.SelectedId;

			if (brandId == 0)
				brandId = CurrentUser.PrimaryBrandId;
            
            MetadataFilters.BrandId = brandId;
            MetadataFilters.LoadFilter();
		}

		private void TogglePanelButtonText()
		{
			FilterResultsLinkButton.Text = (FiltersPanel.Visible) ? "hide filters [-]" : "show filters [+]";
			CategoriesLinkButton.Text = (CategoryNavigationPanel.Visible) ? "hide categories [-]" : "show categories [+]";
		}

        private void ToggleAssetTypeFilters()
        {
            // Get the asset type
            var assetType = AssetType.Get(AssetTypeDropDownList1.SelectedId);
            MetadataFilters.ToggleAssetTypeFilters(assetType);
        }

		private static void AddCategorySearchCriteria()
		{
			// Don't add anything if category search is disabled
			if (WebsiteBrandManager.GetBrand().HideCategorySearch)
				return;

			if (!SavedUserAssetSearch.CurrentCategory.IsNull)
				SavedUserAssetSearch.SelectCategory(SavedUserAssetSearch.CurrentCategoryId);
		}

		#endregion

		private int CategoryId
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

		protected void BreadcrumbRepeater_ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			if (e.CommandName.Equals("SelectCategory"))
			{
				int categoryId = int.Parse(e.CommandArgument.ToString());
				SelectCategory(categoryId, false);
			}
		}

		protected void SubCategoriesRepeater_ItemCommand(object sender, DataListCommandEventArgs e)
		{
			if (e.CommandName.Equals("SelectCategory"))
			{
				int categoryId = int.Parse(e.CommandArgument.ToString());
				SelectCategory(categoryId, false);
			}
		}

		protected void SubCategoriesRepeater_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(ListItemType.Item) || e.Item.ItemType.Equals(ListItemType.AlternatingItem))
            {
                var category = (Category)e.Item.DataItem;
				int assetsCount = category.AvailableAssetCount;

                var subCategoryLink = (LinkButton)e.Item.FindControl("SubCategoryLink");
            	subCategoryLink.Text = category.Name;
                subCategoryLink.ToolTip = string.Concat(category.Brand.Name, " / ", SiteUtils.GetFullCategoryName(category));

                var assetsCountLabel = (Label)e.Item.FindControl("AssetsCountLabel");
                assetsCountLabel.Text = " (" + assetsCount + ")";
            }
        }

		private void SelectCategory(int categoryId, bool reset)
		{
			if (reset)
				SavedUserAssetSearch.AssetFinder = SearchManager.GetBaseAssetFinder(CurrentUser);

			// Get the category
			Category category = CategoryCache.Instance.GetById(categoryId);

			// Set the brand
			SavedUserAssetSearch.AssetFinder.BrandId = (BrandDropDownList1.SelectedId > 0) ? category.BrandId : 0;

			// Select the category
			SavedUserAssetSearch.SelectCategory(categoryId);

			// Reset the page
			SavedUserAssetSearch.Page = 1;

			// Everything set. Redirect to search results.
			Response.Redirect("~/SearchResults.aspx", false);
		}

		private void BindSubCategories(List<Category> categories)
		{
			int categoriesCount = categories.Count;
			decimal numOfColumns = Math.Ceiling(decimal.Divide(categoriesCount, 5));

			if (numOfColumns > 5)
				numOfColumns = 5;

			categories.Sort((a, b) => a.Name.CompareTo(b.Name));

			SubCategoriesRepeater.RepeatColumns = (int) numOfColumns;
			SubCategoriesRepeater.DataSource = categories;
			SubCategoriesRepeater.DataBind();
		}

		private void SetupCategories()
		{
			Category currentCategory = Category.Empty;
			List<Category> categoryList = new List<Category>();

			if (CategoryId > 0)
			{
				// Display all subcategories od the selected category

				currentCategory = CategoryCache.Instance.GetById(CategoryId);
				categoryList = currentCategory.SubCategories;
			}
			else if (BrandDropDownList1.SelectedId == 0)
			{
				// Display all categories across all brands to which the user has access

				currentCategory = Category.Empty;

				if (CurrentUser.Brands.Count == 1)
				{
					currentCategory = CategoryCache.Instance.GetRootCategory(CurrentUser.Brands[0].BrandId.GetValueOrDefault());
					categoryList = currentCategory.SubCategories;
				}
				else
				{
					foreach (Brand brand in CurrentUser.Brands)
					{
						int brandId = brand.BrandId.GetValueOrDefault();

						Category parentCategory = CategoryCache.Instance.GetRootCategory(brandId);
						categoryList.AddRange(parentCategory.SubCategories);
					}
				}
			}
			else if (BrandDropDownList1.SelectedId > 0)
			{
				// Display all categories in the selected brand

				currentCategory = CategoryCache.Instance.GetRootCategory(BrandDropDownList1.SelectedId);
				categoryList = currentCategory.SubCategories;
			}

			if (currentCategory.IsNull)
			{
				CategoryHeaderPlaceHolder.Visible = false;
				CategoryNameLabel.Text = "All Categories";
			}
			else
			{
				CategoryHeaderPlaceHolder.Visible = true;

				// Populate breadcrumbs
				BreadcrumbRepeater.DataSource = CategoryBreadcrumbNode.BuildBreadCrumbs(currentCategory);
				BreadcrumbRepeater.DataBind();

				// Display the category navigation message
				string mailtoHtml = string.Format("<a href=\"mailto:{0}\">{1}</a>", currentCategory.OwnerEmail, currentCategory.OwnerName);
				CategoryNavigationMessage.Text = currentCategory.Message.Replace("[category contact]", mailtoHtml);
				CategoryNavigationMessageDiv.Visible = (!StringUtils.IsBlank(CategoryNavigationMessage.Text));

				// Populate category name label
				string message = (categoryList.Count > 0) ? "Sub-Categories of " + currentCategory.Name : string.Empty;
				CategoryNameLabel.Text = message;

				// Append a BR onto the start of the category name if there's no category navigation, but there are breadcrumbs
				// and we have a category name. This is a very bad way of maintaining the layout, and an in interim fix
				// until the HTML is updated to use proper sematics with CSS properly controlling the layout.
				if (!CategoryNavigationMessageDiv.Visible && BreadcrumbRepeater.Items.Count > 0 && CategoryNameLabel.Text.Length > 0)
					CategoryNameLabel.Text = "<br />" + CategoryNameLabel.Text;
			}

			// No point showing the category name wrapper if we don't have a category name
			if (StringUtils.IsBlank(CategoryNameLabel.Text))
				CategoryNameDiv.Visible = false;

			BindSubCategories(categoryList);
		}

		protected void BreadcrumbRepeater_PreRender(object sender, EventArgs e)
		{
			foreach (RepeaterItem ri in BreadcrumbRepeater.Items)
			{
				if (ri.ItemIndex < BreadcrumbRepeater.Items.Count-1)
					continue;

				LinkButton BreadcrumbLinkButton = (LinkButton)ri.FindControl("BreadcrumbLinkButton");
				BreadcrumbLinkButton.Attributes["style"] = "text-decoration:none";
			}
		}

		protected void ViewAllAssetsInCategoryLinkButton_Click(object sender, EventArgs e)
		{
			SelectCategory(SavedUserAssetSearch.CurrentCategoryId, true);
		}
	}
}