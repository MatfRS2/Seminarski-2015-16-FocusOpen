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

namespace Daydream.Data
{
	public abstract class AbstractDataFactory : IDataFactory
	{
		public abstract IDbConnection CreateConnection(string ConnectionString);
		public abstract IDbCommand CreateCommand();
		public abstract IDbCommand CreateCommand(string CommandText);
		public abstract DbDataAdapter CreateAdapter();
		public abstract DbDataAdapter CreateAdapter(IDbCommand Command);
		public abstract DbDataAdapter CreateAdapter(string CommandText);
		public abstract IDbDataParameter CreateParameter(string paramName, object paramValue);

		protected static object ProcessParameter(object paramValue)
		{
			if (paramValue == null)
				return DBNull.Value;

			if (paramValue is Guid)
			{
				Guid guid = (Guid) paramValue;
				
				if (guid.Equals(Guid.Empty))
					return DBNull.Value;
				
				return guid;
			}

			return paramValue;
		}
	}
}