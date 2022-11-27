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
using Syrinx.Gui.AspNet;

namespace FocusOPEN.Website.Controls
{
	public class RichTextBox : CkEditor
	{
		public RichTextBox()
		{
			RemovePlugins = "save,preview,newpage,templates,forms,subscript,superscript";
			CanCollapseToolbar = false;
			ToolbarStartExpanded = true;
			Width = new Unit("100%");
			Height = new Unit("325px");
			Language = "en";
		}
	}
}