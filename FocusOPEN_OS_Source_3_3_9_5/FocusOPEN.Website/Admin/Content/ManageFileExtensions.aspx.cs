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
using System.Diagnostics;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin.Content
{
	public partial class ManageFileExtensions : BaseAdminPageableEntityListPage<AssetTypeFileExtensionFinder, AssetTypeFileExtension>
	{
		#region BaseAdminAdvancedPageablePage Implementation

		protected override BaseSavedAdminSearch<AssetTypeFileExtensionFinder> SavedSearch
		{
			get
			{
				return new BaseSavedAdminSearch<AssetTypeFileExtensionFinder>();
			}
		}

		protected override ISortableControl DataControl
		{
			get
			{
				return AssetTypeFileExtensionsRepeater;
			}
		}

		protected override EntityList<AssetTypeFileExtension> GetData(AssetTypeFileExtensionFinder finder, int page)
		{
			CookieManager.SetValue("AdminSavedAssetTypeFileExtensionSearchPageSize", PageSize);
			Debug.WriteLine(finder.FindQuery);
			return AssetTypeFileExtension.FindMany(finder, page, PageSize);
		}

		protected override AssetTypeFileExtensionFinder GetFinderFromForm()
		{
			AssetTypeFileExtensionFinder finder = new AssetTypeFileExtensionFinder();
			finder.AssetTypeId = AssetTypeDropDownList1.SelectedId;
			finder.SortExpressions.AddRange(AssetTypeFileExtensionsRepeater.GetSortExpressions());
			return (finder);
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			AssetTypeFileExtensionFinder finder = SavedSearch.Finder;
			AssetTypeDropDownList1.SafeSelectValue(finder.AssetTypeId);

			DataControl.SortExpression = SavedSearch.SortExpression;
			DataControl.SortAscending = SavedSearch.SortAscending;

			int page = SavedSearch.Page;
			PerformSearchAndBindGrid(finder, page);
		}

		#endregion

		protected void AssetTypeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			PerformSearchAndBindGrid(1);
		}

		protected void AssetTypeFileExtensionsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
			{
				AssetTypeFileExtension o = (AssetTypeFileExtension) e.Item.DataItem;

				Image IconImage = (Image) e.Item.FindControl("IconImage");
				IconImage.ImageUrl = SiteUtils.GetFileTypeImageUrl(o.Extension);

				Literal APSSupportedLiteral = (Literal) e.Item.FindControl("APSSupportedLiteral");
				APSSupportedLiteral.Text = APSGateway.Instance.CanProcess(o.Extension) ? "Y" : "N";
			}
		}
	}
}