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
	public partial class CountryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_countryId = null;
		
		// Table columns
		protected string m_code = String.Empty;
		protected string m_name = String.Empty;
		protected int m_rank = 0;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_countryIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Country object.
		/// </summary>
		public Nullable <Int32> CountryId
		{
			get
			{
				return m_countryId;
			}
			set
			{
				if (value != m_countryId)
				{
					m_countryId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Code
		{
			get
			{
				return m_code;
			}
			set
			{
				if (value != m_code)
				{
					m_code = value;
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

		public int Rank
		{
			get
			{
				return m_rank;
			}
			set
			{
				if (value != m_rank)
				{
					m_rank = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> CountryIdList
		{
			get
			{
				return m_countryIdList;
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
				return "[Country]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (CountryIdList != null && CountryIdList.Count > 0)
			{
				JoinableList list = new JoinableList(CountryIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Country.Columns.CountryId));
			}
			
			if (CountryId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@countryId", Country.Columns.CountryId));
				sb.AddDataParameter("@countryId", CountryId.Value);
			}
			
			if (Code != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@code", Country.Columns.Code));
				sb.AddDataParameter("@code", Code);
			}						
	
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Country.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (Rank != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@rank", Country.Columns.Rank));
				sb.AddDataParameter("@rank", Rank);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}