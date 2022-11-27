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
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Brand.
	/// </summary>
	public partial class Brand
	{
		#region Lazy Loads

		private IEnumerable<BrandMetadataSetting> BrandMetadataSettingList
		{
			get
			{
				return BrandMetadataSettingCache.Instance.GetByBrand(BrandId.GetValueOrDefault());
			}
		}

		#endregion

		public BrandMetadataSetting GetMetadataSetting(string fieldId)
		{
			// First look for a setting for this brand
			foreach (BrandMetadataSetting setting in BrandMetadataSettingList)
				if (StringUtils.IgnoreCaseCompare(setting.FieldId, fieldId))
					return setting;

			// Then check the master brand
			foreach (BrandMetadataSetting setting in BrandCache.Instance.GetMasterBrand().BrandMetadataSettingList)
				if (StringUtils.IgnoreCaseCompare(setting.FieldId, fieldId))
					return setting;

			// If all fails, return an empty object
			return BrandMetadataSetting.Empty;
		}
	    public BrandMetadataSetting GetCustomMetadataSetting(int groupNumber)
	    {
	        return GetMetadataSetting(BrandMetadataSettings.METADATA_PREFIX + groupNumber);
	    }
	}
}