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
	/// This object represents a null AuditAssetSearchKeyword.
	/// </summary>
	[Serializable]
	public class NullAuditAssetSearchKeyword : AuditAssetSearchKeyword
	{
		#region Singleton implementation

		private NullAuditAssetSearchKeyword()
		{
		}

		private static readonly NullAuditAssetSearchKeyword m_instance = new NullAuditAssetSearchKeyword();

		public static NullAuditAssetSearchKeyword Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the AuditAssetSearchId of the AuditAssetSearchKeyword object.
		/// </summary>
		public override int AuditAssetSearchId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the SearchKeyword of the AuditAssetSearchKeyword object.
		/// </summary>
		public override string SearchKeyword
		{
			get { return String.Empty; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

