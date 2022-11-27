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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using Orientation = System.Web.UI.WebControls.Orientation;

namespace FocusOPEN.Website.Controls
{
    public partial class MetadataFilters : BaseUserControl
	{
        public int BrandId
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "BrandId", 0);
            }
            set { ViewState["BrandId"] = value; }
        }

        public string Template
        {
            get
            {
                return TemplateHtmlLiteral.Text;
                //return SiteUtils.GetFromStore(ViewState, "Template", "<i>no filter template was loaded..</i>");
            }
            set
            {
                TemplateHtmlLiteral.Text = value;
                //ViewState["Template"] = value;
            }
        }

        /// <summary>
        /// property to store temporary selectables as input by the user and to be used 
        /// upon previewing the meta filter
        /// </summary>
        public List<BrandMetadataSetting> PreviewSelectables { get; set; }

        /// <summary>
        /// added the dynamic initialisations of controls here in an attempt to address
        /// a problem with postback data not being persisted to the controls that later
        /// fixed in another way but still creating the controls here makes sense as
        /// this is where it should be done anyway
        /// </summary>
        protected override void CreateChildControls()
        {
            LoadFilter();

            //LoadControlsState();

            base.CreateChildControls();
        }

        /// <summary>
        /// for some unidentifiable reason the state is not loaded to the dynamically added 
        /// meta inputs alhtough it's submitted to the server via the post - we will do it here 
        /// by ourselves
        /// </summary>
//        private void LoadControlsState()
//        {
//            foreach (MetadataInputWrapper input in TemporaryMetaControlsPlaceHolder.Controls)
//            {
//                foreach (Control underlyingControl in input.Controls)
//                {//set the values of the controls lying in the input's collection
//                    var val = Request.Form[underlyingControl.ClientID];
//
//                    if(string.IsNullOrEmpty(val)) continue;
//
//                    if(underlyingControl is DropDownList)
//                        ((DropDownList) underlyingControl).SelectedValue = val;
//
//
//
//                }
//                var id = input.ClientID;
                //
