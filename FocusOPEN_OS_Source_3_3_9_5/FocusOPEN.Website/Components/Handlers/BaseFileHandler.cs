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
using System.Web;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	/// <summary>
	/// The BaseFileHandler class is the base for all file
	/// handlers and contains common functionality
	/// </summary>
	public abstract class BaseFileHandler : BaseHandler
	{
		private const string m_ForceDownloadMimeType = "application/octet-stream";

		#region Protected helper methods

		protected string[] GetTokensFromFilename()
		{
			string filename = Path.GetFileNameWithoutExtension(Context.Request.Path) ?? string.Empty;
			return filename.Split('.');
		}

		protected string GetTokenFromFilename()
		{
			string[] tokens = GetTokensFromFilename();

			if (tokens.Length != 2)
				throw new HttpException(500, "Invalid filename. Token count must be 3.");

			return tokens[1];
		}

		/// <summary>
		/// Gets the ID from the filename
		/// Eg. NewsItem.12.ashx will return 12.
		/// </summary>
		protected int GetIdFromFilename()
		{
			string token = GetTokenFromFilename();

			try
			{
				return Convert.ToInt32(token);
			}
			catch (FormatException)
			{
				throw new HttpException(500, "Invalid ID");
			}
		}

		/// <summary>
		/// Writes the specified file to the response stream
		/// </summary>
		protected void WriteFileToResponseStream(string filename)
		{
			string downloadFilename = Path.GetFileName(filename);
			WriteFileToResponseStream(filename, downloadFilename);
		}

		/// <summary>
		/// Writes the specified file to the response stream,
		/// with the specified filename.
		/// </summary>
		protected void WriteFileToResponseStream(string path, string downloadFilename)
		{
			WriteFileToResponseStream(path, downloadFilename, false);
		}

		/// <summary>
		/// Writes the specified file to the response stream with the specified filename.
		/// If forceDownload is true, the download will force a download box in the user's browser
		/// </summary>
		protected void WriteFileToResponseStream(string path, string downloadFilename, bool forceDownload)
		{
			string extension = Path.GetExtension(path);
			string contentType = (forceDownload) ? m_ForceDownloadMimeType : MimeTypeManager.GetMimeType(extension);
			WriteFileToResponseStream(path, downloadFilename, contentType);
		}

		/// <summary>
		/// Writes the specified file to the response stream, with the 
		/// specified filename using the specified content type.
		/// </summary>
		protected void WriteFileToResponseStream(string path, string downloadFilename, string contentType)
		{
			// Ensure the file exists before trying to stream it
			if (!File.Exists(path))
				throw new HttpException(500, "File not found: " + path);

			// Check we have a filename, and use default if none
			if (String.IsNullOrEmpty(downloadFilename))
				downloadFilename = Path.GetFileName(path) ?? string.Empty;

			// Workaround for bug in IE6 where it appends [1] onto filenames containing periods
			if (Context.Request.Browser.Browser == "IE")
			{
				string f = Path.GetFileNameWithoutExtension(downloadFilename);
				string f2 = f.Replace(".", "%2e");
				downloadFilename = downloadFilename.Replace(f, f2);
			}

			// Wrap it in quotes to keep spaces
			downloadFilename = string.Format("\"{0}\"", downloadFilename);

			string forceDownloadString = (contentType == m_ForceDownloadMimeType) ? "attachment; " : string.Empty;
			string contentDisposition = string.Format("{0}filename={1}", forceDownloadString, downloadFilename);

			Context.Response.ContentType = contentType;
			Context.Response.AppendHeader("Content-Disposition", contentDisposition);
			Context.Response.AppendHeader("Content-Length", FileUtils.GetFileSize(path).ToString());

			Context.Response.TransmitFile(path);
		}

		#endregion
	}
}