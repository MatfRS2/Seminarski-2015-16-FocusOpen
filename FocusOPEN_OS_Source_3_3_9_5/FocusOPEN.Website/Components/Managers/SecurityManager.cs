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
using System.IO;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Manages all page security.  Ensures that the accessing user has the
	/// correct permissions to access the page.  If it's an admin user, and
	/// belongs to an an UploadUser or BrandAdministrator role, we also
	/// check the querystring for certain keys, and then do a check against the
	/// request business object to ensure that the user has permission to access
	/// the business object they requested.
	/// </summary>
	public static class SecurityManager
	{
		/// <summary>
		/// Checks if the user is in a role that has access to the requested page
		/// </summary>
		public static bool UserHasAccess(User user, AdminSectionPage adminSectionPage)
		{
			if (user.IsNull)
				return false;

			if (adminSectionPage == null)
				return (user.UserRole == UserRole.SuperAdministrator);

			if (adminSectionPage.RestrictToEngineers && !user.IsEngineer)
				return false;

			return adminSectionPage.UserRoleList.Contains(user.UserRole);
		}

		/// <summary>
		/// Checks if the user has access to the requested path.
		/// This method is the 'big kahuna', and does various security checks.  It's main
		/// purpose is to ensure that the admin area pages are correctly restricted, and it
		/// ensures that upload users and brand admins cannot access entities to
		/// which their role does not have permission (ie. those outside their brand).
		/// </summary>
		/// <param name="user">The user requesting access</param>
		/// <param name="path">The path to which access is being requested</param>
		/// <returns>[True] if user can access path.  Otherwise [False].</returns>
		public static bool UserHasAccess(User user, Uri path)
		{
			// Turn the path into a relative one (eg. /AppVirtualDir/Admin/Default.aspx -> ~/Admin/Default.aspx)
			string relativePath = VirtualPathUtility.ToAppRelative(path.AbsolutePath).ToLower();

			// Always allow NeatUpload stuff through
			if (relativePath.StartsWith("~/neatupload/"))
				return true;

			// Login and registration pages open to all.  Admin homepage open to
			// any user belonging to any role greater than normal.
			switch (relativePath)
			{
				case "~/login.aspx":
				case "~/register.aspx":
				case "~/changepassword.aspx":
				case "~/reactivate.aspx":
				case "~/viewcontactsheet.aspx":
				case "~/popups/termsconditions.aspx":
				case "~/popups/privacypolicy.aspx":
					return true;

				case "~/admin/default.aspx":
					return (user.UserRoleId > Convert.ToInt32(UserRole.Normal));
			}

			// Allow access to non-existent pages so we can redirect to 404
			if (!File.Exists(HttpContext.Current.Server.MapPath(relativePath)))
				return true;

			// Everything from here on needs a user
			if (user.IsNull)
				return false;

			// Ensure that the asset popup can only be viewed by authorised users
			if (relativePath.StartsWith("~/popups/assetinfo.aspx"))
			{
				int assetId = GetQuerystringValue(path.Query, "assetId", -1);

				if (assetId != -1)
				{
					Asset asset = Asset.Get(assetId);

					if (asset.IsNull || !EntitySecurityManager.CanViewAssetInfo(user, asset))
						return false;

					HttpContext.Current.Items.Add("Asset", asset);
				}
			}

			if (relativePath.StartsWith("~/admin/"))
			{
				// Get the admin section page by the URL
				AdminSectionPage adminSectionPage = AdminNavigationManager.GetAdminSectionPageByUrl(relativePath);

				// Ensure that we found it in the admin page list
				if (adminSectionPage == null)
					throw new SystemException(string.Format("Unknown admin page: '{0}'. Please check AdminNavigation.Config", relativePath));

				// Ensure that the page being accessed is available to the user role.
				// No point continuing if their role is too weak
				if (!adminSectionPage.UserRoleList.Contains(user.UserRole))
					return false;

				// Do role specific processing, as some roles can access pages, but
				// only when editing certain entities - ie. a Brand Admin can
				// access the user pages, but only when managing users from their own
				// brand.  Here, we check the querystring for values, get the
				// matching entity, and then ensure that it's from the same BU as
				// the requesting user.

				if (user.UserRole == UserRole.SuperAdministrator)
					return true;

				if (user.UserRole == UserRole.Normal)
					throw new SecurityException("Access to admin area denied");

				if (GeneralUtils.ValueIsInList(user.UserRole, UserRole.BrandAdministrator, UserRole.UploadUser))
				{
					// Assume page is okay, as querystring might be blank
					bool ok = true;

					// Check for user ID, and if it exists, ensure the user
					// can be accessed by the requesting user
					int userId = GetQuerystringValue(path.Query, "userId", -1);
					if (userId != -1)
					{
						User u = User.Get(userId);
						if (user.IsNull || !EntitySecurityManager.CanManageUser(user, u))
						{
							ok = false;
						}
						else
						{
							HttpContext.Current.Items.Add("User", u);
						}
					}

					// Check for asset Id, and if it exists, ensure the
					// asset can be accessed by the requesting user
					if (ok)
					{
						int assetId = GetQuerystringValue(path.Query, "assetId", -1);
						if (assetId != -1)
						{
							Asset asset = Asset.Get(assetId);
							if (asset.IsNull || !EntitySecurityManager.CanManageAsset(user, asset))
							{
								ok = false;
							}
							else
							{
								HttpContext.Current.Items.Add("Asset", asset);
							}
						}
					}

					// Check for AssetWorkflowId, and if it exists, ensure the
					// AssetWorkflow can be accessed by the requesting user
					if (ok)
					{
						int assetWorkflowId = GetQuerystringValue(path.Query, "assetWorkflowId", -1);
						if (assetWorkflowId != -1)
						{
							AssetWorkflow assetWorkflow = AssetWorkflow.Get(assetWorkflowId);
							if (assetWorkflow.IsNull || !EntitySecurityManager.CanParticipateInAssetWorkflow(user, assetWorkflow))
							{
								ok = false;
							}
							else
							{
								HttpContext.Current.Items.Add("AssetWorkflow", assetWorkflow);
							}
						}
					}

					return ok;
				}

				throw new SecurityException("Unable to check permissions for user role: " + user.UserRole);
			}
			
			return true;
		}

		#region Private Helper Methods

		/// <summary>
		/// Parses a querystring for a numeric value with the specified key.
		/// If not found, the default value is returned.
		/// </summary>
		private static int GetQuerystringValue(string querystring, string key, int defaultValue)
		{
			string pattern = string.Format(@"[?|&]{0}=(?<key>\d+)", key);
			Match match = Regex.Match(querystring, pattern, RegexOptions.IgnoreCase);

			if (match.Success)
				return Convert.ToInt32(match.Groups["key"].Value);

			return defaultValue;
		}

		#endregion
	}
}