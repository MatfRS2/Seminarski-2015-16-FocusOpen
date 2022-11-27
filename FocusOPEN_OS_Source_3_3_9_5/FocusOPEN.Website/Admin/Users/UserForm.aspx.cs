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
using System.Reflection;
using System.Security;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;

namespace FocusOPEN.Website.Admin
{
	public partial class UserForm : BaseAdminPage
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Helper Methods

		/// <summary>
		/// Removes user roles that are above the role of the current user
		/// ie. To stop a brand admin creating a super admin
		/// </summary>
		private void RemoveUserRoles()
		{
			var userRoles = EnumUtils.GetList<UserRole>();

			UserRoleDropDownList.Items.Clear();

			foreach (var userRole in userRoles)
			{
				int userRoleId = Convert.ToInt32(userRole.Key);
				if (userRoleId <= CurrentUser.UserRoleId)
					UserRoleDropDownList.Items.Add(new ListItem(userRole.Value, userRole.Key.ToString()));
			}
		}

		/// <summary>
		/// Displays the user password expiry date.
		/// </summary>
		/// <param name="user">The user.</param>
		private void DisplayUserPasswordExpiryDate(User user)
		{
			DatePasswordExpiresLabel.Text = (user.IsPasswordNonExpiring) ? "Never" : user.GetPasswordExpiryDate().ToString(Global.DateTimeFormat);
			DatePasswordExpiresLabel.CssClass = (user.GetPasswordExpiryDate() < DateTime.Now) ? "FbkNegTxt" : "PanelTxt Bold";
			UpdatePasswordExpiryDateLinkButton.Visible = (!user.IsPasswordNonExpiring);
		}

		/// <summary>
		/// Displays the user account expiry date.
		/// </summary>
		/// <param name="user">The user.</param>
		private void DisplayUserAccountExpiryDate(User user)
		{
			DateAccountExpiresLabel.Text = (user.IsAccountNonExpiring) ? "Never" : user.GetAccountExpiryDate().ToString(Global.DateTimeFormat);
			DateAccountExpiresLabel.CssClass = (user.GetAccountExpiryDate() < DateTime.Now) ? "FbkNegTxt" : "PanelTxt Bold";
			UpdateAccountExpiryDateLinkButton.Visible = (!user.IsAccountNonExpiring);
		}

		/// <summary>
		/// Saves the selected brands to user.
		/// </summary>
		/// <param name="user">The user.</param>
		private void SaveSelectedBrandsToUser(User user)
		{
			if (user.IsNull)
				throw new SystemException("Brands cannot be added to a null user");

			user.Brands.Clear();
			user.PrimaryBrandId = 0;

			if (BrandSelectorRow.Visible)
			{
				foreach (RepeaterItem ri in BrandSelectorRepeater.Items)
				{
					switch (ri.ItemType)
					{
						case ListItemType.Item:
						case ListItemType.AlternatingItem:

							// Get the brand ID
							HiddenField BrandIdHiddenField = (HiddenField) ri.FindControl("BrandIdHiddenField");
							int brandId = NumericUtils.ParseInt32(BrandIdHiddenField.Value, 0);

							// Get the user interface controls
							CheckBox IsSelectedCheckBox = (CheckBox) ri.FindControl("IsSelectedCheckBox");
							RadioButton IsMainBrandRadioButton = (RadioButton) ri.FindControl("IsMainBrandRadioButton");

							// If the brand is selected or set as the primary, add it to the list
							if (IsSelectedCheckBox.Checked || IsMainBrandRadioButton.Checked)
								user.Brands.Add(BrandCache.Instance.GetById(brandId));

							// Set the primary brand
							if (IsMainBrandRadioButton.Checked)
								user.PrimaryBrandId = brandId;

							break;
					}
				}
			}
			else
			{
				// Brand selector is not visible, either because there's only one brand
				// or the user is a brand administrator.  Therefore, only add the current brand.

				Brand brand = WebsiteBrandManager.GetBrand();
				user.Brands.Add(brand);
				user.PrimaryBrandId = brand.BrandId.GetValueOrDefault();
			}
		}

