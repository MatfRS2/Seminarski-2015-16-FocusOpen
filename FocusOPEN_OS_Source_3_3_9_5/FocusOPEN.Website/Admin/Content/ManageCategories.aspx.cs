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
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;

namespace FocusOPEN.Website.Admin
{
    public partial class ManageCategories : BaseAdminPage
    {
    	#region Private Variables

    	private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    	#endregion

		#region Accessors

    	protected int BrandId
		{
			get
			{
				return (BrandSelector.Visible) ? BrandDropDownList1.SelectedId : CurrentUser.PrimaryBrandId;
			}
		}

		#endregion

    	#region WebMethods

    	[WebMethod]
    	public static string GetCategory(int categoryId)
    	{
    		Category category = Category.Get(categoryId);
    		return GetJsonResponse(category);
    	}

    	[WebMethod]
    	public static string GetAllCategories(int brandId)
    	{
    		IList<Category> categories = GetParentCategories(brandId);
    		return GetJsonResponse(categories);
    	}

		[WebMethod]
		public static void SetupCategoryDelete(int categoryId)
		{
			// Find all assets assigned to this category
			AssetFinder finder = new AssetFinder();
			finder.CategoryIdList.Add(categoryId);
			List<Asset> assets = Asset.FindMany(finder);

			// Get the ID's
			List<int> list = assets.Select(asset => asset.AssetId.GetValueOrDefault()).ToList();

			// Save them in the session. Redirect will take place client side.
			HttpContext.Current.Session["BulkAction_AssetIdList"] = list;
			HttpContext.Current.Session["BulkAction_DefaultCategoryId"] = categoryId;
			HttpContext.Current.Session["BulkAction_DeleteCategory"] = true;
		}

    	[WebMethod]
		public static DeleteCategoryResponse DeleteCategory(int categoryId)
    	{
			try
			{
				CategoryManager.DeleteCategory(categoryId, SessionInfo.Current.User);
				return new DeleteCategoryResponse {CategoryId = categoryId, Success = true, Message = "Category deleted successfully"};
			}
			catch (CategoryException cex)
			{
				return new DeleteCategoryResponse {CategoryId = categoryId, Success = false, Message = cex.Message, ErrorCode = cex.ErrorCode};
			}
			catch (Exception ex)
			{
				return new DeleteCategoryResponse { CategoryId = categoryId, Success = false, Message = ex.Message, ErrorCode = "unknown-error" };
			}
    	}

    	[WebMethod]
    	public static void RenameCategory(int categoryId, string newName)
    	{
    		CategoryManager.RenameCategory(categoryId, newName, CurrentUser);
    	}

    	[WebMethod]
    	public static void MoveCategory(int categoryId, int parentId, int orderId)
    	{
    		CategoryManager.Reorder(categoryId, parentId, orderId, CurrentUser);
    	}

    	#endregion

    	#region Page Events

    	protected void Page_Init(object sender, EventArgs e)
    	{
			// Only show brand selector to super-admins when there is more than one brand available
    		BrandSelector.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);
    	}

    	protected void Page_Load(object sender, EventArgs e)
    	{
			// Reload the category cache
    		CacheManager.InvalidateCache("Category", CacheType.Local);

    		// First ensure brand has a root category
    		CategoryManager.CheckRootCategory(BrandId);

    		// Get list of categories in selected brand
    		IList<Category> categories = GetParentCategories(BrandId);

    		// Create root category if none exists
    		if (categories.Count == 0)
    			CategoryManager.CreateCategory(-1, BrandId, "Assets", string.Empty, string.Empty, string.Empty, CurrentUser);

			if (!Page.IsPostBack)
			{
				int brandId = WebUtils.GetIntRequestParam("BrandId", 0);
				BrandDropDownList1.SafeSelectValue(brandId);
			}
    	}

		protected void UpdateButton_Click(object sender, EventArgs e)
		{
			int categoryId = NumericUtils.ParseInt32(CategoryIdTextBox.Value, 0);
			int parentId = NumericUtils.ParseInt32(ParentIdTextBox.Value, 0);

			if (string.IsNullOrEmpty(CategoryIdTextBox.Value) && !string.IsNullOrEmpty(ParentIdTextBox.Value))
			{
				CreateCategory(parentId);
			}
			else
			{
				UpdateCategory(categoryId);
			}
		}

