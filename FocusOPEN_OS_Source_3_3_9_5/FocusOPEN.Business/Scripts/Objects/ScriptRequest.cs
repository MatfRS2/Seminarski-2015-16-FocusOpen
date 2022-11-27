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
    [Serializable]
    [DataContract]
    [XmlRoot("request", Namespace = "", IsNullable = false)]
    public class ScriptRequest
    {
        private DateTime requestDateTime = DateTime.Now;
        private Guid requestIdentifier;
        private Guid responseIdentifier;
        private string call = "Unspecified";
        private Dictionary<string, object> data = new Dictionary<string, object>();
        private string httpMethod = String.Empty;
        private string ipAddress = String.Empty;
        private string sessionToken = String.Empty;

        [XmlElement("call")]
        [DataMember(Name = "call")]
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
                return requestDateTime;
            }
            set
            {
                requestDateTime = value;
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

        [XmlElement("httpmethod")]
        [DataMember(Name = "httpmethod")]
        public string HttpMethod
        {
            get
            {
                return httpMethod;
            }
            set
            {
                httpMethod = value;
            }
        }


        [XmlElement("ipaddress")]
        [DataMember(Name = "ipaddress")]
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }

        [XmlElement("sessiontoken")]
        [DataMember(Name = "sessiontoken")]
        public string SessionToken
        {
            get
            {
                return sessionToken;
            }
            set
            {
                sessionToken = value;
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
