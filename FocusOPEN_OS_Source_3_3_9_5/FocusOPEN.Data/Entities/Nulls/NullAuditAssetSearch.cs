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
	/// This object represents a null AuditAssetSearch.
	/// </summary>
	[Serializable]
	public class NullAuditAssetSearch : AuditAssetSearch
	{
		#region Singleton implementation

		private NullAuditAssetSearch()
		{
		}

		private static readonly NullAuditAssetSearch m_instance = new NullAuditAssetSearch();

		public static NullAuditAssetSearch Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the SessionId of the AuditAssetSearch object.
		/// </summary>
		public override string SessionId
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IpAddress of the AuditAssetSearch object.
		/// </summary>
		public override string IpAddress
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the UserId of the AuditAssetSearch object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Date of the AuditAssetSearch object.
		/// </summary>
		public override DateTime Date
		{
			get { return DateTime.MinValue; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

