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
	public partial class HomepageFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_homepageId = null;
		
		// Table columns
		protected int m_brandId = 0;
		protected string m_introText = String.Empty;
		protected string m_url1 = String.Empty;
		protected string m_url2 = String.Empty;
		protected string m_url3 = String.Empty;
		protected string m_url4 = String.Empty;
		protected string m_bumperPageHtml = String.Empty;
		protected Nullable <Boolean> m_bumperPageSkip = null;
		protected string m_customHtml = String.Empty;
		protected int m_homepageTypeId = 0;
		protected Nullable <Boolean> m_isPublished = null;
		protected int m_lastModifiedByUserId = 0;
		protected DateTime m_lastModifiedDate = DateTime.MinValue;
		
		// View Variables
		protected string m_brandName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_homepageIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Homepage object.
		/// </summary>
		public Nullable <Int32> HomepageId
		{
			get
			{
				return m_homepageId;
			}
			set
			{
				if (value != m_homepageId)
				{
					m_homepageId = value;
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

		public string IntroText
		{
			get
			{
				return m_introText;
			}
			set
			{
				if (value != m_introText)
				{
					m_introText = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Url1
		{
			get
			{
				return m_url1;
			}
			set
			{
				if (value != m_url1)
				{
					m_url1 = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Url2
		{
			get
			{
				return m_url2;
			}
			set
			{
				if (value != m_url2)
				{
					m_url2 = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Url3
		{
			get
			{
				return m_url3;
			}
			set
			{
				if (value != m_url3)
				{
					m_url3 = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Url4
		{
			get
			{
				return m_url4;
			}
			set
			{
				if (value != m_url4)
				{
					m_url4 = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string BumperPageHtml
		{
			get
			{
				return m_bumperPageHtml;
			}
			set
			{
				if (value != m_bumperPageHtml)
				{
					m_bumperPageHtml = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> BumperPageSkip
		{
			get
			{
				return m_bumperPageSkip;
			}
			set
			{
				if (value != m_bumperPageSkip)
				{
					m_bumperPageSkip = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string CustomHtml
		{
			get
			{
				return m_customHtml;
			}
			set
			{
				if (value != m_customHtml)
				{
					m_customHtml = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int HomepageTypeId
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

		public Nullable <Boolean> IsPublished
		{
			get
			{
				return m_isPublished;
			}
			set
			{
				if (value != m_isPublished)
				{
					m_isPublished = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int LastModifiedByUserId
		{
			get
			{
				return m_lastModifiedByUserId;
			}
			set
			{
				if (value != m_lastModifiedByUserId)
				{
					m_lastModifiedByUserId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime LastModifiedDate
		{
			get
			{
				return m_lastModifiedDate;
			}
			set
			{
				if (value != m_lastModifiedDate)
				{
					m_lastModifiedDate = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> HomepageIdList
		{
			get
			{
				return m_homepageIdList;
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
		
		public virtual string BrandName
		{
			get
			{
				return m_brandName;
			}
			set 
			{ 
				m_brandName = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_Homepage]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (HomepageIdList != null && HomepageIdList.Count > 0)
			{
				JoinableList list = new JoinableList(HomepageIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Homepage.Columns.HomepageId));
			}
			
			if (HomepageId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@homepageId", Homepage.Columns.HomepageId));
				sb.AddDataParameter("@homepageId", HomepageId.Value);
			}
			
			if (BrandId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", Homepage.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId);
			}
	
			if (IntroText != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@introText", Homepage.Columns.IntroText));
				sb.AddDataParameter("@introText", IntroText);
			}						
	
			if (Url1 != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@url1", Homepage.Columns.Url1));
				sb.AddDataParameter("@url1", Url1);
			}						
	
			if (Url2 != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@url2", Homepage.Columns.Url2));
				sb.AddDataParameter("@url2", Url2);
			}						
	
			if (Url3 != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@url3", Homepage.Columns.Url3));
				sb.AddDataParameter("@url3", Url3);
			}						
	
			if (Url4 != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@url4", Homepage.Columns.Url4));
				sb.AddDataParameter("@url4", Url4);
			}						
	
			if (BumperPageHtml != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@bumperPageHtml", Homepage.Columns.BumperPageHtml));
				sb.AddDataParameter("@bumperPageHtml", BumperPageHtml);
			}						
	
			if (BumperPageSkip.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@bumperPageSkip", Homepage.Columns.BumperPageSkip));
				sb.AddDataParameter("@bumperPageSkip", SqlUtils.BitValue(BumperPageSkip.Value));
			}
	
			if (CustomHtml != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@customHtml", Homepage.Columns.CustomHtml));
				sb.AddDataParameter("@customHtml", CustomHtml);
			}						
	
			if (HomepageTypeId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@homepageTypeId", Homepage.Columns.HomepageTypeId));
				sb.AddDataParameter("@homepageTypeId", HomepageTypeId);
			}
	
			if (IsPublished.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isPublished", Homepage.Columns.IsPublished));
				sb.AddDataParameter("@isPublished", SqlUtils.BitValue(IsPublished.Value));
			}
	
			if (LastModifiedByUserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@lastModifiedByUserId", Homepage.Columns.LastModifiedByUserId));
				sb.AddDataParameter("@lastModifiedByUserId", LastModifiedByUserId);
			}
	
			if (LastModifiedDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lastModifiedDate", Homepage.Columns.LastModifiedDate));
				sb.AddDataParameter("@lastModifiedDate", LastModifiedDate);
			}
	
			if (BrandName != String.Empty)
			{
				sb.Criteria.Add("BrandName=@brandName");
				sb.AddDataParameter("@brandName", BrandName);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}