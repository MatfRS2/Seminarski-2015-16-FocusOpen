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
using System.Linq;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageCompanies : BaseAdminPageableLookupPage
	{
		#region BaseAdminPageablePage Implementation

		protected override SortableDataGrid Grid
		{
			get
			{
				return EntityDataGrid;
			}
		}

		protected override void BindGrid(int page)
		{
			CompanyFinder finder = new CompanyFinder();
			finder.SortExpressions.AddRange(Grid.GetSortExpressions());
			EntityList<Company> data = Company.FindMany(finder, page - 1, PageSize);

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

					Company entity = (Company)e.Item.DataItem;

					Label NameLabel = (Label)e.Item.FindControl("NameLabel");
					NameLabel.Text = entity.Name;

					Label BrandsLabel = (Label) e.Item.FindControl("BrandsLabel");
					BrandsLabel.Text = entity.Brands;

					Label DomainLabel = (Label) e.Item.FindControl("DomainLabel");
					DomainLabel.Text = entity.Domain;

					Label InternalLabel = (Label) e.Item.FindControl("InternalLabel");
					InternalLabel.Text = (entity.IsInternal) ? "Y" : "N";

					if (EntityDataGrid.AllowDelete)
					{
						LinkButton DeleteLb = (LinkButton) e.Item.Cells[e.Item.Cells.Count - 1].FindControl("DeleteLb");
						DeleteLb.CommandArgument = entity.CompanyId.ToString();
					}

					break;

				case ListItemType.EditItem:

					entity = (Company)e.Item.DataItem;

					TextBox NameTextBox = (TextBox)e.Item.FindControl("NameTextBox");
					NameTextBox.Text = entity.Name;

					BrandCheckBoxList BrandsCheckBoxList = (BrandCheckBoxList) e.Item.FindControl("BrandsCheckBoxList");
					entity.BrandList.ForEach(b => BrandsCheckBoxList.SelectItem(b.BrandId.GetValueOrDefault()));

					TextBox DomainTextBox = (TextBox) e.Item.FindControl("DomainTextBox");
					DomainTextBox.Text = entity.Domain;

					CheckBox InternalCheckBox = (CheckBox)e.Item.FindControl("InternalCheckBox");
					InternalCheckBox.Checked = entity.IsInternal;

					if (CurrentUser.UserRole == UserRole.BrandAdministrator)
					{
						InternalCheckBox.Visible = false;

						Label InternalCheckBoxLabel = (Label)e.Item.FindControl("InternalCheckBoxLabel");
						InternalCheckBoxLabel.Visible = true;
					}

					LinkButton lb = (LinkButton)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0];
					lb.CommandArgument = entity.CompanyId.ToString();

					break;
			}
		}

		protected override void Grid_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			int id = Convert.ToInt32(e.CommandArgument);
			TextBox NameTextBox = (TextBox)e.Item.FindControl("NameTextBox");
			BrandCheckBoxList BrandsCheckBoxList = (BrandCheckBoxList) e.Item.FindControl("BrandsCheckBoxList");
			TextBox DomainTextBox = (TextBox)e.Item.FindControl("DomainTextBox");
			CheckBox InternalCheckBox = (CheckBox) e.Item.FindControl("InternalCheckBox");

			try
			{
				Company company = Company.Get(id);
				
				company.Name = NameTextBox.Text;
				company.Domain = DomainTextBox.Text;
				company.IsInternal = InternalCheckBox.Checked;

				AddBrandsToCompany(company, BrandsCheckBoxList);
				
				CompanyManager.SaveCompany(company);

				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ModifyCompany, string.Format("Modified Approved Company: {0}, CompanyId: {1}", company.Name, company.CompanyId));
				
				ResetGridState();
				MessageLabel1.SetSuccessMessage("Company updated successfully");
			}
			catch (InvalidCompanyException iacex)
			{
				MessageLabel1.SetErrorMessage("Unable to save company", iacex.Message);
			}
		}

		protected override void Grid_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			int id = Convert.ToInt32(e.CommandArgument);

			try
			{
				CompanyManager.DeleteCompany(id);

				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.DeleteCompany, string.Format("Deleted Company with CompanyId: {0}", id));
				MessageLabel1.SetSuccessMessage("Company deleted successfully");

				BindGrid(CurrentPage);
			}
			catch (Exception ex)
			{
				MessageLabel1.SetErrorMessage("Error deleting company: " + ex.Message);
			}
		}

		protected override void Grid_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "add")
			{
				TextBox NameTextBox = (TextBox)e.Item.FindControl("NewNameTextBox");
				BrandCheckBoxList BrandsCheckBoxList = (BrandCheckBoxList)e.Item.FindControl("NewBrandsCheckBoxList");
				TextBox DomainTextBox = (TextBox) e.Item.FindControl("NewDomainTextBox");
				CheckBox InternalCheckBox = (CheckBox)e.Item.FindControl("NewInternalCheckBox");

				try
				{
					Company company = Company.New();
					
					company.Name = NameTextBox.Text;
					company.Domain = DomainTextBox.Text;
					company.IsInternal = InternalCheckBox.Checked;
					company.CreateDate = DateTime.Now;
					company.CreatedByUserId = CurrentUser.UserId.GetValueOrDefault();

					AddBrandsToCompany(company, BrandsCheckBoxList);

					CompanyManager.SaveCompany(company);

					AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.AddCompany, string.Format("Added Approved Company: {0}, CompanyId: {1}", company.Name, company.CompanyId));

					ResetGridState();
					MessageLabel1.SetSuccessMessage("Company added successfully");
				}
				catch (InvalidCompanyException iacex)
				{
					MessageLabel1.SetErrorMessage("Unable to add company", iacex.Errors);
				}
			}
		}

		private static void AddBrandsToCompany(Company ac, BrandCheckBoxList NewBrandsCheckBoxList)
		{
			// First clear the brands list
			ac.BrandList.Clear();

			// If there's only one brand, then add that
			if (BrandManager.IsSingleBrandMode)
			{
				ac.BrandList.Add(BrandCache.Instance.GetList().First());
				return;
			}

			// Otherwise, add the brands selected in the checkbox list
			NewBrandsCheckBoxList.SelectedItems.ForEach(brandId => ac.BrandList.Add(BrandCache.Instance.GetById(brandId)));
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			// Ensure that we have at least one company available
			CompanyManager.EnsureCompanyExists(CurrentUser);

			// Only show the brands column if there is more than one brand in the system
			EntityDataGrid.Columns[1].Visible = (BrandManager.IsMultipleBrandMode);
		}

		protected void EntityDataGrid_ItemCreated(object sender, DataGridItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Footer:

					if (CurrentUser.UserRole == UserRole.BrandAdministrator)
					{
						CheckBox NewInternalCheckBox = (CheckBox)e.Item.FindControl("NewInternalCheckBox");
						Label NewInternalCheckBoxLabel = (Label)e.Item.FindControl("NewInternalCheckBoxLabel");

						NewInternalCheckBox.Checked = false;
						NewInternalCheckBox.Visible = false;
						NewInternalCheckBoxLabel.Visible = true;
					}

					break;
			}
		}
	}
}