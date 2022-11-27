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
	public partial class LightboxFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxId = null;
		
		// Table columns
		protected int m_userId = 0;
		protected string m_name = String.Empty;
		protected string m_summary = String.Empty;
		protected string m_notes = String.Empty;
		protected Nullable <Boolean> m_isPublic = null;
		protected Nullable <Boolean> m_isDefault = null;
		protected DateTime m_createDate = DateTime.MinValue;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_lightboxIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Lightbox object.
		/// </summary>
		public Nullable <Int32> LightboxId
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

		public int UserId
		{
			get
			{
				return m_userId;
			}
			set
			{
				if (value != m_userId)
				{
					m_userId = value;
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

		public string Summary
		{
			get
			{
				return m_summary;
			}
			set
			{
				if (value != m_summary)
				{
					m_summary = value;
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

		public Nullable <Boolean> IsPublic
		{
			get
			{
				return m_isPublic;
			}
			set
			{
				if (value != m_isPublic)
				{
					m_isPublic = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsDefault
		{
			get
			{
				return m_isDefault;
			}
			set
			{
				if (value != m_isDefault)
				{
					m_isDefault = value;
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
		
		public List<Int32> LightboxIdList
		{
			get
			{
				return m_lightboxIdList;
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
				return "[Lightbox]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (LightboxIdList != null && LightboxIdList.Count > 0)
			{
				JoinableList list = new JoinableList(LightboxIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Lightbox.Columns.LightboxId));
			}
			
			if (LightboxId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxId", Lightbox.Columns.LightboxId));
				sb.AddDataParameter("@lightboxId", LightboxId.Value);
			}
			
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", Lightbox.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Lightbox.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (Summary != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@summary", Lightbox.Columns.Summary));
				sb.AddDataParameter("@summary", Summary);
			}						
	
			if (Notes != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@notes", Lightbox.Columns.Notes));
				sb.AddDataParameter("@notes", Notes);
			}						
	
			if (IsPublic.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isPublic", Lightbox.Columns.IsPublic));
				sb.AddDataParameter("@isPublic", SqlUtils.BitValue(IsPublic.Value));
			}
	
			if (IsDefault.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDefault", Lightbox.Columns.IsDefault));
				sb.AddDataParameter("@isDefault", SqlUtils.BitValue(IsDefault.Value));
			}
	
			if (CreateDate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@createDate", Lightbox.Columns.CreateDate));
				sb.AddDataParameter("@createDate", CreateDate);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}