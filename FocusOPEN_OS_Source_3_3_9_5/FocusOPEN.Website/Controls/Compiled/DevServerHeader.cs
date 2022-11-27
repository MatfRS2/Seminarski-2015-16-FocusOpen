/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FocusOPEN.Website.Controls
{
	public class DevServerHeader : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current.Request.UserHostAddress == "localhost")
			{
				string imagePath = ResolveUrl("~/Images/dev-background-tile.gif");
				string css = "<style type=\"text/css\">body { background-image: url('" + imagePath + "'); }</style>";

				writer.Write(css);
			}
		}
	}
}