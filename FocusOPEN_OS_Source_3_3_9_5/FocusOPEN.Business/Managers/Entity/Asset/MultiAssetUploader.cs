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
using ICSharpCode.SharpZipLib.Zip;
using log4net;

namespace FocusOPEN.Business
{
	/// <summary>
	/// The multi asset uploader can be used to upload both single asset files
	/// and zip files containing multiple asset files.
	/// </summary>
	public class MultiAssetUploader
	{
		#region Events

		/// <summary>
		/// Fired when progress occurs during an asset upload
		/// </summary>
		public event ProgressEventHandler Progress;
		
		/// <summary>
		/// Fired before the asset is saved to the database
		/// </summary>
		public event AssetEventHandler BeforeSave;

		/// <summary>
		/// Fired after the asset is saved to the database
		/// but before the asset file is saved to disk
		/// </summary>
		public event AssetEventHandler AfterSave;

		/// <summary>
		/// Fired before the asset file is saved to disk
		/// </summary>
		public event AssetEventHandler BeforeFileSave;

		/// <summary>
		/// Fired after the asset file is saved to disk
		/// </summary>
		public event AssetEventHandler AfterFileSave;

		/// <summary>
		/// Logs the progress text and fires the progress event
		/// </summary>
		private void OnProgress(string progressText)
		{
			m_Logger.Debug(progressText);

			if (Progress != null)
				Progress(this, new ProgressEventArgs(progressText));
		}

		#endregion

		#region Private variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private List<UploadedAssetResult> m_UploadedAssetResultList;
		private string m_TempFolder;
        
		/// <summary>
        /// Used to hold a list of temporary files and their source path
        /// </summary>
		private readonly Dictionary<string, OriginalLocation> m_TempFiles = new Dictionary<string, OriginalLocation>();

		#endregion

		#region Accessors

		public List<UploadedAssetResult> UploadedAssetResultList
		{
			get
			{
				if (m_UploadedAssetResultList == null)
					m_UploadedAssetResultList = new List<UploadedAssetResult>();

				return m_UploadedAssetResultList;
			}
		}

		/// <summary>
		/// Gets or sets the binary file of the asset being uploaded
		/// </summary>
		public BinaryFile BinaryFile { get; set; }

        /// <summary>
        /// Allows the source location of the asset file to be passed.
        /// </summary>
        public string SourcePath { get; set; }

		/// <summary>
		/// Gets or sets the asset type ID
		/// If this is set to Int32.MinValue, the asset type
		/// will be auto-detected.
		/// </summary>
		public int AssetTypeId { get; set; }


        /// <summary>
        /// Gets or sets the target Category ID
        /// </summary>
        public int TargetCategoryId { get; set; }

        /// <summary>
        /// If true categories will be created based on asset path.
        /// </summary>
        public bool CreateCategorySubFolders { get; set; }

		/// <summary>
		/// Gets or sets the user uploading the asset
		/// </summary>
		public User UploadedBy { get; set; }

		/// <summary>
		/// Gets or sets a boolean flag specifying whether an email notification should be triggered after processing
		/// </summary>
		public bool SendEmailOnCompletion { get; set; }

        /// <summary>
        /// Set this flag to true if you do not want the MultiAssetUploader to process the thumbnail/preview
        /// The Asset.IsProcessed flag will also be set to true.
        /// </summary>
        public bool DoNotProcessForPreview { get; set; }

		/// <summary>
		/// Gets the path to the temp folder where uploaded asset files can be saved
		/// </summary>
		private string TempFolder
		{
			get
			{
				if (string.IsNullOrEmpty(m_TempFolder))
				{
					// The name of the subfolder inside the temp folder where the unzipped files should be saved
					// This is constructed using a range of values to make it unique and identifiable on disk.
					string tempSubFolder = String.Concat(UploadedBy.UserId.GetValueOrDefault(), "_", DateTime.Now.ToString("yyMMddHHmmss"), "_", BusinessHelper.GetCurrentSessionId().Substring(0, 4), "_", Guid.NewGuid().ToString().Replace("-", "").Substring(0, 4));

					// The absolute path to the temp folder
					m_TempFolder = Path.Combine(Settings.TempFolder, tempSubFolder);

					if (!Directory.Exists(m_TempFolder))
					{
						try
						{
							Directory.CreateDirectory(m_TempFolder);
							m_Logger.DebugFormat("- Created user temp folder: {0}", m_TempFolder);
						}
						catch (Exception e)
						{
							m_Logger.ErrorFormat("Error creating temp folder: {0}.  Message: {1}", m_TempFolder, e.Message);
							throw;
						}
					}
				}

				return m_TempFolder;
			}
		}

