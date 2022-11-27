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
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using log4net;

namespace FocusOPEN.APS.WinService
{
	public partial class APService : ServiceBase
	{
		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		readonly ServiceHost m_ServiceHost = new ServiceHost(typeof(ProcessingService));

		public APService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			try
			{
				m_ServiceHost.Open();
				m_Logger.DebugFormat("FocusOPEN Asset Processing Service started successfully");
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(string.Format("Error starting FocusOPEN Asset Processing Service: {0}\n\nStack Trace:\n{1}", ex.Message, ex), EventLogEntryType.FailureAudit);
				m_Logger.Fatal(string.Format("Error opening service: {0}", ex.Message), ex);
				
				throw new SystemException(string.Format("Error starting service: {0}. Check the event log for more details.", ex.Message));
			}
		}

		protected override void OnStop()
		{
			try
			{
				if (m_ServiceHost != null && m_ServiceHost.State != CommunicationState.Closed)
					m_ServiceHost.Close();

				m_Logger.InfoFormat("FocusOPEN Asset Processing Service stopped successfully");
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(string.Format("Error stopping FocusOPEN Asset Processing Service: {0}\n\nStack Trace:\n{1}", ex.Message, ex), EventLogEntryType.Warning);
				m_Logger.Fatal(string.Format("Error stopping service: {0}", ex.Message), ex);
			}
		}
	}
}