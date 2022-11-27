using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;


namespace FocusOPEN.Business.Scripts
{

    public enum ResponseFormat
    {
        JSON,
        XML
    }

    [Serializable]
    [DataContract]
    [XmlRoot("response", Namespace = "", IsNullable = false)]
    public class ScriptResponse
    {
        private ResponseFormat format = ResponseFormat.JSON;
        private ScriptError errorObject = new ScriptError();
        private DateTime responseDateTime = DateTime.Now;
        private Guid requestIdentifier;
        private Guid responseIdentifier;
        private string call = "Unspecified";
        private Dictionary<string, object> data = new Dictionary<string,object>();
        private int? userId;


        [XmlIgnore]
        public ResponseFormat Format 
        {
            get
            {
                return format;
            }
            set{
                format = value;
            }
        }

        [XmlIgnore]
        public int? UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }

        [XmlElement("call")]
        [DataMember(Name="call")]
        public string CallName
        {
            get
            {
                return call;
            }
            set
            {
                if (!String.IsNullOrEmpty(call))
                {
                    call = value;
                }            
            }
        }


        [XmlElement("datime")]
        [DataMember(Name = "datime")]
        public DateTime TimeStamp
        {
            get
            {
                return responseDateTime;
            }
            set
            {
                responseDateTime = value;
            }
        }

        [XmlElement("responseid")]
        [DataMember(Name = "responseid")]
        public Guid ResponseId
        {
            get
            {
                return responseIdentifier;
            }
            set
            {
                responseIdentifier = value;
            }
        }

        [XmlElement("requestid")]
        [DataMember(Name = "requestid")]
        public Guid RequestId
        {
            get
            {
                return requestIdentifier;
            }
            set
            {
                requestIdentifier = value;
            }
        }

        [XmlElement("errors")]
        [DataMember(Name = "errors")]
        public ScriptError Error
        {
            get
            {
                return errorObject;
            }
            set
            {
                errorObject = value;
            }
        }

        [XmlIgnore]
        public Dictionary<string, object> Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }


        [XmlElement("data")]
        public ScriptXmlData XmlData
        {
            get
            {
                return new ScriptXmlData(data);
            }
            set
            {
               //set only required for serialization
                throw new NotImplementedException("XmlData is read only");
            }
        }

        [XmlIgnore]
        [DataMember(Name = "data")]
        public ScriptJSONData JSONData
        {
            get
            {
                return new ScriptJSONData(data);
            }
            set
            {
                //set only required for serialization
                throw new NotImplementedException("JSONData is read only");  
            }

        }


    }
}
