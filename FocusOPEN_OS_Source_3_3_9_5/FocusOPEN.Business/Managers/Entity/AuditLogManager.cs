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
using System.Reflection;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	public static class AuditLogManager
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Search Logging

		/// <summary>
		/// Creates an audit log entry for the specified search and
		/// returns the auditassetsearch (which is needed to track
		/// which assets are viewed for this search on a page-by-page basis)
		/// </summary>
		/// <param name="assetFinder">The asset finder being used for the search</param>
		/// <param name="user">The user performing the search</param>
		public static AuditAssetSearch LogSearch(AssetFinder assetFinder, User user)
		{
			if (user.IsNull)
			{
				m_Logger.WarnFormat("Unable to log search; user is null");
				return AuditAssetSearch.Empty;
			}

			AuditAssetSearch aus = AuditAssetSearch.New();
			aus.SessionId = BusinessHelper.GetCurrentSessionId();
			aus.IpAddress = BusinessHelper.GetCurrentIpAddress();
			aus.UserId = user.UserId.GetValueOrDefault();
			aus.Date = DateTime.Now;
			AuditAssetSearch.Update(aus);

			AuditAssetSearchKeyword aask = AuditAssetSearchKeyword.New();
			aask.AuditAssetSearchId = aus.AuditAssetSearchId.GetValueOrDefault();
			aask.SearchKeyword = assetFinder.GeneralKeyword;
			AuditAssetSearchKeyword.Update(aask);

			int assetCount = Asset.GetCount(assetFinder);

			string notes = string.Format("Searched for '{0}' (Criteria Count: {1}).  Found {2} assets.", assetFinder.GeneralKeyword, assetFinder.FindCriteriaCount, assetCount);
			LogUserAction(user, AuditUserAction.Search, notes);

			return aus;
		}

		public static void LogAssetInSearchResult(int auditAssetSearchId, int assetId)
		{
			if (auditAssetSearchId == 0 || assetId == 0)
				return;

			try
			{
				AuditAssetSearchResult aasr = AuditAssetSearchResult.New();
				aasr.AuditAssetSearchId = auditAssetSearchId;
				aasr.AssetId = assetId;
				aasr.Date = DateTime.Now;
				AuditAssetSearchResult.Update(aasr);
			}
			catch (Exception e)
			{
				m_Logger.Error(string.Format("Error logging asset in search result - AuditAssetSearchId: {0}, AssetId: {1}, Error: {2}", auditAssetSearchId, assetId, e.Message), e);
#if DEBUG
				throw;
#endif
			}
		}

		#endregion

		#region Asset Logging

		public static void LogAssetAction(Asset asset, User user, AuditAssetAction auditAssetAction)
		{
			LogAssetAction(asset.AssetId.GetValueOrDefault(), user, auditAssetAction, string.Empty);
		}

		public static void LogAssetAction(Asset asset, User user, AuditAssetAction auditAssetAction, string notes)
		{
			LogAssetAction(asset.AssetId.GetValueOrDefault(), user, auditAssetAction, notes);
		}

		public static void LogAssetAction(int assetId, User user, AuditAssetAction auditAssetAction)
		{
			LogAssetAction(assetId, user, auditAssetAction, string.Empty);
		}

		public static void LogAssetAction(int assetId, User user, AuditAssetAction auditAssetAction, string notes)
		{
			AuditAssetHistory aah = AuditAssetHistory.New();
			aah.SessionId = BusinessHelper.GetCurrentSessionId();
			aah.IpAddress = BusinessHelper.GetCurrentIpAddress();
			aah.AuditAssetActionId = Convert.ToInt32(auditAssetAction);
			aah.UserId = user.UserId.GetValueOrDefault();
			aah.AssetId = assetId;
			aah.Notes = notes;
			aah.Date = DateTime.Now;
			AuditAssetHistory.Update(aah);

			m_Logger.InfoFormat("LogAssetAction - Asset ID: {0}, User: {1}, Action: {2}, Notes: {3}", assetId, user.FullName, auditAssetAction, notes);
		}

		#endregion

		#region User Logging

		public static void LogUserAction(User user, AuditUserAction auditUserAction, string notes)
		{
			AuditUserHistory auh = AuditUserHistory.New();
			auh.SessionId = BusinessHelper.GetCurrentSessionId();
			auh.IpAddress = BusinessHelper.GetCurrentIpAddress();
			auh.AuditUserActionId = Convert.ToInt32(auditUserAction);
			auh.UserId = user.UserId.GetValueOrDefault();
			auh.Notes = notes;
			auh.Date = DateTime.Now;
			AuditUserHistory.Update(auh);

			m_Logger.InfoFormat("LogUserAction - User: {0}, Action: {1}, Notes: {2}", user.FullName, auditUserAction, notes);
		}

		#endregion
	}
}