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
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	public static class AssetFilePathManager
	{
		#region Fields

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly List<string> m_RequiredSubFolders = new List<string> { "AssetBitmapGroups", "AssetFiles", "AssetFilesZipped", "AssetPreviews", "AssetThumbnails", "CachedAssetFiles", "Homepage", "Temp" };

		#endregion

		#region Public Methods

		/// <summary>
		/// Gets list of asset file paths
		/// </summary>
		public static EntityList<AssetFilePath> GetList()
		{
			AssetFilePathFinder finder = new AssetFilePathFinder();
			return AssetFilePath.FindMany(finder);
		}

		/// <summary>
		/// Get the default asset file path
		/// </summary>
		public static AssetFilePath GetDefault()
		{
			AssetFilePath afp = AssetFilePathCache.Instance.GetList().Find(o => o.IsDefault) ?? AssetFilePath.Empty;

			if (afp.IsNull)
				throw new SystemException("No default asset file path specified.  Please ensure that the default asset file path in the database has 'IsDefault' set to true");

			return afp;
		}

		/// <summary>
		/// Set the default asset file path to that with the specified id
		/// All other asset file paths are marked as not default as there can only be one
		/// default asset file path at any time
		/// </summary>
		public static void SetDefault(int assetFilePathId)
		{
			foreach (var afp in GetList())
			{
				afp.IsDefault = (afp.AssetFilePathId == assetFilePathId);
				AssetFilePath.Update(afp);
			}

			CacheManager.InvalidateCache("AssetFilePath", CacheType.All);
		}

		/// <summary>
		/// Creates a new asset file path with the specified path
		/// </summary>
		public static void CreateNew(string path)
		{
			AssetFilePath afp = AssetFilePath.New();
			afp.Path = path;
			afp.IsDefault = false;
			Save(afp);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Validate and save the asset file path to the database
		/// Also create require subfolders if necessary
		/// </summary>
		private static void Save(AssetFilePath assetFilePath)
		{
			ValidateAssetFilePathWithException(assetFilePath);
			AssetFilePath.Update(assetFilePath);

			foreach (string subfolder in m_RequiredSubFolders)
			{
				string fullpath = Path.Combine(assetFilePath.Path, subfolder);

				if (!Directory.Exists(fullpath))
				{
					try
					{
						Directory.CreateDirectory(fullpath);
						m_Logger.InfoFormat("Created directory: {0}", fullpath);
					}
					catch (Exception ex)
					{
						m_Logger.Warn(string.Format("Unable to create folder: {0}.  Error : {1}", fullpath, ex.Message), ex);
					}
				}
			}

			CacheManager.InvalidateCache("AssetFilePath", CacheType.All);
		}

		#endregion

		#region Validation

		private static void ValidateAssetFilePathWithException(AssetFilePath assetFilePath)
		{
			ErrorList errors = ValidateAssetFilePath(assetFilePath);

			if (errors.Count > 0)
				throw new InvalidAssetFilePathException(errors, assetFilePath);
		}

		private static ErrorList ValidateAssetFilePath(AssetFilePath assetFilePath)
		{
			ErrorList errors = new ErrorList();

			if (assetFilePath.Path.Trim() == string.Empty)
			{
				errors.Add("Path cannot be empty");
			}
			else if (!Directory.Exists(assetFilePath.Path))
			{
				errors.Add("Path does not exist");
			}
			else
			{
				AssetFilePathFinder finder = new AssetFilePathFinder {Path = assetFilePath.Path.Trim()};
				AssetFilePath afp = AssetFilePath.FindOne(finder);

				if (afp.Path == assetFilePath.Path && afp.AssetFilePathId.GetValueOrDefault(-1) != assetFilePath.AssetFilePathId.GetValueOrDefault(0))
					errors.Add("The specified path already has already been entered");
			}

			return errors;
		}

		#endregion
	}
}