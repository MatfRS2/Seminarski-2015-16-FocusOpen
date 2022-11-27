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
using System.Text.RegularExpressions;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class CategoryManager
	{
		/// <summary>
		/// Checks that the brand with the specified ID has a root category
		/// and if not, creates oned named "Assets".
		/// </summary>
		/// <param name="brandId"></param>
		public static void CheckRootCategory(int brandId)
		{
			// Nothing to do if parent category already exists
			if (!CategoryCache.Instance.GetRootCategory(brandId).IsNull)
				return;

			// Otherwise, create a root category named assets
			CreateCategory(null, brandId, "Assets", string.Empty, string.Empty, string.Empty, User.Get(1));
		}

		/// <summary>
		/// Creates a new Category
		/// </summary>
		/// <param name="parentCategoryId">The id of the parent category, 0 belongs to root</param>
		/// <param name="brandId">The brand ID</param>
		/// <param name="name">New name</param>
		/// <param name="externalRef">External Reference</param>
		/// <param name="message">Message to display when selected</param>
		/// <param name="synonyms">Synonyms for the search functionality</param>
		/// <param name="user">The user creating the category</param>
		public static int CreateCategory(int? parentCategoryId, int brandId, string name, string externalRef, string message, string synonyms, User user)
		{
			Category category = Category.New();

			category.ParentCategoryId = parentCategoryId;
			category.BrandId = brandId;
			category.Name = name;
			category.ExternalRef = externalRef;
			category.Message = message;
			category.Synonyms = ProcessSynonyms(synonyms);
			category.OwnerUserId = user.UserId.GetValueOrDefault();

			ErrorList errors = Validate(category);

			if (errors.Count > 0)
				throw new CategoryException(errors);

			int subCategoriesCount = Category.Get(parentCategoryId.GetValueOrDefault()).SubCategories.Count;
			category.CategoryOrder = subCategoriesCount + 1;

			Category.Update(category);

			CacheManager.InvalidateCache("Category", CacheType.All);

			AuditLogManager.LogUserAction(user, AuditUserAction.AddCategory, string.Format("Created category {0}, ID: {1}", category.Name, category.CategoryId));

			return category.CategoryId.GetValueOrDefault();
		}

        /// <summary>
        /// Creates a category hierarchy based on a DOS style path (folder delimiter is "\")
        /// </summary>
        /// <param name="parentCategoryId">The id of the parent category, 0 belongs to root</param>
        /// <param name="brandId">The brand ID</param>
        /// <param name="path">DOS style path statement (excluding drive letter:)</param>
        /// <param name="user">The user creating the category</param>
        /// <returns>The ID of the lowest level category</returns>
        public static int CreateCategoryTreeFromPath(int? parentCategoryId, int brandId, string path, User user)
        {
            var folders = from f in path.Split('\\','/')
                                    where !f.Contains(':') && f!=string.Empty
                                    select f;

            int? currentParentId = parentCategoryId;
            int? newId = parentCategoryId;

            // Create a finder and pre-load the BrandId as this won't be changing
            CategoryFinder finder = new CategoryFinder{BrandId = brandId};
            
            foreach(string folder in folders)
            {
                // Check to see if the category already exists
                finder.ParentCategoryId = currentParentId;
                finder.Name = folder;
                Category existingCategory = Category.FindOne(finder);

                if (existingCategory.IsNull)
				{
					newId = CreateCategory(currentParentId, brandId, folder, string.Empty, string.Empty, string.Empty, user);
				}
                else
				{
                    newId = existingCategory.CategoryId;
				}

                currentParentId = newId;
            }

            return newId.GetValueOrDefault();
        }

		/// <summary>
		/// Updates a Category
		/// </summary>
		/// <param name="categoryId"> The id of the category to be updated</param>        
		/// <param name="categoryName">New name</param>
		/// <param name="externalRef">External Reference</param>
		/// <param name="message">Message to display when selected</param>
		/// <param name="synonyms">Synonyms for the search functionality</param>
		/// <param name="user">The user updating the category</param>
		public static void UpdateCategory(int categoryId, string categoryName, string externalRef, string message, string synonyms, User user)
		{
			Category category = Category.Get(categoryId);
			category.Name = categoryName;
			category.ExternalRef = externalRef;
			category.Message = message;
			category.Synonyms = ProcessSynonyms(synonyms);
			UpdateCategory(category);
			AuditLogManager.LogUserAction(user, AuditUserAction.ModifyCategory, string.Format("Modified category {0}, ID: {1}", category.Name, category.CategoryId));
		}

		/// <summary>
		/// Updates a Category
		/// </summary>
		/// <param name="categoryId"> The id of the category to be updated</param>
		/// <param name="categoryName"> New name</param>
		/// <param name="user">User making the change</param>
		public static void RenameCategory(int categoryId, string categoryName, User user)
		{
			Category category = Category.Get(categoryId);
			string oldname = category.Name;
			category.Name = categoryName;
			UpdateCategory(category);
			AuditLogManager.LogUserAction(user, AuditUserAction.ModifyCategory, string.Format("Renamed {0} to {1}, ID: {2}", oldname, categoryName, category.CategoryId));
		}

		/// <summary>
		/// Gets the number of assets assigned to the category with the specified ID (including it's child categories)
		/// </summary>
		public static int GetFullAssetCount(int categoryId)
		{
			AssetFinder finder = new AssetFinder();
			finder.CategoryIdList.Add(Int32.MinValue);
			finder.CategoryIdList.AddRange(GetCategoryIdsForSearch(categoryId));
			return Asset.GetCount(finder);
		}

		/// <summary>
		/// Gets the number of fully published assets assigned to the category with the specified ID (including it's child categories)
		/// </summary>
		public static int GetPublishedAssetCount(int categoryId)
		{
			AssetFinder finder = new AssetFinder();
			finder.CategoryIdList.Add(Int32.MinValue);
			finder.CategoryIdList.AddRange(GetCategoryIdsForSearch(categoryId));
			finder.IsCompletelyPublished = true;
			return Asset.GetCount(finder);
		}

		/// <summary>
		/// Updates the full asset count for the category witht he specified ID and invalidates the cache
		/// </summary>
		public static void UpdateCategoryFullAssetCount(int categoryId)
		{
			Category category = Category.Get(categoryId);
			category.FullAssetCount = GetFullAssetCount(categoryId);
			UpdateCategory(category);
		}

		/// <summary>
		/// Deletes a Category only if it doesn't have children
		/// </summary>
		/// <param name="categoryId"> The id of the category to be deleted</param>
		/// <param name="user">User doing the delete</param>
		public static void DeleteCategory(int categoryId, User user)
		{
			Category category = Category.Get(categoryId);

			if (category.IsRootCategory)
				throw new CategoryException(string.Format("The category named '{0}' cannot be deleted because it is the root category", category.Name)) { ErrorCode = "root-category"};
			
			if (category.SubCategories.Count > 0)
				throw new CategoryException(string.Format("The category named '{0}' cannot be deleted because it contains sub-categories", category.Name)) { ErrorCode = "has-subcategories" };

			int count = GetFullAssetCount(categoryId);

			if (count > 0)
			{
				// This should be caught client-side, so update the asset count and invalidate cache
				// In case the external category count updater has not updated it yet.

				category.FullAssetCount = count;
				Category.Update(category);
				CacheManager.InvalidateCache("Category", CacheType.All);

				throw new CategoryException(string.Format("The category named '{0}' cannot be deleted because it has {1} assets assigned to it", category.Name, count)) { ErrorCode = "has-assets"};
			}

			Category.Delete(categoryId);
			CacheManager.InvalidateCache("Category", CacheType.All);

			AuditLogManager.LogUserAction(user, AuditUserAction.DeleteCategory, string.Format("Deleted category: {0}, ID: {1}", category.Name, category.CategoryId));
		}

		/// <summary>
		/// Reorders the actual tree
		/// </summary>
		/// <param name="categoryId"> The category to change the order</param>
		/// <param name="parentId"> The parent category</param>
		/// <param name="order"> The new order</param>
		/// <param name="user">The user reordering the categories</param>
		public static void Reorder(int categoryId, int parentId, int order, User user)
		{
			Category category = Category.Get(categoryId);
			Category parentCategory = Category.Get(parentId);
			List<Category> subCategories = parentCategory.SubCategories;

			// Reorder top level categories
			foreach (Category subCategory in subCategories)
			{
				if ((subCategory.CategoryOrder >= order) && (subCategory.CategoryId != categoryId))
				{
					subCategory.CategoryOrder += 1;
					Category.Update(subCategory);
				}
			}

			// Update category order
			category.ParentCategoryId = parentId;
			category.CategoryOrder = order;
			Category.Update(category);

			CacheManager.InvalidateCache("Category", CacheType.All);

			AuditLogManager.LogUserAction(user, AuditUserAction.ModifyCategory, string.Format("Reordered category {0}, ID: {1}", category.Name, category.CategoryId));
		}

		/// <summary>
		/// Process synonyms so it can be filtered during search
		/// </summary>
		/// <param name="synonyms"> Synonyms to process</param>
		/// <returns> Synonyms splited by a ;</returns>
		private static string ProcessSynonyms(string synonyms)
		{
			if (synonyms != string.Empty)
			{
				string result = string.Empty;
				string[] array = Regex.Split(synonyms, "\r\n");

				return array.Where(synonym => !StringUtils.IsBlank(synonym)).Aggregate(result, (m_Current, synonym) => m_Current + (synonym + ";"));
			}
			
			return string.Empty;
		}

		/// <summary>
		/// Updates all the properties of a category
		/// </summary>
		/// <param name="category">Category to update</param>
		private static void UpdateCategory(Category category)
		{
			ErrorList errors = Validate(category);

			if (errors.Count > 0)
				throw new CategoryException(errors);

			Category.Update(category);

			CacheManager.InvalidateCache("Category", CacheType.All);
		}

		/// <summary>
		/// Get category by synonym to include in search
		/// </summary>
		/// <param name="synonym"> Synonym to search by</param>
		/// <returns> Category id</returns>
		public static List<int> SearchCategoryBySynonym(string synonym)
		{
			var categoriesToSearch = new List<int>();
			bool useWildCard = false;

			if (synonym != string.Empty)
			{
				IList<Category> categories = CategoryCache.Instance.GetList();

				synonym = synonym.ToLower().Trim();

				// Using wildcards
				if (synonym.IndexOf("*") > -1)
				{
					useWildCard = true;

					if (synonym.StartsWith("*"))
					{
						synonym = synonym.Substring(1);
					}
					if (synonym.EndsWith("*"))
					{
						synonym = synonym.Substring(0, synonym.Length - 1);
					}
				}

				foreach (Category category in categories)
				{
					string categoryName = category.Name.ToLower().Trim();

					if (useWildCard)
					{
						if ((category.Synonyms.ToLower().IndexOf(synonym) > -1) || (categoryName.IndexOf(synonym) > -1))
						{
							categoriesToSearch.AddRange(GetCategoryIdsForSearch(category.CategoryId.GetValueOrDefault()));
						}
					}
					else
					{
						if (category.Synonyms.ToLower().Contains(synonym.ToLower() + ";") || categoryName.Equals(synonym))
						{
							categoriesToSearch.AddRange(GetCategoryIdsForSearch(category.CategoryId.GetValueOrDefault()));
						}
					}
				}
			}

			return categoriesToSearch;
		}

		/// <summary>
		/// Gets the category and sub-categories ids for search porpouses
		/// </summary>
		/// <param name="categoryId"> The selected category</param>
		/// <returns> Categories ids for search</returns>
		public static List<int> GetCategoryIdsForSearch(int categoryId)
		{
			// No point returning a category list if we don't have a parent
			if (categoryId <= 0)
				return new List<int>();

			// Initialise the list with the parent ID
			var categoryIdList = new List<int> {categoryId};

			// Get the parent category
			var category = CategoryCache.Instance.GetById(categoryId);

			// Add all of the children category ID's
			foreach (var subCategory in category.SubCategories)
				categoryIdList.AddRange(GetCategoryIdsForSearch(subCategory.CategoryId.GetValueOrDefault()));

			// All done, return the list
			return categoryIdList;
		}

		#region Validation

		/// <summary>
		/// Validates if category values are correct
		/// </summary>
		/// <param name="category"> The category to evaluate</param>
		private static ErrorList Validate(Category category)
		{
			var errors = new ErrorList();

			if (StringUtils.IsBlank(category.Name))
				errors.Add("Category name is required");

			if (category.ParentCategoryId.HasValue && CategoryCache.Instance.GetById(category.ParentCategoryId.Value).IsNull)
				errors.Add("Parent category doesn't exist");

			return errors;
		}

		#endregion
	}
}