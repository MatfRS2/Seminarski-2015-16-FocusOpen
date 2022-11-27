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
using FocusOPEN.Data;
using Daydream.Data;
using FocusOPEN.Website.Controls.Admin;

namespace FocusOPEN.Website.Components
{
	public abstract class BaseAdminAssetActionPage : BaseAdminPage
	{
		protected List<Int32> AssetIdList
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "AssetIdList", new List<Int32>());
			}
			set
			{
				ViewState["AssetIdList"] = value;
			}
		}

		/// <summary>
		/// Checks that we have an asset list with at least one asset
		/// </summary>
		protected void CheckAssetList()
		{
			if (AssetIdList == null || AssetIdList.Count == 0)
			{
				Response.Redirect("AssetList.aspx", true);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Page.IsPostBack)
			{
				List<Int32> assetIdList = Session["BulkAction_AssetIdList"] as List<Int32>;

				if (assetIdList == null)
				{
					Response.Redirect("AssetList.aspx");
					return;
				}

				Session.Remove("BulkAction_AssetIdList");

				BindToList(assetIdList);
			}
		}

		protected void BindToList(List<int> assetIdList)
		{
			AssetIdList = assetIdList;

			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(assetIdList);
			EntityList<Asset> assetList = Asset.FindMany(finder);

			AssetList AssetList = (AssetList)SiteUtils.FindControlRecursive(Page, "AssetList1");

			AssetList.Repeater.DataSource = assetList;
			AssetList.Repeater.DataBind();
		}
	}
}