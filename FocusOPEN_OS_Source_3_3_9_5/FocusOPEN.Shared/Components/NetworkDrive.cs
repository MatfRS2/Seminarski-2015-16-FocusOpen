/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

/*=====================================================================================================================
  
  [ NetworkDrive - Map Network Drive API Class ]
  -----------------------------------------------
  Copyright (c)2006-2007 aejw.com
  http://www.aejw.com/
  
 Build:         0028 - March 2007
 Thanks To:     'jsantos98', 'FeLiZk' from CodeProject.com for there comments and help
                'MartinPreis' for reporting two bugs
 EULA:          Creative Commons - Attribution-ShareAlike 2.5
                http://creativecommons.org/licenses/by-sa/2.5/

=====================================================================================================================*/

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FocusOPEN.Shared
{
	/// <summary>
	/// Network Drive Mapping class / wrapper
	/// </summary>
	/// <remarks>Maps, ummaps and general functions for network drives</remarks>
	public class NetworkDrive
	{
		#region Private Variables

		private string m_LocalDrive = null;

		public NetworkDrive()
		{
			ShareName = string.Empty;
			FindNextFreeDrive = false;
			PromptForCredentials = false;
			Force = false;
			Persistent = false;
			SaveCredentials = false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Option to save credentials on reconnection
		/// </summary>
		public bool SaveCredentials { get; set; }

		/// <summary>
		/// Option to reconnect drive after log off / reboot
		/// </summary>
		public bool Persistent { get; set; }

		/// <summary>
		/// Option to force connection if drive is already mapped
		/// or force disconnection if network path is not responding
		/// </summary>
		public bool Force { get; set; }

		/// <summary>
		/// Option to prompt for user credintals when mapping a drive
		/// </summary>
		public bool PromptForCredentials { get; set; }

		/// <summary>
		/// Option to auto select the 'LocalDrive' property to next free driver letter when mapping a network drive
		/// </summary>
		public bool FindNextFreeDrive { get; set; }

		/// <summary>
		/// Share address to map drive to. (eg. '\\Computer\C$')
		/// </summary>
		public string ShareName { get; set; }

		/// <summary>
		/// Drive to be used in mapping / unmapping (eg. 's:')
		/// </summary>
		public string LocalDrive
		{
			get
			{
				return m_LocalDrive;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					m_LocalDrive = null;
				}
				else
				{
					m_LocalDrive = value.Substring(0, 1) + ":";
				}
			}
		}

		#endregion

		#region Public functions

		/// <summary>
		/// Returns a string array of currently mapped network drives
		/// </summary>
		public string[] MappedDrives
		{
			get
			{
				ArrayList driveArray = new ArrayList();
				foreach (string driveLetter in Directory.GetLogicalDrives())
				{
					if (PathIsNetworkPath(driveLetter))
					{
						driveArray.Add(driveLetter);
					}
				}
				return ((string[])driveArray.ToArray(typeof(string)));
			}
		}

		/// <summary>
		/// Map network drive
		/// </summary>
		public void MapDrive()
		{
			mapDrive(null, null);
		}

		/// <summary>
		/// Map network drive (using supplied Username and Password)
		/// </summary>
		/// <param name="username">Username passed for permissions / credintals ('Username' may be passed as null, to map using only a password)</param>
		/// <param name="password">Password passed for permissions / credintals</param>
		public void MapDrive(string username, string password)
		{
			mapDrive(username, password);
		}

		/// <summary>
		/// Set common propertys, then map the network drive
		/// </summary>
		/// <param name="localDrive">LocalDrive to use for connection</param>
		/// <param name="shareName">Share name for the connection (eg. '\\Computer\Share')</param>
		/// <param name="force">Option to force dis/connection</param>
		public void MapDrive(string localDrive, string shareName, bool force)
		{
			m_LocalDrive = localDrive;
			ShareName = shareName;
			Force = force;
			mapDrive(null, null);
		}

		/// <summary>
		/// Set common propertys, then map the network drive
		/// </summary>
		/// <param name="localDrive">Password passed for permissions / credintals</param>
		/// <param name="force">Option to force dis/connection</param>
		public void MapDrive(string localDrive, bool force)
		{
			m_LocalDrive = localDrive;
			Force = force;
			mapDrive(null, null);
		}

		/// <summary>
		/// Unmap network drive
		/// </summary>
		public void UnMapDrive()
		{
			unMapDrive();
		}

		/// <summary>
		/// Unmap network drive
		/// </summary>
		public void UnMapDrive(string localDrive)
		{
			m_LocalDrive = localDrive;
			unMapDrive();
		}

		/// <summary>
		/// Unmap network drive
		/// </summary>
		public void UnMapDrive(string localDrive, bool force)
		{
			m_LocalDrive = localDrive;
			Force = force;
			unMapDrive();
		}

		/// <summary>
		/// Check / restore persistent network drive
		/// </summary>
		public void RestoreDrives()
		{
			restoreDrive(null);
		}

		/// <summary>
		/// Check / restore persistent network drive
		/// </summary>
		public void RestoreDrive(string localDrive)
		{
			restoreDrive(localDrive);
		}

		/// <summary>
		/// Display windows dialog for mapping a network drive (using Desktop as parent form)
		/// </summary>		
		public void ShowConnectDialog()
		{
			displayDialog(IntPtr.Zero, 1);
		}

		/// <summary>
		/// Display windows dialog for mapping a network drive
		/// </summary>
		/// <param name="parentFormHandle">Form used as a parent for the dialog</param>
		public void ShowConnectDialog(IntPtr parentFormHandle)
		{
			displayDialog(parentFormHandle, 1);
		}

		/// <summary>
		/// Display windows dialog for disconnecting a network drive (using Desktop as parent form)
		/// </summary>		
		public void ShowDisconnectDialog()
		{
			displayDialog(IntPtr.Zero, 2);
		}

		/// <summary>
		/// Display windows dialog for disconnecting a network drive
		/// </summary>
		/// <param name="parentFormHandle">Form used as a parent for the dialog</param>
		public void ShowDisconnectDialog(IntPtr parentFormHandle)
		{
			displayDialog(parentFormHandle, 2);
		}

		/// <summary>
		/// Returns the share name of a connected network drive
		/// </summary>
		/// <param name="localDrive">Drive name (eg. 'X:')</param>
		/// <returns>Share name (eg. \\computer\share)</returns>
		public string GetMappedShareName(string localDrive)
		{
			// collect and clean the passed LocalDrive param
			if (string.IsNullOrEmpty(localDrive))
				throw new Exception("Invalid 'localDrive' passed, 'localDrive' parameter cannot be 'empty'");

			localDrive = localDrive.Substring(0, 1);

			// call api to collect LocalDrive's share name 
			int i = 255;
			byte[] bSharename = new byte[i];
			int iCallStatus = WNetGetConnection(localDrive + ":", bSharename, ref i);
			switch (iCallStatus)
			{
				case 1201:
					throw new Exception("Cannot collect 'ShareName', Passed 'DriveName' is valid but currently not connected (API: ERROR_CONNECTION_UNAVAIL)");
				case 1208:
					throw new Exception("API function 'WNetGetConnection' failed (API: ERROR_EXTENDED_ERROR:" + iCallStatus + ")");
				case 1203:
				case 1222:
					throw new Exception("Cannot collect 'ShareName', No network connection found (API: ERROR_NO_NETWORK / ERROR_NO_NET_OR_BAD_PATH)");
				case 2250:
					throw new Exception("Invalid 'DriveName' passed, Drive is not a network drive (API: ERROR_NOT_CONNECTED)");
				case 1200:
					throw new Exception("Invalid / Malfored 'Drive Name' passed to 'GetShareName' function (API: ERROR_BAD_DEVICE)");
				case 234:
					throw new Exception("Invalid 'Buffer' length, buffer is too small (API: ERROR_MORE_DATA)");
			}

			// return collected share name
			return Encoding.GetEncoding(1252).GetString(bSharename, 0, i).TrimEnd((char) 0);
		}

		/// <summary>
		/// Returns true if passed drive is a network drive
		/// </summary>
		/// <param name="localDrive">Drive name (eg. 'X:')</param>
		/// <returns>'True' if the passed drive is a mapped network drive</returns>
		public bool IsNetworkDrive(string localDrive)
		{
			// collect and clean the passed LocalDrive param
			if (localDrive == null || localDrive.Trim().Length == 0)
				throw new Exception("Invalid 'localDrive' passed, 'localDrive' parameter cannot be 'empty'");

			localDrive = localDrive.Substring(0, 1);

			// return status of drive type
			return PathIsNetworkPath(localDrive + ":");
		}

		#endregion

		#region Private functions

		/// <summary>
		/// Map network drive
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		private void mapDrive(string username, string password)
		{
			// if drive property is set to auto select, collect next free drive			
			if (FindNextFreeDrive)
			{
				m_LocalDrive = nextFreeDrive();

				if (string.IsNullOrEmpty(m_LocalDrive))
					throw new Exception("Could not find valid free drive name");
			}

			// create struct data to pass to the api function
			structNetResource stNetRes = new structNetResource();
			stNetRes.Scope = 2;
			stNetRes.Type = RESOURCETYPE_DISK;
			stNetRes.DisplayType = 3;
			stNetRes.Usage = 1;
			stNetRes.RemoteName = ShareName;
			stNetRes.LocalDrive = m_LocalDrive;

			// prepare flags for drive mapping options
			int iFlags = 0;
			if (SaveCredentials)
				iFlags += CONNECT_CMD_SAVECRED;
			if (Persistent)
				iFlags += CONNECT_UPDATE_PROFILE;
			if (PromptForCredentials)
				iFlags += CONNECT_INTERACTIVE + CONNECT_PROMPT;

			// prepare username / password params
			if (username != null && username.Length == 0)
				username = null;
			if (password != null && password.Length == 0)
				password = null;

			// if force, unmap ready for new connection
			if (Force)
			{
				try
				{
					unMapDrive();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex);
				}
			}

			// call and return
			int i = WNetAddConnection(ref stNetRes, password, username, iFlags);

			if (i > 0)
				throw new Win32Exception(i);
		}

		/// <summary>
		/// Unmap network drive
		/// </summary>
		private void unMapDrive()
		{
			// prep vars and call unmap
			int iFlags = 0;

			// if persistent, set flag
			if (Persistent)
			{
				iFlags += CONNECT_UPDATE_PROFILE;
			}

			// if local drive is null, unmap with use connection
			string name = m_LocalDrive ?? ShareName;
			int iRet = WNetCancelConnection(name, iFlags, Convert.ToInt32(Force));

			// if errors, throw exception
			if (iRet > 0)
				throw new Win32Exception(iRet);
		}

		/// <summary>
		/// Check or restore network drive
		/// </summary>
		/// <param name="driveName"></param>
		private static void restoreDrive(string driveName)
		{
			// call restore and return
			int i = WNetRestoreConnection(0, driveName);

			// if error returned, throw
			if (i > 0)
				throw new Win32Exception(i);
		}

		/// <summary>
		/// Display windows dialog
		/// </summary>
		/// <param name="wndHandle"></param>
		/// <param name="dialogToShow"></param>
		private static void displayDialog(IntPtr wndHandle, int dialogToShow)
		{
			// prep variables
			int i = -1;
			int iHandle = 0;

			// get parent handle
			if (wndHandle != IntPtr.Zero)
				iHandle = wndHandle.ToInt32();

			// choose dialog to show based on 
			if (dialogToShow == 1)
				i = WNetConnectionDialog(iHandle, RESOURCETYPE_DISK);
			else if (dialogToShow == 2)
				i = WNetDisconnectDialog(iHandle, RESOURCETYPE_DISK);

			// if error returned, throw
			if (i > 0)
				throw new Win32Exception(i);
		}

		/// <summary>
		/// Get the next viable drive name to use for mapping
		/// </summary>
		private static string nextFreeDrive()
		{
			// loop from c to z and check that drive is free
			string retValue = null;
			for (int i = 67; i <= 90; i++)
			{
				if (GetDriveType(((char) i) + ":") == 1)
				{
					retValue = ((char) i) + ":";
					break;
				}
			}

			// return selected drive
			return retValue;
		}

		#endregion

		#region API functions / calls

		private const int CONNECT_CMD_SAVECRED = 0x00001000;
		//private const int CONNECT_COMMANDLINE = 0x00000800;
		private const int CONNECT_INTERACTIVE = 0x00000008;
		private const int CONNECT_PROMPT = 0x00000010;
		//private const int CONNECT_REDIRECT = 0x00000080;
		private const int CONNECT_UPDATE_PROFILE = 0x00000001;
		private const int RESOURCETYPE_DISK = 0x1;

		[DllImport("mpr.dll", EntryPoint = "WNetAddConnection2A", CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern int WNetAddConnection(ref structNetResource netResStruct, string password, string username, int flags);

		[DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2A", CharSet = CharSet.Ansi, SetLastError = true)]
		private static extern int WNetCancelConnection(string name, int flags, int force);

		[DllImport("mpr.dll", EntryPoint = "WNetConnectionDialog", SetLastError = true)]
		private static extern int WNetConnectionDialog(int hWnd, int type);

		[DllImport("mpr.dll", EntryPoint = "WNetDisconnectDialog", SetLastError = true)]
		private static extern int WNetDisconnectDialog(int hWnd, int type);

		[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int WNetRestoreConnection(int hWnd, string localDrive);

		[DllImport("mpr.dll", EntryPoint = "WNetGetConnection", SetLastError = true)]
		private static extern int WNetGetConnection(string localDrive, byte[] remoteName, ref int bufferLength);

		[DllImport("shlwapi.dll", EntryPoint = "PathIsNetworkPath", SetLastError = true)]
		private static extern bool PathIsNetworkPath(string localDrive);

		[DllImport("kernel32.dll", EntryPoint = "GetDriveType", SetLastError = true)]
		private static extern int GetDriveType(string localDrive);

		[StructLayout(LayoutKind.Sequential)]
		private struct structNetResource
		{
			public int Scope;
			public int Type;
			public int DisplayType;
			public int Usage;
			public string LocalDrive;
			public string RemoteName;
			public string Comment;
			public string Provider;
		}

		#endregion
	}
}