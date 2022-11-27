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
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using ICSharpCode.SharpZipLib.Zip;

namespace FocusOPEN.APS.WinService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		#region Accessors

		/// <summary>
		/// Gets the path to the FocusOPEN APS Installer temp folder
		/// </summary>
		private static string TempFolder
		{
			get
			{
				return @"C:\FocusOPENAPSInstallerTemp";
			}
		}

		#endregion

		#region Constructor

		public ProjectInstaller()
		{
			InitializeComponent();
		}

		#endregion

		#region Overrides

		protected override void OnBeforeInstall(IDictionary savedState)
		{
			base.OnBeforeInstall(savedState);
			Debug.WriteLine("OnBeforeInstall");
			
			StopService();
			
			UnzipSQLite();
			UnzipFFmpeg();
			UpdateConfig();
			DeleteTempFolder();
		}

		public override void Install(IDictionary savedState)
		{
			base.Install(savedState);
			Debug.WriteLine("Install");
		}

		protected override void OnAfterInstall(IDictionary savedState)
		{
			base.OnAfterInstall(savedState);
			Debug.WriteLine("OnAfterInstall");

			StartService();
		}

		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
			StopService();
		}

		protected override void OnAfterUninstall(IDictionary savedState)
		{
			base.OnAfterUninstall(savedState);
			CleanFolder();
		}

		#endregion

		#region Private Methods

		private void UnzipSQLite()
		{
			try
			{
				// Get the installation directory
				string targetDir = GetTargetDir();

				// Get the location of the SQLite zip which contains both x86 and x64 versions
				string zipFile = Path.Combine(TempFolder, "SQLite.zip");

				// Ensure the zip file exists
				if (!File.Exists(zipFile))
				{
					Debug.WriteLine("ERROR: SQLite zip file not found");
					return;
				}

				// Construct path to temp dir to which SQLite.zip will be extracted
				string tempDir = Path.Combine(TempFolder, "FocusOPEN_APS_SQLite");

				// Create the temp dir if it doesn't exist
				if (!Directory.Exists(tempDir))
				{
					Directory.CreateDirectory(tempDir);
					Debug.WriteLine("Created directory: " + tempDir);
				}

				// Unzip the SQLite zip file
				FastZip fz = new FastZip();
				fz.ExtractZip(zipFile, tempDir, FastZip.Overwrite.Always, null, string.Empty, string.Empty, true);

				// Decide which file is needed based on OS type
				string osType = (OSChecker.Is64BitOperatingSystem) ? "x64" : "x86";
				Debug.WriteLine("Detected operating system: " + osType);

				// Construct path to SQLite.DLL based on subdirectory
				string sqliteDll = Path.Combine(tempDir, osType + "\\System.Data.SQLite.DLL");

				// Ensure the file exists
				if (!File.Exists(sqliteDll))
				{
					Debug.WriteLine("ERROR: System.Data.SQLite.DLL not found: " + sqliteDll);
					return;
				}

				try
				{
					string destinationSqliteDll = Path.Combine(targetDir, "System.Data.SQLite.DLL");

					if (File.Exists(destinationSqliteDll))
					{
						File.Delete(destinationSqliteDll);
						Debug.WriteLine("Deleted SQLite DLL: " + destinationSqliteDll);
					}

					File.Move(sqliteDll, destinationSqliteDll);
					Debug.WriteLine("Moved SQLite DLL to: " + destinationSqliteDll);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("ERROR: Moving System.Data.SQLite to installation folder: " + ex.Message);
				}

				try
				{
					File.Delete(zipFile);
					Debug.WriteLine("Deleted file: " + zipFile);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("ERROR: deleting zip file: " + ex.Message);
				}

				try
				{
					Directory.Delete(tempDir, true);
					Debug.WriteLine("Deleted temp directory: " + tempDir);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("ERROR: Deleting temp directory: " + ex.Message);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR: Unzipping SQLite: " + ex.Message);
			}
		}

		private void UnzipFFmpeg()
		{
			try
			{
				// Get path to the FFmpeg zip file
				string zipPath = Path.Combine(TempFolder, "FFmpeg.zip");

				// Ensure it was extracted correctly to temp folder
				if (!File.Exists(zipPath))
				{
					Debug.WriteLine("ERROR: FFMPEG Zip does not exist");
					return;
				}

				// Get the installation directory
				string targetDir = GetTargetDir();

				// Construct the path where FFmpeg should be unzipped to
				string ffmpegDir = Path.Combine(targetDir, @"Apps\FFmpeg");

				// No point unzipping if the directory exists already
				// This might be an upgrade, so don't unzip again.
				if (Directory.Exists(ffmpegDir))
				{
					Debug.WriteLine("FFmpeg directory already exists: " + ffmpegDir);
					return;
				}

				// Otherwise, proceeed. Create the directory.
				Directory.CreateDirectory(ffmpegDir);
				Debug.WriteLine("Created FFmpeg directory: " + ffmpegDir);

				try
				{
					// Unzip the FFMpeg application to the ffmpeg directory
					FastZip fz = new FastZip();
					fz.ExtractZip(zipPath, ffmpegDir, FastZip.Overwrite.Always, null, string.Empty, string.Empty, true);

					// Delete the zip file, we're done with it
					File.Delete(zipPath);
				}
				catch (Exception e)
				{
					Debug.WriteLine("ERROR: Unzipping FFmpeg Zip: " + e.Message);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("ERROR: Unzipping FFmpeg: " + ex.Message);
			}
		}

		/// <summary>
		/// Deletes the temp folder, if it exists.
		/// </summary>
		private static void DeleteTempFolder()
		{
			if (Directory.Exists(TempFolder))
			{
				try
				{
					Directory.Delete(TempFolder, true);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Error deleting temp folder: " + TempFolder + ". Error: " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Updates the config files after installation
		/// </summary>
		private void UpdateConfig()
		{
			string targetDir = GetTargetDir();
			string imageMagickPath = Context.Parameters["ImageMagickPath"];

			ConfigUpdater cu = new ConfigUpdater(targetDir, imageMagickPath);			
			cu.Update();
		}

		/// <summary>
		/// Cleans the folder when uninstalling, removing all files and folders created by the application.
		/// </summary>
		private void CleanFolder()
		{
			// Get the installation path
			string targetDir = GetTargetDir();

			// Ensure installation dir exists, to be safe
			if (!Directory.Exists(targetDir))
			{
				Debug.WriteLine("Directory does not exist: " + targetDir);
				return;
			}

			// Delete the SQLite database
			string db = Path.Combine(targetDir, "APS.s3db");
			if (File.Exists(db))
				File.Delete(db);

			// Delete the SQLite assembly
			string sqlite = Path.Combine(targetDir, "System.Data.SQLite.DLL");
			if (File.Exists(sqlite))
				File.Delete(sqlite);

			// Delete subdirectories created by the application
			foreach (string dir in new[] { "logs", @"apps\FFmpeg"})
			{
				string subDir = Path.Combine(targetDir, dir);

				if (Directory.Exists(subDir))
				{
					Directory.Delete(subDir, true);
					Debug.WriteLine("Deleted directory: " + subDir);
				}
			}
		}

		private void StartService()
		{
			try
			{
				ServiceController serviceController = new ServiceController(APSInstaller.ServiceName);
				serviceController.Start();
				Debug.WriteLine("Service started successfully");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("Service not started: " + ex.Message, ex));
			}
		}

		private void StopService()
		{
			try
			{
				ServiceController serviceController = new ServiceController(APSInstaller.ServiceName);

				if (serviceController.Status != ServiceControllerStatus.Stopped)
				{
					serviceController.Stop();
					Debug.WriteLine("Service stopped successfully");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("Service not stopped: " + ex.Message, ex));
			}
		}

		private string GetTargetDir()
		{
			// Get the installation path
			string targetDir = Context.Parameters["TargetDir"];

			// Clean the installation path
			if (targetDir.EndsWith("|"))
				targetDir = targetDir.Substring(0, targetDir.Length - 1);

			// Ensure ends with slash
			if (!targetDir.EndsWith("\\"))
				targetDir += "\\";

			return targetDir;
		}

		#endregion
	}
}