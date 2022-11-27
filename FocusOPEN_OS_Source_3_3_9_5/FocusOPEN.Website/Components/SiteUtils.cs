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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	public static class SiteUtils
	{
		#region Control Helper Methods

		/// <summary>
		/// Recursive FindControl method, to search a control and all child
		/// controls for a control with the specified ID.
		/// 
		/// http://www.codinghorror.com/blog/archives/000307.html
		/// </summary>
		/// <param name="root">The root control where search should begin.  This will usually be the Page control</param>
		/// <param name="id">The ID of the control to look for</param>
		/// <returns>Control if found or null</returns>
		public static Control FindControlRecursive(Control root, string id)
		{
			if (string.IsNullOrEmpty(id))
				return null;

			if (root.ID == id)
				return root;

			return (from Control c in root.Controls
				select FindControlRecursive(c, id)).FirstOrDefault(t => t != null);
		}

		/// <summary>
		/// Recursuve FindControl method, to search control and go upwards
		/// searching all children until it finds a control with the specified id.
		/// </summary>
		/// <param name="child">The control where the search should begin.  This will typically be 'this' when called from within a control</param>
		/// <param name="id">The ID of the control to look for</param>
		public static Control FindControlRecursiveUp(Control child, string id)
		{
			if (string.IsNullOrEmpty(id))
				return null;

			if (child.ID == id)
				return child;

			Control control = child;

			while (control != null)
			{
				Control c = FindControlRecursive(control, id);

				if (c != null)
					return c;

				control = control.Parent;
			}

			return null;
		}

		#endregion

		#region Data display helper methods

		/// <summary>
		/// Formats seconds into a friendly (descriptive) string
		/// </summary>
		public static string FriendlyDuration(double seconds, string valueIfBlank)
		{
			if (seconds == 0)
				return valueIfBlank;

			TimeSpan ts = TimeSpan.FromSeconds(seconds);
			return string.Format("{0} hrs {1} mins {2} secs", ts.Hours, ts.Minutes, ts.Seconds);
		}

		#endregion

		#region PopulateBlankControl Methods

		/// <summary>
		/// Populates the control's innerHtml property with a non-breaking space if it has no content
		/// </summary>
		public static void PopulateBlankControl(HtmlContainerControl control)
		{
			if (StringUtils.IsBlank(control.InnerText))
				control.InnerHtml = "&nbsp;";
		}

		/// <summary>
		/// Populates the control's Text property with a non-breaking space if it has no content
		/// </summary>
		public static void PopulateBlankControl(ITextControl control)
		{
			if (StringUtils.IsBlank(control.Text))
				control.Text = "&nbsp;";
		}

		#endregion

		#region AddValidateUploadClientSideCheck Methods

		public static void AddValidateUploadClientSideCheck(Button button, Control uploadControl, AssetType assetType, bool isPending)
		{
			string js = string.Format("return validateUpload('{0}', '', '', '{1}', {2})", uploadControl.ClientID, assetType.FileExtensionList.ToDelimitedList(';'), isPending.ToString().ToLower());
			button.Attributes["onClick"] = js;
		}

		#endregion

		#region Text Parsing Methods

		public static string ConvertTextToHtml(string text)
		{
			return ConvertTextToHtml(text, string.Empty);
		}

		public static string ConvertTextToHtml(string text, string linkCssClass)
		{
			string s = text;

			// Linkify URL's
			Regex urlregex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			s = urlregex.Replace(s, string.Format("<a class=\"{0}\" href=\"$1\" target=\"_blank\">$1</a>", linkCssClass));

			// Linkify email addresses
			Regex emailregex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			s = emailregex.Replace(s, string.Format("<a class=\"{0}\" href=\"mailto:$1\">$1</a>", linkCssClass));

			// Convert line breaks
			s = s.Replace("\n", "<br />");

			// All done
			return s;
		}

		#endregion

		#region TieButton Methods

		//---------------------------------------------------------------------
		// Taken from
		// http://www.allasp.net/enterkey.aspx
		//---------------------------------------------------------------------


		/// <summary>
		///     This ties a textbox to a button. 
		/// </summary>
		/// <param name="TextBoxToTie">
		///     This is the textbox to tie to. It doesn't have to be a TextBox control, but must be derived from either HtmlControl or WebControl,
		///     and the html control should accept an 'onkeydown' attribute.
		/// </param>
		/// <param name="ButtonToTie">
		///     This is the button to tie to. All we need from this is it's ClientID. The Html tag it renders should support click()
		/// </param>
		public static void TieButton(Control TextBoxToTie, Control ButtonToTie)
		{
			string formName;
			try
			{
				int i = 0;
				Control c = ButtonToTie.Parent;
				// Step up the control hierarchy until either:
				// 1) We find an HtmlForm control
				// 2) We find a Page control - not what we want, but we should stop searching because we a Page will be higher than the HtmlForm.
				// 3) We complete 500 iterations. Obviously we are in a loop, and should stop.
				while (!(c is HtmlForm) & !(c is Page) && i < 500)
				{
					c = c.Parent;
					i++;
				}
				// If we have found an HtmlForm, we use it's ClientID for the formName.
				// If not, we use the first form on the page ("forms[0]").
				if (c is HtmlForm)
					formName = c.ClientID;
				else
					formName = "forms[0]";
			}
			catch
			{
				//If we catch an exception, we should use the first form on the page ("forms[0]").
				formName = "forms[0]";
			}
			// Tie the button.
			TieButton(TextBoxToTie, ButtonToTie, formName);
		}

		/// <summary>
		///     This ties a textbox to a button. 
		/// </summary>
		/// <param name="TextBoxToTie">
		///     This is the textbox to tie to. It doesn't have to be a TextBox control, but must be derived from either HtmlControl or WebControl,
		///     and the html control should accept an 'onkeydown' attribute.
		/// </param>
		/// <param name="ButtonToTie">
		///     This is the button to tie to. All we need from this is it's ClientID. The Html tag it renders should support click()
		/// </param>
		/// <param name="formName">
		///     This is the ClientID of the form that the button resides in.
		/// </param>
		private static void TieButton(Control TextBoxToTie, Control ButtonToTie, string formName)
		{
			// This is our javascript - we fire the client-side click event of the button if the enter key is pressed.
			string jsString = "if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {document." + formName + ".elements['" + ButtonToTie.UniqueID + "'].click();return false;} else return true; ";

			// We attach this to the onkeydown attribute - we have to cater for HtmlControl or WebControl.
			if (TextBoxToTie is HtmlControl)
				((HtmlControl)TextBoxToTie).Attributes["onKeyDown"] = jsString;
			else if (TextBoxToTie is WebControl)
				((WebControl)TextBoxToTie).Attributes["onkeydown"] = jsString;
			else
			{
				// We throw an exception if TextBoxToTie is not of type HtmlControl or WebControl.
				throw new ArgumentException("Control TextBoxToTie should be derived from either System.Web.UI.HtmlControls.HtmlControl or System.Web.UI.WebControls.WebControl", "TextBoxToTie");
			}
		}

		#endregion

		#region GetBrandName Methods

		public static string GetBrandName(User user)
		{
			if (!user.PrimaryBrand.IsNull)
				return user.PrimaryBrand.Name;

			return "None";
		}

		#endregion

		public static string GetAssetHoverCaption(Asset asset, string fixedCaption)
		{
			if (!StringUtils.IsBlank(fixedCaption))
				return fixedCaption;

			return asset.Description;
		}

		public static string GetAssetHoverText(Asset asset, string fixedText)
		{
			if (!StringUtils.IsBlank(fixedText))
				return fixedText;

			return asset.UsageRestrictions;
		}

		public static string GetIconPath(string filename)
		{
			int brandId = WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();

			if (brandId == 0)
				brandId = WebsiteBrandManager.GetMasterBrand().BrandId.GetValueOrDefault();

			string path = string.Format("~/Brands/Brand_{0}/UI/Images/Icons/{1}", brandId, filename);
			string absPath = HttpContext.Current.Server.MapPath(path);

			if (!File.Exists(absPath))
				path = "~/Images/Icons/" + filename;

			return path;
		}

		public static string GetFileTypeImageUrl(string fileExtension)
		{
			return string.Format("~/FileIcon.{0}.ashx", fileExtension);
		}

		public static void SetHyperLink(string adminSectionPageId, string querystring, HyperLink hyperlink)
		{
			AdminSectionPage adminSectionPage = AdminNavigationManager.GetAdminSectionPageById(adminSectionPageId);
			string url = (adminSectionPage == null) ? "#" : string.Format("{0}{1}", adminSectionPage.Url, querystring);
			hyperlink.NavigateUrl = url;
		}

		public static void SetHyperLink(User user, string adminSectionPageId, string querystring, HyperLink hyperlink)
		{
			AdminSectionPage adminSectionPage = AdminNavigationManager.GetAdminSectionPageById(adminSectionPageId);

			if (adminSectionPage == null)
			{
				hyperlink.Visible = false;
			}
			else
			{
				hyperlink.Visible = SecurityManager.UserHasAccess(user, adminSectionPage);
				hyperlink.NavigateUrl = string.Format("{0}{1}", adminSectionPage.Url, querystring);
			}
		}

		public static void PopulateCell(HtmlTableCell cell, int assetNumber, int totalNumber)
		{
			double x = Convert.ToDouble(assetNumber);
			double y = Convert.ToDouble(totalNumber);
			double percent = 0;

			if (x == 0 && y == 0)
			{
				percent = 0;
			}
			else if (x > 0 && y > 0)
			{
				percent = Math.Round(x / y * 100);
			}

			cell.InnerText = string.Format("{0}/{1} ({2}%)", x, y, percent);
		}

		public static string GetShowAssetInfoUrl(int assetId)
		{
			return GetShowAssetInfoUrl(assetId, true);
		}

		public static string GetShowAssetInfoUrl(int assetId, bool includeJsPrefix)
		{
			bool returnWindow = !includeJsPrefix;

			StringBuilder sb = new StringBuilder();

			if (includeJsPrefix)
				sb.Append("javascript:");

			sb.Append("showAssetInfo(");
			sb.Append(assetId);
			sb.Append(",");
			sb.Append(returnWindow.ToString().ToLower());
			sb.Append(");");

			return sb.ToString();
		}

		public static string GetFullCategoryName(Category category)
		{
			// Initialize with category name
			string name = category.Name;

			// Get parent
			Category parent = category.ParentCategory;

			// Loop through parents, adding them before the name
			// Eg. Parent >> Child >> Child >> This Category
			while (parent != null && !parent.IsNull)
			{
				if (!parent.IsNull)
					name = parent.Name + " / " + name;

				parent = parent.ParentCategory;
			}

			return name;
		}

		internal static void SaveUploadedAssetListToCurrentUser()
		{
            // Create new list to hold asset ID's
            List<Int32> list = new List<Int32>();

            // Strip out duplicates
            foreach (int i in SessionInfo.Current.AdminSessionInfo.UploadedAssetsList)
                if (!list.Contains(i))
                    list.Add(i);

            // Convert into comma-delimited string
            JoinableList jList = new JoinableList(list, ",");

            // Update session
            SessionInfo.Current.User.CatalogueAssetIdList = jList.ToString();
            SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.Clear();
            SessionInfo.Current.AdminSessionInfo.UploadedAssetsList.AddRange(list);

            // Save the user
            User.Update(SessionInfo.Current.User);
		}

		/// <summary>
		/// Prevents the page being retrieved from broswer cache
		/// </summary>
		public static void ExpirePageCache()
		{
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			HttpContext.Current.Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
			HttpContext.Current.Response.Cache.SetLastModified(DateTime.Now);
			HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
			HttpContext.Current.Response.AddHeader("pragma", "no-cache");
			HttpContext.Current.Response.AddHeader("cache-control", "private");
			HttpContext.Current.Response.CacheControl = "no-cache";
		}

		/// <summary>
		/// Makes a string safe for use in javascript
		/// </summary>
		public static string MakeJsSafe(string s)
		{
			string newString = s;

			// Escape apostrophes
			newString = newString.Replace("'", "\\'");

			// Replace line breaks
			newString = newString.Replace(Environment.NewLine, "\\n");

			return newString;
		}

		/// <summary>
		/// Returns the specified path with website url
		/// </summary>
		/// <param name="path">Path</param>
		/// <returns>Website url with path appended</returns>
		public static string GetWebsiteUrl(string path)
		{
			// Remove the relative path
			if (path.StartsWith("~/"))
				path = path.Substring(2);

			// Get the website URL for the current brand, ensuring it ends with a forward slash
			string websiteUrl = WebsiteBrandManager.GetBrand().WebsiteUrl.EnsureEndsWith("/");

			// Get the application path
			string applicationPath = WebUtils.GetApplicationPath();

			// Add it to the website URL if required
			if (!websiteUrl.ToLower().EndsWith(applicationPath.ToLower()))
				websiteUrl += applicationPath;

			// Ensure it has a trailing slash
			websiteUrl = VirtualPathUtility.AppendTrailingSlash(websiteUrl);

			// Combine the two and return
			return websiteUrl + path;
		}

		/// <summary>
		/// Gets the value from the store using the specified key.  If it does not exist
		/// the default value is returned.  The value is automatically cast to the that
		/// of the default value.
		/// </summary>
		/// <typeparam name="T">Type of value to be returned</typeparam>
		/// <param name="store">The store where the key should be retrieved from (eg. ViewState)</param>
		/// <param name="key">The item key</param>
		/// <param name="defaultVal">Default value to return if item does not exist</param>
		/// <returns>Value of specified key in store if found, otherwise default value</returns>
		public static T GetFromStore<T>(IDictionary store, string key, T defaultVal)
		{
			if (store[key] != null)
				return (T) store[key];

			return defaultVal;
		}

		/// <summary>
		/// Gets the date range for the specified timeframe
		/// </summary>
		public static DateRange GetDateRangeFromTimeFrame(TimeFrame timeframe)
		{
			DateTime? startDate;
			DateTime? endDate;

			switch (timeframe)
			{
				case (TimeFrame.SinceUpload):

					startDate = null;
					endDate = null;
					break;

				case (TimeFrame.LastMonth):

					startDate = DateTime.Now.AddMonths(-1);
					endDate = null;

					break;

				case (TimeFrame.Last3Months):

					startDate = DateTime.Now.AddMonths(-3);
					endDate = null;

					break;

				case (TimeFrame.Last6Months):

					startDate = DateTime.Now.AddMonths(-6);
					endDate = null;

					break;

				case (TimeFrame.Last12Months):

					startDate = DateTime.Now.AddMonths(-12);
					endDate = null;

					break;

				default:

					throw new SystemException("Unknown timeframe: " + timeframe);
			}

			return DateRange.Get(startDate, endDate);
		}

		public static void SendAttachedFile(int assetFileId)
		{
			// Ensure we have an asset file ID
			if (assetFileId == 0)
				return;

			// Get the asset file
			AssetFile assetFile = AssetFile.Get(assetFileId);

			// Ensure we have an asset file
			if (assetFile.IsNull)
				return;

			// Download filename
			SendAttachedFile(assetFile);
		}

		public static void SendAttachedFile(AssetFile assetFile)
		{
			SendFile(assetFile.Filename, assetFile.FileContent, true);
		}

		public static void SendFile(string filename, byte[] bytes, bool forceDownload)
		{
			string downloadFilename = filename;

			// Workaround for problem in IE6 where it appends [1] onto filenames containing periods
			if (HttpContext.Current.Request.Browser.Browser == "IE")
			{
				string f = (Path.GetFileNameWithoutExtension(downloadFilename) ?? string.Empty);
				string f2 = f.Replace(".", "%2e");
				downloadFilename = downloadFilename.Replace(f, f2);
			}

			// Wrap it in quotes to keep spaces
			downloadFilename = string.Format("\"{0}\"", downloadFilename);

			// Set content type and disposition
			string contentType = MimeTypeManager.GetMimeType(Path.GetExtension(filename));
			string contentDisposition = string.Format("filename={0}", downloadFilename);

			if (forceDownload)
				contentDisposition = "attachment;" + contentDisposition;

			// Set the headers
			HttpContext.Current.Response.ContentType = contentType;
			HttpContext.Current.Response.AppendHeader("Content-Disposition", contentDisposition);
			HttpContext.Current.Response.AppendHeader("Content-Length", bytes.Length.ToString());

			// Send the file
			if (HttpContext.Current.Response.IsClientConnected)
				HttpContext.Current.Response.BinaryWrite(bytes);

			HttpContext.Current.Response.End();
		}
	}
}