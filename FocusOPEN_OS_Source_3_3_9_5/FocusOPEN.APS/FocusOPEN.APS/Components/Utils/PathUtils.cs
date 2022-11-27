/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.IO;
using System.Reflection;
using log4net;

namespace FocusOPEN.APS
{
	internal class PathUtils : Singleton<PathUtils>
	{
		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public string TempPath { get; set; }

		/// <summary>
		/// Gets the temp path.  This is the temp path configured from the app settings (via the TempPath key).
		/// If no temp path has been set, the system temp path will be returned appended with "FocusOPEN-APS".
		/// This folder will be created if it does not exist.
		/// </summary>
		public string GetTempPath()
		{
			string tempPath = TempPath;

			if (string.IsNullOrEmpty(TempPath))
			{
				tempPath = Path.GetTempPath();
				tempPath = Path.Combine(tempPath, "FocusOPEN-APS");
			}

			if (!tempPath.EndsWith("\\"))
				tempPath += "\\";

			if (!Directory.Exists(tempPath))
			{
				Directory.CreateDirectory(tempPath);
				m_Logger.DebugFormat("Created temp folder: {0}", tempPath);
			}

			return tempPath;
		}

		/// <summary>
		/// Gets the extension, lowercased and without preceding period
		/// </summary>
		public static string GetExtension(string path)
		{
			string extension = Path.GetExtension(path) ?? string.Empty;

			if (extension.StartsWith("."))
				extension = extension.Substring(1);

			return extension.ToLower();
		}
	}
}