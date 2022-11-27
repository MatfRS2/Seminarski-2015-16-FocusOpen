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
	public partial class AssetMetadataFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetMetadataId = null;
		
		// Table columns
		protected int m_assetId = 0;
		protected int m_metadataId = 0;
		
		// View Variables
		protected Nullable <Int32> m_brandId = null;
		protected Nullable <Int32> m_groupNumber = null;
		protected string m_name = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetMetadataIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetMetadata object.
		/// </summary>
		public Nullable <Int32> AssetMetadataId
		{
			get
			{
				return m_assetMetadataId;
			}
			set
			{
				if (value != m_assetMetadataId)
				{
					m_assetMetadataId = value;
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

		public int MetadataId
		{
			get
			{
				return m_metadataId;
			}
			set
			{
				if (value != m_metadataId)
				{
					m_metadataId = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetMetadataIdList
		{
			get
			{
				return m_assetMetadataIdList;
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
		
		public virtual Nullable <Int32> BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{ 
				m_brandId = value; 
			}
		}
		public virtual Nullable <Int32> GroupNumber
		{
			get
			{
				return m_groupNumber;
			}
			set 
			{ 
				m_groupNumber = value; 
			}
		}
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set 
			{ 
				m_name = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AssetMetadata]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetMetadataIdList != null && AssetMetadataIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetMetadataIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetMetadata.Columns.AssetMetadataId));
			}
			
			if (AssetMetadataId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetMetadataId", AssetMetadata.Columns.AssetMetadataId));
				sb.AddDataParameter("@assetMetadataId", AssetMetadataId.Value);
			}
			
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AssetMetadata.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (MetadataId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@metadataId", AssetMetadata.Columns.MetadataId));
				sb.AddDataParameter("@metadataId", MetadataId);
			}
	
			if (BrandId.HasValue)
			{
				if (BrandId.Value == 0)
				{
					sb.Criteria.Add("BrandId IS NULL");
				}
				else
				{
					sb.Criteria.Add("BrandId=@brandId");
					sb.AddDataParameter("@brandId", BrandId.Value);
				}
			}

			if (GroupNumber.HasValue)
			{
				if (GroupNumber.Value == 0)
				{
					sb.Criteria.Add("GroupNumber IS NULL");
				}
				else
				{
					sb.Criteria.Add("GroupNumber=@groupNumber");
					sb.AddDataParameter("@groupNumber", GroupNumber.Value);
				}
			}

			if (Name != String.Empty)
			{
				sb.Criteria.Add("Name=@name");
				sb.AddDataParameter("@name", Name);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}