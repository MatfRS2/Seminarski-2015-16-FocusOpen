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
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;

namespace FocusOPEN.APS
{
	/// <summary>
	/// Allows commands to be executed at the command line and retrieve any output sent
	/// to both the standard and error output streams.
	/// </summary>
	public class CommandLineExecuter
	{
		#region Fields

		private static readonly ILog m_Logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly StringBuilder m_Output = new StringBuilder();
		private bool m_IsProcessing = false;

		#endregion

		#region Properties

		/// <summary>
		/// The path to the application that should be executed
		/// </summary>
		public string ApplicationPath { private get; set; }

		/// <summary>
		/// Command line parameters
		/// </summary>
		public string Arguments { private get; set; }
		
		/// <summary>
		/// Gets the output after the command line has been executed
		/// </summary>
		public string Output
		{
			get
			{
				return m_Output.ToString();
			}
		}

		/// <summary>
		/// Gets the exit code after the command line has been executed
		/// </summary>
		public int ExitCode { private set; get; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Executes the command line and waits until all output has been retrieved.
		/// </summary>
		public void Go()
		{
			m_Logger.DebugFormat("Executing command line: {0} {1}", ApplicationPath, Arguments);

			if (String.IsNullOrEmpty(ApplicationPath))
				throw new SystemException("Unable to execute command line, application path has not been specified");

			if (!File.Exists(ApplicationPath))
				throw new SystemException("Unable to execute command line, application path does not exist: " + ApplicationPath);

			Process process = new Process { EnableRaisingEvents = true, StartInfo = { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true, FileName = ApplicationPath, Arguments = Arguments } };

			string workingDirectory = Path.GetDirectoryName(ApplicationPath);

			if (workingDirectory != null)
				process.StartInfo.WorkingDirectory = workingDirectory;

			process.OutputDataReceived += OutputDataReceived;
			process.ErrorDataReceived += OutputDataReceived;
			process.Exited += ProcessExited;

			m_IsProcessing = true;

			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			while (m_IsProcessing)
				Thread.Sleep(0);

			m_Logger.DebugFormat("Command line exit code: {0}", process.ExitCode);

			ExitCode = process.ExitCode;
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Executes the command line and gets the response
		/// </summary>
		public static string Execute(string app, string args)
		{
			int exitCode;
			return Execute(app, args, out exitCode);
		}

		/// <summary>
		/// Executes the command line and gets the response and exit code
		/// </summary>
		public static string Execute(string app, string args, out int exitCode)
		{
			CommandLineExecuter cle = new CommandLineExecuter { ApplicationPath = app, Arguments = args };
			cle.Go();
			exitCode = cle.ExitCode;
			return cle.Output;
		}

		#endregion

		#region Helper Methods

		private void ProcessExited(object sender, EventArgs e)
		{
			Process process = (Process) sender;
			process.WaitForExit();

			m_IsProcessing = false;
		}

		private void OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			Console.WriteLine(e.Data);
			m_Output.AppendLine(e.Data);
		}

		#endregion
	}
}