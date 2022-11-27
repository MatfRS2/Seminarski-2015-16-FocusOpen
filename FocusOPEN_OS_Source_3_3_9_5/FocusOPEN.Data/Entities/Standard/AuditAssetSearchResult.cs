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
	/// This object represents the properties and methods of a AuditAssetSearchResult.
	/// </summary>
	public partial class AuditAssetSearchResult
	{
		#region Constructor
		
		protected AuditAssetSearchResult()
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
		
		public static AuditAssetSearchResult New ()
		{
			return new AuditAssetSearchResult() ;
		}

		public static AuditAssetSearchResult Empty
		{
			get { return NullAuditAssetSearchResult.Instance; }
		}

		public static AuditAssetSearchResult Get (Nullable <Int32> AuditAssetSearchResultId)
		{
			AuditAssetSearchResult AuditAssetSearchResult = AuditAssetSearchResultMapper.Instance.Get (AuditAssetSearchResultId);
			return AuditAssetSearchResult ?? Empty;
		}

		public static AuditAssetSearchResult Update (AuditAssetSearchResult auditAssetSearchResult)
		{
			return AuditAssetSearchResultMapper.Instance.Update(auditAssetSearchResult) ;
		}

		public static void Delete (Nullable <Int32> AuditAssetSearchResultId)
		{
			AuditAssetSearchResultMapper.Instance.Delete (AuditAssetSearchResultId);
		}

		public static EntityList <AuditAssetSearchResult> FindMany (AuditAssetSearchResultFinder finder, int Page, int PageSize)
		{
			return AuditAssetSearchResultMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AuditAssetSearchResult> FindMany (AuditAssetSearchResultFinder finder)
		{
			return AuditAssetSearchResultMapper.Instance.FindMany (finder);
		}

		public static AuditAssetSearchResult FindOne (AuditAssetSearchResultFinder finder)
		{
			AuditAssetSearchResult AuditAssetSearchResult = AuditAssetSearchResultMapper.Instance.FindOne(finder);
			return AuditAssetSearchResult ?? Empty;
		}

		public static int GetCount (AuditAssetSearchResultFinder finder)
		{
			return AuditAssetSearchResultMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
