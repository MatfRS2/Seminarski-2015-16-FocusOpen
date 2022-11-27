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
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FocusOPEN.Website.Controls
{
	public class PageSizeDropDownList : AbstractDictionaryDropDownList
	{
		public enum Modes
		{
			Admin,
			FrontEnd
		}

		#region Private variables

		private Modes m_Mode = Modes.FrontEnd;

		#endregion

		#region Accessors

		public Modes Mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
			}
		}

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			List<Int32> sizes = new List<int>();

			switch (Mode)
			{
				case Modes.FrontEnd:
#if DEBUG
					if (HttpContext.Current.Request.IsLocal)
						sizes.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7 });
#endif
					sizes.AddRange(new[] { 8, 16, 32, 64, 72 });
					break;

				case Modes.Admin:
#if DEBUG
					if (HttpContext.Current.Request.IsLocal)
						sizes.AddRange(new[] { 1, 2, 3, 4, 5, 1000 });
#endif
					sizes.AddRange(new[] { 10, 20, 40, 60, 80, 100, 250, 500 });
					break;
			}

			sizes.Sort();

			return sizes.ToDictionary(size => size, size => string.Format("{0} per page", size));
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			switch (Mode)
			{
				case Modes.FrontEnd:
					SelectedValue = "8";
					break;

				case Modes.Admin:
					SelectedValue = "100";
					break;
			}
		}
	}
}