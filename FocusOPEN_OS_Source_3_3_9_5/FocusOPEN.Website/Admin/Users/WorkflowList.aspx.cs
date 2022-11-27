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
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class WorkflowList : BaseAdminPageableEntityListPage<WorkflowFinder, Workflow>
	{
		#region BaseAdminPageableEntityListPage Implementation

		protected override ISortableControl DataControl
		{
			get
			{
				return WorkflowsRepeater;
			}
		}

		protected override BaseSavedAdminSearch<WorkflowFinder> SavedSearch
		{
			get
			{
				return new BaseSavedAdminSearch<WorkflowFinder>();
			}
		}

		protected override WorkflowFinder GetFinderFromForm()
		{
			WorkflowFinder finder = new WorkflowFinder();
			
			// Restrict brand admins from seeing workflows in their brand only
			if (CurrentUser.UserRole == UserRole.BrandAdministrator)
			{
				finder.BrandId = CurrentUser.PrimaryBrandId;
			}
			else
			{
				if (BrandSelectorWrapper.Visible)
					finder.BrandId = BrandDropDownList.SelectedId;
			}

			finder.SortExpressions.AddRange(DataControl.GetSortExpressions());

			return finder;
		}

		protected override void RetrieveSearchAndBindGrid()
		{
			throw new NotImplementedException();
		}

		protected override EntityList<Workflow> GetData(WorkflowFinder finder, int page)
		{
			return Workflow.FindMany(finder, page, PageSize);
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// Only show brand selector for super-admins, and when there is more than one brand
				BrandSelectorWrapper.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);
			}
		}

		protected void WorkflowsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Workflow workflow = (Workflow) e.Item.DataItem;

					HyperLink WorkflowNameHyperLink = (HyperLink)e.Item.FindControl("WorkflowNameHyperLink");
					WorkflowNameHyperLink.Text = workflow.Name;

					Label BrandNameLabel = (Label) e.Item.FindControl("BrandNameLabel");
					BrandNameLabel.Text = workflow.BrandName;

					HyperLink WorkflowEditHyperLink = (HyperLink)e.Item.FindControl("WorkflowEditHyperLink");
					WorkflowEditHyperLink.NavigateUrl = string.Format("WorkflowForm.aspx?WorkflowId={0}", workflow.WorkflowId);

					LinkButton WorkflowDeleteLinkButton = (LinkButton) e.Item.FindControl("WorkflowDeleteLinkButton");
					WorkflowDeleteLinkButton.CommandArgument = workflow.WorkflowId.ToString();

					break;
			}
		}

		protected void WorkflowDeleteLinkButton_Command(object sender, CommandEventArgs e)
		{
			int worfkflowId = Convert.ToInt32(e.CommandArgument);

			Workflow workflow = Workflow.Get(worfkflowId);
			workflow.IsDeleted = true;
			Workflow.Update(workflow);

			PerformSearchAndBindGrid(CurrentPage);
			MessageLabel1.SetSuccessMessage("Workflow deleted successfully");
		}

		protected void BrandDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			PerformSearchAndBindGrid(1);
		}
	}
}