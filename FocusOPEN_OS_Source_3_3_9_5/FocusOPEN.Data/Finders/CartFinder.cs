/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Text;
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public partial class CartFinder
	{
		#region Private variables

		private int m_OffSet = 0;

		#endregion

		#region Accessors

		public int OffSet
		{
			get
			{
				return m_OffSet;
			}
			set
			{
				m_OffSet = value;
			}
		}

		#endregion

		protected void SetCustomSearchCriteria (ref SearchBuilder sb)
		{
			if (OffSet > 0)
			{
				StringBuilder subquery = new StringBuilder();
				subquery.AppendFormat("{0} NOT IN (", Cart.Columns.CartId);
				subquery.AppendFormat("SELECT TOP {0} {1} FROM {2}", OffSet, Cart.Columns.CartId, sb.Table);

				if (sb.Criteria.Count > 0)
				{
					JoinableList jList = new JoinableList(sb.Criteria, " AND ");
					subquery.AppendFormat(" WHERE {0}", jList);
				}

				if (SortExpressions.Count > 0)
				{
					JoinableList jList = new JoinableList(SortExpressions);
					subquery.AppendFormat(" ORDER BY {0}", jList);
				}

				subquery.Append(")");

				string query = subquery.ToString();

				sb.Criteria.Add(query);
			}
		}
	}
}