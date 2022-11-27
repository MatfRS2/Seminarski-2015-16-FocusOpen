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
using System.Linq;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class AssetsInSession : BaseAdminPage
	{
		#region Private Variables

		private List<Int32> m_UploadedAssetList = null;

		#endregion

		#region Accessors

		private IEnumerable<int> UploadedAssetIdList
		{
			get
			{
				if (m_UploadedAssetList == null)
				{
					User u = Data.User.Get(SessionInfo.Current.User.UserId);

                    m_UploadedAssetList = new List<int>();
                    m_UploadedAssetList.AddRange(u.GetCatalogueAssetIdList());

                    SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Clear();
                    SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.AddRange(m_UploadedAssetList);
				}

				return m_UploadedAssetList;
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				SetupUI();
			}
		}

		protected void AssetTypeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			BindAssetList();
		}

		protected void SessionAssetsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			AssetSelectionChanged();
		}

		protected void SelectAllLinkButton_Click(object sender, EventArgs e)
		{
			if (SessionAssetsListBox.Items.Count > 0)
			{
				foreach (ListItem li in SessionAssetsListBox.Items)
					li.Selected = true;

				AssetSelectionChanged();
			}
		}

		protected void DeleteAssetLinkButton_Click(object sender, EventArgs e)
		{
			int assetId = Convert.ToInt32(SessionAssetsListBox.SelectedValue);
			AssetManager.DeleteAsset(CurrentUser, assetId);

			SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Remove(assetId);
			SiteUtils.SaveUploadedAssetListToCurrentUser();

			m_UploadedAssetList = null;

			SetupUI();
		}

		protected void CatalogueSelectedAssetsButton_Click(object sender, EventArgs e)
		{
			try
			{
				// Get selected asset ID's
				List<Int32> assetIdList = (from ListItem li in SessionAssetsListBox.Items
				                           where li.Selected
				                           select Convert.ToInt32(li.Value)).ToList();

				// Get the template ID
				int templateId = NumericUtils.ParseInt32(AssetTemplateIdTextBox.Text, -1);

				// Validate assets and use the specified template if required
				// This only applies if we're using a template - not the first asset
				AssetCataloguer.CatalogueAssets(assetIdList, UseTemplateRadioButton.Checked, templateId, AssetTypeDropDownList1.SelectedId);

				// Create a session-based asset pager for the selected assets
				SessionInfo.Current.AdminSessionInfo.AssetPager = new AssetPager(assetIdList);

				// Using the first asset for cataloguing works differently.  We store a boolean
				// in the session so that we know to use the first asset ID in the catalogue as
				// the template, and then when that asset is edited, we run the merge operation.
				SessionInfo.Current.AdminSessionInfo.AssetPager.UseFirstAssetAsTemplate = FirstAssetRadioButton.Checked;

				// Redirect to the asset form for the first asset in the list
				string redirectUrl = string.Format("AssetForm.aspx?assetId={0}", SessionInfo.Current.AdminSessionInfo.AssetPager.FirstAssetId());
				Response.Redirect(redirectUrl, false);
			}
			catch (CatalogueAssetsException ceax)
			{
				FeedbackLabel1.SetErrorMessage(ceax.Message);
				return;
			}
		}

		#endregion

		#region Private Helper Methods

		private List<Asset> GetUploadedAssets()
		{
			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(UploadedAssetIdList);
			return Asset.FindMany(finder);
		}

		private void AssetSelectionChanged()
		{
			bool isSingleSelect = SessionAssetsListBox.GetSelectedIndices().Length == 1;

			if (isSingleSelect)
			{
				UpdateAssetControls();
				MultipleSelectionLabel.Visible = false;
				PreviewPanel.Visible = true;
			}
			else
			{
				MultipleSelectionLabel.Visible = true;
				PreviewPanel.Visible = false;
			}
		}

		private void UpdateAssetControls()
		{
			Asset asset = Asset.Get(Convert.ToInt32(SessionAssetsListBox.SelectedValue));
			AssetThumbnail1.Initialise(asset);
			AssetFilenameLabel.Text = asset.Filename;
		}

		private void BindAssetList()
		{
			// Get assets
			AssetFinder finder = new AssetFinder();
			finder.AssetIdList.Add(0);
			finder.AssetIdList.AddRange(UploadedAssetIdList);
			finder.AssetTypeId = AssetTypeDropDownList1.SelectedId;

			List<Asset> assetList = Asset.FindMany(finder);

            //Ensure that the items that have been added via the "template catalogue" are first in the list.
            List<Asset> tempAssetList = new List<Asset>();

            foreach (int Id in SessionInfo.Current.User.GetCatalogueAssetIdList())
            {
                var asset = assetList.Where(a => a.AssetId == Id).SingleOrDefault();

                if (asset != null)
                {
                    assetList.Remove(asset);
                    tempAssetList.Add(asset);
                }
            }

			// Parse into a new list
            var list = from a in tempAssetList
                       select new { a.AssetId, Text = a.AssetId + " (" + a.Filename + ")" };

			// Bind the listbox
			SessionAssetsListBox.DataSource = list;
			SessionAssetsListBox.DataTextField = "Text";
			SessionAssetsListBox.DataValueField = "AssetId";
			SessionAssetsListBox.DataBind();

			SelectFirstAsset();
		}

		private void SelectFirstAsset()
		{
			if (SessionAssetsListBox.Items.Count > 0)
			{
				SessionAssetsListBox.SelectedIndex = 0;
				UpdateAssetControls();
				NoPreviewLabel.Visible = false;
				PreviewPanel.Visible = true;
				SelectAllLinkButton.Enabled = true;
				CatalogueSelectedAssetsButton.Enabled = true;
			}
			else
			{
				NoPreviewLabel.Visible = true;
				PreviewPanel.Visible = false;
				SelectAllLinkButton.Enabled = false;
				CatalogueSelectedAssetsButton.Enabled = false;
			}

			MultipleSelectionLabel.Visible = false;
		}

		private void SetupUI()
		{
			var uploadedAssets = GetUploadedAssets();

			if (uploadedAssets.Count > 0)
			{
				NoAssetsPanel.Visible = false;
				FormPanel.Visible = true;

				int assetId = WebUtils.GetIntRequestParam("AssetId", 0);

				BindAssetList();

				if (assetId > 0)
				{
					ListItem li = SessionAssetsListBox.Items.FindByValue(assetId.ToString());
					if (li != null)
					{
						SessionAssetsListBox.SelectedIndex = -1;
						li.Selected = true;
						AssetSelectionChanged();
					}
				}
			}
			else
			{
				NoAssetsPanel.Visible = true;
				FormPanel.Visible = false;
			}

			Label BreadCrumbsLabel = (Label)SiteUtils.FindControlRecursive(Page.Master, "BreadCrumbsLabel");
			BreadCrumbsLabel.Text = string.Format("Manage Assets{0}Catalogue Assets: Step One", AdminNavigation.BreadcrumbsSpacer);
		}

		#endregion
	}
}