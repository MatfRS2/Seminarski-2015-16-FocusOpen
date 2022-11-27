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
using System.Xml.Serialization;

namespace Daydream.Data
{
	[Serializable]
	public abstract class AbstractFinder : IFinder
	{
		#region Constructor

		protected AbstractFinder()
		{
			SortExpressions = new SortExpressionList();
			MaxRecords = 0;
		}

		#endregion

		#region Abstract Properties

		protected abstract void SetSearchCriteria(ref SearchBuilder sb);
		public abstract string Table { get; }

		#endregion

		#region Properties

		[XmlIgnore]
		public SortExpressionList SortExpressions { get; set; }

		public int MaxRecords { get; set; }

		#endregion

		#region IFinder Implementation

		public virtual IDbCommand FindCommand
		{
			get
			{
				return GetSearchBuilder().GetFullCommand();
			}
		}

		public virtual IDbCommand CountCommand
		{
			get
			{
				return GetSearchBuilder().GetCountCommand();
			}
		}

		public virtual String FindQuery
		{
			get
			{
				return GetSearchBuilder().GetFullQuery();
			}
		}

		public virtual String CountQuery
		{
			get
			{
				return GetSearchBuilder().GetCountQuery();
			}
		}

		#endregion

		#region Private Methods

		private SearchBuilder GetSearchBuilder()
		{
			SearchBuilder sb = new SearchBuilder {MaxRecords = MaxRecords, Table = Table};

			SetSearchCriteria(ref sb);

			foreach (ISortExpression sortExpression in SortExpressions)
				sb.SortExpressions.Add(sortExpression.ToString());

			return sb;
		}

		#endregion
	}
}