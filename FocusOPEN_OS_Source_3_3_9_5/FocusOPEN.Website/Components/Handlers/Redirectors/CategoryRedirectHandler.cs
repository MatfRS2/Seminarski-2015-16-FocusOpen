/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Linq;
using System.Web.SessionState;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class CategoryRedirectHandler : BaseHandler, IRequiresSessionState
	{
		public override void ProcessRequest()
		{
			// Ensure user is logged in
			if (SessionInfo.Current.User.IsNull)
				Context.Response.Redirect("~/Login.aspx");

			// Get the category
			int categoryId = WebUtils.GetIntRequestParam("CategoryId", 0);

			// Ensure category was specified
			if (categoryId <= 0)
				Context.Response.Redirect("~/Default.aspx");

			// Get the category
			Category category = CategoryCache.Instance.GetById(categoryId);

			// Ensure category was found
			if (category.IsNull)
				Context.Response.Redirect("~/Default.aspx");

			// Ensure user can access the category brand
			if (!SessionInfo.Current.User.CanAccessBrand(category.BrandId))
				Context.Response.Redirect("~/Default.aspx");

			// Setup session with search
			SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.CurrentCategoryId = categoryId;
			SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.AssetFinder = SearchManager.GetBaseAssetFinder(SessionInfo.Current.User);
			SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.AssetFinder.BrandId = category.BrandId;
			SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.AssetFinder.CategoryIdList.AddRange(CategoryManager.GetCategoryIdsForSearch(categoryId));
			
			// Redirect
			Context.Response.Redirect("~/SearchResults.aspx");
		}
	}
}