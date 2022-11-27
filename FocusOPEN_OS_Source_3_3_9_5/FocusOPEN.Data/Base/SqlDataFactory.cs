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
using System.Data.Common;
using System.Data.SqlClient;

namespace Daydream.Data
{
	public class SqlDataFactory : AbstractDataFactory
	{
		#region Singleton Implementation

		private SqlDataFactory()
		{
		}

		private static readonly IDataFactory m_instance = new SqlDataFactory();

		public static IDataFactory Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

		public override IDbConnection CreateConnection(String ConnectionString)
		{
			return new SqlConnection(ConnectionString);
		}

		public override IDbCommand CreateCommand()
		{
			return new SqlCommand();
		}

		public override IDbCommand CreateCommand(String CommandText)
		{
			return new SqlCommand(CommandText);
		}

		public override IDbDataParameter CreateParameter(string paramName, object paramValue)
		{
			return new SqlParameter(paramName, ProcessParameter(paramValue));
		}

		public override DbDataAdapter CreateAdapter()
		{
			return new SqlDataAdapter();
		}

		public override DbDataAdapter CreateAdapter(string CommandText)
		{
			return new SqlDataAdapter(new SqlCommand(CommandText));
		}

		public override DbDataAdapter CreateAdapter(IDbCommand Command)
		{
			return new SqlDataAdapter((SqlCommand) Command);
		}
	}
}