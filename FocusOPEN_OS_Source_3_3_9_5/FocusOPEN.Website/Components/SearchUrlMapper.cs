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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	public static class SearchUrlMapper
	{
		#region Private Variables

		private static readonly Dictionary<CompareType, String> m_CompareTypeDictionary = new Dictionary<CompareType, string>
		                                                                         	{
		                                                                         		{CompareType.Exact, "eq"},
		                                                                         		{CompareType.LessOrEqualThan, "leq"},
		                                                                         		{CompareType.LessThan, "le"},
		                                                                         		{CompareType.MoreOrEqualThan, "meq"},
		                                                                         		{CompareType.MoreThan, "mo"},
		                                                                         	};

		private static readonly Dictionary<Orientation, String> m_OrientationDictionary = new Dictionary<Orientation, string>
		                                                                         	{
		                                                                         		{Orientation.All, string.Empty},
		                                                                         		{Orientation.Landscape, "L"},
		                                                                         		{Orientation.Portrait, "P"},
		                                                                         		{Orientation.Square, "S"}
		                                                                         	};

		#endregion

		#region Constants

		private const string GENERAL_KEYWORD = "gk";
		private const string BRAND_ID = "bid";
		private const string ASSET_TYPE_ID = "atid";
		private const string CATEGORY_ID = "ccid";
		private const string ORIENTATION_ID = "o";
		private const string FILESIZE_COMPARETYPE = "fsct";
		private const string FILESIZE = "fs";
		private const string FROM_PRODUCTION_MONTH = "fpm";
		private const string FROM_PRODUCTION_YEAR = "fpy";
		private const string TO_PRODUCTION_MONTH = "tpm";
		private const string TO_PRODUCTION_YEAR = "tpy";
		private const string PAGE = "p";
		private const string PAGE_SIZE = "ps";

		#endregion

		/// <summary>
		/// Gets the current logged in user.
		/// </summary>
		private static User CurrentUser
		{
			get
			{
				return SessionInfo.Current.User;
			}
		}

		public static void SetupSearchFromRequest(NameValueCollection request, SavedUserAssetSearch searchInfo)
		{
			if (request[CATEGORY_ID] != null)
			{
				int categoryId = NumericUtils.ParseInt32(request[CATEGORY_ID], 0);
				Category category = CategoryCache.Instance.GetById(categoryId);

				if (CurrentUser.CanAccessBrand(category.BrandId))
					searchInfo.SelectCategory(categoryId);
			}

			if (request[GENERAL_KEYWORD] != null)
				searchInfo.AssetFinder.GeneralKeyword = HttpUtility.UrlDecode(request[GENERAL_KEYWORD]);

			for (int i = 1; i <= Settings.NumberOfMetadataFields; i++)
			{
				string key = "md" + i;

				if (request[key] != null)
				{
					var metadataIds = NumericUtils.ParseIntList(request[key], ",");

                    if(metadataIds.Count == 0) return;

				    foreach (var mId in metadataIds)
				    {
				        Metadata metadata = MetadataCache.Instance.GetById(mId);
				        if (metadata.IsNull) return;

                        if (CurrentUser.CanAccessBrand(metadata.BrandId.GetValueOrDefault()))
                        {
                            if (!searchInfo.AssetFinder.MetadataIds.ContainsKey(i)) 
                                searchInfo.AssetFinder.MetadataIds.Add(i, new List<int>());
                            
                            searchInfo.AssetFinder.MetadataIds[i].Add(mId);
                        }
				    }
				}
			}

			if (request[BRAND_ID] != null)
			{
				int brandId = NumericUtils.ParseInt32(request[BRAND_ID], 0);

				if (CurrentUser.CanAccessBrand(brandId))
                    searchInfo.AssetFinder.BrandId = brandId;
			}

			if (request[ASSET_TYPE_ID] != null)
				searchInfo.AssetFinder.AssetTypeId = NumericUtils.ParseInt32(request[ASSET_TYPE_ID], 0);

			if (request[ORIENTATION_ID] != null)
				searchInfo.AssetFinder.Orientation = GetOrientation(request[ORIENTATION_ID]);

			if (request[FILESIZE_COMPARETYPE] != null && request[FILESIZE] != null)
			{
				CompareType compareType = GetCompareType(request[FILESIZE_COMPARETYPE]);
				searchInfo.AssetFinder.AddComplexCriteria(Asset.Columns.FileSize, Decimal.Parse(request[FILESIZE]), compareType);
			}

			if (request[FROM_PRODUCTION_MONTH] != null)
				searchInfo.AssetFinder.AddComplexCriteria(Asset.Columns.ProductionMonth, request[FROM_PRODUCTION_MONTH], CompareType.MoreThan);

			if (request[FROM_PRODUCTION_YEAR] != null)
				searchInfo.AssetFinder.AddComplexCriteria(Asset.Columns.ProductionYear, request[FROM_PRODUCTION_YEAR], CompareType.MoreThan);

			if (request[TO_PRODUCTION_MONTH] != null)
				searchInfo.AssetFinder.AddComplexCriteria(Asset.Columns.ProductionMonth, request[TO_PRODUCTION_MONTH], CompareType.LessThan);

			if (request[TO_PRODUCTION_YEAR] != null)
				searchInfo.AssetFinder.AddComplexCriteria(Asset.Columns.ProductionYear, request[TO_PRODUCTION_YEAR], CompareType.LessThan);

			if (request[PAGE] != null)
				searchInfo.Page = NumericUtils.ParseInt32(request[PAGE], 0);

			if (request[PAGE_SIZE] != null)
				searchInfo.PageSize = NumericUtils.ParseInt32(request[PAGE_SIZE], 0);
		}

		public static string GetUrlForSearch(SavedUserAssetSearch searchInfo)
		{
			AssetFinder finder = searchInfo.AssetFinder;

			StringBuilder sb = new StringBuilder();
			sb.Append("~/SearchRedirector.ashx?x=1");

			AppendSearchValue(sb, CATEGORY_ID, searchInfo.CurrentCategoryId);
			AppendSearchValue(sb, GENERAL_KEYWORD, finder.GeneralKeyword.Replace("&", " "));

            foreach (var ids in finder.MetadataIds)
            {//add all selected meta value ids for each meta under its
                //own param name
                foreach (var id in ids.Value)
                {
                    AppendSearchValue(sb, "md" + ids.Key, id);
                }
            }
			
			AppendSearchValue(sb, BRAND_ID, finder.BrandId);
			AppendSearchValue(sb, ASSET_TYPE_ID, finder.AssetTypeId);
			AppendSearchValue(sb, ORIENTATION_ID, m_OrientationDictionary[finder.Orientation]);

			ComplexCriteria criteria = finder.GetSingleComplexCriteria(Asset.Columns.FileSize);
			if (criteria != null)
				sb.AppendFormat("&{0}={1}&{2}={3}", FILESIZE_COMPARETYPE, m_CompareTypeDictionary[criteria.CompareType], FILESIZE, criteria.Value);

			criteria = finder.GetSingleComplexCriteria(Asset.Columns.ProductionMonth, CompareType.MoreThan);
			if (criteria != null)
				sb.AppendFormat("&{0}={1}", FROM_PRODUCTION_MONTH, criteria.Value);

			criteria = finder.GetSingleComplexCriteria(Asset.Columns.ProductionYear, CompareType.MoreThan);
			if (criteria != null)
				sb.AppendFormat("&{0}={1}", FROM_PRODUCTION_YEAR, criteria.Value);

			criteria = finder.GetSingleComplexCriteria(Asset.Columns.ProductionMonth, CompareType.LessThan);
			if (criteria != null)
				sb.AppendFormat("&{0}={1}", TO_PRODUCTION_MONTH, criteria.Value);

			criteria = finder.GetSingleComplexCriteria(Asset.Columns.ProductionYear, CompareType.LessThan);
			if (criteria != null)
				sb.AppendFormat("&{0}={1}", TO_PRODUCTION_YEAR, criteria.Value);

			sb.AppendFormat("&{0}={1}", PAGE, searchInfo.Page);
			sb.AppendFormat("&{0}={1}", PAGE_SIZE, searchInfo.PageSize);

			return sb.ToString();
		}

		private static void AppendSearchValue(StringBuilder sb, string key, string val)
		{
			if (!StringUtils.IsBlank(val))
				sb.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(val));
		}

		private static CompareType GetCompareType(string compareType)
		{
			return (from kvp in m_CompareTypeDictionary
			        where kvp.Value == compareType
			        select kvp.Key).FirstOrDefault();
		}

		private static Orientation GetOrientation(string orientation)
		{
			return (from kvp in m_OrientationDictionary
			        where kvp.Value == orientation
			        select kvp.Key).FirstOrDefault();
		}

		private static void AppendSearchValue(StringBuilder sb, string key, int id)
		{
			if (id > 0)
				sb.AppendFormat("&{0}={1}", key, id);
		}
	}
}
