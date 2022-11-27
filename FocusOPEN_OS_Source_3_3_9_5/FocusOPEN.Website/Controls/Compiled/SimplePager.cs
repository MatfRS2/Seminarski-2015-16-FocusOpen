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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FocusOPEN.Website.Controls
{
	/// <summary>
	/// Simple pager control
	/// </summary>
	public class SimplePager : BaseWebControl, IPostBackEventHandler, INamingContainer
	{
		#region PostBack Stuff

		private static readonly object EventCommand = new object();

		public event CommandEventHandler Command
		{
			add
			{
				Events.AddHandler(EventCommand, value);
			}
			remove
			{
				Events.RemoveHandler(EventCommand, value);
			}
		}

		protected virtual void OnCommand(CommandEventArgs e)
		{
			CommandEventHandler clickHandler = (CommandEventHandler) Events[EventCommand];
			if (clickHandler != null) clickHandler(this, e);
		}

        public void RaisePostBackEvent(int eventArgument)
        {
            OnCommand(new CommandEventArgs(UniqueID, eventArgument));
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            OnCommand(new CommandEventArgs(UniqueID, Convert.ToInt32(eventArgument)));
        }

		#endregion

		#region Accessors

		public int CurrentPage
		{
			get
			{
				return GetFromViewState("CurrentPage", 1);
			}
			set
			{
				ViewState["CurrentPage"] = value;
			}
		}

		public int ItemCount
		{
			get
			{
				return GetFromViewState("ItemCount", 0);
			}
			set
			{
				ViewState["ItemCount"] = value;
			}
		}

		public int PageSize
		{
			get
			{
				return GetFromViewState("PageSize", 15);
			}
			set
			{
				ViewState["PageSize"] = value;
			}
		}

		public int PageCount
		{
			get
			{
				return GetFromViewState("PageCount", 0);
			}
			set
			{
				ViewState["PageCount"] = value;
			}
		}

		public int MaxPages
		{
			get
			{
				return GetFromViewState("MaxPages", 10);
			}
			set
			{
				ViewState["MaxPages"] = value;
			}
		}

		#endregion

		#region Helper Methods

		public void CalculatePageCount()
		{
			if (PageSize <= 0)
				throw new Exception("PageSize must be greater than 0");

			if (ItemCount == 0)
			{
				PageCount = 0;
				return;
			}

			int remainder = ItemCount % PageSize;

			if (remainder == 0)
			{
				PageCount = (ItemCount / PageSize);
			}
			else
			{
				PageCount = ((ItemCount - remainder) / PageSize) + 1;
			}
		}

		private string RenderBack()
		{
			int index = CurrentPage - 1;

			const string tpl = "<a href=\"{0}\" class=\"PanelTxt\">&lt;&lt;</a>";
			return String.Format(tpl, Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString()));
		}

		private string RenderNext()
		{
			int index = CurrentPage + 1;

			const string tpl = "<a href=\"{0}\" class=\"PanelTxt\">&gt;&gt;</a>";
			return String.Format(tpl, Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString()));
		}

		private string RenderCurrent()
		{
			return "<span class=\"PanelTxt Bold\">" + CurrentPage + "</span>";
		}

		private string RenderOther(int index)
		{
			string tpl = "<a href=\"{0}\" class=\"PanelTxt\">" + index + "</a>";
			return String.Format(tpl, Page.ClientScript.GetPostBackClientHyperlink(this, index.ToString()));
		}

		#endregion

		#region Overrides

		protected override void Render(HtmlTextWriter writer)
		{
			if (Page != null)
				Page.VerifyRenderingInServerForm(this);

			// Don't show anything if we have no pages
			if (PageCount <= 1)
				return;

			// Show the previous page link if we're not on the first page
			if (CurrentPage != 1)
				writer.Write(RenderBack() + Environment.NewLine);

			// Assume starting from page 1 and ending on page 10
			int start = 1;
			int end = PageCount + 1;

			// Some calculation is required if the we have more pages than
			// we can display in one go (eg. if we want to show 10 MaxPages, and 
			// have 25 pages, then we want to manipulate which page numbers are shown)
			if (PageCount > MaxPages)
			{
				// Start from the current page, end at max pages
				decimal x = CurrentPage;
				decimal y = MaxPages;

				// If the current page is a multiple of max pages, then go back
				// Eg. Page 10 = 10-10+1 = start at page 1
				// Eg. Page 20 = 20-10+1 = start at page 11
				if (x%y == 0)
				{
					x = x - y + 1;
				}
				else
				{
					// Otherwise, loop back until we find the lowest page
					// Eg. If we're on page 14, loop back to find the lowest multiple of max pages (10)
					while (x%y != 0)
					{
						x--;
					}

					// Add one to it.  Eg. If we're on page 14, the lowest denominator will be 10
					// and the displayed pages should start from page 11.
					x++;
				}

				// Safety check... make sure we're not starting at 0
				if (x == 0)
					x = 1;

				// Get the start and end
				start = Convert.ToInt32(x);
				end = Convert.ToInt32(start + MaxPages);

				// Ensure that the end page doesn't exceed our page count
				if (end > PageCount + 1)
					end = PageCount + 1;

				// Show the last 10 [MaxPages] if possible
				// Eg. suppose we're on page 23 and have 24 pages, only 20-24 will be shown.
				// This will change it so that we see pages 14-24 instead.
				if (end - start < MaxPages)
					start = end - MaxPages;
			}

			// Display each page number, with a postback link if it's not the current page
			for (int i = start; i < end; i++)
			{
				if (i == CurrentPage)
				{
					writer.Write(RenderCurrent() + Environment.NewLine);
				}
				else
				{
					writer.Write(RenderOther(i) + Environment.NewLine);
				}

				if (i < PageCount)
				{
					writer.Write("<span class=\"PanelTxt\">|</span>" + Environment.NewLine);
				}
			}

			// Show the next page link if we're not at the end
			if (CurrentPage < PageCount)
				writer.Write(RenderNext() + Environment.NewLine);
		}

		#endregion
	}
}