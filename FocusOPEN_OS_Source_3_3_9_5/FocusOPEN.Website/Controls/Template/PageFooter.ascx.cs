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
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public partial class PageFooter : BaseUserControl
	{
		public string HelpPage
		{
			get
			{
				return Footer1.HelpPage;
			}
			set
			{
				Footer1.HelpPage = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}
	}
}