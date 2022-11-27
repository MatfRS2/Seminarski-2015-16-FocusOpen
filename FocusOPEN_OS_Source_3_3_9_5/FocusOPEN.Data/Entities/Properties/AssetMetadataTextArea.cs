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
	public partial class AssetMetadataTextArea : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetMetadataTextAreaId = null;
		
		// Table variables
		protected int m_assetId = 0;
		protected int m_groupNumber = 0;
        protected string m_textAreaValue = string.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetMetadataTextArea object.
		/// </summary>
		public Nullable <Int32> AssetMetadataTextAreaId
		{
			get
			{
				return m_assetMetadataTextAreaId;
			}
			set 
			{
				if (value != m_assetMetadataTextAreaId)
				{
					m_assetMetadataTextAreaId = value;
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
				return (m_assetMetadataTextAreaId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetId of the AssetMetadataTextArea object.
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
		/// Sets or Returns the GroupNumber of the AssetMetadataTextArea object.
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
        /// Sets or Returns the textAreaValue of the AssetMetadataTextArea object.
		/// </summary>
        public virtual string TextAreaValue
		{
			get
			{
                return m_textAreaValue;
			}
			set 
			{
                if (value != m_textAreaValue)
				{
                    m_ChangedProperties["TextAreaValue"] = new ChangedProperty("TextAreaValue", m_textAreaValue, value);

                    m_textAreaValue = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		public enum Columns
		{
			AssetMetadataTextAreaId,
			AssetId,
			GroupNumber,
            TextAreaValue
		}
	}
}

