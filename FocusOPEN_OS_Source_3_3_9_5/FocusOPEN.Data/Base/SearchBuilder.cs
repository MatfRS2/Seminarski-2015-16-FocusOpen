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
using System.Data;
using System.Text;
using FocusOPEN.Shared;

namespace Daydream.Data
{
	public class SearchBuilder
	{
		#region Private Variables

		public SearchBuilder()
		{
			Table = string.Empty;
			TableAlias = "BASE";
			SortExpressions = new JoinableList();
			Criteria = new JoinableList(" AND ", new BracketingTextExtractor());
			Parameters = new Dictionary<string, IDbDataParameter>();
			Joins = new JoinableList(" ");
			Fields = new JoinableList();
			MaxRecords = 0;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// The maximum number of records to retrieve. If this property is not set,
		/// all records are retireved
		/// </summary>
		public int MaxRecords { private get; set; }

		/// <summary>
		/// The table from which to retrieve records
		/// </summary>
		public string Table { get; set; }

		/// <summary>
		/// The alias for the table in the SQL query
		/// </summary>
		public string TableAlias { get; set; }

		/// <summary>
		/// The fields to return in the query. If no fields are added 
		/// to this collection, all fields are returned
		/// </summary>
		public IList Fields { get; private set; }

		/// <summary>
		/// Collection of joins that should be performed
		/// Eg. INNER JOIN MyTable MT ON MT.ID = BASE.ID
		/// </summary>
		public IList Joins { get; private set; }

		/// <summary>
		/// Dictionary of search parameters
		/// </summary>
		public Dictionary<string, IDbDataParameter> Parameters { get; private set; }

		/// <summary>
		/// Collection of search criteria to filter the search, e.g. "RegionId = 4"
		/// </summary>
		public IList Criteria { get; private set; }

		/// <summary>
		/// Collection of sort commands, e.g. "RegionId ASC"
		/// </summary>
		public IList SortExpressions { get; private set; }

		#endregion

		public void AddDataParameter(string name, object value)
		{
			IDbDataParameter parameter = DataContext.Factory.CreateParameter(name, value);
			Parameters[name] = parameter;
		}

		public IDbCommand GetFullCommand()
		{
			return GetCommand(GetFullQuery());
		}

		public IDbCommand GetCountCommand()
		{
			return GetCommand(GetCountQuery());
		}

		private IDbCommand GetCommand(string query)
		{
			IDbCommand command = DataContext.Factory.CreateCommand(query);

			foreach (IDbDataParameter parameter in Parameters.Values)
				command.Parameters.Add(parameter);

			return command;
		}

		public string GetFullQuery()
		{
			StringBuilder sb = new StringBuilder("SELECT ");

			// Max Records
			if (MaxRecords > 0)
				sb.AppendFormat("TOP {0} ", MaxRecords);

			// Fields
			if (Fields.Count > 0)
			{
				sb.AppendFormat("{0} ", Fields);
			}
			else
			{
				sb.Append("* ");
			}

			// Tables
			sb.AppendFormat("FROM {0} ", Table);

			// Table Alias
			if (!String.IsNullOrEmpty(TableAlias))
				sb.AppendFormat("{0} ", TableAlias);

			// Joins
			if (Joins.Count > 0)
				sb.Append(Joins);

			// Criteria
			if (Criteria.Count > 0)
				sb.AppendFormat("WHERE {0} ", Criteria);

			// Sorting
			if (SortExpressions.Count > 0)
				sb.AppendFormat("ORDER BY {0} ", SortExpressions);

			return sb.ToString();
		}

		/// <summary>
		/// Returns a SQL query to get the number of records based on the supplied criteria
		/// </summary>
		public string GetCountQuery()
		{
			StringBuilder sb = new StringBuilder("SELECT COUNT (*) ");

			// Table
			sb.AppendFormat("FROM {0} ", Table);

			// Table Alias
			if (!String.IsNullOrEmpty(TableAlias))
				sb.AppendFormat("{0} ", TableAlias);

			// Joins
			if (Joins.Count > 0)
				sb.Append(Joins);

			// Criteria
			if (Criteria.Count > 0)
				sb.AppendFormat("WHERE {0} ", Criteria);

			return sb.ToString();
		}

		/// <summary>
		/// Returns a SQL query generated from the object's members
		/// </summary>
		public override string ToString()
		{
			return GetFullQuery();
		}
	}
}