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
using System.Diagnostics;

namespace Daydream.Data
{
	public abstract class AbstractDataMapper : IDataMapper
	{
		#region Member Variables

		protected IDataFactory m_factory = null;
		private static Int32 m_openConnectionCount = 0;
		private static String m_connectionString = null;
		private static int m_maxOpenConnections = 0;

		#endregion

		#region Abstract Methods

		protected abstract object ReadRow(IRowReader reader);
		protected abstract IEntityList CreateObjectList();
		public abstract IEntity GetFromKey(object key);

		#endregion

		#region Constructor

		protected AbstractDataMapper()
		{
			ReadDataContextProperties();
			DataContext.Changed += DataContext_Changed;
		}

		#endregion

		#region Find Methods

		public IEntity FindOne(IFinder finder)
		{
#if DEBUG
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
			IEntity entity = (IEntity)LendReader(finder.FindCommand, new SingleRowReader(this));
#if DEBUG
			sw.Stop();
			LogMessage("FindOne", finder.FindCommand, sw.ElapsedMilliseconds);
#endif
			return entity;
		}

		public IEntityList FindMany(IFinder finder)
		{
#if DEBUG
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
			IEntityList result = (IEntityList)LendReader(finder.FindCommand, new MultiRowReader(this));
#if DEBUG
			sw.Stop();
			LogMessage("FindMany", finder.FindCommand, sw.ElapsedMilliseconds);
#endif
			result.PagingInfo = new PagingInfo(result.Count, 0, result.Count);

			return result;
		}

		public IEntityList FindMany(IFinder finder, int Page, int PageSize)
		{
#if DEBUG
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
			IEntityList result = CreateObjectList();
			PagedDataSet ds = GetPagedDataSet(finder, Page, PageSize);
			result.PagingInfo = ds.PagingInfo;

			foreach (DataRow row in ds.Tables[0].Rows)
				result.Add(ReadRow(new RowReader(row)));

#if DEBUG
			sw.Stop();
			LogMessage("FindMany+1", finder.FindCommand, sw.ElapsedMilliseconds);
#endif
			return result;
		}

		#endregion

		public Int32 GetCount(IFinder finder)
		{
#if DEBUG
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
			int count = Convert.ToInt32(ExecScalar(finder.CountCommand));
#if DEBUG
			sw.Stop();
			LogMessage("GetCount", finder.CountCommand, sw.ElapsedMilliseconds);
#endif
			return count;
		}

		public void ExecuteSql(string SqlStatement)
		{
			ExecuteCommand(m_factory.CreateCommand(SqlStatement));
		}

		/// <summary>
		/// Execute the SQL command
		/// </summary>
		public void ExecuteCommand(IDbCommand command)
		{
#if DEBUG
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
			using (IDbConnection connection = OpenConnection())
			{
				try
				{
					command.Connection = connection;
					command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					throw new AbstractDataMapperException(e, command);
				}
				finally
				{
					CloseConnection(connection);
				}
			}
#if DEBUG
			sw.Stop();
			LogMessage("ExecuteCommand", command, sw.ElapsedMilliseconds);
#endif
		}

		public object ExecScalar(IDbCommand command)
		{
			object val;
#if DEBUG
			Stopwatch sw = new Stopwatch();
			sw.Start();
#endif
			using (IDbConnection connection = OpenConnection())
			{
				try
				{
					command.Connection = connection;
					val = command.ExecuteScalar();
				}
				catch (Exception e)
				{
					throw new AbstractDataMapperException(e, command);
				}
				finally
				{
					CloseConnection(connection);
				}
			}
#if DEBUG
			sw.Stop();
			LogMessage("ExecScalar", command, sw.ElapsedMilliseconds);
#endif
			return val;
		}

		protected IDbCommand CreateCommand()
		{
			return m_factory.CreateCommand();
		}

		public IDbDataParameter CreateParameter(string name, object value)
		{
			return m_factory.CreateParameter(name, value);
		}

		protected object LendReader(IDbCommand command, IReader reader)
		{
			using (IDbConnection connection = OpenConnection())
			{
				try
				{
					command.Connection = connection;
					IDataReader dataReader = command.ExecuteReader();
					object result = reader.Read(new SmartDataReader(dataReader));
					dataReader.Close();
					return result;
				}
				catch (Exception e)
				{
					throw new AbstractDataMapperException(e, command);
				}
				finally
				{
					CloseConnection(connection);
				}
			}
		}

