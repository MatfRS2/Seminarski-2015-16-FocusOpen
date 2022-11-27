using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FocusOPEN.Business.Scripts
{
    [Serializable]
    [XmlType("errors")]
    public class ScriptError
    {
        int code = 0;
        string description = "Successful";

        [XmlElement("errormessage")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        [XmlElement("errorcode")]
        public int Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

    }
}
