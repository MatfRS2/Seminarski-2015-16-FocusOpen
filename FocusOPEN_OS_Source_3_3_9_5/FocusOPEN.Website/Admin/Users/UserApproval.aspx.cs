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
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class UserApproval : BaseAdminPage
	{
		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				int userId = GetRequiredQueryStringParameter("userId", "UserList.aspx");
				User user = Data.User.Get(userId);

				if (user.UserStatusId != Convert.ToInt32(UserStatus.PendingAdminApproval))
				{
					MessageLabel1.SetErrorMessage("User has already been processed");
					MessageLabel1.Pinned = true;
					ActionPanel.Visible = false;
					SubmitButton.Visible = false;
					ChangeCancelButton();
				}

				FirstNameLabel.Text = user.FirstName;
				LastNameLabel.Text = user.LastName;
				EmailAddressHyperLink.EmailAddress = user.Email;
				CompanyLabel.Text = user.CompanyName;
				CountryLabel.Text = user.Country.Name;
				PhoneNumberLabel.Text = user.PhoneNumber;
				MobileNumberLabel.Text = user.MobileNumber;
				NotesTextBox.Text = user.Notes;
			}
		}

		protected void SubmitButton_Click(object sender, EventArgs e)
		{
			bool isApproved = (ApprovedRadioButtonList.SelectedValue == "1");
			string notes = NotesTextBox.Text.Trim();

			try
			{
				int userId = GetRequiredQueryStringParameter("userId", "UserList.aspx");
				User user = Data.User.Get(userId);
				UserManager.ProcessNonEmployee(user, CurrentUser, isApproved, notes);

				MessageLabel1.SetSuccessMessage("User processed successfully");

				ApprovedRadioButtonList.Enabled = false;
				NotesTextBox.Enabled = false;
				SubmitButton.Visible = false;

				ChangeCancelButton();
			}
			catch (SystemException sysex)
			{
				MessageLabel1.SetErrorMessage(sysex.Message);
			}
		}

		#endregion

		#region Helper Methods

		private void ChangeCancelButton()
		{
			CancelButton1.Prompt = string.Empty;
			CancelButton1.Text = "&laquo; user list";
		}

		#endregion
	}
}