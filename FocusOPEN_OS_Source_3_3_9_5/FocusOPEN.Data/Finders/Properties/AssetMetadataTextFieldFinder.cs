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
	public partial class AssetMetadataTextFieldFinder : AbstractFinder
	{
        #region Column Variables

        // Primary Key
        protected Nullable<Int32> m_assetMetadataTextFieldId = null;

        // Table columns
        protected int m_assetId = 0;
        protected int m_groupNumber = 0;

        #endregion

        #region Other Private Variables

        private readonly List<Int32> m_assetMetadataTextFieldIdList = new List<Int32>();

        // Count of number of find properties set
        protected int m_FindCriteriaCount = 0;

        #endregion

        #region Simple Accessors

        /// <summary>
        /// Returns the Id of the AssetMetadataTextField object.
        /// </summary>
        public Nullable<Int32> AssetMetadataTextFieldId
        {
            get
            {
                return m_assetMetadataTextFieldId;
            }
            set
            {
                if (value != m_assetMetadataTextFieldId)
                {
                    m_assetMetadataTextFieldId = value;
                    m_FindCriteriaCount++;
                }
            }
        }

        public int AssetId
        {
            get
            {
                return m_assetId;
            }
            set
            {
                if (value != m_assetId)
                {
                    m_assetId = value;
                    m_FindCriteriaCount++;
                }
            }
        }

        public int GroupNumber
        {
            get
            {
                return m_groupNumber;
            }
            set
            {
                if (value != m_groupNumber)
                {
                    m_groupNumber = value;
                    m_FindCriteriaCount++;
                }
            }
        }

        public List<Int32> AssetMetadataTextFieldIdList
        {
            get
            {
                return m_assetMetadataTextFieldIdList;
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
                return "[AssetMetadataTextField]";
            }
        }

        protected override void SetSearchCriteria(ref SearchBuilder sb)
        {
            if (AssetMetadataTextFieldIdList != null && AssetMetadataTextFieldIdList.Count > 0)
            {
                JoinableList list = new JoinableList(AssetMetadataTextFieldIdList);
                sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetMetadataTextField.Columns.AssetMetadataTextFieldId));
            }

            if (AssetMetadataTextFieldId.HasValue)
            {
                sb.Criteria.Add(string.Format("{0}=@assetMetadataTextFieldId", AssetMetadataTextField.Columns.AssetMetadataTextFieldId));
                sb.AddDataParameter("@assetMetadataTextFieldId", AssetMetadataTextFieldId.Value);
            }

            if (AssetId != 0)
            {
                sb.Criteria.Add(string.Format("{0}=@assetId", AssetMetadataTextField.Columns.AssetId));
                sb.AddDataParameter("@assetId", AssetId);
            }

            if (GroupNumber != 0)
            {
                sb.Criteria.Add(string.Format("{0}=@groupNumber", AssetMetadataTextField.Columns.GroupNumber));
                sb.AddDataParameter("@groupNumber", GroupNumber);
            }

            SetCustomSearchCriteria(ref sb);
        }

        #endregion
	}
}