/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object maps data between the database and a AuditAssetHistory object.
	/// </summary>
	internal partial class AuditAssetHistoryMapper
	{
		#region Singleton behaviour

		private AuditAssetHistoryMapper()
		{
		}

		private static readonly AuditAssetHistoryMapper m_instance = new AuditAssetHistoryMapper();

		public static AuditAssetHistoryMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

		public DataTable GetAuditAssetActionList()
		{
			const string sql = "SELECT * FROM AuditAssetAction ORDER BY [Description]";
			return GetDataTable(sql);
		}
	}
}