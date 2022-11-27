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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website
{
	public partial class Login : BasePage
	{
		#region Accessors

		/// <summary>
		/// Shortcut to SessionInfo.Current.User
		/// </summary>
		private static User CurrentUser
		{
			get
			{
				return SessionInfo.Current.User;
			}
			set
			{
				SessionInfo.Current.User = value;
			}
		}

		#endregion

		#region Handled Events

#if DEBUG

		protected void Page_Init(object sender, EventArgs e)
		{
			if (Request.IsLocal && Environment.MachineName.ToLower() == "kiwi")
			{
				UserFinder finder = new UserFinder();
				finder.SortExpressions.Add(new AscendingSort("Email"));

				var users = from u in Data.User.FindMany(finder)
				            select new { Text = u.FullName + " (" + u.Email + ") - " + EnumUtils.GetDescription(u.UserRole), Value = u.Email };

				DropDownList dd = new DropDownList
				                  	{
				                  		ID = "UserDropDownList",
				                  		CssClass = "formInput W300",
				                  		DataSource = users,
				                  		DataTextField = "Text",
				                  		DataValueField = "Value"
				                  	};
				dd.DataBind();

				dd.SelectedValue = Data.User.Get(1).Email;

				EmailCell.Controls.Clear();
				EmailCell.Controls.Add(dd);
			}
		}

#endif

		protected void Page_Load(object sender, EventArgs e)
		{
			PasswordReminderButton.Visible = (ConfigurationManager.AppSettings.GetBoolValue("PasswordResetEnabled"));

			if (!Page.IsPostBack)
			{
				if (!CurrentUser.IsNull && WebUtils.GetRequestParam("action") == "logout")
				{
					SessionEndModule.EndSession();
					LoginManager.Logout(CurrentUser);
					SessionInfo.Current.Reset();

					const string url = "~/login.aspx?message=LoggedOut";
					Response.Redirect(url, false);

					return;
				}

				// Get message key from querystring
				string message = WebUtils.GetRequestParam("message", string.Empty);

				switch (message)
				{
					case "AccountReactivated":
						MessageLabel1.SetSuccessMessage("Your account has been reactivated successfully");
						break;

					case "PasswordChanged":
						MessageLabel1.SetSuccessMessage("Your new password has been activated");
						break;

					case "LoggedOut":
						MessageLabel1.SetSuccessMessage("You have been logged out");
						break;

					case "EmailConfirmed":
						MessageLabel1.SetSuccessMessage("Your email address has been confirmed.  Please login below.");
						break;

					case "AccessDenied":
						MessageLabel1.SetErrorMessage("Access to the requested page is denied due to insufficient user credentials.");
						break;

					case "NoUserOnLoad":
					case "NoUserOnPostback":

						break;
				}


				string cookieEmail = CookieManager.GetValue("EmailAddress");
				RememberMeCheckBox.Checked = (cookieEmail != string.Empty);

				DropDownList dd = SiteUtils.FindControlRecursive(Page, "UserDropDownList") as DropDownList;

				if (dd != null)
				{
					ListItem li = dd.Items.FindByValue(cookieEmail);

					if (li != null)
					{
						dd.SelectedIndex = -1;
						li.Selected = true;
					}
				}
				else
				{
					EmailTextBox.Text = cookieEmail;
				}
			}
		}

		protected void LoginButton_Click(object sender, EventArgs e)
		{
			string email = GetEmailAddress();
			string password = PasswordTextBox.Text.Trim();
			bool saveEmail = RememberMeCheckBox.Checked;

			try
			{
				User user = Data.User.Empty;

#if DEBUG
				if (Request.IsLocal && password == "!!!")
				{
					user = Data.User.GetByEmail(email);
					LoginManager.UpdateLastLoginAuditInfo(user);
				}

				if (Request.IsLocal && password == "~!!!")
				{
					user = Data.User.GetByEmail(email);
					throw new PasswordExpiredException("Debug login - password has expired", user);
				}
#endif

				if (user.IsNull)
					user = LoginManager.Login(email, password);	

#if (!DEBUG)
				Brand brand = WebsiteBrandManager.GetBrand();

				if (!user.CanAccessBrand(brand.BrandId.GetValueOrDefault()))
					Response.Redirect(brand.WebsiteUrl + "Login.aspx?message=AccessDenied&errorDetail=BrandAccessDenied");
#endif

				CurrentUser = user;

				SetupSessionForCurrentSessionUser();

				SaveEmail(saveEmail, user.Email);

				Redirect();
			}
			catch (UserPendingEmailConfirmationException ex)
			{
				MessageLabel1.SetErrorMessage(ex.Message, "An email message has been sent to you containing a link to confirm your email address.  If you do not receive this email message or continue to have problems logging in, please contact your systems administrator for further assistance.");
				UserManager.FireUserCreateEvent(ex.Entity);
			}
			catch (LoginException ex)
			{
				MessageLabel1.SetErrorMessage(ex.Message);
			}
			catch (LoginSecurityException ex)
			{
				MessageLabel1.SetErrorMessage(ex.Message, "Please contact your Systems Administrator for further information.");

				if (ex.NotifyAdmins)
					NotifyEngine.InvalidLoginAttempt(ex);
			}
			catch (AccountExpiredException ex)
			{
				MessageLabel1.SetErrorMessage("Your account has expired", "An email message has been sent to you containing a link to reactivate your account. If you do not receive this email message or continue to have problems logging in, please contact your systems administrator for further assistance.");
				NotifyEngine.SendReactivateAccountMessage(ex.Entity);
			}
			catch (PasswordExpiredException ex)
			{
				Context.Items.Add("User", ex.Entity);
				Server.Transfer("~/ChangePassword.aspx", false);
			}
		}

		protected void PasswordReminderButton_Click(object sender, EventArgs e)
		{
			try
			{
				UserManager.ResetPasswordAndSend(GetEmailAddress());
				MessageLabel1.SetSuccessMessage("A new password has been emailed to you.");
			}
			catch (InvalidUserException iuex)
			{
				MessageLabel1.SetErrorMessage(iuex.Message);
			}
		}

		#endregion

		#region Private Helper Methods

		private string GetEmailAddress()
		{
			string email = EmailTextBox.Text.Trim();

			DropDownList dd = SiteUtils.FindControlRecursive(Page, "UserDropDownList") as DropDownList;

			if (dd != null)
				email = dd.SelectedValue;

			return email;
		}

		private static void SaveEmail(bool saveEmail, string email)
		{
			if (saveEmail)
			{
				CookieManager.SetValue("EmailAddress", email);
			}
			else
			{
				CookieManager.ClearValue("EmailAddress");
			}
		}

		/// <summary>
		/// Checks the query for the return url, and defaults to 'default.asp' if none
		/// is specified, and then redirects to this url.
		/// </summary>
		private void Redirect()
		{
			// Default return url is homepage
			string returnUrl = "~/Default.aspx";

			// Get the return url from the querystring
			if (Request.QueryString.Get("returnUrl") != null)
				returnUrl = UrlEncoder.Decode(Request.QueryString.Get("returnUrl"));

			// No point sending the user back to the login page after login!
			if ((Path.GetFileName(returnUrl) ?? string.Empty).ToLower() == "login.aspx")
				returnUrl = "~/Default.aspx";

			Response.Redirect(returnUrl, false);
		}

		#endregion

		/// <summary>
		/// Set up the session for the user in SessionInfo.Current.User
		/// </summary>
		internal static void SetupSessionForCurrentSessionUser()
		{
			// Ensure user has lightbox
			LightboxManager.EnsureUserHasDefaultLightbox(SessionInfo.Current.User);

			// Set the default lightbox id
			SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.SelectedLightboxId = ContextInfo.LightboxManager.GetDefaultLightbox().LightboxId.GetValueOrDefault();

			// Set other stuff from cookie
			SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.PersistentCartLightboxMode = GeneralUtils.ParseEnum(CookieManager.GetValue("PersistentCartLightboxMode"), PersistentCartLightboxMode.Lightbox);
			SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.PersistentCartLightboxState = GeneralUtils.ParseEnum(CookieManager.GetValue("PersistentCartLightboxState"), PersistentCartLightboxState.Open);
			SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.CategoriesOpen = true;
			SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.SearchResultsView = GeneralUtils.ParseEnum(CookieManager.GetValue("SearchResultsView"), SearchResultsView.Thumbnails);
			SessionInfo.Current.AdminSessionInfo.AdminSavedAssetSearch.PageSize = CookieManager.GetValue("AdminSavedAssetSearchPageSize", 0);
			SessionInfo.Current.AdminSessionInfo.AdminSavedOrderSearch.PageSize = CookieManager.GetValue("AdminSavedOrderSearchPageSize", 0);
			SessionInfo.Current.AdminSessionInfo.AdminSavedUserSearch.PageSize = CookieManager.GetValue("AdminSavedUserSearchPageSize", 0);

			// Get uploaded asset ID's (assets that have been previously uploaded but not catalogued)
			SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Clear();
			SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.AddRange(SessionInfo.Current.User.GetCatalogueAssetIdList());
		}
	}
}