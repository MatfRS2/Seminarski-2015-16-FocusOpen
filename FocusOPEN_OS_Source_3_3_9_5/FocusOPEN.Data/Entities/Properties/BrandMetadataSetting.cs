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
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class BrandMetadataSetting : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_brandMetadataSettingId = null;
		
		// Table variables
		protected int m_brandId = 0;
		protected string m_fieldId = String.Empty;
		protected string m_fieldName = String.Empty;
		protected bool m_isRequired = false;
		protected bool m_onAssetForm = false;
		protected bool m_onAssetDetail = false;
		protected string m_additionalCopy = String.Empty;
		protected string m_toolTip = String.Empty;
		protected bool m_isCustom = false;
        protected int m_uiControlType = 0;
        protected int m_catalogueOrderNum = 0;
        protected int m_assetDetailOrderNum = 0;

		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the BrandMetadataSetting object.
		/// </summary>
		public Nullable <Int32> BrandMetadataSettingId
		{
			get
			{
				return m_brandMetadataSettingId;
			}
			set 
			{
				if (value != m_brandMetadataSettingId)
				{
					m_brandMetadataSettingId = value;
					m_isDirty = true;
				}
			}
		}
		
		public Dictionary<String, ChangedProperty> ChangedProperties
		{
			get
			{
				return m_ChangedProperties;
			}
		}

		public override bool IsNew
		{
			get
			{
				return (m_brandMetadataSettingId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the BrandId of the BrandMetadataSetting object.
		/// </summary>
		public virtual int BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{ 
				if (value != m_brandId)
				{
					m_ChangedProperties["BrandId"] = new ChangedProperty("BrandId", m_brandId, value);
					
					m_brandId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FieldId of the BrandMetadataSetting object.
		/// </summary>
		public virtual string FieldId
		{
			get
			{
				return m_fieldId;
			}
			set 
			{ 
				if (value != m_fieldId)
				{
					m_ChangedProperties["FieldId"] = new ChangedProperty("FieldId", m_fieldId, value);
					
					m_fieldId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FieldName of the BrandMetadataSetting object.
		/// </summary>
		public virtual string FieldName
		{
			get
			{
				return m_fieldName;
			}
			set 
			{ 
				if (value != m_fieldName)
				{
					m_ChangedProperties["FieldName"] = new ChangedProperty("FieldName", m_fieldName, value);
					
					m_fieldName = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsRequired of the BrandMetadataSetting object.
		/// </summary>
		public virtual bool IsRequired
		{
			get
			{
				return m_isRequired;
			}
			set 
			{ 
				if (value != m_isRequired)
				{
					m_ChangedProperties["IsRequired"] = new ChangedProperty("IsRequired", m_isRequired, value);
					
					m_isRequired = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OnAssetForm of the BrandMetadataSetting object.
		/// </summary>
		public virtual bool OnAssetForm
		{
			get
			{
				return m_onAssetForm;
			}
			set 
			{ 
				if (value != m_onAssetForm)
				{
					m_ChangedProperties["OnAssetForm"] = new ChangedProperty("OnAssetForm", m_onAssetForm, value);
					
					m_onAssetForm = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OnAssetDetail of the BrandMetadataSetting object.
		/// </summary>
		public virtual bool OnAssetDetail
		{
			get
			{
				return m_onAssetDetail;
			}
			set 
			{ 
				if (value != m_onAssetDetail)
				{
					m_ChangedProperties["OnAssetDetail"] = new ChangedProperty("OnAssetDetail", m_onAssetDetail, value);
					
					m_onAssetDetail = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AdditionalCopy of the BrandMetadataSetting object.
		/// </summary>
		public virtual string AdditionalCopy
		{
			get
			{
				return m_additionalCopy;
			}
			set 
			{ 
				if (value != m_additionalCopy)
				{
					m_ChangedProperties["AdditionalCopy"] = new ChangedProperty("AdditionalCopy", m_additionalCopy, value);
					
					m_additionalCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ToolTip of the BrandMetadataSetting object.
		/// </summary>
		public virtual string ToolTip
		{
			get
			{
				return m_toolTip;
			}
			set 
			{ 
				if (value != m_toolTip)
				{
					m_ChangedProperties["ToolTip"] = new ChangedProperty("ToolTip", m_toolTip, value);
					
					m_toolTip = value; 
					m_isDirty = true;
				}
			}
		}

        /// <summary>
        /// Sets or Returns the IsCustom of the BrandMetadataSetting object.
        /// </summary>
        public virtual bool IsCustom
        {
            get
            {
                return m_isCustom;
            }
            set
            {
                if (value != m_isCustom)
                {
                    m_ChangedProperties["IsCustom"] = new ChangedProperty("IsCustom", m_isCustom, value);

                    m_isCustom = value;
                    m_isDirty = true;
                }
            }
        }

        /// <summary>
        /// Sets or Returns the UiControlType of the BrandMetadataSetting object.
        /// </summary>
        public virtual int UiControlType
        {
            get
            {
                return m_uiControlType;
            }
            set
            {
                if (value != m_uiControlType)
                {
                    m_ChangedProperties["UiControlType"] = new ChangedProperty("UiControlType", m_uiControlType, value);

                    m_uiControlType = value;
                    m_isDirty = true;
                }
            }
        }
        
        /// <summary>
        /// Sets or Returns the CatalogueOrderNum of the BrandMetadataSetting object.
        /// </summary>
        public virtual int CatalogueOrderNum
        {
            get
            {
                return m_catalogueOrderNum;
            }
            set
            {
                if (value != m_catalogueOrderNum)
                {
                    m_ChangedProperties["CatalogueOrderNum"] = new ChangedProperty("CatalogueOrderNum", m_catalogueOrderNum, value);

                    m_catalogueOrderNum = value;
                    m_isDirty = true;
                }
            }
        }
        
        /// <summary>
        /// Sets or Returns the AssetDetailOrderNum of the BrandMetadataSetting object.
        /// </summary>
        public virtual int AssetDetailOrderNum
        {
            get
            {
                return m_assetDetailOrderNum;
            }
            set
            {
                if (value != m_assetDetailOrderNum)
                {
                    m_ChangedProperties["AssetDetailOrderNum"] = new ChangedProperty("AssetDetailOrderNum", m_assetDetailOrderNum, value);

                    m_assetDetailOrderNum = value;
                    m_isDirty = true;
                }
            }
        }

		#endregion
		
		
		public enum Columns
		{
			BrandMetadataSettingId,
			BrandId,
			FieldId,
			FieldName,
			IsRequired,
			OnAssetForm,
			OnAssetDetail,
			AdditionalCopy,
			ToolTip,
			AllowMultiple,
            UiControlType,
            IsCustom,
            IsEnabled,
            CatalogueOrderNum,
            AssetDetailOrderNum,

        }
	}
}

