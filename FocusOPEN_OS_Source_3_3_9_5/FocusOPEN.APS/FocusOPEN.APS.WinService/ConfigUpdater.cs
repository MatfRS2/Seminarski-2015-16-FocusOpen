/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Diagnostics;
using System.IO;
using System.Xml;

namespace FocusOPEN.APS.WinService
{
	internal class ConfigUpdater
	{
		#region Fields

		private string m_TargetDir;
		private string m_ImageMagickPath;

		#endregion

		#region Constructor

		public ConfigUpdater(string targetDir, string imageMagickPath)
		{
			m_TargetDir = targetDir;
			m_ImageMagickPath = imageMagickPath;
		}

		#endregion

		public void Update()
		{
			// Get target dir
			if (m_TargetDir.EndsWith("|"))
				m_TargetDir = m_TargetDir.Substring(0, m_TargetDir.Length - 1);

			// Get user-entered imagemagick dir
			if (m_ImageMagickPath.EndsWith("|"))
				m_ImageMagickPath = m_ImageMagickPath.Substring(0, m_ImageMagickPath.Length - 1);

			// Strip trailing slash from target dir
			if (m_TargetDir.EndsWith("\\"))
				m_TargetDir = m_TargetDir.Substring(0, m_TargetDir.Length - 1);

			// Strip trailing slash from imagemagick dir
			if (m_ImageMagickPath.EndsWith("\\"))
				m_ImageMagickPath = m_ImageMagickPath.Substring(0, m_ImageMagickPath.Length - 1);

			UpdateAppConfig();
			UpdatePluginConfig();
		}

		#region Private Methods

		private void UpdateAppConfig()
		{
			string appConfigFile = Path.Combine(m_TargetDir, "FocusOPEN.APS.WinService.exe.config");

			if (!File.Exists(appConfigFile))
			{
				Debug.WriteLine("Config file does not exist; nothing to update.");
				return;
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(appConfigFile);

			UpdateAppSetting(doc, "EXIFToolPath", Path.Combine(m_TargetDir, @"Apps\EXIFTool\EXIFTool.exe"));

			doc.Save(appConfigFile);

			Debug.WriteLine("App config updated successfully");
		}

		private static void UpdateAppSetting(XmlDocument doc, string key, string value)
		{
			XmlElement n = (XmlElement)doc.SelectSingleNode("/configuration/appSettings/add[@key='" + key + "']");

			if (n == null)
			{
				Debug.WriteLine(string.Format("Setting '{0}' not found in app config file", key));
				return;
			}

			n.SetAttribute("value", value);
			Debug.WriteLine(string.Format("Updated setting '{0}' with value '{1}' in app config file", key, value));
		}

		private void UpdatePluginConfig()
		{
			string pluginConfigFile = Path.Combine(m_TargetDir, @"Config\Plugins.config");

			if (!File.Exists(pluginConfigFile))
			{
				Debug.WriteLine("Plugin config file does not exist; nothing to update.");
				return;
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(pluginConfigFile);

			Debug.WriteLine("Loaded plugin config file: " + pluginConfigFile);

			// ImageMagickProcessingPlugin Settings
			UpdatePluginSetting(doc, "ImageMagickPath", m_ImageMagickPath);
			UpdatePluginSetting(doc, "ColorProfilesPath", Path.Combine(m_TargetDir, "ColorProfiles"));
			Debug.WriteLine("Updated ImageMagickProcessingPlugin settings");

			// VideoProcessingPlugin Settings
			UpdatePluginSetting(doc, "FFmpegExecutablePath", Path.Combine(m_TargetDir, @"Apps\FFmpeg\FFmpeg.exe"));
			UpdatePluginSetting(doc, "FLVToolExecutablePath", Path.Combine(m_TargetDir, @"Apps\FLVTool2\FLVTool2.exe"));
			Debug.WriteLine("Updated VideoProcessingPlugin settings");

			doc.Save(pluginConfigFile);

			Debug.WriteLine("Saved plugin config file: " + pluginConfigFile);
		}

		private static void UpdatePluginSetting(XmlDocument doc, string key, string value)
		{
			Debug.WriteLine(string.Format("Update plugin setting - {0} : {1}", key, value));

			XmlNodeList nodes = doc.SelectNodes(string.Format("/plugins/plugin/settings/add[@key='{0}']", key));

			if (nodes == null || nodes.Count == 0)
			{
				Debug.WriteLine(string.Format("Setting '{0}' not found anywhere in plugin config file: {1}", key, doc.InnerXml));
				return;
			}

			Debug.WriteLine(string.Format("Found {0} items in plugin config file with name: {1}", nodes.Count, key));

			foreach (XmlNode node in nodes)
			{
				XmlElement e = (XmlElement) node;
				e.SetAttribute("value", value);
			}

			Debug.WriteLine(string.Format("Updated setting '{0}' with value '{1}' in plugin config file", key, value));
		}

		#endregion
	}
}