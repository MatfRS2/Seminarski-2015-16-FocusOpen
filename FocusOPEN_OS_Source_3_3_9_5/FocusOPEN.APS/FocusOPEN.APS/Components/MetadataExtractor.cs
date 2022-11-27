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
using System.Reflection;
using System.Xml.Linq;
using log4net;

namespace FocusOPEN.APS
{
	internal class MetadataExtractor
	{
		#region Fields

		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Properties

		public string InputFile { get; private set; }
		private string EXIFToolPath { get; set; }

		#endregion

		#region Constructor

		public MetadataExtractor(string inputFile)
		{
			InputFile = inputFile;
			EXIFToolPath = ConfigurationManager.AppSettings.GetValue("EXIFToolPath");

			if (String.IsNullOrEmpty(EXIFToolPath))
				throw new SystemException("EXIFTool Executable Path Not Configured");

			if (!File.Exists(EXIFToolPath))
				throw new SystemException("EXIFTool Executable Not Found: " + EXIFToolPath);
		}

		#endregion

		#region Public Methods

		public XDocument GetXml()
		{
			try
			{
				return new XDocument(new XElement("Metadata",
				                                  GetSimpleXml(),
				                                  GetFullXml()));
			}
			catch (Exception ex)
			{
				m_Logger.Warn(string.Format("Error getting metadata from '{0}'.  Error: {1}", InputFile, ex.Message), ex);
				return new XDocument(new XElement("Metadata"));
			}
		}

		#endregion

		#region Private Methods

		private XElement GetSimpleXml()
		{
			string args = string.Format("\"{0}\"", InputFile);

			Process process = new Process { StartInfo = { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true, FileName = EXIFToolPath, Arguments = args } };
			process.Start();

			string output = process.StandardOutput.ReadToEnd();
			process.WaitForExit();

			var dict = from s in output.Split('\n')
			           where s.Length > 0
			           let pair = s.Split(':')
					   let name = pair[0].Trim()
					   let value = pair[1].Trim()
			           select new {Name = name, Value = value};

			return new XElement("SimpleMetadata", from d in dict
			                                      select new XElement("Item",
			                                                          new XElement("Name") {Value = Sanitize(d.Name)},
			                                                          new XElement("Value") {Value = Sanitize(d.Value)}));
		}

		private XElement GetFullXml()
		{
			string args = string.Format("-X \"{0}\"", InputFile);

			Process process = new Process { StartInfo = { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true, FileName = EXIFToolPath, Arguments = args } };
			process.Start();

			string output = process.StandardOutput.ReadToEnd();
			process.WaitForExit();

			output = Sanitize(output);

			return new XElement("FullMetadata", XElement.Parse(output));
		}

		private static string Sanitize(string s)
		{
			return s.Replace("&", string.Empty);
		}

		#endregion
	}
}