//            }  
//        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //-----------
            var groupsParsed = ParseTemplate();

            //----------- 
            //get controls out of the placeholder - we don't really need them rendered
            //with the rest of the html as this would mean we have to remove them afterwards
            //we had them in the placeholder as we needed to preserve their state across postabacks
            //and let asp.net do that for us
            var controls = new List<MetadataInputWrapper>();
            while(TemporaryMetaControlsPlaceHolder.Controls.Count >0)
            {
                controls.Add((MetadataInputWrapper)TemporaryMetaControlsPlaceHolder.Controls[0]);
                TemporaryMetaControlsPlaceHolder.Controls.RemoveAt(0);
            }

            //-----------
            using (var htmlwriter = new HtmlTextWriter(new System.IO.StringWriter()))
            {
                //----------- 
                //get how html looks before we've positioned the inputs as per the template
                base.Render(htmlwriter);
                var html = htmlwriter
                                .InnerWriter
                                .ToString();
                
                //-----------
                //deal with inputs one by one
                foreach (var input in controls)
                {
                    var filterGroupNum = input.BrandMetaSetting.SelectableSetting.FilterGroup;

                    var grp = groupsParsed.First(g => g.num == filterGroupNum);

                    var inputHtml = RenderControl(input);

                    //remove control html from the placeholder spot where it resided temporarily - we need
                    //to put it where it belongs in the template
                    //html = html.Replace(inputHtml, "");
                    //html = Regex.Replace(html, inputHtml, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    //add the label
                    var labelHtml = String.Format(@"<span class=""PanelTxt Bold""><span style=""{0}"">{1}</span></span><br />"
                                                            ,grp.labelStyle
                                                            ,input.BrandMetaSetting.FieldName);

                    html = Regex.Replace(html, grp.rawXml, labelHtml + inputHtml, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    
                }

                //make sure all groups xml is removed from the resulting html - there might be leftovers
                //from the groups that don't have their controls set in ConfigureMetaFilters yet - we don't want
                //junk stuff hanging around on the client side
                foreach (var g in groupsParsed)
                    html = Regex.Replace(html, g.rawXml, "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                writer.Write(html);
            }
        }

        private List<ParsedGroup> ParseTemplate()
        {
            var groupsParsed = new List<ParsedGroup>();
            //parse template as there is some data related to how the inputs should be rendered 
            //stuck in there
            var templateGroups = Regex.Matches(Template, @"<group\b[^>]*>(.*?)</group>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (var tg in templateGroups)
            {
                var s = tg.ToString();
                var grp = new XmlDocument();
                try
                {
                    grp.LoadXml(s);
                }
                catch {return null;}

                var ok = false;
                var p = new ParsedGroup(){ rawXml = s };
                
                //load saved values
                foreach (XmlNode setting in grp.ChildNodes[0].ChildNodes)
                {
                    if (setting.Name == "number")
                    {
                        int n;
                        ok = int.TryParse(setting.InnerText, out n);
                        p.num = ok? n : -1;
                    }
                    if (setting.Name == "labelStyle") p.labelStyle = setting.InnerText;
                    if (setting.Name == "inputStyle") p.inputStyle = setting.InnerText;
                    if (setting.Name == "inputBlankValue") p.inputVal = setting.InnerText;
                    if (setting.Name == "inputBlankText") p.inputTxt = setting.InnerText;
                }

                groupsParsed.Add(p);
            }

            return groupsParsed;
        }

        private string RenderControl(Control ctrl)
        {
            var sb = new StringBuilder();
            var tw = new StringWriter(sb);
            var hw = new HtmlTextWriter(tw);

            ctrl.RenderControl(hw);
            return sb.ToString();
        }

        protected override void  OnInit(EventArgs e)
        {
 	        FileSizeDropDownList.Attributes["OnChange"] = string.Format("toggleFileSize(this, '{0}')", FileSizeTextBox.ClientID);
			FileSizeTextBox.Attributes["disabled"] = "disabled"; 
//            Upload users can search their own unpublished assets
//            Brand Admins can search unpublished assets in their primary brand
//            Super Admin can search all unpublished assets
            IncludeUnpublishedExpiredAssets.Visible = GeneralUtils.ValueIsInList(CurrentUser.UserRole, UserRole.UploadUser, UserRole.BrandAdministrator, UserRole.SuperAdministrator);
            
            base.OnInit(e);
        }

        /// <summary>
        /// loads the current brand filter template and inputs
        /// </summary>
        public void LoadFilter()
        {
            if(BrandId<= 0) return;

            //use preview template if set if not
            //then use the brand one, 
            //if not then the system default
            Template = string.IsNullOrEmpty(Template) ? BrandManager.GetBrandFilterMarkup(BrandId) : Template;
            var parsedGroups = ParseTemplate();
            if(parsedGroups == null) throw new Exception("template could not be parsed. examine whether you are supplying valid xml.");

            //use preview selectables if available 
            //if not query the database
            var all = PreviewSelectables ?? BrandMetadataSettingManager.GetCustomMetadataSettings(BrandId);
            var enabledFilters = all
                .Where(s => s.UiControlType == (int)BrandMetadataUiControlType.Select)
                .ToList()
                .Where(s => s.SelectableSetting.FilterGroup > 0)
                .ToList();

            //reset controls so that other previous calls to this routine are not taken into account
            //and controls that are not required (for ex. the ones persisted from the previous postback) 
            //are not stuffed in here with the rest
            TemporaryMetaControlsPlaceHolder.Controls.Clear();

            foreach (var f in enabledFilters)
            {
                var parsedGrp = parsedGroups.FirstOrDefault(g => g.num == f.SelectableSetting.FilterGroup);
                
                //no group defined in the template or set in the config - therefore we shouldn't 
                //be bothered with it and continue to the next
                if (parsedGrp.num <= 0 || f.SelectableSetting.FilterGroup <= 0) continue;

                var input = new MetadataInputWrapper();
                //visualise as its set to be seen on the catalogue and not as in admin 
                //we are basically overriding the default brand meta setting values for the metadata input
                //this way
                f.SelectableSetting.SelectableType = f.SelectableSetting.FilterSelectableType;
                f.SelectableSetting.Depth = f.SelectableSetting.FilterDepth;
                
                input.SetTempBrandMetaSetting(f);
                input.IncludeJqueryReference = false;
                input.GroupNumber = f.GroupNumber;
                input.BrandId = BrandId;
                input.ResetState();
                input.InitInput(true);
                //apply the settings from the template
                input.ApplyCustomControlAttributes(parsedGrp.inputStyle, parsedGrp.inputTxt, parsedGrp.inputVal);
                input.RefreshFromBrandAndSelet(BrandId, new int[] { }, string.Empty);

                TemporaryMetaControlsPlaceHolder.Controls.Add(input);
            }
        }

        public event EventHandler FilterSearchClick;

        protected void OnFilterSearchClick(EventArgs e)
        {
            if (FilterSearchClick != null)
            {
                FilterSearchClick(this, e);
            }
        }           

		protected void Page_Load(object sender, EventArgs e)
		{
//			SiteUtils.TieButton(KeywordsTextBox, SearchButton1);
//
//			if (SavedUserAssetSearch == null || SavedUserAssetSearch.AssetFinder == null)
//				SavedUserAssetSearch = new SavedUserAssetSearch();
//
//			AssetFinder finder = SavedUserAssetSearch.AssetFinder;
//
//			if (!Page.IsPostBack)
//			{
//				string path = VirtualPathUtility.ToAppRelative(Request.Url.AbsolutePath).ToLower();
//
//				if (path.StartsWith("~/searchresults.aspx"))
//				{
					// If browse option is selected
//					if (Request.QueryString["showCategories"] != null)
//					{
//						SavedUserAssetSearch.CategoriesOpen = true;
//						SavedUserAssetSearch.FilterOpen = false;
//					}
//
					// Setup UI
//					KeywordsTextBox.Text = finder.GeneralKeyword;
//					BrandDropDownList1.SafeSelectValue(finder.BrandId);
//					AssetTypeDropDownList1.SafeSelectValue(finder.AssetTypeId);
//
					// Only open the filter and populate the dropdowns if the filter is open
//					if (SavedUserAssetSearch.CategoriesOpen || SavedUserAssetSearch.FilterOpen || WebUtils.GetIntRequestParam("openfilter", 0) == 1)
//					{
//						ToggleSearchVisibility(SavedUserAssetSearch.FilterOpen, SavedUserAssetSearch.CategoriesOpen);
//					}
//				}
//
//				CategoryId = SavedUserAssetSearch.CurrentCategoryId;
//			}
//
//			SetupCategories();
		}

        protected void SearchButton_Click(object sender, EventArgs e)
		{
            OnFilterSearchClick(EventArgs.Empty);
		}

		protected void ResetSearchButton_Click(object sender, EventArgs e)
		{
//			SavedUserAssetSearch.AssetFinder = SearchManager.GetBaseAssetFinder(CurrentUser);
//			SavedUserAssetSearch.AssetFinder.BrandId = WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();
//			SavedUserAssetSearch.SelectCategory(0);
//			Response.Redirect("~/SearchResults.aspx", false);
		}

		public void AddAdvancedSearchCriteria(ref AssetFinder finder)
		{
		    // Don't add anything if filter search is disabled
			if (WebsiteBrandManager.GetBrand().HideFilterSearch)
				return;
        //-------------------------------------------------------------------
        // Metadata input selections
        //-------------------------------------------------------------------
		    foreach (MetadataInputWrapper input in TemporaryMetaControlsPlaceHolder.Controls)
		    {
                IEnumerable<int> metaIds;

                input.GetSelection(out metaIds);

                if(metaIds!=null)
                    finder.MetadataIds[input.GroupNumber] = metaIds.ToList();
		    }

			if (FileSizeDropDownList.SelectedValue != "any")
			{
				long parsedFileSize = ConvertUserInputToFileSize(FileSizeTextBox.Text);

				if (parsedFileSize > 0)
				{
					CompareType compareType = GeneralUtils.ParseEnum(FileSizeDropDownList.SelectedValue, CompareType.Exact);
					finder.AddComplexCriteria(Asset.Columns.FileSize, parsedFileSize, compareType);
				}
			}

		//-------------------------------------------------------------------
		// Production date criteria
		//-------------------------------------------------------------------
            finder.FromProductionDay = FromDayDropDownList.SelectedId;
            finder.FromProductionMonth = FromMonthDropDownList.SelectedId;
            finder.FromProductionYear = FromYearDropDownList.SelectedId;
            finder.ToProductionDay = ToDayDropDownList.SelectedId;
            finder.ToProductionMonth = ToMonthDropDownList.SelectedId;
            finder.ToProductionYear = ToYearDropDownList.SelectedId;


		//-------------------------------------------------------------------
		// Asset type specific criteria
		//-------------------------------------------------------------------
			if (AssetTypeCache.Instance.GetById(finder.AssetTypeId).FileExtensionList.Select(extension => AssetTypeInfo.Get(extension)).Any(ati => ati.HasOrientation))
			{
				if (OrientationDropDownList.SelectedValue != "all")
					finder.Orientation = GeneralUtils.ParseEnum(OrientationDropDownList.SelectedValue, Data.Orientation.All);
			}

			SetupHiddenAssetOptions(finder);
		}

		private void SetupHiddenAssetOptions(AssetFinder finder)
		{
		// Update finder
			finder.IncludeUnpublishedExpiredAssets = IncludeUnpublishedExpiredAssets.Checked;

		// The option is not enabled, so no special logic is required
			if (!finder.IncludeUnpublishedExpiredAssets)
				return;

		// The option is enabled by a super admin so we simply reset the finder
		// so that it doesn't restrict the search just to completely published assets
			if (CurrentUser.UserRole == UserRole.SuperAdministrator)
			{
				finder.IsCompletelyPublished = false;
				return;
			}

		// Brand admins can only see unpublished & expired assets in their
		// own brand, so we need to restrict the search accordingly.
			if (CurrentUser.UserRole == UserRole.BrandAdministrator)
				finder.IncludeUnpublishedExpiredAssets_BrandId = CurrentUser.PrimaryBrandId;

		// Users can always see their own assets
			finder.IncludeUnpublishedExpiredAssets_UserId = CurrentUser.UserId.GetValueOrDefault();
		}

		private static long ConvertUserInputToFileSize(string userInput)
		{
			try
			{
				decimal input = Convert.ToDecimal(userInput);
				return Convert.ToInt64(input*1024*1024);
			}
			catch
			{
				return 0;
			}
		}

		private static string ConvertFileSizeToUserInput(long fileSize)
		{
			try
			{
				return Convert.ToString(Decimal.Round(fileSize/1024M/1024M, 2));
			}
			catch
			{
				return string.Empty;
			}
		}

        public void ToggleAssetTypeFilters(AssetType assetType)
        {
            // Enable the orientation dropdown if any extensions in this group support orientation
            OrientationDropDownList.Enabled = (assetType.FileExtensionList.Select(extension => AssetTypeInfo.Get(extension)).Any(ati => ati.HasOrientation));
            //
            // Reset to the first option if orientation is not supported            
            if (!OrientationDropDownList.Enabled)
            	OrientationDropDownList.SelectedIndex = 0;
        }

        public void LoadFilterFromStoredValues(AssetFinder finder)
        {
            // load lists and dropdowns
            foreach (MetadataInputWrapper input in TemporaryMetaControlsPlaceHolder.Controls)
            {
                var ids = finder.GetMetadataIds(input.GroupNumber);
                input.SetSelection(ids);
            }

            IncludeUnpublishedExpiredAssets.Checked = finder.IncludeUnpublishedExpiredAssets;

			//Select filesize
			ComplexCriteria fileSizeCriteria = finder.GetSingleComplexCriteria(Asset.Columns.FileSize);
			if (fileSizeCriteria != null)
			{
				//Convert the entered filesize from bytes to megabytes by dividing it up (1024/1024)
				FileSizeTextBox.Text = ConvertFileSizeToUserInput(Convert.ToInt64(fileSizeCriteria.Value));

				ListItem fileSizeListItem = FileSizeDropDownList.Items.FindByValue(fileSizeCriteria.CompareType.ToString());
				if (fileSizeListItem != null)
				{
					FileSizeDropDownList.SelectedIndex = -1;
					FileSizeTextBox.Attributes.Remove("disabled");
					fileSizeListItem.Selected = true;
				}
			}

			//Select production date dropdowns
            FromDayDropDownList.SafeSelectValue(finder.FromProductionDay);
            FromMonthDropDownList.SafeSelectValue(finder.FromProductionMonth);
            FromYearDropDownList.SafeSelectValue(finder.FromProductionYear);
            ToDayDropDownList.SafeSelectValue(finder.ToProductionDay);
            ToMonthDropDownList.SafeSelectValue(finder.ToProductionMonth);
            ToYearDropDownList.SafeSelectValue(finder.ToProductionYear);


			//Select orientation
			ListItem li = OrientationDropDownList.Items.FindByValue(finder.Orientation.ToString().ToLower());
			if (li != null)
			{
				OrientationDropDownList.SelectedIndex = -1;
				li.Selected = true;
			}
        }

        public void DisableButtons()
        {
            SearchButton2.Enabled = false;
            ResetSearchButton2.Enabled = false;
        }
	}

    struct ParsedGroup
    {
        public int    num;
        public string labelStyle;
        public string inputStyle;
        public string inputVal;
        public string inputTxt;
        
        public string rawXml;
    }
}