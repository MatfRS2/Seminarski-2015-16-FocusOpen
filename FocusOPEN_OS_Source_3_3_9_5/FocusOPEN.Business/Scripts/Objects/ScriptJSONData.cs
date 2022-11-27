using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FocusOPEN.Business.Scripts
{


    [Serializable]
    public class ScriptJSONData : ISerializable
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        public ScriptJSONData()
        {

        }

        public ScriptJSONData(Dictionary<string, object> dict)
        {
            data = new Dictionary<string, object>(dict);
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (string key in data.Keys)
            {
                info.AddValue(key, data[key]);
            }
        }

        public void Add(string key, object value)
        {
            data.Add(key, value);
        }

        public object this[string index]
        {
            set { data[index] = value; }
            get { return data[index]; }
        }

    }

}
