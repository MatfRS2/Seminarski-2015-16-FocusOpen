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
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin.Assets
{
	public partial class AssetsBulkDelegation : BaseAdminAssetActionPage
	{
		protected void DelegateButton_Click(object sender, EventArgs e)
		{
			CheckAssetList();

			string email = EmailTextBox.Text;

			if (StringUtils.IsBlank(email))
			{
				MessageLabel1.SetErrorMessage("Email address is required");
				return;
			}

			User user = Data.User.GetByEmail(email);

			if (user.IsNull)
			{
				MessageLabel1.SetErrorMessage("Email address does not belong to a registered user");
				return;
			}

			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(AssetIdList);
			List<Asset> assetList = Asset.FindMany(finder);

			foreach (Asset asset in assetList)
			{
				asset.UploadedByUserId = user.UserId.GetValueOrDefault();
				asset.ContactEmail = user.Email;
				Asset.Update(asset);
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.EditAsset, "Delegated asset to " + asset.ContactEmail);
			}

			Response.Redirect("AssetList.aspx?PostAction=BulkAssetDelegationSuccessful");
		}
	}
}