/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a LightboxSent.
	/// </summary>
	public partial class LightboxSent
	{
		#region Private variables

		private Lightbox m_Lightbox = null;
		private User m_RecipientUser = null;
		private User m_Sender = null;

		#endregion

		#region Lazy Loads

		public Lightbox Lightbox
		{
			get
			{
				if (m_Lightbox == null)
					m_Lightbox = Lightbox.Get(LightboxId);

				return m_Lightbox;
			}
		}

		public User Sender
		{
			get
			{
				if (m_Sender == null)
					m_Sender = User.Get(SenderId);

				return (m_Sender);
			}
		}

		public User RecipientUser
		{
			get
			{
				if (m_RecipientUser == null)
					m_RecipientUser = User.GetByEmail(RecipientEmail);

				return m_RecipientUser;
			}
		}

		#endregion
	}
}