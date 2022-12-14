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
	/// This object maps data between the database and a AuditAssetSearchResult object.
	/// </summary>
	internal partial class AuditAssetSearchResultMapper
	{
		#region Singleton behaviour

		private AuditAssetSearchResultMapper()
		{
		}

		private static readonly AuditAssetSearchResultMapper m_instance = new AuditAssetSearchResultMapper();

		public static AuditAssetSearchResultMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion
	}
}