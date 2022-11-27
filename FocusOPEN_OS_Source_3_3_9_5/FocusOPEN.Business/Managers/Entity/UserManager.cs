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
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class UserManager
	{
		#region Events

		public static event UserEventHandler UserCreatedByAdmin;

		/// <summary>
		/// Fired when a preapproved user is created (ie. approved email domain and ip address)
		/// </summary>
		public static event UserEventHandler PreApprovedUserCreated;

		/// <summary>
		/// Fired when an unapproved user is created (ie. approved email domain and unapproved ip address)
		/// </summary>
		public static event UserEventHandler UnapprovedUserCreated;

		/// <summary>
		/// Fired when a user requests a password reminder email
		/// </summary>
		public static event UserEventHandler PasswordResetRequested;

		/// <summary>
		/// Fired when an admin processes a non-employee
		/// </summary>
		public static event UserEventHandler NonEmployeeProcessed;

		#endregion

		#region Properties

		/// <summary>
		/// The number of days after which an account should expire
		/// </summary>
		public static int AccountExpiryDays { get; set; }

		/// <summary>
		/// The number of days after which a password should expire
		/// </summary>
		public static int PasswordExpiryDays { get; set; }

		/// <summary>
		/// 
		/// </summary>
        public static RegistrationEmailFormatType RegistrationEmailFormat { get; set; }

		#endregion

		#region New User Methods

		/// <summary>
		/// Gets a new user with default properties set
		/// </summary>
		public static User GetNewUser()
		{
			User user = User.New();

			user.PasswordSalt = PasswordGenerator.GeneratePassword(6).ToUpper();
			user.PasswordExpiryDate = DateTime.Now.AddMinutes(-1);
			user.IsPasswordExpired = true;
			user.RegisterDate = DateTime.Now;
			user.LastLoginDate = null;

			// Set the default account expiry date
			user.AccountExpiryDate = DateTime.Now.AddDays(AccountExpiryDays);

			return user;
		}

		/// <summary>
		/// Used for users created by an admin from the admin area
		/// </summary>
		/// <param name="user">The user being created</param>
		/// <param name="creator">The admin user creating the user</param>
		/// <param name="notify">Boolean specifying whether welcome email should be sent</param>
		public static void Create(User user, User creator, bool notify)
		{
			// First run standard validations
			ErrorList errorList = ValidateUser(user);

			if (!notify && string.IsNullOrEmpty(user.UnencryptedPassword))
				errorList.Add("Password must be specified when welcome email is not being sent");

			// Quit if any validation errors occurred
			if (errorList.Count > 0)
				throw new InvalidUserException(errorList, user);

			// Bypass the standard admin approval as this user is created by an admin
			// Just pre-approve this account.
			user.UserStatus = UserStatus.Approved;

			// Set account expiry date
			user.AccountExpiryDate = DateTime.Now.AddDays(AccountExpiryDays);

			// Expire password immediately so that user must change it on first login
			user.PasswordExpiryDate = DateTime.Now.AddDays(-1);

			// Save user to database
			SaveUser(user);

            //generate a new user api token
            user.RegenerateUserAPIToken();

			// Update the audit log for both the admin and new user
			AuditLogManager.LogUserAction(creator, AuditUserAction.AddUser, string.Format("Created user: {0}, with UserId: {1}", user.FullName, user.UserId));
			AuditLogManager.LogUserAction(user, AuditUserAction.RegisteredByAdmin, string.Format("Created by admin: {0}", creator.FullName));

			// Fire events
			if (notify)
				if (UserCreatedByAdmin != null)
					UserCreatedByAdmin(null, new UserEventArgs(user));
		}

		/// <summary>
		/// Used for public registrations
		/// </summary>
		public static void Register(User user)
		{
			// Initialise the approved company to a null instance
			Company company = Company.Empty;

			// Get the IP Address
			string ipAddress = BusinessHelper.GetCurrentIpAddress();

            // Only check for an approved company if email is not blank. 
            if (!StringUtils.IsBlank(user.Email))
            {
                company = UserSecurityManager.GetCompanyByDomain(user.Email);
            }

            //Process depends on the value defined in the AppSettings key "RegisterUserRequiresKnownEmailFormat".
            switch (RegistrationEmailFormat)
            {
                case RegistrationEmailFormatType.Empty:
                {
                    if (user.IsEmployee && !company.IsNull)
                    {
                        user.UserStatus = UserStatus.PendingEmailConfirmation;
                    }
                    else
                    { 
                        user.UserStatus = UserStatus.PendingAdminApproval;
                    }

                    break;
                }
                case RegistrationEmailFormatType.InternalUsers:
                {
                    if (user.IsEmployee)
                    {
                        if (company.IsNull)
                        {
                            throw new RegistrationSecurityException(user, "Your email address is not recognised as being from a company authorised to use this website. Please contact the system administrator for further information.", ipAddress);
                        }

                        user.UserStatus = UserStatus.PendingEmailConfirmation;
                    }
                    else
                    {
                        user.UserStatus = UserStatus.PendingAdminApproval;
                    }

                    break;
                }
                case RegistrationEmailFormatType.AllUsers:
                {
                    if (company.IsNull)
                    {
                        throw new RegistrationSecurityException(user, "Your email address is not recognised as being from a company authorised to use this website. Please contact the system administrator for further information.", ipAddress);
                    }

                    user.UserStatus = UserStatus.PendingEmailConfirmation;

                    break;
                }
            }

            // Check if user is employee or belongs to an internal company
            // (If a user belongs to an internal company, then they are effectively an employee)
            bool isEmployeeOrInternal = (user.IsEmployee || company.IsInternal);

			// They are an employee or their email belongs to an internal company, but their ip address isn't approved so quit
			if (Settings.IpAddressRestrictionEnabled && isEmployeeOrInternal && !UserSecurityManager.IsApprovedIpAddress(ipAddress))
				throw new RegistrationSecurityException(user, "Your IP address is not recognised as being from a company authorised to use this website. Please contact the system administrator for further information.", ipAddress);

			// Everything passed, update some standard user settings
			user.IsAccountNonExpiring = false;
			user.IsPasswordNonExpiring = false;
			user.UserRole = UserRole.Normal;
			user.RegisterDate = DateTime.Now;

			// Set up default user rules and expiry dates
			user.IsAllowedExternalAccess = !user.IsEmployee;
            user.EnableFilePathIngestion = false;
			user.AccountExpiryDate = DateTime.Now.AddDays(AccountExpiryDays);
			user.PasswordExpiryDate = DateTime.Now.AddDays(PasswordExpiryDays);

			// Validate standard business objecr requirements
			ErrorList errors = ValidateUser(user);

			// Throw an error if the user fields are not all valid
			if (errors.Count > 0)
				throw new InvalidUserException(errors, user);

			// Save the user
			User u = SaveUser(user);

			FireUserCreateEvent(user);

			AuditLogManager.LogUserAction(u, AuditUserAction.Register, string.Format("Registration successful. Registered from IP Address: {0}.  Account status is: {1}", ipAddress, u.UserStatus));
		}

		/// <summary>
		/// Fires the relevant user created event
		/// </summary>
		/// <param name="user">The user to checked and passed to the event</param>
		public static void FireUserCreateEvent(User user)
		{
			// Fire corresponding event (so emails can be sent, etc)
			switch (user.UserStatus)
			{
				case (UserStatus.PendingEmailConfirmation):
					if (PreApprovedUserCreated != null)
						PreApprovedUserCreated(null, new UserEventArgs(user));

					break;

				case (UserStatus.PendingAdminApproval):
					if (UnapprovedUserCreated != null)
						UnapprovedUserCreated(null, new UserEventArgs(user));

					break;
			}
		}

		/// <summary>
		/// Validates and saves the existing user
		/// </summary>
		public static void Update(User user)
		{
			if (user.IsNew)
				throw new SystemException("This method cannot be used to save new users");

			// Validate standard business object requirements
			ErrorList errors = ValidateUser(user);

			// Throw an error if the user fields are not all valid
			if (errors.Count > 0)
				throw new InvalidUserException(errors, user);

			// NB: Do not inline as PasswordChanged is reset in SaveUser()
			bool passwordChanged = user.ChangedProperties.ContainsKey(User.Columns.Password.ToString());

			// Save the user
			SaveUser(user);

			// Update the password history if the password was changed
			if (passwordChanged)
				PasswordHistory.UpdateUserPasswordHistory(user);
		}

		/// <summary>
		/// Saves the user.  If the account is marked as non-expiring,
		/// the account expiry date is changed to DateTime.MaxValue before saving.
		/// Will also set the user's CompanyId based on their email address
		/// </summary>
		private static User SaveUser(User user)
		{
			if (user.IsPasswordNonExpiring)
				user.PasswordExpiryDate = null;

			if (user.IsAccountNonExpiring)
				user.AccountExpiryDate = null;

			return User.Update(user);
		}

		#endregion

		#region User Validation

		private static ErrorList ValidateUser(User user)
		{
			ErrorList errors = new ErrorList();

			if (StringUtils.IsBlank(user.FirstName))
				errors.Add("First name is required");

			if (StringUtils.IsBlank(user.LastName))
				errors.Add("Last name is required");

			if (StringUtils.IsBlank(user.Email))
			{
				errors.Add("Email address is required");
			}
			else if (!StringUtils.IsEmail(user.Email))
			{
				errors.Add("Invalid email address");
			}
			else
			{
                Company company = UserSecurityManager.GetCompanyByDomain(user.Email);

                if (company.IsNull)
                {
                    switch (RegistrationEmailFormat)
                    {
                        case RegistrationEmailFormatType.InternalUsers:
                        {
                            if (user.IsEmployee && company.IsNull)
                            {
                                errors.Add("Email address is not valid for external users");
                            }

                            break;
                        }
                        case RegistrationEmailFormatType.AllUsers:
                        {
                            if (company.IsNull)
                            {
                                errors.Add("Email address must belong to approved domain");
                            }

                            break;
                        }
                    }
                }
                else
                {
                    User u = User.GetByEmail(user.Email);

                    if (!u.IsNull && !u.UserId.Equals(user.UserId))
                        errors.Add("Email address is already in use");
                }
			}

			if (StringUtils.IsBlank(user.CompanyName))
				errors.Add("Company name is required");

			if (StringUtils.IsBlank(user.Password))
				errors.Add("Password is required");

			if (user.PasswordChanged)
			{
				if (user.UnencryptedPassword.Length == 0)
				{
					errors.Add("Password confirmation is required");
				}
				else if (user.UnencryptedPassword != user.UnencryptedConfirmPassword)
				{
					errors.Add("Password and confirm password do not match");
				}
				else
				{
					ErrorList e = PasswordGenerator.ValidatePassword(user.UnencryptedPassword);

					if (e.Count > 0)
					{
						errors.AddRange(e);
					}
					else if (!user.IsNew && PasswordHistory.IsRecentPassword(user, user.Password))
					{
						errors.Add("Password has been used recently and cannot be used again");
					}
				}
			}

			if (user.Brands.Count == 0)
			{
				errors.Add("User must be assigned to at least one brand");
			}
			else
			{
				if (user.PrimaryBrandId <= 0)
				{
					errors.Add("One brand must be select as the main brand");
				}
				else if (user.Brands.FindIndex(b => b.BrandId == user.PrimaryBrandId) < 0)
				{
					errors.Add("The main brand must be selected");
				}
			}

			if (StringUtils.IsBlank(user.CompanyName))
				errors.Add("Company name is required");

			if (user.UserRoleId == 0)
				errors.Add("System Error : User role must be specified");

			if (user.UserRole == UserRole.BrandAdministrator)
			{
				// Only one brand admin per business-unit
				UserFinder finder = new UserFinder {UserRole = UserRole.BrandAdministrator, PrimaryBrandId = user.PrimaryBrandId};
				User u = User.FindOne(finder);

				// Found a user, and user id is different from this user, so don't allow change
				if (!u.IsNull && !u.UserId.Equals(user.UserId))
				{
					string message = string.Format("The user {0} is already assigned to the Brand Administrator role for the selected brand(s)", u.FullName);
					errors.Add(message);
				}
			}

			return (errors);
		}

		#endregion

		#region User re-activation

		public static void ReactivateUser(User user)
		{
			user.AccountExpiryDate = DateTime.Now.AddDays(AccountExpiryDays);
			User.Update(user);

			string notes = string.Format("Account reactivated, expiry date updated to: {0}", user.AccountExpiryDate.GetValueOrDefault().ToString("dd MMMM yyyy HH:mm"));
			AuditLogManager.LogUserAction(user, AuditUserAction.ReactivateAccount, notes);
		}

		#endregion

		#region Password Management

		public static void ChangePassword(User user, string existingPassword, string newPassword, string newPasswordConfirmation)
		{
			// First make sure that the existing password the user has entered is valid
			if (!user.CheckPassword(existingPassword))
				throw new ChangePasswordException("Old password is incorrect");

			// Ensure that they entered a new password
			if (StringUtils.IsBlank(newPassword))
				throw new ChangePasswordException("New password cannot be blank");

			// Ensure that the new password and confirm new password match
			if (newPassword.Trim() != newPasswordConfirmation.Trim())
				throw new ChangePasswordException("New password does not match confirmed password");

			// Ensure that the user has not used the new password recently
			if (PasswordHistory.IsRecentPassword(user, newPassword))
				throw new ChangePasswordException("New password has been used recently and cannot be used again");

			// Validate the new password, ensuring it meets all password criteria
			ErrorList e = PasswordGenerator.ValidatePassword(newPassword);
			if (e.Count > 0) throw new ChangePasswordException(e[0].ToString());

			// Everything has password.  Set the new password and update the user's
			// password expiry date.  Then save the user back to the database.
			user.SetPassword(newPassword);
			user.PasswordExpiryDate = DateTime.Now.AddDays(PasswordExpiryDays);
			User.Update(user);

			// Update the user's password history (this is so that we can stop the same
			// password from being used again in future).
			PasswordHistory.UpdateUserPasswordHistory(user);

			// Update the audit log
			AuditLogManager.LogUserAction(user, AuditUserAction.ChangePassword, "Changed password");
		}

		/// <summary>
		/// Sends a password reminder to the specified user
		/// </summary>
		/// <param name="email">Email address of user requesting password</param>
		/// <exception cref="InvalidUserException">Thrown if email address is empty, or no user with email address exists</exception>
		public static void ResetPasswordAndSend(string email)
		{
			if (StringUtils.IsBlank(email))
				throw new InvalidUserException("Email address is required", User.Empty);

			User user = User.GetByEmail(email);

			if (user.IsNull)
				throw new InvalidUserException("Your account could not be found", user);

			user.SetPassword(PasswordGenerator.GeneratePassword());
			user.PasswordExpiryDate = DateTime.Now.AddMinutes(-1);
			User.Update(user);

			if (PasswordResetRequested != null)
				PasswordResetRequested(null, new UserEventArgs(user));

			AuditLogManager.LogUserAction(user, AuditUserAction.PasswordReminder, "Requested a password reminder");
		}

		#endregion

		/// <summary>
		/// Changes an employee from PendingAdminApproval to PendingEmailConfirmation.
		/// Sends them an email with their updated status too.
		/// </summary>
		/// <param name="user">The processed user</param>
		/// <param name="adminUser">The admin user performing the action</param>
		/// <param name="approved">True if user is approved, false if rejected</param>
		/// <param name="notes">Admin notes attached to this approval or denial</param>
		/// <exception cref="SystemException"></exception>
		public static void ProcessNonEmployee(User user, User adminUser, bool approved, string notes)
		{
			// Ensure unapproved users have notes
			if (!approved && notes.Length == 0)
				throw new SystemException("Please specify the reason why the user is rejected in the notes");

			// Update the user
			user.UserStatus = (approved) ? UserStatus.PendingEmailConfirmation : UserStatus.Rejected;
			user.Notes = notes;

			// Rejected users should be flagged as deleted
			if (!approved)
				user.IsDeleted = true;

			SaveUser(user);

			// Fire notifications
			if (NonEmployeeProcessed != null)
				NonEmployeeProcessed(null, new UserEventArgs(user));

			// Update audit log
			string auditNotes = string.Format("Account processed by {0} and {1}", adminUser.FullName, ((approved) ? "approved" : "rejected"));
			AuditLogManager.LogUserAction(user, AuditUserAction.AuthoriseUser, auditNotes);
		}

		/// <summary>
		/// Changes the user status from pending email confirmation to approved
		/// </summary>
		public static void ConfirmEmailAddress(User user)
		{
			if (!user.IsNull)
			{
				if (user.UserStatusId == Convert.ToInt32((UserStatus.PendingEmailConfirmation)))
				{
					user.UserStatusId = Convert.ToInt32(UserStatus.Approved);
					SaveUser(user);

					AuditLogManager.LogUserAction(user, AuditUserAction.ConfirmEmailAddress, string.Format("Confirmed email address: {0}", user.Email));
				}
			}
		}

		/// <summary>
		/// Gets the user authorised to manage (approve) the specified user's account
		/// </summary>
		public static User GetAuthorisingUser(User user)
		{
			User owner;

			if (user.IsEmployee)
			{
				UserFinder finder = new UserFinder {UserRole = UserRole.BrandAdministrator, PrimaryBrandId = user.PrimaryBrandId, IsSuspended = false};
				owner = User.FindOne(finder);
			}
			else
			{
				// Get the user that created the company this user belongs to
				return UserSecurityManager.GetCompanyByDomain(user.Email).CreatedByUser;
			}

			// Ensure that they are still active and in an authorised role
			// If not, try and get a super admin instead
			if (!user.IsActive() || owner.UserRoleId < Convert.ToInt32(UserRole.BrandAdministrator))
			{
				UserFinder finder = new UserFinder {UserRole = UserRole.SuperAdministrator, IsSuspended = false};
				owner = User.FindOne(finder);

				if (owner.IsNull)
					throw new SystemException("Authorising user could not be found.  User's company creator is invalid and no super-administrators are available.");
			}

			return owner;
		}

		/// <summary>
		/// Deletes a user (marks them as deleted)
		/// </summary>
		/// <param name="adminUser">The admin user performing the delete operation</param>
		/// <param name="userId">The user to be marked as deleted</param>
		public static void DeleteUser(User adminUser, int userId)
		{
			User user = User.Get(userId);

			if (!user.IsNull)
			{
				// Change email address and append user id onto it
				// This is in case another user tries to register
				// with it or this user tries to re-register.
				user.Email = user.UserId + "_" + user.Email;

				user.IsDeleted = true;
				User.Update(user);
				AuditLogManager.LogUserAction(adminUser, AuditUserAction.DeleteUser, string.Format("Deleted user: {0}, with UserId: {1}", user.FullName, user.UserId));
			}
		}

		public static void ChangeUserHash(int userId, out string hash, out string guid)
		{
			User user = User.Get(userId);
			user.Guid = GeneralUtils.GetGuid();
			User.Update(user);

			hash = StringHasher.HashString(user.Email);
			guid = user.Guid;
		}

		public static User GetUser(int userId, string hash, string guid)
		{
			UserFinder finder = new UserFinder {UserId = userId, Guid = guid};
			User u = User.FindOne(finder);

			if (StringHasher.VerifyHash(hash, u.Email))
				return u;

			return User.Empty;
		}


        /// <summary>
        /// Updates the Last API Operation to current date time
        /// </summary>
        /// <param name="userId">The user id.</param>
        public static void UpdateLastAPIOperation(User user)
        {
            if (!user.IsNull)
            {
                user.LastAPIOperation = DateTime.Now;
                User.Update(user);
            }
        }


        /// <summary>
        /// Clears the user's SessionAPIToken
        /// </summary>
        public static User ClearSessionAPIToken(User user)
        {
             if (!user.IsNull)
             {
                 user.SessionAPIToken = null;
                 User.Update(user);
             }
             return user;
        }

        /// <summary>
        /// creates a new SessionAPIToken for the user
        /// </summary>
        public static User RenewSessionAPIToken(User user)
        {

             if (!user.IsNull)
             {
                 user.SessionAPIToken = System.Guid.NewGuid().ToString();
                 user.LastAPIOperation = DateTime.Now;
                 User.Update(user);
             }
             return user;
        }


        /// <summary>
        /// validates that the user's session api token is current
        /// </summary>
        public static bool APISessionIsValid(User user)
        {
            //checks that current datetime is later than 
            //the last successful operation plus the session timeout length
            if (!String.IsNullOrEmpty(user.SessionAPIToken))
            {
                if (user.LastAPIOperation.HasValue && System.DateTime.Now <= user.LastAPIOperation.Value.AddMinutes(Convert.ToDouble(Settings.SessionTimeout)))
                {
                    return true;
                }
            }

            return false;
        }





	}
}
