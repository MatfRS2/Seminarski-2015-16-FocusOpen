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
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using InfoSoftGlobal;

namespace FocusOPEN.Website.Admin
{
	public partial class DriveInfo : BaseAdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// List of drives for which info should be displayed
				List<String> driveList = new List<string> { @"C:\" };

				// The list of drives to list, from the config file
				string[] watchedDriveList = ConfigurationManager.AppSettings.GetValue("WatchedDrives").Split(',');

				foreach (string drive in watchedDriveList)
				{
					if (!StringUtils.IsBlank(drive))
					{
						string a = drive.ToUpper().Trim();

						if (!driveList.Contains(a))
							driveList.Add(a);
					}
				}

				DriveRepeater.DataSource = driveList;
				DriveRepeater.DataBind();
			}
		}

		protected void DriveRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:

					string path = e.Item.DataItem.ToString();

					NetworkDrive networkDrive = null;

					Literal DriveLiteral = (Literal) e.Item.FindControl("DriveLiteral");
					DriveLiteral.Text = e.Item.DataItem.ToString();

					try
					{
						if (path.StartsWith("\\"))
						{
							if (path.EndsWith(@"\"))
								path = path.Substring(0, path.Length - 1);

							networkDrive = new NetworkDrive
							{
								FindNextFreeDrive = true,
								Force = true,
								ShareName = path
							};

							networkDrive.MapDrive();
							path = networkDrive.LocalDrive;
						}

						System.IO.DriveInfo drive = new System.IO.DriveInfo(path);

						string friendlyAvailableFreeSpace = FileUtils.FriendlyFileSize(drive.AvailableFreeSpace);
						string friendlyTotalFreeSpace = FileUtils.FriendlyFileSize(drive.TotalFreeSpace);
						string friendlyTotalSize = FileUtils.FriendlyFileSize(drive.TotalSize);
						long totalUsedSpace = (drive.TotalSize - drive.TotalFreeSpace);
						string friendlyTotalUsedSpace = FileUtils.FriendlyFileSize(totalUsedSpace);

						SetLabel(e.Item, "DriveNameLabel", drive.Name);
						SetLabel(e.Item, "DriveTypeLabel", drive.DriveType);
						SetLabel(e.Item, "DriveVolumeLabel", drive.VolumeLabel);
						SetLabel(e.Item, "DriveFormatLabel", drive.DriveFormat);
						SetLabel(e.Item, "DriveReadyLabel", drive.IsReady);
						SetLabel(e.Item, "DriveUsedSpaceLabel", friendlyTotalUsedSpace);
						SetLabel(e.Item, "DriveAvailableFreeSpaceLabel", friendlyAvailableFreeSpace);
						SetLabel(e.Item, "DriveTotalFreeSpaceLabel", friendlyTotalFreeSpace);
						SetLabel(e.Item, "DriveTotalSpaceLabel", friendlyTotalSize);

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
							new XAttribute("showhovercap", 0),
							new XElement("set",
								new XAttribute("name", "Free Space (" + friendlyTotalFreeSpace + ")"),
								new XAttribute("value", drive.TotalFreeSpace),
								new XAttribute("color", "007fff"),
								new XAttribute("hoverText", "Total Free Space: " + friendlyTotalFreeSpace)),
							new XElement("set",
								new XAttribute("name", "Used Space (" + friendlyTotalUsedSpace + ")"),
								new XAttribute("value", totalUsedSpace),
								new XAttribute("color", "ff0000"),
								new XAttribute("hoverText", "Total Used Space: " + friendlyTotalUsedSpace))
							);

						// Get the chart literal and setup the chart
						Literal ChartLiteral = (Literal)e.Item.FindControl("ChartLiteral");
						ChartLiteral.Text = FusionCharts.RenderChart(ResolveUrl("~/FusionCharts/FCF_Pie3D.swf"), string.Empty, xml.ToUnformattedXmlForFusionCharts(), "DriveChart_" + ChartLiteral.ClientID, "450", "300", false, false);
					}
					catch (Exception ex)
					{
						Label ErrorLabel = (Label) e.Item.FindControl("ErrorLabel");
						ErrorLabel.Visible = true;
						ErrorLabel.Text = "Unable to display drive information: " + ex.Message;

						Panel DriveInfoPanel = (Panel) e.Item.FindControl("DriveInfoPanel");
						DriveInfoPanel.Visible = false;
#if DEBUG
						if (Request.IsLocal)
							throw;
#endif
					}

					if (networkDrive != null)
						networkDrive.UnMapDrive();

					break;
			}
		}

		private static void SetLabel(Control ri, string id, object text)
		{
			Label label = (Label) ri.FindControl(id);
			label.Text = text.ToString();
		}
	}
}