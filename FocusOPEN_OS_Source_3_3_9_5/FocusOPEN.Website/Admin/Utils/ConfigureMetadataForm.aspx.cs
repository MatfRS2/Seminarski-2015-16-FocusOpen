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
using System.Web.Services;
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
	public partial class ConfigureMetadataForm : BaseAdminPage
	{
		#region Private Variables

		private readonly List<MetadataOption> m_MetadataOptions = new List<MetadataOption>();
        private List<BrandMetadataSetting> m_MetadataCached = new List<BrandMetadataSetting>();

		#endregion

		#region Constructor

		private void ConfigureMetadataOptions()
		{
			// Metadata options have different settings.  Some should not display checkboxes as those values are always
			// displayed.  Others have checkboxes which are always disabled, to indicate that the content is required.
			// We set up this logic below, by specifying each field and the display options for each setting:
			// Display on asset form, Required, Display on asset detail (info popup).

			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.DATE_UPLOADED, "Date", true, DisplayOption.NA, DisplayOption.NA, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.FILENAME, "Text", true, DisplayOption.NA, DisplayOption.NA, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.FILEHASH, "Text", true, DisplayOption.NA, DisplayOption.NA, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.FILESIZE, "Text", true, DisplayOption.NA, DisplayOption.NA, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.ASSET_TYPE, "Dropdown", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.DATE_PRODUCED, "Date", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.ORIGINATOR, "Text", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.TITLE, "Text", true, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.PROJECT_CODE, "Text", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.BRAND, "Dropdown", true, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.CATEGORIES, "Hierarchy", true, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.PRIMARY_CATEGORY, "Dropdown", true, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.DESCRIPTION, "Text", true, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.ADDITIONAL_KEYWORDS, "Text", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.COPYRIGHT_OWNER, "Text", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.USAGE_RESTRICTIONS, "Text", true, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.ATTACHED_FILES, "File", true, DisplayOption.StandardCheckBox, DisplayOption.NA, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.LINKED_ASSETS, "Text", true, DisplayOption.StandardCheckBox, DisplayOption.NA, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.DOWNLOAD_RESTRICTIONS, "Radio", true, DisplayOption.StandardCheckBox, DisplayOption.NA, DisplayOption.NA));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.CONTACT_EMAIL, "Text", true, DisplayOption.DisabledCheckBox, DisplayOption.DisabledCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.PUBLICATION_DATE, "Date", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.EXPIRY_DATE, "Date", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.FEEDBACK_FORM, "Form", false, DisplayOption.NA, DisplayOption.NA, DisplayOption.StandardCheckBox));
			m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.SHOW_FILE_METADATA, "Text", false, DisplayOption.StandardCheckBox, DisplayOption.NA, DisplayOption.StandardCheckBox));


            //			for (int i = 1; i <= Settings.NumberOfMetadataFields; i++)
            //				m_MetadataOptions.Add(new MetadataOption(BrandMetadataSettings.METADATA_PREFIX + i, "Dropdown", true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox));

		}

		protected ConfigureMetadataForm()
		{
            PageId = "Utils.ConfigureMetadata";
		    ConfigureMetadataOptions();
		}

		#endregion

		#region Properties
        
	    protected ConfigureMetadataFormMode Mode
	    {
	        get
	        {
	            return EnumUtils.GetEnumFromValue<ConfigureMetadataFormMode>(ConfigurationModeRadios.SelectedValue);
	        }
	    }

	    private int BrandId
	    {
	        get
	        {
                var brandId = GetRequiredQueryStringParameter("BrandId", "ConfigureMetadata.aspx");

	            return brandId;
	        }
	    }

        /// <summary>
        /// temporary repository to keep what the user selected in between changing the mode (and 
        /// the rendered repeater data) via the mode radios
        /// </summary>
        private Dictionary<int, List<bool>> CatalogueModeTempSelection
	    {
	        get
	        {
                var all = SiteUtils.GetFromStore(ViewState, "CatalogueModeTempSelection", new Dictionary<int, List<bool>>());

                return all;
	        }
            set { ViewState["CatalogueModeTempSelection"] = value; }
	    }

        /// <summary>
        /// temporary repository to keep what the user selected in between changing the mode (and 
        /// the rendered repeater data) via the mode radios
        /// </summary>
        private Dictionary<int, List<bool>> AssetDetailModeTempSelection
        {
            get
            {
                var all = SiteUtils.GetFromStore(ViewState, "AssetDetailModeTempSelection", new Dictionary<int, List<bool>>());

                return all;
            }
            set { ViewState["AssetDetailModeTempSelection"] = value; }
        }

        #endregion
        
        #region Page Events

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
				if (brand.IsNull)
					Response.Redirect("ConfigureMetadata.aspx");

				// Get all of the metadata settings for this brand
			    BindRepeater();
                
                // General brand options
				HideFilterSearchCheckBox.Checked = brand.HideFilterSearch;
				HideCategorySearchCheckBox.Checked = brand.HideCategorySearch;
			}
		}

	    protected void MetadataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
			{
				// Get the brand metadata setting
				BrandMetadataSetting setting = (BrandMetadataSetting)e.Item.DataItem;

				// Get the option details
				MetadataOption option = GetMetadataOption(setting.FieldId, setting.UiControlType, setting.IsCustom);

				// Populate the hidden field
				HiddenField BrandMetadataSettingIdHiddenField = (HiddenField)e.Item.FindControl("BrandMetadataSettingIdHiddenField");
				BrandMetadataSettingIdHiddenField.Value = setting.BrandMetadataSettingId.ToString();
                
				// Populate the field name label
				Label FieldNameLabel = (Label)e.Item.FindControl("FieldNameLabel");
				FieldNameLabel.Text = setting.FieldName;
				FieldNameLabel.ToolTip = string.Format("Field ID: {0}", setting.FieldId);

                // Populate detail edition control
                var uc = (BrandMetadataSettingDetails)e.Item.FindControl("ucEditDetails");
                uc.LoadData(setting);

                // show delete option only for custom metadata
                var DeletePromptLinkButton = (PromptLinkButton) e.Item.FindControl("DeletePromptLinkButton");
			    DeletePromptLinkButton.Visible = setting.IsCustom;

				// Populate the field name textbox (for editing)
//				TextBox FieldNameTextBox = (TextBox) e.Item.FindControl("FieldNameTextBox");
//				FieldNameTextBox.Text = setting.FieldName;

				// Populate the uiControlType label
				Label DescriptionLabel = (Label)e.Item.FindControl("DescriptionLabel");
                DescriptionLabel.Text = option.UIControlType;

                // do not allow checking and grey name if in asset detail mode 
                // AND catalogue is not checked
                if(Mode == ConfigureMetadataFormMode.AssetDetail && !setting.OnAssetForm)
                {
                    var ExpandCollapseDetailsAnchor = (HtmlAnchor)e.Item.FindControl("ExpandCollapseDetailsAnchor");
                    ExpandCollapseDetailsAnchor.Attributes["class"] += " greyedBoldTxt";
                    if (!setting.OnAssetDetail)
                        option.AssetDetailOption = DisplayOption.DisabledCheckBox;
                }
                
				// Populate the checkboxes
				SetUI("Catalogue", option.AssetFormOption, e.Item, setting.OnAssetForm);
				SetUI("Required", option.RequiredOption, e.Item, setting.IsRequired);
                SetUI("AssetDetail", option.AssetDetailOption, e.Item, setting.OnAssetDetail);// && option.AssetDetailOption == DisplayOption.StandardCheckBox);

				// move up down image buttons
                var UpOrderImageButton = (ImageButton)e.Item.FindControl("UpOrderImageButton");
                var DownOrderImageButton = (ImageButton)e.Item.FindControl("DownOrderImageButton");
			    UpOrderImageButton.CommandArgument = setting.BrandMetadataSettingId.GetValueOrDefault().ToString();
                DownOrderImageButton.CommandArgument = setting.BrandMetadataSettingId.GetValueOrDefault().ToString();
			    UpOrderImageButton.CommandName = "-"; //minus because in order to push up we decrease the order number
			    DownOrderImageButton.CommandName = "+";
//			      UpOrderImageButton.Enabled = m_MetadataCached.First().BrandMetadataSettingId != setting.BrandMetadataSettingId;
//                DownOrderImageButton.Enabled = m_MetadataCached.Last().BrandMetadataSettingId != setting.BrandMetadataSettingId;


                // switch between visible sets of columns depending on current mode
                var AssetDetailTd = (HtmlTableCell)e.Item.FindControl("AssetDetailTd");
                var CatalogueTd = (HtmlTableCell)e.Item.FindControl("CatalogueTd");
                var RequiredTd = (HtmlTableCell)e.Item.FindControl("RequiredTd");
			    AssetDetailTd.Visible = Mode == ConfigureMetadataFormMode.AssetDetail;
                CatalogueTd.Visible = Mode == ConfigureMetadataFormMode.Catalogue;
                RequiredTd.Visible = Mode == ConfigureMetadataFormMode.Catalogue;
			}

            if (e.Item.ItemType == ListItemType.Footer)
            {
                InitAddNewInterface(e.Item);
            }

            if (e.Item.ItemType == ListItemType.Header)
            {
                // switch between visible sets of columns depending on current mode
                var AssetDetailHeaderTd = (HtmlTableCell)e.Item.FindControl("AssetDetailHeaderTd");
                var CatalogueHeaderTd = (HtmlTableCell)e.Item.FindControl("CatalogueHeaderTd");
                var RequiredHeaderTd = (HtmlTableCell)e.Item.FindControl("RequiredHeaderTd");
                
                AssetDetailHeaderTd.Visible = Mode == ConfigureMetadataFormMode.AssetDetail;
                
                CatalogueHeaderTd.Visible = Mode == ConfigureMetadataFormMode.Catalogue;
                RequiredHeaderTd.Visible = Mode == ConfigureMetadataFormMode.Catalogue;
            }
		}
		protected void CancelFieldNameLinkButton_Click(object sender, EventArgs e)
		{
            ScriptManager.RegisterStartupScript((LinkButton)sender, typeof(string), "initCtrlsScript", "InitControls();", true);

			//TogglePanels(sender, false);
		}
		protected void DeletePromptLinkButton_Click(object sender, EventArgs e)
		{
            LinkButton lb = (LinkButton)sender;

            for (var i = 0; i < MetadataRepeater.Controls.Count; i++)
            {
                var ctrl = MetadataRepeater.Controls[i];

                if (!(ctrl is RepeaterItem)) continue;

                var ri = (RepeaterItem)ctrl;

                if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem, ListItemType.Footer))
                {
                    LinkButton LinkButton = (LinkButton)ri.FindControl(lb.ID);

                    if (lb.UniqueID.Equals(LinkButton.UniqueID))
                    {
                        HiddenField BrandMetadataSettingIdHiddenField = (HiddenField)ri.FindControl("BrandMetadataSettingIdHiddenField");
                        int brandMetadataSettingId = NumericUtils.ParseInt32(BrandMetadataSettingIdHiddenField.Value, 0);

                        if (brandMetadataSettingId > 0)
                        {
                            try
                            {

                                BrandMetadataSettingManager.DeleteSettingAndValues(brandMetadataSettingId);

                                var container = (HtmlGenericControl)ri.FindControl("DetailsContainerDiv");
                                var openContainerScript = String.Format("HideRow('{0}');", container.ClientID);

                                ScriptManager.RegisterClientScriptBlock(
                                            Page,
                                            Page.GetType(),
                                            "hide-deleted",
                                            string.Format("InitControls(); {0}", openContainerScript), true);
                                
                                return;
                            }
                            catch (SystemException sysex)
                            {
                                ShowAlertErrorMessage(ri, sysex.Message);
                                return;
                            }
                        }
                       

                        //ensure controls are reupdated upon ajax postback
                        ScriptManager.RegisterStartupScript((LinkButton)sender, typeof(string), "initCtrlsScript", "InitControls();", true);

                        return;
                    }
                }

            }


		}
		protected void SaveFieldNameLinkButton_Click(object sender, EventArgs e)
		{
			LinkButton lb = (LinkButton)sender;

            for (var i = 0; i < MetadataRepeater.Controls.Count; i++ )
            {
                var ctrl = MetadataRepeater.Controls[i];

                if (!(ctrl is RepeaterItem)) continue;

                var ri = (RepeaterItem)ctrl;

                if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem, ListItemType.Footer))
                {
                    LinkButton LinkButton = (LinkButton)ri.FindControl(lb.ID);

                    if (lb.UniqueID.Equals(LinkButton.UniqueID))
                    {
                        // Get UI elements to be used for updating
                        var CatalogueCheckBox = (CheckBox)ri.FindControl("CatalogueCheckBox");
                        var RequiredCheckBox = (CheckBox)ri.FindControl("RequiredCheckBox");
                        var AssetDetailCheckBox = (CheckBox)ri.FindControl("AssetDetailCheckBox");

                        //get texts
                        var uc = (BrandMetadataSettingDetails)ri.FindControl("ucEditDetails");

                        HiddenField BrandMetadataSettingIdHiddenField = (HiddenField)ri.FindControl("BrandMetadataSettingIdHiddenField");
                        int brandMetadataSettingId = NumericUtils.ParseInt32(BrandMetadataSettingIdHiddenField.Value, 0);

                        if (brandMetadataSettingId > 0)
                        {
                            try
                            {
                                Label FieldNameLabel = (Label)ri.FindControl("FieldNameLabel");
                                Label FeedbackLabel1 = (Label)ri.FindControl("FeedbackLabel1");
                                FeedbackLabel1.Text = string.Empty;

                                var setting = BrandMetadataSetting.Get(brandMetadataSettingId);
                                var option = GetMetadataOption(setting.FieldId, setting.UiControlType, setting.IsCustom);

                                uc.RetrieveData(ref setting);

                                if (string.IsNullOrEmpty(setting.FieldName))
                                    throw new SystemException("A field name must be specified!");

                                SetRowUIToClosed(ri);

                                if(Mode == ConfigureMetadataFormMode.Catalogue)
                                {
                                    setting.OnAssetForm = GetCheckboxValue(CatalogueCheckBox);
                                    setting.IsRequired = (option.RequiredOption != DisplayOption.NA && setting.OnAssetForm && GetCheckboxValue(RequiredCheckBox));
                                }
                                else
                                {
                                    setting.OnAssetDetail = (setting.OnAssetForm && GetCheckboxValue(AssetDetailCheckBox));
                                }

                                BrandMetadataSetting.Update(setting);

                                InvalidateCaches();
                                
                                FieldNameLabel.Text = setting.FieldName;
                                FeedbackLabel1.Text = "saved";
                            }
                            catch (SystemException sysex)
                            {
                                ShowAlertErrorMessage(ri, sysex.Message);
                                return;
                            }
                        }
                        else
                        {//adding new custom metadata entry
                            int brandId = GetRequiredQueryStringParameter("BrandId", "ConfigureMetadata.aspx");

                            var dropDown = (DropDownList)ri.FindControl("UiControlTypeDropDown");

                            var setting = BrandMetadataSetting.New();

                            setting.IsCustom = true;
                            setting.BrandId = brandId;
                            setting.FieldId = GetNewCustomMetadataFieldId();
                            
                            uc.RetrieveData(ref setting);

                            if (string.IsNullOrEmpty(setting.FieldName))
                            {
                                const string errMsg = "A field name must be specified!";
                                ShowAlertErrorMessage(ri, errMsg);
                                GeneralFeedbackLabel.SetErrorMessage(errMsg);
                                return;
                            }

                            SetRowUIToClosed(ri);

                            setting.UiControlType = int.Parse(dropDown.SelectedValue);

                            setting.OnAssetForm = CatalogueCheckBox.Checked;
                            setting.IsRequired = RequiredCheckBox.Checked;
                            //setting.OnAssetDetail = AssetDetailCheckBox.Checked;

                            //deal with order numbers - newly created setting is last for both modes
                            var allSettings = GetMetadataSettings(BrandId);
                            var tempSett = allSettings.OrderBy(s => s.CatalogueOrderNum).LastOrDefault();
                            setting.CatalogueOrderNum = tempSett == null ? 1 : tempSett.CatalogueOrderNum + 1;
                            tempSett = allSettings.OrderBy(s => s.AssetDetailOrderNum).LastOrDefault();
                            setting.AssetDetailOrderNum = tempSett == null ? 1 : tempSett.AssetDetailOrderNum + 1;

                            BrandMetadataSetting.Update(setting);
                            
                            InvalidateCaches();

                            //now also add the default selectable setting if meta setting is of type selectable
                            if(setting.UiControlType == (int)BrandMetadataUiControlType.Select)
                            {
                                var sel = BrandMetadataSelectableSetting.GetDefault();
                                sel.BrandMetadataSettingId = setting.BrandMetadataSettingId.GetValueOrDefault();
                                BrandMetadataSelectableSetting.Update(sel);
                            }

                            BindRepeater();

                            GeneralFeedbackLabel.SetSuccessMessage("Metadata scheme saved successfully");

                        }

                        //ensure controls are reupdated upon ajax postback
                        ScriptManager.RegisterStartupScript((LinkButton)sender, typeof(string), "initCtrlsScript", "InitControls();", true);

                        return;
                    }
                }

            }
		}
        protected void UpDownImageButton_Command(object sender, ImageClickEventArgs e)
        {
            var button = (ImageButton) sender;
            var brSettingId = int.Parse(button.CommandArgument);
            var settings = GetSettingsDependingOnMode();
            var setting = settings.FirstOrDefault(s => s.BrandMetadataSettingId == brSettingId);
            if(setting == null) return;
            
            BrandMetadataSetting toBeChanged = null;
            if (button.CommandName == "+")
            {
                if(Mode == ConfigureMetadataFormMode.Catalogue)
                    toBeChanged = settings.Where(s => s.BrandMetadataSettingId != setting.BrandMetadataSettingId &&
                                        s.CatalogueOrderNum > setting.CatalogueOrderNum)
                        .FirstOrDefault();
                if(Mode == ConfigureMetadataFormMode.AssetDetail)
                    toBeChanged = settings.Where(s => s.BrandMetadataSettingId != setting.BrandMetadataSettingId &&
                                        s.AssetDetailOrderNum > setting.AssetDetailOrderNum)
                        .FirstOrDefault();
            }
            else if(button.CommandName == "-")
            {
                if (Mode == ConfigureMetadataFormMode.Catalogue)
                    toBeChanged = settings.Where(s => s.BrandMetadataSettingId != setting.BrandMetadataSettingId &&
                                        s.CatalogueOrderNum < setting.CatalogueOrderNum)
                        .LastOrDefault();
                if (Mode == ConfigureMetadataFormMode.AssetDetail)
                    toBeChanged = settings.Where(s => s.BrandMetadataSettingId != setting.BrandMetadataSettingId &&
                                        s.AssetDetailOrderNum < setting.AssetDetailOrderNum)
                        .LastOrDefault();
            }

            if(toBeChanged!=null)
            {
                int tmp;
                if (Mode == ConfigureMetadataFormMode.Catalogue)
                {
                    tmp = setting.CatalogueOrderNum;
                    setting.CatalogueOrderNum = toBeChanged.CatalogueOrderNum;
                    toBeChanged.CatalogueOrderNum = tmp;
                }else
                {
                    tmp = setting.AssetDetailOrderNum;
                    setting.AssetDetailOrderNum = toBeChanged.AssetDetailOrderNum;
                    toBeChanged.AssetDetailOrderNum = tmp;
                }

                BrandMetadataSetting.Update(toBeChanged);
                BrandMetadataSetting.Update(setting);

                InvalidateCaches();
            }

            BindRepeater();
        }
	    protected void SaveButton_Click(object sender, EventArgs e)
	    {
	        var all = GetCurrentRepeaterSelection();

            foreach (RepeaterItem ri in MetadataRepeater.Items)
            {
                if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
                {
                    // Get the metadatasetting record ID to be updated
                    var BrandMetadataSettingIdHiddenField = (HiddenField) ri.FindControl("BrandMetadataSettingIdHiddenField");
                    var CatalogueCheckBox = (CheckBox)ri.FindControl("CatalogueCheckBox");
                    var RequiredCheckBox = (CheckBox)ri.FindControl("RequiredCheckBox");
                    var AssetDetailCheckBox = (CheckBox)ri.FindControl("AssetDetailCheckBox");

                    var brandMetadataSettingId = NumericUtils.ParseInt32(BrandMetadataSettingIdHiddenField.Value, 0);

                    var setting = all.FirstOrDefault(b => b.BrandMetadataSettingId == brandMetadataSettingId);
                    if(setting==null) continue;

                    //restore the non selected mode's checkboxes if there was one
                    //this helps when the user changed anything in mode 1 (say catalogue)
                    //and then moved on to mode 2 (asset details) and clicked save there
                    if (Mode == ConfigureMetadataFormMode.AssetDetail && CatalogueModeTempSelection.ContainsKey(brandMetadataSettingId))
                    {
                        setting.IsRequired  = CatalogueModeTempSelection[brandMetadataSettingId][0] || setting.OnAssetDetail;
                        setting.OnAssetForm = CatalogueModeTempSelection[brandMetadataSettingId][1];

                    }else if (Mode == ConfigureMetadataFormMode.Catalogue && AssetDetailModeTempSelection.ContainsKey(brandMetadataSettingId))
                    {
                        setting.OnAssetDetail = AssetDetailModeTempSelection[brandMetadataSettingId][0] && setting.OnAssetForm;
                        //make sure on asset detail is only displayed if on asset form is displayed as well
                        //setting.OnAssetDetail = setting.OnAssetDetail && setting.OnAssetForm;
                    }

                    //save setting
                    BrandMetadataSetting.Update(setting);

                    // Update UI checkboxes with the right checkbox combinations
                    RequiredCheckBox.Checked = setting.IsRequired;
                    CatalogueCheckBox.Checked = setting.OnAssetForm;
                    AssetDetailCheckBox.Checked = setting.OnAssetDetail;
                }
            }

            //reset mode temp selectec persistance logic so that stuff doesn't get messed up
            //later
            CatalogueModeTempSelection = AssetDetailModeTempSelection = null;

            // Get the brand being edited
            var brandId = GetRequiredQueryStringParameter("BrandId", "ConfigureMetadata.aspx");
            var brand = Brand.Get(brandId);

            // Update filter and category search options
            brand.HideFilterSearch = HideFilterSearchCheckBox.Checked;
            brand.HideCategorySearch = HideCategorySearchCheckBox.Checked;

            // Save it
            Brand.Update(brand);

            InvalidateCaches();
            
//            refresh repeater to reflect possible ajax changes before this postback
//            otherwise the old state before the ajax calls that is stored in the viewstate
//            would be rendered again to the client
            BindRepeater();
            
            GeneralFeedbackLabel.SetSuccessMessage("Metadata scheme saved successfully");

//            foreach (RepeaterItem ri in MetadataRepeater.Items)
//			{
//				if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
//				{
					// Get the metadatasetting record ID to be updated
//					HiddenField BrandMetadataSettingIdHiddenField = (HiddenField)ri.FindControl("BrandMetadataSettingIdHiddenField");
//					int brandMetadataSettingId = NumericUtils.ParseInt32(BrandMetadataSettingIdHiddenField.Value, 0);
//
					// Get UI elements to be used for updating
//					CheckBox CatalogueCheckBox = (CheckBox) ri.FindControl("CatalogueCheckBox");
//					CheckBox RequiredCheckBox = (CheckBox)ri.FindControl("RequiredCheckBox");
//					CheckBox AssetDetailCheckBox = (CheckBox)ri.FindControl("AssetDetailCheckBox");
//					TextBox AdditionalCopyTextBox = (TextBox)ri.FindControl("AdditionalCopyTextBox");
//					TextBox ToolTipTextBox = (TextBox)ri.FindControl("ToolTipTextBox");
//
					// Get the setting and the metadat option
//					BrandMetadataSetting setting = BrandMetadataSetting.Get(brandMetadataSettingId);
//                    MetadataOption option = GetMetadataOption(setting.FieldId, setting.UiControlType, setting.IsCustom);
//
					// Update the brand metadata setting in the database
//					setting.OnAssetForm = GetCheckboxValue(CatalogueCheckBox);
//					setting.IsRequired = (option.RequiredOption != DisplayOption.NA && setting.OnAssetForm && GetCheckboxValue(RequiredCheckBox));
//					setting.OnAssetDetail = (setting.OnAssetForm && GetCheckboxValue(AssetDetailCheckBox));
//					setting.AdditionalCopy = AdditionalCopyTextBox.Text.Trim();
//					setting.ToolTip = ToolTipTextBox.Text.Trim();
//					BrandMetadataSetting.Update(setting);
//
					// Update UI checkboxes
//					RequiredCheckBox.Checked = setting.IsRequired;
//					CatalogueCheckBox.Checked = setting.OnAssetForm;
//					AssetDetailCheckBox.Checked = setting.OnAssetDetail;
//				}
//
				// Get the brand being edited
//				int brandId = GetRequiredQueryStringParameter("BrandId", "ConfigureMetadata.aspx");
//				Brand brand = Brand.Get(brandId);
//
				// Update filter and category search options
//				brand.HideFilterSearch = HideFilterSearchCheckBox.Checked;
//				brand.HideCategorySearch = HideCategorySearchCheckBox.Checked;
//
				// Save it
//				Brand.Update(brand);
//
//			}
//
//            InvalidateCaches();
//
            //refresh repeater to reflect possible ajax changes before this postback
            //otherwise the old state before the ajax calls that is stored in the viewstate
            //would be rendered again to the client
//            BindRepeater();
//
//			GeneralFeedbackLabel.SetSuccessMessage("Metadata scheme saved successfully");
		}
        protected void ConfigurationModeRadios_SelectedIndexChanged(object sender, EventArgs e)
        {
            //preserve previous selection so that we know the user chose and are able
            //to save that selection with the other mode ones upon a 'save all' button click
            var all = GetCurrentRepeaterSelection();

            if (Mode == ConfigureMetadataFormMode.AssetDetail)
            {//we are in asset detail mode therefore the catalogue one was chosen before
                //i.e. repeater still contains the catalogue stuff so tore that
                CatalogueModeTempSelection = all.ToDictionary(b => b.BrandMetadataSettingId.GetValueOrDefault(), b => new List<bool>() { b.IsRequired, b.OnAssetForm });
            }
            else if (Mode == ConfigureMetadataFormMode.Catalogue)
            {//we are in catalogue mode therefore the asset detail one was chosen before
                //i.e. repeater still contains the asset details stuff so tore that
                AssetDetailModeTempSelection = all.ToDictionary(b => b.BrandMetadataSettingId.GetValueOrDefault(), b => new List<bool>() { b.OnAssetDetail });    
            }

            //rebind list
            BindRepeater();
        }

		#endregion

		#region Private Metadata Stuff

		private static bool GetCheckboxValue(CheckBox cb)
		{
			if (!cb.Visible)
				return true;

			if (!cb.Enabled)
				return true;

			return cb.Checked;
		}

		private MetadataOption GetMetadataOption(string field, int uiControlType, bool isCustom)
		{
			foreach (MetadataOption option in m_MetadataOptions)
				if (StringUtils.IgnoreCaseCompare(option.FieldId, field))
					return option;

            if (isCustom)
            {//custom added metadata has the same display settings plus the control type depends on what's stored in the database
                var uiControlTypeName = EnumUtils.GetDescription<BrandMetadataUiControlType>(uiControlType);
                return new MetadataOption(field, uiControlTypeName, true, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox, DisplayOption.StandardCheckBox);
            }
 
			throw new SystemException(string.Format("Field '{0}' has not been configured", field));
		}

		/// <summary>
		/// Internal class representing a metadata option, that belongs to a brand and contains details about
		/// metadata such as the field ID, uiControlType, and the radio options available for it.
		/// </summary>
		private class MetadataOption
		{
			public string FieldId { get; private set; }
			public string UIControlType { get; private set; }
			public bool CanRenameDescription { get; private set; }
			public DisplayOption AssetFormOption { get; private set; }
			public DisplayOption RequiredOption { get; private set; }
			public DisplayOption AssetDetailOption { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="MetadataOption"/> class.
			/// </summary>
			/// <param name="fieldName">Name of the field.</param>
			/// <param name="uiControlType">The uiControlType of the field to make it easier to identify.</param>
			/// <param name="canRename">Boolean value specifying whether uiControlType can be renamed</param>
			/// <param name="assetFormOption">The asset form option.</param>
			/// <param name="requiredOption">The required option.</param>
			/// <param name="assetDetailOption">The asset detail option.</param>
			public MetadataOption(string fieldName, string uiControlType, bool canRename, DisplayOption assetFormOption, DisplayOption requiredOption, DisplayOption assetDetailOption)
			{
				FieldId = fieldName;
				UIControlType = uiControlType;
				CanRenameDescription = canRename;
				AssetFormOption = assetFormOption;
				RequiredOption = requiredOption;
				AssetDetailOption = assetDetailOption;
			}
		}

		private enum DisplayOption
		{
			NA,
			DisabledCheckBox,
			StandardCheckBox
		}

        protected enum ConfigureMetadataFormMode
        {
            Catalogue = 1,
            AssetDetail = 2
        }

		#endregion

		#region Helper Methods

        private void BindRepeater()
        {
            var settings = GetSettingsDependingOnMode();

            //preserve previous selection so that we know the user chose and are able
            //to save that selection with the other mode ones upon a 'save all' button click
            foreach (var setting in settings)
            {//get and override any preserved settings stored from before

                var brandMetadataSettingId = setting.BrandMetadataSettingId.GetValueOrDefault();
                
                if (CatalogueModeTempSelection.ContainsKey(brandMetadataSettingId))
                {
                    setting.IsRequired = CatalogueModeTempSelection[brandMetadataSettingId][0];
                    setting.OnAssetForm = CatalogueModeTempSelection[brandMetadataSettingId][1];
                }
                if (AssetDetailModeTempSelection.ContainsKey(brandMetadataSettingId))
                {
                    setting.OnAssetDetail = AssetDetailModeTempSelection[brandMetadataSettingId][0];    
                }
                
                //apply the interrelated checkboxes logic here
                setting.OnAssetDetail = setting.OnAssetForm && setting.OnAssetDetail;
                setting.IsRequired = setting.IsRequired || setting.OnAssetDetail;
            }


            m_MetadataCached = settings;

            // Bind repeater to the settings list
            MetadataRepeater.DataSource = settings;
            MetadataRepeater.DataBind();
        }

	    private List<BrandMetadataSetting> GetSettingsDependingOnMode()
	    {
            var settings = GetMetadataSettings(BrandId);

            //order settings depending on which mode we are in
            settings = (ConfigureMetadataFormMode.Catalogue == Mode
                            ? settings.OrderBy(s => s.CatalogueOrderNum)
                            : settings.OrderBy(s => s.AssetDetailOrderNum))
                .ToList();

	        return settings;
	    }

	    private void InitAddNewInterface(RepeaterItem footer)
        {
            var FooterRow = (HtmlTableRow)footer.FindControl("FooterRow");

            //hide add new if metadata limit is exceeded
            int brandId = GetRequiredQueryStringParameter("BrandId", "ConfigureMetadata.aspx");
            var customCount = GetMetadataSettings(brandId).Where(m => m.IsCustom).Count();
            if (customCount >= Settings.NumberOfMetadataFields)
            {//limit exceeded - hide add new
                FooterRow.Visible = false;
                return;
            }

            //make sure add new footer is visible if in catalogue mode only
            if (Mode != ConfigureMetadataFormMode.Catalogue)
            {
                FooterRow.Visible = false;
                return;
            }

            FooterRow.Visible = true;

            //ui type drop down
            var dropDown = (DropDownList)footer.FindControl("UiControlTypeDropDown");
            dropDown.DataSource = EnumUtils.GetList<BrandMetadataUiControlType>();
            dropDown.DataBind();

            //the textboxes and areas
            var uc = (BrandMetadataSettingDetails)footer.FindControl("ucEditDetails");

            var metaName = GetNewCustomMetadataFieldId();

            metaName = metaName.Replace(BrandMetadataSettings.METADATA_PREFIX, BrandMetadataSettings.METADATA_PREFIX + " ");

            uc.ResetUI(metaName);

            //the checkboxes
            foreach (var pref in new[] { "Catalogue", "Required" })//, "AssetDetail" })
            {
                var checkbox = (CheckBox)footer.FindControl(pref + "CheckBox");
                checkbox.Checked = (pref == "Catalogue" || pref == "AssetDetail");
            }
        }

        private void SetRowUIToClosed(RepeaterItem ri)
        {
            var container = (HtmlGenericControl)ri.FindControl("DetailsContainerDiv");
            container.Attributes["class"] = "detailsContainer";
            var literal = (Literal)ri.FindControl("NameSignLiteral");
            literal.Text = literal.Text.Replace("[-]", "[+]");
        }

        private void ShowAlertErrorMessage(RepeaterItem ri, string message)
        {
            //retrieve the container's ID so that we can open the details container
            var container = (HtmlGenericControl)ri.FindControl("DetailsContainerDiv");
            container.Attributes["class"] = "detailsContainer2";
//            var openContainerScript = String.Format("OpenDetails('{0}');", container.ClientID);
//            openContainerScript = "";
            var literal = (Literal)ri.FindControl("NameSignLiteral");
            literal.Text = literal.Text.Replace("[+]", "[-]");

            ScriptManager.RegisterClientScriptBlock(
                        Page,
                        Page.GetType(),
                        "fieldname-error-alert",
                        string.Format("InitControls(); alert('{0}');", SiteUtils.MakeJsSafe(message)), true);
                        //string.Format("$(document).ready(function() {{ alert('{0}'); InitControls();}});", SiteUtils.MakeJsSafe(message)), true);
            return;
        }

		private List<BrandMetadataSetting> GetMetadataSettings(int brandId)
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
			if (settings.Count == 0)
			{
				// Still no settings so try and add these using our configuration settings
				foreach (MetadataOption option in m_MetadataOptions)
					AddMetadataSettingToDatabase(brandId, option, settings);
			}

			// There should be the same number of settings in the database as there are default metadata
			// options or this means that these are out of sync. In this case, add the new setting to the list.

			if (settings.Count < m_MetadataOptions.Count)
			{
				// First get the missing settings.  These are items that exist in the metadata options list
				// but not in the list we got from the database.
				var missingSettings = (from o in m_MetadataOptions
				                       where !settings.Any(s => s.FieldId == o.FieldId)
				                       select o);

				// Now add them to the database and the settings list
				foreach (var setting in missingSettings)
					AddMetadataSettingToDatabase(brandId, setting, settings);
			}

			return settings;
		}

		private static void AddMetadataSettingToDatabase(int brandId, MetadataOption option, List<BrandMetadataSetting> settings)
		{
			BrandMetadataSetting setting = BrandMetadataSetting.New();
			setting.BrandId = brandId;
			setting.FieldId = option.FieldId;
			setting.FieldName = GeneralUtils.SplitIntoSentence(option.FieldId);
			setting.IsRequired = (GeneralUtils.ValueIsInList(option.RequiredOption, DisplayOption.DisabledCheckBox, DisplayOption.NA));
			setting.OnAssetForm = (GeneralUtils.ValueIsInList(option.AssetFormOption, DisplayOption.DisabledCheckBox, DisplayOption.NA));
			setting.OnAssetDetail = (GeneralUtils.ValueIsInList(option.AssetDetailOption, DisplayOption.DisabledCheckBox, DisplayOption.NA));
			setting.AdditionalCopy = string.Empty;
			setting.ToolTip = string.Empty;
			BrandMetadataSetting.Update(setting);

			settings.Add(setting);
		}

		private void TogglePanels(object linkbutton, bool editing)
		{
            return;

			LinkButton lb = (LinkButton)linkbutton;

			foreach (RepeaterItem ri in MetadataRepeater.Items)
			{
				if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
				{
					LinkButton LinkButton = (LinkButton)ri.FindControl(lb.ID);

					if (lb.UniqueID.Equals(LinkButton.UniqueID))
					{
                        var container = (HtmlGenericControl)ri.FindControl("DetailsContainerDiv");
					    container.Style["display"] = "none";
					    container.Style.Add("display", "none");

//						Panel ViewPanel = (Panel)ri.FindControl("ViewPanel");
//						Panel EditPanel = (Panel)ri.FindControl("EditPanel");
//
//						ViewPanel.Visible = !editing;
//						EditPanel.Visible = editing;


					}
				}
			}
		}

		private static void SetUI(string controlPrefix, DisplayOption option, RepeaterItem item, bool selected)
		{
			Label label = (Label)item.FindControl(controlPrefix + "Label");
			CheckBox checkbox = (CheckBox)item.FindControl(controlPrefix + "CheckBox");

			if (option == DisplayOption.NA)
			{
				label.Text = "N/A";
				checkbox.Visible = false;
			}

			if (option == DisplayOption.DisabledCheckBox)
			{
				label.Visible = false;
                checkbox.Checked = selected;
				checkbox.Enabled = false;
			}

			if (option == DisplayOption.StandardCheckBox)
			{
				label.Visible = false;
				checkbox.Visible = true;
				checkbox.Checked = selected;
			}
		}

        private static string GetNewCustomMetadataFieldId()
        {
            var f = new BrandMetadataSettingFinder() {IsCustom = true};

            var existing = BrandMetadataSetting.FindMany(f);

            var stringIds = existing
                            .Where(e => e.FieldId.ToLower().StartsWith(BrandMetadataSettings.METADATA_PREFIX.ToLower()))
                            .Select(e => e.FieldId.Replace(BrandMetadataSettings.METADATA_PREFIX, ""))
                            .ToList();

            int tmpInt;

            var orderedIntegers = stringIds
                    .Where(s => int.TryParse(s, out tmpInt))
                    .Select(s => int.Parse(s))
                    .OrderByDescending(i => i)
                    .ToList();

            //find the last integer postfix for the list, increment that and return as the new metadata's fieldId
            var number = orderedIntegers.Count > 0 ? orderedIntegers.First() + 1 : 1;

            return String.Format("{0}{1}", BrandMetadataSettings.METADATA_PREFIX, number);
        }

        private static void InvalidateCaches()
        {
            BrandMetadataSettingCache.Instance.InvalidateCache();
            BrandCache.Instance.InvalidateCache();
            CacheManager.InvalidateCache("BrandMetadata", CacheType.All);
        }

        private List<BrandMetadataSetting> GetCurrentRepeaterSelection()
        {
            var result = new List<BrandMetadataSetting>();

            foreach (RepeaterItem ri in MetadataRepeater.Items)
            {
                if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
                {
                    // Get the metadatasetting record ID to be updated
                    var BrandMetadataSettingIdHiddenField = (HiddenField) ri.FindControl("BrandMetadataSettingIdHiddenField");
                    int brandMetadataSettingId = NumericUtils.ParseInt32(BrandMetadataSettingIdHiddenField.Value, 0);

                    // Get UI elements to be used for updating
                    var CatalogueCheckBox = (CheckBox) ri.FindControl("CatalogueCheckBox");
                    var RequiredCheckBox = (CheckBox) ri.FindControl("RequiredCheckBox");
                    var AssetDetailCheckBox = (CheckBox) ri.FindControl("AssetDetailCheckBox");

                    // Get the setting and the metadat option
                    var setting = BrandMetadataSetting.Get(brandMetadataSettingId);
                    var option = GetMetadataOption(setting.FieldId, setting.UiControlType, setting.IsCustom);

                    // Update the brand metadata setting in the database
                    setting.OnAssetForm     = GetCheckboxValue(CatalogueCheckBox);
                    setting.IsRequired      = (option.RequiredOption != DisplayOption.NA && setting.OnAssetForm && GetCheckboxValue(RequiredCheckBox));
                    setting.OnAssetDetail   = (setting.OnAssetForm && AssetDetailCheckBox.Checked);//GetCheckboxValue(AssetDetailCheckBox));
                    
                    result.Add(setting);
                }
            }

            return result;
        }

		#endregion

	    #region Ajax Handlers

	    [WebMethod]
        public static string MoveUpDown(string swappedA, string swappedB, int mode)
	    {
//            var finder = new BrandMetadataSettingFinder { BrandId = brandId };
//            var settings = BrandMetadataSetting.FindMany(finder);
	        var id1 = int.Parse(swappedA);
	        var id2 = int.Parse(swappedB);

            var sett1 = BrandMetadataSetting.Get(id1);
            var sett2 = BrandMetadataSetting.Get(id2);
	        int tempOrderNum;

            if (mode == (int)ConfigureMetadataFormMode.AssetDetail)
            {
                tempOrderNum = sett1.AssetDetailOrderNum;
                sett1.AssetDetailOrderNum = sett2.AssetDetailOrderNum;
                sett2.AssetDetailOrderNum = tempOrderNum;
            }else
            {
                tempOrderNum = sett1.CatalogueOrderNum;
                sett1.CatalogueOrderNum = sett2.CatalogueOrderNum;
                sett2.CatalogueOrderNum = tempOrderNum;
            }

            BrandMetadataSetting.Update(sett1);
            BrandMetadataSetting.Update(sett2);
            
            InvalidateCaches();

	        return "ok";
	    }

        [WebMethod]
        public static string DragMeta(string ids, int mode)
        {
            var list = ids
                        .Split(","[0])
                        .Where(NumericUtils.IsInt32)
                        .Select(i=>NumericUtils.ParseInt32(i, -1))
                        .ToList();

            var f = new BrandMetadataSettingFinder();
            f.BrandMetadataSettingIdList.AddRange(list);
            
            var all = BrandMetadataSetting.FindMany(f);

            var counter = 1;
            foreach (var id in list)
            {
                var setting = all.FirstOrDefault(i => i.BrandMetadataSettingId == id);
                if(setting==null) continue;

                if (mode == (int)ConfigureMetadataFormMode.AssetDetail)
                    setting.AssetDetailOrderNum = counter;
                else
                    setting.CatalogueOrderNum = counter;

                counter++;
            }

            //persist to DB
            foreach (var setting in all)
                BrandMetadataSetting.Update(setting);

            InvalidateCaches();

            return "ok";
        }

	    #endregion
	}
}