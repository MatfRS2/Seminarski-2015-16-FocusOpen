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
	public partial class AssetCategoryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetCategoryId = null;
		
		// Table columns
		protected int m_assetId = 0;
		protected int m_categoryId = 0;
		protected Nullable <Boolean> m_isPrimary = null;
		
		// View Variables
		protected string m_name = String.Empty;
		protected Nullable <Int32> m_parentCategoryId = null;
		protected string m_synonyms = String.Empty;
		protected Nullable <Int32> m_categoryOrder = null;
		protected string m_nameAndSynonyms = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetCategoryIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetCategory object.
		/// </summary>
		public Nullable <Int32> AssetCategoryId
		{
			get
			{
				return m_assetCategoryId;
			}
			set
			{
				if (value != m_assetCategoryId)
				{
					m_assetCategoryId = value;
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

		public int CategoryId
		{
			get
			{
				return m_categoryId;
			}
			set
			{
				if (value != m_categoryId)
				{
					m_categoryId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsPrimary
		{
			get
			{
				return m_isPrimary;
			}
			set
			{
				if (value != m_isPrimary)
				{
					m_isPrimary = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetCategoryIdList
		{
			get
			{
				return m_assetCategoryIdList;
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
		public virtual Nullable <Int32> ParentCategoryId
		{
			get
			{
				return m_parentCategoryId;
			}
			set 
			{ 
				m_parentCategoryId = value; 
			}
		}
		public virtual string Synonyms
		{
			get
			{
				return m_synonyms;
			}
			set 
			{ 
				m_synonyms = value; 
			}
		}
		public virtual Nullable <Int32> CategoryOrder
		{
			get
			{
				return m_categoryOrder;
			}
			set 
			{ 
				m_categoryOrder = value; 
			}
		}
		public virtual string NameAndSynonyms
		{
			get
			{
				return m_nameAndSynonyms;
			}
			set 
			{ 
				m_nameAndSynonyms = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AssetCategory]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetCategoryIdList != null && AssetCategoryIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetCategoryIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetCategory.Columns.AssetCategoryId));
			}
			
			if (AssetCategoryId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetCategoryId", AssetCategory.Columns.AssetCategoryId));
				sb.AddDataParameter("@assetCategoryId", AssetCategoryId.Value);
			}
			
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AssetCategory.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (CategoryId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@categoryId", AssetCategory.Columns.CategoryId));
				sb.AddDataParameter("@categoryId", CategoryId);
			}
	
			if (IsPrimary.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isPrimary", AssetCategory.Columns.IsPrimary));
				sb.AddDataParameter("@isPrimary", SqlUtils.BitValue(IsPrimary.Value));
			}
	
			if (Name != String.Empty)
			{
				sb.Criteria.Add("Name=@name");
				sb.AddDataParameter("@name", Name);
			}						

			if (ParentCategoryId.HasValue)
			{
				if (ParentCategoryId.Value == 0)
				{
					sb.Criteria.Add("ParentCategoryId IS NULL");
				}
				else
				{
					sb.Criteria.Add("ParentCategoryId=@parentCategoryId");
					sb.AddDataParameter("@parentCategoryId", ParentCategoryId.Value);
				}
			}

			if (Synonyms != String.Empty)
			{
				sb.Criteria.Add("Synonyms=@synonyms");
				sb.AddDataParameter("@synonyms", Synonyms);
			}						

			if (CategoryOrder.HasValue)
			{
				if (CategoryOrder.Value == 0)
				{
					sb.Criteria.Add("CategoryOrder IS NULL");
				}
				else
				{
					sb.Criteria.Add("CategoryOrder=@categoryOrder");
					sb.AddDataParameter("@categoryOrder", CategoryOrder.Value);
				}
			}

			if (NameAndSynonyms != String.Empty)
			{
				sb.Criteria.Add("NameAndSynonyms=@nameAndSynonyms");
				sb.AddDataParameter("@nameAndSynonyms", NameAndSynonyms);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}