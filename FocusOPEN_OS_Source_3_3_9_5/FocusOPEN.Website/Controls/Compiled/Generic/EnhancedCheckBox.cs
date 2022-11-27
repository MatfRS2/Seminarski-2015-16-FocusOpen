/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class EnhancedCheckBox : CheckBox
	{
		#region Accessors

		public string Suffix
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "Suffix", string.Empty);
			}
			set
			{
				ViewState["Suffix"] = value;
			}
		}

		#endregion

		#region Overrides

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);

			if (Suffix != string.Empty)
				writer.Write(Suffix);
		}

		#endregion
	}
}