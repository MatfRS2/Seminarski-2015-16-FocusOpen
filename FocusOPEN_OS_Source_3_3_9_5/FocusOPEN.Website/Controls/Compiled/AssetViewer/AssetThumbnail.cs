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
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using Image = System.Web.UI.WebControls.Image;

namespace FocusOPEN.Website.Controls
{
	public class AssetThumbnail : Image
	{
		#region Private variables

		private bool m_HighlightIfRestricted = true;
		private bool m_ImageLinksToAssetInfo = true;
		private int m_LineBreaks = 0;
		private string m_OtherCssClass = string.Empty;
		private bool m_ShowInfoOnHover = true;
		private bool m_UseMarginCssClass = true;

		#endregion

		#region Accessors

		public int LineBreaks
		{
			get
			{
				return m_LineBreaks;
			}
			set
			{
				m_LineBreaks = value;
			}
		}

		public bool ShowInfoOnHover
		{
			get
			{
				return m_ShowInfoOnHover;
			}
			set
			{
				m_ShowInfoOnHover = value;
			}
		}

		public bool HighlightIfRestricted
		{
			get
			{
				return m_HighlightIfRestricted;
			}
			set
			{
				m_HighlightIfRestricted = value;
			}
		}

		public bool ImageLinksToAssetInfo
		{
			get
			{
				return m_ImageLinksToAssetInfo;
			}
			set
			{
				m_ImageLinksToAssetInfo = value;
			}
		}

		public bool UseMarginCssClass
		{
			get
			{
				return m_UseMarginCssClass;
			}
			set
			{
				m_UseMarginCssClass = value;
			}
		}

		public string OtherCssClass
		{
			get
			{
				return m_OtherCssClass;
			}
			set
			{
				m_OtherCssClass = value;
			}
		}

		public string HoverCaption
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "HoverCaption", string.Empty);
			}
			set
			{
				ViewState["HoverCaption"] = value;
			}
		}

		public string HoverText
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "HoverText", string.Empty);
			}
			set
			{
				ViewState["HoverText"] = value;
			}
		}

		private string NavigateUrl
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "NavigateUrl", string.Empty);
			}
			set
			{
				ViewState["NavigateUrl"] = value;
			}
		}

		public bool UseCacheBuster
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "UseCacheBuster", false);
			}
			set
			{
				ViewState["UseCacheBuster"] = value;
			}
		}

		#endregion

		#region Overrides

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (!Page.ClientScript.IsClientScriptIncludeRegistered("overlib"))
				Page.ClientScript.RegisterClientScriptInclude("overlib", Page.ResolveClientUrl("~/Includes/Javascript/Overlib/overlib.js"));
		}

		protected override void Render(HtmlTextWriter writer)
		{
			bool renderLinkTag = !StringUtils.IsBlank(NavigateUrl);

			if (renderLinkTag)
			{
				writer.WriteBeginTag("a");
				writer.WriteAttribute("href", NavigateUrl);
				writer.Write(HtmlTextWriter.TagRightChar);
			}

			base.Render(writer);

			if (renderLinkTag)
				writer.WriteEndTag("a");

			for (int i = 0; i < LineBreaks; i++)
			{
				writer.WriteBreak();
			}
		}

		#endregion

		#region Private Helper Methods

		private void SetImageProperties(Asset asset)
		{
			ImageUrl = Page.ResolveClientUrl(AssetFileUrlHelper.GetThumbnailUrl(asset.AssetId.GetValueOrDefault()));

			if (UseCacheBuster)
				ImageUrl += string.Format("?cb={0}", DateTime.Now.Ticks);

			if (ImageLinksToAssetInfo)
				NavigateUrl = SiteUtils.GetShowAssetInfoUrl(asset.AssetId.GetValueOrDefault());

			if (ShowInfoOnHover)
			{
				string captionTitle = SiteUtils.GetAssetHoverCaption(asset, HoverCaption);
				string captionText = SiteUtils.GetAssetHoverText(asset, HoverText);

				if (!asset.IsPublishedAndAvailable())
				{
					captionText += "<br/><br/>";

					if (asset.PublishDate > DateTime.Now)
						captionText += "<strong style='color:red'>Asset publish date is in the future</strong><br/>";

					if (asset.ExpiryDate < DateTime.Now)
						captionText += "<strong style='color:red'>Asset has expired</strong><br/>";

					if (asset.AssetPublishStatus != AssetPublishStatus.Published)
						captionText += "<strong style='color:red'>Asset is not published</strong><br/>";
				}

				captionTitle = SiteUtils.MakeJsSafe(captionTitle).Replace("\\n", "<br/>");
				captionText = SiteUtils.MakeJsSafe(captionText).Replace("\\n", "<br/>");

				string onmouseover = string.Format("showToolTip('{0}', '{1}')", captionTitle, captionText);
				const string onmouseout = "hideToolTip();";

				Attributes.Add("onmouseover", onmouseover);
				Attributes.Add("onmouseout", onmouseout);
			}
		}

		private void SetCssClasses(Asset asset)
		{
			// We don't want to apply this CSS everywhere
			if (UseMarginCssClass)
				CssClass += " ImgThbMrg";

			if (!String.IsNullOrEmpty(OtherCssClass))
				CssClass += " " + OtherCssClass;

			bool highlighted = false;

			if (HighlightIfRestricted)
			{
				if (!asset.IsPublishedAndAvailable())
				{
					highlighted = true;
					BorderWidth = new Unit("3px");
					BorderColor = Color.DarkGray;
				}
				else if (EntitySecurityManager.IsAssetRestricted(SessionInfo.Current.User, asset))
				{
					highlighted = true;
					BorderWidth = new Unit("1px");
					CssClass += " ImgThbOutline0";
				}
			}

			if (highlighted)
			{
				if (BorderWidth.IsEmpty)
					BorderWidth = new Unit("2px");
			}
			else
			{
				BorderWidth = new Unit("1px");
				CssClass += " ImgThbOutline1";
			}

			// Strip double spaces
			CssClass = CssClass.Replace("  ", " ");

			// Trim additional whitespace
			CssClass = CssClass.Trim();
		}

		#endregion

		#region Public Methods

		public void Initialise(Asset asset)
		{
			// We don't want any alt text, as this pops up and clashes with the dhtml popup
			AlternateText = string.Empty;
			ToolTip = string.Empty;

			SetImageProperties(asset);
			SetCssClasses(asset);
		}

		#endregion
	}
}