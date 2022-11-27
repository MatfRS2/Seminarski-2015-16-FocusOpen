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
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a AuditAssetHistory.
	/// </summary>
	public partial class AuditAssetHistory
	{
		#region Constructor
		
		protected AuditAssetHistory()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static AuditAssetHistory New ()
		{
			return new AuditAssetHistory() ;
		}

		public static AuditAssetHistory Empty
		{
			get { return NullAuditAssetHistory.Instance; }
		}

		public static AuditAssetHistory Get (Nullable <Int32> AuditAssetHistoryId)
		{
			AuditAssetHistory AuditAssetHistory = AuditAssetHistoryMapper.Instance.Get (AuditAssetHistoryId);
			return AuditAssetHistory ?? Empty;
		}

		public static AuditAssetHistory Update (AuditAssetHistory auditAssetHistory)
		{
			return AuditAssetHistoryMapper.Instance.Update(auditAssetHistory) ;
		}

		public static void Delete (Nullable <Int32> AuditAssetHistoryId)
		{
			AuditAssetHistoryMapper.Instance.Delete (AuditAssetHistoryId);
		}

		public static EntityList <AuditAssetHistory> FindMany (AuditAssetHistoryFinder finder, int Page, int PageSize)
		{
			return AuditAssetHistoryMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AuditAssetHistory> FindMany (AuditAssetHistoryFinder finder)
		{
			return AuditAssetHistoryMapper.Instance.FindMany (finder);
		}

		public static AuditAssetHistory FindOne (AuditAssetHistoryFinder finder)
		{
			AuditAssetHistory AuditAssetHistory = AuditAssetHistoryMapper.Instance.FindOne(finder);
			return AuditAssetHistory ?? Empty;
		}

		public static int GetCount (AuditAssetHistoryFinder finder)
		{
			return AuditAssetHistoryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
