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
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public sealed class HelpHyperLink : HyperLink
	{
		#region Accessors

		public string HelpPage { get; set; }

		private static string HelpFolder
		{
			get
			{
				return string.Format("~/Brands/Brand_{0}/Help/", WebsiteBrandManager.GetBrand().BrandId);
			}
		}

		#endregion

		#region Constructor

		public HelpHyperLink()
		{
			EnableViewState = false;
		}

		#endregion

		#region Overrides

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Directory.Exists(HttpContext.Current.Server.MapPath(HelpFolder)))
				Visible = false;
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (!Page.ClientScript.IsClientScriptBlockRegistered("HelpHyperLinkScript"))
			{
				string script = @"function showHelp(page)
								{
									var base_url = '" + ResolveUrl(HelpFolder) + @"';
									var url = base_url + page;" + @"
									var width = 520;
									var height = 550;
									var leftPosition = (screen.width)?(screen.width-width)/2:100;
									var topPosition = (screen.height)?(screen.height-height)/2:100;
									var settings = 'width='+width+',height='+height+',top='+topPosition+',left='+leftPosition+',scrollbars=yes,location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=yes';
									var win = window.open(url, 'help', settings);
									win.focus();
								}";

				Page.ClientScript.RegisterClientScriptBlock(GetType(), "HelpHyperLinkScript", script, true);
			}

			NavigateUrl = "javascript:showHelp('" + HelpPage + "');";
		}

		#endregion
	}
}