/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Controls
{
	public class DropDownMenu : DropDownMenuBase
	{
		public DropDownMenu()
		{
			Items = new DropDownMenuItemCollection(this);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.WriteBeginTag("ul");
			writer.WriteAttribute("class", "dropdown");
			writer.Write(HtmlTextWriter.TagRightChar);

			RenderDropDownMenuItems(Items, writer);

			writer.WriteEndTag("ul");
		}

		private void RenderDropDownMenuItems(DropDownMenuItemCollection items, HtmlTextWriter writer)
		{
			foreach (DropDownMenuItem item in items.FindAll(item => item.Visible))
			{
				string url = GeneralUtils.GetNonEmptyString(item.NavigateUrl, "#");

				if (url != "#" && !url.EndsWith("/") && !url.Contains("?"))
				{
					string path = HttpContext.Current.Server.MapPath(url);
					url = (File.Exists(path)) ? ResolveUrl(url) : "#";
                }
                else if (url.Contains("?"))
                {
                    url = ResolveUrl(url);
                }

				writer.WriteBeginTag("li");
				if (!string.IsNullOrEmpty(item.CssClass))
					writer.WriteAttribute("class", item.CssClass);
				writer.Write(HtmlTextWriter.TagRightChar);

				writer.WriteBeginTag("a");
				writer.WriteAttribute("href", url);
				writer.Write(HtmlTextWriter.TagRightChar);
				writer.Write(item.Text);
				writer.WriteEndTag("a");

				if (item.Items.Count > 0)
				{
					writer.WriteBeginTag("ul");
					writer.WriteAttribute("class", "sub_menu");
					writer.Write(HtmlTextWriter.TagRightChar);

					RenderDropDownMenuItems(item.Items, writer);

					writer.WriteEndTag("ul");
				}

				writer.WriteEndTag("li");
			}
		}
	}

	public sealed class DropDownMenuItem : DropDownMenuBase
	{
		public string Text { get; set; }
		public string NavigateUrl { get; set; }
		public string CssClass { get; set; }

		public DropDownMenuItem()
		{
			Items = new DropDownMenuItemCollection(this);
			Visible = true;
		}

		private DropDownMenuItem(string text) : this()
		{
			Text = text;
		}

		public DropDownMenuItem(string text, string navigateUrl) : this(text)
		{
			NavigateUrl = navigateUrl;
		}
	}

	public class DropDownMenuItemCollection : List<DropDownMenuItem>
	{
		private IDropDownMenuItemContainer Owner { get; set;}

		public DropDownMenuItemCollection(IDropDownMenuItemContainer owner)
		{
			Owner = owner;
		}

		new public void Add(DropDownMenuItem item)
		{
			item.Owner = Owner;
			base.Add(item);
		}
	}

	public interface IDropDownMenuItemContainer
	{
		IDropDownMenuItemContainer Owner { get;}
	}

	public abstract class DropDownMenuBase : Control, IDropDownMenuItemContainer
	{
		public DropDownMenuItemCollection Items { get; protected set; }
		public IDropDownMenuItemContainer Owner { get; set; }
	}
}