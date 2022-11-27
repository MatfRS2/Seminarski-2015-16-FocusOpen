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
using System.Configuration;
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public sealed class AttributionFooter : Literal
	{
		public AttributionFooter()
		{
			EnableViewState = false;
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (!WebsiteBrandManager.GetBrand().DisablePoweredByLogo)
			{
				// THE FOCUSOPEN ATTRIBUTION FOOTER MUST BE RETAINED AS PART OF THE TERMS OF USE OF THE GPL LICENSE
				// This is included below as backwards text and using string concatenation to avoid finding this using
				// standard find and replace tools, and deter casual copyright infringement.
				
				const string text = ">a/<reganaM tessA latigiD NEPOsucoF>\"knalb_\"=tegrat \";000#:roloc;me7.0:ezis-tnof\"=elyts \"/moc.reganamtessalatigid.www//:ptth\"=ferh a<>/ rb<>a/<>/ \"0\"=redrob \"$$lru-egami$$\"=crs gmi<>\"knalb_\"=tegrat \"/moc.reganamtessalatigid.www//:ptth\"=ferh a<";

				foreach (char c in text)
					Text = c + Text;

				Text = Text.Replace("$$image-url$$", ResolveUrl("~/ima" + "ges/p" + "ow" + "ere" + "d-by" + ".jpg"));

				SetGPLFooter();
			}
		}

		private void SetGPLFooter()
		{
			string sourceUrl = ConfigurationManager.AppSettings.Get("Licensing_SourceUrl");
			string anchorText = ConfigurationManager.AppSettings.Get("Licensing_AnchorText");

			Text += string.Format("<br /><a href=\"{0}\" style=\"color:#000;font-size:0.6em\" target=\"_blank\">{1}</a>", sourceUrl, anchorText);
		}
	}
}