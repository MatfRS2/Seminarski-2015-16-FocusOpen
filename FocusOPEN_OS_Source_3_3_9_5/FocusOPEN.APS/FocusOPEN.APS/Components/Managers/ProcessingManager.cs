/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Reflection;
using log4net;

namespace FocusOPEN.APS
{
	public static class ProcessingManager
	{
		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static JobResult ProcessJob(Job job)
		{
			m_Logger.DebugFormat("Sending job for asset {0} for processing. No job id available.", job.AssetId);

			JobProcessor processor = new JobProcessor
			                         	{
			                         		JobId = 0,
											AssetId = job.AssetId,
											InputPath = job.InputPath,
											WatermarkPath = job.WatermarkPath,
											PluginName = job.PluginName,
											CreatePreview = job.CreatePreview,
											CreateThumbnail = job.CreateThumbnail,
											OverrideHeight = job.OverrideHeight,
											OverrideWidth = job.OverrideWidth
			                         	};

			processor.Go();

			return new JobResult(job.AssetId, job.InputPath, processor.PreviewPath, processor.ThumbnailPath, job.AdditionalData, processor.MetadataXml.Root);
		}
	}
}
