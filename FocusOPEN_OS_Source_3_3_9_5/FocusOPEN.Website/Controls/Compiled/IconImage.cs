/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class IconImage : Image
	{
		public string Filename { get; set; }

		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);

			Width = new Unit("14px");
			Height = new Unit("14px");
		}

		protected override void OnLoad(System.EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				ImageUrl = SiteUtils.GetIconPath(Filename);
			}
		}
	}
}