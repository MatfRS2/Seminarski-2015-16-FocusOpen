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
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;

namespace FocusOPEN.Website.Admin.Assets
{
	public partial class AssetsBulkChangeCategory : BaseAdminAssetActionPage
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				string s = (Session["BulkAction_DefaultCategoryId"] == null) ? "0" : Session["BulkAction_DefaultCategoryId"].ToString();
				bool delete = (Session["BulkAction_DeleteCategory"] == null) ? false : Convert.ToBoolean(Session["BulkAction_DeleteCategory"]);

				Session.Remove("BulkAction_DefaultCategoryId");
				Session.Remove("BulkAction_DeleteCategory");
				
				int defaultCategoryId = NumericUtils.ParseInt32(s, 0);

				if (defaultCategoryId > 0)
				{
					Category category = CategoryCache.Instance.GetById(defaultCategoryId);

					BrandDropDownList1.SafeSelectValue(category.BrandId);
					SourceCategoryDropDownList.RefreshFromBrandAndSelect(BrandDropDownList1.SelectedId, defaultCategoryId);
					TargetCategoryDropDownList.RefreshFromBrand(BrandDropDownList1.SelectedId);

					if (delete)
					{
						DeleteSourceCategoryCheckBox.Checked = true;
						CancelButton.RedirectUrl = ResolveUrl("~/Admin/Content/ManageCategories.aspx?BrandId=" + category.BrandId);
					}
				}
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			int sourceCategoryId = SourceCategoryDropDownList.SelectedId;

			if (sourceCategoryId > 0)
			{
				ListItem li = TargetCategoryDropDownList.Items.FindByValue(sourceCategoryId.ToString());

				if (li != null)
					TargetCategoryDropDownList.Items.Remove(li);
			}
		}

		protected void BrandDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int brandId = BrandDropDownList1.SelectedId;

			SourceCategoryDropDownList.RefreshFromBrand(brandId);
			TargetCategoryDropDownList.RefreshFromBrand(brandId);
		}

		protected void SourceCategoryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			AssetFinder finder = new AssetFinder();
			finder.CategoryIdList.Add(SourceCategoryDropDownList.SelectedId);
			List<Asset> assets = Asset.FindMany(finder);

			if (assets.Count == 0)
			{
				MessageLabel1.SetErrorMessage("The selected category does not have any assets");
				return;
			}

			var assetIdList = (from a in assets
			                   select a.AssetId.GetValueOrDefault()).ToList();

			BindToList(assetIdList);
		}

		protected void ReassignButton_Click(object sender, EventArgs e)
		{
			CheckAssetList();

			int sourceId = SourceCategoryDropDownList.SelectedId;
			int targetId = TargetCategoryDropDownList.SelectedId;

			Category sourceCategory = CategoryCache.Instance.GetById(sourceId);
			Category targetCategory = CategoryCache.Instance.GetById(targetId);

			ErrorList errors = new ErrorList();

			if (sourceId <= 0)
				errors.Add("Source category must be selected");

			if (targetId <= 0)
				errors.Add("Target category must be selected");

			if (sourceId > 0 && sourceId == targetId)
				errors.Add("Source and target categories cannot be the same");

			if (errors.Count > 0)
			{
				MessageLabel1.SetErrorMessage("Error reassigning assets", errors);
				return;
			}

			if (targetCategory.IsNull)
			{
				MessageLabel1.SetErrorMessage("Invalid target category selected");
				return;
			}

			if (DeleteSourceCategoryCheckBox.Checked)
			{
				if (sourceCategory.IsRootCategory)
				{
					MessageLabel1.SetErrorMessage("The source category cannot be deleted after reassignment as it is a root category");
					return;
				}

				if (sourceCategory.SubCategories.Count > 0)
				{
					MessageLabel1.SetErrorMessage("The source category cannot be deleted after reassignment as it contains subcategories");
					return;
				}
			}

			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(AssetIdList);
			List<Asset> assetList = Asset.FindMany(finder);

			foreach (Asset asset in assetList)
			{
				if (ClearExistingCategoriesCheckBox.Checked)
					asset.CategoryList.Clear();

				asset.CategoryList.RemoveAll(c => c.CategoryId == sourceId);
				asset.CategoryList.Add(targetCategory);

				if (asset.CategoryList.Count == 1)
					asset.PrimaryCategoryId = asset.CategoryList[0].CategoryId.GetValueOrDefault();
				
				Asset.Update(asset);
				Asset.SaveAssetMetadata(asset);

				AuditLogManager.LogAssetAction(asset, CurrentUser, AuditAssetAction.SavedAsset, string.Format("Assigned category '{0}' to asset to using bulk reassignment tool", targetCategory.Name));
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.EditAsset, string.Format("Bulk edited asset categories, assigned asset ID {0} to category '{1}'", asset.AssetId, targetCategory.Name));
			}

			string returnUrl = "AssetList.aspx?PostAction=BulkAssetCategoryUpdateSuccessful";

			if (DeleteSourceCategoryCheckBox.Checked)
			{
				try
				{
					CategoryManager.DeleteCategory(sourceId, SessionInfo.Current.User);
					returnUrl = ResolveUrl("~/Admin/Content/ManageCategories.aspx?BrandId=" + sourceCategory.BrandId + "&message=TransferComplete");
				}
				catch (Exception ex)
				{
					m_Logger.Warn("Error deleting category: " + ex.Message, ex);
				}
			}
			else
			{
				CategoryManager.UpdateCategoryFullAssetCount(sourceId);
			}
			
			CategoryManager.UpdateCategoryFullAssetCount(targetId);

			Response.Redirect(returnUrl);
		}
	}
}