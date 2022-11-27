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

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Used to save a list of assets in memory so that they can be paged through one by one
	/// when cataloging the assets (this is typically used when cataloging many assets together)
	/// </summary>
	[Serializable]
	public class AssetPager
	{
		#region Private variables

		private readonly List<Int32> m_AssetIdList = new List<int>();
		private int m_CurrentAssetId = -1;
		private bool m_UseFirstAssetAsTemplate = false;

		#endregion

		#region Constructors

		public AssetPager()
		{
		}

		public AssetPager(List<int> assetIdList)
		{
			m_AssetIdList = assetIdList;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets list of asset ID's in the pager
		/// </summary>
		public List<int> AssetIdList
		{
			get
			{
				return m_AssetIdList;
			}
		}

		/// <summary>
		/// Gets the current asset ID
		/// </summary>
		public int CurrentAssetId
		{
			get
			{
				return m_CurrentAssetId;
			}
			set
			{
				m_CurrentAssetId = value;
			}
		}

		/// <summary>
		/// Gets or sets a boolean value specifying whether the
		/// first asset should be used as the template
		/// </summary>
		public bool UseFirstAssetAsTemplate
		{
			get
			{
				return m_UseFirstAssetAsTemplate;
			}
			set
			{
				m_UseFirstAssetAsTemplate = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Checks if the asset ID list is empty
		/// </summary>
		/// <returns>[True] if empty, otherwise [False]</returns>
		public bool IsEmpty()
		{
			return (m_AssetIdList.Count == 0);
		}

		/// <summary>
		/// Checks if the current asset ID is the first in the list
		/// </summary>
		/// <returns>[True] if first, otherwise [False]</returns>
		public bool IsFirst()
		{
			return (!IsEmpty() && FirstAssetId() == m_CurrentAssetId);
		}

		/// <summary>
		/// Checks if the current asset ID is the last in the list
		/// </summary>
		/// <returns>[True] if last, otherwise [False]</returns>
		public bool IsLast()
		{
			return (!IsEmpty() && LastAssetId() == m_CurrentAssetId);
		}

		/// <summary>
		/// Gets the first asset ID in the list.  If list is empty, returns 0.
		/// </summary>
		public int FirstAssetId()
		{
			if (IsEmpty())
				return 0;

			return m_AssetIdList[0];
		}

		/// <summary>
		/// Gets the last asset ID in the list.  If list is empty, returns 0.
		/// </summary>
		public int LastAssetId()
		{
			if (IsEmpty())
				return 0;

			return m_AssetIdList[m_AssetIdList.Count - 1];
		}

		/// <summary>
		/// Gets the next asset ID in the list.  If list is empty or the
		/// current asset is the last in the list, returns 0.
		/// </summary>
		public int NextAssetId()
		{
			if (IsLast() || IsEmpty())
				return 0;

			for (int i = 0; i < m_AssetIdList.Count; i++)
			{
				if (m_AssetIdList[i] == m_CurrentAssetId)
					return m_AssetIdList[i + 1];
			}

			return 0;
		}

		/// <summary>
		/// Gets the previous asset ID in the list.  If list is empty or the
		/// current asset is the first in the list, returns 0.
		/// </summary>
		public int PreviousAssetId()
		{
			if (IsFirst() || IsEmpty())
				return 0;

			for (int i = 0; i < m_AssetIdList.Count; i++)
			{
				if (m_AssetIdList[i] == m_CurrentAssetId)
					return m_AssetIdList[i - 1];
			}

			return 0;
		}

		#endregion
	}
}