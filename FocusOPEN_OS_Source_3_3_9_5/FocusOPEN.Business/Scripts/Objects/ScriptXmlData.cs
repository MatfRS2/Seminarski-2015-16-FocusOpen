using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;

namespace FocusOPEN.Business.Scripts
{
    [XmlRoot("data")]
    public class ScriptXmlData : Dictionary<string, object>, IXmlSerializable
    {
        public ScriptXmlData()
        : base()
       {
       }

       public ScriptXmlData(IDictionary<string, object> dictionary)
           : base(dictionary)
       {
       }

       public ScriptXmlData(IEqualityComparer<string> comparer)
           : base(comparer)
       {
       }

       public ScriptXmlData(int capacity)
           : base(capacity)
       {
       }

       public ScriptXmlData(IDictionary<string, object> dictionary, IEqualityComparer<string> comparer)
           : base(dictionary, comparer)
       {
       }

       public ScriptXmlData(int capacity, IEqualityComparer<string> comparer)
        : base(capacity, comparer)
       {
       }

       protected ScriptXmlData(SerializationInfo info, StreamingContext context)
           : base(info, context)
       {
       }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }


        public void ReadXml(System.Xml.XmlReader reader)
        {
    
            XmlSerializer valueSerializer = new XmlSerializer(typeof(string));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                string key = XmlConvert.DecodeName(reader.Name);
                string value = (string)valueSerializer.Deserialize(reader);
                
                this.Add(key, value);
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }


        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer valueSerializer = new XmlSerializer(typeof(string));

            foreach (string key in this.Keys)
            {
                string xmlKey = XmlConvert.EncodeName(key);
                writer.WriteStartElement(xmlKey);
                if (this[key] != null)
                {
                    writer.WriteValue(this[key]);
                }
                writer.WriteEndElement();
            }
        }


        #endregion

    }


  
}
