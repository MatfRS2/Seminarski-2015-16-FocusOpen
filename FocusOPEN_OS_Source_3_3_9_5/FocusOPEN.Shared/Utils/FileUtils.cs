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
using System.Security.Cryptography;
using System.Web;
using System.Xml.Serialization;
using System.Xml;
using System.Text;

namespace FocusOPEN.Shared
{
	public static class FileUtils
	{
		/// <summary>
		/// Gets the filesize of the file at the specified path
		/// </summary>
		public static long GetFileSize(string path)
		{
			if (String.IsNullOrEmpty(path))
				return 0;

			FileInfo fileInfo = new FileInfo(path);
			return fileInfo.Length;
		}

		/// <summary>
		/// Gets the filesize in a friendly string representation
		/// </summary>
		public static string FriendlyFileSize(long bytes)
		{
			// Bytes
			if (bytes < 1024)
				return string.Format("{0} B", bytes);

			// Kilobytes
			if (bytes < 1024 * 1024)
				return string.Format("{0} KB", Decimal.Round(bytes / 1024, 2));

			// Divide down into megabytes
			decimal mb = Decimal.Round(Convert.ToDecimal(bytes) / (1024 * 1024), 2);

			// Megabytes
			if (mb < 1024)
				return string.Format("{0} MB", Decimal.Round(mb, 2));

			// Gigabytes
			if (mb < 1024 * 1000)
				return string.Format("{0} GB", Decimal.Round(mb / 1024, 2));

			// Terabytes
			if (mb < 1024 * 10000)
				return string.Format("{0} TB", Decimal.Round(mb / (1024 * 1024), 2));

			return bytes.ToString();
		}

		/// <summary>
		/// Gets the filename, truncated the specified number of characters
		/// whilst still maintaining the file extension
		/// </summary>
		public static string GetTruncatedFilename(string filename, int maxLength)
		{
			if (filename.Length <= maxLength)
				return filename;

			if (maxLength < 10)
				throw new ArgumentException("MaxLength must be at least 10");

			string filenameOnly = Path.GetFileNameWithoutExtension(filename) ?? string.Empty;
			string extension = Path.GetExtension(filename) ?? string.Empty;

			const int elipseCount = 3;
			const int suffixCharsCount = 4;

			return filenameOnly.Substring(0, maxLength - extension.Length - (elipseCount + suffixCharsCount)) + new string('.', elipseCount) + filenameOnly.Substring(filenameOnly.Length - suffixCharsCount) + extension;
		}

		/// <summary>
		/// Gets the hash for a file based on its contents
		/// </summary>
		public static string GetHash(string path)
		{
			using (HashAlgorithm hashAlg = new SHA1Managed())
			{
				using (Stream file = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					byte[] bytes = hashAlg.ComputeHash(file);
					string hash = BitConverter.ToString(bytes).ToLower().Replace("-", string.Empty);

					return hash;
				}
			}
		}

		/// <summary>
		/// Gets the hash for the file based on its contents
		/// </summary>
		public static string GetHash(Stream stream)
		{
			using (HashAlgorithm hashAlg = new SHA1Managed())
			{
				byte[] bytes = hashAlg.ComputeHash(stream);
				return BitConverter.ToString(bytes).ToLower().Replace("-", string.Empty);
			}
		}

        /// <summary>
        /// Gets the MD5 hash for a folder
        /// </summary>
        public static int GetFolderHash(string path, bool includeSubFolders)
        {
            int hasher = -1;

            using (HashAlgorithm hashAlg = new MD5CryptoServiceProvider())
            {

                foreach (string filePath in Directory.GetFiles(path))
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = hashAlg.ComputeHash(file);
                        string hash = BitConverter.ToString(bytes);
                        hasher ^= hash.GetHashCode();
                    }
                }
            }

            if (includeSubFolders)
			{
                foreach (string folder in Directory.GetDirectories(path))
                {
                    hasher ^= GetFolderHash(folder, true);
                }
            }

            return hasher;
        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <param name="dontEscape">Boolean indicating whether to add uri safe escapes to the relative path</param>
        /// <returns>The relative path from the start directory to the end path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static String MakeRelativePath(string fromPath, string toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);

            return relativeUri.ToString();

        }

        /// <summary>
        /// Reads the specified file into a string.
        /// </summary>
        public static string ReadFileToString(string path)
        {
			// TODO: Why is this method needed when there is a File.ReadAllText method in the .NET framework?

            string text = String.Empty;

            using(StreamReader streamReader = new StreamReader(path))
			{
                text = streamReader.ReadToEnd();
            }

            return text;
        }

        /// <summary>
        /// Writes the specified string into a file
        /// </summary>
        public static void WriteFileToString(string path, string write)
        {
			// TODO: Why is this method needed when there is a File.WriteAllText method in the .NET framework?

            using (StreamWriter streamWriter = new StreamWriter(path,false))
            {
                streamWriter.Write(write);
            }
        }

        /// <summary>
        /// Serializes the object to the specified path as xml
        /// </summary>
        public static void SaveToXmlFile(string path, object toSave)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8;
                settings.Indent = true;
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter xwriter = XmlTextWriter.Create(fs, settings))
                {
                    XmlSerializer xserializer = new XmlSerializer(toSave.GetType());
                    xserializer.Serialize(xwriter, toSave);
                }
            }
        }

		public static void TransmitFile(string path, HttpContext context)
		{
			TransmitFile(path, null, context);
		}

		/// <summary>
		/// Transmits the file using the specified http context
		/// </summary>
		/// <param name="path">The file to be sent</param>
		/// <param name="downloadFilename">The filename the browser should see</param>
		/// <param name="context">The HttpContext to which the file should be streamed</param>
		public static void TransmitFile(string path, string downloadFilename, HttpContext context)
		{
			// Ensure we have a http context
			if (context == null)
				throw new ArgumentException("context cannot be null");

			// Ensure the file exists before trying to stream it
			if (!File.Exists(path))
				throw new HttpException(500, "File not found: " + path);

			// Get the filename
			if (StringUtils.IsBlank(downloadFilename))
				downloadFilename = (Path.GetFileName(path) ?? "unknown");

			// Workaround for problem in IE6 where it appends [1] onto filenames containing periods
			if (context.Request.Browser.Browser == "IE")
			{
				string f = (Path.GetFileNameWithoutExtension(downloadFilename) ?? string.Empty);
				string f2 = f.Replace(".", "%2e");
				downloadFilename = downloadFilename.Replace(f, f2);
			}

			// Wrap it in quotes to keep spaces
			downloadFilename = string.Format("\"{0}\"", downloadFilename);

			// Get content type and content disposition
			string contentType = MimeTypeManager.GetMimeType(Path.GetExtension(path));
			string contentDisposition = string.Format("attachment;filename={0}", downloadFilename);

			// Set the headers
            context.Response.Clear();
			context.Response.ContentType = contentType;
			context.Response.AppendHeader("Content-Disposition", contentDisposition);
			context.Response.AppendHeader("Content-Length", GetFileSize(path).ToString());

			// Send the file
			context.Response.TransmitFile(path);
            context.Response.End();
		}
	}
}