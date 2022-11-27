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
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace FocusOPEN.Shared
{
	public static class ImageUtils
	{
		#region Properties

		/// <summary>
		/// Gets or sets the path to the watermark image
		/// </summary>
		public static string DEFAULT_WATERMARK { get; set; }

		/// <summary>
		/// Path to ImageMagick convert.exe
		/// </summary>
		public static string ImageMagickConvert { get; set; }

		/// <summary>
		/// Path to ImageMagick composite.exe
		/// </summary>
		public static string ImageMagickComposite { get; set; }

		/// <summary>
		/// Path to ImageMagick identify.exe
		/// </summary>
		public static string ImageMagickIdentify { get; set; }

		/// <summary>
		/// Path to CMYK profile
		/// </summary>
		public static string ColorspaceProfileCMYK { get; set; }

		/// <summary>
		/// Path to RGB profile
		/// </summary>
		public static string ColorspaceProfileRGB { get; set; }

		#endregion

		/// <summary>
		/// Gets image dimensions
		/// </summary>
		/// <param name="path">The path to the image</param>
		/// <returns>Image dimensions.</returns>
		public static Size GetImageDimensions(string path)
		{
			try
			{
				using (Image image = Image.FromFile(path))
				{
					return image.Size;
				}
			}
			catch
			{
				return new Size();
			}
		}

		/// <summary>
		/// Resizes the image to the specified dimensions if it is too large. If it is too small, no action is taken
		/// </summary>
		public static void ResizeImageDown(string path, int height, int width)
		{
			Size size = GetImageDimensions(path);

			if (size.Height > height || size.Width > width)
			{
				ImageProcessingJob job = new ImageProcessingJob { SourceFilePath = path, Height = height, Width = width };
				job.Go();
			}
		}

		/// <summary>
		/// Gets the colorspace for an image
		/// </summary>
		/// <param name="imagePath">The image path</param>
		/// <returns>The colorspace as found by ImageMagick</returns>
		public static string GetImageColorspace(string imagePath)
		{
			Process process = new Process {StartInfo = {CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true}};

			process.StartInfo.FileName = ImageMagickIdentify;
			process.StartInfo.Arguments = "-format %r \"" + imagePath + "\"";

			process.Start();
			process.WaitForExit();

			if (process.ExitCode != 0)
				throw new SystemException("ImageUtils::ResizeImage::ImageMagickIdentify Unable to identify image colorspace. Exited with code: " + process.ExitCode);

			return process.StandardOutput.ReadToEnd();
		}

		/// <summary>
		/// Resize an image
		/// </summary>
		/// <param name="inputFile">The image to be resized</param>
		/// <param name="outputFile">The path where the resized image should be saved</param>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		/// <param name="keepAspectRatio">Boolean value specifying whether aspect ratio should be maintained</param>
		public static void ResizeImage(string inputFile, string outputFile, int width, int height, bool keepAspectRatio)
		{
			string colorspace = GetImageColorspace(inputFile);
			Process process = new Process {StartInfo = {CreateNoWindow = true, UseShellExecute = false, FileName = ImageMagickConvert}};

			// Assume image is RGB
			process.StartInfo.Arguments = "-colorspace RGB -quality 90 -resize " + width + "x" + height + (keepAspectRatio ? " " : "! ") + "\"" + inputFile + "\"[0] \"" + outputFile + "\"";

			// Unless the colorspace returned CMYK
			if (colorspace.Contains("CMYK"))
				process.StartInfo.Arguments = " +profile icm -profile \"" + ColorspaceProfileCMYK + "\" -profile \"" + ColorspaceProfileRGB + "\" -quality 90 -resize " + width + "x" + height + (keepAspectRatio ? " " : "! ") + "\"" + inputFile + "\"[0] \"" + outputFile + "\"";

			process.Start();
			process.WaitForExit();

			if (process.ExitCode != 0)
				throw new SystemException("ImageUtils::ResizeImage::ImageMagickConvert exited with code: " + process.ExitCode);
		}

		/// <summary>
		/// Stamps the image
		/// </summary>
		/// <param name="imagePath">The image to be stamped</param>
		/// <param name="stampImagePath">The stamp to be used</param>
		public static void StampImage(string imagePath, string stampImagePath)
		{
			Process process = new Process {StartInfo = {CreateNoWindow = true, UseShellExecute = false}};

			process.StartInfo.FileName = ImageMagickComposite;
			process.StartInfo.Arguments = " -gravity SouthEast \"" + stampImagePath + "\" \"" + imagePath + "\" \"" + imagePath + "\"";

			process.Start();
			process.WaitForExit();

			if (process.ExitCode != 0)
				throw new SystemException("ImageUtils::StampImage::ImageMagickComposite exited with code: " + process.ExitCode);
		}

		/// <summary>
		/// Watermarks an image
		/// </summary>
		/// <param name="imagePath"></param>
		/// <param name="watermarkImagePath"></param>
		public static void WatermarkImage(string imagePath, string watermarkImagePath)
		{
			Process process = new Process {StartInfo = {CreateNoWindow = true, UseShellExecute = false}};

			process.StartInfo.FileName = ImageMagickComposite;
			process.StartInfo.Arguments = "-dissolve 30 -gravity center \"" + watermarkImagePath + "\" \"" + imagePath + "\" \"" + imagePath + "\"";

			process.Start();
			process.WaitForExit();

			if (process.ExitCode != 0)
				throw new SystemException("ImageUtils::StampImage::ImageMagickComposite exited with code: " + process.ExitCode);
		}

		/// <summary>
		/// Change the DPI for an image
		/// </summary>
		/// <param name="imagePath">The image to be modified</param>
		/// <param name="dpi">The DPI to be set</param>
		public static void ChangeDPI(string imagePath, int dpi)
		{
			Process process = new Process {StartInfo = {CreateNoWindow = true, UseShellExecute = false}};

			process.StartInfo.FileName = ImageMagickConvert;
			process.StartInfo.Arguments = string.Format("-density {0}x{0} -units PixelsPerInch \"{1}\" \"{1}\"", dpi, imagePath);

			process.Start();
			process.WaitForExit();

			if (process.ExitCode != 0)
				throw new SystemException("ImageUtils::ChangeDPI::ImageMagickConvert exited with code: " + process.ExitCode);
		}

		/// <summary>
		/// Converts the input file to the output file.  Conversion is automatic based on file extension
		/// </summary>
		/// <param name="inputPath">The image file to be converted</param>
		/// <param name="outputPath">The path where the converted image file should be saved. Will be overwritten.</param>
		/// <example>ConvertFormat(@"c:\test.gif", @"c:\test.jpg") will convert the file from GIF to JPG.</example>
		public static void ConvertFormat(string inputPath, string outputPath)
		{
			string inputExtension = StringUtils.GetFileExtension(inputPath);
			string outputExtension = StringUtils.GetFileExtension(outputPath);

			if (inputExtension == outputExtension)
			{
				File.Copy(inputPath, outputPath, true);
				return;
			}

			if (File.Exists(outputPath))
				File.Delete(outputPath);

			string args = string.Format("\"{0}\" -compress None \"{1}\"", inputPath, outputPath);

			Process process = new Process {StartInfo = {CreateNoWindow = true, UseShellExecute = false, FileName = ImageMagickConvert, Arguments = args}};
			process.Start();
			process.WaitForExit();

			if (process.ExitCode != 0)
				throw new SystemException("ImageUtils::ConvertFormat::ImageMagickConvert exited with code: " + process.ExitCode);
		}
	}

	public class ImageProcessingJob
	{
		/// <summary>
		/// Gets or sets the image file to be processed
		/// </summary>
		public string SourceFilePath { get; set; }

		/// <summary>
		/// Gets or sets the location of the output file
		/// </summary>
		public string TargetFilePath { get; set; }

		/// <summary>
		/// Gets or sets the width of the output file
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Gets or sets the height of the output file
		/// </summary>
		public int Height { get; set; }
		
		/// <summary>
		/// Gets or sets the image DPI
		/// </summary>
		public int DPI { get; set; }

		/// <summary>
		/// Gets or sets the location of the watermark image
		/// </summary>
		public string WatermarkImage { get; set; }

		/// <summary>
		/// Gets or sets the location of the stamp image
		/// </summary>
		public string StampImage { get; set; }

		public void Go()
		{
			string target = (!String.IsNullOrEmpty(TargetFilePath)) ? TargetFilePath : SourceFilePath;

			string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(target));

			ImageUtils.ResizeImage(SourceFilePath, tempPath, Width, Height, true);

			if (!String.IsNullOrEmpty(WatermarkImage))
				ImageUtils.WatermarkImage(tempPath, WatermarkImage);

			if (!String.IsNullOrEmpty(StampImage))
				ImageUtils.StampImage(tempPath, StampImage);

			if (DPI > 0)
				ImageUtils.ChangeDPI(tempPath, DPI);

			if (File.Exists(target))
				File.Delete(target);

			File.Move(tempPath, target);
		}
	}
}