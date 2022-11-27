/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Text.RegularExpressions;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class AssetTypeFileExtensionManager
	{
		public static void Save(AssetTypeFileExtension atfe, BinaryFile iconFile)
		{
			Validate(atfe, iconFile);

			if (iconFile.IsEmpty)
			{
				atfe.IconFilename = string.Empty;
				atfe.IconImage = ByteArray.Empty.ContentBytes;
			}
			else
			{
				atfe.IconFilename = iconFile.FileName;
				atfe.IconImage = ByteArray.New(iconFile.InputStream).ContentBytes;
			}

			AssetTypeFileExtension.Update(atfe);

			CacheManager.InvalidateCache("AssetTypeFileExtension", CacheType.All);
			CacheManager.InvalidateCache("AssetType", CacheType.All);
		}

		private static void Validate(AssetTypeFileExtension atfe, BinaryFile iconFile)
		{
			ErrorList errors = new ErrorList();

			if (StringUtils.IsBlank(atfe.Extension))
			{
				errors.Add("Extension is required");
			}
			else if (atfe.Extension.Length > 4)
			{
				errors.Add("Extension cannot exceed 4 characters");
			}
			else if (!Regex.IsMatch(atfe.Extension, "^[0-9a-z]+$"))
			{
				errors.Add("Extension can only contain letters and numbers");
			}
			else
			{
				AssetTypeFileExtension o = AssetTypeFileExtensionCache.Instance.GetByExtension(atfe.Extension);

				if (atfe.IsNew && !o.IsNull)
					errors.Add("File extension is already assigned to asset type: " + o.AssetType.Name);

				if (!atfe.IsNew && !o.IsNull && !o.AssetTypeFileExtensionId.Equals(atfe.AssetTypeFileExtensionId))
					errors.Add("File extension is already assigned to asset type: " + o.AssetType.Name);
			}

			if (StringUtils.IsBlank(atfe.Name))
				errors.Add("Name is required");

			if (atfe.AssetTypeId == 0)
				errors.Add("Asset Type is required");

			if (!iconFile.IsEmpty && !GeneralUtils.ValueIsInList(iconFile.FileExtension, "gif", "jpg", "png"))
				errors.Add("Icon must be a GIF, JPG or PNG file");

			if (errors.Count > 0)
				throw new AssetTypeFileExtensionException(errors, atfe);
		}

		public static void Delete(AssetTypeFileExtension atfe)
		{
			// First delete the asset type file extension from the database
			AssetTypeFileExtension.Delete(atfe.AssetTypeFileExtensionId);

			// Invalidate caches
			CacheManager.InvalidateCache("AssetTypeFileExtension", CacheType.All);
			CacheManager.InvalidateCache("AssetType", CacheType.All);
		}

		public static void Duplicate(AssetTypeFileExtension atfe)
		{
			// Reset ID to create new record
			atfe.AssetTypeFileExtensionId = null;

			// Set extension to extension prefixed with tilde
			// If we get two people editing extension at same time,
			// this will cause an error as the extension must be unique
			atfe.Extension = "~" + atfe.Extension.Substring(0, 4);

			// Make it invisible for now
			atfe.IsVisible = false;

			// Save it
			AssetTypeFileExtension.Update(atfe);
		}
	}
}