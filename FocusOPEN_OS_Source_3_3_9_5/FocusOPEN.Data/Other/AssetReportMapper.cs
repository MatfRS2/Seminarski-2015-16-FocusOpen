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
using System.Data;
using Daydream.Data;

namespace FocusOPEN.Data
{
	public class AssetReportMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation

		protected override object ReadRow(IRowReader reader)
		{
			throw new NotImplementedException();
		}

		protected override IEntityList CreateObjectList()
		{
			throw new NotImplementedException();
		}

		public override IEntity GetFromKey(object key)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Singleton Pattern

		private static AssetReportMapper m_Instance;

		private AssetReportMapper()
		{
		}

		public static AssetReportMapper Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = new AssetReportMapper();

				return (m_Instance);
			}
		}

		#endregion

		public DataRow GetAssetStats(int assetId, DateRange timeframe)
		{
			using (IDbCommand command = CreateCommand())
			{
				command.CommandText = "usp_GetAssetStats";
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add(CreateParameter("@assetId", assetId));
				
				if (!timeframe.IsNull)
				{
					command.Parameters.Add(CreateParameter("@startDate", timeframe.StartDate));
					command.Parameters.Add(CreateParameter("@endDate", timeframe.EndDate));
				}

				return GetDataRow(command);
			}
		}

		public DataRow GetAssetReportSummary(int brandId, DateRange timeframe)
		{
			using (IDbCommand command = CreateCommand())
			{
				command.CommandText = "usp_GetAssetReportSummary";
				command.CommandType = CommandType.StoredProcedure;
				
				if (brandId > 0)
					command.Parameters.Add(CreateParameter("@brandId", brandId));

				if (!timeframe.IsNull)
				{
					command.Parameters.Add(CreateParameter("@startDate", timeframe.StartDate));
					command.Parameters.Add(CreateParameter("@endDate", timeframe.EndDate));
				}

				return GetDataRow(command);
			}
		}

		public DataTable GetTopSearchTerms(int assetId)
		{
			string sql = "EXEC usp_GetTopSearchTerms @assetId=" + assetId;
			return GetDataTable(sql);
		}

		public DataTable GetLastUsersToDownload(int assetId)
		{
			string sql = "EXEC usp_GetLastDownloadUsers @assetId=" + assetId;
			return GetDataTable(sql);
		}

		public DataTable GetPopularAssetList(int top, int assetTypeId, int? brandId, DateRange timeframe)
		{
			using (IDbCommand command = CreateCommand())
			{
				command.CommandText = "usp_GetAssetPopularityList";
				command.CommandType = CommandType.StoredProcedure;

				if (top > 0)
					command.Parameters.Add(CreateParameter("@top", top));

				if (assetTypeId > 0)
					command.Parameters.Add(CreateParameter("@assetTypeId", top));

				if (brandId.GetValueOrDefault() > 0)
					command.Parameters.Add(CreateParameter("@brandId", brandId));

				if (!timeframe.IsNull)
				{
					command.Parameters.Add(CreateParameter("@startDate", timeframe.StartDate));
					command.Parameters.Add(CreateParameter("@endDate", timeframe.EndDate));
				}

				return GetDataTable(command);
			}
		}

		/// <summary>
		/// Update the popularity rank for all assets
		/// </summary>
		public void UpdateAllAssetsPopularityRank()
		{
			UpdateAssetPopularityRank(0);
		}

		/// <summary>
		/// Update the popularity rank for the asset with the specified ID
		/// </summary>
		public void UpdateAssetPopularityRank(int assetId)
		{
			string sql = "UPDATE v_AssetPopularityRank SET PopularityRank = ActualPopularityRank";

			if (assetId > 0)
				sql += string.Format(" WHERE (AssetId = {0})", assetId);

			ExecuteSql(sql);
		}
	}
}