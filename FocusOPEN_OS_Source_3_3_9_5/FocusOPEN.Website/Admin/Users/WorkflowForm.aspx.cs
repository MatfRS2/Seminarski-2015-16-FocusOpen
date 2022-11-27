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
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class WorkflowForm : BaseAdminPage
	{
		#region Accessors

		private Workflow Workflow
		{
			get
			{
				Workflow workflow = ViewState["Workflow"] as Workflow;

				if (workflow == null)
				{
					workflow = Workflow.New();
					ViewState["Workflow"] = workflow;
				}

				return workflow;
			}
			set
			{
				ViewState["Workflow"] = value;
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			int workflowId = WebUtils.GetIntRequestParam("WorkflowId", 0);

			string pageTitle = (workflowId == 0) ? "Add Workflow" : "Edit Workflow";
			SetPageTitle(pageTitle);

			// Only super-admins can select the brand, if there is more than one available
			WorkflowBrandSelector.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator && BrandCache.Instance.GetList().Count > 1);

			if (!Page.IsPostBack)
			{
				workflowId = WebUtils.GetIntRequestParam("WorkflowId", 0);

				if (workflowId > 0)
				{
					Workflow workflow = Workflow.Get(workflowId);

					if (!workflow.IsNull)
						Workflow = workflow;
				}

				PopulateUserListBox();

				if (Workflow.IsNew)
				{
					HeaderTextDiv.InnerText = "Add Workflow";

					bool displayWorkflowWrapper = (BrandManager.IsMultipleBrandMode);

					NewWorkflowInfoPlaceHolder1.Visible = displayWorkflowWrapper;
					NewWorkflowInfoPlaceHolder2.Visible = displayWorkflowWrapper;

					WorkflowBrandDropDownList.SafeSelectValue(CurrentUser.PrimaryBrandId);
				}
				else
				{
					HeaderTextDiv.InnerText = "Edit Workflow";
					ExistingWorkflowInfoPlaceHolder1.Visible = true;

					WorkflowBrandDropDownList.SafeSelectValue(Workflow.BrandId);
					WorkflowNameTextBox.Text = Workflow.Name;
					PopulateCurrentWorkflowUsers();
				}
			}
		}

		protected void MoveUpButton_Click(object sender, EventArgs e)
		{
			if (SelectedUsersListBox.SelectedIndex == -1)
			{
				MessageLabel1.SetErrorMessage("No user selected");
				return;
			}

			if (SelectedUsersListBox.SelectedIndex == 0)
			{
				MessageLabel1.SetErrorMessage("The first user in the list cannot be moved up");
				return;
			}

			SelectedUsersListBox.Items.Insert(SelectedUsersListBox.SelectedIndex + 1, SelectedUsersListBox.Items[SelectedUsersListBox.SelectedIndex - 1]);
			SelectedUsersListBox.Items.RemoveAt(SelectedUsersListBox.SelectedIndex - 1);
		}

		protected void MoveDownButton_Click(object sender, EventArgs e)
		{
			if (SelectedUsersListBox.SelectedIndex == -1)
			{
				MessageLabel1.SetErrorMessage("No user selected");
				return;
			}

			if (SelectedUsersListBox.SelectedIndex == SelectedUsersListBox.Items.Count-1)
			{
				MessageLabel1.SetErrorMessage("The last user in the list cannot be moved down");
				return;
			}

			SelectedUsersListBox.Items.Insert(SelectedUsersListBox.SelectedIndex, SelectedUsersListBox.Items[SelectedUsersListBox.SelectedIndex + 1]);
			SelectedUsersListBox.Items.RemoveAt(SelectedUsersListBox.SelectedIndex + 1);
		}

		protected void AddToWorkflowButton_Click(object sender, EventArgs e)
		{
			if (AllUsersListBox.SelectedIndex == -1)
			{
				MessageLabel1.SetErrorMessage("No user selected");
				return;
			}

			List<ListItem> selectedItems = new List<ListItem>();

			foreach (ListItem li in AllUsersListBox.Items)
			{
				if (li.Selected)
				{
					if (SelectedUsersListBox.Items.FindByValue(li.Value) != null)
					{
						MessageLabel1.SetErrorMessage("One or more selected users are already in workflow");
						return;
					}

					selectedItems.Add(li);
				}
			}

			foreach (ListItem li in selectedItems)
			{
				li.Selected = false;
				SelectedUsersListBox.Items.Insert(SelectedUsersListBox.Items.Count, li);
			}

			DeselectAllUsers();
		}

		protected void RemoveFromWorkflowButton_Click(object sender, EventArgs e)
		{
			if (SelectedUsersListBox.SelectedIndex == -1)
			{
				MessageLabel1.SetErrorMessage("No user selected");
				return;
			}

			while (SelectedUsersListBox.SelectedIndex > -1)
				SelectedUsersListBox.Items.RemoveAt(SelectedUsersListBox.SelectedIndex);

			DeselectAllUsers();
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			Workflow.BrandId = WorkflowBrandDropDownList.SelectedId;
			Workflow.Name = WorkflowNameTextBox.Text;

			Workflow.WorkflowUserList.Clear();

			int position = 0;

			foreach (ListItem li in SelectedUsersListBox.Items)
			{
				position++;

				int userId = Convert.ToInt32(li.Value);

				WorkflowUser wfu = WorkflowUser.New();
				wfu.UserId = userId;
				wfu.Position = position;
				wfu.DateAdded = DateTime.Now;

				Workflow.WorkflowUserList.Add(wfu);
			}

			try
			{
				WorkflowManager.SaveWorkflow(Workflow);
				MessageLabel1.SetSuccessMessage("Workflow saved successfully");

				CancelButton.Text = "Back to workflow list";
				CancelButton.Prompt = string.Empty;

				DeselectAllUsers();
			}
			catch (ValidationException vex)
			{
				MessageLabel1.SetErrorMessage("Error saving workflow", vex.Errors);
			}
		}

		protected void CreateWorkflowFromBrandButton_Click(object sender, EventArgs e)
		{
			if (BrandDropDownList.SelectedId <= 0)
			{
				MessageLabel1.SetErrorMessage("Please select a brand");
				return;
			}

			int brandId = BrandDropDownList.SelectedId;
			Brand brand = Brand.Get(brandId);

			if (brand.IsNull)
			{
				MessageLabel1.SetErrorMessage("Invalid brand");
				return;
			}

			// Remove old items
			SelectedUsersListBox.Items.Clear();

			// New workflow will have same name as brand
			WorkflowNameTextBox.Text = brand.Name;

			// First get superadmin
			User firstSuperAdmin = GetFirstUser(UserRole.SuperAdministrator, 0);
			
			// first BU Admin
			User firstBrandAdmin = GetFirstUser(UserRole.BrandAdministrator, brandId);

			// Add super admin to listbox
			if (!firstSuperAdmin.IsNull)
				AddUserToListBox(firstSuperAdmin, SelectedUsersListBox);

			// Add BU Admin to listbox
			if (!firstBrandAdmin.IsNull)
				AddUserToListBox(firstBrandAdmin, SelectedUsersListBox);

			// Deselect the brand
			BrandDropDownList.SelectedIndex = -1;

			// Show feedback
			MessageLabel1.SetSuccessMessage("Default workflow created for brand.  Add or remove users, or press 'Save' to finish.");
		}

		protected void WorkflowBrandDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			PopulateUserListBox();
		}

		#region Private Helper Methods

		private static User GetFirstUser(UserRole userRole, int brandId)
		{
			UserFinder finder = new UserFinder { UserRole = userRole };
			finder.SortExpressions.Add(new AscendingSort(Data.User.Columns.RegisterDate));
			finder.SortExpressions.Add(new AscendingSort(Data.User.Columns.UserId));

			if (userRole != UserRole.SuperAdministrator)
				finder.PrimaryBrandId = brandId;

			return Data.User.FindOne(finder);
		}

		private void DeselectAllUsers()
		{
			AllUsersListBox.SelectedIndex = -1;
			SelectedUsersListBox.SelectedIndex = -1;
		}

		private void PopulateUserListBox()
		{
			// Only get users who are between upload users or super admins
			// They must also belong to the selected brand
			UserFinder finder = new UserFinder
			                    	{
			                    		PrimaryBrandId = WorkflowBrandDropDownList.SelectedId,
										MinimumUserRoleId = Convert.ToInt32(UserRole.UploadUser),
										MaximumUserRoleId = Convert.ToInt32(UserRole.SuperAdministrator)
			                    	};

			finder.SortExpressions.Add(new AscendingSort(Data.User.Columns.LastName));
			finder.SortExpressions.Add(new AscendingSort(Data.User.Columns.FirstName));
			List<User> users = Data.User.FindMany(finder);

			AllUsersListBox.Items.Clear();

			foreach (User user in users)
				AddUserToListBox(user, AllUsersListBox);
		}

		private void PopulateCurrentWorkflowUsers()
		{
			SelectedUsersListBox.Items.Clear();

			foreach (WorkflowUser wu in Workflow.WorkflowUserList)
				AddUserToListBox(wu.User, SelectedUsersListBox);
		}

		private static void AddUserToListBox(User user, ListControl listBox)
		{
			string name = string.Format("{0}, {1} ({2})", user.LastName, user.FirstName, user.Email);
			ListItem listItem = new ListItem(name, user.UserId.ToString());
			
			listBox.Items.Add(listItem);
		}

		#endregion
	}
}