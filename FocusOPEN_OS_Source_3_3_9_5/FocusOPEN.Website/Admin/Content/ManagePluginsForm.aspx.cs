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
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using log4net;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using Daydream.Data;
using FocusOPEN.Website.Controls;


namespace FocusOPEN.Website.Admin.Content
{
    public partial class ManagePluginsForm : BaseAdminPage
    {

        #region Accessors

        public ContextType CurrentContextType
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "CurrentContextType", ContextType.Standard);
            }
            set
            {
                ViewState["CurrentContextType"] = value;
            }
        }

        public int CurrentAssetID
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "CurrentAssetID", -1);
            }
            set
            {
                ViewState["CurrentAssetID"] = value;
            }

        }

        #endregion


        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                Plugin plugin = ContextInfo.PluginManager.GetAnyPluginById(WebUtils.GetIntRequestParam("PluginId", 0));

                if (!plugin.IsNull)
                {
                    // Populate the user interface
                    PluginNameLabel.Text = plugin.Name;
                    PluginVersionLabel.Text = (plugin.HasPluginFile?plugin.PluginFile.PluginVersion.ToString():"Unknown");
                    PluginFolderLabel.Text = plugin.PluginPath;
                    PluginGuidLabel.Text = (plugin.IsUnregistered ? "Unregistered (save to register)" : plugin.RegistrationKey.ToString());
                    //unregister plugin option only available if registered and not currently in use
                    PluginUnregisterPlaceHolder.Visible = (!plugin.IsUnregistered && !ContextInfo.PluginManager.IsUsed(plugin));                   
                    DownloadPluginPackLinkButton.Text = plugin.Name + ".zip (download)";
                    RequireAllFilesLabel.Text = (plugin.HasPluginFile ? (plugin.PluginFile.RequireAllResourceFiles ? "Yes" : "No") : "Unknown");
                    MissingFilesLabel.Text = (plugin.HasPluginFile ? ((plugin.Status & PluginStatus.ResourcesMissing) > 0 ? "Yes" : "No") : "Unknown");
                    PluginActiveCheckbox.Checked = !plugin.IsDisabled;
                    PluginActiveCheckbox.Text = (plugin.IsDisabled ? "No" : "Yes");  

                    //bind the plugin context type info
                    BindContexts();
                }
            }

            //set the feedback label if page reloaded after registering/unregistering
            if(Session["FeedbackLabel"] != null && !String.IsNullOrEmpty((string)Session["FeedbackLabel"]))
            {
                switch (((string)Session["FeedbackLabel"]).ToLower())
                {
                    case "registered":
                        SaveFeedbackLabel.SetSuccessMessage("Plug-in Registered", "The plug-in was successfully registered");
                        break;
                    case "unregistered":
                        SaveFeedbackLabel.SetSuccessMessage("Plug-in Unregistered", "The plug-in was successfully unregistered");
                        break;
                }
                Session["FeedbackLabel"] = null;
                FeedbackPlaceholder.Visible = true;
            } else{
                FeedbackPlaceholder.Visible = false;
            }
        }

        protected void ContextsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
            {
                string contextType = (string)e.Item.DataItem;

                // Get the plugin
                Plugin plugin = GetCurrentPlugin();

                //get the current context type
                ContextType type = (ContextType)Enum.Parse(typeof(ContextType), contextType.Replace(" ", ""));

                //bind dropdown to contexts
                PluginContextDropDownList contextNames = (PluginContextDropDownList)e.Item.FindControl("ContextNamesDropDownList");

                contextNames.ContextType = type;
                contextNames.PluginId = plugin.PluginId.GetValueOrDefault();
                contextNames.RefreshFromDataSource();      

            }
        }



        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Get the plugin
            Plugin plugin = GetCurrentPlugin();

            if (plugin.HasPluginFile)
            {

                int oldId = plugin.PluginId.GetValueOrDefault();

                //set the plugin values
                plugin.PluginFile.Disabled = (!PluginActiveCheckbox.Checked);

                //set the plugins context values by iterating through data repeater values
                for (int i = 0; i < ContextsRepeater.Items.Count; i++)
                {
                    PluginContextDropDownList contextNames = (PluginContextDropDownList)ContextsRepeater.Items[i].FindControl("ContextNamesDropDownList");

                    IPluginContext context = PluginManager.GetContext(plugin, contextNames.ContextType);

                    if (context.Name != (string)contextNames.SelectedValue)
                    {
                        //selected context has changed                 
                        PluginManager.AddContextTypeInSection(plugin, contextNames.ContextType, (string)contextNames.SelectedValue);
                    }
                }

                //register and update the plugin
                int pluginId = ContextInfo.PluginManager.RegisterPlugin(plugin);

                if (oldId < 0)
                {
                    if (oldId != pluginId)
                    {
                        Session["FeedbackLabel"] = "registered";
                        //reload the page using the new registered plugin id
                        ReloadPlugin(pluginId);
                    }
                    else
                    {
                        FeedbackPlaceholder.Visible = true;
                        SaveFeedbackLabel.SetErrorMessage("Not Successful", "The preview plug-in was not registered");
                    }
                }
                else
                {
                    //update active status text
                    PluginActiveCheckbox.Text = (PluginActiveCheckbox.Checked ? "Yes" : "No");

                    FeedbackPlaceholder.Visible = true;
                    SaveFeedbackLabel.SetSuccessMessage("Plug-in Saved", "The plug-in was successfully updated");
                }
            }
            else
            {
                FeedbackPlaceholder.Visible = true;
                SaveFeedbackLabel.SetErrorMessage("Not Successful", "Preview plug-ins cannot be registered with invalid plugin files");
            }

        }

        protected void PluginUnregisterLinkButton_Click(object sender, EventArgs e)
        {
            // Get the plugin
            Plugin plugin = GetCurrentPlugin();

            int oldId = plugin.PluginId.GetValueOrDefault();

            //unregister it
            int pluginId = ContextInfo.PluginManager.UnregisterPlugin(plugin);

            if (oldId > 0 && oldId != pluginId)
            {
                Session["FeedbackLabel"] = "unregistered";
                //update details
                ReloadPlugin(pluginId);
            }
            else
            {
                FeedbackPlaceholder.Visible = true;
                SaveFeedbackLabel.SetErrorMessage("Not Successful", "The preview plug-in could not be unregistered");

            }
        }

        protected void DownloadPluginPackLinkButton_Click(object sender, EventArgs e)
        {
            // Get the plugin
            Plugin plugin = GetCurrentPlugin();

            // plugin folder path
            string absPath = plugin.PluginPath;

            // Exit if plugin folder doesn't exist
            if (!Directory.Exists(absPath))
                return;

            // Temp path to zip file
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");

            // Zip up the plugin folder
            FastZip fz = new FastZip();
            fz.CreateZip(path, absPath, true, null);

            // Send it
            string downloadFilename = plugin.Name + ".zip";
            FileUtils.TransmitFile(path, downloadFilename, HttpContext.Current);
        }


        protected void ViewManifestLinkButton_Click(object sender, EventArgs e)
        {
            ViewManifestPlaceHolder.Visible = !ViewManifestPlaceHolder.Visible;

            if (ViewManifestPlaceHolder.Visible)
            {
                ViewManifestLinkButton.Text = "[-] Hide Manifest";
                // Get the plugin ID
                int pluginId = WebUtils.GetIntRequestParam("PluginId", 0);
                // Get the plugin
                Plugin plugin = ContextInfo.PluginManager.GetAnyPluginById(pluginId);

                //get the resources for the plugin          
                List<string> resources = PluginManager.GetResources(plugin);

                if (resources.Count > 0)
                {
                    ViewManifestRepeater.DataSource = resources;
                    ViewManifestRepeater.DataBind();
                }

                NoManifestPlaceHolder.Visible = (resources.Count == 0);

            }
            else
            {
                ViewManifestLinkButton.Text = "[+] View Manifest";
            }

        }


        protected void EditContactLinkButton_Command(object sender, CommandEventArgs e)
        {
            //get the context type to edit
            string contextName = e.CommandArgument.ToString().Replace(" ", "");
            ContextType contextType = (ContextType)Enum.Parse(typeof(ContextType), contextName);

            EditContext(contextType);
        }

        protected void TestContextLinkButton_Command(object sender, CommandEventArgs e)
        {
            //get the context type to edit
            string contextName = e.CommandArgument.ToString().Replace(" ", "");
            ContextType contextType = (ContextType)Enum.Parse(typeof(ContextType), contextName);
            TestContext(contextType);
        }


        #region  Editor Event Handlers

        protected void EditorSaveCloseButton_Click(object sender, EventArgs e)
        {
            ContextType contextType = CurrentContextType;
            SaveMarkup(contextType);
            ContextEditorPlaceHolder.Visible = false;
        }

        protected void EditorSaveTestButton_Click(object sender, EventArgs e)
        {
            ContextType contextType = CurrentContextType;
            SaveMarkup(contextType);
            TestContext(contextType);
        }

        protected void EditorCancelButton_Click(object sender, EventArgs e)
        {
            ContextEditorPlaceHolder.Visible = false;
        }


        #endregion


        #region Tester Event Handlers

        protected void ContextTesterEditButton_Click(object sender, EventArgs e)
        {
            ContextType contextType = CurrentContextType;
            EditContext(contextType);
        }

        protected void ContextTesterCancelButton_Click(object sender, EventArgs e)
        {
            ContextTesterPlaceHolder.Visible = false;
        }

        protected void SwapTesterAssetLinkButton_Click(object sender, EventArgs e)
        {
            int assetId = CurrentAssetID;
            Int32.TryParse(ContextTesterAssetIDTextbox.Text, out assetId);
            CurrentAssetID = assetId;

            ContextType contextType = CurrentContextType;
            TestContext(contextType);
        }
        
        #endregion


        #endregion

        #region Private Methods

        private void ReloadPlugin(int pluginId)
        {
            Response.Redirect("ManagePluginsForm.aspx?PluginId=" + pluginId.ToString());
        }

        private Plugin GetCurrentPlugin()
        {
            int pluginId = WebUtils.GetIntRequestParam("PluginId", 0);

            // Get the plugin
            return ContextInfo.PluginManager.GetAnyPluginById(pluginId);
        }


        private void BindContexts()
        {
            //get context types
            string[] contextTypes = Enum.GetNames(typeof(ContextType));
            //format them so they're more user-friendly
            List<string> typeList = (from c in contextTypes select StringUtils.SplitAtCapitalLetters(c)).ToList();

            ContextsRepeater.DataSource = typeList;
            ContextsRepeater.DataBind();
        }

        private void EditContext(ContextType contextType)
        {

            Plugin plugin = GetCurrentPlugin();
            IPluginContext context = PluginManager.GetContext(plugin, contextType);

            if (context != null)
            {
                //get markup
                string strMarkup = ContextInfo.PluginManager.GetMarkup(plugin, contextType);
                ContextEditorTextArea.Text = strMarkup;
                ContextEditorCaptionLabel.Text = "Edit: " + (PluginManager.GetContext(plugin, contextType).Name ?? "Unnamed");

                ContextEditorPlaceHolder.Visible = true;
                ContextTesterPlaceHolder.Visible = false;

                CurrentContextType = contextType;
            }
        }


        private void TestContext(ContextType contextType)
        {
            Plugin plugin = GetCurrentPlugin();
            IPluginContext context = PluginManager.GetContext(plugin, contextType);

            if (context != null)
            {
                //get the asset for testing
                Asset asset = Asset.Empty;
                int assetId = CurrentAssetID;

                if (assetId > 0)
                {
                    asset = Asset.Get(assetId);
                }

                if (asset.IsNull)
                {
                    if (plugin.HasPluginFile)
                    {
                        //no asset currently selected so find a suitable 
                        //one to display in the asset preview control
                        foreach (string extension in plugin.PluginFile.FileExtensions)
                        {
                            AssetFinder finder = new AssetFinder { FileExtension = extension };
                            asset = Asset.FindOne(finder);
                            if (!asset.IsNull)
                            {
                                break; //asset found
                            }
                        }
                    }
                }

                //set asset in testing panel
                ContextTesterAssetIDTextbox.Text = asset.AssetId.GetValueOrDefault().ToString();

                //set the asset, context and pluginid for the viewer to use
                ContextTesterAssetPreview.ContextType = contextType;
                ContextTesterAssetPreview.PluginId = plugin.PluginId.GetValueOrDefault();
                ContextTesterAssetPreview.Asset = asset;

                ContextTesterPlaceHolder.Visible = true;
                ContextEditorPlaceHolder.Visible = false;

                CurrentContextType = contextType;
                CurrentAssetID = asset.AssetId.GetValueOrDefault();

            }
        }

        private void SaveMarkup(ContextType contextType)
        {
            Plugin plugin = GetCurrentPlugin();
            string markUp = ContextEditorTextArea.Text;
            ContextInfo.PluginManager.SaveMarkup(plugin, contextType, markUp);
        }

        #endregion

    }
}