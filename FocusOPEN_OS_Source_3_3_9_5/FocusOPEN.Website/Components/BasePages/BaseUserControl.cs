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
using System.Web.UI;
using FocusOPEN.Data;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Forms the base for all user controls (ASCX files), exposing
	/// common properties and methods
	/// </summary>
	public abstract class BaseUserControl : UserControl
	{
		private string m_MessageLabelControlId = string.Empty;
		private FeedbackLabel m_MessageLabel = null;

		/// <summary>
		/// Gets or sets the ID of the message label control on the page
		/// </summary>
		public string MessageLabelControlId
		{
			get
			{
				return m_MessageLabelControlId;
			}
			set
			{
				m_MessageLabelControlId = value;
			}
		}

		/// <summary>
		/// Gets or sets the MessageLabel control to which this
		/// user control should send any messages
		/// </summary>
		protected FeedbackLabel MessageLabel
		{
			get
			{
				if (m_MessageLabel == null)
				{
					if (m_MessageLabelControlId == string.Empty)
					{
						//throw new Exception("MessageLabelControlId has not been specified, which is required before the MessageLabel can be used.");
						return new FeedbackLabel();
					}

					m_MessageLabel = SiteUtils.FindControlRecursive(Page, m_MessageLabelControlId) as FeedbackLabel;

					if (m_MessageLabel == null)
						throw new Exception(string.Format("A MessageLabel with ID '{0}' was not found on the page", MessageLabelControlId));
				}
				return (m_MessageLabel);
			}
			set
			{
				m_MessageLabelControlId = value.ID;
				m_MessageLabel = value;
			}
		}

		/// <summary>
		/// Gets the current user
		/// </summary>
		protected static User CurrentUser
		{
			get
			{
				return SessionInfo.Current.User;
			}
		}

		protected T GetFromViewState<T>(string key, T defaultVal)
		{
			return SiteUtils.GetFromStore(ViewState, key, defaultVal);
		}
	}
}