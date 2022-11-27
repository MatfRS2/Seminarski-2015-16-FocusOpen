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
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageBrands : BaseAdminPageableEntityListPage<BrandFinder, Brand>
	{
		#region BaseAdminPageableEntityListPage Implementation

		protected override ISortableControl DataControl
		{
			get
			{
				return BrandsRepeater;
			}
		}

		protected override BaseSavedAdminSearch<BrandFinder> SavedSearch
		{
			get
			{
				return new BaseSavedAdminSearch<BrandFinder>();
			}
		}

		protected override BrandFinder GetFinderFromForm()
		{
			BrandFinder finder = new BrandFinder();
			finder.SortExpressions.AddRange(DataControl.GetSortExpressions());

			return finder;
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			throw new NotImplementedException();
		}

		protected override EntityList<Brand> GetData(BrandFinder finder, int page)
		{
			return Brand.FindMany(finder, page, PageSize);
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			ChangeMasterBrandButton.Attributes["onClick"] = "return confirm('Are you sure you want to change the master brand?')";

			if (!Page.IsPostBack)
				MasterBrandDropDownList.SafeSelectValue(WebsiteBrandManager.GetMasterBrand().BrandId);
		}

		protected void BrandsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Brand brand = (Brand)e.Item.DataItem;

					string editUrl = string.Format("ManageBrandsForm.aspx?BrandId={0}", brand.BrandId);

					HyperLink BrandNameHyperLink = (HyperLink)e.Item.FindControl("BrandNameHyperLink");
					BrandNameHyperLink.Text = brand.Name;
					BrandNameHyperLink.NavigateUrl = editUrl;

					Label IsMasterLabel = (Label) e.Item.FindControl("IsMasterLabel");
					IsMasterLabel.Visible = (brand.IsMasterBrand);

					HyperLink BrandEditHyperLink = (HyperLink)e.Item.FindControl("BrandEditHyperLink");
					BrandEditHyperLink.NavigateUrl = editUrl;

					break;
			}
		}

		protected void OpenChangeMasterBrandLinkButton_Click(object sender, EventArgs e)
		{
			ChangeMasterBrandWrapper.Visible = !ChangeMasterBrandWrapper.Visible;
		}

		protected void ChangeMasterBrandButton_Click(object sender, EventArgs e)
		{
			BrandManager.ChangeMasterBrand(MasterBrandDropDownList.SelectedId);
			ChangeMasterBrandWrapper.Visible = false;
			PerformSearchAndBindGrid(1);
		}
	}
}