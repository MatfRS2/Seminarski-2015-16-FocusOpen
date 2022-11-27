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
using System.Xml;
using System.Xml.Linq;

namespace FocusOPEN.Shared
{
	public static class XmlExtensions
	{
		/// <summary>
		/// Gets the attribute from the XElement.  If it is not found, the default is returned.
		/// </summary>
		public static T GetAttribute<T>(this XElement element, string name, T defaultVal)
		{
			XAttribute attribute = element.Attribute(name);

			if (attribute == null)
				return defaultVal;

			string val = attribute.Value;

			try
			{
				return (T)Convert.ChangeType(val, typeof(T));
			}
			catch (Exception)
			{
				return defaultVal;
			}
		}

		public static string ToUnformattedXmlForFusionCharts(this XElement element)
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(element.ToString());
			return doc.ToUnformattedXmlForFusionCharts();
		}

		public static string ToUnformattedXmlForFusionCharts(this XmlDocument doc)
		{
			string xml = doc.InnerXml;
			xml = xml.Replace("\"", "'");
			return xml;
		}
	}
}