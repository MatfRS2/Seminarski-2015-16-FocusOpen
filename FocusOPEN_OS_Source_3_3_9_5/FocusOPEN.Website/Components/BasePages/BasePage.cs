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
using System.Web;
using System.Web.UI;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// The base class of all pages in the website.  Only has some script to focus on the first empty
	/// textbox at the moment, but could be extended for things like SSL support, etc.
	/// </summary>
	public abstract class BasePage : Page
	{
		protected enum PersisterType
		{
			Standard,
			ZipCompressed,
			Session
		}

		#region Private Variables

		private PageStatePersister m_PageStatePersister;
		private PersisterType m_PageStatePersisterType = PersisterType.Standard;

		#endregion

		#region Accessors

		protected static int CurrentBrandId
		{
			get
			{
				return WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();
			}
		}

		protected PersisterType PageStatePersisterType
		{
			get
			{
				return m_PageStatePersisterType;
			}
			set
			{
				m_PageStatePersisterType = value;
			}
		}

		#endregion

		#region Overridden Accessors

		protected override PageStatePersister PageStatePersister
		{
			get
			{
				if (m_PageStatePersister == null)
				{
					switch (PageStatePersisterType)
					{
						case (PersisterType.Standard):
							m_PageStatePersister = base.PageStatePersister;
							break;

						case (PersisterType.Session):
							m_PageStatePersister = new SessionPageStatePersister(this);
							break;

						case (PersisterType.ZipCompressed):
							m_PageStatePersister = new ViewStateCompressor(this);
							break;

						default:
							throw new Exception(string.Format("Unknown PersisterType: {0}", PageStatePersisterType));
					}
				}
				return m_PageStatePersister;
			}
		}

		#endregion

		#region Overridden Methods

		protected override void OnLoad(EventArgs e)
		{
			SiteUtils.ExpirePageCache();
			base.OnLoad(e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			// Setup app root
			string appRootScript = string.Format("APP_ROOT='{0}';", VirtualPathUtility.AppendTrailingSlash(VirtualPathUtility.ToAbsolute("~/")));
			ClientScript.RegisterStartupScript(GetType(), "AppRootScript", appRootScript, true);

            //add get API SessionToken javascript routine
            string getAPISessionToken = string.Format("function getAPISessionToken(){{return '{0}';}}",SessionInfo.Current.User.SessionAPIToken);
            ClientScript.RegisterClientScriptBlock(GetType(), "APISessionToken", getAPISessionToken, true);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			// Required on all pages as part of license.
			// Removal is a breach of conditions of use
			// Using string concat to avoid being found easily with find and replace

			string relativePath = VirtualPathUtility.ToAppRelative(Request.Url.AbsolutePath).ToLower();

			if (relativePath != "~/default.aspx")
			{
				if (SiteUtils.FindControlRecursive(Page, "Attr" + "ibu" + "tio" + "nFo" + "oter") == null)
				{
					writer.Write("Att" + "ribu" + "tio" + "n Fo" + "ote" + "r mus" + "t be" + " on a" + "ll pa" + "ges");
					return;
				}
			}

			base.Render(writer);
		}

		#endregion
	}
}