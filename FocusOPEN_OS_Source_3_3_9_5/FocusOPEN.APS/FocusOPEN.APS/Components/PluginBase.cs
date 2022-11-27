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
using System.Drawing;
using System.IO;
using System.Linq;

namespace FocusOPEN.APS
{
	public abstract class PluginBase
	{
		#region Constructor

		protected PluginBase()
		{
			FileDataItems = new Dictionary<string, object>();
		}

		#endregion

		#region Abstract Methods & Properties to be implemented

		/// <summary>
		/// Boolean value specifying whether or this plugin
		/// can generate previews for its asset type
		/// </summary>
		public abstract bool CanGeneratePreview { get; }

		/// <summary>
		/// Boolean value specifying whether or this plugin
		/// can generate thumbnails for its asset type
		/// </summary>
		public abstract bool CanGenerateThumbnail { get; }

		/// <summary>
		/// Gets a list of file extensions for which this plugin
		/// can generate thumbnails and/or previews
		/// </summary>
		public abstract IEnumerable<string> SupportedFileExtensions { get; }

		/// <summary>
		/// Generate a preview for the asset
		/// </summary>
		public abstract string GeneratePreview();

		/// <summary>
		/// Generate a thumbnail for the asset
		/// </summary>
		public abstract string GenerateThumbnail();

        /// <summary>
        /// Gets a list of file extensions which this plugin
        /// will generate thumbnails and/or previews as
        /// </summary>
        public abstract IEnumerable<string> FileExtensionsGenerated { get; }

		#endregion

		#region Virtual Methods

		/// <summary>
		/// Called when the plugin is initialized
		/// </summary>
		public virtual void Setup()
		{}

		/// <summary>
		/// Called when plugin has been initialized with all settings
		/// just before preview and thumbnail generation is started
		/// </summary>
		public virtual void BeforeProcessing()
		{ }

		/// <summary>
		/// Called after all processing is complete
		/// </summary>
		public virtual void Cleanup()
		{ }


        /// <summary>
        /// Returns the extension generated for an input file type of inputExtension
        /// </summary>
        public virtual string GetExtensionGenerated(string inputExtension)
        {
            List<string> extensions = FileExtensionsGenerated.ToList();

            if (!extensions.Contains(inputExtension, StringComparer.OrdinalIgnoreCase))
            {
                return extensions[0]; //default
            }
            else
            {
                return inputExtension;
            }
        }


		#endregion

		#region Properties

		/// <summary>
		/// Plugin settings
		/// </summary>
		public NameValueCollection Settings { get; internal set; }

		/// <summary>
		/// Gets or sets the path of the file to be processed
		/// </summary>
		public string InputPath { get; internal set; }

		/// <summary>
		/// Gets or sets the path of the watermark to be applied
		/// </summary>
		public string WatermarkPath { get; internal set; }

		/// <summary>
		/// Gets or sets the size of the preview to be generated
		/// </summary>
		public Size PreviewSize { get; internal set; }

		/// <summary>
		/// Gets or sets the size of the thumbnail to be generated
		/// </summary>
		public Size ThumbnailSize { get; internal set; }

		/// <summary>
		/// List of any other data to be included in postback
		/// </summary>
		public Dictionary<string, object> FileDataItems { get; private set; }

		#endregion

		#region Protected Methods

		protected static string GetTempFilename(string extension)
		{
			if (extension.StartsWith("."))
				extension = extension.Substring(1);

			string filename = Guid.NewGuid() + "." + extension;
			string path = Path.Combine(PathUtils.Instance.GetTempPath(), filename);

			if (File.Exists(path))
				File.Delete(path);

			return path;
		}

		/// <summary>
		/// Gets a setting from the plugin settings
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected string GetSetting(string key)
		{
			return GetSetting<String>(key);
		}

		/// <summary>
		/// Gets a setting from the plugin settings
		/// </summary>
		protected T GetSetting<T>(string key)
		{
			if (!Settings.ContainsKey(key))
				throw new MissingSettingException(key);

			return (T) Convert.ChangeType(Settings.GetValue(key), typeof (T));
		}

		/// <summary>
		/// Gets a setting from the plugin settings
		/// </summary>
		/// <typeparam name="T">The type of value that should be returned</typeparam>
		/// <param name="key">The setting key</param>
		/// <param name="defaultVal">The default value that should be returned if no setting value exists</param>
		/// <returns>Setting from plugin settings or default value</returns>
		protected T GetSetting<T>(string key, T defaultVal)
		{
			if (!Settings.ContainsKey(key))
				return defaultVal;

			try
			{
				return (T) Convert.ChangeType(Settings.GetValue(key), typeof (T));
			}
			catch (Exception)
			{
				return defaultVal;
			}
		}

		#endregion
	}
}