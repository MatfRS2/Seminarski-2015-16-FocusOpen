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
using System.IO;
using System.Web.UI.WebControls;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class LogViewer : BaseAdminPage
	{
		#region Accessors

		private string AbsoluteLogFileFolder
		{
			get
			{
				return Path.Combine(Server.MapPath("~/"), "../Logs/");
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				List<String> fileList = new List<String>();
				fileList.AddRange(Directory.GetFiles(AbsoluteLogFileFolder, "*.log"));

				fileList.Sort();
				fileList.Reverse();

				LogFilesDataList.DataSource = fileList;
				LogFilesDataList.DataBind();
			}
		}

		protected void DownloadLogLinkButton_Command(object sender, CommandEventArgs e)
		{
			string path = Path.Combine(AbsoluteLogFileFolder, e.CommandArgument.ToString());

			string tmp = Path.GetTempFileName();
			File.Copy(path, tmp, true);

			Response.ContentType = "application/octet-stream";
			Response.AppendHeader("Content-Disposition", string.Format("attachment;filename=\"" + Path.GetFileName(path) + "\""));
			Response.AppendHeader("Content-Length", FileUtils.GetFileSize(tmp).ToString());
			Response.TransmitFile(tmp);
			Response.End();
		}

		protected void LogFilesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:

					string path = e.Item.DataItem.ToString();

					string filename = Path.GetFileName(path);

					string display = filename;

					string[] tokens = (Path.GetFileNameWithoutExtension(filename) ?? string.Empty).Split('-');

					if (tokens.Length == 3)
					{
						int year = NumericUtils.ParseInt32(tokens[0], 0);
						int month = NumericUtils.ParseInt32(tokens[1], 0);
						int day = NumericUtils.ParseInt32(tokens[2], 0);

						if (year > 0 && month > 0 && day > 0)
						{
							DateTime dt = new DateTime(year, month, day);
							display = dt.ToString("dd MMMM yyyy") + " (" + filename + ")";
						}
					}

					LinkButton DownloadLogLinkButton = (LinkButton)e.Item.FindControl("DownloadLogLinkButton");
					DownloadLogLinkButton.Text = display;
					DownloadLogLinkButton.CommandArgument = filename;

					break;
			}
		}

		#endregion
	}
}