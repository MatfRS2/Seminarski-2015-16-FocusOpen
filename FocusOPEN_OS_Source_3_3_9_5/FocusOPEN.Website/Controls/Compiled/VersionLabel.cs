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
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using FocusOPEN.Shared;
using FocusOPEN.Website.About;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class VersionLabel : Literal
	{
		public enum DisplayModes : uint
		{
			Html = 1,
			Comment = 2
		}

		public DisplayModes DisplayMode { get; set; }

		public VersionLabel()
		{
			DisplayMode = DisplayModes.Comment;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!HttpContext.Current.Request.IsLocal && SessionInfo.Current.User.UserRole != UserRole.SuperAdministrator)
				return;

			Assembly currentAssembly = Assembly.GetExecutingAssembly();
			AssemblyInfo currentAssemblyInfo = new AssemblyInfo(currentAssembly);

			string name = currentAssembly.GetName().Name;
			string lastModified = currentAssemblyInfo.LastModifiedDate.ToString("dd MMMM yyyy HH:mm");
			string version = currentAssemblyInfo.Version;

			TimeSpan ts = DateTime.Now - currentAssemblyInfo.LastModifiedDate;

			if (ts.Days > 0)
				lastModified += " (" + ts.Days + " days ago)";
			else if (ts.Hours > 0)
				lastModified += " (" + ts.Hours + " hours ago)";
			else if (ts.Minutes > 0)
				lastModified += " (" + ts.Minutes + " minutes ago)";
			else if (ts.Seconds > 0)
				lastModified += " (" + ts.Seconds + " seconds ago)";

			string prefix = string.Empty;
			string suffix = string.Empty;

			if (DisplayMode == DisplayModes.Comment)
			{
				prefix = "<!--";
				suffix = "-->";
			}

			Text = string.Concat(prefix, name, ". Version: ", version, ". Last modified: ", lastModified, suffix);
		}
	}
}