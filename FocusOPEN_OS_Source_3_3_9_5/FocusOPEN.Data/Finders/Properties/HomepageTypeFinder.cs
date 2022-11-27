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
	public partial class HomepageTypeFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_homepageTypeId = null;
		
		// Table columns
		protected string m_description = String.Empty;
		protected string m_shortName = String.Empty;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_homepageTypeIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the HomepageType object.
		/// </summary>
		public Nullable <Int32> HomepageTypeId
		{
			get
			{
				return m_homepageTypeId;
			}
			set
			{
				if (value != m_homepageTypeId)
				{
					m_homepageTypeId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Description
		{
			get
			{
				return m_description;
			}
			set
			{
				if (value != m_description)
				{
					m_description = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string ShortName
		{
			get
			{
				return m_shortName;
			}
			set
			{
				if (value != m_shortName)
				{
					m_shortName = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> HomepageTypeIdList
		{
			get
			{
				return m_homepageTypeIdList;
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
				return "[HomepageType]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (HomepageTypeIdList != null && HomepageTypeIdList.Count > 0)
			{
				JoinableList list = new JoinableList(HomepageTypeIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", HomepageType.Columns.HomepageTypeId));
			}
			
			if (HomepageTypeId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@homepageTypeId", HomepageType.Columns.HomepageTypeId));
				sb.AddDataParameter("@homepageTypeId", HomepageTypeId.Value);
			}
			
			if (Description != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@description", HomepageType.Columns.Description));
				sb.AddDataParameter("@description", Description);
			}						
	
			if (ShortName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@shortName", HomepageType.Columns.ShortName));
				sb.AddDataParameter("@shortName", ShortName);
			}						
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}