        /// <summary>
        /// Flag to indicate whether ZIP files should be preserved
        /// </summary>
        public bool PreserveZipFile {get; set; }

		#endregion

		#region Constructor

		public MultiAssetUploader()
		{
			AssetTypeId = Int32.MinValue;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Ensures that all required values have been set.
		/// </summary>
		/// <exception cref="InvalidAssetException">Thrown if required properties for asset upload have not been set</exception>
		private void ValidateRequiredValues()
		{
			ErrorList errors = new ErrorList();

			// Ensure that a file has been uploaded
			if (BinaryFile == null || BinaryFile.IsEmpty)
				errors.Add("Asset file is required");

			// Ensure that the asset type has been selected
			if (AssetTypeId == 0)
				errors.Add("Asset type is required");

			// Ensure that the upload user has been specified
			if (UploadedBy == null || UploadedBy.IsNull)
				errors.Add("System Error: Uploaded user is required");

			if (errors.Count > 0)
				throw new InvalidAssetException(errors, Asset.Empty);
		}

		/// <summary>
		/// Saves the asset into the database, and the file to disk.
		/// </summary>
		private Asset SaveUploadedAsset(BinaryFile file, string sourceFolder)
		{
			// Basic asset values
			Asset asset = Asset.New();
			asset.Filename = file.FileName;
			asset.UploadDate = DateTime.Now;
			asset.UploadedByUserId = UploadedBy.UserId.GetValueOrDefault();
			asset.BrandId = UploadedBy.PrimaryBrandId;
			asset.ContactEmail = UploadedBy.Email;
			asset.UsageRestrictions = UploadedBy.PrimaryBrand.DefaultUsageRestrictionsCopy;
			asset.CopyrightOwner = UploadedBy.CompanyName;
			asset.CreateDate = DateTime.Now;
			asset.LastUpdate = DateTime.Now;

			// Get the asset type ID based on file extension if the type was not specified
			asset.AssetTypeId = (AssetTypeId == Int32.MinValue) ? AssetTypeManager.GetAssetTypeId(file.FileExtension) : AssetTypeId;

			// Set the asset publish status depending on whether the uploading user is required to use workflow
			asset.AssetPublishStatus = (UploadedBy.UseWorkflow) ? AssetPublishStatus.NotApproved : AssetPublishStatus.Approved;

			// Set the asset file path (required for referential integrity in the database)
			asset.AssetFilePathId = AssetFilePathManager.GetDefault().AssetFilePathId.GetValueOrDefault();

			// Default download restrictions
			asset.WatermarkPreview = false;
			asset.InternalUsers_DownloadApprovalRequired = false;
			asset.InternalUsers_HideFromUsers = false;
			asset.ExternalUsers_DownloadApprovalRequired = false;
			asset.ExternalUsers_HideFromUsers = false;

			// Default production year to current year
			asset.ProductionYear = DateTime.Now.Year;

			// Publish year cannot be null, so default to current year
			// and set expiry date to 2 years ahead
			asset.PublishDate = DateTime.Now;
			asset.ExpiryDate = DateTime.Now.AddYears(2);

            // Get the base Category which willl be the default brand category if Specify during cataloguing
            // or the target category if a target category has been passed.
            Category baseCategory;
            int categoryId;

			if (TargetCategoryId == int.MinValue)
			{
				baseCategory = CategoryCache.Instance.GetRootCategory(asset.BrandId);
			}
			else
			{
				baseCategory = CategoryCache.Instance.GetById(TargetCategoryId);
			}

			if (sourceFolder != String.Empty && CreateCategorySubFolders)
			{
				categoryId = CategoryManager.CreateCategoryTreeFromPath(baseCategory.CategoryId, asset.BrandId, sourceFolder, asset.UploadedByUser);
			}
			else
			{
				categoryId = baseCategory.CategoryId.GetValueOrDefault();
			}

			// Set processed flag depending on the DoNotProcessForPreview. If this is true
            // the IsProcessed flag should also be set to true.
            asset.IsProcessed = DoNotProcessForPreview;

			// Assign default categories
			asset.CategoryList.Clear();
            asset.CategoryList.Add(CategoryCache.Instance.GetById(categoryId));
			asset.PrimaryCategoryId = asset.CategoryList[0].CategoryId.GetValueOrDefault();

			OnProgress("Saving asset...");

			if (BeforeSave != null)
				BeforeSave(this, new AssetEventArgs(asset));

			Asset.Update(asset);
			Asset.SaveAssetMetadata(asset);
			OnProgress("Saved with reference: " + asset.AssetId);

			if (AfterSave != null)
				AfterSave(this, new AssetEventArgs(asset));

			if (BeforeFileSave != null)
				BeforeFileSave(this, new AssetEventArgs(asset));

			if (!file.IsEmpty)
			{
				// Hash should only be checked for non super-admins
				bool checkHash = (UploadedBy.UserRole != UserRole.SuperAdministrator);

				// Save the file
				OnProgress("Saving asset file to disk: " + file.FileName);
				AssetFileManager.SaveAssetFile(asset, file, SendEmailOnCompletion, checkHash, DoNotProcessForPreview);
				OnProgress("Saved file '" + file.FileName + "' to disk");

				// Fire post-save events
				if (AfterFileSave != null)
					AfterFileSave(this, new AssetEventArgs(asset));
			}

			m_Logger.DebugFormat("Saved {0} to database, asset ID: {1}", file.FileName, asset.AssetId);

			return asset;
		}

        /// <summary>
        /// Returns a unique filename for the current upload process
        /// if the file is already being processed with the same filename
        /// an incrementing digit value is added to end of the filename.
        /// </summary>
        private string GetUniqueFilepath(string originalPath)
        {
            string newFilename = originalPath;
            int ct = 1;
            while (m_TempFiles.ContainsKey(newFilename))
            {
                newFilename = string.Format("{0}#{1:000}{2}", Path.GetFileNameWithoutExtension(originalPath), ct, Path.GetExtension(originalPath));
                ct++;
            }
            return newFilename;
        }

		private void LogUploadedAsset(Asset asset, string notes)
		{
			AuditLogManager.LogAssetAction(asset, UploadedBy, AuditAssetAction.UploadedAsset, notes);
			AuditLogManager.LogUserAction(UploadedBy, AuditUserAction.UploadAsset, string.Format("Uploaded {0} asset - AssetId: {1}. {2}", asset.AssetType.Name, asset.AssetId, notes));
		}

		/// <summary>
		/// Copies the data in the source stream to the destination
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		private static void CopyStream(Stream source, Stream destination)
		{
			byte[] buffer = new byte[4096];
			int countBytesRead;
			while ((countBytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
			{
				destination.Write(buffer, 0, countBytesRead);
			}
		}

		#endregion

		#region Filename Checkers

		private static bool IsDuplicateFile(ZipFile zipFile, ZipEntry zipEntry, out Asset asset)
		{
			using (Stream stream = zipFile.GetInputStream(zipEntry))
			{
				string hash = FileUtils.GetHash(stream);

				AssetFinder finder = new AssetFinder { FileHash = hash, IsProcessed = true };
				finder.SortExpressions.Add(new DescendingSort("AssetId"));
				asset = Asset.FindOne(finder);

				return (!asset.IsNull);
			}
		}

		/// <summary>
		/// Checks if the path should be ignored
		/// </summary>
		private static bool IgnorePath(string path)
		{
			if (path.ToLower().Contains("__macosx"))
				return true;

			return false;
		}

		/// <summary>
		/// Checks if the file should be ignored based on its filename
		/// </summary>
		private static bool IgnoreFile(string filename)
		{
			if (filename.ToLower() == "thumbs.db")
				return true;

			return false;
		}

		#endregion

		public void Upload()
		{
			ValidateRequiredValues();

			m_Logger.DebugFormat("Uploading file: {0}", BinaryFile.FileName);

			if (BinaryFile.FileExtension == "zip" && !PreserveZipFile)
			{
				m_Logger.Debug("File is zip file");

				// If we're using a zip file, we need to go through the files inside
				// the zip and check them.  Files that are not valid are not unzipped,
				// we just skip over them as there's no point in unzipping files we don't need.

				using (ZipFile zipFile = new ZipFile(BinaryFile.InputStream))
				{
					int fileCount = 0;

					foreach (ZipEntry zipEntry in zipFile)
					{
						// Ignore non-files completely
						if (!zipEntry.IsFile)
						{
							m_Logger.DebugFormat(" - {0} skipped, not a file", zipEntry.Name);
							continue;
						}

						string filename = (Path.GetFileName(zipEntry.Name) ?? string.Empty).ToLower();

						// Ignore blank filenames completely
						if (StringUtils.IsBlank(filename))
						{
							m_Logger.Debug(" - skipped empty filename");
							continue;
						}

						m_Logger.DebugFormat(" - {0} being processed", zipEntry.Name);

						fileCount++;

						// Initialise the uploaded asset result
						UploadedAssetResult uar = new UploadedAssetResult {Filename = filename, ZipFileIndex = zipEntry.ZipFileIndex};

						Asset asset;

						if (IsDuplicateFile(zipFile, zipEntry, out asset))
						{
							uar.FileStatus = FileStatus.DuplicateHash;
							uar.Asset = asset;

							if (UploadedBy.UserRole == UserRole.SuperAdministrator)
							{
								uar.SaveFile = true;
								m_Logger.Debug("  -- File is duplicate hash, but uploader is superadmin so save anyway");
							}
							else
							{
								uar.SaveFile = false;
								m_Logger.Debug("  -- File is ignored, duplicate hash");
							}
						}
						else if (IgnorePath(zipEntry.Name))
						{
							uar.SaveFile = false;
							uar.FileStatus = FileStatus.IgnoredPath;
							m_Logger.Debug("  -- Path is ignored");
						}
						else if (IgnoreFile(filename))
						{
							uar.SaveFile = false;
							uar.FileStatus = FileStatus.IgnoredFile;
							m_Logger.Debug("  -- File is ignored");
						}
						else
						{
							uar.SaveFile = true;
							uar.FileStatus = FileStatus.Processed;
							m_Logger.Debug("  -- File is valid");
						}

						UploadedAssetResultList.Add(uar);
					}

					// Ensure that the zip file has at least one file
					// No point in continuing otherwise.
					if (fileCount == 0)
					{
						m_Logger.Debug("- Zip file does not contain any assets and cannot be processed");
						throw new AssetUploadException("Zip file does not contain any assets");
					}

					foreach (ZipEntry zipEntry in zipFile)
					{
						// Ignore non-files completely
						if (!zipEntry.IsFile)
							continue;

						// Get the filename
						string filename = (Path.GetFileName(zipEntry.Name) ?? string.Empty);

						// Ignore blank filenames completely
						if (StringUtils.IsBlank(filename))
							continue;

						// Parsed filename
						string parsedFilename = (Path.GetFileName(zipEntry.Name) ?? string.Empty).ToLower();

						// Only process the file if SaveFile is true (ie. it meets the save requirements)
						bool process = (UploadedAssetResultList.FindAll(o => o.Filename.ToLower() == parsedFilename && o.SaveFile).Count > 0);

						if (process)
						{
							OnProgress("Unzipping file: " + parsedFilename);

							// Construct the filepath where the file will be unzipped to
							string filePath = GetUniqueFilepath(Path.Combine(TempFolder, filename));

							// Unzip the file
							using (Stream inputStream = zipFile.GetInputStream(zipEntry))
							{
								using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
								{
									CopyStream(inputStream, fileStream);
									fileStream.Close();
								}
							}

							if (CreateCategorySubFolders)
							{
								// Store the temporary filename along with the original path
                                m_TempFiles.Add(filePath, new OriginalLocation { OriginalPath = Path.GetDirectoryName(zipEntry.Name), ZipFileIndex = zipEntry.ZipFileIndex });
							}
							else
							{
                                m_TempFiles.Add(filePath, new OriginalLocation { OriginalPath = String.Empty, ZipFileIndex = zipEntry.ZipFileIndex });
							}

							OnProgress("Unzipped file: " + parsedFilename);
						}
					}
				}
			}
			else
			{
				m_Logger.DebugFormat("Processing single file upload: {0}", BinaryFile.FileName);

				// Single file uploads.  These are managed a bit differently from zip uploads.
				// They are validated and if anything fails, we throw an AssetUpload exception.

				// Ensure that the file isn't one that should be ignored
				if (IgnoreFile(BinaryFile.FileName))
				{
					m_Logger.DebugFormat("File {0} is not a valid asset file", BinaryFile.FileName);
					throw new AssetUploadException(string.Format("The file '{0}' is not a valid asset file", BinaryFile.FileName));
				}

				// Search for assets with the same file hash
				AssetFinder finder = new AssetFinder { FileHash = BinaryFile.FileHash, IsProcessed =  true };
				finder.SortExpressions.Add(new DescendingSort("AssetId"));
				Asset duplicateAsset = Asset.FindOne(finder);

				// If a duplicate asset was found and the uploading user is not a superadmin, halt the upload and don't save the file to disk
				// Include the duplicate asset ID in the error message for information purposes.
				// We allow duplicate uploads from super-admins as we assume they know wha they are doing.
				if (!duplicateAsset.IsNull)
				{
					// Log this duplicate
					m_Logger.DebugFormat("The file {0} has already been uploaded to the system and has the Asset ID: {1}", BinaryFile.FileName, duplicateAsset.AssetId);

					// Halt the upload if the user doing the upload is not a super-admin
					if (UploadedBy.UserRole != UserRole.SuperAdministrator)
					{
						string message = string.Format("This file has already been uploaded to the system and has the ID: {0}", duplicateAsset.AssetId);
						throw new DuplicateHashException(message, BinaryFile, duplicateAsset);
					}
				}

				// Get the filename where this file should be saved to on disk
				string filename = Path.Combine(TempFolder, BinaryFile.FileName);

				// Save the file
				OnProgress("Saving asset file to disk");
				BinaryFile.SaveAs(filename);
                m_TempFiles.Add(filename, new OriginalLocation { OriginalPath = SourcePath, ZipFileIndex = -1 });
				OnProgress("Saved asset file to disk");

				// Set the filestatus based on whether the asset is a duplicate
				FileStatus fileStatus = (!duplicateAsset.IsNull) ? FileStatus.DuplicateHash : FileStatus.Processed;
				
				// Add it to the resut list
				UploadedAssetResult uar = new UploadedAssetResult { Filename = BinaryFile.FileName, FileStatus = fileStatus, SaveFile = true, ZipFileIndex=-1 };
				UploadedAssetResultList.Add(uar);
			}

			// Get all of the uploaded files and upload these to the system
			// foreach (string path in Directory.GetFiles(TempFolder))
            foreach(KeyValuePair<string, OriginalLocation> kvp in m_TempFiles)
			{
				// Save the asset file to the system
				OnProgress("Saving asset");
                OriginalLocation origLocation = (OriginalLocation)kvp.Value;
				// Asset asset = SaveUploadedAsset(new BinaryFile(path, BinaryFile.SaveMode.Move));
                Asset asset = SaveUploadedAsset(new BinaryFile(kvp.Key, BinaryFile.SaveMode.Move), origLocation.OriginalPath);
				OnProgress("Saved asset with ID: " + asset.AssetId);
				
				// Update audit logs
				LogUploadedAsset(asset, string.Format("Uploaded from: {0}", BinaryFile.FileName));

				// Update the uploaded asset result list item with the asset record that was created
				UploadedAssetResult uar;
                if (origLocation.ZipFileIndex != -1)
                    // Original was from a zip file so use the zip file index
                    uar = UploadedAssetResultList.Find(o => o.ZipFileIndex == origLocation.ZipFileIndex && o.SaveFile);
                else
                    // Coming from a folder heirarchy so use filename.
                    uar = UploadedAssetResultList.Find(o => o.Filename.ToLower() == asset.Filename.ToLower() && o.SaveFile);

                uar.Asset = asset;
			}

			try
			{
				OnProgress("Deleting temp folder");
				Directory.Delete(TempFolder, true);
				OnProgress("Deleted temp folder");

				m_Logger.DebugFormat("- Deleted directory: {0}", TempFolder);
			}
			catch (Exception e)
			{
				m_Logger.Warn("Error deleting: " + TempFolder, e);
			}
		}
	}

	public enum FileStatus
	{
		Processed,
		DuplicateHash,
		IgnoredFile,
		IgnoredPath,
		InvalidFile
	}

	public class UploadedAssetResult
	{
		public string Filename { get; set; }
		public FileStatus FileStatus { get; set; }
		public Asset Asset { get; set; }
		internal bool SaveFile { get; set; }
        public long ZipFileIndex { get; set; }
	}

    public class OriginalLocation
    {
        public string OriginalPath { get; set; }
        public long ZipFileIndex { get; set; }
    }

	public class AssetUploadException : Exception
	{
		public AssetUploadException(string message) : base(message)
		{
		}
	}

	public class DuplicateHashException : AssetUploadException
	{
		public BinaryFile BinaryFile { get; private set; }
		public Asset Asset { get; private set; }

		public DuplicateHashException(string message, BinaryFile binaryFile, Asset asset) : base(message)
		{
			BinaryFile = binaryFile;
			Asset = asset;
		}
	}
}