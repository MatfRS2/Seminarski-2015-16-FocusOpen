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
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a BrandMetadataSetting.
	/// </summary>
	public partial class BrandMetadataSetting
	{
        private BrandMetadataSelectableSetting m_BrandMetadataSelectableSetting;

        /// <summary>
        /// returns the selectable setting of the brand metadata setting, if ui control type is not 
        /// selectable or the selectable setting had not been initialised (i.e. there is no record in the database) 
        /// then it returns a null.
        /// </summary>
        public BrandMetadataSelectableSetting SelectableSetting
        {
            get
            {
                //metadata is not selectable - no setting should be returned then
                if (UiControlType != (int)BrandMetadataUiControlType.Select) return NullBrandMetadataSelectableSetting.Instance;
                
                //new brandmetasetting - no selectable setting yet
                if (!BrandMetadataSettingId.HasValue) return NullBrandMetadataSelectableSetting.Instance;

                //lazy load the setting from the database
                if(m_BrandMetadataSelectableSetting == null)
                {
                    var finder = new BrandMetadataSelectableSettingFinder() {BrandMetadataSettingId = BrandMetadataSettingId.Value};
                    m_BrandMetadataSelectableSetting = BrandMetadataSelectableSetting.FindOne(finder);
                }

                return m_BrandMetadataSelectableSetting;
            }
            set { m_BrandMetadataSelectableSetting = value; }
        }
        /// <summary>
        /// overrides the default logic where the selecatble setting is retrieved from the database. this way a selectable setting
        /// that is not yet persisted to the database can be used instead for example for previewing purposes
        /// </summary>
        /// <param name="setting"></param>
        public void SetSelectableSetting(BrandMetadataSelectableSetting setting)
        {
            m_BrandMetadataSelectableSetting = setting;
        }
        /// <summary>
        /// returns the group number for custom metadata and -1 for the rest
        /// </summary>
	    public int GroupNumber
	    {
	        get
	        {
                if (!IsCustom) return -1;

	            int grp;

	            var ok = int.TryParse(FieldId.ToLower().Replace(BrandMetadataSettings.METADATA_PREFIX.ToLower(), ""), out grp);

	            return ok ? grp : -1;
	        }
	    }

	    
	}
}
