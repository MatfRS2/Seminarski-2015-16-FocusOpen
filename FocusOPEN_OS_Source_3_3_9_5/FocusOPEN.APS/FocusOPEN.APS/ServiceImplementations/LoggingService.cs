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
using System.ServiceModel;

namespace FocusOPEN.APS
{
	[ServiceBehavior(Namespace = "http://www.digitalassetmanager.com/FocusOPEN/APS/LoggingService/1.0/", InstanceContextMode = InstanceContextMode.Single)]
	public class LoggingService : BaseService, ILoggingService
	{
		#region Singleton

		private LoggingService m_Instance;

		private LoggingService()
		{
		}

		public LoggingService Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = new LoggingService();

				return m_Instance;
			}
		}

		#endregion

		public void AddLogEntry(int jobId, string message)
		{
			LogManager.AddLogEntry(jobId, message);
		}

		public List<LogEntry> GetLogEntries()
		{
			return LogManager.GetLogEntries();
		}

		public List<LogEntry> GetLogEntriesByJobId(int jobId)
		{
			return LogManager.GetLogEntriesByJobId(jobId);
		}

		public List<LogEntry> GetLogEntriesByAssetId(int assetId)
		{
			return LogManager.GetLogEntriesByAssetId(assetId);
		}

		public List<APSDebugLog> GetDebugLogEntries()
		{
			return LogManager.GetDebugLogEntries();
		}
	}
}