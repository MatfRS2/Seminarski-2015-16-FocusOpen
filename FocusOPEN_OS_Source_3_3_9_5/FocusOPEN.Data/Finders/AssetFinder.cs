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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public partial class AssetFinder
	{
		#region Private variables

		private readonly List<ComplexCriteria> m_ComplexCriteria    = new List<ComplexCriteria>();
        private readonly Dictionary<int, List<int>> m_MetadataIds   = new Dictionary<int, List<int>>();

		private static readonly JoinableList m_StandardFields = new JoinableList();

		#endregion

		#region Constructor

		static AssetFinder()
		{
			// Default text search options
			FullTextSearchedEnabled = true;
			FileIndexingEnabled = false;
			FileContentSearchingEnabled = false;

			// Table columns
			m_StandardFields.Add(Asset.Columns.CopyrightOwner);
			m_StandardFields.Add(Asset.Columns.Description);
			m_StandardFields.Add(Asset.Columns.Keywords);
			m_StandardFields.Add(Asset.Columns.Filename);
			m_StandardFields.Add(Asset.Columns.Originator);
			m_StandardFields.Add(Asset.Columns.ProjectCode);
			m_StandardFields.Add(Asset.Columns.Title);
			m_StandardFields.Add(Asset.Columns.FileHash);
			m_StandardFields.Add(Asset.Columns.AssetCategories);
			m_StandardFields.Add(Asset.Columns.AssetMetadataVals);
			m_StandardFields.Add(Asset.Columns.MetadataSearchVals);

			// View Columns
			m_StandardFields.Add("AssetTypeName");
			m_StandardFields.Add("UploadedByUserName");
			m_StandardFields.Add("BrandName");
		}

		public AssetFinder()
		{
			BrandIdList = new List<int>();
			CategoryIdList = new List<int>();
			Orientation = Orientation.All;
			GeneralKeyword = string.Empty;
			IsCompletelyPublished = false;
			IsBeforePublicationDate = false;
			IsPublished = null;
			IsExpired = null;
			ExpiryDateRange = DateRange.Empty;
			AuditAssetHistoryFinder = null;

			// TODO: Should this be custom?
			// Otherwise, SortExpressions are not evaluated
			OrderBy = OrderBy.Relevance;
		}

		#endregion

		#region Properties

		public Dictionary<int, List<int>> MetadataIds
		{
			get
			{
				return m_MetadataIds;
			}
		}

        public List<int> GetMetadataIds(int group)
		{
			if (MetadataIds.ContainsKey(group))
				return MetadataIds[group];

			return null;
		}

		public static bool FullTextSearchedEnabled { get; set; }

		public static bool FileIndexingEnabled { get; set; }
		
		public static bool FileContentSearchingEnabled { get; set; }

		public AuditAssetHistoryFinder AuditAssetHistoryFinder { get; set; }

		public Orientation Orientation { get; set; }

		public string GeneralKeyword { get; set; }

		public bool IsBeforePublicationDate { get; set; }

		public bool? IsExpired { get; set; }

		public DateRange ExpiryDateRange { get; set; }

		public bool IsCompletelyPublished { get; set; }

		public bool? IsPublished { get; set; }

		public List<int> CategoryIdList { get; private set; }

		public List<int> BrandIdList { get; private set; }

        public int FromProductionDay { get; set; }

        public int FromProductionMonth { get; set; }

		public int FromProductionYear { get; set; }

        public int ToProductionDay { get; set; }

        public int ToProductionMonth { get; set; }

		public int ToProductionYear { get; set; }

		public bool IncludeUnpublishedExpiredAssets { get; set; }

		public int IncludeUnpublishedExpiredAssets_BrandId { get; set; }
		
		public int IncludeUnpublishedExpiredAssets_UserId { get; set; }

		public OrderBy OrderBy { get; set; }

		#endregion

		#region ComplexCriteria Public Methods

		public void AddComplexCriteria(object field, object value, CompareType compareType)
		{
			ComplexCriteria cc = new ComplexCriteria(field.ToString(), value, compareType);
			m_ComplexCriteria.Add(cc);
		}

		public ComplexCriteria GetSingleComplexCriteria(object field)
		{
			List<ComplexCriteria> list = GetComplexCriteria(field);
			return (list.Count > 0) ? list[0] : null;
		}

		public ComplexCriteria GetSingleComplexCriteria(object field, CompareType compareType)
		{
			List<ComplexCriteria> list = GetComplexCriteria(field, compareType);
			return (list.Count > 0) ? list[0] : null;
		}

		private List<ComplexCriteria> GetComplexCriteria(object field)
		{
			return m_ComplexCriteria.Where(cc => cc.Field == field.ToString()).ToList();
		}

		private List<ComplexCriteria> GetComplexCriteria(object field, CompareType compareType)
		{
			return m_ComplexCriteria.Where(cc => cc.Field == field.ToString() && cc.CompareType == compareType).ToList();
		}

		#endregion

		private void SetCustomSearchCriteria(ref SearchBuilder sb)
		{
			sb.Table = "[v_Asset]";

			if (AuditAssetHistoryFinder != null)
			{
				AuditAssetHistoryFinder.OnlyDistinctAssetIds = true;
				sb.Criteria.Add(sb.TableAlias + ".AssetId IN (" + AuditAssetHistoryFinder.FindQuery + ")");
			}

			if (Orientation != Orientation.All)
			{
				string sql = string.Empty;

				switch (Orientation)
				{
					case (Orientation.Portrait):
						sql = string.Format("[Height] > [Width]");
						break;

					case (Orientation.Landscape):
						sql = string.Format("[Height] < [Width]");
						break;

					case (Orientation.Square):
						sql = string.Format("[Height] = [Width]");
						break;
				}

				if (sql != string.Empty)
					sb.Criteria.Add(string.Format("({0})", sql));
			}

			if (GeneralKeyword != string.Empty)
			{
				if (FullTextSearchedEnabled)
				{
					UserQueryParser uq = new UserQueryParser();
					bool isValid = uq.ParseTokens(GeneralKeyword);

					if (isValid)
					{
						string query = uq.GetSqlQuery();
						SetFullTextSearchCriteria(sb, query);
					}
					else
					{
						string error = string.Format("Error parsing user query: \"{0}\" - {1}", GeneralKeyword, uq.Error);
						Debug.WriteLine(error);
					}
				}
				else
				{
					JoinableList jList = GetStandardSectorSearchSql();
					sb.Criteria.Add(string.Format("({0})", jList));
				}
			}

			if (IsBeforePublicationDate)
			{
				sb.Criteria.Add(string.Format("({0} > getdate())", Asset.Columns.PublishDate));
			}

			if (IsExpired.HasValue)
			{
				sb.Criteria.Add("dbo.IsExpired(" + sb.TableAlias + ".ExpiryDate) = @isExpired");
				sb.AddDataParameter("@isExpired", SqlUtils.BitValue(IsExpired.GetValueOrDefault()));
			}

			if (!ExpiryDateRange.IsNull)
			{
				const string dateFormat = "dd MMMM yyyy HH:mm:ss";

				if (ExpiryDateRange.StartDate.HasValue && ExpiryDateRange.EndDate.HasValue)
				{
					sb.Criteria.Add(string.Format("({0} BETWEEN '{1}' AND '{2}')", Asset.Columns.ExpiryDate, ExpiryDateRange.StartDate.Value.ToString(dateFormat), ExpiryDateRange.EndDate.Value.ToString(dateFormat)));
				}
				else
				{
					if (ExpiryDateRange.StartDate.HasValue)
						sb.Criteria.Add(string.Format("({0} >= '{1}')", Asset.Columns.ExpiryDate, ExpiryDateRange.StartDate.Value.ToString(dateFormat)));

					if (ExpiryDateRange.EndDate.HasValue)
						sb.Criteria.Add(string.Format("({0} <= '{1}')", Asset.Columns.ExpiryDate, ExpiryDateRange.EndDate.Value.ToString(dateFormat)));
				}
			}

			foreach (ComplexCriteria cc in m_ComplexCriteria)
			{
				string operand;

				switch (cc.CompareType)
				{
					case (CompareType.LessThan):
						operand = "<";
						break;

					case (CompareType.MoreThan):
						operand = ">";
						break;

					default:
						operand = "=";
						break;
				}

				sb.Criteria.Add(string.Format("({0} {1} {2})", cc.Field, operand, cc.Value));
			}

			if (IsCompletelyPublished)
			{
				// Entire clause
				JoinableList jList1 = new JoinableList(" OR ");
				
				// Currently published
				JoinableList jList2 = new JoinableList(" AND ");
				jList2.Add(string.Format("{0} = {1}", Asset.Columns.AssetPublishStatusId, Convert.ToInt32(AssetPublishStatus.Published)));
				jList2.Add(string.Format("{0} < getdate()", Asset.Columns.PublishDate));
				jList2.Add(string.Format("{0} > getdate()", Asset.Columns.ExpiryDate));

				// Add to entire clause
				jList1.Add(jList2);

				// If unpublished and expired assets need to be displayed too, we need to expand this criteria as follows:
				// 1. All admin users should see their own assets
				// 2. Brand admins should see assets in their primary brand
				// 3. Super admins should see all assets
				if (IncludeUnpublishedExpiredAssets)
				{
					JoinableList jList3 = new JoinableList(" OR ");

					if (IncludeUnpublishedExpiredAssets_UserId > 0)
						jList3.Add(string.Format("({0}={1})", Asset.Columns.UploadedByUserId, IncludeUnpublishedExpiredAssets_UserId));

					if (IncludeUnpublishedExpiredAssets_BrandId > 0)
						jList3.Add(string.Format("({0}={1})", Asset.Columns.BrandId, IncludeUnpublishedExpiredAssets_BrandId));

					if (jList3.Count > 0)
						jList1.Add(jList3);
				}

				string criteria = jList1.ToString();

				if (!StringUtils.IsBlank(criteria))
					sb.Criteria.Add(string.Format("({0})", criteria));
			}

			if (IsPublished.HasValue)
			{
				string op = (IsPublished.Value) ? " = " : " <> ";
				sb.Criteria.Add(string.Concat(Asset.Columns.AssetPublishStatusId, op, Convert.ToInt32(AssetPublishStatus.Published)));
			}

			if (BrandIdList.Count > 0)
			{
				JoinableList jList = new JoinableList(BrandIdList);
				sb.Criteria.Add(string.Format("BrandId IN ({0})", jList));
			}

			// Production date filter
			SetProductionMonthDayCriteria(sb);

			// Metadata filters
			AddMetadataCriteria(sb);

			// Category filters
            AddManyToManyCriteria(sb, "AssetCategory", "CategoryId", CategoryIdList);
			
			// Setup results ordering
			AddOrderByClause();

			Debug.WriteLine(string.Format("AssetFinder: {0}", sb.GetFullQuery()));
		}

		private void SetProductionMonthDayCriteria(SearchBuilder sb)
		{
            if (FromProductionDay + FromProductionMonth + FromProductionYear +
                ToProductionDay + ToProductionMonth + ToProductionYear <= 0)
				return;

			int fromYear = (FromProductionYear > 0) ? FromProductionYear : 1753;
            int fromMonth = (FromProductionMonth > 0) ? FromProductionMonth : 1;
            int fromDay = (FromProductionDay > 0) ? FromProductionDay : 1;
            // check for Feb 30 and etc. invalid dates
            fromDay = Math.Min(fromDay, DateTime.DaysInMonth(fromYear, fromMonth));
            DateTime fromDate = new DateTime(fromYear, fromMonth, fromDay, 0, 0, 0);

			int toYear = (ToProductionYear > 0) ? ToProductionYear : 2079;
			int toMonth = (ToProductionMonth > 0) ? ToProductionMonth : (toYear == 2079) ? 6 : 12;
            int toDay = (ToProductionDay > 0) ? ToProductionDay : DateTime.DaysInMonth(toYear, toMonth);
            // check for Feb 30 and etc. invalid dates
            toDay = Math.Min(toDay, DateTime.DaysInMonth(toYear, toMonth));
            DateTime toDate = new DateTime(toYear, toMonth, toDay, 23, 59, 59);

			sb.Criteria.Add(string.Format("(dbo.GetProductionDate(ProductionYear, ProductionMonth, ProductionDay) BETWEEN '{0}' AND '{1}')", SqlUtils.SafeDate(fromDate), SqlUtils.SafeDate(toDate)));
		}

		private void SetFullTextSearchCriteria(SearchBuilder sb, string parsedQuery)
		{
			//---------------------------------------------------------------------
			// Fields to included in select statement
			//---------------------------------------------------------------------
			sb.Fields.Add("DISTINCT [BASE].*");
			sb.Fields.Add("(ISNULL([AMD].[Rank], 0)*100) AS AssetMetadataRank");

			if (FileContentSearchingEnabled)
			{
				//---------------------------------------------------------------------
				// Fields
				//---------------------------------------------------------------------
				sb.Fields.Add("ISNULL([AFC].[Rank], 0) AS FileContentRank");
				sb.Fields.Add("ISNULL([AFC].[Rank], 0) + (ISNULL([AMD].[Rank], 0)*100) AS CombinedRank");

				//---------------------------------------------------------------------
				// Asset File Join
				//---------------------------------------------------------------------
				sb.Joins.Add("LEFT OUTER JOIN [AssetFile] AF ON AF.AssetId = BASE.AssetId");

				//---------------------------------------------------------------------
				// File Rank Join
				//---------------------------------------------------------------------
				StringBuilder sb1 = new StringBuilder();
				sb1.Append(" LEFT OUTER JOIN CONTAINSTABLE");
				sb1.Append("(");
				sb1.Append(" [AssetFile],");
				sb1.Append(" ([FileName], [FileContent]),");
				sb1.AppendFormat("'{0}'", parsedQuery);
				sb1.Append(")");
				sb1.Append(" AFC ON [AFC].[Key] = [AF].[AssetFileId]");
				sb.Joins.Add(sb1.ToString());
			}
			else
			{
				// We're not using file indexing, so the file content rank
				// will always be 0, and the combined rank should only
				// take the metadata into account.

				sb.Fields.Add("0 AS FileContentRank");
				sb.Fields.Add("(ISNULL([AMD].[Rank], 0)*100) AS CombinedRank");				
			}

			// Set up the list to hold the fields to be searched
			JoinableList jFieldList = new JoinableList(new TextExtractor("[", "]"));

			// This is a hack!  We don't want to add all the fields into the query
			// if our search terms contain a negative term.  This is because SQL Server
			// will still return all rows unless *every single row* doesn't contain the
			// term to be excluded, which isn't the behaviour we want, so we'll just
			// search the SearchableData field instead.
			// http://stackoverflow.com/questions/1296181/using-not-keyword-with-full-text-search-and-multiple-columns
			if (!parsedQuery.Contains("AND NOT"))
				jFieldList.AddRange(m_StandardFields);

			// Always search the SearchableData field, as this is our concatenated
			// field and includes all of the keywords that would be relevant.
			if (!jFieldList.Contains("SearchableData"))
				jFieldList.Add("SearchableData");

			//---------------------------------------------------------------------
			// Metadata Rank
			//---------------------------------------------------------------------
			StringBuilder sb2 = new StringBuilder();
			sb2.Append(" LEFT OUTER JOIN CONTAINSTABLE");
			sb2.Append("(");
			sb2.AppendFormat("  {0} ,", sb.Table);
			sb2.AppendFormat(" ({0}),", jFieldList);
			sb2.AppendFormat(" '{0}' ", parsedQuery);
			sb2.Append(")");
			sb2.AppendFormat(" AMD ON [AMD].[Key] = [{0}].[{1}]", sb.TableAlias, Asset.Columns.AssetId);
			sb.Joins.Add(sb2.ToString());

			//---------------------------------------------------------------------
			// Filters
			//---------------------------------------------------------------------
			JoinableList jList = new JoinableList(" OR ");

			if (FileContentSearchingEnabled)
			{
				// File indexing is enabled, so we can use the rank from searching
				// the file contents to determine what is returned.  Restrict our
				// result set to assets where there is a match somewhere, either
				// in the metadata or the asset file.  We do this by adding both
				// ranks, and filtering the results to assets where this value
				// is greater than 0.

				jList.Add("(ISNULL(AFC.[Rank], 0) + ISNULL(AMD.[Rank], 0) > 0)");
			}
			else
			{
				// Otherwise, file content is not returned, so we can't use any
				// content ranking from there.  Instead, we'll just use the rank
				// from the full-text search on the metadata.

				jList.Add("(ISNULL(AMD.[Rank], 0) > 0)");
			}

			// Include the asset matching the asset id, if a number was entered
			if (NumericUtils.IsInt32(GeneralKeyword))
				jList.Add(string.Format("{0}.{1} = {2}", sb.TableAlias, Asset.Columns.AssetId, GeneralKeyword));

			sb.Criteria.Add("(" + jList + ")");

			//---------------------------------------------------------------------
			// Sort Expressions
			// Here, we're forcing the rank column to be the first sort column
			// so that the most relevant assets appear first in the list
			//---------------------------------------------------------------------
			if (OrderBy == OrderBy.Relevance)
			{
				IList<ISortExpression> sorts = new List<ISortExpression>();

				foreach (ISortExpression sortExpression in sb.SortExpressions)
					sorts.Add(sortExpression);

				sb.SortExpressions.Clear();
				sb.SortExpressions.Add(new DescendingSort("CombinedRank"));

				foreach (ISortExpression sortExpression in sorts)
					sb.SortExpressions.Add(sortExpression);
			}
		}

		#region Private Stuff

		private JoinableList GetStandardSectorSearchSql()
		{
			JoinableList jList = new JoinableList(" OR ");

			string[] keywords = GeneralKeyword.ToLower().Split(' ');

			foreach (string field in m_StandardFields)
			{
				JoinableList jList2 = new JoinableList(" AND ");

				foreach (string keyword in keywords)
				{
					if (!UserQueryParser.NoiseWords.Contains(keyword))
					{
						string text = keyword.Replace("'", "''");

						if (text.StartsWith("*"))
							text = text.Substring(1);

						if (text.EndsWith("*"))
							text = text.Substring(0, text.Length - 1);

						jList2.Add(string.Format("({0} LIKE '%{1}%')", field, text));
					}
				}

				jList.Add(jList2);
			}

			return jList;
		}

		private void AddOrderByClause()
		{
			switch (OrderBy)
			{
				case OrderBy.Relevance:

					// Relevance ordering is done within the full text search
					// However, for empty search we also need to order by date

					if (StringUtils.IsBlank(GeneralKeyword))
					{
						SortExpressions.RemoveAll(delegate(ISortExpression sortExpression)
						{
							string fieldName = sortExpression.FieldName.ToLower();

							return (
										fieldName == Asset.Columns.PublishDate.ToString().ToLower() ||
										fieldName == Asset.Columns.AssetId.ToString().ToLower()
									);
						});

						SortExpressions.Add(new DescendingSort(Asset.Columns.PublishDate));
						SortExpressions.Add(new DescendingSort(Asset.Columns.AssetId));
					}

					break;

				case OrderBy.Popularity:

					// Order assets by download count.  If the download count is the same
					// favour newer assets, on the basis that they have achieved the same
					// download count in a shorter space of time.  This system breaks if
					// assets are published and unpublished, as the download count does not
					// reset when an asset is unpublished, so it will not be accurate.

					SortExpressions.Clear();
					SortExpressions.Add(new DescendingSort(Asset.Columns.DownloadCount));
					SortExpressions.Add(new DescendingSort(Asset.Columns.UploadDate));

					break;

				case OrderBy.Date:

					// Order by assets by date, with newer assets first

					SortExpressions.Clear();
					SortExpressions.Add(new DescendingSort(Asset.Columns.PublishDate));
					SortExpressions.Add(new DescendingSort(Asset.Columns.AssetId));

					break;
			}
		}

		private void AddMetadataCriteria(SearchBuilder sb)
		{
//            List<int> idList1 = new List<int>();
//            List<int> idList2 = new List<int>();
//
//            for (int i = 1; i <= Settings.NumberOfMetadataFields; i++)
//            {
//                int id = 0;//GetMetadataId(i);
//
//                if (id > 0)
//                    idList1.Add(id);
//            }

			var idList1 = new List<int>();
			var idList2 = new List<int>();

            var ids = MetadataIds
                            .SelectMany(m=>m.Value)
                            .Where(m => m>0)
                            .Distinct()
                            .ToList();

            idList1.AddRange(ids);

			idList2.AddRange(idList1);

			foreach (int id in idList1)
			{
				Metadata metadata = MetadataCache.Instance.GetById(id);

				if (metadata.IsNull || metadata.MetadataId == 0)
					continue;

				idList2.AddRange(metadata.GetIdList());
			}

			AddManyToManyCriteria(sb, "AssetMetadata", "MetadataId", idList2);
		}

		private static void AddManyToManyCriteria(SearchBuilder sb, string table, string field, IList list)
		{
			if (list.Count <= 0)
				return;

			JoinableList jList = new JoinableList(list);
			string sql = string.Format("({0}.AssetId IN (SELECT AssetId FROM {1} RT WHERE RT.{2} IN ({3})))", sb.TableAlias, table, field, jList);
			sb.Criteria.Add(sql);
		}

		#endregion
	}
}