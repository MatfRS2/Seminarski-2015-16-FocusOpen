using System.Collections.Generic;
using System.Xml;

namespace FocusOPEN.Shared
{
    /// <summary>
    /// A tree-like data structure that is used to represent an xml document
    /// </summary>
    public struct XmlTree
    {
        public XmlTree(string nodeName) : this()
        {
            Attributes = new Dictionary<string, string>();
            NodeName = nodeName;
            NodeValue = string.Empty;
            Children = new List<XmlTree>();
        }
        public XmlTree(string nodeName, string nodeValue) : this()
        {
            Attributes = new Dictionary<string, string>();
            NodeName = nodeName;
            NodeValue = nodeValue;
            Children = new List<XmlTree>();
        }
        public XmlTree(string nodeName, List<XmlTree> childr, Dictionary<string, string> attr) : this()
        {
            Attributes = attr;
            NodeName = nodeName;
            Children = childr;
        }

        /// <summary>
        /// Attributes of the current node
        /// </summary>
        public Dictionary<string,string> Attributes { get; set; }
        /// <summary>
        /// The name of the node that the instance represents
        /// </summary>
        public string NodeName { get; set; }
        /// <summary>
        /// The value of the current node
        /// </summary>
        public string NodeValue { get; set; }
        /// <summary>
        /// Children of the current node
        /// </summary>
        public List<XmlTree> Children { get; set; }

        /// <summary>
        /// converts the structure by setting the current node as the root element
        /// </summary>
        /// <returns></returns>
        public XmlDocument ConvertToXmlDocument()
        {
            var root = XmlUtils.GetBlankXmlDoc(NodeName);
            if (root.DocumentElement == null) return root;

            var rootElem = root.DocumentElement;

            if (!string.IsNullOrEmpty(NodeValue))
                rootElem.InnerText = NodeValue;
            else
            {
                var children = GetChildren(root);
                foreach (var child in children)
                {
                    rootElem.AppendChild(child);
                }
            }

            return root;
        }
        
        public List<XmlElement> GetChildren(XmlDocument doc)
        {
            var list = new List<XmlElement>();

            if (Children.Count == 0) return list;

            foreach (var child in Children)
            {
                var elem = doc.CreateElement(child.NodeName);
                //firstly append all attributes of the current item
                foreach (var attribute in child.Attributes)
                {
                    elem.SetAttribute(attribute.Key, attribute.Value);
                }

                //then take care of its value if it has one or
                //of its children of it hasn't
                if(!string.IsNullOrEmpty(child.NodeValue))
                {
                    elem.InnerText = child.NodeValue;
                }
                else
                {
                    var children = child.GetChildren(doc);
                    foreach (var childsChild in children)
                    {
                        elem.AppendChild(childsChild);
                    }
                }

                list.Add(elem);
            }

            return list;
        }
    }
}
