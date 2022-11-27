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
	/// This object represents the properties and methods of a AuditUserHistory.
	/// </summary>
	public partial class AuditUserHistory
	{
		#region Constructor
		
		protected AuditUserHistory()
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
		
		public static AuditUserHistory New ()
		{
			return new AuditUserHistory() ;
		}

		public static AuditUserHistory Empty
		{
			get { return NullAuditUserHistory.Instance; }
		}

		public static AuditUserHistory Get (Nullable <Int32> AuditUserHistoryId)
		{
			AuditUserHistory AuditUserHistory = AuditUserHistoryMapper.Instance.Get (AuditUserHistoryId);
			return AuditUserHistory ?? Empty;
		}

		public static AuditUserHistory Update (AuditUserHistory auditUserHistory)
		{
			return AuditUserHistoryMapper.Instance.Update(auditUserHistory) ;
		}

		public static void Delete (Nullable <Int32> AuditUserHistoryId)
		{
			AuditUserHistoryMapper.Instance.Delete (AuditUserHistoryId);
		}

		public static EntityList <AuditUserHistory> FindMany (AuditUserHistoryFinder finder, int Page, int PageSize)
		{
			return AuditUserHistoryMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AuditUserHistory> FindMany (AuditUserHistoryFinder finder)
		{
			return AuditUserHistoryMapper.Instance.FindMany (finder);
		}

		public static AuditUserHistory FindOne (AuditUserHistoryFinder finder)
		{
			AuditUserHistory AuditUserHistory = AuditUserHistoryMapper.Instance.FindOne(finder);
			return AuditUserHistory ?? Empty;
		}

		public static int GetCount (AuditUserHistoryFinder finder)
		{
			return AuditUserHistoryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
