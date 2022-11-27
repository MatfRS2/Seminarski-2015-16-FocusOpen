/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;
using System.Collections.Generic;
using FocusOPEN.Business;
using System;
using System.Linq;

namespace FocusOPEN.Website.Controls
{
    public class PluginDropDownList : AbstractDictionaryDropDownList
    {

        public PluginDropDownList()
        {
            BindOnInit = false; //requires extension to be set first
        }


        #region Accessors


        public bool ShowSupportedOnly
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "ShowSupportedOnly", true);
            }
            set
            {
                ViewState["ShowSupportedOnly"] = value;
            }

        }
        public string Extension
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "Extension", String.Empty);
            }
            set
            {
                ViewState["Extension"] = value;
            }

        }

        #endregion


        #region AbstractDropDownList Implementation

        public override object GetDataSource()
        {
            Dictionary<int, String> displayPlugins = new Dictionary<int, string>();

            IEnumerable<Plugin> plugins;

            if(ShowSupportedOnly){
                plugins = from p in ContextInfo.PluginManager.ValidPlugins
                          where p.HasPluginFile && p.PluginFile.FileExtensions.Contains(Extension)
                          select p;
            }else{
                plugins = from p in ContextInfo.PluginManager.ValidPlugins
                          where p.HasPluginFile
                          select p;
            }

            foreach (Plugin plugin in plugins)
            {
                displayPlugins.Add(plugin.PluginId.GetValueOrDefault(), plugin.Name);
            }

            return displayPlugins;
        }

        #endregion


    }
}