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
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class BackToSearchHyperlink : HyperLink
	{
		#region Accessors

		public bool ShowRightDivider
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "ShowRightDivider", false);
			}
			set
			{
				ViewState["ShowRightDivider"] = value;
			}
		}

		#endregion

		#region Overrides

		protected override void OnInit(EventArgs e)
		{
			CssClass = "BodyTxt Bold H20";
			Text = "&laquo; Back To Search Results";
			NavigateUrl = "~/SearchResults.aspx";
			Visible = SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.HasSearch;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

			if (ShowRightDivider)
				writer.WriteLine(" <span class=\"BodyTxt\">|</span> ");
		}

		#endregion
	}
}