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
using log4net;

namespace FocusOPEN.Business
{
	public static class LoginManager
	{
		#region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(typeof(LoginManager));
		private static int m_BadLoginCountThreshold = 5;
		private static int m_BadLoginLockoutMinutes = 5;

		#endregion

		#region Accessors

		/// <summary>
		/// The number of bad logins after which an account should be locked
		/// </summary>
		public static int BadLoginCountThreshold
		{
			get
			{
				return m_BadLoginCountThreshold;
			}
			set
			{
				m_BadLoginCountThreshold = value;
			}
		}

		/// <summary>
		/// The number of minutes an account should be locked for
		/// after consecutive bad logins
		/// </summary>
		public static int BadLoginLockoutMinutes
		{
			get
			{
				return m_BadLoginLockoutMinutes;
			}
			set
			{
				m_BadLoginLockoutMinutes = value;
			}
		}

		#endregion

		/// <summary>
		/// Verify the user credentials and the account, and if if valid, returns the user
		/// </summary>
		/// <param name="email">Email Address</param>
		/// <param name="password">Password</param>
		/// <exception cref="LoginException">Thrown if email or password is missing, account is not found, or password is invalid</exception>
		/// <exception cref="LoginSecurityException">Thrown if the account is pending admin approval, inactive, suspended, or the IP address is unknown and home access is not enabled</exception>
		/// <exception cref="UserPendingEmailConfirmationException">Thrown if the account is pending email confirmation</exception>
		/// <exception cref="AccountExpiredException">Thrown if the account is expired</exception>
		/// <exception cref="PasswordExpiredException">Thrown if the password is expired</exception>
		public static User Login(string email, string password)
		{
			// Remove space around posted values
			email = email.Trim();
			password = password.Trim();

			// Ensure email and password exists
			if (email.Length == 0 || password.Length == 0)
				throw new LoginException("Please enter email and password", User.Empty);

			// Get user matching email
			User user = User.GetByEmail(email);

			// Ensure user was found
			if (user.IsNull)
				throw new LoginException("User account not found", user);

			// Get any info about bad logins
			BadLoginInfo badLoginInfo = user.GetBadLoginInfo(m_BadLoginLockoutMinutes);

			// Check that the account isn't locked based on the bad login info
			if (AccountIsLocked(badLoginInfo))
				throw new LoginException("User account is locked. Please try again in " + m_BadLoginLockoutMinutes + " minutes.", user);

			// Get the IP address as we'll need it later
			string ipAddress = BusinessHelper.GetCurrentIpAddress();

			// Ensure password is correct
			if (!user.CheckPassword(password))
			{
				// Update log
				m_Logger.DebugFormat("Login failed for {0}, UserId: {1}  Invalid password.", user.FullName, user.UserId);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, "Login Failed.  Invalid Password.");

				// Update bad login info
				badLoginInfo.BadLoginCount++;
				badLoginInfo.LastBadLoginDate = DateTime.Now;

				// Check to see if this failed login has locked the account
				if (AccountIsLocked(badLoginInfo))
					throw new LoginException("Too many bad login attempts. Please try again in " + m_BadLoginLockoutMinutes + " minutes.", user);
				
				// Otherwise, bad login but account isn't locked
				throw new LoginException("Invalid Password", user);
			}

			// Ensure user is not pending admin approval
			if (user.UserStatus == UserStatus.PendingAdminApproval)
			{
				m_Logger.DebugFormat("Login failed for {0}, UserId: {1}  Account is pending admin approval.", user.FullName, user.UserId);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, "Login Failed.  Account pending admin approval.");
				throw new LoginSecurityException(user, "This account is awaiting approval", ipAddress);
			}

