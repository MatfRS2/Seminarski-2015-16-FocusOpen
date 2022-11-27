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
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace FocusOPEN.Website.Controls
{
	public sealed class TextArea : TextBox
	{
		#region Constructor

		public TextArea()
		{
			TextMode = TextBoxMode.MultiLine;
		}

		#endregion

        #region Properties

        /// <summary>
        /// Removes angled brackets to allow control's content to pass RequestValidation in a page.
        /// </summary>
        public bool EmailsOnly {

            get
            {
                object objVal = ViewState["EmailsOnly"];
                return ((objVal == null)?false:(bool)objVal);
            }
            set
            {
                ViewState["EmailsOnly"] = value;
            }
        }

        #endregion



        #region Overrides

        protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (!CssClass.Contains("sizable"))
				CssClass += " sizable";

			if (MaxLength > 0)
			{
				if (!Page.ClientScript.IsClientScriptBlockRegistered("TextAreaRestrictText"))
				{
					StringBuilder script = new StringBuilder();

					script.AppendLine("function SetCaretToEnd(input) { if (input.createTextRange){ var range = input.createTextRange();range.collapse(false);range.select();}}");
					script.AppendLine("function RestrictText(o, max) { if (o.value.length > max) { alert('You can only enter a maximum of ' + max + ' characters in this textbox.'); o.value = o.value.substring(0, max); SetCaretToEnd(o);}}");

					Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "TextAreaRestrictText", script.ToString(), true);
				}

				Attributes["onkeyup"] = string.Format("RestrictText(this, {0})", MaxLength);
			}

             //remove angular brackets at client-side to prevent RequestValidation failure 
            if (EmailsOnly)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "TextAreaEmailsOnly", @"function ParseEmails(o) {var vals=o.value.match(/\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/gi);o.value='';for(i=0;i<vals.length;i++){o.value += vals[i] + ';';}}", true);
                Attributes["onblur"] = "ParseEmails(this)";       
            }

		}



		#endregion
	}
}