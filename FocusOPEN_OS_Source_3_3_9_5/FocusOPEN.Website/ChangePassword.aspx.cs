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
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website
{
	public partial class ChangePassword : BaseAccountChangePage
	{
		protected void ChangePasswordButton_Click(object sender, EventArgs e)
		{
			string existingPassword = ExistingPasswordTextBox.Text;
			string newPassword = NewPasswordTextBox.Text;
			string newPasswordConfirmation = ConfirmPasswordTextBox.Text;

			try
			{
				UserManager.ChangePassword(CurrentUser, existingPassword, newPassword, newPasswordConfirmation);
				SessionInfo.Current.User = CurrentUser;
				LoginManager.UpdateLastLoginAuditInfo(SessionInfo.Current.User);
				Login.SetupSessionForCurrentSessionUser();
				Response.Redirect("~/Default.aspx");
			}
			catch (ChangePasswordException cpex)
			{
				FeedbackLabel1.SetErrorMessage(cpex.Message);
			}
		}
	}
}