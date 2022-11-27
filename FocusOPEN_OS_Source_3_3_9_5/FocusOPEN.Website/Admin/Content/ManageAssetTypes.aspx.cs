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
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageAssetTypes : BaseAdminPageableLookupPage
	{
		#region BaseAdminPageableLookupPage Implementation

		protected override SortableDataGrid Grid
		{
			get
			{
				return EntityDataGrid;
			}
		}

		protected override void BindGrid(int page)
		{
			AssetTypeFinder finder = new AssetTypeFinder();
			finder.SortExpressions.AddRange(Grid.GetSortExpressions());
			EntityList<AssetType> data = AssetType.FindMany(finder, page - 1, PageSize);

			Grid.DataSource = data;
			Grid.DataBind();

			Grid.Visible = (data.Count > 0);
			NoResultsPanel.Visible = (data.Count == 0);

			UpdatePagingControls(data.PagingInfo);
		}

		protected override void Grid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):
					{
						AssetType entity = (AssetType) e.Item.DataItem;

						Label NameLabel = (Label)e.Item.FindControl("NameLabel");
						NameLabel.Text = entity.Name.Trim();

						PlaceHolder FileExtensionsPlaceHolder = (PlaceHolder)e.Item.FindControl("FileExtensionsPlaceHolder");

						if (StringUtils.IgnoreCaseCompare(entity.Name, "generic"))
						{
							Label lbl = new Label {CssClass = "BodyTxt"};
							lbl.Attributes["style"] = "color:#666";
							lbl.Text = "Generic asset type cannot have any file extensions";
							FileExtensionsPlaceHolder.Controls.Add(lbl);
						}
						else
						{
							foreach (var atfe in entity.AssetTypeFileExtensionList)
							{
								HyperLink link = new HyperLink { Text = atfe.Extension, NavigateUrl = string.Format("ManageFileExtensionsForm.aspx?AssetTypeFileExtensionId={0}", atfe.AssetTypeFileExtensionId), CssClass = "BodyTxt Black" };
								link.Attributes["style"] = "margin-right:10px;";
								FileExtensionsPlaceHolder.Controls.Add(link);
							}

							HyperLink addLink = new HyperLink {Text = "[add new]", NavigateUrl = "ManageFileExtensionsForm.aspx?AssetTypeId=" + entity.AssetTypeId, CssClass = "BodyTxt Black Bold"};
							FileExtensionsPlaceHolder.Controls.Add(addLink);
						}

						Label IsVisibleLabel = (Label) e.Item.FindControl("IsVisibleLabel");
						IsVisibleLabel.Text = entity.IsVisible ? "Y" : "N";

						LinkButton DeleteLb = (LinkButton)e.Item.Cells[e.Item.Cells.Count - 1].FindControl("DeleteLb");
						DeleteLb.CommandArgument = entity.AssetTypeId.GetValueOrDefault().ToString();
					}
					break;

				case ListItemType.EditItem:
					{
						AssetType entity = (AssetType)e.Item.DataItem;

						TextBox NameTextBox = (TextBox)e.Item.FindControl("NameTextBox");
						NameTextBox.Text = entity.Name.Trim();

						PlaceHolder FileExtensionsPlaceHolder = (PlaceHolder)e.Item.FindControl("FileExtensionsPlaceHolder");

						foreach (string extension in entity.FileExtensionList)
						{
							Label lbl = new Label { Text = extension, CssClass = "BodyTxt" };
							lbl.Attributes["style"] = "margin-right:10px;color:#fff";
							FileExtensionsPlaceHolder.Controls.Add(lbl);
						}

						CheckBox IsVisibleCheckBox = (CheckBox)e.Item.FindControl("IsVisibleCheckBox");
						IsVisibleCheckBox.Checked = entity.IsVisible;

						LinkButton lb = (LinkButton)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0];
						lb.CommandArgument = entity.AssetTypeId.ToString();
					}
					break;
			}
		}

		protected override void Grid_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			int assetTypeId = Convert.ToInt32(e.CommandArgument);
			TextBox NameTextBox = (TextBox)e.Item.FindControl("NameTextBox");
			CheckBox IsVisibleCheckBox = (CheckBox)e.Item.FindControl("IsVisibleCheckBox");

			try
			{
				AssetType assetType = AssetType.Get(assetTypeId);
				assetType.Name = NameTextBox.Text.Trim();
				assetType.IsVisible = IsVisibleCheckBox.Checked;
				AssetTypeManager.SaveAssetType(assetType);

				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ModifyAssetType, string.Format("Modified Asset Type: {0}, AssetTypeId: {1}", assetType.Name, assetType.AssetTypeId));

				ResetGridState();
				MessageLabel1.SetSuccessMessage("Asset type updated successfully");
			}
			catch (InvalidAssetTypeException imtex)
			{
				MessageLabel1.SetErrorMessage("Unable to update asset type", imtex.Message);
				BindGrid(CurrentPage);
			}
		}

		protected override void Grid_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			try
			{
				int assetTypeId = Convert.ToInt32(e.CommandArgument);
				AssetType assetType = AssetTypeCache.Instance.GetById(assetTypeId);

				AssetTypeManager.DeleteAssetType(assetTypeId);
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.DeleteAssetType, string.Format("Deleted Asset Type: {0}, AssetTypeId: {1}", assetType.Name, assetTypeId));

				MessageLabel1.SetSuccessMessage("Asset type deleted successfully");
			}
			catch (SystemException sysex)
			{
				MessageLabel1.SetErrorMessage(sysex.Message);
			}

			BindGrid(CurrentPage);
		}

		protected override void Grid_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "add")
			{
				TextBox NewNameTextBox = (TextBox)e.Item.FindControl("NewNameTextBox");
				CheckBox NewIsVisibleCheckBox = (CheckBox)e.Item.FindControl("NewIsVisibleCheckBox");

				try
				{
					AssetType assetType = AssetType.New();
					assetType.Name = NewNameTextBox.Text.Trim();
					assetType.IsVisible = NewIsVisibleCheckBox.Checked;
					AssetTypeManager.SaveAssetType(assetType);

					AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.AddAssetType, string.Format("Added Asset Type: {0}, AssetTypeId: {1}", assetType.Name, assetType.AssetTypeId));

					ResetGridState();
					MessageLabel1.SetSuccessMessage("Asset type added successfully");
				}
				catch (InvalidAssetTypeException imtex)
				{
					MessageLabel1.SetErrorMessage("Unable to add asset type", imtex.Errors);
					BindGrid(CurrentPage);
				}
			}
		}

		#endregion
	}
}