			// Ensure user is not pending email confirmation
			if (user.UserStatus == UserStatus.PendingEmailConfirmation)
			{
				m_Logger.DebugFormat("Login failed for {0}, UserId: {1}  Account is pending email confirmation.", user.FullName, user.UserId);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, "Login Failed.  Account pending email confirmation.");
				throw new UserPendingEmailConfirmationException("This account is pending email confirmation", user);
			}

			// Ensure user is not rejected
			if (user.UserStatus == UserStatus.Rejected)
			{
				m_Logger.DebugFormat("Login failed for {0}, UserId: {1}  Account is rejected.", user.FullName, user.UserId);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, "Login Failed.  Account status is rejected.");
				throw new LoginSecurityException(user, "This account is inactive", ipAddress);
			}

			// Ensure user is not suspended
			if (user.IsSuspended)
			{
				m_Logger.DebugFormat("Login failed for {0}, UserId: {1}  Account is suspended.", user.FullName, user.UserId);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, "Login Failed.  Account is suspended.");
				throw new LoginSecurityException(user, "This account has been suspended", ipAddress);
			}

			// Ensure user can login from unapproved ip address if they are logging in from an unapproved ip address
			if (Settings.IpAddressRestrictionEnabled && !UserSecurityManager.IsApprovedIpAddress(ipAddress) && !user.IsAllowedExternalAccess)
			{
				m_Logger.DebugFormat("Login failed for {0}, UserId: {1}  The IP address '{2}' is unapproved and account does not have external access enabled.", user.FullName, user.UserId, ipAddress);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, string.Format("Login Failed.  The IP address '{0}' is unapproved and user account does not have home access rights.", ipAddress));
				throw new LoginSecurityException(user, "Unable to log in. Unknown IP Address.", ipAddress);
			}

			// Ensure the user account has not expired
			if (user.GetAccountExpiryDate() < DateTime.Now)
			{
				m_Logger.DebugFormat("Login succeeded for {0}, UserId: {1} but account is expired", user.FullName, user.UserId);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, "Login succeeded, but account has expired.");
				throw new AccountExpiredException("This account has expired.", user);
			}

			// Ensure the password has not expired
			if (user.GetPasswordExpiryDate() < DateTime.Now)
			{
				m_Logger.DebugFormat("Login succeeded for {0}, UserId: {1} but password is expired", user.FullName, user.UserId);
				AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, "Login succeeded, but password has expired");
				throw new PasswordExpiredException("This account's password has expired", user);
			}

            //create new session api token
            if (!UserManager.APISessionIsValid(user))
            {
                UserManager.RenewSessionAPIToken(user);
            }

			// Update login date, audit login, etc
			UpdateLastLoginAuditInfo(user);

			return user;
		}

		/// <summary>
		/// Logs the user out (records an audit log event)
		/// </summary>
		public static void Logout(User user)
		{
			m_Logger.DebugFormat("User: {0}, UserId: {1} logged out successfully", user.FullName, user.UserId);
			AuditLogManager.LogUserAction(user, AuditUserAction.Logout, "Logged out manually");
		}

		/// <summary>
		/// Updates the last login date, account expiry date, and adds an user login audit entry
		/// </summary>
		public static void UpdateLastLoginAuditInfo(User user)
		{
			// Update the user
			user.LastLoginDate = DateTime.Now;
			user.AccountExpiryDate = DateTime.Now.AddDays(UserManager.AccountExpiryDays);
			User.Update(user);

			// Update the audit log
			string message = string.Format("Login successful. Account expiry date updated to {0}.", user.AccountExpiryDate.GetValueOrDefault().ToString("dd MMM yyyy HH:mm"));
			AuditLogManager.LogUserAction(user, AuditUserAction.UserLogin, message);
		}

		/// <summary>
		/// Checks if the user's account is locked
		/// </summary>
		private static bool AccountIsLocked(BadLoginInfo badLoginInfo)
		{
			if (badLoginInfo.BadLoginCount >= m_BadLoginCountThreshold && badLoginInfo.LastBadLoginDate.HasValue)
			{
				TimeSpan ts = DateTime.Now.Subtract(badLoginInfo.LastBadLoginDate.GetValueOrDefault());
				return (ts.TotalMinutes < m_BadLoginLockoutMinutes);
			}

			return false;
		}
	}
}