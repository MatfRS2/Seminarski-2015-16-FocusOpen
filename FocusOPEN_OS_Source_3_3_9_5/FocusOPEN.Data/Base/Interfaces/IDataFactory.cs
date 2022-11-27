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
	public interface IDataFactory
	{
		// Connections
		IDbConnection CreateConnection (string ConnectionString);
		
		// Commands
		IDbCommand CreateCommand ();
		IDbCommand CreateCommand (String CommandText);

		// Adapters
		DbDataAdapter CreateAdapter ();
		DbDataAdapter CreateAdapter (IDbCommand Command);
		DbDataAdapter CreateAdapter (String CommandText);
	
		// Parameters 
		IDbDataParameter CreateParameter (string paramName, object paramValue);
	}
}
