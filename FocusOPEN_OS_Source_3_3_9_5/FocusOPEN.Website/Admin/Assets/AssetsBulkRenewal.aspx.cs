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
	public partial class AssetsBulkRenewal : BaseAdminAssetActionPage
	{
		protected void RenewButton_Click(object sender, EventArgs e)
		{
			CheckAssetList();

			int months = Convert.ToInt32(ExtendByMonthsDropDownList.SelectedValue);

			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(AssetIdList);
			List<Asset> assetList = Asset.FindMany(finder);

			foreach (Asset asset in assetList)
			{
				asset.ExpiryDate = asset.ExpiryDate.AddMonths(months);
				Asset.Update(asset);
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.EditAsset, "Updated expiry date to " + asset.ExpiryDate.ToString(Global.DateFormat));
			}

			Response.Redirect("AssetList.aspx?PostAction=BulkAssetRenewalSuccessful");
		}
	}
}