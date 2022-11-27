/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.Xml.Linq;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using InfoSoftGlobal;

namespace FocusOPEN.Website.Controls
{
	public partial class LineChart : BaseUserControl
	{
		#region Private Variables

		private readonly IList<KeyValuePair<string, int>> m_DataPoints = new List<KeyValuePair<string, int>>();

		#endregion

		#region Accessors

		public IList<KeyValuePair<string, int>> DataPoints
		{
			get
			{
				return m_DataPoints;
			}
		}

		#endregion

		public void Initialise()
		{
			// Register the FusionCharts javascript
			if (!Page.ClientScript.IsClientScriptIncludeRegistered("FusionCharts"))
				Page.ClientScript.RegisterClientScriptInclude("FusionCharts", ResolveUrl("~/FusionCharts/FusionCharts.js"));

			// Setup the XML for the chart
			XElement xml = new XElement("graph",
				new XAttribute("showNames", 1),
				new XAttribute("showValues", 0),
				new XAttribute("showPercentageValues", 0),
				new XAttribute("showPercentageInLabel", 0),
				new XAttribute("decimalPrecision", 1),
				new XAttribute("animation", 0),
				new XAttribute("rotateNames", (DataPoints.Count >= 31) ? 1 : 0),
				new XAttribute("showAlternateHGridColor", 1),
				new XAttribute("AlternateHGridColor", "ff5904"),
				new XAttribute("divLineColor", "999999"),
				new XAttribute("divLineAlpha", "20"),
				new XAttribute("alternateHGridAlpha", "5"),
				new XAttribute("canvasBorderColor", "666666"),
				new XAttribute("baseFontColor", "666666"),
				new XAttribute("LineColor", "999999"),
				new XAttribute("lineThickness", "2"),
				new XAttribute("lineAlpha", "50"),
				new XAttribute("limitsDecimalPrecision", "0"),
				new XAttribute("divLineDecimalPrecision", "0"),
				new XAttribute("bgColor", "f1f1f1"),
				new XAttribute("xAxisName", string.Empty),
				new XAttribute("yAxisName", "Count"),
				new XAttribute("showhovercap", 1));

			foreach (KeyValuePair<string, int> datapoint in DataPoints)
			{
				// Get the month and the count
				string label = datapoint.Key;
				int count = datapoint.Value;

				// Add the XML node
				xml.AddFirst(new XElement("set",
					new XAttribute("name", label),
					new XAttribute("value", count),
					new XAttribute("hoverText", count)
				));
			}

			// Get the chart literal and setup the chart
			ChartLiteral.Text = FusionCharts.RenderChart(ResolveUrl("~/FusionCharts/FCF_Line.swf"), string.Empty, xml.ToUnformattedXmlForFusionCharts(), "DriveChart_" + ChartLiteral.ClientID, "500", "300", false, false);
		}
	}
}