		public PagedDataSet GetPagedDataSet(IFinder finder, Int32 Page, Int32 RecordsPerPage)
		{
			using (IDbConnection connection = OpenConnection())
			{
				try
				{
#if DEBUG
					Stopwatch sw = new Stopwatch();
					sw.Start();
#endif
					PagedDataSet ds = new PagedDataSet(finder.Table);

					using (IDbCommand command = finder.CountCommand)
					{
						command.Connection = connection;
						int numRecords = (int)command.ExecuteScalar();
						ds.PagingInfo = new PagingInfo(numRecords, Page, RecordsPerPage);
#if DEBUG
						LogMessage("GetPagedDataSet", finder.CountCommand, sw.ElapsedMilliseconds);
#endif
					}

					using (IDbCommand command = finder.FindCommand)
					{
						command.Connection = connection;
						DbDataAdapter da = m_factory.CreateAdapter(command);
						da.Fill(ds, ds.PagingInfo.StartRecord - 1, ds.PagingInfo.PageSize, finder.Table);
#if DEBUG
						LogMessage("GetPagedDataSet", finder.FindCommand, sw.ElapsedMilliseconds);
#endif
					}
#if DEBUG
					sw.Stop();
#endif
					return ds;
				}
				finally
				{
					CloseConnection(connection);
				}
			}
		}

		protected DataSet GetDataSet(IDbCommand command)
		{
			using (IDbConnection connection = OpenConnection())
			{
				try
				{
#if DEBUG
					Stopwatch sw = new Stopwatch();
					sw.Start();
#endif
					DataSet ds = new DataSet();
					using (command)
					{
						command.Connection = connection;
						IDataAdapter da = m_factory.CreateAdapter(command);
						da.Fill(ds);
					}
#if DEBUG
					sw.Stop();
					LogMessage("GetDataSet", command, sw.ElapsedMilliseconds);
#endif
					return ds;
				}
				finally
				{
					CloseConnection(connection);
				}
			}
		}

		protected DataSet GetDataSet(string SqlStatement)
		{
			using (IDbCommand command = m_factory.CreateCommand(SqlStatement))
			{
				return GetDataSet(command);
			}
		}

		protected DataTable GetDataTable(IDbCommand command)
		{
			DataSet ds = GetDataSet(command);
			return ds.Tables[0];
		}

		protected DataTable GetDataTable(string sqlStatement)
		{
			DataSet ds = GetDataSet(sqlStatement);
			return ds.Tables[0];
		}

		protected DataRow GetDataRow(IDbCommand command)
		{
			DataTable dt = GetDataTable(command);
			return (dt.Rows.Count == 0) ? null : dt.Rows[0];
		}

		protected DataRow GetDataRow(string sqlStatement)
		{
			DataTable dt = GetDataTable(sqlStatement);
			return (dt.Rows.Count == 0) ? null : dt.Rows[0];
		}

		public object ReadSingleRow(IRowReader reader)
		{
			if (reader.Read())
				return (ReadRow(reader));

			return null;
		}

		public object ReadRowSet(IRowReader reader)
		{
			IEntityList list = CreateObjectList();
			while (reader.Read())
			{
				list.Add(ReadRow(reader));
			}
			return list;
		}

		#region Private Methods

		private void DataContext_Changed(object sender, EventArgs e)
		{
			ReadDataContextProperties();
		}

		private void ReadDataContextProperties()
		{
			m_factory = DataContext.Factory;
			m_connectionString = DataContext.ConnectionString;
			m_maxOpenConnections = DataContext.MaxOpenConnections;
		}

		private IDbConnection OpenConnection()
		{
			try
			{
				if (m_openConnectionCount > m_maxOpenConnections)
				{
					throw new Exception("Open Connections is at " + m_openConnectionCount);
				}

				IDbConnection connection = m_factory.CreateConnection(m_connectionString);
				connection.Open();
				++m_openConnectionCount;
				return connection;
			}
			catch (Exception e)
			{
				Debug.WriteLine(String.Format("There are {0} connections open at the moment", m_openConnectionCount));
				Exception innerE = e.GetBaseException();
				Debug.WriteLine(String.Format("Exception message was \"{0}\"", innerE.Message));
				Debug.WriteLine(String.Format("Exception stacktrace was \"{0}\"", innerE.StackTrace));

				throw;
			}
		}

		private static void CloseConnection(IDbConnection connection)
		{
			connection.Close();
			--m_openConnectionCount;
		}

#if DEBUG
		private static void LogMessage(string commandName, IDbCommand command, long ms)
		{
			string commandText = (command == null) ? "(Not Available)" : command.CommandText;

			Debug.WriteLine(string.Format("{0} : Executed SQL: {1}.  Took {2}ms", commandName, commandText, ms));

			if (command != null)
				foreach (IDataParameter p in command.Parameters)
					Debug.WriteLine(string.Format(" - {0} : {1}", p.ParameterName, p.Value));
		}
#endif

		#endregion
	}
}