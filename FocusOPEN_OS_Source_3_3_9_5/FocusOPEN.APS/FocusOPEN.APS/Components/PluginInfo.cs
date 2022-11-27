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
using System.Collections.Specialized;
using System.Linq;
using System.Xml.Linq;

namespace FocusOPEN.APS
{
	public class PluginInfo
	{
		#region Constructor

		public PluginInfo(XElement element)
		{
			if (element == null)
				throw new ArgumentException("Element cannot be null");

			Name = element.GetAttribute("name", string.Empty);
			Extensions = element.GetAttribute("extensions", string.Empty);
			ExcludedExtensions = element.GetAttribute("excludedextensions", string.Empty);
			Type = element.GetAttribute("type", string.Empty);
			Priority = element.GetAttribute("priority", 0);
			ProcessingPriority = element.GetAttribute("processingpriority", Int32.MaxValue);
			Enabled = element.GetAttribute("enabled", true);
			Settings = new NameValueCollection();

			if (element.Descendants("settings").Count() > 0)
				foreach (XElement setting in element.Descendants("settings").First().Descendants())
					Settings[setting.GetAttribute("key", string.Empty)] = setting.GetAttribute("value", string.Empty);
		}

		#endregion

		#region Simple Accessors

		/// <summary>
		/// Gets the plugin name
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the list of extensions allowed, as specified in the config file
		/// </summary>
		public string Extensions { get; private set; }

		/// <summary>
		/// Gets the list of extensions explicitly excluded, as specified in the config file
		/// </summary>
		/// <remarks>
		/// This is in case a plugin supports an extension, but needs to be overridden to specifically
		/// disallow support for that extension for whatever reason.
		/// </remarks>
		public string ExcludedExtensions { get; private set; }


		/// <summary>
		/// Gets the plugin type
		/// </summary>
		public string Type { get; private set; }

		/// <summary>
		/// Gets the plugin type, determining what priority this plugin is ranked when selecting
		/// which plugin to use for a file extension when no plugin is specified
		/// </summary>
		public int Priority { get; private set; }


		/// <summary>
		/// Gets the processing priority, which determines which order jobs are processed
		/// Pending jobs to be processed using higher processing priority plugins will be processed first
		/// </summary>
		public int ProcessingPriority { get; private set; }

		/// <summary>
		/// Boolean value specifying whether this plugin is enabled
		/// </summary>
		public bool Enabled { get; private set; }


		/// <summary>
		/// Gets the plugin settings dictionary
		/// </summary>
		public NameValueCollection Settings { get; private set; }

		#endregion

		/// <summary>
		/// Gets the list of extensions that have been specifically excluded
		/// via the plugin settings configuration file.
		/// </summary>
		public List<string> ExcludedExtensionsList
		{
			get
			{
				return (from e in ExcludedExtensions.Split(',')
						let Extension = e.Trim()
						where e.Length > 0
						select Extension).ToList();
			}
		}

		/// <summary>
		/// Gets the list of extensions that have been specifically included
		/// via the plugin settings configuration file.
		/// </summary>
		public List<string> ExtensionsList
		{
			get
			{
				return (from e in Extensions.Split(',')
				        let Extension = e.Trim()
				        where e.Length > 0
				        select Extension).ToList();
			}
		}





		/// <summary>
		/// Checks if the plugin supports the specified extension
		/// </summary>
		public bool SupportsExtension(string extension)
		{
			// Empty extensions are never supported
			if (String.IsNullOrEmpty(extension))
				return false;

			// Lowercase it
			string ext = extension.ToLower();

			// Strip preceding period
			if (ext.StartsWith("."))
				ext = ext.Substring(1);

			// If this extension has explicitly been excluded, return false
			if (ExcludedExtensionsList.Contains(ext))
				return false;

			// If this extension has explicitly been included, return true
			if (ExtensionsList.Contains(ext))
				return true;

			try
			{
				// Otherwise, check if the actual plugin thinks it can
				// support this extension, and if so, return true.
				if (GetAssetProcessingPlugin().SupportedFileExtensions.Contains(ext))
					return true;
			}
			catch (NotImplementedException)
			{
				// If the plugin does not have any supported file extensions, just
				// catch the error.  We can't do much about this, so let it fall through.
			}

			// Everything failed, so we don't support this extension
			return false;
		}

		/// <summary>
		/// Gets the plugin to be used for processing.
		/// </summary>
		/// <returns>Plugin to be used for processing</returns>
		/// <exception cref="PluginInvocationException">Thrown if the plugin could not be invoked</exception>
		public PluginBase GetAssetProcessingPlugin()
		{
			// Create an instance of the plugin
			PluginBase plugin = Activator.CreateInstance(System.Type.GetType(Type)) as PluginBase;

			// Ensure plugin was created
			if (plugin == null)
				throw new PluginInvocationException(string.Format("Unable to create instance of {0}. Please check PluginBase is implemented.", Type));

			// Provide settings
			plugin.Settings = Settings;

			return plugin;
		}
	}
}