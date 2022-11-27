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

namespace FocusOPEN.Business
{
	/// <summary>
	/// Represents an uploaded file
	/// </summary>
	public class BinaryFile
	{
		public enum SaveMode
		{
			Copy,
			Move
		}

		#region Private Variables

		private string m_FileName = null;
		private readonly FileInfo m_File = null;
		private readonly long m_FileSize = 0;
		private string m_FileHash = null;
		private readonly HttpPostedFile m_HttpPostedFile = null;
		private readonly SaveMode m_SaveMode = SaveMode.Move;
		private const string NO_FILE_EXCEPTION = "No File.  Please check for IsEmpty before querying this property.";

		#endregion

		#region Constructors

		private BinaryFile()
		{
		}

		public BinaryFile(HttpPostedFile httpPostedFile)
		{
			m_HttpPostedFile = httpPostedFile;

			if (m_HttpPostedFile != null)
				m_FileSize = m_HttpPostedFile.InputStream.Length;
		}

		public BinaryFile(string path)
		{
			if (!File.Exists(path))
				throw new FileNotFoundException("File not found: " + path);

			m_File = new FileInfo(path);

			try
			{
				using (FileStream fs = m_File.OpenRead())
				{
					m_FileSize = fs.Length;
					fs.Close();
				}
			}
			catch (Exception)
			{
				// If the class is instantiated whilst the file is locked
				// by another process, just set the filesize to 0

				m_FileSize = 0;
			}
		}

		public BinaryFile(string path, SaveMode saveMode) : this(path)
		{
			m_SaveMode = saveMode;
		}

		#endregion

		public static BinaryFile Empty
		{
			get
			{
				return new BinaryFile();
			}
		}

		/// <summary>
		/// Gets the name of the file without any path information (eg. MyFile.txt)
		/// </summary>
		public string FileName
		{
			get
			{
				if (m_FileName != null)
					return m_FileName;

				if (m_HttpPostedFile != null)
					return Path.GetFileName(m_HttpPostedFile.FileName);

				if (m_File != null)
					return Path.GetFileName(m_File.FullName);

				throw new Exception(NO_FILE_EXCEPTION);
			}
			set
			{
				m_FileName = value;
			}
		}

		/// <summary>
		/// Gets the file extension lowercased and without a preceding period
		/// </summary>
		public string FileExtension
		{
			get
			{
				return StringUtils.GetFileExtension(FileName);
			}
		}

		public string ContentType
		{
			get
			{
				if (m_HttpPostedFile != null)
					return m_HttpPostedFile.ContentType;

				if (m_File != null)
					return MimeTypeManager.GetMimeType(FileExtension);

				throw new Exception(NO_FILE_EXCEPTION);
			}
		}

		public bool IsEmpty
		{
			get
			{
				if (m_HttpPostedFile != null && m_HttpPostedFile.ContentLength > 0)
					return false;

				if (m_File != null)
					return false;

				return true;
			}
		}

		public long FileSize
		{
			get
			{
				return m_FileSize;
			}
		}

		public Stream InputStream
		{
			get
			{
				if (m_HttpPostedFile != null)
					return m_HttpPostedFile.InputStream;

				if (m_File != null)
					return m_File.OpenRead();

				throw new Exception(NO_FILE_EXCEPTION);
			}
		}

		public string FileHash
		{
			get
			{
				if (m_FileHash == null)
				{
					using (Stream stream = InputStream)
						m_FileHash = FileUtils.GetHash(stream);
				}

				return m_FileHash;
			}
		}

		public void SaveAs(string filename)
		{
			if (m_HttpPostedFile != null)
				m_HttpPostedFile.SaveAs(filename);

			if (m_File != null)
			{
				switch (m_SaveMode)
				{
					case (SaveMode.Move):
						m_File.MoveTo(filename);
						break;

					case (SaveMode.Copy):
						m_File.CopyTo(filename);
						break;
				}
			}
		}
	}
}