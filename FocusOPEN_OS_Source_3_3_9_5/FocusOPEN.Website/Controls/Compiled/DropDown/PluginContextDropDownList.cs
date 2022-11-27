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
using System.Web;
using FocusOPEN.Data;
using Daydream.Data;
using FocusOPEN.Shared;
using FocusOPEN.Business;
using FocusOPEN.Website.Components;
using System.Web.UI;

namespace FocusOPEN.Website.Controls
{
    public class PluginContextDropDownList :AbstractDropDownList
    {
        public PluginContextDropDownList(){
            BindOnInit = false; //requires plugin and contexttype to be set first 
            OmitBlankItem = true;
        }

        #region Accessors

        public int PluginId 
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState,"ContextDropDownPluginId", 0);
            }
            set
            {
                ViewState["ContextDropDownPluginId"] = value;
            }
        
        }
        public ContextType ContextType {
            get
            {
                return SiteUtils.GetFromStore(ViewState,"ContextDropDownType", ContextType.Standard);
            }
            set
            {
                ViewState["ContextDropDownType"] = value;
            }
        
        }

        #endregion

        #region AbstractDropDownList Implementation

        public override object GetDataSource()
        {
            //bind to plugins context names
            Plugin plugin = ContextInfo.PluginManager.GetAnyPluginById(PluginId);
            return PluginManager.GetContextNames(plugin);
        }

        protected override void BindList()
        {
            base.BindList();

            //find the appropriate context name in use for this type
            Plugin plugin = ContextInfo.PluginManager.GetAnyPluginById(PluginId);
            IPluginContext context = PluginManager.GetContext(plugin, ContextType);

            if (context != null)
            {
                //select the context in the dropdownlist
                SafeSelectValue(context.Name);
            }

        }


        #endregion



    }
}