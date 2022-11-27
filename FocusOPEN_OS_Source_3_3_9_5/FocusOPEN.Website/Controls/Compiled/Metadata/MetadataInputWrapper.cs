using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls.Compiled;

namespace FocusOPEN.Website.Controls
{
	public class MetadataInputWrapper : PlaceHolder
	{
	    private bool initCompleted = false;

		public int BrandId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "BrandId", 0);
			}
			set
			{
				ViewState["BrandId"] = value;
			}
		}
		public int GroupNumber
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "GroupNumber", 0);
			}
			set
			{
				ViewState["GroupNumber"] = value;
			}
        }

//        /// <summary>
//        /// Which is the catalogue filter group of the input i.e. the value that governs
//        /// where in the catalogue filter control this input is to be placed
//        /// </summary>
//        public int FilterGroupNumber
//        {
//            get
//            {
//                return SiteUtils.GetFromStore(ViewState, "FilterGroupNumber", 0);
//            }
//            set
//            {
//                ViewState["FilterGroupNumber"] = value;
//            }
//        }
        private BrandMetadataSetting brandMetaSetting;
        public BrandMetadataSetting BrandMetaSetting
        {
            get
            {
                if (brandMetaSetting == null)
                {
                    var brand = BrandCache.Instance.GetById(BrandId);
                    brandMetaSetting = brand.GetMetadataSetting(BrandMetadataSettings.METADATA_PREFIX + GroupNumber);
                }
                return brandMetaSetting;
            }
        }
	    /// <summary>
	    /// in many cases jquery had already been included on the page for us already - if that's the case
	    /// use this property to omit inclusion of the jquery script
	    /// </summary>
        public bool IncludeJqueryReference
	    {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "IncludeJqueryReference", true);
            }
            set
            {
                ViewState["IncludeJqueryReference"] = value;
            }
	    }

        public void RefreshFromBrandAndSelet(int brandId, IEnumerable<int> metadataIds, string stringVal)
		{
			if (Controls.Count == 0)
				throw new SystemException("No controls");

			if (Controls[0] is MetadataTreeView)
			{
				var treeview = (MetadataTreeView) Controls[0];
				treeview.RefreshFromBrandAndSelect(brandId, metadataIds);
			}

            if (Controls[0] is MetadataListBox)
            {
                var listbox = (MetadataListBox)Controls[0];
                listbox.RefreshFromBrandAndSelect(brandId, metadataIds);
                return;
            }

            if (Controls[0] is MetadataDropDownList && Controls.Count == 1)
            {
                var dd = (MetadataDropDownList)Controls[0];
                dd.RefreshFromBrandAndSelect(brandId, metadataIds.FirstOrDefault());
                return;
            }

            if (Controls[0] is MetadataRadioButtonList)
            {
                var dd = (MetadataRadioButtonList)Controls[0];
                dd.RefreshFromBrandAndSelect(brandId, metadataIds.FirstOrDefault());
                return;
            }

            if (Controls[0] is MetadataCheckBoxList)
            {
                var listbox = (MetadataCheckBoxList)Controls[0];
                listbox.RefreshFromBrandAndSelect(brandId, metadataIds);
                return;
            }

            //===== string inputs
            if (Controls[0] is TextArea)
            {
                var dd = (TextArea)Controls[0];
                dd.Text = stringVal;
                return;
            }

            if (Controls[0] is TextBox)
            {
                var dd = (TextBox)Controls[0];
                dd.Text = stringVal;
                return;
            }

            if (Controls.Count > 2 && Controls[2] is TextArea)
            {
                var dd = (TextArea)Controls[2];
                dd.Text = stringVal;
                return;
            }
		}

		public void GetSelection(out IEnumerable<int> ids)
		{
		    var ttt = string.Empty;

		    GetSelection(out ids, out ttt);
		}

		public void GetSelection(out IEnumerable<int> ids, out string stringVal)
		{
		    stringVal = null;
		    ids = null;

			if (Controls[0] is MetadataTreeView)
			{
				var treeview = (MetadataTreeView)Controls[0];

                ids = (from node in treeview.GetSelectedNodes()
				        let val = NumericUtils.ParseInt32(node.Value, 0)
				        where val > 0
				        select val);
                return;
			}

            if (Controls[0] is MetadataListBox)
            {
                var listbox = (MetadataListBox)Controls[0];

                ids = listbox.SelectedIdsWithBlank;
                return;
            }

			if (Controls[0] is MetadataDropDownList && Controls.Count == 1)
			{
				var dd = (MetadataDropDownList)Controls[0];
				ids = new [] {dd.SelectedId};
                return;
			}

            if (Controls[0] is MetadataRadioButtonList)
            {
                var dd = (MetadataRadioButtonList)Controls[0];
                ids = new[] { dd.SelectedId };
                return;
            }

            if (Controls[0] is MetadataCheckBoxList)
            {
                var dd = (MetadataCheckBoxList)Controls[0];
                ids = (from val in dd.SelectedIds
                       where val > 0
                       select val);
                return;
            }

            //===== string inputs
            if (Controls[0] is TextArea)
            {
                var dd = (TextArea)Controls[0];
                stringVal = dd.Text;
                return;
            }

            if (Controls[0] is TextBox)
            {
                var dd = (TextBox)Controls[0];
                stringVal = dd.Text;
                return;
            }

            if (Controls.Count > 1 && Controls[2] is TextArea)
            {
                var dd = (TextArea)Controls[2];
                stringVal = dd.Text;
                return;
            }

			throw new SystemException("Unable to get selected ID's.  Unknown control");
		}

		protected override void OnLoad(EventArgs e)
		{
		    base.OnLoad(e);

            InitInput(true);
		}

        public void ResetState()
        {
            initCompleted = false;
            Controls.Clear();
            ClearChildState();
            Controls.Clear();
        }

	    public void InitInput(bool resetControls)
	    {
            //ensure all is set or that we haven't been here already
            if (initCompleted || Controls.Count > 0) return;

	        initCompleted = true;

            var setting = BrandMetaSetting;

            if (setting.UiControlType == (int)BrandMetadataUiControlType.Select)
            {
                var selSetting = setting.SelectableSetting;

                try
                {
                    switch ((SelectableMetadataType)selSetting.SelectableType)
                    {
                        case SelectableMetadataType.DropDown: InitDropDown(selSetting); break;
                        case SelectableMetadataType.ComboBox: InitComboBox(selSetting); break;
                        case SelectableMetadataType.RadioButtons: InitRadioButtons(selSetting); break;
                        case SelectableMetadataType.Checkboxes: InitCheckboxes(selSetting); break;
                        case SelectableMetadataType.PresetTextArea: InitPresetTextArea(selSetting); break;
                        default:
                            //if selectable hasn't been initialised yet go with the default setting i.e. a nested dropdown
                            InitDropDown(BrandMetadataSelectableSetting.GetDefault());
                            break;
                    }
                }
                catch (HttpException ex)
                {
                    //the type of underlying control that is being loaded was changed on the fly
                    //in a request before this one. now we are trying to set a control based on the
                    //database held settings and this results in a view state exception - probably
                    //the logic flow will deal with loading the right control after the Init event handler
                    //therefore don't do anything now and only return the control back without doing anything
                    if(ex.Message.ToLower().Contains("failed to load viewstate")) return;

                    throw new Exception("An error occurred while loading the metadata input wrapper inner control(s) - check whether the right control is being loaded in the Init event handler.", ex);
                }
            }
            else if (setting.UiControlType == (int)BrandMetadataUiControlType.TextField)
                InitTextField();
            else if (setting.UiControlType == (int)BrandMetadataUiControlType.TextArea)
                InitTextArea();
            else
                //if for some reason the BrandMetaSetting's UiControlType was not initialised go with the 
                //default i.e. a nested dropdown
                InitDropDown(BrandMetadataSelectableSetting.GetDefault());
	    }

        public  void SetTempBrandMetaSetting(BrandMetadataSetting setting)
        {
            brandMetaSetting = setting;
        }

        /// <summary>
        /// overrides the default styling and default values of controls - currently only the dropd down and listbox 
        /// are implemented
        /// </summary>
        public void ApplyCustomControlAttributes(string inputStyle, string inputTxt, string inputVal)
        {
            if (Controls[0] is MetadataListBox)
            {
                var dd = (MetadataListBox)Controls[0];

                if (!string.IsNullOrEmpty(inputTxt))
                {
                    dd.BlankItemText = inputTxt;
                    dd.BlankItemValue = 0;
                    dd.ShowBlankItem = AbstractListBox.BlankItemLocation.Top;
                }

//                if (!string.IsNullOrEmpty(inputVal) && NumericUtils.IsInt32(inputVal))
//                {
//                    dd.BlankItemValue = NumericUtils.ParseInt32(inputVal);
//                    dd.ShowBlankItem = AbstractListBox.BlankItemLocation.Top;
//                }

                if (!string.IsNullOrEmpty(inputStyle))
                    dd.CssClass = inputStyle;
                
                return;
            }

            if (Controls[0] is MetadataDropDownList && Controls.Count == 1)
            {
                var dd = (MetadataDropDownList)Controls[0];

                if (!string.IsNullOrEmpty(inputTxt))
                {
                    dd.BlankText  = inputTxt;
                    dd.BlankValue = "0";
                }
                
//                if (!string.IsNullOrEmpty(inputVal))
//                    dd.BlankValue = inputVal;

                if (!string.IsNullOrEmpty(inputStyle))
                    dd.CssClass = inputStyle;
                
                return;
            }
        }
	    /// <summary>
	    /// sets the selection to the already loaded underlying controls 
        /// without querying the database as in RefreshFromBrandAndSelet.
        /// currently implemented for dropdowns and comboes only
	    /// </summary>
	    /// <param name="metadataIds"></param>
        public void SetSelection(IEnumerable<int> metadataIds)
	    {
            if(metadataIds == null || metadataIds.Count() == 0) return;

            if (Controls[0] is MetadataListBox)
            {
                var dd = (MetadataListBox)Controls[0];

                foreach (var mId in metadataIds)
                    dd.SafeSelectValue(mId);

                return;
            }

            if (Controls[0] is MetadataDropDownList && Controls.Count == 1)
            {
                var dd = (MetadataDropDownList)Controls[0];

                if(dd.Items.Count == 0) return;

                foreach (ListItem item in dd.Items)
                {
                    var id = int.Parse(item.Value);
                    if (metadataIds.Contains(id))
                        dd.SelectedValue = id.ToString();
                }

                return;
            }
	    }
        /// <summary>
        /// either returns the group number if one is set or a random string if it isn't
        /// </summary>
        /// <returns></returns>
        private string PrepareID()
        {
            return ID + "_" + (GroupNumber > 0 ? GroupNumber.ToString() : Guid.NewGuid().ToString().Substring(0, 6));
        }
        private void InitTextArea()
	    {
	        var ctrl = new TextArea() {CssClass = "PanelTxt"};

	        Controls.Add(ctrl);
	    }
	    private void InitTextField()
	    {
            var ctrl = new TextBox() { CssClass = "PanelTxt" };

            Controls.Add(ctrl);
	    }
	    private void InitPresetTextArea(BrandMetadataSelectableSetting selSetting)
	    {
            //=== add dropdown
            var dd = new MetadataDropDownList
            {
                BrandId = BrandId,
                GroupNumber = GroupNumber,
                OmitBlankItem = false,
                BlankText = "Select to populate",
                BlankValue = "0",
                CssClass = "presetDropDown formInput W225",
                LinearOutlay = selSetting.IsLinear,
                //adding the ID of the control prevents problems with retrieving the posted back
                //data not being persisted to the meta input underlying controls
                //in a scenario when they are dynamically added to another control
                ID = "MetadataDropDownList" + PrepareID()
                
            };
	        dd.RefreshFromDataSource();
            Controls.Add(dd);
            //=== add the line break and then the text area
            Controls.Add(new Literal(){Text = "<br />"});

	        var ta = new TextArea()
	                     {
                             CssClass = "PanelTxt W225",
                             Rows = 6,
                             Columns = 30,
                             //Width = "",
                             //adding the ID of the control prevents problems with retrieving the posted back
                             //data not being persisted to the meta input underlying controls
                             //in a scenario when they are dynamically added to another control
                             ID = "MetadataTextArea" + PrepareID()
	                     };
	        Controls.Add(ta);

            //=== add the javascript/jquery logic for the presets selection
            //this is the url the ajax function will be calling to fetch back the respective metadata upon preset change
            var url = SiteUtils.GetWebsiteUrl("~/MetadataGetTextPreset.ashx?metadataId=");
            //ensure there is jquery included on the page
            if (IncludeJqueryReference)
                Page.ClientScript.RegisterClientScriptInclude("jqueryinclude", "/Includes/Javascript/jQuery/jquery-1.4.1.min.js");
            //function gets metadata val upon dropdown change and inserts it into the text area
            var script = String.Format(@"
                $(document).ready(function(){{
                        
                        var url = '{0}';
                        $('.presetDropDown').change(function(){{
                            
                            var metaId = $(this).val();
                            var tarea = $(this).parent().find('textarea');//nextAll('textarea');   
                            $.get(url + metaId, function(data){{
                                
                                $(tarea).val(data);
                            }});
                            
                            return false;
                        }});
                    }}
                 );", url);

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "dropdownPresets", script, true);
	    }
	    private void InitCheckboxes(BrandMetadataSelectableSetting selSetting)
	    {
            if(selSetting.IsLinear)
            {//checkbox list required
                var chklist = new MetadataCheckBoxList()
                                  {
                                      BrandId = BrandId,
                                      GroupNumber = GroupNumber,
                                      CssClass = "PanelTxt",
                                      LinearOutlay = selSetting.IsLinear,
                                      SortType = (SelectableMetadataSortType)selSetting.SortType,
                                      OrderType = (SelectableMetadataOrderType)selSetting.OrderType,
                                      RepeatColumns = selSetting.ColumnCount,
                                      //adding the ID of the control prevents problems with retrieving the posted back
                                      //data not being persisted to the meta input underlying controls
                                      //in a scenario when they are dynamically added to another control
                                      ID = "MetadataCheckBoxList" + PrepareID(),
                                      AutoPostBack = false,
                                      
                                  };
                chklist.RefreshFromDataSource();
                if (selSetting.OrderType == (int)SelectableMetadataOrderType.ByColumn)
                {//order by column means the number of rows is limited
                    //therefore we need to calculate the number of columns to fit all items
                    //in this number of rows
                    chklist.RepeatColumns = chklist.Items.Count / selSetting.ColumnCount + (chklist.Items.Count % selSetting.ColumnCount > 0 ? 1 : 0);
                    chklist.DataBind();
                }
                Controls.Add(chklist);    
            }
            else
            {//hierarchical checkbox treeview required
                var treeview = new MetadataTreeView
                {
                    BrandId = BrandId,
                    GroupNumber = GroupNumber,
                    ShowCheckBoxes = TreeNodeTypes.All,
                    SortType = (SelectableMetadataSortType)selSetting.SortType
                    
                };

                treeview.NodeStyle.CssClass = "PanelTxt";
                treeview.RefreshFromBrandAndSelect(BrandId, new[] { 1 });
                Controls.Add(treeview);    
            }
            
	    }
	    private void InitRadioButtons(BrandMetadataSelectableSetting selSetting)
	    {
            var radiolist = new MetadataRadioButtonList()
            {
                BrandId = BrandId,
                GroupNumber = GroupNumber,
                CssClass = "PanelTxt",
                LinearOutlay = selSetting.IsLinear,
                SortType = (SelectableMetadataSortType)selSetting.SortType,
                OrderType = (SelectableMetadataOrderType)selSetting.OrderType,
                RepeatColumns = selSetting.ColumnCount,
                //adding the ID of the control prevents problems with retrieving the posted back
                //data not being persisted to the meta input underlying controls
                //in a scenario when they are dynamically added to another control
                ID = "MetadataRadioButtonList" + PrepareID()
                
            };

            radiolist.RefreshFromDataSource();

            if (selSetting.OrderType == (int)SelectableMetadataOrderType.ByColumn)
            {//order by column means the number of rows is limited
                //therefore we need to calculate the number of columns to fit all items
                //in this number of rows
                radiolist.RepeatColumns = radiolist.Items.Count / selSetting.ColumnCount + (radiolist.Items.Count % selSetting.ColumnCount > 0 ? 1 : 0);
                radiolist.DataBind();
            }

            Controls.Add(radiolist);    
	    }
	    private void InitComboBox(BrandMetadataSelectableSetting selSetting)
	    {
            var dd = new MetadataListBox()
            {
                BrandId = BrandId,
                GroupNumber = GroupNumber,
                CssClass = "formInput W225",
                SelectionMode = ListSelectionMode.Multiple,
                Rows = selSetting.Depth,
                LinearOutlay = selSetting.IsLinear,
                //adding the ID of the control prevents problems with retrieving the posted back
                //data not being persisted to the meta input underlying controls
                //in a scenario when they are dynamically added to another control
                ID = "MetadataListBox" + PrepareID(),
                SortType = (SelectableMetadataSortType)selSetting.SortType
            };

            dd.RefreshFromDataSource();
            Controls.Add(dd);
	    }
	    private void InitDropDown(BrandMetadataSelectableSetting selSetting)
	    {
            var dd = new MetadataDropDownList
                         {
                             BrandId = BrandId,
                             GroupNumber = GroupNumber,
                             OmitBlankItem = false,
                             BlankText = "Not Specified",
                             BlankValue = "0",
                             CssClass = "formInput W225",
                             LinearOutlay = selSetting.IsLinear,
                             //adding the ID of the control prevents problems with retrieving the posted back
                             //data not being persisted to the meta input underlying controls
                             //in a scenario when they are dynamically added to another control
                             ID = "MetaDropDown" + PrepareID(),
                             SortType = (SelectableMetadataSortType)selSetting.SortType
                         };
	        
            dd.RefreshFromDataSource();
            Controls.Add(dd);
	    }



	}
}