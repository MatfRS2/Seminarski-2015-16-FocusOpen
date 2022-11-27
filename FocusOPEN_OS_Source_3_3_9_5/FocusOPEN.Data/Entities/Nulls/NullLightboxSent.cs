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

namespace FocusOPEN.Data
{

    /// <summary>
    /// This object represents a null LightboxSent.
    /// </summary>
	[Serializable]
	public class NullLightboxSent : LightboxSent
	{
		#region Singleton implementation

		private NullLightboxSent()
		{
		}

		private static readonly NullLightboxSent m_instance = new NullLightboxSent();

		public static NullLightboxSent Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the LightboxId of the LightboxSent object.
		/// </summary>
		public override int LightboxId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreatedLightboxId of the LightboxSent object.
		/// </summary>
		public override Nullable <Int32> CreatedLightboxId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the SenderId of the LightboxSent object.
		/// </summary>
		public override int SenderId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the RecipientEmail of the LightboxSent object.
		/// </summary>
		public override string RecipientEmail
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Subject of the LightboxSent object.
		/// </summary>
		public override string Subject
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Message of the LightboxSent object.
		/// </summary>
		public override string Message
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the DateSent of the LightboxSent object.
		/// </summary>
		public override DateTime DateSent
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the ExpiryDate of the LightboxSent object.
		/// </summary>
		public override Nullable <DateTime> ExpiryDate
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the DownloadLinks of the LightboxSent object.
		/// </summary>
		public override Nullable <Boolean> DownloadLinks
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the LightboxLinkedId of the LightboxSent object.
		/// </summary>
		public override Nullable <Int32> LightboxLinkedId
		{
			get { return null; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

