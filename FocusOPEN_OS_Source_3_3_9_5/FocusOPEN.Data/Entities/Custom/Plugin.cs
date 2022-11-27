/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using Daydream.Data;
using FocusOPEN.Shared;
using System;
using System.IO;


namespace FocusOPEN.Data
{

	public partial class Plugin
	{
        #region Private variables

        private IPlugin m_plugin = null;
        private PluginStatus m_status = PluginStatus.None;
        private string m_pluginpath = String.Empty;
        private string m_formatError = String.Empty;
        private bool m_isUnregistered = false;

        #endregion


        #region Public properties

        public bool HasPluginFile
        {
            get
            {
                return (m_plugin != null);
            }
        }

        public IPlugin PluginFile
        {
            get
            {
                return m_plugin;
            }
        }

        public string PluginPath
        {
            get
            {
                return m_pluginpath;
            }

            set
            {
                m_pluginpath = value;
            }

        }

        public string FilePath
        {
            get
            {
                return Path.Combine(PluginPath, Filename);
            }
        }

        public PluginStatus Status
        {
            get
            {
                return m_status;
            }
            set
            {
                m_status = value;
            }
        }

        public string FormatErrorMessage
        {
            get
            {
                return m_formatError;
            }
            set
            {
                m_formatError = value;
            }
        }


        public bool IsUnregistered
        {
            get
            {
                return m_isUnregistered;
            }
            set
            {
                m_isUnregistered = value;
            }
        }

        public bool IsDisabled
        {
            get
            {
                return (HasPluginFile && PluginFile.Disabled);
            }
        }


        #endregion


        #region Public methods

        /// <summary>
        /// Adds a plugin config file to the data class
        /// </summary>
        public void AddPluginFile(IPlugin plugin, string fileName, string filePath, string relativePath)
        {
            m_plugin = plugin;
            Filename = fileName;
            PluginPath = filePath;
            RelativePath = relativePath;
        }

        #endregion
    }



}
