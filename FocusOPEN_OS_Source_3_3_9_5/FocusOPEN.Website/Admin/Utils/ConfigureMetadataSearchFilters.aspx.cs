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
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;
using FocusOPEN.Website.Controls.Admin;

namespace FocusOPEN.Website.Admin
{
	public partial class ConfigureMetadataSearchFilters : BaseAdminPage
	{
		#region Constructor

        protected ConfigureMetadataSearchFilters()
		{
            PageId = "Utils.ConfigureMetadataSearchFilters";
		}

		#endregion

		#region Properties
        
	    private int BrandId
	    {
	        get
	        {
                if (string.IsNullOrEmpty(Request["BrandId"]))
                    Page.Response.Redirect("ConfigureMetadata.aspx?redirect=search-filters", true);

	            var brandId = int.Parse(Request["BrandId"]);

	            return brandId;
	        }
	    }

        private int SelectedGroup
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "SelectedGroup", 0);
            }
            set { ViewState["SelectedGroup"] = value; }
        }

        /// <summary>
        /// preserves the selection for the group selectable types - only the ones
        /// that have been changed so that they can be persisted to the database
        /// when changes are saved
        /// </summary>
        private Dictionary<int,int> GroupSelectableTypeChanges
        {
            get
            {
                if (ViewState["GroupSelectableTypeChanges"] == null)
                {
                    var dd = new Dictionary<int, int>();

                    ViewState["GroupSelectableTypeChanges"] = dd;
                }

                return (Dictionary<int, int>)ViewState["GroupSelectableTypeChanges"];
            }
        }

        /// <summary>
        /// preserves the selection for the group selectable rows count - only the ones
        /// that have been changed so that they can be persisted to the database
        /// when changes are saved
        /// </summary>
        private Dictionary<int, int> GroupSelectableRowsCountChanges
        {
            get
            {
                if (ViewState["GroupSelectableRowsCountChanges"] == null)
                {
                    var dd = new Dictionary<int, int>();

                    ViewState["GroupSelectableRowsCountChanges"] = dd;
                }

                return (Dictionary<int, int>)ViewState["GroupSelectableRowsCountChanges"];
            }
        }

        private Dictionary<int, int> Selection
        {
            get
            {
                if (ViewState["Selection"] == null)
                {
                    var dd = Enumerable.Range(1, 12).ToDictionary(k => k, k => 0);

                    var fromDb = SelectableSettings
                            .Where(s => s.SelectableSetting.FilterGroup > 0)
                            .Select(s => new
                            {
                                s.SelectableSetting.FilterGroup,
                                s.SelectableSetting.BrandMetadataSelectableSettingId
                            })
                            .ToList();

                    //populate values
                    foreach (var pair in fromDb)
                    {
                        if (pair.FilterGroup > 0 && pair.FilterGroup <= 12)
                            dd[pair.FilterGroup] = pair.BrandMetadataSelectableSettingId.GetValueOrDefault();
                    }

                    ViewState["Selection"] = dd;
                }

                return (Dictionary<int, int>)ViewState["Selection"];
            }
            set { ViewState["Selection"] = value; }
        }

        private List<BrandMetadataSetting> selectables;

        /// <summary>
        /// returns all meta settings of type selectable for the current brand
        /// </summary>
	    private List<BrandMetadataSetting> SelectableSettings
	    {
	        get
	        {
                if(selectables==null)
                {
                    var all = BrandMetadataSettingManager.GetCustomMetadataSettings(BrandId);
                    selectables = all
                        .Where(s => s.UiControlType == (int)BrandMetadataUiControlType.Select)
                        .ToList()
                        .Where(s => s.SelectableSetting.SelectableType != (int)SelectableMetadataType.PresetTextArea)
                        .ToList();

                    for(var i = 0; i < selectables.Count; i++)
                    {
                        var sel = selectables[i];
                        if(sel.SelectableSetting.IsNew || sel.SelectableSetting.IsNull)
                        {//selectable setting is missing - insert the default one
                            var newsel = BrandMetadataSelectableSetting.GetDefault();
                            newsel.BrandMetadataSettingId = sel.BrandMetadataSettingId.GetValueOrDefault();
                            BrandMetadataSelectableSetting.Update(newsel);
                            selectables[i].SelectableSetting = newsel;
                        }
                    }
                }

	            return selectables;
	        }
	    }

        private BrandMetadataSetting GetSelectedMetaSetting()
        {
            var selId = GetSelectedSelectableSettingId();

            if(selId <= 0) return null;

            var brandMetaSetting = SelectableSettings.FirstOrDefault(s => s.SelectableSetting.BrandMetadataSelectableSettingId == selId);

            return brandMetaSetting;
        }

        #endregion
        
        #region Page Events
        
        protected void Page_Init(object sender, EventArgs e)
        {
            //

        }

        protected void Page_Load(object sender, EventArgs e)
		{
			// Ensure brand ID is specified
            int brandId = BrandId;

			// Ensure brand admin is accessing their own brand
			if (CurrentUser.UserRole.Equals(UserRole.BrandAdministrator) && !brandId.Equals(CurrentUser.PrimaryBrandId))
				Response.Redirect("~/Admin/Default.aspx");

			// Get the brand
			Brand brand = Brand.Get(brandId);

			// Set the page header
			PageHeaderDiv.InnerText = string.Format("Configure Metadata for {0}", brand.Name);

			if (!Page.IsPostBack)
			{
				// Ensure we have a brand
				if (brand.IsNull) Response.Redirect("ConfigureMetadata.aspx?redirect=search-filters", true);

				// Get all of the metadata settings for this brand
			    BindDataList();
                
                // load markup
			    MarkupTextBox.Text = BrandManager.GetBrandFilterMarkup(brandId);

                // General brand options
				EnableFiltersCheckbox.Checked = !brand.HideFilterSearch;
			}
            else
            {
                //=======
                //make sure the preview state of preview list/drop down is preserved - otherwise they'll just load
                //what's stored in the database
                //load the other one as it should show - otherwise the meta setting stored in the database will kick in
                LoadPreviewState();

                //=======
                //we need to query the request this way because we are overriding the
                //asp.net radiobutton behaviour with a custom implementation that would allow
                //us to have many radio inputs with the same name rendered with via a datalist
                var eventArg = Request["__EVENTARGUMENT"];

                if (!string.IsNullOrEmpty(eventArg))
                {
                    SelectedGroup = int.Parse(eventArg.Replace("@@@@@",""));
                    GroupChanged();
                    BindDataList();
                } 
            }
		}

	    protected void GroupsDataList_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
			{
				// Get the brand metadata setting
				var group = (int)e.Item.DataItem;
			    var setting = SelectableSettings.Where(s => s.SelectableSetting.FilterGroup == group).FirstOrDefault();
                
                var groupLabel          = (Label)e.Item.FindControl("GroupLabel");
                var groupRadioButton    = (Label)e.Item.FindControl("GroupRadioButtonLabel");
                var groupDropDownList   = (DropDownList)e.Item.FindControl("GroupDropDownList");

                //title the group section and store the id
			    groupLabel.Text += group;

			    //the metas dropdown selection
			    groupDropDownList.ValidationGroup += group;
                groupDropDownList.DataSource = SelectableSettings
                                                    .Where(s=>!s.SelectableSetting.IsNull)
                                                    .OrderBy(s => s.FieldName)
                                                    .Select(s=>new{s.FieldName, s.SelectableSetting.BrandMetadataSelectableSettingId })
                                                    .ToList();
                groupDropDownList.DataBind();

                if(Selection[group] > 0)
                {//set stored selection
                    groupDropDownList.SelectedValue = Selection[group].ToString();
                }

                //radio
			    var postbackGroupChanged = ClientScript.GetPostBackEventReference(GroupsDataList, "@@@@@" + group);
                groupRadioButton.Text = String.Format(
                                        @"<input type=""radio"" name= ""Group"" class=""searchFiltersGroupRadio""  value=""{0}"" 
                                                onclick=""javascript:{1}""  {2}
                                          />", 
                                                      group,
                                                      postbackGroupChanged,
                                                      SelectedGroup == group ? @"checked=""checked""" : ""
                                                      
                                                      );

                if (setting != null)
                {//there is a setting selected
                    groupDropDownList.SelectedValue = setting.SelectableSetting.BrandMetadataSelectableSettingId.ToString();
                }
			}
		}

        protected void GroupDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {

            var dd = (DropDownList) sender;
            
            //get group
            var group = int.Parse(dd.ValidationGroup.Replace("Group@@@", ""));

            //preserve selection in viewstate for now as we don't want to save to db before user confirmed
            Selection[group] = int.Parse(dd.SelectedValue);

            GroupChanged();
        }

	    protected void LoadDefaultTemplateButton_Click(object sender, EventArgs e)
	    {
	        MarkupTextBox.Text = BrandManager.GetBrandFilterMarkup(BrandId);
            GeneralFeedbackLabel.SetSuccessMessage("template markup was reset. in order to apply the new template you will have to save the changes.");
	    }

	    protected void TestChangesButton_Click(object sender, EventArgs e)
	    {
	        MetadataFilters.BrandId = BrandId;
            MetadataFilters.PreviewSelectables = GetInputSettings();
	        MetadataFilters.Template = MarkupTextBox.Text;
	        MetadataFilters.DisableButtons();

	        try
	        {
	            MetadataFilters.LoadFilter();
	        }
	        catch (Exception ex)
	        {
	            GeneralFeedbackLabel.SetErrorMessage(ex.Message);
                return;
	        }

	        MainMultiView.ActiveViewIndex = 1;
	    }

	    protected void MakeLiveButton_Click(object sender, EventArgs e)
	    {
	        var saveCandidates = GetInputSettings();

	        foreach (var setting in saveCandidates)
	        {
	            BrandMetadataSelectableSetting.Update(setting.SelectableSetting);
	        }

	        var b = Brand.Get(BrandId);
	        b.FilterMarkup = MarkupTextBox.Text;
	        Brand.Update(b);

            MainMultiView.ActiveViewIndex = 0;
            GeneralFeedbackLabel.SetSuccessMessage("changes were saved.");

	        GroupSelectableTypeChanges.Clear();
	        GroupSelectableRowsCountChanges.Clear();

            LoadPreviewState();
	    }

        protected void CancelPreviewButton_Click(object sender, EventArgs e)
	    {
            MainMultiView.ActiveViewIndex = 0;

            LoadPreviewState();
	    }

	    protected void ApplyButton_Click(object sender, EventArgs e)
        {
            var selId = GetSelectedSelectableSettingId();

            if (selId <= 0)
            {
                GeneralFeedbackLabel.SetSuccessMessage("no group was selected from the list.");
                return;
            }

	        var f = BrandMetadataSelectableSetting.Get(selId);

	        f.FilterDepth = int.Parse(RowsDropDownList.SelectedValue);
	        f.FilterSelectableType = Option2RadioButton.Checked
	                                     ? (int) SelectableMetadataType.ComboBox
	                                     : (int) SelectableMetadataType.DropDown;

	        BrandMetadataSelectableSetting.Update(f);

            GeneralFeedbackLabel.SetSuccessMessage("display settings of the selected metadata were updated.");
        }


        protected void RowsDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var brandMetaSetting = GetSelectedMetaSetting();

            if (brandMetaSetting == null) return;

            GroupSelectableRowsCountChanges[SelectedGroup] = int.Parse(RowsDropDownList.SelectedValue);

            GeneralFeedbackLabel.SetSuccessMessage("rows count applied. changes will be saved after you preview the filter.");

            LoadMetaPreview(PreviewMetaInputList, brandMetaSetting, int.Parse(RowsDropDownList.SelectedValue), SelectableMetadataType.ComboBox);
        }

        protected void OptionsRadioButton_OnCheckedChanged(object sender, EventArgs e)
        {
            if (SelectedGroup <= 0) return;

            var radbutt = ((RadioButton) sender);

            var newType = Option1RadioButton.ID == radbutt.ID ? SelectableMetadataType.DropDown : SelectableMetadataType.ComboBox;

            GroupSelectableTypeChanges[SelectedGroup] = (int) newType;
            
            //no need for this as row count state is preserved upon the dropdown change
            //GroupSelectableRowsCountChanges[SelectedGroup] = int.Parse(RowsDropDownList.SelectedValue);

            GeneralFeedbackLabel.SetSuccessMessage("filter type applied. changes will be saved after you preview the filter.");

            LoadPreviewState();
        }

		#endregion

        private void BindDataList()
        {
            var settings = Enumerable.Range(1, 12).ToList();

            // Bind repeater to the settings list
            GroupsDataList.DataSource = settings;
            GroupsDataList.DataBind();
        }

        private void GroupChanged()
        {
            var brandMetaSetting = GetSelectedMetaSetting();

            if (brandMetaSetting == null)
            {
                GroupDetailsPanel.Visible = false;
                return;
            }

            GroupDetailsPanel.Visible = true;

            //reset and load both demo controls with the current values for this meta setting
            //temp fix: try catch in a hollow structure as there is a hard to catch issue with selected value here
            try {RowsDropDownList.SelectedValue = brandMetaSetting.SelectableSetting.FilterDepth.ToString();} catch{}

            Option1RadioButton.Checked = brandMetaSetting.SelectableSetting.FilterSelectableType == (int) SelectableMetadataType.DropDown;
            Option2RadioButton.Checked = brandMetaSetting.SelectableSetting.FilterSelectableType == (int) SelectableMetadataType.ComboBox;

            //sort out the preview controls - this would also override the
            //database settings we added above with any current changes from 
            //the session that haven't been saved
            LoadPreviewState();
        }

	    private void LoadMetaPreview(MetadataInputWrapper input,BrandMetadataSetting brandMetaSetting, int depth, SelectableMetadataType type)
	    {
            brandMetaSetting.SelectableSetting.SelectableType = (int)type;
            brandMetaSetting.SelectableSetting.Depth = depth;
            
            input.SetTempBrandMetaSetting(brandMetaSetting);
            input.IncludeJqueryReference = false;
            input.GroupNumber = brandMetaSetting.GroupNumber;
            input.BrandId = BrandId;
            input.ResetState();
            input.InitInput(true);
            input.RefreshFromBrandAndSelet(BrandId, new int[] { }, string.Empty);

	    }

	    private int GetSelectedSelectableSettingId()
        {
            if (SelectedGroup <= 0) return 0;

            foreach (DataListItem ri in GroupsDataList.Items)
            {
                if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                {
                    var grpLabel = (Label) ri.FindControl("GroupLabel");

                    if (!grpLabel.Text.EndsWith(" " + SelectedGroup)) continue;

                    var dropdown = (DropDownList) ri.FindControl("GroupDropDownList");

                    return int.Parse(dropdown.SelectedValue);
                }
            }

	        return 0;
        }

        private void LoadPreviewState()
        {
            if(GroupSelectableTypeChanges.ContainsKey(SelectedGroup))
            {//apply the previously saved selection for the group 
                //filter selectable type in case we have such i.e. if a group is selected
                //AND it's selectable type was changed
                var selection = (SelectableMetadataType) GroupSelectableTypeChanges[SelectedGroup];
                Option1RadioButton.Checked = selection == SelectableMetadataType.DropDown;
                Option2RadioButton.Checked = selection == SelectableMetadataType.ComboBox;
            }

            if (GroupSelectableRowsCountChanges.ContainsKey(SelectedGroup))
            {
                RowsDropDownList.SelectedValue = GroupSelectableRowsCountChanges[SelectedGroup].ToString();
            }

            var brandMetaSetting = GetSelectedMetaSetting();

            if (brandMetaSetting != null && GroupDetailsPanel.Visible)
            {
                LoadMetaPreview(PreviewMetaInputDropDown, brandMetaSetting, 1, SelectableMetadataType.DropDown);
                LoadMetaPreview(PreviewMetaInputList, brandMetaSetting, int.Parse(RowsDropDownList.SelectedValue), SelectableMetadataType.ComboBox);
            }
        }

        /// <summary>
        /// returns all changed
        /// </summary>
        /// <returns></returns>
	    private List<BrandMetadataSetting> GetInputSettings()
	    {
            var result = new List<BrandMetadataSetting>();

	        foreach (var selected in Selection)
	        {
                //retrieve if this group had a different selection before
                var prevSelection = SelectableSettings.FirstOrDefault(s => s.SelectableSetting.FilterGroup == selected.Key);
                if (prevSelection != null && prevSelection.SelectableSetting.BrandMetadataSelectableSettingId != selected.Value)
                {//remove selection from a previously selected setting that is now changing and no longer selected
                    prevSelection.SelectableSetting.FilterGroup = 0;
                    result.Add(prevSelection);
                }

	            if(selected.Value > 0)
	            {//selected - apply selection 
                    var newSelected = SelectableSettings.FirstOrDefault(s => s.SelectableSetting.BrandMetadataSelectableSettingId == selected.Value);
	                newSelected.SelectableSetting.FilterGroup = selected.Key;
                    if (GroupSelectableTypeChanges.ContainsKey(selected.Key))
                        newSelected.SelectableSetting.FilterSelectableType = GroupSelectableTypeChanges[selected.Key];
                    if (GroupSelectableRowsCountChanges.ContainsKey(SelectedGroup))
                        newSelected.SelectableSetting.FilterDepth = GroupSelectableRowsCountChanges[SelectedGroup];
                    result.Add(newSelected);
	            }
	        }

            return result;
	    }
        
        private static List<BrandMetadataSetting> GetMetadataSettings(int brandId)
		{
			// Get all of the metadata settings for this brand
			BrandMetadataSettingFinder finder = new BrandMetadataSettingFinder { BrandId = brandId };
			List<BrandMetadataSetting> settings = BrandMetadataSetting.FindMany(finder);

			// Return settings if found
			if (settings.Count == 0)
			{
				// Otherwise, we need to get the default settings
				List<int> brandIdList = new List<int>();

				// Check the master brand first
				if (brandId != WebsiteBrandManager.GetMasterBrand().BrandId)
					brandIdList.Add(WebsiteBrandManager.GetMasterBrand().BrandId.GetValueOrDefault());

				// Then all subsequent brands
				foreach (Brand brand in BrandCache.Instance.GetList())
					if (!brandIdList.Contains(brand.BrandId.GetValueOrDefault()))
						brandIdList.Add(brand.BrandId.GetValueOrDefault());

				// Now check each brand
				foreach (int id in brandIdList)
				{
					// Get the metadata settings in this brand
					finder = new BrandMetadataSettingFinder {BrandId = id, IsCustom = false };
					settings = BrandMetadataSetting.FindMany(finder);

					if (settings.Count > 0)
					{
						// Settings found.  Copy all of them to the brand
						// being edited and drop out of the loop as we've
						// found what we're looking for.

						foreach (BrandMetadataSetting setting in settings)
						{
							setting.BrandMetadataSettingId = null;
							setting.BrandId = brandId;
							BrandMetadataSetting.Update(setting);
						}

						break;
					}
				}
			}

			// Still no settings, so copy them from the default metadata options
//			if (settings.Count == 0)
//			{
				// Still no settings so try and add these using our configuration settings
//				foreach (MetadataOption option in m_MetadataOptions)
//					AddMetadataSettingToDatabase(brandId, option, settings);
//			}
//
			// There should be the same number of settings in the database as there are default metadata
			// options or this means that these are out of sync. In this case, add the new setting to the list.
//
//			if (settings.Count < m_MetadataOptions.Count)
//			{
				// First get the missing settings.  These are items that exist in the metadata options list
				// but not in the list we got from the database.
//				var missingSettings = (from o in m_MetadataOptions
//				                       where !settings.Any(s => s.FieldId == o.FieldId)
//				                       select o);
//
				// Now add them to the database and the settings list
//				foreach (var setting in missingSettings)
//					AddMetadataSettingToDatabase(brandId, setting, settings);
//			}

			return settings;
		}

		private void TogglePanels(object linkbutton, bool editing)
		{
            return;

			LinkButton lb = (LinkButton)linkbutton;

//			foreach (RepeaterItem ri in MetadataRepeater.Items)
//			{
//				if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
//				{
//					LinkButton LinkButton = (LinkButton)ri.FindControl(lb.ID);
//
//					if (lb.UniqueID.Equals(LinkButton.UniqueID))
//					{
//                        var container = (HtmlGenericControl)ri.FindControl("DetailsContainerDiv");
//					    container.Style["display"] = "none";
//					    container.Style.Add("display", "none");
//
//						Panel ViewPanel = (Panel)ri.FindControl("ViewPanel");
//						Panel EditPanel = (Panel)ri.FindControl("EditPanel");
//
//						ViewPanel.Visible = !editing;
//						EditPanel.Visible = editing;
//
//
//					}
//				}
//			}
		}

	}
}