    	#endregion

    	#region Private Methods

		private static IList<Category> GetParentCategories(int brandId)
		{
			CategoryFinder finder = new CategoryFinder { BrandId = brandId, ParentCategoryId = Int32.MinValue };
			return Category.FindMany(finder);
		}

    	/// <summary>
    	/// Generates a Json response as expected by jsTree
    	/// </summary>
    	/// <param name="categories">The categories that will be sent in the response</param>
    	/// <returns>A Json formatted string</returns>
    	private static string GetJsonResponse(IList<Category> categories)
    	{
    		StringBuilder response = new StringBuilder();
    		response.Append("[");

    		for (int i = 0; i < categories.Count; i++)
    		{
    			Category category = categories[i];

    			response.Append("{");
    			response.AppendFormat("attributes: {{ id: \"{0}\", categoryOrder:\"{1}\", assetCount:\"{2}\"}}", category.CategoryId, category.CategoryOrder, category.FullAssetCount);
    			response.AppendFormat(", data: \"{0}\"", category.Name);

    			if (category.SubCategories.Count > 0)
    				response.AppendFormat(", children: {0}", GetJsonResponse(category.SubCategories));

    			response.Append("}");

    			if (i + 1 < categories.Count)
    				response.Append(", ");
    		}

    		response.Append("]");
    		return response.ToString();
    	}

    	private static string GetJsonResponse(Category category)
    	{
    		JavaScriptSerializer serializer = new JavaScriptSerializer();
    		return serializer.Serialize(GetCategoryForJson(category));
    	}

    	private static CategoryForJson GetCategoryForJson(Category category)
    	{
			return new CategoryForJson { CategoryId = category.CategoryId, Name = category.Name, ExternalRef = category.ExternalRef, Message = category.Message, synonyms = category.Synonyms, AssetCount = category.FullAssetCount };
    	}

		private void CreateCategory(int parentId)
		{
			try
			{
				string name = NameTextBox.Text;
				string reference = ExternalRefTextBox.Text;
				string message = MessageTextBox.Text;
				string synonyms = SynonymsTextBox.Text;

				int newCatId = CategoryManager.CreateCategory(parentId, BrandId, name, reference, message, synonyms, CurrentUser);
				CategoryIdTextBox.Value = newCatId.ToString();
				FeedbackLabel1.SetSuccessMessage("Category created successfully");
			}
			catch (CategoryException categoryEx)
			{
				FeedbackLabel1.SetErrorMessage("the following errors occured:", categoryEx.Errors);
			}
			catch (Exception ex)
			{
				m_Logger.Fatal(string.Format("Error creating category with parent id: {0}", parentId), ex);
				FeedbackLabel1.SetErrorMessage("An unhandled error occured", ex.ToString());
			}
		}

		private void UpdateCategory(int categoryId)
		{
			try
			{
				string name = NameTextBox.Text;
				string reference = ExternalRefTextBox.Text;
				string message = MessageTextBox.Text;
				string synonyms = SynonymsTextBox.Text;

				CategoryManager.UpdateCategory(categoryId, name, reference, message, synonyms, CurrentUser);
				FeedbackLabel1.SetSuccessMessage("Category updated successfully");
			}
			catch (CategoryException categoryEx)
			{
				FeedbackLabel1.SetErrorMessage("the following errors occured:", categoryEx.Errors);
			}
			catch (Exception ex)
			{
				m_Logger.Fatal(string.Format("Error editing category with id: {0}", categoryId), ex);
				FeedbackLabel1.SetErrorMessage("An unhandled error occured", ex.ToString());
			}
		}

    	#endregion
    }

	[Serializable]
	public struct DeleteCategoryResponse
	{
		public int CategoryId { get; set; }
		public bool Success { get; set; }
		public string Message { get; set; }
		public string ErrorCode { get; set; }
	}

    [Serializable]
    public struct CategoryForJson
    {
		public int? CategoryId { get; set; }
        public string ExternalRef { get; set; }
		public string Name { get; set; }
		public string synonyms { get; set; }
		public string Message { get; set; }
		public int AssetCount { get; set; }
    }
}
