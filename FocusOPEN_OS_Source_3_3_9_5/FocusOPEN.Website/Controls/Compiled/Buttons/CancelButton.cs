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
using System.Web;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Controls
{
	/// <summary>
	/// Subclassed PromptButton allowing us to attach a redirect url to the button.  When clicked,
	/// the prompt is displayed, and if the user continues, they are redirected to the specified url.
	/// </summary>
	public class CancelButton : PromptButton
	{
		#region Accessors

		public string RedirectUrl
		{
			get
			{
				if (ViewState["RedirectUrl"] == null)
					return string.Empty;

				string redirectUrl = ViewState["RedirectUrl"].ToString();

				if (redirectUrl.StartsWith("~/"))
					redirectUrl = ResolveUrl(redirectUrl);

				return redirectUrl;
			}
			set
			{
				ViewState["RedirectUrl"] = value;
			}
		}

		#endregion

		#region Overrides

		protected override void OnPreRender(EventArgs e)
		{
			if (!StringUtils.IsBlank(RedirectUrl))
			{
				if (!StringUtils.IsBlank(Prompt))
				{
					string js = string.Format("if (window.confirm('{0}')) window.location.href='{1}'; return false;", Prompt.Replace("'", "\\'"), RedirectUrl);
					Attributes.Add("onClick", js);
				}
				else
				{
					string js = string.Format("window.location.href='{0}'; return false;", RedirectUrl);
					Attributes.Add("onClick", js);
				}
			}
			else
			{
				Attributes.Remove("onClick");
			}
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);

			if (!StringUtils.IsBlank(RedirectUrl))
				HttpContext.Current.Response.Redirect(RedirectUrl, false);
		}

		#endregion
	}
}