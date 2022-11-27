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
using System.Diagnostics;
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public partial class AuditUserHistoryFinder
	{
		#region Private Variables

		public AuditUserHistoryFinder()
		{
			SuccessfulLogins = false;
			EndDate = null;
			StartDate = null;
			Keyword = string.Empty;
			UserIdFinder = null;
			AuditUserActionList = new List<AuditUserAction>();
		}

		#endregion

		#region Accessors

		public UserIdFinder UserIdFinder { get; set; }

		public string Keyword { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public bool SuccessfulLogins { get; set; }

		public List<AuditUserAction> AuditUserActionList { get; private set; }

		public AuditUserAction AuditUserAction
		{
			get
			{
				return EnumUtils.GetEnumFromValue<AuditUserAction>(AuditUserActionId);
			}
			set
			{
				AuditUserActionId = Convert.ToInt32(value);
			}
		}

		#endregion

		protected void SetCustomSearchCriteria (ref SearchBuilder sb)
		{
			if (UserIdFinder != null)
				sb.Criteria.Add("UserId IN (" + UserIdFinder.FindQuery + ")");

			if (!StringUtils.IsBlank(Keyword))
			{
				JoinableList jList = new JoinableList(" OR ");

				if (NumericUtils.IsInt32(Keyword))
					jList.Add(string.Format("({0}={1})", AuditUserHistory.Columns.UserId, Keyword));

				string safeSector = SqlUtils.SafeValue(Keyword);

				jList.Add(string.Format("({0} LIKE '%{1}%')", AuditUserHistory.Columns.SessionId, safeSector));
				jList.Add(string.Format("({0} LIKE '%{1}%')", AuditUserHistory.Columns.IpAddress, safeSector));
				jList.Add(string.Format("({0} LIKE '%{1}%')", AuditUserHistory.Columns.Notes, safeSector));
				jList.Add(string.Format("(Description LIKE '%{0}%')", safeSector));
				jList.Add(string.Format("(UserName LIKE '%{0}%')", safeSector));
				jList.Add(string.Format("(UserEmail LIKE '%{0}%')", safeSector));

				sb.Criteria.Add(jList.ToString());
			}

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

			if (SuccessfulLogins)
			{
				// TODO: Doing a LIKE query is not very efficient.  Need to change this... seperate login events perhaps
				// and add an AuditUserAction for Successful Login and Login Errors

				string criteria = string.Format("(([{0}]={1}) AND ([Notes] LIKE 'Login Successful.%'))", AuditUserHistory.Columns.AuditUserActionId, Convert.ToInt32(AuditUserAction.UserLogin));
				sb.Criteria.Add(criteria);
			}

			if (AuditUserActionList.Count > 0)
			{
				JoinableList jList = new JoinableList();

				foreach (AuditUserAction action in AuditUserActionList)
					jList.Add(Convert.ToInt32(action));

				sb.Criteria.Add("AuditUserActionId IN (" + jList + ")");
			}

			Debug.WriteLine(sb.GetCountQuery());
		}
	}
}

