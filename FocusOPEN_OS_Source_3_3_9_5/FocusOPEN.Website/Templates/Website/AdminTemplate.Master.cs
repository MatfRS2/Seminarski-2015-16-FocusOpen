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
using System.Text;
using System.Web.UI;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website
{
	public partial class AdminTemplate : MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				UserNameHref.InnerText = SessionInfo.Current.User.FullName;
				UserNameHref.Title = GetUserTooltip(SessionInfo.Current.User);

				if (StringUtils.IsBlank(BreadCrumbsLabel.Text))
					BreadCrumbsLabel.Text = AdminNavigation1.GetBreadCrumbs();
			}
		}

		#region Helper Methods

		private static string GetUserTooltip(User user)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0}\n(", EnumUtils.GetDescription(user.UserRole));

			sb.AppendFormat("Brand: {0}", user.PrimaryBrand.Name);

			sb.Append(")");

			return sb.ToString();
		}

		#endregion
	}
}