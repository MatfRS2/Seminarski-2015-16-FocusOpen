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
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using log4net;

namespace FocusOPEN.APS
{
	public class PluginManager : Singleton<PluginManager>
	{
		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Properties

		/// <summary>
		/// Gets or sets the path to the plugin configuration file
		/// </summary>
		public string PluginConfigPath { get; set; }

		#endregion

		#region Constructor

		public PluginManager()
		{
			PluginConfigPath = "./Config/Plugins.Config";
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets a list of all supported extensions
		/// </summary>
		public IEnumerable<string> GetAllSupportedExtensions()
		{
			List<string> supportedList = new List<string>();

			foreach (PluginInfo pi in GetPluginList())
			{
				try
				{
					// Add extensions explicitly supported
					supportedList.AddRange(pi.ExtensionsList);

					// Add extensions the asset thinks it can support
					supportedList.AddRange(pi.GetAssetProcessingPlugin().SupportedFileExtensions);

					// Remove extensions that have been explicitly excluded
					pi.ExcludedExtensionsList.ForEach(s => supportedList.RemoveAll(x => x == s));
				}
				catch (Exception ex)
				{
					m_Logger.Warn(string.Format("Error getting supported extensions for plugin: {0}.  Error: {1}", pi.Name, ex.Message), ex);
				}
			}

			// Remove duplicate extensions
			// (eg. if more than one plugin can process an extension)
			supportedList = supportedList.Distinct().ToList();

			// Sort the list
			supportedList.Sort();

			// All done, return list
			return supportedList;
		}


        /// <summary>
        /// Gets a list of all supported extensions
        /// </summary>
        public IEnumerable<string> GetAllExtensionsGenerated()
        {
            List<string> generatedList = new List<string>();

            foreach (PluginInfo pi in GetPluginList())
            {
                try
                {            
                    // Add extensions the asset thinks it can support
                    generatedList.AddRange(pi.GetAssetProcessingPlugin().FileExtensionsGenerated);
                }
                catch (Exception ex)
                {
                    m_Logger.Warn(string.Format("Error getting generate extensions for plugin: {0}.  Error: {1}", pi.Name, ex.Message), ex);
                }
            }

            // Remove duplicate extensions
            // (eg. if more than one plugin can process an extension)
            generatedList = generatedList.Distinct().ToList();

            // Sort the list
            generatedList.Sort();

            // All done, return list
            return generatedList;
        }





		/// <summary>
		/// Gets a list of all plugins
		/// </summary>
		public IEnumerable<PluginInfo> GetFullPluginList()
		{
			return (from p in XElement.Load(PluginConfigPath).Elements("plugin")
					orderby p.GetAttribute("name", string.Empty)
					select new PluginInfo(p));
		}

		/// <summary>
		/// Gets a list of all active plugins
		/// </summary>
		public IEnumerable<PluginInfo> GetPluginList()
		{
			return (from p in XElement.Load(PluginConfigPath).Elements("plugin")
			        let enabled = p.GetAttribute("enabled", false)
			        where (enabled)
			        orderby p.GetAttribute("priority", 0), p.GetAttribute("name", string.Empty)
			        select new PluginInfo(p));
		}

		/// <summary>
		/// Gets a list of plugins available to process the specified file extension
		/// </summary>
		public IEnumerable<PluginInfo> GetPluginsForExtension(string extension)
		{
			return (from p in GetPluginList()
			        where p.SupportsExtension(extension)
			        select p);
		}

		/// <summary>
		/// Gets the first plugin specified that is able to process the
		/// specified extension
		/// </summary>
		public PluginInfo GetFirstPluginForExtension(string extension)
		{
			return (from p in GetPluginsForExtension(extension)
			        select p).FirstOrDefault();
		}

		/// <summary>
		/// Gets plugin info by name
		/// </summary>
		public PluginInfo GetPluginInfo(string name)
		{
			return (from p in GetPluginList()
			        where (p.Name == name)
			        select p).FirstOrDefault();
		}

		/// <summary>
		/// Gets the plugin info based on the name, or extension if no name is specified
		/// </summary>
		/// <returns>PluginInfo for specified name, or if not specified, then extension</returns>
		/// <exception cref="PluginNotFoundException">Thrown if no plugin could be found for the specified name and extension.</exception>
		public PluginInfo GetPluginInfo(string name, string extension)
		{
			// Plugin to be used for processing
			PluginInfo pluginInfo = null;

			// If a plugin was specified, then try and get it
			if (!String.IsNullOrEmpty(name))
				pluginInfo = GetPluginInfo(name);

			// If no plugin was found, then get the first plugin for the file
			// This will work in order of priority, as defined in the config file.
			if (pluginInfo == null)
				pluginInfo = GetFirstPluginForExtension(extension);

			// Ensure we have a plugin
			if (pluginInfo == null)
			{
				string pluginName = String.IsNullOrEmpty(name) ? "Not Specified" : name;
				throw new PluginNotFoundException(pluginName);
			}

			return pluginInfo;
		}

		#endregion
	}
}