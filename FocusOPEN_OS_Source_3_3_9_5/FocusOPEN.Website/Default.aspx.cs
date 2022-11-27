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
using System.Text;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website
{
	public partial class Default : BaseSecuredPage
	{
		private const string COOKIE_KEY = "ViewedBumperPages";

		#region Handled Page Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (WebUtils.GetIntRequestParam("ClearBumperCookie", 0) == 1)
					CookieManager.ClearValue(COOKIE_KEY);

				// Initialize to empty homepage
				Homepage homepage = Homepage.Empty;

				// Check if we're previewing a homepage
				int previewId = WebUtils.GetIntRequestParam("PreviewHomepageId", 0);

				// Get the preview homepage if required
				if (previewId > 0)
					homepage = Homepage.Get(previewId);

				// Get the current homepage if required
				if (homepage.IsNull)
					homepage = HomepageManager.GetCurrentHomepage(CurrentBrandId);

				// Previewed homepages must use the brand of the homepage
				if (!homepage.IsPublished)
					BrandHeader1.BrandId = homepage.BrandId;

				// Preview bar only available for unpublished homepages
				PreviewToolbarPanel.Visible = !homepage.IsPublished;

				// Check if we need to show the bumper page
				bool showBumper = CheckShowBumper(homepage);

				if (showBumper)
				{
					// Show the custom bumper page
					ShowCustomHtmlWithoutNav(homepage.BumperPageHtml);

					// Check if the skip option is enabled, in which case
					// we need to add this homepage to the cookie so that
					// subsequent visits to the homepage bypass the bumper
					// page and take the user straight to the real homepage.

					if (homepage.IsPublished && homepage.BumperPageSkip)
					{
						// Get currently seen bumper pages
						string val = CookieManager.GetValue(COOKIE_KEY);

						// Value to append
						string av = "|" + homepage.HomepageId + "|";

						// Only add val if it's not already set
						if (!val.Contains(av))
						{
							// Append this one
							val += av;

							// Replace double pipes
							val = val.Replace("||", "|");

							// Set the new bumper cookie value
							CookieManager.SetValue(COOKIE_KEY, val);
						}
					}
				}
				else
				{
					int homepageTypeId = WebUtils.GetIntRequestParam("HomepageTypeId", homepage.HomepageTypeId);

					if (homepageTypeId == 1)
					{
						// Standard homepage

						IntroTextLiteral.Text = homepage.IntroText;
						HomepageImage1.HomepageId = homepage.HomepageId.GetValueOrDefault();
						HomepageImage2.HomepageId = homepage.HomepageId.GetValueOrDefault();
						HomepageImage3.HomepageId = homepage.HomepageId.GetValueOrDefault();
						HomepageImage4.HomepageId = homepage.HomepageId.GetValueOrDefault();

						QuickLink1HyperLink.NavigateUrl = homepage.Url1;
						QuickLink2HyperLink.NavigateUrl = homepage.Url2;
						QuickLink3HyperLink.NavigateUrl = homepage.Url3;
						QuickLink4HyperLink.NavigateUrl = homepage.Url4;

						FeaturedCategoriesRepeater.Visible = (homepage.CategoryList.Count > 0);
						FeaturedCategoriesRepeater.DataSource = homepage.CategoryList;
						FeaturedCategoriesRepeater.DataBind();

						StandardTemplatePageContentWrapper.Visible = true;
						CustomHtmlPageContentWrapper_NoNav.Visible = false;
						CustomHtmlPageContentWrapper_WithNav.Visible = false;
					}
					else if (homepageTypeId == 2)
					{
						// Custom HTML, No Navigation

						ShowCustomHtmlWithoutNav(homepage.CustomHtml);
					}
					else if (homepageTypeId == 3)
					{
						// Custom HTML, With Navigation

						StandardTemplatePageContentWrapper.Visible = false;
						CustomHtmlPageContentWrapper_NoNav.Visible = false;
						CustomHtmlPageContentWrapper_WithNav.Visible = true;
						WithNav_HtmlContentLiteral.Text = homepage.CustomHtml;
					}
					else
					{
						throw new SystemException("Unknown homepage type: " + homepage.HomepageTypeId);
					}
				}

				CheckAction();
			}
		}

		protected void FeaturedCategoriesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:

					Category category = (Category) e.Item.DataItem;

					HyperLink FeaturedCategoryHyperLink = (HyperLink) e.Item.FindControl("FeaturedCategoryHyperLink");

					FeaturedCategoryHyperLink.Text = string.Concat(category.Name, " (", category.AvailableAssetCount, ")");
					FeaturedCategoryHyperLink.NavigateUrl = "~/CategoryRedirector.ashx?CategoryId=" + category.CategoryId;

					break;
			}
		}

		#endregion

		#region Private Methods

		private void ShowCustomHtmlWithoutNav(string html)
		{
			StandardTemplatePageContentWrapper.Visible = false;
			CustomHtmlPageContentWrapper_NoNav.Visible = true;
			CustomHtmlPageContentWrapper_WithNav.Visible = false;
			NoNav_HtmlContentLiteral.Text = html;
		}

		private static bool CheckShowBumper(Homepage homepage)
		{
			// Can't show bumper page is none is available
			if (!homepage.HasBumperPage)
				return false;

			// Show bumper if querystring value is set
			if (WebUtils.GetIntRequestParam("ShowBumper", 0) == 1)
				return true;

			// Show bumper if querystring value is set
			if (WebUtils.GetIntRequestParam("HideBumper", 0) == 1)
				return false;

			// Don't show bumper if it's been seen
			if (CookieManager.GetValue(COOKIE_KEY, string.Empty).Contains("|" + homepage.HomepageId + "|"))
				return false;

			return true;
		}

		private void CheckAction()
		{
			string action = WebUtils.GetRequestParam("action", string.Empty).ToLower();

			if (!StringUtils.IsBlank(action))
			{
				int assetId = WebUtils.GetIntRequestParam("assetId", 0);

				if (assetId > 0)
				{
					StringBuilder sb = new StringBuilder();

					switch (action)
					{
						case "detail":

							const string message = "Error displaying asset info. If you have a popup blocker, please disable it for this domain.";

							sb.Append("var w = ");
							sb.Append(SiteUtils.GetShowAssetInfoUrl(assetId, false));
							sb.Append(";");
							sb.AppendLine();
							sb.AppendFormat("if (!w) alert ('{0}');", SiteUtils.MakeJsSafe(message));

							Page.ClientScript.RegisterStartupScript(Page.GetType(), "showAssetDetail", sb.ToString(), true);

							break;
					}
				}
			}
		}

		#endregion
	}
}