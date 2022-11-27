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

namespace FocusOPEN.Website.MyAccount
{
	public partial class EditProfile : BaseSecuredPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				UserDetailsForm1.FirstName = CurrentUser.FirstName;
				UserDetailsForm1.LastName = CurrentUser.LastName;
				UserDetailsForm1.Email = CurrentUser.Email;
				UserDetailsForm1.IsStaffUser = CurrentUser.IsEmployee;
				UserDetailsForm1.CompanyName = CurrentUser.CompanyName;
				UserDetailsForm1.BrandId = CurrentUser.PrimaryBrandId;
				UserDetailsForm1.CountryId = CurrentUser.CountryId;
				UserDetailsForm1.PhoneNumber = CurrentUser.PhoneNumber;
				UserDetailsForm1.MobileNumber = CurrentUser.MobileNumber;
			}
		}

		protected void SaveChangesButton_Click(object sender, EventArgs e)
		{
			// Get the user data from the database
			// This is to ensure it is up-to-date, rather than just using the current user in
			// session in case the user has been updated by an admin since the user logged in.
			User user = Data.User.Get(CurrentUser.UserId);

			// Initialise user from posted values
			user.FirstName = UserDetailsForm1.FirstName;
			user.LastName = UserDetailsForm1.LastName;
			user.Email = UserDetailsForm1.Email;
			user.SetConfirmPassword(UserDetailsForm1.ConfirmPassword);
			user.IsEmployee = UserDetailsForm1.IsStaffUser;
			user.CompanyName = UserDetailsForm1.CompanyName;
			user.PrimaryBrandId = UserDetailsForm1.BrandId;
			user.CountryId = UserDetailsForm1.CountryId;
			user.PhoneNumber = UserDetailsForm1.PhoneNumber;
			user.MobileNumber = UserDetailsForm1.MobileNumber;

			if (!StringUtils.IsBlank(UserDetailsForm1.Password))
			{
				user.SetPassword(UserDetailsForm1.Password);
				
				if (user.PasswordChanged)
					user.PasswordExpiryDate = DateTime.Now.AddDays(UserManager.PasswordExpiryDays);
			}

			try
			{
				UserManager.Update(user);
				SessionInfo.Current.User = user;

				AuditLogManager.LogUserAction(user, AuditUserAction.AccountUpdate, "Profile updated successfully.");

				FeedbackLabel1.SetSuccessMessage("Your profile has been updated");
			}
			catch (InvalidUserException iuex)
			{
				FeedbackLabel1.SetErrorMessage("the following errors occured:", iuex.Errors);
			}
		}
	}
}