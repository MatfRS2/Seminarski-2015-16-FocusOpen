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
	public class ImageMagickProcessingPlugin : ImageProcessingPluginBase
	{
		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region PluginBase Members

		public override void BeforeProcessing()
		{
			GetImageDimensions();
		}

		public override bool CanGeneratePreview
		{
			get
			{
				return true;
			}
		}

		public override bool CanGenerateThumbnail
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
				return new[] {"gif", "jpg", "jpeg", "png", "tif", "tiff", "pdf", "ps", "bmp", "eps", "psd", "ai"};
			}
		}

        public override IEnumerable<string> FileExtensionsGenerated
        {
            get
            {
                return new[] { "jpg","gif","png" };
            }
        }


		public override string GeneratePreview()
		{
			string outputPath = GetTempOutputPath();

			ResizeImage(InputPath, outputPath, PreviewSize.Width, PreviewSize.Height, true);

			if (!String.IsNullOrEmpty(WatermarkPath) && File.Exists(WatermarkPath))
				ApplyWatermark(outputPath, WatermarkPath);

			return outputPath;
		}

		public override string GenerateThumbnail()
		{
			string outputPath = GetTempOutputPath();
			ResizeImage(InputPath, outputPath, ThumbnailSize.Width, ThumbnailSize.Height, true);
			return outputPath;
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Gets the image dimensions and adds them to the file data items so that they are posted back to the web application.
		/// </summary>
		private void GetImageDimensions()
		{
			Size dimensions = GetDimensions(InputPath);
			Size dotsperinch = GetDotsPerInch(InputPath);

			FileDataItems["Height"] = dimensions.Height;
			FileDataItems["Width"] = dimensions.Width;
			
			FileDataItems["DPI_X"] = dotsperinch.Width;
			FileDataItems["DPI_Y"] = dotsperinch.Height;
		}

		private static Size GetDimensions(string imagePath)
		{
			try
			{
				using (Image image = Image.FromFile(imagePath))
				{
					return image.Size;
				}
			}
			catch
			{
				return Size.Empty;
			}
		}

		private Size GetDotsPerInch(string imagePath)
		{
			try
			{
				string imageMagickIdentifyPath = Settings.GetValue("ImageMagickIdentify");
				
				int exitCode;

				string args = string.Format("-format \"%[fx:resolution.x]x%[fx:resolution.y]\" \"{0}\"", imagePath);
				string output = CommandLineExecuter.Execute(imageMagickIdentifyPath, args, out exitCode).Trim();

				const string pattern = @"(?<X>\d.+)x(?<Y>\d.+)$";
				Match match = Regex.Match(output, pattern);

				if (match.Success)
				{
					int x = Convert.ToInt32(match.Groups["X"].Value);
					int y = Convert.ToInt32(match.Groups["Y"].Value);

					m_Logger.DebugFormat("Got DPI from file: {0}.  X:{1}, Y:{2}", imagePath, x, y);
					
					return new Size(x, y);
				}

				m_Logger.WarnFormat("Unable to get DPI from file: {0}.  Output not parsed: {1}", imagePath, output);

				return Size.Empty;
			}
			catch (Exception ex)
			{
				m_Logger.WarnFormat("Errorgetting DPI from file: {0}.  Error: {1}", imagePath, ex.Message);
				return Size.Empty;
			}
		}

		private string GetImageColorspace(string imagePath)
		{
			try
			{
				string imageMagickIdentifyPath = Settings.GetValue("ImageMagickIdentify");

				int exitCode;
				string args = string.Format("-format %r \"{0}\"", imagePath);

				string output = CommandLineExecuter.Execute(imageMagickIdentifyPath, args, out exitCode);

				if (exitCode == 0)
					return output;

				m_Logger.WarnFormat("ImageMagickIdentify Unable to identify image colorspace. Exited with code: {0}", exitCode);
			}
			catch (Exception ex)
			{
				m_Logger.Warn(string.Format("Error getting colorspace for {0}.  Error: {1}", imagePath, ex.Message), ex);
			}

			return "RGB";
		}

		private void ResizeImage(string inputFile, string outputFile, int width, int height, bool keepAspectRatio)
		{
			// Paths to command line utils required
			string colorspaceProfileCMYKPath = Settings.GetValue("ColorspaceProfileCMYK");
			string colorspaceProfileRGBPath = Settings.GetValue("ColorspaceProfileRGB");
			string imageMagickConvertPath = Settings.GetValue("ImageMagickConvert");

			// Get the image colorspace (CMYK or RGB)
			string colorspace = GetImageColorspace(inputFile);

			// Assume image is RGB
			string arguments = "-colorspace RGB -quality 90 -resize " + width + "x" + height + (keepAspectRatio ? " " : "! ") + "\"" + inputFile + "\"[0] \"" + outputFile + "\"";

			// Unless the colorspace returned CMYK
			if (colorspace.Contains("CMYK"))
				arguments = " +profile icm -profile " + colorspaceProfileCMYKPath.WrapInQuotes() + " -profile " + colorspaceProfileRGBPath.WrapInQuotes() + " -quality 90 -resize " + width + "x" + height + (keepAspectRatio ? " " : "! ") + "\"" + inputFile + "\"[0] \"" + outputFile + "\"";

			// Initialize exit code
			int exitCode;

			// Execute the command line
			CommandLineExecuter.Execute(imageMagickConvertPath, arguments, out exitCode);

			// Check exit code
			if (exitCode != 0)
				throw new SystemException(string.Format("Error resizing image: {0}.  Exit code: {1}", inputFile, exitCode));
		}

		private void ApplyWatermark(string imagePath, string watermarkImagePath)
		{
			int exitCode;
			string imageMagickCompositePath = Settings.GetValue("ImageMagickComposite");
			string args = "-dissolve 30 -gravity center \"" + watermarkImagePath + "\" \"" + imagePath + "\" \"" + imagePath + "\"";

			CommandLineExecuter.Execute(imageMagickCompositePath, args, out exitCode);

			if (exitCode != 0)
				throw new SystemException("Error stamping image. ImageMagickComposite exited with code: " + exitCode);
		}

		#endregion
	}
}