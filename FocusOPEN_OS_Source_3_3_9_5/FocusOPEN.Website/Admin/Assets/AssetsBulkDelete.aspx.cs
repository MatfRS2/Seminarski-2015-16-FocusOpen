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
using FocusOPEN.Business;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin.Assets
{
	public partial class AssetsBulkDelete : BaseAdminAssetActionPage
	{
		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			CheckAssetList();

			string password = PasswordTextBox.Text.Trim();

			if (StringUtils.IsBlank(password))
			{
				FeedbackLabel1.SetErrorMessage("Please enter your password");
				return;
			}

			if (!CurrentUser.CheckPassword(password))
			{
				FeedbackLabel1.SetErrorMessage("Incorrect password. Please try again.");
				return;
			}

			foreach (int assetId in AssetIdList)
				AssetManager.DeleteAsset(CurrentUser, assetId);

			Response.Redirect("AssetList.aspx?PostAction=BulkAssetDeleteSuccessful");
		}
	}
}