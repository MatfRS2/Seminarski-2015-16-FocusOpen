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
	public partial class AssetTypeFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetTypeId = null;
		
		// Table columns
		protected string m_name = String.Empty;
		protected Nullable <Boolean> m_isVisible = null;
		protected Nullable <Boolean> m_isDeleted = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetTypeIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetType object.
		/// </summary>
		public Nullable <Int32> AssetTypeId
		{
			get
			{
				return m_assetTypeId;
			}
			set
			{
				if (value != m_assetTypeId)
				{
					m_assetTypeId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				if (value != m_name)
				{
					m_name = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsVisible
		{
			get
			{
				return m_isVisible;
			}
			set
			{
				if (value != m_isVisible)
				{
					m_isVisible = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set
			{
				if (value != m_isDeleted)
				{
					m_isDeleted = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetTypeIdList
		{
			get
			{
				return m_assetTypeIdList;
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
		
		#region View Accessors
		
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AssetType]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetTypeIdList != null && AssetTypeIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetTypeIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetType.Columns.AssetTypeId));
			}
			
			if (AssetTypeId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetTypeId", AssetType.Columns.AssetTypeId));
				sb.AddDataParameter("@assetTypeId", AssetTypeId.Value);
			}
			
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", AssetType.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (IsVisible.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isVisible", AssetType.Columns.IsVisible));
				sb.AddDataParameter("@isVisible", SqlUtils.BitValue(IsVisible.Value));
			}
	
			if (IsDeleted.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDeleted", AssetType.Columns.IsDeleted));
				sb.AddDataParameter("@isDeleted", SqlUtils.BitValue(IsDeleted.Value));
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}