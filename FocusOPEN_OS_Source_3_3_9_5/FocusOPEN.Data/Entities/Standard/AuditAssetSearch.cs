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
	/// This object represents the properties and methods of a AuditAssetSearch.
	/// </summary>
	public partial class AuditAssetSearch
	{
		#region Constructor
		
		protected AuditAssetSearch()
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
		
		public static AuditAssetSearch New ()
		{
			return new AuditAssetSearch() ;
		}

		public static AuditAssetSearch Empty
		{
			get { return NullAuditAssetSearch.Instance; }
		}

		public static AuditAssetSearch Get (Nullable <Int32> AuditAssetSearchId)
		{
			AuditAssetSearch AuditAssetSearch = AuditAssetSearchMapper.Instance.Get (AuditAssetSearchId);
			return AuditAssetSearch ?? Empty;
		}

		public static AuditAssetSearch Update (AuditAssetSearch auditAssetSearch)
		{
			return AuditAssetSearchMapper.Instance.Update(auditAssetSearch) ;
		}

		public static void Delete (Nullable <Int32> AuditAssetSearchId)
		{
			AuditAssetSearchMapper.Instance.Delete (AuditAssetSearchId);
		}

		public static EntityList <AuditAssetSearch> FindMany (AuditAssetSearchFinder finder, int Page, int PageSize)
		{
			return AuditAssetSearchMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <AuditAssetSearch> FindMany (AuditAssetSearchFinder finder)
		{
			return AuditAssetSearchMapper.Instance.FindMany (finder);
		}

		public static AuditAssetSearch FindOne (AuditAssetSearchFinder finder)
		{
			AuditAssetSearch AuditAssetSearch = AuditAssetSearchMapper.Instance.FindOne(finder);
			return AuditAssetSearch ?? Empty;
		}

		public static int GetCount (AuditAssetSearchFinder finder)
		{
			return AuditAssetSearchMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
