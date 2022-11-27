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
using System.Linq;

namespace FocusOPEN.APS
{
	public static class LogManager
	{
		/// <summary>
		/// Adds a new message to the log
		/// </summary>
		/// <param name="jobId">Job ID</param>
		/// <param name="message">Message</param>
		public static void AddLogEntry(long jobId, string message)
		{
			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				APSLog log = new APSLog {JobId = jobId, Message = message, Date = DateTime.Now};
				db.AddToAPSLog(log);
				db.SaveChanges();
			}
		}

		/// <summary>
		/// Gets the most recent 200 log entries
		/// </summary>
		public static List<LogEntry> GetLogEntries()
		{
			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				var entries = (from log in db.APSLog
				               orderby log.Date descending , log.LogId descending
							   select new LogEntry { JobId = log.JobId, Message = log.Message, Date = log.Date }).Take(200).ToList();

				return entries;
			}
		}

		/// <summary>
		/// Gets the most recent 200 entries for the specified job id
		/// </summary>
		public static List<LogEntry> GetLogEntriesByJobId(int jobId)
		{
			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				var entries = (from log in db.APSLog
				               where log.JobId == jobId
				               orderby log.Date descending , log.LogId descending
				               select new LogEntry { JobId = log.JobId, Message = log.Message, Date = log.Date }).Take(200).ToList();

				return entries;
			}
		}

		/// <summary>
		/// Gets the most recent 200 entries for the specified asset id
		/// </summary>
		public static List<LogEntry> GetLogEntriesByAssetId(int assetId)
		{
			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				var entries = (from log in db.APSLog
				               join qj in db.APSQueuedJob on log.JobId equals qj.QueuedJobId
				               where qj.AssetId == assetId
				               orderby log.Date descending , log.LogId descending
				               select new LogEntry {JobId = log.JobId, Message = log.Message, Date = log.Date}).Take(200).ToList();

				return entries;
			}
		}

		/// <summary>
		/// Gets the most recent 200 debug log entries
		/// </summary>
		public static List<APSDebugLog> GetDebugLogEntries()
		{
			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				var entries = (from log in db.APSDebugLog
				               where log.Level.ToLower() == "debug"
				               orderby log.Date descending , log.APSDebugLogId descending
				               select log).Take(200);

				return entries.ToList();
			}
		}
	}

	[Flags]
	internal enum LogEntryTarget
	{
		Database = 1,
		File = 2,
		All = Database | File
	}
}