using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FocusOPEN.Website.Controls
{
    public class PluginStatusLabel : FeedbackLabel
    {

        public PluginStatusLabel()
        {
            Pinned = true;
        }

        public string TextClass { get; set; }

        protected override string GetTextClass()
        {
            return TextClass;
        }

    }
}