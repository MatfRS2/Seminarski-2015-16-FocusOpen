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
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public partial class AuditAssetHistoryFinder
	{
		#region Constructor

		public AuditAssetHistoryFinder()
		{
			IncludeActionsList = new List<AuditAssetAction>();
			EndDate = null;
			StartDate = null;
		}

		#endregion

		#region Accessors

		public List<AuditAssetAction> IncludeActionsList { get; private set; }

		public int BrandId { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public bool OnlyDistinctAssetIds { get; set; }

		public AuditAssetAction AuditAssetAction
		{
			get
			{
				return EnumUtils.GetEnumFromValue<AuditAssetAction>(AuditAssetActionId);
			}
			set
			{
				AuditAssetActionId = Convert.ToInt32(value);
			}
		}

		#endregion

		protected void SetCustomSearchCriteria (ref SearchBuilder sb)
		{
			if (OnlyDistinctAssetIds)
			{
				sb.TableAlias = "AAH";
				sb.Fields.Add("DISTINCT AssetId");
			}

			if (IncludeActionsList.Count > 0)
			{
				JoinableList jList = new JoinableList(" OR ");

				foreach (AuditAssetAction auditAssetAction in IncludeActionsList)
				{
					string criteria = string.Format("{0}={1}", AuditAssetHistory.Columns.AuditAssetActionId, Convert.ToInt32(auditAssetAction));
					jList.Add(criteria);
				}

				sb.Criteria.Add(jList.ToString());
			}

			if (!StringUtils.IsBlank(UserEmail))
			{
				sb.Criteria.Add("[UserEmail] = @userEmail");
				sb.AddDataParameter("@userEmail", UserEmail);
			}

			if (BrandId != 0)
				sb.Criteria.Add(string.Format("AssetBrandId={0}", BrandId));

			if (StartDate.HasValue)
			{
				string criteria = string.Format("[Date] >= '{0}'", StartDate.Value.ToString("yyyy-MM-dd"));
				sb.Criteria.Add(criteria);
			}

			if (EndDate.HasValue)
			{
				string criteria = string.Format("[Date] <= '{0}'", EndDate.Value.ToString("yyyy-MM-dd"));
				sb.Criteria.Add(criteria);
			}
		}
	}
}

