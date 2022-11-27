using System;
using System.Linq;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls.Admin
{
    public partial class SelectableMetadataSettingDetails : BaseUserControl
    {
        public int BrandMetadataSelectableSettingId
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "BrandMetadataSelectableSettingId", 0);
            }
            set
            {
                ViewState["BrandMetadataSelectableSettingId"] = value;
            }
        }
        public int SelectableType
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "SelectableType", (int)SelectableMetadataType.DropDown);
            }
            set
            {
                ViewState["SelectableType"] = value;
            }
        }

        private BrandMetadataSelectableSetting setting;
        public BrandMetadataSelectableSetting SelectableSetting
        {
            set
            {
                setting = value;
                BrandMetadataSelectableSettingId = setting.BrandMetadataSelectableSettingId.GetValueOrDefault();
                
                //if none yet then set dropdown as a the default 
                SelectableType = setting.IsNull ? (int)SelectableMetadataType.DropDown : setting.SelectableType;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void TypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        { 
            SelectableType = int.Parse(TypeDropDownList.SelectedValue);

            OpenDetails();

            ApplySelectableTypeLogicToControls();
        }
        protected void LinearCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            if (int.Parse(TypeDropDownList.SelectedValue) == (int)SelectableMetadataType.Checkboxes)
                LayoutOptionsPanel.Visible = LinearCheckBox.Checked;

            OpenDetails();
        }

        /// <summary>
        /// Enabled/Disable controls
        /// depending on the currently selected selectable type of the metadata. It also inits all 
        /// controls to their default (i.e. no selection made yet) states
        /// </summary>
        private void ApplySelectableTypeLogicToControls()
        {
            LinearCheckBox.Checked = 
            DepthPanel.Visible = 
            AllowMultipleCheckbox.Enabled = 
            AllowMultipleCheckbox.Checked = 
            LayoutOptionsPanel.Visible = false;

            LinearCheckBox.Enabled = true;

            OrderTypeRadioButtonList.SelectedValue = ((int) SelectableMetadataOrderType.ByRow).ToString();
            ColumnsDropDownList.SelectedValue = "1";
            DepthDropDownList.SelectedValue = "3";

            if(SelectableType == (int)SelectableMetadataType.DropDown)
            {//the default type upon saving a new selectable

                LinearCheckBox.Enabled = true;
                //treeview mode on or off
                AllowMultipleCheckbox.Enabled = true;

            }if(SelectableType == (int)SelectableMetadataType.ComboBox)
            {
                DepthPanel.Visible = true;
                
                //it's multiple allowed by default so check it
                AllowMultipleCheckbox.Checked = true;

            }if(SelectableType == (int)SelectableMetadataType.Checkboxes)
            {
                LayoutOptionsPanel.Visible = false;
                LinearCheckBox.Enabled = true;
                AllowMultipleCheckbox.Checked = true;

            }if(SelectableType == (int)SelectableMetadataType.RadioButtons)
            {
                LayoutOptionsPanel.Visible = true;
                LinearCheckBox.Checked = true;
                LinearCheckBox.Enabled = false;
            }if (SelectableType == (int)SelectableMetadataType.PresetTextArea)
            {
                

            }
        }
        public override void DataBind()
        {
            base.DataBind();

            //load data & default selections
            TypeDropDownList.DataSource = EnumUtils.GetList<SelectableMetadataType>();
            TypeDropDownList.DataBind();

            DepthDropDownList.DataSource = Enumerable.Range(2, Settings.MaxSelectableMetadataDepth);
            DepthDropDownList.DataBind();

            SortTypeRadioButtonList.DataSource = EnumUtils.GetList<SelectableMetadataSortType>();
            SortTypeRadioButtonList.DataBind();
            SortTypeRadioButtonList.SelectedValue = ((int)SelectableMetadataSortType.AlphaNumeric).ToString();

            OrderTypeRadioButtonList.DataSource = EnumUtils.GetList<SelectableMetadataOrderType>();
            OrderTypeRadioButtonList.DataBind();

            ColumnsDropDownList.DataSource = Enumerable.Range(1, 10);
            ColumnsDropDownList.DataBind();

            ApplySelectableTypeLogicToControls();

            //init controls depending on supplied selectable metadata setting);
            if (setting != null && BrandMetadataSelectableSettingId > 0)
            {
                TypeDropDownList.SelectedValue          = setting.SelectableType.ToString();
                DepthDropDownList.SelectedValue         = setting.Depth.ToString();
                SortTypeRadioButtonList.SelectedValue   = setting.SortType.ToString();
                OrderTypeRadioButtonList.SelectedValue  = setting.OrderType.ToString();
                ColumnsDropDownList.SelectedValue       = setting.ColumnCount.ToString();

                LinearCheckBox.Checked                  = setting.IsLinear;
                AllowMultipleCheckbox.Checked           = setting.AllowMultiple;

                if (setting.SelectableType == (int)SelectableMetadataType.Checkboxes)
                    LayoutOptionsPanel.Visible = setting.IsLinear;
            }
        }
        public void OpenDetails()
        {
            SelectableDetailsDiv.Attributes["style"] = string.Empty;
            SignLiteral.Text = SignLiteral.Text.Replace("[+]", "[-]");
        }
        public BrandMetadataSelectableSetting RetrieveSetting()
        {
            //var result = BrandMetadataSelectableSetting.New();

            //set the id if we are editing an existing setting as opposed to adding a new one - this will trigger an update at the mapper instance
            //if (BrandMetadataSelectableSettingId > 0) result.BrandMetadataSelectableSettingId = BrandMetadataSelectableSettingId;

            var result = BrandMetadataSelectableSettingId > 0
                             ? BrandMetadataSelectableSetting.Get(BrandMetadataSelectableSettingId)
                             : BrandMetadataSelectableSetting.New();


            result.SelectableType   = GetIntOrDefault(TypeDropDownList.SelectedValue);
            result.Depth            = GetIntOrDefault(DepthDropDownList.SelectedValue);
            result.SortType         = GetIntOrDefault(SortTypeRadioButtonList.SelectedValue);
            result.OrderType        = GetIntOrDefault(OrderTypeRadioButtonList.SelectedValue);
            result.ColumnCount      = GetIntOrDefault(ColumnsDropDownList.SelectedValue);
            
            result.IsLinear = LinearCheckBox.Checked;
            result.AllowMultiple = AllowMultipleCheckbox.Checked;

            //leave as they are if existing and set the default values if not
            result.FilterGroup          = result.IsNew ? 0 : result.FilterGroup;
            result.FilterSelectableType = result.IsNew ? (int)SelectableMetadataType.DropDown : result.FilterSelectableType;
            result.FilterDepth          = result.IsNew ? 3 : result.FilterDepth;

            return result;
        }
        private static int GetIntOrDefault(string selectedValue)
        {
            if (string.IsNullOrEmpty(selectedValue)) return -1;

            return int.Parse(selectedValue);
        }
    }
}