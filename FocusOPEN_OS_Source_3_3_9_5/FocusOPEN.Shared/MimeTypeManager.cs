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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Xml;
using log4net;

namespace FocusOPEN.Shared
{
	/// <summary>
	/// The MimeTypeManager class is used to calculate the content type for a specified
	/// file extension.  These are stored in an external XML file, which is used to configure
	/// this class using the Initialise() method.
	/// 
	/// This class also contains methods to check if the mimetype for a specific extension
	/// is known, and adding filetypes and content types to the configuration file.
	/// </summary>
	public static class MimeTypeManager
	{
		#region Private variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static string m_ConfigurationPath = string.Empty;

		#endregion

		#region Accessors

		/// <summary>
		/// Gets or sets the configuration path where mime type information is stored
		/// </summary>
		public static string ConfigurationPath
		{
			get
			{
				return m_ConfigurationPath;
			}
			set
			{
				if (!File.Exists(value))
					throw new FileNotFoundException("MimeTypes configuration file not found: " + value);

				m_ConfigurationPath = value;
			}
		}

		#endregion

		/// <summary>
		/// Gets the list of mimetypes
		/// </summary>
		public static List<MimeType> MimeTypeList
		{
			get
			{
				// Get the list from the cache
				List<MimeType> list = HttpRuntime.Cache.Get("MimeTypeList") as List<MimeType>;

				if (list == null)
				{
					// Initialise the list
					list = new List<MimeType>();

					// Load the mime type list
					XmlDocument doc = new XmlDocument();
					doc.Load(ConfigurationPath);

					// Get the nodes
					XmlNodeList mimeTypeNodes = doc.SelectNodes("/MimeTypes/MimeType");

					// Ensure we have nodes
					if (mimeTypeNodes == null)
						throw new SystemException("Missing MimeType node");

					foreach (XmlNode mimeTypeNode in mimeTypeNodes)
					{
						if (mimeTypeNode.Attributes == null)
							continue;

						string contentType = mimeTypeNode.Attributes.GetNamedItem("ContentType").Value.ToLower();
						MimeType mimeType = new MimeType(contentType);

						XmlNodeList fileExtensionNodes = mimeTypeNode.SelectNodes("FileExtension");

						if (fileExtensionNodes == null)
							throw new SystemException("Missing FileExtension node");

						foreach (XmlNode fileExtensionNode in fileExtensionNodes)
						{
							if (fileExtensionNode.Attributes == null)
								continue;

							string fileExtension = ParseExtension(fileExtensionNode.Attributes.GetNamedItem("value").Value);
							mimeType.FileExtensions.Add(fileExtension);
						}

						list.Add(mimeType);
					}

					HttpRuntime.Cache.Insert("MimeTypeList", list, new CacheDependency(ConfigurationPath));

					m_Logger.InfoFormat("Initialised MimeType list successfully");
				}

				return list;
			}
		}

		#region Public Methods

		/// <summary>
		/// Checks if the specified file extension has a known mimetype
		/// </summary>
		public static bool CheckMimeType(string extension)
		{
			extension = ParseExtension(extension);
			return MimeTypeList.Any(mimeType => mimeType.FileExtensions.Contains(extension));
		}

		/// <summary>
		/// Gets the mimetype for the specified extension.
		/// If the extension does not have a known mimetype,
		/// the specified default mimetype is returned.
		/// </summary>
		public static string GetMimeType(string extension, string defaultMimeType)
		{
			if (string.IsNullOrEmpty(extension))
				throw new UnknownMimeTypeException("Extension cannot be empty");

			extension = ParseExtension(extension);

			if (string.IsNullOrEmpty(extension))
				throw new UnknownMimeTypeException("Parsed extension is empty");

			foreach (MimeType mimeType in MimeTypeList.Where(mimeType => mimeType.FileExtensions.Contains(extension)))
				return (mimeType.ContentType);

			if (defaultMimeType == string.Empty)
				throw new UnknownMimeTypeException(string.Format("Unable to find mime type for file extension: {0}", extension));

			return defaultMimeType;
		}

		/// <summary>
		/// Gets the mimetype for the specified extension
		/// If the extension does not have a known mimetype,
		/// an UnknownMimeTypeException is thrown.
		/// </summary>
		public static string GetMimeType(string extension)
		{
			return GetMimeType(extension, string.Empty);
		}

		/// <summary>
		/// Adds the specified extension and content type to the configuration path.
		/// 
		/// If the extension already exists, even if under a different content type,
		/// it is not added to the config file.
		/// 
		/// If the content type exists, a new FileExtension node is added to it.
		/// 
		/// If the content type does not exist, a new MimeType node is created with
		/// the specified content type, and a FileExtension node is added to that.
		/// </summary>
		public static void AddMimeType(string extension, string contentType)
		{
			if (string.IsNullOrEmpty(extension))
				throw new UnknownMimeTypeException("Extension cannot be empty");

			extension = ParseExtension(extension);

			if (string.IsNullOrEmpty(extension))
				throw new UnknownMimeTypeException("Parsed extension is empty");

			if (CheckMimeType(extension))
				return;

			contentType = contentType.ToLower();

			XmlDocument doc = new XmlDocument();
			doc.Load(ConfigurationPath);

			// Check if this content type is already in the list
			XmlNode MimeTypeNode = doc.SelectSingleNode(string.Format("/MimeTypes/MimeType[@ContentType='{0}']", contentType));

			// Create a new content type node if it doesn't exist
			if (MimeTypeNode == null)
			{
				MimeTypeNode = doc.CreateNode(XmlNodeType.Element, "MimeType", string.Empty);

				XmlAttribute attribute = doc.CreateAttribute("ContentType");
				attribute.Value = contentType;
				
				if (MimeTypeNode.Attributes != null)
					MimeTypeNode.Attributes.Append(attribute);

				if (doc.DocumentElement != null)
					doc.DocumentElement.AppendChild(MimeTypeNode);
			}

			XmlElement FileExtension = doc.CreateElement("FileExtension");
			FileExtension.SetAttribute("value", extension);
			FileExtension.SetAttribute("dateadded", DateTime.Now.ToString("s"));
			MimeTypeNode.AppendChild(FileExtension);

			try
			{
				doc.Save(ConfigurationPath);
				m_Logger.DebugFormat("Added MimeType - Extension: {0}, ContentType: {1}", extension, contentType);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("Error saving MimeTypes.Config: {0}", ex));
				m_Logger.WarnFormat("Error adding mimetype: {0}", ex.Message);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Parses and returns the extensions.
		/// This typically means stripping the preceding
		/// period if it exists and lowercasing it.
		/// </summary>
		private static string ParseExtension(string extension)
		{
			if (extension.StartsWith("."))
				extension = extension.Substring(1);

			extension = extension.ToLower();

			return extension;
		}

		#endregion

		#region Helper class

		/// <summary>
		/// Private class represents a mime type
		/// Contains the content type string and list of file extensions
		/// </summary>
		public class MimeType
		{
			public readonly string ContentType;
			public readonly List<String> FileExtensions = new List<String>();

			public MimeType(string contentType)
			{
				ContentType = contentType;
			}
		}

		#endregion
	}

	public class UnknownMimeTypeException : Exception
	{
		public UnknownMimeTypeException(string message) : base(message)
		{
		}
	}
}