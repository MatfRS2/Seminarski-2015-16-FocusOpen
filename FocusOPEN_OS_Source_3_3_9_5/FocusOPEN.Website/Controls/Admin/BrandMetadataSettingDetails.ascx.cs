using System;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls.Admin
{
    public partial class BrandMetadataSettingDetails : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {}

        public void LoadData(BrandMetadataSetting setting)
        {
            FieldNameTextBox.Text = setting.FieldName;
            AdditionalCopyTextBox.Text = setting.AdditionalCopy;
            ToolTipTextBox.Text = setting.ToolTip;
        }

        public void RetrieveData(ref BrandMetadataSetting setting)
        {
            setting.FieldName = FieldNameTextBox.Text.Trim();
            setting.AdditionalCopy = AdditionalCopyTextBox.Text.Trim();
            setting.ToolTip = ToolTipTextBox.Text.Trim();
        }

        public void ResetUI(string metaName)
        {
            FieldNameTextBox.Text = metaName;
            AdditionalCopyTextBox.Text = string.Empty;
            ToolTipTextBox.Text = string.Empty;
        }
    }
}