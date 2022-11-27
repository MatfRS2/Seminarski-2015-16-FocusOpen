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
	/// This object represents the properties and methods of a AuditAssetSearchKeyword.
	/// </summary>
	public partial class AuditAssetSearchKeyword
	{
		#region Constructor
		
		protected AuditAssetSearchKeyword()
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
		
		public static AuditAssetSearchKeyword New ()
		{
			return new AuditAssetSearchKeyword() ;
		}

		public static AuditAssetSearchKeyword Empty
		{
			get { return NullAuditAssetSearchKeyword.Instance; }
		}

		public static AuditAssetSearchKeyword Get (Nullable <Int32> AuditAssetSearchKeywordId)
		{
			AuditAssetSearchKeyword AuditAssetSearchKeyword = AuditAssetSearchKeywordMapper.Instance.Get (AuditAssetSearchKeywordId);
			return AuditAssetSearchKeyword ?? Empty;
		}

		public static AuditAssetSearchKeyword Update (AuditAssetSearchKeyword auditAssetSearchSector)
		{
			return AuditAssetSearchKeywordMapper.Instance.Update(auditAssetSearchSector) ;
		}

		public static void Delete (Nullable <Int32> AuditAssetSearchKeywordId)
		{
			AuditAssetSearchKeywordMapper.Instance.Delete (AuditAssetSearchKeywordId);
		}

		public static EntityList <AuditAssetSearchKeyword> FindMany (AuditAssetSearchKeywordFinder finder, int Page, int PageSize)
		{
			return AuditAssetSearchKeywordMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AuditAssetSearchKeyword> FindMany (AuditAssetSearchKeywordFinder finder)
		{
			return AuditAssetSearchKeywordMapper.Instance.FindMany (finder);
		}

		public static AuditAssetSearchKeyword FindOne (AuditAssetSearchKeywordFinder finder)
		{
			AuditAssetSearchKeyword AuditAssetSearchKeyword = AuditAssetSearchKeywordMapper.Instance.FindOne(finder);
			return AuditAssetSearchKeyword ?? Empty;
		}

		public static int GetCount (AuditAssetSearchKeywordFinder finder)
		{
			return AuditAssetSearchKeywordMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
