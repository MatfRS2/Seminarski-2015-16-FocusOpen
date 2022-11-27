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
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageAssetFilePaths : BaseAdminPage
	{
		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				BindList();
			}
		}

		protected void AssetFilePathsDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					AssetFilePath assetFilePath = (AssetFilePath)e.Item.DataItem;

					HiddenField AssetFilePathIdHiddenField = (HiddenField)e.Item.FindControl("AssetFilePathIdHiddenField");
					Label PathLabel = (Label)e.Item.FindControl("PathLabel");
					Label AssetCountLabel = (Label)e.Item.FindControl("AssetCountLabel");
					RadioButton IsDefaultRadioButton = (RadioButton)e.Item.FindControl("IsDefaultRadioButton");

					AssetFilePathIdHiddenField.Value = assetFilePath.AssetFilePathId.ToString();
					PathLabel.Text = assetFilePath.Path;
					AssetCountLabel.Text = assetFilePath.AssetCount.ToString();
					IsDefaultRadioButton.Checked = assetFilePath.IsDefault;

					IsDefaultRadioButton.Attributes.Add("onClick", "selectRadio(this)");

					break;
			}
		}

		protected void AssetFilePathsDataGrid_OnItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "add")
			{
				try
				{
					TextBox NewFilePathTextBox = (TextBox) e.Item.FindControl("NewFilePathTextBox");
					string path = NewFilePathTextBox.Text;

					AssetFilePathManager.CreateNew(path);
					AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.AddFilepath, string.Format("Added file path: {0}", path));

					MessageLabel1.SetSuccessMessage("Asset file path added successfully");

					BindList();
				}
				catch (InvalidAssetFilePathException iafpex)
				{
					ErrorList errors = iafpex.Errors;

					if (errors.Count == 1)
					{
						MessageLabel1.SetErrorMessage(errors[0].ToString());
					}
					else
					{
						MessageLabel1.SetErrorMessage("Unable to add new file path", errors);
					}
				}
			}
		}

		protected void UpdateDefaultButton_Click(object sender, EventArgs e)
		{
			foreach (DataGridItem dgi in AssetFilePathsDataGrid.Items)
			{
				HiddenField AssetFilePathIdHiddenField = (HiddenField)dgi.FindControl("AssetFilePathIdHiddenField");
				RadioButton IsDefaultRadioButton = (RadioButton)dgi.FindControl("IsDefaultRadioButton");

				if (IsDefaultRadioButton.Checked)
				{
					int assetFilePathId = NumericUtils.ParseInt32(AssetFilePathIdHiddenField.Value, 0);

					try
					{
						AssetFilePathManager.SetDefault(assetFilePathId);
						AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ChangeDefaultFilepath, string.Format("Changed default file path to AssetFilePathId: {0}", assetFilePathId));

						MessageLabel1.SetSuccessMessage("Default asset file path updated successfully");
					}
					catch (InvalidAssetFilePathException ifpx)
					{
						MessageLabel1.SetErrorMessage("Unable to change default path", ifpx.Errors);
					}

					break;
				}
			}
		}

		#endregion

		#region Private Methods

		private void BindList()
		{
			AssetFilePathsDataGrid.DataSource = AssetFilePathManager.GetList();
			AssetFilePathsDataGrid.DataBind();
		}

		#endregion
	}
}