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
	/// This object maps data between the database and a AuditAssetSearchKeyword object.
	/// </summary>
	internal partial class AuditAssetSearchKeywordMapper
	{
		#region Singleton behaviour

		private AuditAssetSearchKeywordMapper()
		{
		}
		
		private static readonly AuditAssetSearchKeywordMapper m_instance = new AuditAssetSearchKeywordMapper();
		public static AuditAssetSearchKeywordMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion
	}
}

