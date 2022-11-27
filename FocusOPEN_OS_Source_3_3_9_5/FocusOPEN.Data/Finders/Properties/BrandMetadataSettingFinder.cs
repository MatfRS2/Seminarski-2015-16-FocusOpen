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
using SearchBuilder=Daydream.Data.SearchBuilder;

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class BrandMetadataSettingFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_brandMetadataSettingId = null;
		
		// Table columns
		protected int m_brandId = 0;
		protected string m_fieldId = String.Empty;
		protected string m_fieldName = String.Empty;
		protected Nullable <Boolean> m_isRequired = null;
		protected Nullable <Boolean> m_onAssetForm = null;
		protected Nullable <Boolean> m_onAssetDetail = null;
		protected string m_additionalCopy = String.Empty;
		protected string m_toolTip = String.Empty;
		protected Nullable <Boolean> m_allowMultiple = null;
        protected int m_uiControlType = 0;
        protected Nullable<Boolean>  m_isCustom = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_brandMetadataSettingIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
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
					m_FindCriteriaCount++;
				}
			}
		}

		public int BrandId
		{
			get
			{
				return m_brandId;
			}
			set
			{
				if (value != m_brandId)
				{
					m_brandId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string FieldId
		{
			get
			{
				return m_fieldId;
			}
			set
			{
				if (value != m_fieldId)
				{
					m_fieldId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string FieldName
		{
			get
			{
				return m_fieldName;
			}
			set
			{
				if (value != m_fieldName)
				{
					m_fieldName = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsRequired
		{
			get
			{
				return m_isRequired;
			}
			set
			{
				if (value != m_isRequired)
				{
					m_isRequired = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> OnAssetForm
		{
			get
			{
				return m_onAssetForm;
			}
			set
			{
				if (value != m_onAssetForm)
				{
					m_onAssetForm = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> OnAssetDetail
		{
			get
			{
				return m_onAssetDetail;
			}
			set
			{
				if (value != m_onAssetDetail)
				{
					m_onAssetDetail = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string AdditionalCopy
		{
			get
			{
				return m_additionalCopy;
			}
			set
			{
				if (value != m_additionalCopy)
				{
					m_additionalCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string ToolTip
		{
			get
			{
				return m_toolTip;
			}
			set
			{
				if (value != m_toolTip)
				{
					m_toolTip = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> AllowMultiple
		{
			get
			{
				return m_allowMultiple;
			}
			set
			{
				if (value != m_allowMultiple)
				{
					m_allowMultiple = value;
					m_FindCriteriaCount++;
				}
			}
		}

        public Nullable<Boolean> IsCustom
        {
            get
            {
                return m_isCustom;
            }
            set
            {
                if (value != m_isCustom)
                {
                    m_isCustom = value;
                    m_FindCriteriaCount++;
                }
            }
        }

        public int UiControlType
        {
            get
            {
                return m_uiControlType;
            }
            set
            {
                if (value != m_uiControlType)
                {
                    m_uiControlType = value;
                    m_FindCriteriaCount++;
                }
            }
        }

		public List<Int32> BrandMetadataSettingIdList
		{
			get
			{
				return m_brandMetadataSettingIdList;
			}
		}
		
		public int FindCriteriaCount
		{
			get
			{
				return m_FindCriteriaCount;
			}
		}
		
		#endregion
		

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[BrandMetadataSetting]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (BrandMetadataSettingIdList != null && BrandMetadataSettingIdList.Count > 0)
			{
				JoinableList list = new JoinableList(BrandMetadataSettingIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", BrandMetadataSetting.Columns.BrandMetadataSettingId));
			}
			
			if (BrandMetadataSettingId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@brandMetadataSettingId", BrandMetadataSetting.Columns.BrandMetadataSettingId));
				sb.AddDataParameter("@brandMetadataSettingId", BrandMetadataSettingId.Value);
			}
			
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", BrandMetadataSetting.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			if (FieldId != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@fieldId", BrandMetadataSetting.Columns.FieldId));
				sb.AddDataParameter("@fieldId", FieldId);
			}						
	
			if (FieldName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@fieldName", BrandMetadataSetting.Columns.FieldName));
				sb.AddDataParameter("@fieldName", FieldName);
			}						
	
			if (IsRequired.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isRequired", BrandMetadataSetting.Columns.IsRequired));
				sb.AddDataParameter("@isRequired", SqlUtils.BitValue(IsRequired.Value));
			}
	
			if (OnAssetForm.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@onAssetForm", BrandMetadataSetting.Columns.OnAssetForm));
				sb.AddDataParameter("@onAssetForm", SqlUtils.BitValue(OnAssetForm.Value));
			}
	
			if (OnAssetDetail.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@onAssetDetail", BrandMetadataSetting.Columns.OnAssetDetail));
				sb.AddDataParameter("@onAssetDetail", SqlUtils.BitValue(OnAssetDetail.Value));
			}
	
			if (AdditionalCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@additionalCopy", BrandMetadataSetting.Columns.AdditionalCopy));
				sb.AddDataParameter("@additionalCopy", AdditionalCopy);
			}						
	
			if (ToolTip != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@toolTip", BrandMetadataSetting.Columns.ToolTip));
				sb.AddDataParameter("@toolTip", ToolTip);
			}						
	
			if (AllowMultiple.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@allowMultiple", BrandMetadataSetting.Columns.AllowMultiple));
				sb.AddDataParameter("@allowMultiple", SqlUtils.BitValue(AllowMultiple.Value));
			}
            
            if (IsCustom.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isCustom", BrandMetadataSetting.Columns.IsCustom));
                sb.AddDataParameter("@isCustom", SqlUtils.BitValue(IsCustom.Value));
			}

            if (UiControlType != 0)
            {
                sb.Criteria.Add(string.Format("{0}=@uiControlType", BrandMetadataSetting.Columns.UiControlType));
                sb.AddDataParameter("@uiControlType", UiControlType);
            }

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}