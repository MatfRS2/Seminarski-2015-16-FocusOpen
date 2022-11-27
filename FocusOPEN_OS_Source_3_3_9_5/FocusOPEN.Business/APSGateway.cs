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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using FocusOPEN.Business.APS;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Shared.Other;
using log4net;

namespace FocusOPEN.Business
{
	public class APSGateway
	{
		#region Singleton

		private static APSGateway m_Instance;

		private APSGateway()
		{
		}

		public static APSGateway Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = new APSGateway();

				return m_Instance;
			}
		}

		#endregion

		#region Fields

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private List<string> m_ProcesssableExtensions;

		#endregion

		#region Accessors

		/// <summary>
		/// The path to the watermark that should be applied to previews
		/// </summary>
		public string BrandsPath { get; set; }

		/// <summary>
		/// The callback URL that data should be posted to after processing
		/// </summary>
		public string OverrideCallbackUrl { get; set; }

		/// <summary>
		/// List of file extensions that can be processed
		/// This is cached after the first request.
		/// </summary>
		public List<string> ProcesssableExtensions
		{
			get
			{
				if (m_ProcesssableExtensions == null)
				{
					m_ProcesssableExtensions = new List<string>();

					try
					{
						string[] extensions = new string[0];

						ServiceHelper.Using<ProcessingServiceClient>(processingService => { extensions = processingService.GetSupportedExtensions(); });

						m_ProcesssableExtensions.AddRange(extensions);
					}
					catch (Exception ex)
					{
						m_Logger.Error("Error retrieving processable extensions from APS. Check service is available", ex);
					}
				}

				return m_ProcesssableExtensions;
			}
		}

		#endregion

		public void InvalidateProcessableExtensionsCache()
		{
			m_ProcesssableExtensions = null;
		}

		private string GetWatermarkImagePath(int brandId)
		{
			string path = Path.Combine(BrandsPath, string.Format("Brand_{0}", brandId));

			return (from file in Directory.GetFiles(path)
			        let filename = Path.GetFileName(file)
			        where filename.ToLower().StartsWith("watermark") && Regex.IsMatch(StringUtils.GetFileExtension(filename), "^jpg|gif|png$")
			        select file).FirstOrDefault();
		}

		public bool CanProcess(string extension)
		{
			return ProcesssableExtensions.Contains(extension);
		}

		public string GeneratesFileExtension(string extension)
		{
			string generatesExtension = String.Empty;

			try
			{
				ServiceHelper.Using<ProcessingServiceClient>(processingService => { generatesExtension = processingService.GetExtensionGenerated(String.Empty, extension); });
			}
			catch (Exception ex)
			{
				m_Logger.Error("Error retrieving file extension generated from APS. Check service is available", ex);
			}

			return generatesExtension;
		}

		public bool ProcessFile(Asset asset, bool notify, FileOutputs output)
		{
			AssetFileInfo info = new AssetFileInfo(asset);

			string watermark = GetWatermarkImagePath(asset.BrandId);

			Job job = new Job
			          	{
			          		AssetId = asset.AssetId.GetValueOrDefault(),
			          		InputPath = info.FilePath,
			          		WatermarkPath = (asset.WatermarkPreview && !String.IsNullOrEmpty(watermark)) ? watermark : string.Empty,
			          		CreatePreview = output.IsSet(FileOutputs.Preview),
			          		CreateThumbnail = output.IsSet(FileOutputs.Thumbnail),
			          		CallbackUrl = "~/ProcessingCallback.ashx",
							AdditionalData = string.Format("<AdditionalData><Notify>{0}</Notify></AdditionalData>", notify)
			          	};

			long jobId = SubmitJob(job);

			return (jobId > 0);
		}

		public long SubmitJob(Job job)
		{
			try
			{
				if (job.CallbackUrl.StartsWith("~/"))
				{
					var websiteUrl = (!StringUtils.IsBlank(OverrideCallbackUrl)) ? OverrideCallbackUrl : WebUtils.GetBaseUrl();
					job.CallbackUrl = websiteUrl.EnsureEndsWith("/") + job.CallbackUrl.Substring(2);
				}

				long jobId = 0;

				ServiceHelper.Using<ProcessingServiceClient>(processingService => { jobId = processingService.SubmitJob(job); });

				if (jobId <= 0)
				{
					m_Logger.DebugFormat("Error submitting asset {0} to processing service. Probably a validation error.", job.AssetId);
					return -1;
				}

				m_Logger.DebugFormat("Submitted asset {0} to processing service. Job ID: {1}", job.AssetId, jobId);
				return jobId;
			}
			catch (Exception ex)
			{
				m_Logger.Fatal("Error submitting job to APS.  Check service is available.", ex);
				return -1;
			}
		}

		public static ServiceStatus GetServiceStatus()
		{
			ServiceStatus status = null;

			try
			{
				ServiceHelper.Using<ProcessingServiceClient>(processingService => { status = processingService.GetServiceStatus(); });
			}
			catch (Exception ex)
			{
				m_Logger.Warn("Error getting APS Status: " + ex.Message, ex);
			}

			return status;
		}

		public static VersionInfo GetVersionInfo()
		{
			VersionInfo vi = null;

			try
			{
				ServiceHelper.Using<ProcessingServiceClient>(processingService => { vi = processingService.GetVersionInfo(); });
			}
			catch (Exception ex)
			{
				m_Logger.Warn("Error getting APS Version: " + ex.Message, ex);
			}

			return vi;
		}
	}

	[Flags]
	public enum FileOutputs
	{
		Thumbnail = 1,
		Preview = 2,
		All = Thumbnail | Preview
	}
}