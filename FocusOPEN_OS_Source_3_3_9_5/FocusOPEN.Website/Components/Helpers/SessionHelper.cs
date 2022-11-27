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
using System.IO;
using System.Web;
using System.Web.SessionState;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components
{
	public class SessionHelper
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(typeof(SessionHelper));

		#region Private Variables

		private readonly SessionInfo m_SessionInfo;

		#endregion

		#region Constructor

		private SessionHelper(SessionInfo sessionInfo)
		{
			m_SessionInfo = sessionInfo;
		}

		#endregion

		#region Factory Methods

		public static SessionHelper GetForCurrentSession()
		{
			return GetForSession(HttpContext.Current.Session);
		}

		public static SessionHelper GetForSession(HttpSessionState session)
		{
			SessionInfo sessionInfo = session[SessionInfo.SessionKey] as SessionInfo;

			if (sessionInfo == null)
				sessionInfo = SessionInfo.Empty;

			return new SessionHelper(sessionInfo);
		}

		public static SessionHelper GetForSessionInfo(SessionInfo sessionInfo)
		{
			return new SessionHelper(sessionInfo);
		}

		#endregion

		#region Private Accessors

		private SessionInfo CurrentSessionInfo
		{
			get
			{
				return m_SessionInfo;
			}
		}

		#endregion

		/// <summary>
		/// Gets the path to the temp folder for the currently logged in user.
		/// Returns empty string is no user is logged in
		/// </summary>
		private string GetUserIdFolder()
		{
			if (CurrentSessionInfo.User.UserId.HasValue)
			{
				string userIdFolderName = string.Format("U{0}", CurrentSessionInfo.User.UserId);
				return Path.Combine(Settings.TempFolder, userIdFolderName);
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the path to the temp folder for the currently logged in user.
		/// </summary>
		public string GetSessionTempFolder()
		{
			string userIdFolder = GetUserIdFolder();

			if (userIdFolder == string.Empty)
				userIdFolder = Path.Combine(Settings.TempFolder, "__NULLUSER");

			return userIdFolder;
		}

		/// <summary>
		/// Gets the path to the temp folder for the current user and session.
		/// If the folder does not exist, it is created.
		/// </summary>
		/// <returns></returns>
		public string CreateSessionTempFolder()
		{
			string sessionTempFolder = GetSessionTempFolder();

			if (!Directory.Exists(sessionTempFolder))
			{
				Directory.CreateDirectory(sessionTempFolder);
				m_Logger.DebugFormat("CreateSessionTempFolder() - Created directory: {0}", sessionTempFolder);
			}

			string sessionFolder = Path.Combine(sessionTempFolder, HttpContext.Current.Session.SessionID);

			if (!Directory.Exists(sessionFolder))
			{
				Directory.CreateDirectory(sessionFolder);
				m_Logger.DebugFormat("GetOrCreateSessionTempFolder() - Created directory: {0}", sessionFolder);
			}

			return sessionFolder;
		}

		/// <summary>
		/// Deletes the temp folder for the currently logged in user
		/// </summary>
		public void DeleteSessionTempFolder()
		{
			m_Logger.Debug("DeleteSessionTempFolder()");

			string userIdFolder = GetUserIdFolder();

			if (userIdFolder != string.Empty)
			{
				if (Directory.Exists(userIdFolder))
				{
					try
					{
						Directory.Delete(userIdFolder, true);
						m_Logger.DebugFormat("Deleted directory: {0}", userIdFolder);
					}
					catch (Exception ex)
					{
						m_Logger.Warn(string.Format("Error deleting: {0}", userIdFolder), ex);
					}
				}
				else
				{
					m_Logger.DebugFormat("Folder does not exist: {0}.  No cleanup required", userIdFolder);
				}
			}
		}
	}
}