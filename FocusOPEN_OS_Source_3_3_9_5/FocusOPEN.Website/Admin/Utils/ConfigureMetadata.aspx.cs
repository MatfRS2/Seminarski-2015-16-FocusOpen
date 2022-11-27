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
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin.Utils
{
	public partial class ConfigureMetadata : BaseAdminPageableEntityListPage<BrandFinder, Brand>
	{

        protected ConfigureMetadata()
		{
            PageId = "Utils.ConfigureMetadata";
		}
        
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

	    private string DestinationUrl
	    {
	        get
	        {
	            return String.IsNullOrEmpty(Request["redirect"]) || Request["redirect"] != "search-filters"
	                       ? "ConfigureMetadataForm.aspx"
                           : "ConfigureMetadataSearchFilters.aspx";
	        }
	    }

        protected string PageHeaderTitle
        {
            get
            {
                return String.IsNullOrEmpty(Request["redirect"]) || Request["redirect"] != "search-filters"
                           ? "Configure Metadata Fields"
                           : "Configure Metadata SearchFilters";
            }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            PageId = "Utils." + DestinationUrl.Replace(".aspx", "");
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
			{
				if (CurrentUser.UserRole.Equals(UserRole.BrandAdministrator))
					Response.Redirect(DestinationUrl + "?BrandId=" + CurrentUser.PrimaryBrandId);

				if (CurrentUser.Brands.Count == 1)
                    Response.Redirect(DestinationUrl + "?BrandId=" + CurrentUser.Brands[0].BrandId);
			}
		}

		protected void BrandsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Brand brand = (Brand)e.Item.DataItem;

                    string editUrl = string.Format(DestinationUrl + "?BrandId={0}", brand.BrandId);

					HyperLink BrandNameHyperLink = (HyperLink)e.Item.FindControl("BrandNameHyperLink");
					BrandNameHyperLink.Text = brand.Name;
					BrandNameHyperLink.NavigateUrl = editUrl;

					HyperLink BrandEditHyperLink = (HyperLink)e.Item.FindControl("BrandEditHyperLink");
					BrandEditHyperLink.NavigateUrl = editUrl;

					break;
			}
		}
	}
}