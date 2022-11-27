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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using log4net;

namespace FocusOPEN.APS
{
	public class JobManager : Singleton<JobManager>
	{
		#region Properties

		/// <summary>
		/// Specifies number of jobs that are currently being processed
		/// </summary>
		internal int NumberOfJobsInProgress { get; set;}

		/// <summary>
		/// Specifies maximum number of jobs that can be concurrently processed
		/// </summary>
		internal int MaxNumberOfConcurrentJobs { get; set; }

		/// <summary>
		/// Specifies whether generated files should be deleted after the callback is completed
		/// </summary>
		internal bool DeleteGeneratedFilesAfterCallback { get; set; }

		#endregion

		#region Fields

		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly Timer m_Timer = new Timer();

		#endregion

		#region Constructor

		public JobManager()
		{
			MaxNumberOfConcurrentJobs = Int32.Parse(ConfigurationManager.AppSettings.GetValue("MaxNumberOfConcurrentJobs"));
			DeleteGeneratedFilesAfterCallback = Boolean.Parse(ConfigurationManager.AppSettings.GetValue("DeleteGeneratedFilesAfterCallback"));
			NumberOfJobsInProgress = 0;

			m_Timer.Elapsed += TimerElapsed;
			m_Timer.AutoReset = true;
			m_Timer.Interval = 5*1000; // 5 seconds
		}

		#endregion

		public void StartMonitoring()
		{
			m_Timer.Start();
			m_Logger.DebugFormat("Started monitoring for jobs.  Timer interval: {0}ms", m_Timer.Interval);
		}

		public void StopMonitoring()
		{
			m_Timer.Stop();
			Console.WriteLine();
			m_Logger.Debug("Stopped monitoring for jobs.");
		}

		/// <summary>
		/// Adds a job to the processing queue
		/// </summary>
		/// <param name="job">The job to be added</param>
		/// <returns>Job ID</returns>
		public long AddJobToQueue(Job job)
		{
			// This method is not static as we want to
			// ensure that this class gets instantiated
			// so that the timer kicks off and processes jobs

			if (!JobIsValid(job))
				return 0;

			using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
			{
				m_Logger.DebugFormat("Received job to add to queue.  Asset Id: {0}", job.AssetId);

				APSQueuedJob qj = new APSQueuedJob
				               	{
				               		AssetId = job.AssetId,
				               		InputPath = job.InputPath,
									WatermarkPath = job.WatermarkPath ?? string.Empty,
				               		PluginName = job.PluginName ?? string.Empty,
				               		CreateThumbnail = job.CreateThumbnail,
				               		CreatePreview = job.CreatePreview,
									OverrideWidth = job.OverrideWidth,
									OverrideHeight = job.OverrideHeight,
				               		DateAdded = DateTime.Now,
				               		DateProcessed = null,
				               		Status = Status.NotStarted,
				               		Message = "Added",
				               		CallbackUrl = job.CallbackUrl ?? string.Empty,
									AdditionalData = job.AdditionalData
				               	};

				db.AddToAPSQueuedJob(qj);
				db.SaveChanges();

				string message = string.Format("Job for asset with id '{0}' added to queue with Id '{1}'", qj.AssetId, qj.QueuedJobId);
				AddLogEntry(qj.QueuedJobId, message, LogEntryTarget.All);

				return qj.QueuedJobId;
			}
		}

		private static bool JobIsValid(Job job)
		{
			if (string.IsNullOrEmpty(job.InputPath))
			{
				m_Logger.WarnFormat("Processing job for asset ID {0} rejected.  Input path is null or empty.", job.AssetId);
				return false;
			}

			if (!File.Exists(job.InputPath))
			{
				m_Logger.WarnFormat("Processing job for asset ID {0} rejected.  Input path does not exist.", job.AssetId);
				return false;
			}

			if (!string.IsNullOrEmpty(job.WatermarkPath) && !File.Exists(job.WatermarkPath))
			{
				m_Logger.WarnFormat("Processing job for asset ID {0} rejected.  Watermark is specified but does not exist.", job.AssetId);
				return false;
			}

			return true;
		}

