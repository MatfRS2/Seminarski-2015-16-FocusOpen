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
using System.Linq;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class AssetTypeManager
	{
		private static void Validate(AssetType assetType)
		{
			ErrorList errors = new ErrorList();

			if (StringUtils.IsBlank(assetType.Name))
			{
				errors.Add("Name is required");
			}
			else
			{
				// Get asset type matching the name of this asset type
				// so that we can ensure that it's not a duplicate
				AssetType at = (from o in AssetTypeCache.Instance.GetList()
				                where StringUtils.IgnoreCaseCompare(o.Name, assetType.Name)
				                select o).FirstOrDefault() ?? AssetType.Empty;

				if (!at.IsNull)
				{
					if (assetType.IsNew)
					{
						errors.Add("There is already another asset type with the same name");
					}
					else if (!at.AssetTypeId.Equals(assetType.AssetTypeId))
					{
						errors.Add("There is already another asset type with the same name");
					}
				}
			}

			if (errors.Count > 0)
				throw new InvalidAssetTypeException(errors, assetType);
		}

		public static void SaveAssetType(AssetType assetType)
		{
			Validate(assetType);
			AssetType.Update(assetType);
			CacheManager.InvalidateCache("AssetType", CacheType.All);
		}

		public static void DeleteAssetType(int assetTypeId)
		{
			AssetType at = AssetType.Get(assetTypeId);

			if (at.AssetTypeFileExtensionList.Count > 0)
				throw new SystemException("This asset type cannot be deleted as it has file extensions assigned to it");

			AssetFinder finder = new AssetFinder {AssetTypeId = assetTypeId};
			int count = Asset.GetCount(finder);

			if (count > 0)
				throw new SystemException("This asset type cannot be deleted as assets of this type have been uploaded");

			at.IsDeleted = true;
			AssetType.Update(at);
			CacheManager.InvalidateCache("AssetType", CacheType.All);
		}

		/// <summary>
		/// Gets the asset type ID from the file extension.
		/// If no asset type matches, the generic asset type is returned
		/// </summary>
		public static int GetAssetTypeId(string filenameOrExtension)
		{
			string extension = StringUtils.GetFileExtension(filenameOrExtension);

			foreach (AssetType assetType in AssetTypeCache.Instance.GetList())
				if (assetType.FileExtensionList.Contains(extension))
					return assetType.AssetTypeId.GetValueOrDefault();

			foreach (AssetType assetType in AssetTypeCache.Instance.GetList())
				if (assetType.Name.ToLower() == "generic")
					return assetType.AssetTypeId.GetValueOrDefault();

			throw new SystemException("Unable to get asset type id from file extension");
		}

		#region Single AssetType Retrieval

		public static List<AssetType> GetAssetTypeListForFileExtension(string fileExtension)
		{
			return AssetTypeCache.Instance.GetVisibleAssetTypeList().Where(assetType => assetType.FileExtensionList.Contains(fileExtension.ToLower())).ToList();
		}

		#endregion
	}
}