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
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// When an ASP.NET State Server other than InProc, the Session_End event
	/// does not get fired.  This is an HttpModule which uses some workarounds
	/// and fires a static event when a session ends, with the value of a single
	/// configurable session variable in the event arguments.
	/// </summary>
	public class SessionEndModule : IHttpModule
	{
		#region Private Variables

		private HttpApplication m_HttpApplication;

		#endregion

		#region Accessors

		/// <summary>
		/// This is the key of the item in the session which should be returned
		/// in the SessionEnd event (as the SessionObject).
		/// </summary>
		/// <example>
		///	If you're storing the user ID in the session, under a key called 'UserId'
		/// and need to do something with it in the SessionEnd event, you would set
		/// this to 'UserId', which would cause the value of the session key called
		/// 'UserId' to be returned.
		/// </example>
		public static string SessionObjectKey { get; set; }

		#endregion

		#region IHttpModule Implementation

		public void Init(HttpApplication context)
		{
			m_HttpApplication = context;
			m_HttpApplication.PreRequestHandlerExecute += new EventHandler(OnPreRequestHandlerExecute);
		}

		public void Dispose()
		{
			// Do Nothing
		}

		#endregion

		#region Events

		/// <summary>
		/// Event raised when the session ends
		/// </summary>
		public static event SessionEndEventHandler SessionEnd;

		#endregion

		/// <summary>
		/// Fires the session end event and removes it from the
		/// application cache (so it doesnt fire again)
		/// </summary>
		public static void EndSession()
		{
			// Ensure we have a session key
			if (SessionObjectKey == null)
				return;

			// Ensure we have a HttpContext
			if (HttpContext.Current == null)
			{
				Debug.WriteLine("No current http context");
				return;
			}

			// Get the current session
			HttpSessionState currentSession = HttpContext.Current.Session;

			// Ensure we have a current session
			if (currentSession == null)
			{
				Debug.WriteLine("No current session");
				return;
			}

			if (SessionEnd != null)
			{
				string key = currentSession.SessionID;
				object value = currentSession[SessionObjectKey];

				SessionEndedEventArgs e = new SessionEndedEventArgs(key, value);
				SessionEnd(null, e);
			}

			// Remove the session from the cache
			HttpContext.Current.Cache.Remove(currentSession.SessionID);
		}

		private void OnPreRequestHandlerExecute(object sender, EventArgs e)
		{
			// We only want to update the session when an ASPX page is being viewed
			// We're also doing this in the PreRequestHandler, as doing it elsewhere
			// (like the PostRequestHandler) can cause some strange behaviour.

			if (StringUtils.GetFileExtension(m_HttpApplication.Context.Request.Path) == "aspx")
			{
				// Ensure we have a session key
				if (SessionObjectKey == null)
					return;

				// Ensure we have a HttpContext
				if (HttpContext.Current == null)
				{
					Debug.WriteLine("No current http context");
					return;
				}

				// Get the current session
				HttpSessionState currentSession = HttpContext.Current.Session;

				// Ensure we have a current session
				if (currentSession == null)
				{
					Debug.WriteLine("No current session");
					return;
				}

				// Get the session timeout
				TimeSpan sessionTimeout = new TimeSpan(0, 0, currentSession.Timeout, 0, 0);

				// Get the object in the session we want to retrieve when the session times out
				object sessionObject = currentSession[SessionObjectKey];

				// Add the object to the cache with the current session id, and set a cache removal callback method
				HttpContext.Current.Cache.Insert(currentSession.SessionID, sessionObject, null, DateTime.MaxValue, sessionTimeout, CacheItemPriority.NotRemovable, new CacheItemRemovedCallback(CacheItemRemovedCallbackMethod));
			}
		}

		/// <summary>
		/// This method is fired when an item is removed from the cache.  It is used to detect when a cache item
		/// expires, indicating that the session has expired, and fires the SessionEnd event.
		/// </summary>
		private void CacheItemRemovedCallbackMethod(string key, object value, CacheItemRemovedReason reason)
		{
			if (reason == CacheItemRemovedReason.Expired)
			{
				if (SessionEnd != null)
				{
					SessionEndedEventArgs e = new SessionEndedEventArgs(key, value);
					SessionEnd(this, e);
				}
			}
		}
	}

	/// <summary>
	/// Event handler for handling the SessionEnd event
	/// </summary>
	public delegate void SessionEndEventHandler(object sender, SessionEndedEventArgs e);

	/// <summary>
	/// SessionEndedEventArgs for use in the SessionEnd event
	/// </summary>
	public class SessionEndedEventArgs : EventArgs
	{
		public readonly string SessionId;
		public readonly object SessionObject;

		public SessionEndedEventArgs(string sessionId, object sessionObject)
		{
			SessionId = sessionId;
			SessionObject = sessionObject;
		}
	}
}