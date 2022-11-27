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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using log4net;

namespace FocusOPEN.APS
{
	internal class JobProcessor
	{
		#region Fields

		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Properties

		public long JobId { get; set; }
		public long AssetId { get; set; }
		public string InputPath { get; set; }
		public string WatermarkPath { get; set; }
		public string PluginName { get; set; }
		public bool CreatePreview { get; set; }
		public bool CreateThumbnail { get; set; }
		public int OverrideHeight { get; set; }
		public int OverrideWidth { get; set; }

		public string PluginUsed { get; private set; }
		public string PreviewPath { get; private set; }
		public string ThumbnailPath { get; private set; }
		public XDocument MetadataXml { get; private set; }

		public bool HasErrors { get; private set; }
		public Dictionary<string, object> FileDataItems { get; private set; }

		#endregion

		#region Constructor

		public JobProcessor()
		{
			MetadataXml = new XDocument(new XElement("MetadataXml"));
			FileDataItems = new Dictionary<string, object>();
		}

		#endregion

		#region Public Methods

		public void Go()
		{
			AddLogEntry("Starting processing");

			try
			{
				DoProcessing();
				AddLogEntry("Processing completed successfully");
			}
			catch (Exception ex)
			{
				AddLogEntry(string.Format("Error processing: {0}", ex.Message), LogEntryTarget.Database);
				m_Logger.Error("Error processing job: " + ex.Message, ex);
			}
		}

		#endregion

		#region Private Helper Methods

		private void DoProcessing()
		{
			string inputPath = InputPath.Trim();

			if (string.IsNullOrEmpty(inputPath))
			{
				string error = string.Format("Input file path is empty.  Processing ignored. File: {0}", inputPath);
				AddLogEntry(error, LogEntryTarget.Database);
				m_Logger.Error(error);
				throw new FileNotFoundException("File not found. Input path is empty");
			}

			if (!File.Exists(inputPath))
			{
				string error = string.Format("Input file not found.  Processing ignored. File: {0}", inputPath);
				AddLogEntry(error, LogEntryTarget.Database);
				m_Logger.Error(error);
				throw new FileNotFoundException("File not found", inputPath);
			}

			try
			{
				// Plugin to be used for processing
				PluginInfo pluginInfo = PluginManager.Instance.GetPluginInfo(PluginName, PathUtils.GetExtension(inputPath));

				// Get the actual plugin to do the processing
				PluginBase plugin = pluginInfo.GetAssetProcessingPlugin();

				// Do any setup stuff
				plugin.Setup();

				// Set the plugin actually used for processing
				PluginUsed = pluginInfo.Name;

				AddLogEntry("Plugin to be used for processing: " + PluginUsed);

				// Set required properties
				plugin.InputPath = inputPath;
				plugin.WatermarkPath = WatermarkPath;
				plugin.ThumbnailSize = (OverrideWidth > 0 && OverrideHeight > 0) ? new Size(OverrideWidth, OverrideHeight) : new Size(100, 100);
				plugin.PreviewSize = (OverrideWidth > 0 && OverrideHeight > 0) ? new Size(OverrideWidth, OverrideHeight) : new Size(320, 300);

				m_Logger.DebugFormat("Dimensions to be used - thumbnail: {0}(w)x{1}h, preview: {2}(w)x{3}h", plugin.ThumbnailSize.Width, plugin.ThumbnailSize.Height, plugin.PreviewSize.Width, plugin.PreviewSize.Height);

				try
				{
					plugin.BeforeProcessing();

					m_Logger.InfoFormat("Processing {0}.  Preview: {1} (Watermark: {2}).  Thumbnail: {3}", inputPath, CreatePreview, (!String.IsNullOrEmpty(WatermarkPath)).ToString().ToLower(), CreateThumbnail);

					if (CreatePreview)
					{
						if (plugin.CanGeneratePreview)
						{
							PreviewPath = plugin.GeneratePreview();

							if (!String.IsNullOrEmpty(PreviewPath) && !File.Exists(PreviewPath))
								m_Logger.WarnFormat("Plugin reports that the preview file has been created, but file does not exist: {0}", PreviewPath);

							AddLogEntry("Generated preview: " + PreviewPath);
						}
						else
						{
							const string message = "Preview generation requested, but plugin cannot generate preview";

							AddLogEntry(message, LogEntryTarget.Database);

							m_Logger.Warn(message);
						}
					}
				}
				catch (NotImplementedException)
				{
					const string error = "Preview not created: plugin does not support this feature";

					AddLogEntry(error, LogEntryTarget.Database);

					m_Logger.Error(error);
				}
				catch (Exception e)
				{
					LogError("Error creating preview", e);
				}

				try
				{
					if (CreateThumbnail)
					{
						if (plugin.CanGenerateThumbnail)
						{
							ThumbnailPath = plugin.GenerateThumbnail();

							if (!String.IsNullOrEmpty(ThumbnailPath) && !File.Exists(ThumbnailPath))
								m_Logger.WarnFormat("Plugin reports thumbnail has been created, but file does not exist: {0}", ThumbnailPath);

							AddLogEntry("Generated thumbnail: " + ThumbnailPath);
						}
						else
						{
							const string message = "Thumbnail generation requested, but plugin cannot generate thumbnail";

							AddLogEntry(message, LogEntryTarget.Database);

							m_Logger.Warn(message);
						}
					}
				}
				catch (NotImplementedException)
				{
					const string error = "Thumbnail not created: plugin does not support this feature";

					AddLogEntry(error, LogEntryTarget.Database);

					m_Logger.Error(error);
				}
				catch (Exception e)
				{
					LogError("Error creating thumbnail", e);
				}

				// Add any other data to be posted back by plugin
				foreach (var item in plugin.FileDataItems)
					FileDataItems[item.Key] = item.Value;

				// Do any cleanup stuff
				plugin.Cleanup();
			}
			catch (PluginNotFoundException ex)
			{
				LogError("Plugin not found or not available", ex);
			}

			try
			{
				MetadataExtractor mex = new MetadataExtractor(inputPath);
				MetadataXml = mex.GetXml();
				AddLogEntry("Got file metadata XML");
			}
			catch (Exception ex)
			{
				LogError("Error getting metadata XML", ex);
			}
		}

		#endregion

		#region Private Logging Methods

		/// <summary>
		/// Adds a log entry to the job log and log file
		/// </summary>
		private void AddLogEntry(string message)
		{
			AddLogEntry(message, LogEntryTarget.All);
		}

		/// <summary>
		/// Adds a log entry to the job log in database and a debug entry log file (where required)
		/// </summary>
		private void AddLogEntry(string message, LogEntryTarget target)
		{
			if (target.IsSet(LogEntryTarget.Database) && JobId > 0)
				LogManager.AddLogEntry(JobId, message);

			if (target.IsSet(LogEntryTarget.File))
				m_Logger.DebugFormat("JobId: {0} / AssetId: {1} - {2}", JobId, AssetId, message);
		}

		/// <summary>
		/// Logs an error. Saves the error to the database, adds a warning entry in the log file, and sets HasErrors to true.
		/// </summary>
		private void LogError(string message, Exception ex)
		{
			string error = message + ": " + ex.Message;

			AddLogEntry(error, LogEntryTarget.Database);

			m_Logger.Warn(error, ex);

			HasErrors = true;
		}

		#endregion
	}
}