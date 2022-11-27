/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Data;

namespace FocusOPEN.Business
{
	public class LoginSecurityException : RegistrationSecurityException
	{
		#region Private Variables

		private bool m_NotifyAdmins = true;

		#endregion

		#region Accessors

		public bool NotifyAdmins
		{
			get
			{
				return m_NotifyAdmins;
			}
			set
			{
				m_NotifyAdmins = value;
			}
		}

		#endregion

		#region Constructor

		public LoginSecurityException(User entity, string message, string ipAddress) : base(entity, message, ipAddress)
		{
		}

		public LoginSecurityException(User entity, string message, string ipAddress, bool notifyAdmins) : this(entity, message, ipAddress)
		{
			m_NotifyAdmins = notifyAdmins;
		}

		#endregion
	}
}