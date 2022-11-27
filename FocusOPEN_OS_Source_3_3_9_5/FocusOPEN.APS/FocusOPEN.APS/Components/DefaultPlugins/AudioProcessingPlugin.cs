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

namespace FocusOPEN.APS
{
	public class AudioProcessingPlugin : PluginBase
	{
		#region PluginBase Members

		public override bool CanGenerateThumbnail
		{
			get
			{
				return false;
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
				return new[] {"mp3", "wav", "aif", "aiff", "wma"};
			}
		}

        public override IEnumerable<string> FileExtensionsGenerated
        {
            get
            {
                return new[] { "mp3","wav" };
            }
        }



		public override string GeneratePreview()
		{
			// Get the preview file extension, assume same as input file
			string extension = (Path.GetExtension(InputPath) ?? string.Empty).ToLower();

			// Only WAV and MP3 are allowed, so default to MP3
			// if the input file is neither of these.
            extension = GetExtensionGenerated(extension);

			// Get the temp output path where generated file should be saved
			string outputPath = GetTempFilename(extension);

			// Get settings
			string ffmpeg = GetSetting("FFmpegExecutablePath");
			int duration = GetSetting("CropDuration", 0);

			if (duration == 0)
			{
				File.Copy(InputPath, outputPath, true);
			}
			else
			{
				string args = string.Format("-t {2} -acodec copy -i \"{0}\" \"{1}\"", InputPath, outputPath, duration);
				CommandLineExecuter.Execute(ffmpeg, args);
			}

			return outputPath;
		}

		public override string GenerateThumbnail()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}