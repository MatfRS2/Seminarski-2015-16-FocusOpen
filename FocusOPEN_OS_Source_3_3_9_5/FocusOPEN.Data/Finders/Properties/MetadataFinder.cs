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
	public partial class MetadataFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_metadataId = null;
		
		// Table columns
		protected Nullable <Int32> m_brandId = null;
		protected Nullable <Int32> m_parentMetadataId = null;
		protected string m_name = String.Empty;
		protected string m_externalRef = String.Empty;
		protected string m_synonyms = String.Empty;
		protected int m_groupNumber = 0;
		protected Nullable <Boolean> m_isDeleted = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_metadataIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Metadata object.
		/// </summary>
		public Nullable <Int32> MetadataId
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

		public Nullable <Int32> BrandId
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

		public Nullable <Int32> ParentMetadataId
		{
			get
			{
				return m_parentMetadataId;
			}
			set
			{
				if (value != m_parentMetadataId)
				{
					m_parentMetadataId = value;
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
		
		public List<Int32> MetadataIdList
		{
			get
			{
				return m_metadataIdList;
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
				return "[v_Metadata]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (MetadataIdList != null && MetadataIdList.Count > 0)
			{
				JoinableList list = new JoinableList(MetadataIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Metadata.Columns.MetadataId));
			}
			
			if (MetadataId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@metadataId", Metadata.Columns.MetadataId));
				sb.AddDataParameter("@metadataId", MetadataId.Value);
			}
			
			if (BrandId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", Metadata.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId.Value);
			}
	
			if (ParentMetadataId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@parentMetadataId", Metadata.Columns.ParentMetadataId));
				sb.AddDataParameter("@parentMetadataId", ParentMetadataId.Value);
			}
	
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Metadata.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (ExternalRef != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@externalRef", Metadata.Columns.ExternalRef));
				sb.AddDataParameter("@externalRef", ExternalRef);
			}						
	
			if (Synonyms != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@synonyms", Metadata.Columns.Synonyms));
				sb.AddDataParameter("@synonyms", Synonyms);
			}						
	
			if (GroupNumber != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@groupNumber", Metadata.Columns.GroupNumber));
				sb.AddDataParameter("@groupNumber", GroupNumber);
			}
	
			if (IsDeleted.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDeleted", Metadata.Columns.IsDeleted));
				sb.AddDataParameter("@isDeleted", SqlUtils.BitValue(IsDeleted.Value));
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}