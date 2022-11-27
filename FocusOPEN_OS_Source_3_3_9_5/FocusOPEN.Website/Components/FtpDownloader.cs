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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using EnterpriseDT.Net.Ftp;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components
{
	public class FtpDownloader
	{
		#region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly StringBuilder m_ServerResponses = new StringBuilder();
		
		#endregion

		#region Constructor

		public FtpDownloader()
		{
			User = User.Empty;
			Files = new List<FtpFile>();
			PassiveMode = false;
			RemoteFolder = string.Empty;
			Password = string.Empty;
			Username = string.Empty;
			Port = 0;
			Host = string.Empty;
			BackgroundTransfer = false;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Boolean value specifying whether the FTP upload should be done in a seperate thread
		/// </summary>
		public bool BackgroundTransfer { get; set; }

		/// <summary>
		/// Gets or sets the FTP Host
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// Gets or sets the FTP Port
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// Gets or sets the Username to be used to connect to the FTP server
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets the Password to be used to connect to the FTP server
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the remote folder where files should be uploaded to
		/// </summary>
		public string RemoteFolder { get; set; }

		/// <summary>
		/// Gets or sets a boolean value specifying whether passive mode should be used
		/// </summary>
		public bool PassiveMode { get; set; }

		/// <summary>
		/// Gets the list of files to be uploaded
		/// </summary>
		public List<FtpFile> Files { get; set; }

		/// <summary>
		/// Gets or sets the user making the upload
		/// </summary>
		public User User { get; set; }

		#endregion

		/// <summary>
		/// Fired when all files have been uploaded
		/// </summary>
		public event FtpDownloadCompleteEventHandler UploadComplete;

		public void Go()
		{
			TestConnectionAndSendFile();

			if (BackgroundTransfer)
			{
				ThreadStart ts = new ThreadStart(DoUpload);
				Thread thread = new Thread(ts);
				thread.Start();
			}
			else
			{
				DoUpload();
			}
		}

		#region Private Helper Methods

		private void DoUpload()
		{
			try
			{
				using (FTPConnection ftp = new FTPConnection())
				{
					ftp.ServerAddress = Host;
					ftp.ServerPort = Port;
					ftp.UserName = Username;
					ftp.Password = Password;
					ftp.AutoPassiveIPSubstitution = PassiveMode;

					ftp.ReplyReceived += new FTPMessageHandler(ServerResponse);

					ftp.Connect();

					if (!StringUtils.IsBlank(RemoteFolder))
						ftp.ChangeWorkingDirectory(RemoteFolder);

					int count = 0;

					foreach (FtpFile file in Files)
					{
						m_Logger.DebugFormat("Uploading {0} (file {1}/{2}) to {3} for {4}", file, count + 1, Files.Count, Host, User.FullName);

						if (StringUtils.IsBlank(file.LocalPath))
						{
							m_Logger.Warn("File LocalPath is empty. Nothing to upload.");
							continue;
						}

						if (!File.Exists(file.LocalPath))
						{
							m_Logger.WarnFormat("Asset File '{0}' does not exist. Nothing to upload.", file.LocalPath);
							continue;
						}
						
						m_Logger.DebugFormat("Uploading {0} to FTP server...", file.LocalPath);
						ftp.UploadFile(file.LocalPath, file.RemoteFilename);
						m_Logger.Debug("...Done");

						count++;
					}

					ftp.Close();

					m_Logger.DebugFormat("Uploaded {0} files to {1} for {2}", count, Host, User.FullName);
				}

				if (UploadComplete != null)
				{
					FtpDownloadCompleteEventArgs e = new FtpDownloadCompleteEventArgs {ServerMessages = m_ServerResponses.ToString(), User = User};
					UploadComplete(this, e);
				}
			}
			catch (Exception ex)
			{
				// Initialise error message
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("An error occured when doing an FTP transfer");
				
				// Add the error message
				sb.AppendLine(ex.ToString());
				sb.AppendLine();

				// Add the server messages
				sb.Append(m_ServerResponses.ToString());
				sb.AppendLine();

				string message = sb.ToString();

				m_Logger.Error(message, ex);
			}
		}

		/// <summary>
		/// Opens a connection to the FTP server and sends a file called Log.txt
		/// with the current date and time, to check that we can upload files.
		/// </summary>
		private void TestConnectionAndSendFile()
		{
			try
			{
				using (FTPConnection ftp = new FTPConnection())
				{
					ftp.ServerAddress = Host;
					ftp.ServerPort = Port;
					ftp.UserName = Username;
					ftp.Password = Password;
					ftp.AutoPassiveIPSubstitution = PassiveMode;

					try
					{
						ftp.Connect();
					}
					catch (Exception ex)
					{
						throw new FtpDownloadException("Unable to connect to server", ex);
					}

					try
					{
						if (!StringUtils.IsBlank(RemoteFolder))
							ftp.ChangeWorkingDirectory(RemoteFolder);
					}
					catch (Exception ex)
					{
						throw new FtpDownloadException("Remote folder not found: " + RemoteFolder, ex);
					}

					// Message builder
					StringBuilder sb = new StringBuilder();

					// Add date and user information
					sb.AppendFormat("Date: {0}\n", DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss"));
					sb.AppendFormat("User: {0}\n", User.FullName);

					// Add ip address of requesting user (if known)
					if (HttpContext.Current != null)
						sb.AppendFormat("Ip Address: {0}\n", HttpContext.Current.Request.UserHostAddress);

					sb.AppendLine();

					// Add file count
					sb.AppendFormat("File count: {0}", Files.Count);

					sb.AppendLine();

					// Add filenames
					foreach (FtpFile file in Files)
						sb.AppendFormat("- {0}, Remote Filename: {1}\n", Path.GetFileName(file.LocalPath), file.RemoteFilename);

					sb.AppendLine();
					sb.AppendLine("******************************");
					sb.AppendLine();
					sb.AppendLine();
					sb.AppendLine();

					try
					{
						// Upload the log
						UTF8Encoding encoding = new UTF8Encoding();
						byte[] messageBytes = encoding.GetBytes(sb.ToString());
						ftp.UploadByteArray(messageBytes, "Log.txt", false);
					}
					catch (Exception ex)
					{
						throw new FtpDownloadException("Unable to upload files: " + ex.Message, ex);
					}

					ftp.Close();
				}
			}
			catch (Exception ex)
			{
				throw new FtpDownloadException(ex.Message, ex);
			}
		}

		private void ServerResponse(object sender, FTPMessageEventArgs e)
		{
			// Get the message
			string message = e.Message;

			// Define regex to check if this is a PUT command containing the path
			// to the asset file on the server. We want to remove this, as
			// it's a bit of a security risk to show this to the user in the FTP log
			const string pattern = @"PUT\s(?<path>[^\s]+)\s(?<filename>[^\s]+)$";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

			// If it is, then strip out the path and replace it with just the filename
			if (regex.Match(message).Success)
				message = regex.Replace(message, "PUT $2 $2");

			// Append this message onto the string
			m_ServerResponses.AppendLine(message);
		}

		#endregion
	}

	public delegate void FtpDownloadCompleteEventHandler(object sender, FtpDownloadCompleteEventArgs e);

	public class FtpDownloadCompleteEventArgs : EventArgs
	{
		public string ServerMessages;
		public User User;
	}

	public class FtpDownloadException : Exception
	{
		public FtpDownloadException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	public struct FtpFile
	{
		public readonly string LocalPath;
		public readonly string RemoteFilename;

		public FtpFile(string localPath)
		{
			LocalPath = localPath;
			RemoteFilename = Path.GetFileName(LocalPath);
		}

		public FtpFile(string localPath, string remoteFilename) : this(localPath)
		{
			RemoteFilename = remoteFilename;
		}
	}
}