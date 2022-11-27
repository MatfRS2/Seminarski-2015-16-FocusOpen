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
using System.Configuration;
using System.Linq;
using System.Reflection;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class EntitySecurityManager
	{
		#region Asset Download Checks

		/// <summary>
		/// Checks if the specified user can download the specified asset
		/// </summary>
		public static bool CanUserDownloadAsset(User user, Asset asset)
		{
			// Unrestricted assets don't need further checks
			if (!IsAssetRestricted(user, asset))
				return true;

			// Can't authorise with a blank user
			if (user.IsNull)
				return false;

			// ... or blank asset
			if (asset.IsNull)
				return false;

			// Don't allow users to download unavailable assets
			// Super admins, BU admins and asset owners will still be able to download, as the
			// IsAssetRestricted() check above will return false for them.
			if (AssetManager.GetAssetStatusForUser(asset, user) != AssetStatus.Available)
				return false;

			OrderItemFinder finder = new OrderItemFinder {UserId = user.UserId.GetValueOrDefault(-1), AssetId = asset.AssetId.GetValueOrDefault(-1)};
			OrderItem orderItem = OrderItem.FindOne(finder);

			if (orderItem.IsNull)
				return false;

			switch (orderItem.OrderItemStatus)
			{
				case (OrderItemStatus.Preapproved):
				case (OrderItemStatus.Approved):
					return true;

				default:
					return false;
			}
		}

		/// <summary>
		/// Checks if the specified asset is restricted for the specified user (ie. does it require authorisation)
		/// </summary>
		/// <param name="user">User requesting the asset</param>
		/// <param name="asset">Asset being requested</param>
		/// <returns>True if restricted, otherwise False</returns>
		public static bool IsAssetRestricted(User user, Asset asset)
		{
			// Super admins can always access everything
			if (user.UserRole == UserRole.SuperAdministrator)
				return false;

			// The user who uploaded an asset can always access it
			if (asset.UploadedByUserId == user.UserId.GetValueOrDefault())
				return false;

			// For assets where download approval required is set, this will not apply to super-admins, original upload users or primary brand admins.
			// The first two cases are covered above.  Here, we're doing the third check, where brand admins for the primary brand are allowed to download.
			// In effect, brand admins can ALWAYS access all assets assigned to their brand [as the primary]
			if (user.UserRole == UserRole.BrandAdministrator && user.PrimaryBrandId == asset.BrandId)
				return false;

			// Should never apply as assets should be hidden from external users
			// but this is simply a secondary security check
			if (CheckAssetRestriction(asset, user, AssetRestrictionType.HideFromUsers))
				return true;

			// Assets that are available and don't have the download approval permission set are not restricted
			if (AssetManager.GetAssetStatusForUser(asset, user) == AssetStatus.Available && !CheckAssetRestriction(asset, user, AssetRestrictionType.DownloadApprovalRequired))
				return false;

			// Nothing passed, so restricted
			return true;
		}

		private static bool CheckAssetRestriction(Asset asset, User user, AssetRestrictionType art)
		{
			string prefix = user.IsEmployee ? "InternalUsers_" : "ExternalUsers_";

			PropertyInfo pi = asset.GetType().GetProperty(prefix + art);
			bool val = (bool)pi.GetValue(asset, null);

			return val;
		}

		#endregion

		#region View asset thumbnail and preview images

		public static bool CanUserViewAssetThumbnail(User user, Asset asset)
		{
			// Null user's can view thumbnails
			// These are going to requests from broadcast emails and contact sheets.
			if (user.IsNull)
				return true;

			return CanUserViewAssetPreviewOrThumbnail(user, asset);
		}

		public static bool CanUserViewAssetPreview(User user, Asset asset)
		{
			// Null users cannot view previews
			// They must be logged in to do this
			if (user.IsNull)
				return false;

			return CanUserViewAssetPreviewOrThumbnail(user, asset);
		}

		private static bool CanUserViewAssetPreviewOrThumbnail(User user, Asset asset)
		{
			if (AssetManager.GetAssetStatusForUser(asset, user) == AssetStatus.Available)
				return true;

			if (user.UserRole == UserRole.SuperAdministrator)
				return true;

			if (asset.UploadedByUserId == user.UserId.GetValueOrDefault())
				return true;

			if (user.UserRole == UserRole.BrandAdministrator && user.PrimaryBrandId == asset.BrandId)
				return true;

			return false;
		}

		#endregion

		/// <summary>
		/// Checks if [User] is allowed to manage [TargetUser]
		/// </summary>
		public static bool CanManageUser(User user, User targetUser)
		{
			// Superadmins can edit everyone
			if (user.UserRole == UserRole.SuperAdministrator)
				return true;

			// Can edit thyself...
			if (user.UserId.GetValueOrDefault(0) == targetUser.UserId.GetValueOrDefault(-1))
				return true;

			// User access permissions for brand administrators
			if (user.UserRole == UserRole.BrandAdministrator)
			{
				// Otherwise, can edit if in same brands and in a lower or equal role
				if (targetUser.UserRoleId <= user.UserRoleId && user.PrimaryBrandId.Equals(targetUser.PrimaryBrandId))
					return true;

				// BU Admins can edit users belonging to companies they created
				if (UserSecurityManager.GetCompanyByDomain(targetUser.Email).CreatedByUserId == user.UserId.GetValueOrDefault())
					return true;
			}

			// Can't edit anyone else
			// m_Logger.DebugFormat("No approval checks passed.  {0} cannot access {1}", user.FullName, targetUser.FullName);
			return false;
		}

		public static bool CanViewAssetInfo(User user, Asset asset)
		{
			// Empty user can't view assets
			if (user.IsNull)
				return false;

			// Empty assets can't be viewed
			if (asset.IsNull)
				return false;

			// Super Admins can always view all assets
			if (user.UserRole == UserRole.SuperAdministrator)
				return true;

			// Brand admins can always view assets in their same brand(s)
			if (user.UserRole == UserRole.BrandAdministrator && user.PrimaryBrandId == asset.BrandId)
				return true;

			// Upload users can always view their own assets
			if (asset.UploadedByUserId == user.UserId.GetValueOrDefault())
				return true;

			// Non-published assets can't be viewed
			if (asset.AssetPublishStatus != AssetPublishStatus.Published)
				return false;

			// Assets not yet published or expired can't be viewed
			if (asset.PublishDate > DateTime.Now || asset.ExpiryDate < DateTime.Now)
				return false;

			// Otherwise, the asset can only be viewed by users with access to the asset's brand
			if (user.CanAccessBrand(asset.BrandId))
				return true;

			// Everything else can't be viewed
			return false;
		}

        public static bool CanViewAssetOrderHistory(User user, Asset asset)
        {
            // Empty user can't view assets
            if (user.IsNull)
                return false;

            // Empty assets can't be viewed
            if (asset.IsNull)
                return false;

            // Get the control setting from app settings
			string controlSetting = ConfigurationManager.AppSettings.GetValue("OrderHistoryAccess");

			// No control setting specified, so all users can access
			if (string.IsNullOrEmpty(controlSetting))
				return true;

			// Control setting is set to none, so no users can access
            if (controlSetting.ToLower() == "none")
                return false;

			// Super Admins can always view unless the controlSetting is NONE (which has already been tested above)
			if (user.UserRole == UserRole.SuperAdministrator)
				return true;

			// Otherwise, we have a user role, so check the access based on that
			// First convert the string to a user role enum so we can work with it.
        	UserRole configuredRole = EnumUtils.GetEnumFromValue<UserRole>(controlSetting);

            if (configuredRole == UserRole.BrandAdministrator)
            {
                // Brand admins can always view assets in their same brand(s)
            	return (user.UserRole == UserRole.BrandAdministrator && user.PrimaryBrandId == asset.BrandId);
            }

            if (configuredRole == UserRole.UploadUser)
            {
                // Upload users can always view their own assets
            	return (asset.UploadedByUserId == user.UserId.GetValueOrDefault());
            }

            // Everything else can't be viewed
            return false;
        }

		/// <summary>
		/// Checks if [User] is allowed to manage [Asset]
		/// </summary>
		public static bool CanManageAsset(User user, Asset asset)
		{
			// Empty users can't manage assets
			if (user.IsNull)
				return false;

			// Non admin users can never manage assets
			if (user.UserRole == UserRole.Normal)
				return false;

			// Super admins can always manage all assets
			if (user.UserRole == UserRole.SuperAdministrator)
				return true;

			// Users can always manage their own assets
			if (asset.UploadedByUserId == user.UserId.GetValueOrDefault())
				return true;

			// Brand admins can manage assets in their BU
			if (user.UserRole == UserRole.BrandAdministrator && user.PrimaryBrandId == asset.BrandId)
				return true;

			// Everyone else can't manage assets
			return false;
		}

		/// <summary>
		/// Checks if the specified user can manage the specified lightbox
		/// </summary>
		public static bool CanManageLightbox(User user, Lightbox lightbox)
		{
			// Super admins can manage any lightbox
			if (user.UserRole == UserRole.SuperAdministrator)
				return true;

            //if lightbox is linked then can edit if editable attribute set 
            if (lightbox.IsLinked && lightbox.IsEditable)
                return true;

			// Otherwise, lightbox can only be managed by its owner
			if (lightbox.UserId == user.UserId.GetValueOrDefault())
				return true;

			return false;
		}

		/// <summary>
		/// Checks if the specified user can manage the specified assetworkflow
		/// (Either as a user or a commenter)
		/// </summary>
		public static bool CanParticipateInAssetWorkflow(User user, AssetWorkflow assetWorkflow)
		{
			// User can participiate in workflow
			if (assetWorkflow.AssetWorkflowUserList.Any(au => au.UserId == user.UserId.GetValueOrDefault()))
				return true;

			// Commenter can participate in workflow
			if (assetWorkflow.AssetWorkflowCommenterList.Any(ac => ac.UserId == user.UserId.GetValueOrDefault()))
				return true;

			// No-one else can participate in workflow
			// Must be a user or invited commenter.
			return false;
		}
	}
}