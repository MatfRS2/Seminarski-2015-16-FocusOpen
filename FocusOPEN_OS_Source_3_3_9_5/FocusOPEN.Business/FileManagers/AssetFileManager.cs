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
using System.Linq;
using System.Reflection;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	/// <summary>
	/// The AssetFileManager class handles all interaction between files and disk.  It saves
	/// files to disk, retrieves file paths, file urls, checks mimetypes, etc.
	/// </summary>
	public static class AssetFileManager
	{
		#region Private variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly List<string> m_IndexableExtensions = new List<string>();

		#endregion

		#region Constructor

		static AssetFileManager()
		{
			// TODO: Make this configurable
			m_IndexableExtensions.Add("doc");
			m_IndexableExtensions.Add("docx");
			m_IndexableExtensions.Add("dot");
			m_IndexableExtensions.Add("rtf");
			m_IndexableExtensions.Add("xls");
			m_IndexableExtensions.Add("xlsx");
			m_IndexableExtensions.Add("ppt");
			m_IndexableExtensions.Add("pptx");
			m_IndexableExtensions.Add("pdf");
			m_IndexableExtensions.Add("htm");
			m_IndexableExtensions.Add("html");
			m_IndexableExtensions.Add("txt");
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Boolean value specifying whether a zipped copy should be created
		/// of asset files when they are uploaded.
		/// </summary>
		public static bool ZipAssetFiles { get; set; }

		#endregion

		#region Private Helper Methods

		/// <summary>
		/// Stamps the asset thumbnail.
		/// </summary>
		private static void StampThumbnail(Asset asset)
		{
			AssetThumbnailInfo info = new AssetThumbnailInfo(asset);

			if (!info.FileExists)
				return;

			if (!AssetTypeChecker.IsImage(StringUtils.GetFileExtension(info.FilePath)))
				return;

			string description = asset.AssetType.Name.Replace(" ", string.Empty);
			string filename = string.Format("{0}.gif", description);
			string path = Path.Combine(Settings.ImageFolder, "Asset/Stamp_Icons/");
			string stamp = Path.Combine(path, filename);

			if (!File.Exists(stamp))
				return;

			ImageUtils.StampImage(info.FilePath, stamp);
		}

		/// <summary>
		/// Saves the posted file to disk and adds its mimetype
		/// to the mimetype manager if it does not exist already
		/// </summary>
		private static void SavePostedFile(BinaryFile file, string path)
		{
			file.SaveAs(path);
			m_Logger.DebugFormat("File '{0}' saved to '{1}'", file.FileName, path);

			MimeTypeManager.AddMimeType(file.FileExtension, file.ContentType);
		}

		/// <summary>
		/// Gets the absolute folder where files of the specified type should be saved for the specified asset
		/// </summary>
		internal static string GetFolder(AssetFilePath path, AssetFileType fileType)
		{
			if (GeneralUtils.ValueIsInList(fileType, AssetFileType.AssetFile, AssetFileType.AssetPreview, AssetFileType.AssetThumbnail))
				return Path.Combine(path.Path, fileType + "s");

			if (GeneralUtils.ValueIsInList(fileType, AssetFileType.AssetFileZipped))
				return Path.Combine(path.Path, "AssetFilesZipped");

			if (GeneralUtils.ValueIsInList(fileType, AssetFileType.AttachedFile))
				throw new SystemException("Attached files are not stored on disk");

			throw new SystemException("Unknown file type: " + fileType);
		}

		/// <summary>
		/// Deletes all files with the specified prefix in the specified folder
		/// </summary>
		private static void DeleteFiles(string prefix, string folder)
		{
			var files = (from path in Directory.GetFiles(folder)
						 where (Path.GetFileName(path) ?? string.Empty).ToLower().StartsWith(prefix)
						 select path).ToList();

			foreach (string file in files)
			{
				m_Logger.DebugFormat("Deleted file: {0}", file);
				File.Delete(file);
			}
		}

		#endregion

		#region Asset Save Functionality

		/// <summary>
		/// Saves the asset file to disk.
		/// Asset previews are automatically resized.
		/// Asset thumbnails are automatically resized and stamped.
		/// </summary>
		public static void SaveAssetFile(Asset asset, BinaryFile file, AssetFileType assetFileType)
		{
			// Check asset
			if (asset == null || asset.IsNull || asset.IsNew)
				throw new InvalidAssetException("Asset is null or new");

			// Check we've got a file
			if (file == null || file.IsEmpty)
				throw new InvalidAssetException("file is empty");

			// Check that we're not trying to save an asset file.  This must be done
			// using a different overload of this method as it has more processes involved.
			if (assetFileType == AssetFileType.AssetFile)
				throw new SystemException("This method cannot be used to save raw asset files; it is designed to save related asset files only");

			// Folder where this file will reside
			string folder = GetFolder(asset.AssetFilePath, assetFileType);

			// Delete the old file
			DeleteFiles(asset.FileReference, folder);

			// Construct path to file to be saved
			string filename = String.Concat(asset.FileReference, ".", file.FileExtension);
			string path = Path.Combine(folder, filename);

			// Save the file
			SavePostedFile(file, path);

			// Resize previews
			if (AssetTypeChecker.IsImage(file.FileExtension) && assetFileType == AssetFileType.AssetPreview)
				ImageUtils.ResizeImage(path, path, 320, 300, true);

			// Resize thumbnails
			if (AssetTypeChecker.IsImage(file.FileExtension) && assetFileType == AssetFileType.AssetThumbnail)
			{
				ImageUtils.ResizeImageDown(path, 100, 100);
				StampThumbnail(asset);
			}
		}

		public static void SaveAssetFile(Asset asset, BinaryFile file, bool sendEmailOnCompletion)
		{
			SaveAssetFile(asset, file, sendEmailOnCompletion, true);
		}

        public static void SaveAssetFile(Asset asset, BinaryFile file, bool notify, bool checkHash)
        {
            SaveAssetFile(asset, file, notify, checkHash, false);
        }

		public static void SaveAssetFile(Asset asset, BinaryFile file, bool notify, bool checkHash, bool doNotProcessForPreview)
		{
			// Quit if empty asset
			if (asset == null || asset.IsNull || asset.IsNew)
				return;

			// Quit if no file was uploaded
			if (file == null || file.IsEmpty)
				return;

			if (checkHash)
			{
				// Check the hash.  Don't allow a processed asset to be re-uploaded
				AssetFinder finder = new AssetFinder {FileHash = file.FileHash, IsProcessed = true};
				Asset assetFromHash = Asset.FindOne(finder);

				if (!assetFromHash.IsNull)
					throw new InvalidAssetFileException(string.Format("{0} is already uploaded and has the Asset ID: {1}", file.FileName, assetFromHash.AssetId));
			}

			m_Logger.DebugFormat("SaveAssetFile - AssetId: {0}", asset.AssetId);

			// Ensure asset has path
			if (asset.AssetFilePath.IsNull)
				asset.AssetFilePathId = AssetFilePathManager.GetDefault().AssetFilePathId.GetValueOrDefault();

			// Delete old files
			foreach (string path in Directory.GetFiles(asset.AssetFilePath.Path, asset.FileReference + ".*", SearchOption.AllDirectories))
				File.Delete(path);

			// Delete any cached files
			string cacheFolder = Path.Combine(Settings.CachedAssetFilesFolder, asset.AssetId.ToString());
			if (Directory.Exists(cacheFolder))
			{
				Directory.Delete(cacheFolder, true);
				m_Logger.DebugFormat("Deleted cache folder: {0}", cacheFolder);
			}

			// Update the asset file path
			asset.AssetFilePathId = AssetFilePathManager.GetDefault().AssetFilePathId.GetValueOrDefault();

			// Update asset file information
			asset.Filename = file.FileName;
			asset.FileSize = file.FileSize;
			asset.FileHash = file.FileHash;

			// The file extension comes from the view but set this for now
			// in case we need it elsewhere in the code before the Asset entity
			// is refreshed from the database.
			asset.FileExtension = file.FileExtension;
            
            // If not processing for Preview the asset.IsProcessed flag should be set
            // depending on the file extension.
            if (!doNotProcessForPreview)
			    asset.IsProcessed = (!APSGateway.Instance.CanProcess(file.FileExtension));

			// Save asset to database
			Asset.Update(asset);
			Asset.SaveAssetMetadata(asset);

			// Construct location to save asset file
			string assetFolder = GetFolder(asset.AssetFilePath, AssetFileType.AssetFile);
			string assetFilePath = Path.Combine(assetFolder, asset.FileReference + "." + file.FileExtension);

			// Save the file.  Don't bother doing a mimetype check here, as the
			// files will be forced to open the download box by using 'application/octet-stream'
			// as the content type, so the server doesn't need to set this value.
			file.SaveAs(assetFilePath);
			m_Logger.DebugFormat("Saved file: {0}", assetFilePath);

			// Send this to the gateway for processing unless the do not process flag is set.
			// When the preview and thumbnail are ready, the APS will callback to the web app to save them.
			if (!doNotProcessForPreview && !asset.IsProcessed)
				APSGateway.Instance.ProcessFile(asset, notify, FileOutputs.All);

			// Regenerate bitmaps.  This submits the assets to the APS and returns.
			// When the files are ready, the APS will callback to the web app to save them.
			AssetBitmapGroupManager.Generate(asset);

			// Check if file indexing is enabled, and if so, read the file contents into a byte
			// array and then store this in the database so that we can search it using full-text
			// indexing.  As we're using an indexed view, this is done using a subquery.
			// See AssetFinder.cs for full details of how this is implemented.
			if (AssetFinder.FileIndexingEnabled && m_IndexableExtensions.Contains(file.FileExtension))
			{
				ByteArray byteArray = ByteArray.New(file.InputStream);
				Asset.AddUpdateFileContents(asset.AssetId.GetValueOrDefault(), file.FileName, AssetFileType.AssetFile, byteArray, false);
			}

			// Now create the zip file if required
			if (ZipAssetFiles)
				AssetFileZipper.CreateZip(asset);
		}

		public static void SaveAttachedFile(Asset asset, BinaryFile file)
		{
			if (!file.IsEmpty)
			{
				ByteArray byteArray = ByteArray.New(file.InputStream);
				Asset.AddUpdateFileContents(asset.AssetId.GetValueOrDefault(), file.FileName, AssetFileType.AttachedFile, byteArray, true);
			}
		}

		#endregion
	}
}