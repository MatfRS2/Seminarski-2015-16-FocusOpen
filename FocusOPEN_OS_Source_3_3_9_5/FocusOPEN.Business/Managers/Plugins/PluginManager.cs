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
using System.Web;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	/// <summary>
	/// The plugin manager handles the management of user plugins
	/// </summary>
	public class PluginManager
	{
		#region Properties

		private static string m_pluginsPath = String.Empty;
		private static readonly ILog m_Logger = LogManager.GetLogger(typeof (PluginManager));

		/// <summary>
		/// Absolute path to the application's plugins folder
		/// </summary>
        public static string PluginsPath
        {
            get
            {
                return m_pluginsPath;
            }
            set
            {
                m_pluginsPath = WebUtils.GetSubFolderPath(value);
            }
        }

		#endregion

		#region Accessors

		/// <summary>
		/// Gets all user lightboxes
		/// </summary>
		public IEnumerable<Plugin> AllPlugins
		{
			get
			{
				List<Plugin> allPlugins = new List<Plugin>();
				allPlugins.AddRange(RegisteredPlugins);
				allPlugins.AddRange(UnregisteredPlugins);

				return allPlugins;
			}
		}

		/// <summary>
		/// Gets the unregistered plugins.
		/// </summary>
		public IEnumerable<Plugin> UnregisteredPlugins
		{
			get
			{
				//plugins are cached
				return PluginsUnregisteredCache.Instance.GetList();
			}
		}

		/// <summary>
		/// Gets all registered plugins
		/// </summary>
		public List<Plugin> RegisteredPlugins
		{
			get
			{
				//plugins are cached
				return PluginsRegisteredCache.Instance.GetList();
			}
		}

		/// <summary>
		/// Gets registered plugins with valid status flags
		/// </summary>
		public List<Plugin> ValidPlugins
		{
			get
			{
				return (from p in RegisteredPlugins
				        where !p.IsDisabled &&
				              (p.Status == PluginStatus.None ||
				               p.Status == PluginStatus.ChecksumMismatch || //currently checksum mismatch not enough to invalidate plugin
				               (p.Status == PluginStatus.ResourcesMissing && !p.PluginFile.RequireAllResourceFiles))
				        select p).ToList();
			}
		}

		#endregion

		#region Protected Methods

		internal List<Plugin> GetRegisteredPlugins()
		{
			List<Plugin> registered = new List<Plugin>();

			//check the plugins path is valid
			if (Directory.Exists(PluginsPath))
			{
				PluginFinder pluginFinder = new PluginFinder();
				EntityList<Plugin> registeredPlugins = Plugin.FindMany(pluginFinder);

				foreach (Plugin plugin in registeredPlugins)
				{
					//get the plugins path info
					string pluginFolder = Path.Combine(PluginsPath, plugin.RelativePath);
					string filePath = Path.Combine(pluginFolder, plugin.Filename);

					//try and load the plugin's xml config file
					IPlugin pluginFile = null;
					if (File.Exists(filePath))
					{
						//try and deserialize the plugin
						try
						{
							pluginFile = PluginHelper.ReadPlugin(filePath);
						}
						catch (Exception e)
						{
							//an error occurred so assume the config xml 
							//is invalid and does not validate correctly
							plugin.FormatErrorMessage = String.Format("{0} {1}", e.Message, (e.InnerException != null ? ":- " + e.InnerException.Message : String.Empty));
						}
					}
					
					//add the plugin file and sets the status flags
					AddPluginFile(plugin, pluginFile, filePath);

					//add to the list
					registered.Add(plugin);
				}
			}
			else
			{
				m_Logger.Error(string.Format("The plugins path is not set to a valid folder - {0}", PluginsPath));
			}
			return registered;
		}

		internal List<Plugin> GetUnregisteredPlugins()
		{
			List<Plugin> unregistered = new List<Plugin>();

			//check the plugins path is valid
			if (Directory.Exists(PluginsPath))
			{
				string[] pluginFiles = Directory.GetFiles(PluginsPath, "*.xml", SearchOption.AllDirectories);
				int tempId = Int32.MinValue;

				foreach (string filePath in pluginFiles)
				{
					//check to see if filepath listed as a registered plugin
					if (RegisteredPlugins.FirstOrDefault(p => p.FilePath.ToLower() == filePath.ToLower()) == null)
					{
						//create new unregistered plugin
						Plugin plugin = Plugin.New();
						plugin.IsUnregistered = true;

						//add a temporary pluginId for identification purposes
						plugin.PluginId = tempId++;

						//try and deserialize the plugin xml config
						IPlugin pluginFile = null;
						try
						{
							pluginFile = PluginHelper.ReadPlugin(filePath);
						}
						catch (Exception e)
						{
							//an error occurred so assume the config xml 
							//is invalid and does not validate correctly
							plugin.FormatErrorMessage = String.Format("{0} {1}", e.Message, (e.InnerException != null ? ":- " + e.InnerException.Message : String.Empty));
						}

						AddPluginFile(plugin, pluginFile, filePath);

						if (plugin.HasPluginFile)
						{
							if (plugin.RegistrationKey != Guid.Empty)
							{
								//plugin has a registration key so see if 
								//it can be found in registered plugins
								Plugin registeredPlugin = RegisteredPlugins.FirstOrDefault(p => p.RegistrationKey == plugin.RegistrationKey);

								if (registeredPlugin != null)
								{
									//is registered plugin listed with a missing file status
									if ((registeredPlugin.Status & PluginStatus.PluginMissing) != 0)
									{
										//attach plugin file to its registered plugin
										AddPluginFile(registeredPlugin, plugin.PluginFile, filePath);
										//update new path details
										Update(registeredPlugin);
									}
									continue; //move to next file
								}
								else
								{
									//reset the registration key as must be copied from somewhere else
									plugin.RegistrationKey = Guid.Empty;
								}
							}
						}
						unregistered.Add(plugin);
					}
				}
			}
			else
			{
				m_Logger.Error(string.Format("The plugins path is not set to a valid folder - {0}", PluginsPath));
			}
			return unregistered;
		}

		/// <summary>
		/// Sets the plugin status attributes dependent on the plugin file
		/// </summary>
		protected void SetPluginStatus(Plugin plugin)
		{
			plugin.Status = PluginStatus.None;

			if (plugin.FormatErrorMessage.Length > 0)
			{
				plugin.Status |= PluginStatus.InvalidFormat;
			}
			else
			{
				if (plugin.HasPluginFile)
				{
					//check that plugin has a standard context
					if (!(from c in plugin.PluginFile.ContextItems
					      where (c.UseForType != null && c.UseForType.Contains(ContextType.Standard))
					      select c).Any())
					{
						plugin.Status |= PluginStatus.NoStandardContext;
					}

					//check that plugin resources all present
					foreach (IPluginContext context in plugin.PluginFile.ContextItems)
					{
						if (!PluginHelper.ResourcesExist(plugin.PluginPath, context.Resources))
						{
							plugin.Status |= PluginStatus.ResourcesMissing;
							break;
						}
					}

					if (!plugin.IsUnregistered)
					{
						//check that plugin checksum ok if registered
						if (!PluginHelper.ChecksumsMatch(plugin.PluginPath, plugin.Checksum.GetValueOrDefault()))
						{
							plugin.Status |= PluginStatus.ChecksumMismatch;
						}
					}
				}
				else
				{
					plugin.Status |= PluginStatus.PluginMissing;
				}
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Adds the plugin xml config to the plugin class and sets status flags
		/// </summary>
		public void AddPluginFile(Plugin plugin, IPlugin pluginFile, string filePath)
		{
			//get the path info
			string pluginPath = Path.GetDirectoryName(filePath);
			string filename = Path.GetFileName(filePath);

			//sort out backslashes as MakeRelativePath is fussy about these
			if (!PluginsPath.Trim().EndsWith(@"\"))
			{
				PluginsPath += @"\";
			}
			if (pluginPath.Trim().EndsWith(@"\"))
			{
				pluginPath.Remove(pluginPath.Length - 1, 1);
			}

			string relativePath = FileUtils.MakeRelativePath(PluginsPath, pluginPath);

			plugin.AddPluginFile(pluginFile, filename, pluginPath, relativePath);

			if (plugin.HasPluginFile)
			{
				//synch the name
				plugin.Name = plugin.PluginFile.Name;

				//remove any dot prefixes on the file extensions for consistency
				//with how file extensions are held 
				for (int i = 0; i < plugin.PluginFile.FileExtensions.Length; i++)
				{
					plugin.PluginFile.FileExtensions[i] = plugin.PluginFile.FileExtensions[i].TrimStart(new[] {'.'});
				}
			}
			else
			{
				if (String.IsNullOrEmpty(plugin.Name))
					plugin.Name = "Unknown";
			}

			//set the status flags
			SetPluginStatus(plugin);
		}

		/// <summary>
		/// Gets the valid plugin by id.
		/// </summary>
		public Plugin GetValidPluginById(int pluginId)
		{
			Plugin plugin = (from p in ValidPlugins
			                 where
			                 	p.PluginId.GetValueOrDefault() == pluginId
			                 select p).FirstOrDefault();

			if (plugin != null)
				return plugin;

			return Plugin.Empty;
		}

		/// <summary>
		/// Gets any plugin by id.
		/// </summary>
		public Plugin GetAnyPluginById(int pluginId)
		{
			Plugin plugin = (from p in AllPlugins
			                 where p.PluginId.GetValueOrDefault() == pluginId
			                 select p).FirstOrDefault();

			if (plugin != null)
				return plugin;

			return Plugin.Empty;
		}

		/// <summary>
		/// Gets the valid plugin by registration key
		/// </summary>
		public Plugin GetValidPluginByKey(Guid registrationKey)
		{
			Plugin plugin = (from p in ValidPlugins
			                 where p.RegistrationKey == registrationKey
			                 select p).FirstOrDefault();

			if (plugin != null)
				return plugin;

			return Plugin.Empty;
		}

		/// <summary>
		////Get the default plugin for the asset's file extension by context
		/// </summary>
		public Plugin GetDefaultPluginForAsset(Asset asset)
		{
            string fileExtension = asset.FileExtension;

			if (fileExtension == "flv")
			{
				string rtmpStreamingServer = ConfigurationManager.AppSettings.GetValue("RTMPStreamingServer");
				if (!StringUtils.IsBlank(rtmpStreamingServer))
				{
					fileExtension += "-streaming";
				}
			}

			AssetTypeFileExtension atfe = AssetTypeFileExtensionCache.Instance.GetByExtension(fileExtension);
			
			//try and locate the plugin for the file extension
			if (atfe.Plugin != Guid.Empty)
			{
				Plugin plugin = GetValidPluginByKey(atfe.Plugin);
                if (!plugin.IsNull)
                {
                    //check whether plugin support this extension or if force preview set
                    return plugin;                   
                }
			}

			//use the system default
			return GetDefaultPlugin();
		}

		/// <summary>
		/// Gets the plugin for use with the asset
		/// </summary>
		public Plugin GetPluginForAsset(Asset asset)
		{
			if (asset.Plugin != Guid.Empty)
			{
				//try and get non-default plugin for asset
				Plugin plugin = GetValidPluginByKey(asset.Plugin);

				if (plugin != Plugin.Empty && plugin.HasPluginFile)
				{
					//no need to check extension or force preview settings
                    //if asset specifies a preview viewer override
				    return plugin;
				}
			}

			//use the default for the asset's file extension
			return GetDefaultPluginForAsset(asset);
		}

		/// <summary>
		/// Gets the system's default plugin.
		/// </summary>
		public Plugin GetDefaultPlugin()
		{
			Plugin plugin = (from p in RegisteredPlugins
			                 where p.IsDefault.GetValueOrDefault(false)
			                 select p).FirstOrDefault();

			if (plugin == null)
			{
				//no default specified so select first plugin processing images
				plugin = (from p in RegisteredPlugins
				          where (p.PluginFile != null) &&
				                (p.PluginFile.FileExtensions != null) &&
				                (p.PluginFile.FileExtensions.ContainsAny("jpg", "jpeg", "gif"))
				          select p).FirstOrDefault();

				if (plugin == null)
				{
					//still no plugin so just return the first
					//if any exist
					if (RegisteredPlugins.Count > 0)
						return RegisteredPlugins[0];

					return Plugin.Empty;
				}
			}
			return plugin;
		}

		public string GetMarkup(Plugin plugin, ContextType context)
		{
			string markup = String.Empty;

			if (!plugin.IsNull)
			{
				//retrieve the appropriate plugin context 
				IPluginContext pluginContext = GetContext(plugin, context);

				if (pluginContext != null)
				{
					if (!String.IsNullOrEmpty(pluginContext.Markup.src))
					{
						//check to see if source can be found
						string srcPath = Path.Combine(plugin.PluginPath, pluginContext.Markup.src);

						if (File.Exists(srcPath))
						{
							markup = FileUtils.ReadFileToString(srcPath);
						}
					}

					if (String.IsNullOrEmpty(markup))
					{
						//markup not found using the src attribute
						//so use the Value instead
						markup = pluginContext.Markup.Value;
					}
				}
			}
			else
			{
				m_Logger.Error(string.Format("The plugin {0} ({1}) does not contain markup for a standard context", plugin.Name, plugin.RegistrationKey));
			}

			return markup ?? string.Empty;
		}

		/// <summary>
		/// Saves the context's markup to the src or plugin file
		/// </summary>
		public void SaveMarkup(Plugin plugin, ContextType context, string markup)
		{
			if (!plugin.IsNull)
			{
				//retrieve the appropriate plugin context 
				IPluginContext pluginContext = GetContext(plugin, context);

				if (pluginContext != null)
				{
					if (!String.IsNullOrEmpty(pluginContext.Markup.src))
					{
						//create path to write markup to
						string srcPath = Path.Combine(plugin.PluginPath, pluginContext.Markup.src);
						//write to the src path
						FileUtils.WriteFileToString(srcPath, markup);
					}
					else
					{
						//markup not found using the src attribute
						//so update the markup element instead
						pluginContext.Markup.Value = markup;
						Update(plugin);

						//invalidate appropriate cache item
						CacheManager.InvalidateCache(plugin.IsUnregistered ? "PluginsUnregistered" : "PluginsRegistered", CacheType.Local);
					}
				}
			}
		}

		/// <summary>
		/// Determines whether the specified plugin is valid.
		/// </summary>
		public bool IsValid(Plugin plugin)
		{
			return (from p in ValidPlugins
			        where p.PluginId.GetValueOrDefault(0) == plugin.PluginId.GetValueOrDefault(-1)
			        select p).Any();
		}

		/// <summary>
		/// Determines whether the specified plugin is currently in use
		/// </summary>
		public bool IsUsed(Plugin plugin)
		{
			bool active = false;

			if (!plugin.IsUnregistered && plugin.RegistrationKey != Guid.Empty)
			{
				//check if any file extensions use it
				AssetTypeFileExtensionFinder atfeFinder = new AssetTypeFileExtensionFinder {Plugin = plugin.RegistrationKey};
				AssetTypeFileExtension atfe = AssetTypeFileExtension.FindOne(atfeFinder);
				if (!atfe.IsNull)
				{
					active = true;
				}
				else
				{
					//check to see if in use by any assets
					AssetFinder assetFinder = new AssetFinder {Plugin = plugin.RegistrationKey};
					Asset asset = Asset.FindOne(assetFinder);
					active = !asset.IsNull;
				}
			}

			return active;
		}

		/// <summary>
		/// Tries to register the plugin with the database and returns the registration key
		/// Guid if successful
		/// </summary>
		public int RegisterPlugin(Plugin plugin)
		{
			int newPluginId = plugin.PluginId.GetValueOrDefault();

			//check to see if the plugin has a plugin xml
			//file associated with it
			if (plugin.HasPluginFile)
			{
				//get appropriate guid for the registration key
				if (plugin.IsUnregistered || plugin.RegistrationKey == Guid.Empty)
				{
					plugin.RegistrationKey = Guid.NewGuid();
					plugin.PluginFile.RegistrationKey = plugin.RegistrationKey.ToString();
				}

				plugin.IsUnregistered = false;
				//remove any temporary pluginid
				if (plugin.PluginId.GetValueOrDefault() < 0)
				{
					plugin.PluginId = null;
				}

				string filePath = plugin.FilePath;
				//register plugin
				Update(plugin);

				//clear the cache
				CacheManager.InvalidateCache("Plugins", CacheType.Local);

				//get the new plugin id
				newPluginId = (from p in RegisteredPlugins
				               where p.FilePath == filePath
				               select p.PluginId.GetValueOrDefault()).FirstOrDefault();
			}
			return newPluginId;
		}

		/// <summary>
		/// Unregisters the plugin.
		/// </summary>
		public int UnregisterPlugin(Plugin plugin)
		{
			int newPluginId = plugin.PluginId.GetValueOrDefault();

			if (!plugin.IsUnregistered)
			{
				if (plugin.HasPluginFile)
				{
					//remove guid from the plugin file
					plugin.PluginFile.RegistrationKey = string.Empty;
					plugin.RegistrationKey = Guid.Empty;
				}

				//delete from db
				Plugin.Delete(plugin.PluginId);

				string filePath = plugin.FilePath;

				//update the file
				plugin.IsUnregistered = true;
				Update(plugin);

				//clear the cache
				CacheManager.InvalidateCache("Plugins", CacheType.Local);

				//get the new plugin id
				newPluginId = (from p in UnregisteredPlugins
				               where p.FilePath == filePath
				               select p.PluginId.GetValueOrDefault()).FirstOrDefault();
			}

			return newPluginId;
		}

		#endregion

		#region Public static methods

		/// <summary>
		/// Updates registered plugins and prevents unregistered plugins from writing to the database
		/// </summary>
		public static Plugin Update(Plugin plugin)
		{
			//update the file if it has one
			try
			{
				if (plugin.HasPluginFile)
				{
                    FileUtils.SaveToXmlFile(plugin.FilePath, plugin.PluginFile);
				}
			}
			catch (UnauthorizedAccessException)
			{
				m_Logger.Error(string.Format("Unable to write to {0}. Please check the folder privileges are set correctly.", plugin.FilePath));
			}

			if (plugin.IsUnregistered)
			{
				//can't update the plugin to the database
				return plugin;
			}

			//set checksum - required here after the plugin file has 
			//been written to its folder
			plugin.Checksum = Checksum(plugin);

			return Plugin.Update(plugin);
		}

		/// <summary>
		/// Gets the plugin's registration key.
		/// </summary>
		public static Guid GetRegistrationKey(int pluginId)
		{
			PluginFinder finder = new PluginFinder {PluginId = pluginId};
			Plugin plugin = Plugin.FindOne(finder);

			if (!plugin.IsNull)
				return plugin.RegistrationKey;

			return Guid.Empty;
		}

		/// <summary>
		/// Gets the plugin's Id.
		/// </summary>
		public static int GetPluginId(Guid registrationKey)
		{
			PluginFinder finder = new PluginFinder {RegistrationKey = registrationKey};
			Plugin plugin = Plugin.FindOne(finder);
			if (!plugin.IsNull)
				return plugin.PluginId.GetValueOrDefault();

			return 0;
		}

		public static int Checksum(Plugin plugin)
		{
			return FileUtils.GetFolderHash(plugin.PluginPath, true);
		}

		/// <summary>
		/// Gets the number of contexts specified in the plugin
		/// </summary>
		public static int NumberOfContexts(Plugin plugin)
		{
			int count = 0;
			if (plugin.HasPluginFile)
			{
				count = (from c in plugin.PluginFile.ContextItems
				         select c).Count();
			}
			return count;
		}

		/// <summary>
		/// Gets all the resources specified in the plugin's contexts
		/// </summary>
		public static List<string> GetResources(Plugin plugin)
		{
			if (plugin.HasPluginFile)
			{
				var resources = from c in plugin.PluginFile.ContextItems
				                where c.Resources != null
				                select c.Resources;

				return (from res in resources
				        from r in res
				        where r != null
				        select r).Distinct().ToList();
			}

			return new List<string>();
		}

		public static List<string> GetContextNames(Plugin plugin)
		{
			if (plugin.HasPluginFile)
			{
				return (from c in plugin.PluginFile.ContextItems
				        where c.Name != null
				        select c.Name).ToList();
			}
			return new List<string>();
		}

		/// <summary>
		/// Gets the context in use for the plugin's context type.
		/// </summary>
		public static IPluginContext GetContext(Plugin plugin, ContextType context)
		{
			IPluginContext pluginContext = null;

			if (plugin.HasPluginFile)
			{
				pluginContext = (from c in plugin.PluginFile.ContextItems
				                 where (c.UseForType != null && c.UseForType.Contains(context))
				                 select c).FirstOrDefault();

				if (pluginContext == null)
				{
					//no context for this type so resort to standard context
					pluginContext = (from c in plugin.PluginFile.ContextItems
					                 where (c.UseForType != null && c.UseForType.Contains(ContextType.Standard))
					                 select c).FirstOrDefault();
				}
			}

			return pluginContext;
		}

		/// <summary>
		/// Adds the context type to the context section with the matching name
		/// </summary>
		public static void AddContextTypeInSection(Plugin plugin, ContextType contextType, string contextName)
		{
			if (plugin.HasPluginFile)
			{
				//find currently assigned context section
				List<IPluginContext> pluginContexts = (from c in plugin.PluginFile.ContextItems
				                                       where (c.UseForType != null && c.UseForType.Contains(contextType))
				                                       select c).ToList();

				if (pluginContexts.Count > 0)
				{
					foreach (IPluginContext context in pluginContexts)
					{
						//clear this contextType from the section in preparation for reassigning
						List<ContextType> useFor = context.UseForType.ToList();
						useFor.Remove(contextType);
						context.UseForType = useFor.ToArray();
					}
				}

				//find the context with the name and assign it this contextType
				IPluginContext pluginContext = (from c in plugin.PluginFile.ContextItems
				                                where c.Name.ToLower() == contextName.ToLower()
				                                select c).FirstOrDefault();
				if (pluginContext != null)
				{
					if (!pluginContext.UseForType.Contains(contextType))
					{
						List<ContextType> useFor = pluginContext.UseForType.ToList();
						useFor.Add(contextType);
						pluginContext.UseForType = useFor.ToArray();
					}
				}
			}
		}

		/// <summary>
		/// Gets the context type by string
		/// </summary>
		public static ContextType GetContextType(string contextRequest)
		{
			if (contextRequest == null)
				contextRequest = String.Empty;

			try
			{
				return EnumUtils.GetEnumFromValue<ContextType>(contextRequest);
			}
			catch (Exception)
			{
				return ContextType.Standard;
			}
		}

		#endregion
	}
}