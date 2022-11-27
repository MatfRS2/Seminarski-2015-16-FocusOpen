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
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace FocusOPEN.Shared
{
	public static class XmlUtils
	{
		public static XmlNode AddNode(XmlDocument doc, XmlNode parent, string name, object val)
		{
			XmlNode node = doc.CreateNode(XmlNodeType.Element, name, string.Empty);
			node.InnerText = (val == null) ? string.Empty : val.ToString();

			parent.AppendChild(node);
			
			return node;
		}

		/// <summary>
		/// Adds an attribute to the specified node with the specified name and value
		/// </summary>
		public static void AddAttribute(XmlDocument doc, XmlNode node, string name, object val)
		{
			XmlAttribute attr = doc.CreateAttribute(name);
			attr.Value = (val == null) ? string.Empty : val.ToString();
			
			if (node.Attributes != null)
				node.Attributes.Append(attr);
		}

		/// <summary>
		/// Gets a blank XML document with a root element named 'root'
		/// </summary>
		public static XmlDocument GetBlankXmlDoc()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
			xmlDocument.AppendChild(xmlDocument.CreateElement("root"));
			return xmlDocument;
		}

        /// <summary>
        /// Gets a blank XML document with a root element named rootName
        /// </summary>
        public static XmlDocument GetBlankXmlDoc(string rootName)
        {
            if (string.IsNullOrEmpty(rootName)) return GetBlankXmlDoc();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
            xmlDocument.AppendChild(xmlDocument.CreateElement(rootName));
            return xmlDocument;
        }

		/// <summary>
		/// Transforms the XML using the specified XSL file with the specified parameters
		/// </summary>
		/// <param name="xml">The XML to be transformed</param>
		/// <param name="xslFile">Path to the XSL file to be used for transformation</param>
		/// <param name="xslParams">Parameters to be passed to XSL</param>
		/// <returns>Transformed XML</returns>
		public static string Transform(string xml, string xslFile, Hashtable xslParams)
		{
			XmlReaderSettings settings = new XmlReaderSettings {ProhibitDtd = false};
			using (XmlReader reader = XmlReader.Create(xslFile, settings))
			{
				XslCompiledTransform xslTrans = new XslCompiledTransform();
				xslTrans.Load(reader);

				return Transform(xml, xslTrans, xslParams);
			}
		}

		/// <summary>
		/// Transforms the XML using the specified XSL transformation with the specified parameters
		/// </summary>
		/// <param name="xml">The XML to be transformed</param>
		/// <param name="xslTrans">The XSL transformation to be used for transformation</param>
		/// <param name="xslParams">Parameters to be passed to XSL</param>
		/// <returns>Transformed XML</returns>
		private static string Transform(string xml, XslCompiledTransform xslTrans, Hashtable xslParams)
		{
			if (StringUtils.TrimWhiteSpace(xml).Length == 0)
				return String.Empty;

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);

			XsltArgumentList xslArgs = null;

			if (xslParams != null)
			{
				xslArgs = new XsltArgumentList();

				foreach (string paramName in xslParams.Keys)
				{
					string paramValue = xslParams[paramName].ToString();
					xslArgs.AddParam(paramName, "", paramValue);
				}
			}

			StringWriter sw = new StringWriter();
			xslTrans.Transform(xmlDoc, xslArgs, sw);

			return sw.ToString();
		}

//        public static XmlTree ParseToXmlTree()
//        {
//            
//        }

		public static string SerializeObject<T>(T pObject)
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
				xs.Serialize(xmlTextWriter, pObject);
				memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
				return UTF8ByteArrayToString(memoryStream.ToArray());
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		public static T DeserializeObject<T>(String pXmlizedString)
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
			return (T) xs.Deserialize(memoryStream);
		}

		private static string UTF8ByteArrayToString(byte[] characters)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			return encoding.GetString(characters);
		}

		private static byte[] StringToUTF8ByteArray(string xmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			return encoding.GetBytes(xmlString);
		}
	}
}