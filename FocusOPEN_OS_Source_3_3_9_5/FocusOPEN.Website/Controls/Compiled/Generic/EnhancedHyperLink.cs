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

namespace FocusOPEN.Website.Controls
{
	public class EnhancedHyperLink : HyperLink
	{
		#region Accessors

		public bool ShowLeftDivider { get; set; }

		public bool ShowRightDivider { get; set; }

		public string Prefix { get; set; }
		
		public string Suffix { get; set; }

		#endregion

		#region Overrides

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(Prefix))
				writer.Write(Prefix);

			if (ShowLeftDivider)
				writer.WriteLine(" <span class=\"BodyTxt\">|</span> ");

			base.Render(writer);

			if (ShowRightDivider)
				writer.WriteLine(" <span class=\"BodyTxt\">|</span> ");

			if (!string.IsNullOrEmpty(Suffix))
				writer.Write(Suffix);
		}

		#endregion
	}
}