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
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class DatePicker : TextBox
	{
		public void Clear()
		{
			EnsureChildControls();
			Text = string.Empty;
		}

		public bool AllowDateBeforeToday
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "AllowDateBeforeToday", true);
			}
			set
			{
				ViewState["AllowDateBeforeToday"] = value;
			}
		}

		public DateTime? SelectedDate
		{
			get
			{
				EnsureChildControls();

				try
				{
					return Convert.ToDateTime(Text);
				}
				catch (Exception)
				{
					return null;
				}
			}
			set
			{
				EnsureChildControls();

				if (value.HasValue)
				{
					Text = value.GetValueOrDefault().ToString(Global.DateFormat);
				}
				else
				{
					Text = string.Empty;
				}
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			CssClass += " formInput datePicker";

			if (!AllowDateBeforeToday)
				CssClass += " pastDaysDisallowed";
		}
	}
}