		public void ProcessQueuedJobs()
		{
			Console.WriteLine("Processing queued jobs");

			if (NumberOfJobsInProgress >= MaxNumberOfConcurrentJobs)
			{
				Debug.Write(string.Format("Already processing maximum number of concurrent jobs available - in progress: {0}, max: {1}", NumberOfJobsInProgress, MaxNumberOfConcurrentJobs));
				Console.WriteLine("Already processing maximum number of concurrent jobs available");
				return;
			}

			try
			{
				using (APSEntities db = new APSEntities(DBHelper.GetConnectionString()))
				{
					Console.WriteLine("Checking queue...");

					ServiceStatus.Instance.QueueLastChecked = DateTime.Now;

					while (GetPendingJobs(db).Count() > 0)
					{
						Console.WriteLine("Found jobs");

						var jobs = GetPendingJobs(db);

						m_Logger.DebugFormat("Found {0} pending jobs needing to be processed", jobs.Count());

						APSQueuedJob job = jobs.FirstOrDefault();

						if (job == null)
							break;

						AddLogEntry(job.QueuedJobId, string.Format("Processing job: {0}", job.QueuedJobId), LogEntryTarget.All);

						try
						{
							NumberOfJobsInProgress++;

							job.Message = "Processing...";
							job.Status = Status.Processing;
							db.SaveChanges();

							m_Logger.Debug("Sending to job processor");

							JobProcessor processor = new JobProcessor
							                         	{
							                         		JobId = job.QueuedJobId,
							                         		AssetId = job.AssetId,
							                         		InputPath = job.InputPath,
							                         		WatermarkPath = job.WatermarkPath,
							                         		PluginName = job.PluginName,
							                         		CreatePreview = job.CreatePreview,
							                         		CreateThumbnail = job.CreateThumbnail,
															OverrideHeight = job.OverrideHeight,
															OverrideWidth =  job.OverrideWidth
							                         	};
							processor.Go();

							AddLogEntry(job.QueuedJobId, "Done processing job", LogEntryTarget.All);

							job.DateProcessed = DateTime.Now;

							if (processor.HasErrors)
							{
								job.Message = "Completed with errors. Check log for details";
								job.Status = Status.CompletedWithErrors;
							}
							else
							{
								job.Message = "Processing completed successfully";
								job.Status = Status.CompletedSuccessfully;
							}

							PerformCallback(job, processor);

							if (DeleteGeneratedFilesAfterCallback)
							{
								DeleteGeneratedFiles(processor);
							}
							else
							{
								m_Logger.Debug("Generated files not deleted as DeleteGeneratedFilesAfterCallback setting is false");
							}
						}
						catch (Exception ex)
						{
							AddLogEntry(job.QueuedJobId, "Processing completed with errors: " + ex.Message, LogEntryTarget.Database);

							m_Logger.Error("Error processing job: " + ex.Message, ex);

							job.Message = "Processing completed with errors: " + ex.Message;
							job.Status = Status.CompletedWithErrors;
						}
						finally
						{
							NumberOfJobsInProgress--;
							ServiceStatus.Instance.NumberOfJobsProcessed++;
						}

						db.SaveChanges();

						Console.WriteLine("Finished processing job: {0}", job.QueuedJobId);
					}

					Console.WriteLine("No more pending jobs to be processed");
				}
			}
			catch (Exception ex)
			{
				m_Logger.Error(string.Format("ProcessQueuedJobs: Error processing queued jobs: {0}", ex.Message), ex);
			}

			Console.WriteLine("Number of other jobs currently in progress: {0}", NumberOfJobsInProgress);

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("...");
			Console.WriteLine();
			Console.WriteLine();
		}

		private static void DeleteGeneratedFiles(JobProcessor processor)
		{
			m_Logger.Debug("Deleting generated files...");
			DeleteGeneratedFile(processor.PreviewPath, "Preview");
			DeleteGeneratedFile(processor.ThumbnailPath, "Thumbnail");
			m_Logger.Debug("Done deleting generated files");
		}

		private static void DeleteGeneratedFile(string path, string description)
		{
			if (File.Exists(path))
			{
				try
				{
					File.Delete(path);
					m_Logger.InfoFormat("Deleted generated {0} file: {1}", description, path);
				}
				catch (Exception ex)
				{
					m_Logger.Warn(string.Format("Error deleting generated {0} file: {1}.  Error: {2}", description, path, ex.Message), ex);
				}
			}
			else
			{
				m_Logger.DebugFormat("{0} file not deleted; it does not exist. Path: {1}", description, path);
			}
		}

