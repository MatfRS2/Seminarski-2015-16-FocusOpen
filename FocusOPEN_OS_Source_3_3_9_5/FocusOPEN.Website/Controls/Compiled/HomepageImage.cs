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
	public class HomepageImage : Image
	{
		#region Properties

		public int HomepageId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "HomepageId", 0);
			}
			set
			{
				ViewState["HomepageId"] = value;
			}
		}

		public int ImageNumber
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "ImageNumber", 0);
			}
			set
			{
				ViewState["ImageNumber"] = value;
			}
		}

		#endregion

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			ImageUrl = string.Format("~/Images/HomepageImage.{0}.ashx?image={1}", HomepageId, ImageNumber);
			base.Render(writer);
		}
	}
}
