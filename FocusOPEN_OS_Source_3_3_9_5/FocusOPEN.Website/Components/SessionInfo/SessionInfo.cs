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
using System.Diagnostics;
using System.Web;
using System.Web.SessionState;
using FocusOPEN.Data;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// This class is used for storing all information which needs to be 
	/// maintained at Session Scope. If more items need to be stored in 
	/// Session later in the project, add a property to this class.
	/// </summary>
	[Serializable]
	public class SessionInfo
	{
		public const string SessionKey = "SessionInfo";

		#region Private Variables

		private AdminSessionInfo m_AdminSessionInfo;
		private UserSessionInfo m_UserSessionInfo;

		#endregion

		#region Accessors

		public User User { get; set; }

		public AdminSessionInfo AdminSessionInfo
		{
			get
			{
				if (m_AdminSessionInfo == null)
					m_AdminSessionInfo = new AdminSessionInfo();

				return m_AdminSessionInfo;
			}
			set
			{
				m_AdminSessionInfo = value;
			}
		}

		public UserSessionInfo UserSessionInfo
		{
			get
			{
				if (m_UserSessionInfo == null)
					m_UserSessionInfo = new UserSessionInfo();

				return m_UserSessionInfo;
			}
			set
			{
				m_UserSessionInfo = value;
			}
		}

		#endregion

		#region Static Accessors

		/// <summary>
		/// Returns a SessionInfo instance for the current user session
		/// </summary>
		/// 
		public static SessionInfo Current
		{
			get
			{
				if (HttpContext.Current == null || HttpContext.Current.Session == null)
				{
					Debug.WriteLine("SessionInfo.Current : No HttpContext, so session info will not be saved");
					return new SessionInfo();
				}

				return ForSession(HttpContext.Current.Session);
			}
		}

		public static SessionInfo Empty
		{
			get
			{
				return new SessionInfo();
			}
		}

		#endregion

		// Instances of SessionInfo should only be created by 
		// the Current property, so make the constructor private
		private SessionInfo()
		{
			Reset();
		}

		/// <summary>
		/// Reset all session settings to default values
		/// </summary>
		public void Reset()
		{
			User = User.Empty;
			UserSessionInfo.Reset();
			AdminSessionInfo.Reset();
		}

		/// <summary>
		/// Gets the SessionInfo for the specified session
		/// </summary>
		public static SessionInfo ForSession(HttpSessionState session)
		{
			SessionInfo sessionInfo = session[SessionKey] as SessionInfo;
			if (sessionInfo == null)
			{
				sessionInfo = new SessionInfo();
				session.Add(SessionKey, sessionInfo);
			}
			return sessionInfo;
		}
	}
}