		private static void PerformCallback(APSQueuedJob job, JobProcessor processor)
		{
			m_Logger.Debug("Called PerformCallback()");

			if (String.IsNullOrEmpty(job.CallbackUrl))
			{
				AddLogEntry(job.QueuedJobId, "No callback URL specified, data not posted", LogEntryTarget.All);
			}
			else
			{
				string parsedMetadataXml = processor.MetadataXml.ToString().Replace("&", string.Empty);

				HttpPoster poster = new HttpPoster {Url = job.CallbackUrl};
				poster.AddParameter("AssetId", processor.AssetId);
				poster.AddParameter("InputPath", processor.InputPath);
				poster.AddParameter("JobId", job.QueuedJobId);
				poster.AddParameter("MetadataXml", parsedMetadataXml);
				poster.AddParameter("AdditionalData", job.AdditionalData);
				poster.AddParameter("PluginRequested", processor.PluginUsed);
				poster.AddParameter("PluginUsed", processor.PluginUsed);
				poster.AddParameter("PreviewPath", processor.PreviewPath);
				poster.AddParameter("ThumbnailPath", processor.ThumbnailPath);
				poster.AddParameter("StatusId", job.QueuedJobStatusId);
				poster.AddParameter("Status", job.Status);
				poster.AddParameter("DateAdded", job.DateAdded);
				poster.AddParameter("DateProcessed", job.DateProcessed);

				foreach (var fdi in processor.FileDataItems)
					poster.AddParameter("FDI_" + fdi.Key, fdi.Value);

				try
				{
					string response = poster.Post();
					AddLogEntry(job.QueuedJobId, string.Format("Posted data to callback url: {0}.  Response: {1}", poster.Url, response), LogEntryTarget.All);
				}
				catch (Exception ex)
				{
					AddLogEntry(job.QueuedJobId, string.Format("Error posting data to callback url: {0}.  Error: {1}", poster.Url, ex.Message), LogEntryTarget.Database);

					m_Logger.Error(string.Format("JobId: {0}, AssetId: {1} - Error posting data to callback url: {2}. Error: {3}", job.QueuedJobId, job.AssetId, poster.Url, ex.Message), ex);
				}
			}

			m_Logger.Debug("Exited PerformCallback()");
		}

		internal static IEnumerable<APSQueuedJob> GetPendingJobs(APSEntities db)
		{
			Debug.WriteLine("Called GetPendingJobs()");

			int jobStatusId = Convert.ToInt32(Status.NotStarted);

			// First get all of the queued jobs
			var list1 = (from job in db.APSQueuedJob
			             where (job.QueuedJobStatusId == jobStatusId)
			             select job).ToList();

			// Now sort them by the processing priority.  This is so that assets using plugins that are quick to process
			// such as image processing can be prioritized ahead of the others which take longer.
			var list2 = from job in list1
			            let ProcessingPriority = PluginManager.Instance.GetPluginInfo(job.PluginName, PathUtils.GetExtension(job.InputPath)).ProcessingPriority
			            orderby ProcessingPriority ascending, job.QueuedJobId ascending
			            select job;

			List<APSQueuedJob> pendingJobs = list2.ToList();

			int jobCount = pendingJobs.Count();

			m_Logger.DebugFormat("Found {0} pending jobs", jobCount);

			return pendingJobs;
		}

		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("Called TimerElapsed()");

			try
			{
				ProcessQueuedJobs();
			}
			catch (Exception ex)
			{
				m_Logger.Warn(string.Format("TimerElapsed: Error processing queued jobs.  Error: {0}", ex.Message), ex);
			}
		}

		/// <summary>
		/// Adds a log entry for the specified job and logs message to log file
		/// </summary>
		private static void AddLogEntry(long jobId, string message, LogEntryTarget target)
		{
			if (target.IsSet(LogEntryTarget.Database) && jobId > 0)
				LogManager.AddLogEntry(jobId, message);

			if (target.IsSet(LogEntryTarget.File))
				m_Logger.DebugFormat("JobId: {0} : {1}", jobId, message);
		}
	}
}