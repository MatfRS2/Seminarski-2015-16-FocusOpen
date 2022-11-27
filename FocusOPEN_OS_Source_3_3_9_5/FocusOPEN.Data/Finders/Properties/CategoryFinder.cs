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
	public partial class CategoryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_categoryId = null;
		
		// Table columns
		protected Nullable <Int32> m_parentCategoryId = null;
		protected int m_brandId = 0;
		protected string m_name = String.Empty;
		protected string m_externalRef = String.Empty;
		protected string m_message = String.Empty;
		protected string m_synonyms = String.Empty;
		protected int m_ownerUserId = 0;
		protected int m_categoryOrder = 0;
		protected int m_fullAssetCount = 0;
		protected int m_availableAssetCount = 0;
		
		// View Variables
		protected string m_ownerEmail = String.Empty;
		protected string m_ownerName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_categoryIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Category object.
		/// </summary>
		public Nullable <Int32> CategoryId
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

		public Nullable <Int32> ParentCategoryId
		{
			get
			{
				return m_parentCategoryId;
			}
			set
			{
				if (value != m_parentCategoryId)
				{
					m_parentCategoryId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int BrandId
		{
			get
			{
				return m_brandId;
			}
			set
			{
				if (value != m_brandId)
				{
					m_brandId = value;
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

		public string ExternalRef
		{
			get
			{
				return m_externalRef;
			}
			set
			{
				if (value != m_externalRef)
				{
					m_externalRef = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Message
		{
			get
			{
				return m_message;
			}
			set
			{
				if (value != m_message)
				{
					m_message = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Synonyms
		{
			get
			{
				return m_synonyms;
			}
			set
			{
				if (value != m_synonyms)
				{
					m_synonyms = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int OwnerUserId
		{
			get
			{
				return m_ownerUserId;
			}
			set
			{
				if (value != m_ownerUserId)
				{
					m_ownerUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int CategoryOrder
		{
			get
			{
				return m_categoryOrder;
			}
			set
			{
				if (value != m_categoryOrder)
				{
					m_categoryOrder = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int FullAssetCount
		{
			get
			{
				return m_fullAssetCount;
			}
			set
			{
				if (value != m_fullAssetCount)
				{
					m_fullAssetCount = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AvailableAssetCount
		{
			get
			{
				return m_availableAssetCount;
			}
			set
			{
				if (value != m_availableAssetCount)
				{
					m_availableAssetCount = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> CategoryIdList
		{
			get
			{
				return m_categoryIdList;
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
		
		public virtual string OwnerEmail
		{
			get
			{
				return m_ownerEmail;
			}
			set 
			{ 
				m_ownerEmail = value; 
			}
		}
		public virtual string OwnerName
		{
			get
			{
				return m_ownerName;
			}
			set 
			{ 
				m_ownerName = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_Category]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (CategoryIdList != null && CategoryIdList.Count > 0)
			{
				JoinableList list = new JoinableList(CategoryIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Category.Columns.CategoryId));
			}
			
			if (CategoryId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@categoryId", Category.Columns.CategoryId));
				sb.AddDataParameter("@categoryId", CategoryId.Value);
			}
			
			if (ParentCategoryId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@parentCategoryId", Category.Columns.ParentCategoryId));
				sb.AddDataParameter("@parentCategoryId", ParentCategoryId.Value);
			}
	
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", Category.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Category.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (ExternalRef != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@externalRef", Category.Columns.ExternalRef));
				sb.AddDataParameter("@externalRef", ExternalRef);
			}						
	
			if (Message != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@message", Category.Columns.Message));
				sb.AddDataParameter("@message", Message);
			}						
	
			if (Synonyms != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@synonyms", Category.Columns.Synonyms));
				sb.AddDataParameter("@synonyms", Synonyms);
			}						
	
			if (OwnerUserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@ownerUserId", Category.Columns.OwnerUserId));
				sb.AddDataParameter("@ownerUserId", OwnerUserId);
			}
	
			if (CategoryOrder != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@categoryOrder", Category.Columns.CategoryOrder));
				sb.AddDataParameter("@categoryOrder", CategoryOrder);
			}
	
			if (FullAssetCount != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@fullAssetCount", Category.Columns.FullAssetCount));
				sb.AddDataParameter("@fullAssetCount", FullAssetCount);
			}
	
			if (AvailableAssetCount != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@availableAssetCount", Category.Columns.AvailableAssetCount));
				sb.AddDataParameter("@availableAssetCount", AvailableAssetCount);
			}
	
			if (OwnerEmail != String.Empty)
			{
				sb.Criteria.Add("OwnerEmail=@ownerEmail");
				sb.AddDataParameter("@ownerEmail", OwnerEmail);
			}						

			if (OwnerName != String.Empty)
			{
				sb.Criteria.Add("OwnerName=@ownerName");
				sb.AddDataParameter("@ownerName", OwnerName);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}