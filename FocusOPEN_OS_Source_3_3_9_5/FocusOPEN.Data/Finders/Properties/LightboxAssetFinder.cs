/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
	public partial class LightboxAssetFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxAssetId = null;
		
		// Table columns
		protected int m_lightboxId = 0;
		protected int m_assetId = 0;
		protected string m_notes = String.Empty;
		protected DateTime m_createDate = DateTime.MinValue;
		protected Nullable <Int32> m_orderNumber = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_lightboxAssetIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxAsset object.
		/// </summary>
		public Nullable <Int32> LightboxAssetId
		{
			get
			{
				return m_lightboxAssetId;
			}
			set
			{
				if (value != m_lightboxAssetId)
				{
					m_lightboxAssetId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int LightboxId
		{
			get
			{
				return m_lightboxId;
			}
			set
			{
				if (value != m_lightboxId)
				{
					m_lightboxId = value;
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

		public string Notes
		{
			get
			{
				return m_notes;
			}
			set
			{
				if (value != m_notes)
				{
					m_notes = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set
			{
				if (value != m_createDate)
				{
					m_createDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> OrderNumber
		{
			get
			{
				return m_orderNumber;
			}
			set
			{
				if (value != m_orderNumber)
				{
					m_orderNumber = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> LightboxAssetIdList
		{
			get
			{
				return m_lightboxAssetIdList;
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
				return "[v_LightboxAsset]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (LightboxAssetIdList != null && LightboxAssetIdList.Count > 0)
			{
				JoinableList list = new JoinableList(LightboxAssetIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", LightboxAsset.Columns.LightboxAssetId));
			}
			
			if (LightboxAssetId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxAssetId", LightboxAsset.Columns.LightboxAssetId));
				sb.AddDataParameter("@lightboxAssetId", LightboxAssetId.Value);
			}
			
			if (LightboxId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxId", LightboxAsset.Columns.LightboxId));
				sb.AddDataParameter("@lightboxId", LightboxId);
			}
	
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", LightboxAsset.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (Notes != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@notes", LightboxAsset.Columns.Notes));
				sb.AddDataParameter("@notes", Notes);
			}						
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", LightboxAsset.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			if (OrderNumber != null)
			{
				sb.Criteria.Add(string.Format("{0}=@orderNumber", LightboxAsset.Columns.OrderNumber));
				sb.AddDataParameter("@orderNumber", OrderNumber.Value);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}