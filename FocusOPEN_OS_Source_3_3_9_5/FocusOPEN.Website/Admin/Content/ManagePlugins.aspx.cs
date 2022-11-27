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
using Daydream.Data;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace FocusOPEN.Website.Admin.Content
{
    public partial class ManagePlugins : BaseAdminPageablePage
    {

        #region Page Event Handlers


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                //populate the plugin status filters box
                ListItem allItem = new ListItem("All", "-1");
                allItem.Selected = true;
                PluginStatusDropDownList.Items.Add(allItem);

                ListItem registeredItem = new ListItem("Valid", "0");
                PluginStatusDropDownList.Items.Add(registeredItem);

                foreach (int v in Enum.GetValues(typeof(PluginStatus)))
                {
                    if (v > 0)
                    {
                        ListItem item = new ListItem(StringUtils.SplitAtCapitalLetters(((PluginStatus)v).ToString()), v.ToString());
                        PluginStatusDropDownList.Items.Add(item);
                    }
                }

                //bind repeater
                Bind(1);
            }

            //hides add plugin panel message if postback after displayed
            if (!AddPluginPanel.Visible)
            {
                AddPluginFeedbackLabel.Visible = false;
            }
            
        }


        protected void PluginsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
            {
                Plugin p = (Plugin)e.Item.DataItem;

                //check to see if plugin is valid
                if (p.IsUnregistered || (p.Status != PluginStatus.None && p.Status != PluginStatus.ChecksumMismatch))
                {
                    //not valid so user red for row colour
                    HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("itemRow");
                    tr.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "#ff0000");
                }

                Literal name = (Literal)e.Item.FindControl("PluginName");
                name.Text = p.Name;

                Literal folder = (Literal)e.Item.FindControl("Folder");
                folder.Text = p.RelativePath;

                Literal contexts = (Literal)e.Item.FindControl("Contexts");
                contexts.Text = PluginManager.NumberOfContexts(p).ToString();

                Literal registered = (Literal)e.Item.FindControl("Registered");
                registered.Text = (p.IsUnregistered ? "No" : "Yes");

                Literal active = (Literal)e.Item.FindControl("Active");
                active.Text = (p.IsDisabled ? "No" : "Yes");

                Label status = (Label)e.Item.FindControl("Status");
                status.Text = GetStatusText(p);
                status.ToolTip = p.FormatErrorMessage;

                Literal version = (Literal)e.Item.FindControl("Version");
                version.Text = (p.HasPluginFile?p.PluginFile.PluginVersion.ToString():"");

                Literal guid = (Literal)e.Item.FindControl("Guid");
                guid.Text = (p.IsUnregistered ? "N/A" : p.RegistrationKey.ToString());
              
            }
        }


        protected void PluginFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind(1);
        }

        protected void PageSizeDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind(1);
        }

        protected void SimplePager_OnCommand(object source, CommandEventArgs e)
        {
            int pg = Convert.ToInt32(e.CommandArgument);
            Bind(pg);
        }

        protected void SortListLinkButton_Command(object sender, CommandEventArgs e)
        {
            PluginsRepeater.UpdateSortExpression(e.CommandArgument.ToString());
            Bind(1);
        }

        protected void AddPluginButton_Click(object sender, EventArgs e)
        {
            AddPluginPanel.Visible = true;
        }

        protected void AddPluginSaveButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(PluginManager.PluginsPath))
            {
                BinaryFile pluginPack = new BinaryFile(PluginFileUpload.PostedFile);

                if (!pluginPack.IsEmpty)
                {
                    // Save the zip file to disk
                    string tempZipPath = Path.GetTempFileName() + ".zip";
                    pluginPack.SaveAs(tempZipPath);

                    // Extract the uploaded zip into the brand folder
                    FastZip fz = new FastZip();
                    fz.ExtractZip(tempZipPath, PluginManager.PluginsPath, FastZip.Overwrite.Always, null, string.Empty, string.Empty, true);

                    // Delete the temp zip file
                    File.Delete(tempZipPath);

                    //invalidate the unregistered plugins cache and re-bind
                    CacheManager.InvalidateCache("Plugins", CacheType.Local);
                    Bind(1);

                    //add success message
                    AddPluginFeedbackLabel.SetSuccessMessage("Plugin added successfully");
                    AddPluginFeedbackLabel.Visible = true;
                    AddPluginPanel.Visible = false;
                }
            }
            else
            {
                AddPluginFeedbackLabel.SetErrorMessage("Plugin path does not exist");
                AddPluginFeedbackLabel.Visible = true;
                AddPluginPanel.Visible = false;
            }
        }

        protected void AddPluginCancelButton_Click(object sender, EventArgs e)
        {
            AddPluginPanel.Visible = false;
        }


        #endregion


        #region Private methods

        private string GetStatusText(Plugin p)
        {
            //compile composite description
            StringBuilder statusText = new StringBuilder();

            if (p.Status == PluginStatus.None)
            {
                //valid plugins have a Registered status text
                statusText.AppendLine("Valid");
            }
            else
            {
                foreach (int v in Enum.GetValues(typeof(PluginStatus)))
                {
                    if (v > 0)
                    {
                        if ((p.Status & (PluginStatus)v) > 0)
                        {
                            statusText.AppendLine(StringUtils.SplitAtCapitalLetters(((PluginStatus)v).ToString()));
                            statusText.Append("<br/>");
                        }
                    }
                }
            }

            return statusText.ToString();
        }


        private void Bind(int page)
        {

            List<Plugin> plugins = GetFilteredList();
            // Get the asset count
            int total = plugins.Count;

            // Setup the pager
            BottomPager.CurrentPage = page;
            BottomPager.PageSize = PageSize;
            BottomPager.ItemCount = total;
            BottomPager.CalculatePageCount();

            // Setup page variables
            CurrentPage = page;
            RecordCount = total;
            TotalPages = BottomPager.PageCount;

            //sort the plugins
            Sort(plugins);

            // Bind the asset list to relevant page
            PluginsRepeater.DataSource = plugins.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
            PluginsRepeater.DataBind();


            // Toggle no results panel
            PluginsRepeater.Visible = (plugins.Count > 0);
            NoResultsPanel.Visible = (plugins.Count == 0);
        }


        private List<Plugin> GetFilteredList()
        {
            // Get the filter values 
            int statusFilter = -1;
            Int32.TryParse(PluginStatusDropDownList.SelectedValue, out statusFilter);

            int registeredFilter = -1;
            Int32.TryParse(PluginRegisteredDropDownList.SelectedValue, out registeredFilter);

            int activeFilter = -1;
            Int32.TryParse(PluginActiveDropDownList.SelectedValue, out activeFilter);

            //filter the AllPlugins collection
            return (from p in ContextInfo.PluginManager.AllPlugins where 
                        (registeredFilter < 0 || (registeredFilter == 0 && p.IsUnregistered) || (registeredFilter >0 && !p.IsUnregistered)) &&
                        (activeFilter < 0 || (activeFilter == 0 && p.IsDisabled) || (activeFilter > 0 && !p.IsDisabled)) &&
                        (statusFilter < 0 || (((PluginStatus)statusFilter & p.Status) > 0) || (((PluginStatus)statusFilter) == PluginStatus.None && p.Status ==0))                                              
                        select p).ToList();
        }


        private void Sort(List<Plugin> plugins)
        {
            switch (PluginsRepeater.GetSortExpressions()[0].FieldName.ToLower())
            {
                case "name":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => string.Compare(x.Name, y.Name));
                    }
                    else
                    {
                        plugins.Sort((x, y) => string.Compare(y.Name, x.Name));
                    }
                    break;
                case "guid":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => string.Compare((x.RegistrationKey == Guid.Empty ? "N/A" : x.RegistrationKey.ToString()), (y.RegistrationKey == Guid.Empty ? "N/A" : y.RegistrationKey.ToString())));
                    }
                    else
                    {
                        plugins.Sort((x, y) => string.Compare((y.RegistrationKey == Guid.Empty ? "N/A" : y.RegistrationKey.ToString()), (x.RegistrationKey == Guid.Empty ? "N/A" : x.RegistrationKey.ToString())));
                    }
                    break;
                case "version":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => decimal.Compare(x.PluginFile.PluginVersion, y.PluginFile.PluginVersion));
                    }
                    else
                    {
                        plugins.Sort((x, y) => decimal.Compare(y.PluginFile.PluginVersion, x.PluginFile.PluginVersion));
                    }
                    break;
                case "status":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => string.Compare(GetStatusText(x), GetStatusText(y)));
                    }
                    else
                    {
                        plugins.Sort((x, y) => string.Compare(GetStatusText(y),GetStatusText(x)));
                    }
                    break;
                case "folder":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => string.Compare(x.RelativePath, y.RelativePath));
                    }
                    else
                    {
                        plugins.Sort((x, y) => string.Compare(y.RelativePath, x.RelativePath));
                    }
                    break;
                case "contexts":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => string.Compare(PluginManager.NumberOfContexts(x).ToString(), PluginManager.NumberOfContexts(y).ToString()));
                    }
                    else
                    {
                        plugins.Sort((x, y) => string.Compare(PluginManager.NumberOfContexts(y).ToString(), PluginManager.NumberOfContexts(x).ToString()));
                    }
                    break;
                case "active":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => string.Compare((x.IsDisabled ? "No" : "Yes"), (y.IsDisabled ? "No" : "Yes")));
                    }
                    else
                    {
                        plugins.Sort((x, y) => string.Compare((y.IsDisabled ? "No" : "Yes"), (x.IsDisabled ? "No" : "Yes")));
                    }
                    break;
                case "registered":
                    if (PluginsRepeater.GetSortExpressions()[0].IsAscending)
                    {
                        plugins.Sort((x, y) => string.Compare((x.IsUnregistered ? "No" : "Yes"), (y.IsUnregistered ? "No" : "Yes")));
                    }
                    else
                    {
                        plugins.Sort((x, y) => string.Compare((y.IsUnregistered ? "No" : "Yes"), (x.IsUnregistered ? "No" : "Yes")));
                    }
                    break;
            }
        }


        #endregion


    }
}