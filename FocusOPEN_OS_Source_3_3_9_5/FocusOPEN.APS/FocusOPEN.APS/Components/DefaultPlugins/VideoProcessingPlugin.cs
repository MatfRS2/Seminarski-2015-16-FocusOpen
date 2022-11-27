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
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace FocusOPEN.APS
{
	public class VideoProcessingPlugin : PluginBase
	{
		#region Fields

		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private int m_Height;
		private int m_Width;

		#endregion

		#region IAssetProcessingPlugin Members

		public override void BeforeProcessing()
		{
			GetVideoData();
		}

		public override bool CanGenerateThumbnail
		{
			get
			{
				return true;
			}
		}

		public override bool CanGeneratePreview
		{
			get
			{
				return true;
			}
		}

		public override IEnumerable<string> SupportedFileExtensions
		{
			get
			{
				return new[] { "avi", "mpg", "mpeg", "mov", "mp4", "m4v", "wmv" };
			}
		}

        public override IEnumerable<string> FileExtensionsGenerated
        {
            get
            {
                return new[] { "flv" };
            }
        }

		public override string GeneratePreview()
		{
           	string extension = (Path.GetExtension(InputPath) ?? string.Empty).ToLower();

			// Get temp path where FLV should be stored
			string outputPath = GetTempFilename(GetExtensionGenerated(extension));

			// Get settings
			string ffmpeg = GetSetting("FFmpegExecutablePath");
			string args = GetSetting("FFmpegPreviewArgs");

			// Replace keywords
			args = args.Replace("[INPUT]", InputPath.WrapInQuotes());
			args = args.Replace("[OUTPUT]", outputPath.WrapInQuotes());
			args = args.Replace("[WIDTH]", PreviewSize.Width.ToString());
			args = args.Replace("[HEIGHT]", PreviewSize.Height.ToString());

			// Apply watermark
			if (!String.IsNullOrEmpty(WatermarkPath) && File.Exists(WatermarkPath))
			{
				string watermarkArgs = GetSetting("FFmpegWatermarkArgs");

				if (String.IsNullOrEmpty(watermarkArgs))
				{
					m_Logger.WarnFormat("Unable to apply watermark; no command arguments specified");
				}
				else
				{
					watermarkArgs = watermarkArgs.Replace("[WATERMARK]", WatermarkPath.WrapInQuotes());
					args = args.Replace("[WATERMARK-ARGS]", watermarkArgs);
				}
			}

			// Remove watermark args placeholder, in case no watermark was requested or file not found
			args = args.Replace("[WATERMARK-ARGS]", string.Empty);

			// Execute FFmpeg command line
			CommandLineExecuter.Execute(ffmpeg, args);

			// Update FLV metadata (fixes duration & seek problems)
			string flvtool = GetSetting("FLVToolExecutablePath");
			string flvtoolargs = string.Format("-U {0}", outputPath);
			CommandLineExecuter.Execute(flvtool, flvtoolargs);

			return outputPath;
		}

		public override string GenerateThumbnail()
		{
			// Get temp path where thumbnail should be stored
			string outputPath = GetTempFilename("jpg");

			// Get actual video dimensions
			m_Logger.DebugFormat("Video dimensions are: {0}x{1}", m_Width, m_Height);

			// Get dimensions of the thumbnail to be generated
			Size dimensions = GetAspectImageSize(m_Width, m_Height, ThumbnailSize.Width, ThumbnailSize.Height);
			m_Logger.DebugFormat("Thumbnail will be resized to: {0}x{1}", dimensions.Width, dimensions.Height);

			// Get settings
			string ffmpeg = GetSetting("FFmpegExecutablePath");
			string args = GetSetting("FFmpegThumbnailArgs");

			// Replace keywords
			args = args.Replace("[INPUT]", InputPath.WrapInQuotes());
			args = args.Replace("[OUTPUT]", outputPath.WrapInQuotes());
			args = args.Replace("[WIDTH]", dimensions.Width.ToString());
			args = args.Replace("[HEIGHT]", dimensions.Height.ToString());

			// Execute FFmpeg command line
			CommandLineExecuter.Execute(ffmpeg, args);

			return outputPath;
		}

		#endregion

		#region Private Helper Methods

		private void GetVideoData()
		{
			int width = 0;
			int height = 0;
			int duration = 0;

			string ffmpeg = GetSetting("FFmpegExecutablePath");
			string args = string.Format("-i \"{0}\"", InputPath);

			string output = CommandLineExecuter.Execute(ffmpeg, args);

			Regex re = new Regex(@" (?<width>[0-9\.]{1,4})x(?<height>[0-9\.]{1,4})");
			Match match = re.Match(output);

			if (match.Success)
			{
				width = Convert.ToInt32(match.Groups["width"].Value);
				height = Convert.ToInt32(match.Groups["height"].Value);
			}
			else
			{
				m_Logger.WarnFormat("Unable to get video dimensions from FFmpeg output.  Output content length: {0}. Content: {1}", output.Length, output);
			}

			re = new Regex(@"Duration:\s(?<hours>\d+):(?<minutes>\d+):(?<seconds>\d+\.\d+)");
			match = re.Match(output);

			if (match.Success)
			{
				decimal hours = Decimal.Parse(match.Groups["hours"].Value);
				decimal minutes = Decimal.Parse(match.Groups["minutes"].Value);
				decimal seconds = Decimal.Parse(match.Groups["seconds"].Value);

				seconds += (hours*60*60);
				seconds += (minutes*60);

				duration = Convert.ToInt32(Math.Round(seconds));
			}
			else
			{
				m_Logger.WarnFormat("Unable to get video duration from FFmpeg output.  Output content length: {0}. Content: {1}", output.Length, output);
			}

			m_Height = height;
			m_Width = width;

			FileDataItems["Height"] = height;
			FileDataItems["Width"] = width;
			FileDataItems["Duration"] = duration;

			m_Logger.DebugFormat("Got video metadata - height: {0}, width: {1}, duration: {2} seconds", height, width, duration);
		}

		private static Size GetAspectImageSize(int actualWidth, int actualHeight, int maxWidth, int maxHeight)
		{
			if (actualWidth == 0 || actualHeight == 0)
				return new Size(maxWidth, Convert.ToInt32(maxWidth * 0.8));

			decimal widthScale = Decimal.Divide(maxWidth, actualWidth);
			decimal heightScale = Decimal.Divide(maxHeight, actualHeight);

			decimal scale = Math.Min(widthScale, heightScale);

			int newWidth = Convert.ToInt32(Math.Round(actualWidth * scale));
			int newHeight = Convert.ToInt32(Math.Round(actualHeight * scale));

			if (newWidth % 2 != 0)
				newWidth++;

			if (newHeight % 2 != 0)
				newHeight++;

			return new Size(newWidth, newHeight);
		}

		#endregion
	}
}