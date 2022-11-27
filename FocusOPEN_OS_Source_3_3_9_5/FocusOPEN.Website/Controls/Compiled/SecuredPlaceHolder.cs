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
using System.Linq;
using System.Web.UI.WebControls;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	/// <summary>
	/// Secured placeholder control can be wrapped around any controls and ensures
	/// that it is only displayed when the user's role is in the list of allowed roles
	/// </summary>
	public class SecuredPlaceHolder : PlaceHolder
	{
		#region Accessors

		/// <summary>
		/// Gets or sets the allowed roles
		/// </summary>
		public string AllowedRoles { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this panel should be restricted to engineers only
		/// </summary>
		public bool IsEngineer { get; set; }

		#endregion

		#region Overrides

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// Hide panel if it's restricted to engineers and current
			// user is not an engineer.
			if (IsEngineer && !SessionInfo.Current.User.IsEngineer)
			{
				Visible = false;
				return;
			}

			// Split the list of allowed roles into a list of user roles
			// This is a bit of a hack as we can't pass a collection
			// into the user control, so we pass in a string instead
			// and then convert it into the UserRole enum here.
			var allowedRoles = from role in AllowedRoles.Split(',')
			                   select EnumUtils.GetEnumFromValue<UserRole>(role);

			// Make this visible if the current user's role is in the list of allowed roles.
			Visible = allowedRoles.Contains(SessionInfo.Current.User.UserRole);
		}

		#endregion
	}
}