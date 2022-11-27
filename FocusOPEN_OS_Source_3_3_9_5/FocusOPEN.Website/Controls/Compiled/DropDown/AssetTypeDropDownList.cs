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

namespace FocusOPEN.Website.Controls
{
	public class AssetTypeDropDownList : AbstractDropDownList
	{
        #region Properties

        /// <summary>
		/// Gets or sets a value indicating whether all asset types should be displayed, or only visible types
		/// </summary>
		public bool DisplayAll { get; set; }

		/// <summary>
		/// Show system choose option in dropdown.
		/// </summary>
		public bool ShowSystemChooseOption { get; set; }
        

		#endregion

		#region Fields

		IList<AssetType> m_InternalData = null;

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
            if (m_InternalData == null)
            {
            	m_InternalData = (DisplayAll) ? AssetTypeCache.Instance.GetList() : AssetTypeCache.Instance.GetVisibleAssetTypeList();

                if (ShowSystemChooseOption)
                {
                    AssetType assetType = AssetType.New();
                    assetType.AssetTypeId = Int32.MinValue;
                    assetType.Name = "Let the system choose";
                    m_InternalData.Insert(0, assetType);
                }
            }

            return m_InternalData;
		}

		public override string GetDataTextField()
		{
			return AssetType.Columns.Name.ToString();
		}

		public override string GetDataValueField()
		{
			return AssetType.Columns.AssetTypeId.ToString();
		}

		#endregion
	}
}