		#endregion

		#region Accessors

		protected int UserId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "UserId", 0);
			}
			private set
			{
				ViewState["UserId"] = value;
			}
		}

		private User UserBeingEdited
		{
			get
			{
				User u = Context.Items["User"] as User;

				if (u == null || u.UserId.GetValueOrDefault(0) != UserId)
					u = Data.User.Get(UserId);

				return u ?? Data.User.Empty;
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Init(object sender, EventArgs e)
		{
			UpdatePasswordExpiryDateLinkButton.Text = string.Format("[Extend by {0} days]", UserManager.PasswordExpiryDays);
			UpdateAccountExpiryDateLinkButton.Text = string.Format("[Extend by {0} days]", UserManager.AccountExpiryDays);

			UserId = WebUtils.GetIntRequestParam("UserId", 0);

			RemoveUserRoles();

			List<Workflow> workflowList;

			switch (CurrentUser.UserRole)
			{
				case UserRole.SuperAdministrator:
					{
						IsStaffUserRow.Visible = true;
						AdvancedOptionsPanel.Visible = true;
						EmulateUserButton.Visible = true;

						// Only display the brand selector if there is more than one brand
						BrandSelectorRow.Visible = (BrandManager.IsMultipleBrandMode);

						WorkflowFinder finder = new WorkflowFinder();
						finder.SortExpressions.Add(new AscendingSort(Workflow.Columns.Name.ToString()));
						workflowList = Workflow.FindMany(finder);
					}
					break;

				case UserRole.BrandAdministrator:
					{
						IsStaffUserRow.Visible = false;
						IsStaffUserRadioButtonList.SelectedValue = (CurrentUser.IsEmployee) ? "1" : "0";

						AdvancedOptionsPanel.Visible = false;
						EmulateUserButton.Visible = false;
						BrandSelectorRow.Visible = false;

						WorkflowFinder finder = new WorkflowFinder {BrandId = CurrentUser.PrimaryBrandId};
						finder.SortExpressions.Add(new AscendingSort(Workflow.Columns.Name.ToString()));
						workflowList = Workflow.FindMany(finder);
					}
					break;

				default:
					throw new SecurityException("Access denied");
			}

			WorkflowDropDownList.DataSource = workflowList;
			WorkflowDropDownList.DataTextField = Workflow.Columns.Name.ToString();
			WorkflowDropDownList.DataValueField = Workflow.Columns.WorkflowId.ToString();
			WorkflowDropDownList.DataBind();

			WorkflowDropDownList.Items.Insert(0, new ListItem("Let user choose", "0"));

			BrandSelectorRepeater.DataSource = BrandCache.Instance.GetList();
			BrandSelectorRepeater.DataBind();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			MessageLabel1.Visible = false;

			if (!Page.IsPostBack)
			{
				if (UserBeingEdited.IsNull)
				{
					// This is a new user so we need to modify the UI accordingly
					// Hide panels and buttons that are not relevant

					IsStaffUserRadioButtonList.SelectedValue = (CurrentUser.IsEmployee) ? "1" : "0";
					RefreshCompanyDropDownList();

					if (CurrentUser.UserRole == UserRole.SuperAdministrator)
					{
						GeneratedPasswordPanel.Visible = false;
						EditPasswordPanel.Visible = true;
						NewUserPasswordTooltip.Visible = true;
						WelcomeEmailCheckBoxRow.Visible = true;
					}
					else
					{
						GeneratedPasswordPanel.Visible = true;
						EditPasswordPanel.Visible = false;
						WelcomeEmailCheckBoxRow.Visible = false;
					}

					DatesPanel.Visible = false;
					AccountExternalAccessAllowedCheckbox.Checked = true;
					ViewAuditTrailHyperLink.Visible = false;
					DeleteButton.Visible = false;
					EmulateUserButton.Visible = false;
					WorkflowDropDownList.SelectValue(0);
				}
				else
				{
					GeneratedPasswordPanel.Visible = false;
					EditPasswordPanel.Visible = true;
					NewUserPasswordTooltip.Visible = false;
					WelcomeEmailCheckBoxRow.Visible = false;

					FirstNameTextBox.Text = UserBeingEdited.FirstName;
					LastNameTextBox.Text = UserBeingEdited.LastName;
					EmailAddressTextBox.Text = UserBeingEdited.Email;
					CompanyNameTextBox.Text = UserBeingEdited.CompanyName;
					PhoneNumberTextBox.Text = UserBeingEdited.PhoneNumber;
					MobileNumberTextBox.Text = UserBeingEdited.MobileNumber;
					CountryDropDownList1.SafeSelectValue(UserBeingEdited.CountryId);

					UserStatusLabel.Text = EnumUtils.GetDescription(UserBeingEdited.UserStatus);
					DateRegisteredLabel.Text = UserBeingEdited.RegisterDate.ToString(Global.DateTimeFormat);
                    APITokenLabel.Text = UserBeingEdited.UserAPIToken;

					DateLastLoginLabel.Text = (UserBeingEdited.LastLoginDate.HasValue) ? UserBeingEdited.LastLoginDate.Value.ToString(Global.DateTimeFormat) : "Never";
					if (DateLastLoginLabel.Text.ToLower() == "never")
						DateLastLoginLabel.CssClass = "FbkNegTxt";

					DisplayUserPasswordExpiryDate(UserBeingEdited);
					DisplayUserAccountExpiryDate(UserBeingEdited);
					
					AccountSuspendedCheckbox.Checked = UserBeingEdited.IsSuspended;
					PasswordNonExpiringCheckBox.Checked = UserBeingEdited.IsPasswordNonExpiring;
					AccountNonExpiringCheckbox.Checked = UserBeingEdited.IsAccountNonExpiring;
					AccountExternalAccessAllowedCheckbox.Checked = UserBeingEdited.IsAllowedExternalAccess;
                    EnableFilePathIngestionCheckbox.Checked = UserBeingEdited.EnableFilePathIngestion;                

					UserRoleDropDownList.SelectedValue = UserBeingEdited.UserRoleId.ToString();
					ApproverUserDropDownList.SafeSelectValue(UserBeingEdited.ApproverUserId);
					UseWorkflowCheckbox.Checked = UserBeingEdited.UseWorkflow;
					WorkflowDropDownList.SelectValue(UserBeingEdited.WorkflowId);

					IsStaffUserRadioButtonList.SelectedValue = (UserBeingEdited.IsEmployee) ? "1" : "0";
					RefreshCompanyDropDownList();

					// Only show the audit trail link if the current user has access to the audit trail page
					SiteUtils.SetHyperLink(CurrentUser, "Reports.AuditTrail", string.Format("?email={0}", UserBeingEdited.Email), ViewAuditTrailHyperLink);
				}
			}
		}

		protected void UpdatePasswordExpiryDateLinkButton_Click(object sender, EventArgs e)
		{
			// Get the user
			int userId = GetRequiredQueryStringParameter("UserId", "UserList.aspx");
			User user = Data.User.Get(userId);

			// Get the expiry date
			DateTime expiryDate = user.GetPasswordExpiryDate();

			// Update it, if the user has an expiry date (ie. is non expiring)
			if (expiryDate < DateTime.MaxValue)
			{
				DateTime dt = (expiryDate < DateTime.Now) ? DateTime.Now : expiryDate;
				DateTime newExpiryDate = dt.AddDays(UserManager.PasswordExpiryDays);

				// Also approve user
				user.UserStatus = UserStatus.Approved;

				// Update user
				user.PasswordExpiryDate = newExpiryDate;
				Data.User.Update(user);

				// Update UI
				DisplayUserPasswordExpiryDate(user);
				MessageLabel1.SetSuccessMessage(string.Format("password expiry date updated to: {0}", newExpiryDate.ToString(Global.DateTimeFormat)));

				// Update audit log
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ModifyUser, string.Format("Updated {0}'s (Userid: {1}) password expiry date to: {2}", user.FullName, user.UserId, newExpiryDate.ToString("dd MMM yyyy HH:mm")));
			}
			else
			{
				MessageLabel1.SetErrorMessage("password never expires, expiry date not updated");
			}
		}

		protected void UpdateAccountExpiryDateLinkButton_Click(object sender, EventArgs e)
		{
			// Get the user
			int userId = GetRequiredQueryStringParameter("UserId", "UserList.aspx");
			User user = Data.User.Get(userId);

			// Get the expiry date
			DateTime expiryDate = user.GetAccountExpiryDate();

			// Update it, if the user has an expiry date (ie. is non expiring)
			if (expiryDate < DateTime.MaxValue)
			{
				DateTime dt = (expiryDate < DateTime.Now) ? DateTime.Now : expiryDate;
				DateTime newExpiryDate = dt.AddDays(UserManager.AccountExpiryDays);

				// Also approve user
				user.UserStatus = UserStatus.Approved;

				// Update user
				user.AccountExpiryDate = newExpiryDate;
				Data.User.Update(user);
				
				// Update UI
				DisplayUserAccountExpiryDate(user);
				MessageLabel1.SetSuccessMessage(string.Format("account expiry date updated to: {0}", newExpiryDate.ToString(Global.DateTimeFormat)));

				// Update audit log
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ModifyUser, string.Format("Updated {0}'s (Userid: {1}) account expiry date to: {2}", user.FullName, user.UserId, newExpiryDate.ToString("dd MMM yyyy HH:mm")));
			}
			else
			{
				MessageLabel1.SetErrorMessage("account never expires, expiry date not updated");
			}
		}

        protected void RegenerateTokenLinkButton_Click(object sender, EventArgs e)
		{
			// Get the user
			int userId = GetRequiredQueryStringParameter("UserId", "UserList.aspx");
            User user = Data.User.Get(userId);
            user.RegenerateUserAPIToken();
            APITokenLabel.Text = user.UserAPIToken;
        }

        

		protected void IsStaffUserRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshCompanyDropDownList();
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			// Get the user id
			int userId = WebUtils.GetIntRequestParam("UserId", 0);

			// Get the user
			User user = Data.User.Get(userId);

			if (user.IsNull)
			{
				// Get new user with default values set
				user = UserManager.GetNewUser();

				if (GeneratedPasswordPanel.Visible || (!NoWelcomeEmailCheckBox.Checked && StringUtils.IsBlank(PasswordTextBox.Text)))
				{
					// Set password
					string password = PasswordGenerator.GeneratePassword();
					user.SetPassword(password);
					user.SetConfirmPassword(password);
				}

				// Conditional settings for brand admins
				// (As they cannot set these values from the user interface)
				if (CurrentUser.UserRole == UserRole.BrandAdministrator)
				{
					user.IsEmployee = CurrentUser.IsEmployee;
					user.PrimaryBrandId = CurrentUser.PrimaryBrandId;
					user.IsAccountNonExpiring = false;
					user.IsPasswordNonExpiring = false;
					user.IsAllowedExternalAccess = false;
                    user.EnableFilePathIngestion = false;
                }
			}

			user.FirstName = FirstNameTextBox.Text.Trim();
			user.LastName = LastNameTextBox.Text.Trim();
			user.Email = EmailAddressTextBox.Text;
			user.CompanyName = CompanyNameTextBox.Text.Trim();
			user.CountryId = CountryDropDownList1.SelectedId;
			user.PhoneNumber = PhoneNumberTextBox.Text.Trim();
			user.MobileNumber = MobileNumberTextBox.Text.Trim();
			user.IsSuspended = AccountSuspendedCheckbox.Checked;
			user.UserRoleId = Convert.ToInt32(UserRoleDropDownList.SelectedValue);

			if (user.CountryId == 0)
				user.CountryId = null;

			user.ApproverUserId = null;

			if (ApproverUserDropDownList.SelectedId > 0)
				user.ApproverUserId = ApproverUserDropDownList.SelectedId;
			
			user.UseWorkflow = (user.UserRole == UserRole.UploadUser && UseWorkflowCheckbox.Checked);
			user.WorkflowId = null;

			if (user.UseWorkflow && WorkflowDropDownList.GetSelectedId() != 0)
				user.WorkflowId = WorkflowDropDownList.GetSelectedId();

			if (PasswordTextBox.Text.Trim() != string.Empty)
			{
				user.SetPassword(PasswordTextBox.Text);
				user.SetConfirmPassword(ConfirmPasswordTextBox.Text);
			}

			if (IsStaffUserRow.Visible)
				user.IsEmployee = (IsStaffUserRadioButtonList.SelectedValue == "1");

			SaveSelectedBrandsToUser(user);

			if (AdvancedOptionsPanel.Visible)
			{
				user.IsAccountNonExpiring = AccountNonExpiringCheckbox.Checked;
				user.IsPasswordNonExpiring = PasswordNonExpiringCheckBox.Checked;
				user.IsAllowedExternalAccess = AccountExternalAccessAllowedCheckbox.Checked;
                user.EnableFilePathIngestion = EnableFilePathIngestionCheckbox.Checked;
			}

			try
			{
				if (user.IsNew)
				{
					bool notify = true;

					if (WelcomeEmailCheckBoxRow.Visible && NoWelcomeEmailCheckBox.Checked)
						notify = false;

					UserManager.Create(user, CurrentUser, notify);
				}
				else
				{
					UserManager.Update(user);
					AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ModifyUser, string.Format("Modified user: {0}, with UserId: {1}", user.FullName, user.UserId));
				}

				UserId = user.UserId.GetValueOrDefault();
				FormPanel.Visible = false;
				SuccessPanel.Visible = true;
			}
			catch (InvalidUserException iuex)
			{
				MessageLabel1.SetErrorMessage("the following errors occured:", iuex.Errors);
			}
		}

		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			// Get the user id
			int userId = GetRequiredQueryStringParameter("UserId", "UserList.aspx");

			// Delete user
			UserManager.DeleteUser(CurrentUser, userId);

			// Redirect to the user list
			Response.Redirect("UserList.aspx", false);
		}

		protected void EmulateUserButton_Click(object sender, EventArgs e)
		{
			int userId = WebUtils.GetIntRequestParam("UserId", 0);

			User currentUser = CurrentUser;
			User emulatedUser = Data.User.Get(userId);

			if (emulatedUser.IsNull)
			{
				MessageLabel1.SetErrorMessage("User not found");
				return;
			}

            //renew session api token if user not currently active
            if (!UserManager.APISessionIsValid(emulatedUser))
            {
                UserManager.RenewSessionAPIToken(emulatedUser);
            }

			m_Logger.DebugFormat("User: {0} emulating: {1}, user id: {2}", CurrentUser.FullName, emulatedUser.FullName, emulatedUser.UserId);

			AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.UserLogin, string.Format("Emulating: {0}", emulatedUser.FullName));

			SessionInfo.Current.User = emulatedUser;
			SessionInfo.Current.UserSessionInfo.Reset();
			SessionInfo.Current.AdminSessionInfo.Reset();

			Login.SetupSessionForCurrentSessionUser();

			AuditLogManager.LogUserAction(emulatedUser, AuditUserAction.UserLogin, string.Format("User being emulated by {0}", currentUser.FullName));

			Response.Redirect("~/Default.aspx");
		}

		protected void BrandSelectorRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:

					Brand brand = (Brand)e.Item.DataItem;
					int brandId = brand.BrandId.GetValueOrDefault();

					HiddenField BrandIdHiddenField = (HiddenField)e.Item.FindControl("BrandIdHiddenField");
					BrandIdHiddenField.Value = brandId.ToString();

					Label BrandNameLabel = (Label)e.Item.FindControl("BrandNameLabel");
					BrandNameLabel.Text = brand.Name;

					bool isSelected;
					bool isMainBrand;

					if (UserBeingEdited.IsNull)
					{
						isSelected = WebsiteBrandManager.GetBrand().BrandId == brandId;
						isMainBrand = isSelected;
					}
					else
					{
						isSelected = (UserBeingEdited.PrimaryBrandId == brandId || UserBeingEdited.CanAccessBrand(brandId));
						isMainBrand = (UserBeingEdited.PrimaryBrandId == brandId);
					}

					CheckBox IsSelectedCheckBox = (CheckBox)e.Item.FindControl("IsSelectedCheckBox");
					IsSelectedCheckBox.Checked = isSelected;

					RadioButton IsMainBrandRadioButton = (RadioButton)e.Item.FindControl("IsMainBrandRadioButton");
					IsMainBrandRadioButton.Checked = isMainBrand;

					const string script = "setUniqueRadioButton('BrandSelectorRepeater.*IsMainBrand',this)";
					IsMainBrandRadioButton.Attributes.Add("onclick", script);

					break;
			}
		}

		protected void IsMainBrandRadioButton_CheckChanged(object sender, EventArgs e)
		{
			foreach (RepeaterItem ri in BrandSelectorRepeater.Items)
			{
				if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
				{
					CheckBox IsSelectedCheckBox = (CheckBox)ri.FindControl("IsSelectedCheckBox");
					RadioButton IsMainBrandRadioButton = (RadioButton)ri.FindControl("IsMainBrandRadioButton");

					IsSelectedCheckBox.Enabled = true;

					if (IsMainBrandRadioButton.Checked)
					{
						// The main brand must be selected
						IsSelectedCheckBox.Checked = true;
						IsSelectedCheckBox.Enabled = false;
					}
				}
			}

			RefreshCompanyDropDownList();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Refreshes the company drop down list.
		/// </summary>
		private void RefreshCompanyDropDownList()
		{
			// Get the list of the companies which are available to the selected brand.
			// If the staff setting is selected, the only show internal companies.  Otherwise, only
			// show external companies.  Employees must be assigned to an internal company, and
			// non-employees must be assigned to a non-internal company.
			// If we only have a single brand, then ignore the brand assignment settings.

			var companies = from c in CompanyCache.Instance.GetList()
			                where (
			                      	((BrandManager.IsSingleBrandMode) || (c.BrandList.Any(b => b.BrandId == GetPrimaryBrandId())))
			                      	&& (c.IsInternal == (NumericUtils.ParseInt32(IsStaffUserRadioButtonList.SelectedValue, 0) == 1))
			                      )
			                select c;

			CompanyDropDownList.DataSource = companies;
			CompanyDropDownList.DataTextField = Company.Columns.Name.ToString();
			CompanyDropDownList.DataValueField = Company.Columns.Name.ToString();
			CompanyDropDownList.DataBind();

			CompanyDropDownList.Items.Insert(0, new ListItem("Other", string.Empty));
		}

		/// <summary>
		/// Gets the primary brand id.
		/// </summary>
		/// <returns></returns>
		private int GetPrimaryBrandId()
		{
			if (BrandSelectorRow.Visible)
			{
				return (from RepeaterItem ri in BrandSelectorRepeater.Items
				        where GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem)
				        let IsMainBrandRadioButton = (RadioButton) ri.FindControl("IsMainBrandRadioButton")
				        where IsMainBrandRadioButton.Checked
				        select (HiddenField) ri.FindControl("BrandIdHiddenField")
				        into BrandIdHiddenField
				        select NumericUtils.ParseInt32(BrandIdHiddenField.Value, 0)).FirstOrDefault();
			}

			return WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();
		}

		#endregion
	}
}