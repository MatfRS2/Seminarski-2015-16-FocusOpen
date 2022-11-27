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
using System.Web;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website
{
	public partial class Register : BasePage
	{
		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Global.AllowPublicRegistration)
				throw new HttpException(404, "Page not found");

			if (!Page.IsPostBack)
			{
				UserDetailsForm1.BrandId = WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();
			}
		}

		protected void RegisterButton_Click(object sender, EventArgs e)
		{
			// Get new user with default values set
			User user = UserManager.GetNewUser();

			// Set the default account expiry date
			user.AccountExpiryDate = DateTime.Now.AddDays(UserManager.AccountExpiryDays);

			// Initialise user from posted values
			user.FirstName = UserDetailsForm1.FirstName;
			user.LastName = UserDetailsForm1.LastName;
			user.Email = UserDetailsForm1.Email;
			user.SetPassword(UserDetailsForm1.Password);
			user.SetConfirmPassword(UserDetailsForm1.ConfirmPassword);
			user.IsEmployee = UserDetailsForm1.IsStaffUser;
			user.CompanyName = UserDetailsForm1.CompanyName;
			user.CountryId = UserDetailsForm1.CountryId;
			user.PhoneNumber = UserDetailsForm1.PhoneNumber;
			user.MobileNumber = UserDetailsForm1.MobileNumber;

			// Give the user access to the selected brand and set this as
			// the primary brand.  Additional brands can be added by an admin
			user.PrimaryBrandId = UserDetailsForm1.BrandId;
			user.Brands.Add(user.PrimaryBrand);

			try
			{
				UserManager.Register(user);

				FormPanel.Visible = false;
				FeedbackPanel.Visible = true;

                PendingEmailMessage.Visible = (user.UserStatus == FocusOPEN.Shared.UserStatus.PendingEmailConfirmation);
                AdminApprovalMessage.Visible = (user.UserStatus == FocusOPEN.Shared.UserStatus.PendingAdminApproval);
			}
			catch (InvalidUserException iuex)
			{
				FeedbackLabel1.SetErrorMessage("the following errors occured:", iuex.Errors);
			}
			catch (RegistrationSecurityException rsuex)
			{
				FeedbackLabel1.SetErrorMessage(rsuex.Message);
				NotifyEngine.InvalidRegistrationAttempt(rsuex);
			}
			catch (Exception ex)
			{
				FeedbackLabel1.SetErrorMessage("An unknown error occured:", ex.ToString());
			}
		}

		#endregion
	}
}