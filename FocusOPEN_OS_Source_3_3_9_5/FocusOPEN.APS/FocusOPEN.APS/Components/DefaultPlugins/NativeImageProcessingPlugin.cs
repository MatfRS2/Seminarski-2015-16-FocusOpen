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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace FocusOPEN.APS
{
	public class NativeImageProcessingPlugin : ImageProcessingPluginBase
	{
		#region PluginBase Implementation

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
				return new[] {"gif", "jpg", "jpeg", "png", "tif"};
			}
		}

        public override IEnumerable<string> FileExtensionsGenerated
        {
            get
            {
                return new[] { "jpg" };
            }
        }

        public override string GetExtensionGenerated(string inputExtension)
        {
            return "jpg";
        }


		public override string GeneratePreview()
		{
			string image = GetResizedImage(PreviewSize);

			if (!String.IsNullOrEmpty(WatermarkPath) && File.Exists(WatermarkPath))
				ApplyWatermark(image, WatermarkPath);

			return image;
		}

		public override string GenerateThumbnail()
		{
			return GetResizedImage(ThumbnailSize);
		}

		#endregion

		#region Private Methods

		private void ApplyWatermark(string imagePath, string watermarkPath)
		{
			// TODO: Watermark preview image
		}

		private string GetResizedImage(Size size)
		{
			string outputPath = GetTempOutputPath();
			File.Copy(InputPath, outputPath);
			ImageUtils.ResizeToBox(outputPath, size.Width, size.Height, true);
			return outputPath;
		}

		#endregion

		#region Helper Classes

		private static class ImageUtils
		{
			private static Size GetImageDimensions(string path)
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

			private static ImageFormat GetImageFormat(string path)
			{
				string extension = (Path.GetExtension(path) ?? ".") .Substring(1).ToLower();

				switch (extension)
				{
					case "gif":
						return ImageFormat.Gif;

					case "png":
						return ImageFormat.Png;

					case "jpg":
					case "jpeg":
						return ImageFormat.Jpeg;

					case "tif":
					case "tiff":
						return ImageFormat.Tiff;

					case "bmp":
						return ImageFormat.Bmp;
				}

				throw new Exception("Unable to get image format from extension: " + extension);
			}

			private static void SaveFile(string path, string temp)
			{
				FileInfo fi = new FileInfo(temp);

				if (fi.Length <= 0)
					return;

				File.Delete(path);
				File.Move(temp, path);
			}

			private static void SaveImage(Image image, int sourceWidth, int sourceHeight, int destWidth, int destHeight, int destX, int destY, int sourceX, int sourceY, string path, ImageFormat format)
			{
				using (Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb))
				{
					bmPhoto.SetResolution(image.HorizontalResolution, image.VerticalResolution);

					using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
					{
						grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
						grPhoto.DrawImage(image, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
					}

					bmPhoto.Save(path, format);
				}

				Size size = GetImageDimensions(path);

				if (size.Height > destHeight || size.Width > destWidth)
					throw new SystemException("Error creating resized image.  New image is bigger than requested dimensions");
			}

			public static void ResizeToBox(string path, int width, int height, bool keepAspectRatio)
			{
				Size size = GetImageDimensions(path);

				if (size.Width == width && size.Height == height)
					return;

				string temp = Path.GetTempFileName();
				ImageFormat format = GetImageFormat(path);

				using (Image image = Image.FromFile(path))
				{
					if (keepAspectRatio)
					{
						AspectImageSize ais = new AspectImageSize();
						ais.CalculateAspectRatio(image, width, height);

						if (ais.AspectCalulated)
						{
							SaveImage(image, image.Width, image.Height, ais.AspectWidth, ais.AspectHeight, 0, 0, 0, 0, temp, format);
						}
					}
					else
					{
						SaveImage(image, image.Width, image.Height, width, height, 0, 0, 0, 0, temp, format);
					}
				}

				SaveFile(path, temp);
			}

			#region Nested type: AspectImageSize

			private struct AspectImageSize
			{
				public int AspectHeight;
				public int AspectWidth;

				public bool AspectCalulated
				{
					get
					{
						return (AspectHeight != 0 && AspectWidth != 0);
					}
				}

				public void CalculateAspectRatio(Image image, int maxWidth, int maxHeight)
				{
					// If image is within constraints exit early
					if ((image.Height < (maxHeight + 1)) & (image.Width < (maxWidth + 1)))
						return;

					if (image.Height > image.Width)
					{
						// Shrink height to the maxHeight
						// Shrink width using aspect

						decimal newWidth = (decimal)maxHeight / image.Height * image.Width;

						AspectHeight = maxHeight;
						AspectWidth = Convert.ToInt32(Math.Round(newWidth));

						// If still to wide shrink using new values
						if (AspectWidth > maxWidth)
						{
							decimal newHeight = (decimal)maxWidth / AspectWidth * AspectHeight;

							AspectHeight = Convert.ToInt32(Math.Round(newHeight));
							AspectWidth = maxWidth;
						}
					}
					else
					{
						// Shrink width to the maxWidth
						// Shrink height using aspect

						decimal newHeight = (decimal)maxWidth / image.Width * image.Height;

						AspectHeight = Convert.ToInt32(Math.Round(newHeight));
						AspectWidth = maxWidth;

						// If still to tall shrink using new values
						if (AspectHeight > maxHeight)
						{
							//decimal newWidth = (decimal)maxWidth / AspectWidth * AspectHeight;

							AspectHeight = Convert.ToInt32(Math.Round(newHeight));
							AspectWidth = maxWidth;
						}
					}
				}
			}

			#endregion
		}

		#endregion
	}
}
