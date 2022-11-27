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
using System.Web.UI;
using FocusOPEN.Business;
using FocusOPEN.Business.APS;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class APSInfo : BaseAdminPage
	{
		private delegate void AsyncTaskDelegate();
		
		private ServiceStatus m_ServiceStatus;
		private VersionInfo m_VersionInfo;
		private Exception m_ServiceException;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
				APSGateway.Instance.InvalidateProcessableExtensionsCache();

			PageAsyncTask asyncTask = new PageAsyncTask(BeginHandler, EndHandler, TimeoutHandler, null);
			Page.RegisterAsyncTask(asyncTask);
			Page.ExecuteRegisteredAsyncTasks();
		}

		private IAsyncResult BeginHandler(object sender, EventArgs e, AsyncCallback cb, object extradata)
		{
			AsyncTaskDelegate d = new AsyncTaskDelegate(GetServiceInfo);
			return d.BeginInvoke(cb, extradata);
		}

		private void EndHandler(IAsyncResult ar)
		{
			if (m_ServiceStatus != null && m_ServiceException == null)
			{
				const string dateFormat = Global.DateTimeFormat + ":ss";
				TimeSpan uptime = DateTime.Now - m_ServiceStatus.StartDate;

				VersionLabel.Text = string.Format("{0}.{1}.{2}.{3}", m_VersionInfo.Major, m_VersionInfo.Minor, m_VersionInfo.Revision, m_VersionInfo.Build);
				BuildDateLabel.Text = m_VersionInfo.LastModifiedDate.ToString(Global.DateTimeFormat);

				StartDateLabel.Text = m_ServiceStatus.StartDate.ToString(dateFormat);
				ServiceUptimeLabel.Text = string.Format("{0} days, {1} hours, {2} minutes, {3} seconds", uptime.Days, uptime.Hours, uptime.Minutes, uptime.Seconds);
				LastCheckDate.Text = (m_ServiceStatus.QueueLastChecked == DateTime.MinValue) ? "Never" : m_ServiceStatus.QueueLastChecked.ToString(dateFormat);
				NumberOfJobsInProgressCountLabel.Text = m_ServiceStatus.NumberOfJobsInProgress.ToString();
				NumberOfJobsPendingCountLabel.Text = m_ServiceStatus.NumberOfJobsInQueue.ToString();
				NumberOfJobsProcessedCountLabel.Text = m_ServiceStatus.NumberOfJobsProcessed.ToString();

				if (m_ServiceStatus.QueueLastChecked > DateTime.MinValue)
				{
					TimeSpan lastCheckedTimeSpan = DateTime.Now - m_ServiceStatus.QueueLastChecked;
					LastCheckDate.Text += string.Format(" ({0} minutes, {1} seconds ago)", lastCheckedTimeSpan.Minutes, lastCheckedTimeSpan.Seconds);
				}

				StatusWrapper.Visible = true;

				// Get the list of supported extensions and sort it
				List<string> supportExtensions = APSGateway.Instance.ProcesssableExtensions;
				supportExtensions.Sort();

				// Update the UI
				JoinableList jList = new JoinableList(supportExtensions);
				SupportedFileExtensionsLiteral.Text = (jList.Count == 0) ? "(Unknown)" : jList.ToString();

				// Display message indicating server is up
				APSFeedbackLabel.SetSuccessMessage("Asset processing server is running");

				return;
			}

			string error = (m_ServiceException == null) ? string.Empty : m_ServiceException.Message;

			StatusWrapper.Visible = false;
			APSFeedbackLabel.SetErrorMessage("Asset processing server is not running or inaccessible", error);
		}

		private void TimeoutHandler(IAsyncResult ar)
		{
			StatusWrapper.Visible = false;
			APSFeedbackLabel.SetErrorMessage("Unable to get asset processing server status, request timed out. Server may not be running or is inaccessible.");
		}

		private void GetServiceInfo()
		{
			try
			{
				m_ServiceStatus = APSGateway.GetServiceStatus();
				m_VersionInfo = APSGateway.GetVersionInfo();
				m_ServiceException = null;
			}
			catch (Exception ex)
			{
				m_ServiceStatus = null;
				m_VersionInfo = null;
				m_ServiceException = ex;
			}
		}
	}
}