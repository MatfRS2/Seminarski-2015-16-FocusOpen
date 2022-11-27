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
using System.Linq;
using System.Web;
using System.Web.UI;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	/// <summary>
	/// The Brand Header control ensures that the correct skin is referenced based on the URL
	/// that is being used to access the web application.
	/// </summary>
	public class BrandHeader : Control
	{
		#region Accessors

		public string Css { get; set; }

		private static HttpContext CurrentContext
		{
			get
			{
				return HttpContext.Current;
			}
		}

		public int BrandId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "BrandId", 0);
			}
			set
			{
				ViewState["BrandId"] = value;
			}
		}

		#endregion

		protected override void Render(HtmlTextWriter writer)
		{
			// Don't do anything if no CSS has been specified
			if (StringUtils.IsBlank(Css))
				return;

			int brandId = WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();

			// Use the brand ID if one has specified
			if (BrandId > 0)
			{
				Brand brand = WebsiteBrandManager.GetBrandById(BrandId);

				if (brand != null)
					brandId = brand.BrandId.GetValueOrDefault();
			}

			if (brandId == 0)
			{
				brandId = WebsiteBrandManager.GetMasterBrand().BrandId.GetValueOrDefault();
				writer.WriteLine("<!-- SKIN FOLDER NOT SPECIFIED. USING MASTER BRAND INSTEAD -->");
			}

			// List of CSS files
			string[] cssFiles = Css.Split('|');

			// Check that the specified files exist and load these into a list
			List<string> list = (from cssFile in cssFiles
			                     select string.Format("~/Brands/Brand_{0}/UI/{1}.css", brandId, cssFile)
			                     into relativePath let absolutePath = CurrentContext.Server.MapPath(relativePath)
								 where File.Exists(absolutePath)
								 select relativePath).ToList();

			// Render the link tags for the CSS files
			foreach (string s in list)
			{
				writer.AddAttribute("rel", "stylesheet");
				writer.AddAttribute("type", "text/css");
				writer.AddAttribute("href", ResolveUrl(s));
				writer.RenderBeginTag("link");
			}
		}
	}
}
