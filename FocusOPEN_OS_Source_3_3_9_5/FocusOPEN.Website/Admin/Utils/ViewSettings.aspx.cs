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
using System.Configuration;
using System.Reflection;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;

namespace FocusOPEN.Website.Admin
{
	public partial class ViewSettings : BaseAdminPage
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected void Page_Load(object sender, EventArgs e)
		{
			GeneralFeedbackLabel.Visible = false;

			ServerNameLabel.Text = Environment.MachineName;

			if (!Page.IsPostBack)
			{
				// Get the settings list loaded into the config object, and sort it
				SortedList sortedSettingsList = new SortedList();

				for (int i = 0; i < ConfigurationManager.AppSettings.Count; i++)
				{
					string key = ConfigurationManager.AppSettings.GetKey(i);
					string rawVal = ConfigurationManager.AppSettings[i];
					string actualVal = ConfigurationManager.AppSettings.GetValue(key);

					if (rawVal != actualVal)
						rawVal += "<br />(Parsed value: " + actualVal + ")";

					sortedSettingsList.Add(key, rawVal);
				}

				// Remove the connection string for security reasons
				sortedSettingsList.Remove("ConnectionString");

				// Bind the settings to our grid
				SettingsDataGrid.DataSource = sortedSettingsList;
				SettingsDataGrid.DataBind();
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (WebUtils.GetRequestParam("msg") == "application_restarted")
				GeneralFeedbackLabel.SetSuccessMessage("Application restarted successfully");
		}

		protected void RestartApplicationButton_Click(object sender, EventArgs e)
		{
			m_Logger.InfoFormat("Application restart initiated by {0} on {1}", CurrentUser.FullName, DateTime.Now.ToString(Global.DateTimeFormat));
			System.Web.HttpRuntime.UnloadAppDomain();
			Response.Redirect("~/Admin/Utils/ViewSettings.aspx?msg=application_restarted");
		}

		protected void ReloadSettingsButton_Click(object sender, EventArgs e)
		{
			Global.LoadConfigSettings();
			GeneralFeedbackLabel.SetSuccessMessage("Settings reloaded successfully");
		}
	}
}