using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
    public static class BrandMetadataSettingManager
    {
        /// <summary>
        /// deletes the setting, all stored possible selection values and all selected values for all assets
        /// </summary>
        /// <param name="brandMetadataSettingId">the setting to be deleted</param>
        public static void DeleteSettingAndValues(int brandMetadataSettingId)
        {
            var setting = BrandMetadataSetting.Get(brandMetadataSettingId);

            if(setting == null) return;

            var group = setting.GroupNumber;

            var metaValuesFinder = new MetadataFinder() {GroupNumber = group};
            
            var vals = Metadata.FindMany(metaValuesFinder);

            var assetMetaFinder = new AssetMetadataFinder() { };

            //iterate through all values, delete existing assetmetadata selection and then delete values themselves as well
            foreach (var metaValue in vals)
            {
                assetMetaFinder.MetadataId = metaValue.MetadataId.GetValueOrDefault();
                
                var assetMetaSelections = AssetMetadata.FindMany(assetMetaFinder);

                foreach (var assetMeta in assetMetaSelections)
                {
                    AssetMetadata.Delete(assetMeta.AssetMetadataId);
                }

                Metadata.Delete(metaValue.MetadataId);
            }

            //delete selectable setting if one exists
            if(!setting.SelectableSetting.IsNull && !setting.SelectableSetting.IsNew)
                BrandMetadataSelectableSetting.Delete(setting.SelectableSetting.BrandMetadataSelectableSettingId);

            //delete the setting itself
            BrandMetadataSetting.Delete(brandMetadataSettingId);
        }

        /// <summary>
        /// returns all custom metadata settings for a supplied brand
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static EntityList<BrandMetadataSetting> GetCustomMetadataSettings(int brandId)
        {
            var finder = new BrandMetadataSettingFinder { BrandId = brandId, IsCustom = true };
            var settings = BrandMetadataSetting.FindMany(finder);

            return settings;
        }
    }
}
