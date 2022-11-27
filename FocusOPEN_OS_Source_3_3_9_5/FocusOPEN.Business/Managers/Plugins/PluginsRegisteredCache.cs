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
using System.Text;
using FocusOPEN.Data;

namespace FocusOPEN.Business
{
    /// <summary>
    /// Cache to store registered plugin objects
    /// </summary>
    public class PluginsRegisteredCache : BaseEntityCache<Plugin>
    {
        #region Singleton

        private PluginsRegisteredCache()
        {
        }

        private static readonly PluginsRegisteredCache m_Instance = new PluginsRegisteredCache();

        public static PluginsRegisteredCache Instance
        {
            get
            {
                return m_Instance;
            }
        }

        #endregion

        #region BaseEntityCache Implementation

        protected override List<Plugin> GetData()
        {
            PluginManager pm = new PluginManager();
            return pm.GetRegisteredPlugins();
        }


        protected override Plugin Empty
        {
            get
            {
                return Plugin.Empty;
            }
        }

        protected override string CacheKey
        {
            get
            {
                return "DataListCache_RegisteredPlugin";
            }
        }

        protected override bool Compare(Plugin plugin, int id)
        {
            return plugin.PluginId.GetValueOrDefault() == id;
        }


        #endregion

    }

}
