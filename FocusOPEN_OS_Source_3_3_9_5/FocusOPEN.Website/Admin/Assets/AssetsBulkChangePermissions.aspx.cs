using System;
using System.Collections.Generic;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin.Assets
{
	public partial class AssetsBulkChangePermissions : BaseAdminAssetActionPage
	{
		protected void ReassignButton_Click(object sender, EventArgs e)
		{
			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(AssetIdList);
			List<Asset> assetList = Asset.FindMany(finder);

			foreach (Asset asset in assetList)
			{
				// Update internal permissions
				asset.InternalUsers_HideFromUsers = (InternalUsersRestrictionsRadioButtonList.SelectedValue == "NotVisible");
				asset.InternalUsers_DownloadApprovalRequired = (InternalUsersRestrictionsRadioButtonList.SelectedValue == "ApprovalRequired");

				// Update external permissions
				asset.ExternalUsers_HideFromUsers = (ExternalUsersRestrictionsRadioButtonList.SelectedValue == "NotVisible");
				asset.ExternalUsers_DownloadApprovalRequired = (ExternalUsersRestrictionsRadioButtonList.SelectedValue == "ApprovalRequired");

				// Save changes back to DB
				Asset.Update(asset);

				// Update log
				AuditLogManager.LogAssetAction(asset, CurrentUser, AuditAssetAction.SavedAsset, "Permissions changed via bulk permissions change tool");
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.EditAsset, "Permissions changed via bulk permissions change tool");
			}

			const string returnUrl = "AssetList.aspx?PostAction=BulkAssetPermissionChangeSuccessful";
			Response.Redirect(returnUrl);
		}
	}
}