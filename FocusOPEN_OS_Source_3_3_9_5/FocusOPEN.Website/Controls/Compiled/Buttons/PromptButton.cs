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

namespace FocusOPEN.Website.Controls
{
	/// <summary>
	/// Subclassed button allowing a javascript confirm prompt to be added to the button
	/// </summary>
	public class PromptButton : Button
	{
		#region Accessors

		public string Prompt
		{
			get
			{
				if (ViewState["Prompt"] == null)
					return string.Empty;

				return ViewState["Prompt"].ToString();
			}
			set
			{
				ViewState["Prompt"] = value;
			}
		}

		#endregion

		#region Overrides

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (Prompt != string.Empty)
			{
				string js = string.Format("return (window.confirm('{0}'));", Prompt.Replace("'", "\'"));
				Attributes.Add("onClick", js);
			}
		}

		#endregion
	}
}