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
	public partial class AssetMetadataTextField : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetMetadataTextFieldId = null;

        // Table variables
        protected int m_assetId = 0;
        protected int m_groupNumber = 0;
        protected string m_textFieldValue = string.Empty;

        // Other variables
        private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();

        #endregion

        #region Standard Accessors

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
                return (m_assetMetadataTextFieldId.HasValue == false);
            }
        }

        #endregion

        #region Table Accessors

        /// <summary>
        /// Sets or Returns the AssetId of the AssetMetadataTextField object.
        /// </summary>
        public virtual int AssetId
        {
            get
            {
                return m_assetId;
            }
            set
            {
                if (value != m_assetId)
                {
                    m_ChangedProperties["AssetId"] = new ChangedProperty("AssetId", m_assetId, value);

                    m_assetId = value;
                    m_isDirty = true;
                }
            }
        }

        /// <summary>
        /// Sets or Returns the GroupNumber of the AssetMetadataTextField object.
        /// </summary>
        public virtual int GroupNumber
        {
            get
            {
                return m_groupNumber;
            }
            set
            {
                if (value != m_groupNumber)
                {
                    m_ChangedProperties["GroupNumber"] = new ChangedProperty("GroupNumber", m_groupNumber, value);

                    m_groupNumber = value;
                    m_isDirty = true;
                }
            }
        }


        /// <summary>
        /// Sets or Returns the textFieldValue of the AssetMetadataTextField object.
        /// </summary>
        public virtual string TextFieldValue
        {
            get
            {
                return m_textFieldValue;
            }
            set
            {
                if (value != m_textFieldValue)
                {
                    m_ChangedProperties["TextFieldValue"] = new ChangedProperty("TextFieldValue", m_textFieldValue, value);

                    m_textFieldValue = value;
                    m_isDirty = true;
                }
            }
        }

        #endregion

        public enum Columns
        {
            AssetMetadataTextFieldId,
            AssetId,
            GroupNumber,
            TextFieldValue
        }
	}
}

