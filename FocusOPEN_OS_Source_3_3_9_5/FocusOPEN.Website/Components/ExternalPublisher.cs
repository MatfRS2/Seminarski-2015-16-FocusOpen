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
using System.Reflection;
using System.Web.Security;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components
{
	public class ExternalPublisher
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Singleton

		private ExternalPublisher()
		{
		}

		private static ExternalPublisher m_Instance;

		public static ExternalPublisher Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = new ExternalPublisher();

				return m_Instance;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the folder where files should be published to
		/// </summary>
		public string Folder { get; set; }

		/// <summary>
		/// Gets or sets the prefix to be used when generating the hash
		/// </summary>
		public string Prefix { get; set; }

		#endregion

		#region Public Methods

		public bool IsEnabled()
		{
			return (!StringUtils.IsBlank(Folder) && Directory.Exists(Folder));
		}

		public string GetHash(int assetId)
		{
			string s = string.Concat(Prefix, "_", assetId);
			string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5");

			if (string.IsNullOrEmpty(hash))
				throw new SystemException("Error hashing string: " + s);

			return hash.ToLower();
		}

		public bool IsPublished(int assetId)
		{
			if (!IsEnabled())
				return false;

			string hash = GetHash(assetId);
			string[] files = Directory.GetFiles(Folder, hash + ".*", SearchOption.TopDirectoryOnly);
			return (files.Length > 0);
		}

		public bool Publish(int assetId)
		{
			if (!IsEnabled())
				return false;

			Asset asset = Asset.Get(assetId);
			AssetFileInfo afi = new AssetFileInfo(asset);

			if (!afi.FileExists)
			{
				m_Logger.WarnFormat("Unable to publish asset: {0}.  Asset preview file does not exist.", assetId);
				return false;
			}

			string extension = Path.GetExtension(afi.FilePath);
			string filename = GetHash(assetId) + extension;
			string newpath = Path.Combine(Folder, filename);

			if (File.Exists(newpath))
				File.Delete(newpath);

			File.Copy(afi.FilePath, newpath);

			return true;
		}

		public void Unpublish(int assetId)
		{
			if (!IsEnabled())
				return;

			DeleteFiles(assetId);
		}

		#endregion

		#region Private Methods

		private void DeleteFiles(int assetId)
		{
			string hash = GetHash(assetId);
			string[] files = Directory.GetFiles(Folder, hash + ".*", SearchOption.TopDirectoryOnly);

			foreach (string file in files)
			{
				File.Delete(file);
				m_Logger.DebugFormat("Deleted {0}", file);
			}
		}

		#endregion
	}
}
