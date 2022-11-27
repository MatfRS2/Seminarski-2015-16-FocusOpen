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
	public class RegistrationSecurityException : BaseEntityException<User>
	{
		#region Private Variables

		private string m_IpAddress = string.Empty;

		#endregion

		#region Accessors

		public string IpAddress
		{
			get
			{
				return m_IpAddress;
			}
			set
			{
				m_IpAddress = value;
			}
		}

		#endregion

		#region Constructor

		public RegistrationSecurityException(User entity, string message, string ipAddress) : base(message, entity)
		{
			m_IpAddress = ipAddress;
		}

		#endregion

	}
}