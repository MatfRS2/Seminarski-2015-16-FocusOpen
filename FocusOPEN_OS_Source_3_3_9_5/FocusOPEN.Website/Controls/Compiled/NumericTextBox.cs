using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace FocusOPEN.Website.Controls
{
    public class NumericTextBox : TextBox
    {
        protected override void OnPreRender(EventArgs e)
		{
            string script = "function NumericOnly(e){var unicode = e.charCode?e.charCode:e.keyCode;if(unicode == 8 || unicode == 9 || (unicode >= 48 && unicode <= 57)){return true;}else{return false;}}";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NumericTextScript", script, true);           
			Attributes["onkeypress"] = "return NumericOnly(event)";

            base.OnPreRender(e);
         }

    }
}