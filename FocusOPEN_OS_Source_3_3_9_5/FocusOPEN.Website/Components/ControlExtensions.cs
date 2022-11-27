/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

#region

using System.Web.UI.WebControls;
using FocusOPEN.Shared;

#endregion

namespace FocusOPEN.Website.Components
{
	public static class ControlExtensions
	{
		public static void SelectValue(this DropDownList dropdown, object value)
		{
			if (value == null)
				return;

			string s = value.ToString();

			if (dropdown.Items.FindByValue(s) != null)
				dropdown.SelectedValue = s;
		}

		public static int GetSelectedId(this DropDownList dropdown)
		{
			return NumericUtils.ParseInt32(dropdown.SelectedValue, 0);
		}

		public static void SafeSelectValue(this DropDownList dropdown, object val)
		{
			if (val == null)
				return;

			string s = val.ToString();

			if (dropdown.Items.FindByValue(s) != null)
				dropdown.SelectedValue = s;
		}
	}
}