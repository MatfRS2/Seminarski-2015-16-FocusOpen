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
using System.Linq;
using System.Runtime.Serialization;

namespace FocusOPEN.APS
{
	[DataContract]
	public class ServiceStatus : Singleton<ServiceStatus>
	{
		/// <summary>
		/// Gets the date that the service was started
		/// </summary>
		[DataMember]
		public DateTime StartDate { get; set; }

		/// <summary>
		/// Gets the number of jobs in progress
		/// </summary>
		[DataMember]
		public int NumberOfJobsInProgress { get; set; }

		/// <summary>
		/// Gets the number of pending jobs currently in the queue
		/// </summary>
		[DataMember]
		public int NumberOfJobsInQueue { get; set; }

		/// <summary>
		/// Gets the number of jobs that have been processed
		/// </summary>
		[DataMember]
		public int NumberOfJobsProcessed { get; set; }

		/// <summary>
		/// Gets the last date and time that the queue was checked
		/// </summary>
		[DataMember]
		public DateTime QueueLastChecked { get; set; }

		/// <summary>
		/// Gets the service status
		/// </summary>
		public static ServiceStatus GetServiceStatus()
		{
			// Update the number of jobs in progress count
			Instance.NumberOfJobsInProgress = JobManager.Instance.NumberOfJobsInProgress;

			// Update the number of pending jobs count
			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
				Instance.NumberOfJobsInQueue = JobManager.GetPendingJobs(db).Count();

			return Instance;
		}
	}
}