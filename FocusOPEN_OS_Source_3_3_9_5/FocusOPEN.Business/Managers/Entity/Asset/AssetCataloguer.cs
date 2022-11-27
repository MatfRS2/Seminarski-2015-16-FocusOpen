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
using FocusOPEN.Data;

namespace FocusOPEN.Business
{
	public static class AssetCataloguer
	{
		public static void CatalogueAssets(List<Int32> assetIdList, bool useTemplate, int templateId, int assetTypeId)
		{
			if (assetIdList.Count == 0)
				throw new CatalogueAssetsException("No assets have been selected");

			if (!useTemplate)
				return;

			// Missing template ID
			if (templateId == -1)
				throw new CatalogueAssetsException("Invalid or missing template ID");

			Asset templateAsset = Asset.Get(templateId);

			// Ensure template exists
			if (templateAsset.IsNull)
				throw new CatalogueAssetsException(string.Format("No assets exist with ID: {0}", templateId));

			// Everything is okay, so go ahead and merge
			foreach (int assetId in assetIdList)
				AssetManager.MergeForCataloguing(templateAsset, assetId);
		}
	}
}