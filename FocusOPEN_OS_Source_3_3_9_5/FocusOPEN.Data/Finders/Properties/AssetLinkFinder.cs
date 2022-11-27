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
	public partial class AssetLinkFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetLinkId = null;
		
		// Table columns
		protected int m_assetId = 0;
		protected int m_linkedAssetId = 0;
		
		// View Variables
		protected string m_linkedAssetTitle = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetLinkIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetLink object.
		/// </summary>
		public Nullable <Int32> AssetLinkId
		{
			get
			{
				return m_assetLinkId;
			}
			set
			{
				if (value != m_assetLinkId)
				{
					m_assetLinkId = value;
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

		public int LinkedAssetId
		{
			get
			{
				return m_linkedAssetId;
			}
			set
			{
				if (value != m_linkedAssetId)
				{
					m_linkedAssetId = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetLinkIdList
		{
			get
			{
				return m_assetLinkIdList;
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
		
		public virtual string LinkedAssetTitle
		{
			get
			{
				return m_linkedAssetTitle;
			}
			set 
			{ 
				m_linkedAssetTitle = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AssetLink]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetLinkIdList != null && AssetLinkIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetLinkIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetLink.Columns.AssetLinkId));
			}
			
			if (AssetLinkId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetLinkId", AssetLink.Columns.AssetLinkId));
				sb.AddDataParameter("@assetLinkId", AssetLinkId.Value);
			}
			
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AssetLink.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (LinkedAssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@linkedAssetId", AssetLink.Columns.LinkedAssetId));
				sb.AddDataParameter("@linkedAssetId", LinkedAssetId);
			}
	
			if (LinkedAssetTitle != String.Empty)
			{
				sb.Criteria.Add("LinkedAssetTitle=@linkedAssetTitle");
				sb.AddDataParameter("@linkedAssetTitle", LinkedAssetTitle);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}