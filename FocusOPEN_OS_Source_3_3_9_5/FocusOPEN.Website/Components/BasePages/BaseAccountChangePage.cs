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

namespace FocusOPEN.Website.Components
{
	public class BaseAccountChangePage : BasePage
	{
		#region Private variables

		#endregion

		#region Accessors

		protected User CurrentUser { get; set; }

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Page.IsPostBack)
			{
				CurrentUser = Context.Items["User"] as User;

				if (CurrentUser == null || CurrentUser.IsNull)
				{
					Response.Redirect("~/Login.aspx?error=NoUserOnLoad", false);
					return;
				}

				// Add some things to the viewstate so we
				// can retrieve the user on the postback
				ViewState.Add("UserId", CurrentUser.UserId);
				ViewState.Add("Email", CurrentUser.Email);
			}
			else
			{
				int userId = Convert.ToInt32(ViewState["UserId"]);
				string email = ViewState["Email"].ToString();

				User user = Data.User.Get(userId);

				if (user.Email == email)
				{
					CurrentUser = user;
				}
				else
				{
					Response.Redirect("~/Login.aspx?error=NoUserOnPostback", false);
					return;
				}
			}
		}
	}
}