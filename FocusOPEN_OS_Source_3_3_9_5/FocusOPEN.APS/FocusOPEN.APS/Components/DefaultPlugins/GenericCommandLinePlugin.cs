/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.Drawing;

namespace FocusOPEN.APS
{
	public class GenericCommandLinePlugin : PluginBase
	{
		#region PluginBase Implementation

		public override bool CanGeneratePreview
		{
			get
			{
				return (!string.IsNullOrEmpty(GetSetting("PreviewCommandLine_App")));
			}
		}

		public override bool CanGenerateThumbnail
		{
			get
			{
				return (!string.IsNullOrEmpty(GetSetting("ThumbnailCommandLine_App")));
			}
		}

		public override IEnumerable<string> SupportedFileExtensions
		{
			get
			{
				return new List<string>();
			}
		}

        public override IEnumerable<string> FileExtensionsGenerated
        {
            get
            {
                return new[] { "Dynamic" };
            }
        }


        public override string GetExtensionGenerated(string inputExtension)
        {
            return "Dynamic"; //not used by GeneratePreview so purely for info purposes
        }

		public override string GeneratePreview()
		{
			return ExecuteCommandLine("Preview", PreviewSize);
		}

		public override string GenerateThumbnail()
		{
			return ExecuteCommandLine("Thumbnail", ThumbnailSize);
		}

		#endregion

		private string ExecuteCommandLine(string setting, Size size)
		{
			// Get command line app settings
			string commandLineApp = GetSetting(setting + "CommandLine_App");
			string commandLineArgs = GetSetting(setting + "CommandLine_Args");
			string extension = GetSetting(setting + "_Extension");

			// Get input and output paths
			string inputPath = InputPath;
			string outputPath = GetTempFilename(extension);

			// Replace command line arg placeholders with actual values
			commandLineArgs = commandLineArgs.Replace("[INPUT]", inputPath.WrapInQuotes());
			commandLineArgs = commandLineArgs.Replace("[OUTPUT]", outputPath.WrapInQuotes());
			commandLineArgs = commandLineArgs.Replace("[WIDTH]", size.Width.ToString());
			commandLineArgs = commandLineArgs.Replace("[HEIGHT]", size.Height.ToString());

			// Execute command line
			CommandLineExecuter.Execute(commandLineApp, commandLineArgs);

			return outputPath;
		}
	}
}