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
using System.IO;
using System.Reflection;
using System.Web;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	public static class AssetImageManager
	{
		#region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Image Resize Methods

		public static string GetResizedAssetImage(Asset asset, int assetImageSizeId, DownloadFormat downloadFormat, bool create)
		{
			//---------------------------------------------------------------------------------------------------------
			// Get asset file info
			//---------------------------------------------------------------------------------------------------------
			AssetFileInfo fileInfo = new AssetFileInfo(asset);

			//---------------------------------------------------------------------------------------------------------
			// Ensure asset file exists
			//---------------------------------------------------------------------------------------------------------
			if (!fileInfo.FileExists)
				return string.Empty;

			//---------------------------------------------------------------------------------------------------------
			// No options specified
			//---------------------------------------------------------------------------------------------------------
			if (assetImageSizeId == 0 && downloadFormat == DownloadFormat.Original)
				return fileInfo.FilePath;

			//---------------------------------------------------------------------------------------------------------
			// Only images can be resized
			//---------------------------------------------------------------------------------------------------------
			if (!AssetTypeChecker.IsImage(asset.FileExtension))
				return fileInfo.FilePath;

			//---------------------------------------------------------------------------------------------------------
			// PDFs are an exception as they are classified as images but cant be resized
			//---------------------------------------------------------------------------------------------------------
			if (StringUtils.GetFileExtension(asset.Filename) == "pdf")
				return fileInfo.FilePath;

			//---------------------------------------------------------------------------------------------------------
			// Get the requested image size
			//---------------------------------------------------------------------------------------------------------
			AssetImageSize ais = AssetImageSizeCache.Instance.GetById(assetImageSizeId);

			//---------------------------------------------------------------------------------------------------------
			// Construct the path to the output filename
			//---------------------------------------------------------------------------------------------------------
			string suffix = (StringUtils.IsBlank(ais.FileSuffix)) ? "original" : ais.FileSuffix;
			string reference = Path.GetFileNameWithoutExtension(asset.Filename) + "_" + suffix;
			string extension = GetDownloadFormat(downloadFormat, asset.FileExtension);
			string cacheFolder = VirtualPathUtility.AppendTrailingSlash(Path.Combine(Settings.CachedAssetFilesFolder, asset.AssetId.ToString()));
			string outputFilename = Path.Combine(cacheFolder, reference) + "." + extension;

			if (File.Exists(outputFilename))
				return outputFilename;

			if (!create)
				return string.Empty;

			//---------------------------------------------------------------------------------------------------------
			// Create the cache folder if it doesn't exist
			//---------------------------------------------------------------------------------------------------------
			if (!Directory.Exists(cacheFolder))
			{
				Directory.CreateDirectory(cacheFolder);
				m_Logger.DebugFormat("Created cache folder: {0}", cacheFolder);
			}

			//------------------------------------------------------------------------------------------------
			// Now we've verified that we need to do some work.
			//------------------------------------------------------------------------------------------------
			string tempPath = Path.Combine(Settings.TempFolder, asset.AssetId + "_" + Guid.NewGuid() + Path.GetExtension(fileInfo.FilePath));

			//---------------------------------------------------------------------------------------------------------
			// Create resized image
			//---------------------------------------------------------------------------------------------------------
			if (ais.Width > 0 && ais.Height > 0)
			{
				ImageProcessingJob job = new ImageProcessingJob
				{
					SourceFilePath = fileInfo.FilePath,
					TargetFilePath = tempPath,
					Width = ais.Width,
					Height = ais.Height
				};

				job.Go();

				m_Logger.DebugFormat("Generated resized image for asset id: {0}, assetImageSize: {1}, height: {2}, width: {3} at: {4}", asset.AssetId, ais.Description, ais.Height, ais.Width, job.TargetFilePath);
			}

			//---------------------------------------------------------------------------------------------------------
			// Ensure we've got our image
			//---------------------------------------------------------------------------------------------------------
			if (!File.Exists(tempPath))
			{
				File.Copy(fileInfo.FilePath, tempPath);
				m_Logger.DebugFormat("Original asset copied to: {0}", tempPath);
			}

			//---------------------------------------------------------------------------------------------------------
			// Now convert the DPI to the value requested
			//---------------------------------------------------------------------------------------------------------
			if (ais.DotsPerInch > 0)
			{
				ImageUtils.ChangeDPI(tempPath, ais.DotsPerInch);
				m_Logger.DebugFormat("Changed DPI of file: {0} to: {1}", tempPath, ais.DotsPerInch);
			}

			//------------------------------------------------------------------------------------------------
			// Do download format conversion
			//------------------------------------------------------------------------------------------------
			if (StringUtils.GetFileExtension(tempPath) != StringUtils.GetFileExtension(outputFilename))
				ImageUtils.ConvertFormat(tempPath, outputFilename);

			//------------------------------------------------------------------------------------------------
			// Ensure we've got the output file now
			//------------------------------------------------------------------------------------------------
			if (!File.Exists(outputFilename))
				File.Move(tempPath, outputFilename);

			//---------------------------------------------------------------------------------------------------------
			// All done, return path
			//---------------------------------------------------------------------------------------------------------
			return outputFilename;
		}

		#endregion

		#region Private Helper Methods
		
		private static string GetDownloadFormat(DownloadFormat downloadFormat, string defaultExtension)
		{
			switch (downloadFormat)
			{
				case DownloadFormat.Original:
					return defaultExtension;

				case DownloadFormat.JPEG:
					return "jpg";

				case DownloadFormat.PNG:
					return "png";

				case DownloadFormat.TIFF:
					return "tif";

				default:
					throw new SystemException("Unknown download format: " + downloadFormat);
			}
		}

		#endregion
	}
}
