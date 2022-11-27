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
	public partial class AssetMetadataTextAreaFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetMetadataTextAreaId = null;
		
		// Table columns
		protected int m_assetId = 0;
		protected int m_groupNumber = 0;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetMetadataTextAreaIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
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
		
		public List<Int32> AssetMetadataTextAreaIdList
		{
			get
			{
				return m_assetMetadataTextAreaIdList;
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
				return "[AssetMetadataTextArea]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetMetadataTextAreaIdList != null && AssetMetadataTextAreaIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetMetadataTextAreaIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetMetadataTextArea.Columns.AssetMetadataTextAreaId));
			}
			
			if (AssetMetadataTextAreaId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetMetadataTextAreaId", AssetMetadataTextArea.Columns.AssetMetadataTextAreaId));
				sb.AddDataParameter("@assetMetadataTextAreaId", AssetMetadataTextAreaId.Value);
			}
			
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AssetMetadataTextArea.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (GroupNumber != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@groupNumber", AssetMetadataTextArea.Columns.GroupNumber));
				sb.AddDataParameter("@groupNumber", GroupNumber);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}