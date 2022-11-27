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
using System.Data.EntityClient;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using log4net;

namespace FocusOPEN.APS
{
	public static class DBHelper
	{
		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Public Methods

		public static string GetConnectionString()
		{
			EnsureDatabaseExists();

			EntityConnectionStringBuilder ee = new EntityConnectionStringBuilder
			                                   	{
			                                   		Provider = @"System.Data.SQLite",
			                                   		Metadata = @"res://*/APSModel.csdl|res://*/APSModel.ssdl|res://*/APSModel.msl",
			                                   		ProviderConnectionString = string.Format("data source={0}", GetFilePath())
			                                   	};

			return ee.ConnectionString;
		}

		public static void EnsureDatabaseExists()
		{
			string databasePath = GetFilePath();

			if (File.Exists(databasePath) && (new FileInfo(databasePath).Length) > 0)
				return;

			WriteMessage("Database does not exist in: {0}", databasePath);
				
			SQLiteConnection.CreateFile(databasePath);

			WriteMessage("Created database file in: {0}", databasePath);

			string connString = string.Format("Data Source={0};Version=3", databasePath);

			using (SQLiteConnection connection = new SQLiteConnection(connString))
			{
				using (SQLiteCommand command = connection.CreateCommand())
				{
					command.CommandText = GetSchemaSQL();

					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();

					WriteMessage("Created database schema in: {0}", databasePath);
				}
			}
		}

		public static void DeleteDatabase()
		{
			string path = GetFilePath();

			if (File.Exists(path))
			{
				File.Delete(path);
				Console.WriteLine("Deleted database: " + path);
				return;
			}

			Console.WriteLine("Database not deleted; does not exist");
		}

		#endregion

		#region Private Methods

		private static void WriteMessage(string s1, params string[] s2)
		{
			string message = string.Format(s1, s2);
			Console.WriteLine(message);
			Console.WriteLine();

			m_Logger.Debug(message);
		}

		private static string GetFilePath()
		{
			string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			if (string.IsNullOrEmpty(assemblyPath))
				throw new SystemException("Unable to get application path");

			return Path.Combine(assemblyPath, "APS.s3db");
		}

		private static string GetSchemaSQL()
		{
			string sql;

			using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FocusOPEN.APS.Config.DBSchema.SQL") ?? Stream.Null))
				sql = sr.ReadToEnd();

			if (string.IsNullOrEmpty(sql))
				throw new SystemException("Error getting schema creation SQL");

			return sql;
		}

		#endregion
	}
}