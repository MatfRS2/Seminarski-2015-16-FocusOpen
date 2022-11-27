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
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using System.Linq;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageMetadataValues : BaseAdminPage
	{
		#region Page Events

		protected void Page_Init(object sender, EventArgs e)
		{
			// Only show brand selector to super-admins when there is more than one brand available
			BrandSelector.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int brandId = WebUtils.GetIntRequestParam("BrandId", 0);
				BrandDropDownList1.SafeSelectValue(brandId);

				BindGrid();
			}
		}

		protected void BrandDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindGrid();
		}

		protected void MetadataGroupsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
			{
				MetadataInfo info = (MetadataInfo) e.Item.DataItem;

				HyperLink MetadataNameHyperLink = (HyperLink) e.Item.FindControl("MetadataNameHyperLink");
				HyperLink EditMetadataGroupHyperLink = (HyperLink)e.Item.FindControl("EditMetadataGroupHyperLink");

				MetadataNameHyperLink.Text = info.Name;
				EditMetadataGroupHyperLink.NavigateUrl = MetadataNameHyperLink.NavigateUrl = string.Format("ManageMetadataValuesDetail.aspx?BrandId={0}&GroupNumber={1}", info.BrandId, info.GroupNumber);
			}
		}


		#endregion

		#region Private Stuff

		private void BindGrid()
		{
			// Get the brand for which metadata is being edited
			int brandId = (BrandSelector.Visible) ? BrandDropDownList1.SelectedId : CurrentUser.PrimaryBrandId;
			Brand brand = BrandCache.Instance.GetById(brandId);

			// Initialize list to store our metadata
			List<MetadataInfo> metadataList = new List<MetadataInfo>();

		    var settings = BrandMetadataSettingManager.GetCustomMetadataSettings(brandId);
		    foreach (var setting in settings.Where(s=>s.UiControlType == (int)BrandMetadataUiControlType.Select))
		    {
                // Only add it to the list if this metadata field is available for cataloguing
                // Otherwise, we'll just skip it as it's of no use to us.
                if (setting.OnAssetForm)
                {
                    MetadataInfo info = new MetadataInfo { BrandId = brand.BrandId.GetValueOrDefault(), GroupNumber = setting.GroupNumber, Name = setting.FieldName };
                    metadataList.Add(info);
                }
		    }

			// Toggle control visibility based on results
			NoMetadataPanel.Visible = (metadataList.Count == 0);
			MetadataGroupsRepeater.Visible = (!NoMetadataPanel.Visible);
			
			// Only bind the grid if we have metadata to display
			if (MetadataGroupsRepeater.Visible)
			{
				// Sort the list by name
				metadataList.Sort((a, b) => a.Name.CompareTo(b.Name));

				// Bind the list to the repeater
				MetadataGroupsRepeater.DataSource = metadataList;
				MetadataGroupsRepeater.DataBind();
			}
		}

		private class MetadataInfo
		{
			public int BrandId { internal set; get; }
			public int GroupNumber { internal set; get; }
			public string Name { internal set; get; }
		}

		#endregion
	}
}