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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Xml.Linq;
using log4net;

namespace FocusOPEN.APS
{
	[ServiceBehavior(Namespace = "http://www.digitalassetmanager.com/FocusOPEN/APS/ProcessingService/1.0/", InstanceContextMode = InstanceContextMode.Single)]
	public class ProcessingService : BaseService, IProcessingService, IDisposable
	{
		#region Fields

		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private int m_ErrorCount;

		#endregion

		#region Constructor

		public ProcessingService()
		{
			m_Logger.Info("Initializing processing service...");

			AssemblyInfo ai = new AssemblyInfo(Assembly.GetExecutingAssembly());
			m_Logger.InfoFormat("APS Version: {0}. Last Modified: {1}", ai.Version, ai.LastModifiedDate.ToString("dd MMM yyyy HH:mm"));

			ServiceStatus.Instance.StartDate = DateTime.Now;
			Initialize();

			m_Logger.Info("Done initializing processing service");
		}

		#endregion

		#region IProcessingService Implementation

		public List<string> GetSupportedExtensions()
		{
			return PluginManager.Instance.GetAllSupportedExtensions().ToList();
		}

		public List<string> GetPlugins(string extension)
		{
			return (from pi in PluginManager.Instance.GetPluginsForExtension(extension)
			        select pi.Name).ToList();
		}

		public long SubmitJob(Job job)
		{
			return JobManager.Instance.AddJobToQueue(job);
		}

		public JobResult ProcessJob(Job job)
		{
			return ProcessingManager.ProcessJob(job);
		}

		public XElement GetAssetMetadata(string path)
		{
			MetadataExtractor mex = new MetadataExtractor(path);
			return mex.GetXml().Root;
		}

		public ServiceStatus GetServiceStatus()
		{
			Console.WriteLine("Received request for service status");
			return ServiceStatus.GetServiceStatus();
		}

		public VersionInfo GetVersionInfo()
		{
			AssemblyInfo ai = new AssemblyInfo(Assembly.GetExecutingAssembly());
			return new VersionInfo
			       	{
			       		Major = ai.Version.Major,
			       		Minor = ai.Version.Minor,
			       		Revision = ai.Version.Revision,
			       		Build = ai.Version.Build,
			       		CreateDate = ai.CreateDate,
			       		LastModifiedDate = ai.LastModifiedDate
			       	};
		}

		public List<string> FileExtensionsGenerated()
		{
			return PluginManager.Instance.GetAllExtensionsGenerated().ToList();
		}

		public string GetExtensionGenerated(string pluginName, string extension)
		{
			try
			{
				PluginInfo pi = PluginManager.Instance.GetPluginInfo(pluginName, extension);
				if (pi != null)
				{
					return pi.GetAssetProcessingPlugin().GetExtensionGenerated(extension);
				}
			}
			catch (PluginNotFoundException)
			{
				//ignore error
			}

			return String.Empty;
		}

		#endregion

		#region Private Helper Methods

		private void Initialize()
		{
			Console.WriteLine();

			CheckDatabaseConnectivity();

			Console.WriteLine();

			SetupTempPath();

			Console.WriteLine();

			SetPluginConfigPath();

			WritePluginsToLog();

			Console.WriteLine();

			if (m_ErrorCount > 0)
			{
				m_Logger.ErrorFormat("Job monitoring not started.  {0} errors occurred when starting service.", m_ErrorCount);
			}
			else
			{
				JobManager.Instance.StartMonitoring();
			}

			Console.WriteLine();
		}

		private void SetPluginConfigPath()
		{
			try
			{
				string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

				if (string.IsNullOrEmpty(assemblyPath))
					throw new SystemException("Unable to get application path");

				string configPath = Path.Combine(assemblyPath, "Config\\Plugins.config");

				if (!File.Exists(configPath))
					throw new FileNotFoundException("Plugin config not found", configPath);

				PluginManager.Instance.PluginConfigPath = configPath;

				m_Logger.DebugFormat("Set plugin config path: {0}", configPath);
			}
			catch (Exception e)
			{
				m_Logger.Error(string.Format("Error setting plugin config path: {0}", e.Message), e);
				m_ErrorCount++;
			}
		}

		private static void WritePluginsToLog()
		{
			var pluginList = PluginManager.Instance.GetFullPluginList();

			m_Logger.DebugFormat("Found {0} plugins", pluginList.Count());

			foreach (PluginInfo pi in pluginList)
			{
				m_Logger.DebugFormat(" - Name: {0}, Enabled: {1}", pi.Name, pi.Enabled);
			}
		}

		private static void SetupTempPath()
		{
			string tempPath = ConfigurationManager.AppSettings.Get("TempPath");

			if (!string.IsNullOrEmpty(tempPath))
				PathUtils.Instance.TempPath = Path.GetFullPath(tempPath);

			m_Logger.DebugFormat("Temp path: {0}", PathUtils.Instance.GetTempPath());
		}

		private void CheckDatabaseConnectivity()
		{
			try
			{
				int statusId = Convert.ToInt32(Status.NotStarted);

				using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
				{
					int count = (from qj in db.APSQueuedJob
					             where qj.QueuedJobStatusId == statusId
					             select qj).Count();

					m_Logger.DebugFormat("Database connectivity verified (job count: {0})", count);
				}
			}
			catch (Exception ex)
			{
				m_Logger.Error(string.Format("Error with local database connectivity: {0}", ex.Message), ex);
				m_ErrorCount++;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			JobManager.Instance.StopMonitoring();
		}

		#endregion
	}
}