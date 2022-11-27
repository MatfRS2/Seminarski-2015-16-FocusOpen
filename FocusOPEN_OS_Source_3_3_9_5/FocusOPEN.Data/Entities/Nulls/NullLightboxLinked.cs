/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullLightboxLinked : LightboxLinked
	{
		#region Singleton implementation

		private NullLightboxLinked()
		{
		}

		private static readonly NullLightboxLinked m_instance = new NullLightboxLinked();

		public static NullLightboxLinked Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the LightboxId of the LightboxLinked object.
		/// </summary>
		public override int LightboxId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserId of the LightboxLinked object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsEditable of the LightboxLinked object.
		/// </summary>
		public override Nullable <Boolean> IsEditable
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the ExpiryDate of the LightboxLinked object.
		/// </summary>
		public override Nullable <DateTime> ExpiryDate
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the Disabled of the LightboxLinked object.
		/// </summary>
		public override Nullable <Boolean> Disabled
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

