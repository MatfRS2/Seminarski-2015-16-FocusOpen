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
	public partial class LightboxSentFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_lightboxSentId = null;
		
		// Table columns
		protected int m_lightboxId = 0;
		protected Nullable <Int32> m_createdLightboxId = null;
		protected int m_senderId = 0;
		protected string m_recipientEmail = String.Empty;
		protected string m_subject = String.Empty;
		protected string m_message = String.Empty;
		protected DateTime m_dateSent = DateTime.MinValue;
		protected Nullable <DateTime> m_expiryDate = null;
		protected Nullable <Boolean> m_downloadLinks = null;
		protected Nullable <Int32> m_lightboxLinkedId = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_lightboxSentIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the LightboxSent object.
		/// </summary>
		public Nullable <Int32> LightboxSentId
		{
			get
			{
				return m_lightboxSentId;
			}
			set
			{
				if (value != m_lightboxSentId)
				{
					m_lightboxSentId = value;
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

		public Nullable <Int32> CreatedLightboxId
		{
			get
			{
				return m_createdLightboxId;
			}
			set
			{
				if (value != m_createdLightboxId)
				{
					m_createdLightboxId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int SenderId
		{
			get
			{
				return m_senderId;
			}
			set
			{
				if (value != m_senderId)
				{
					m_senderId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string RecipientEmail
		{
			get
			{
				return m_recipientEmail;
			}
			set
			{
				if (value != m_recipientEmail)
				{
					m_recipientEmail = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Subject
		{
			get
			{
				return m_subject;
			}
			set
			{
				if (value != m_subject)
				{
					m_subject = value;
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

		public DateTime DateSent
		{
			get
			{
				return m_dateSent;
			}
			set
			{
				if (value != m_dateSent)
				{
					m_dateSent = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <DateTime> ExpiryDate
		{
			get
			{
				return m_expiryDate;
			}
			set
			{
				if (value != m_expiryDate)
				{
					m_expiryDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> DownloadLinks
		{
			get
			{
				return m_downloadLinks;
			}
			set
			{
				if (value != m_downloadLinks)
				{
					m_downloadLinks = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> LightboxLinkedId
		{
			get
			{
				return m_lightboxLinkedId;
			}
			set
			{
				if (value != m_lightboxLinkedId)
				{
					m_lightboxLinkedId = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> LightboxSentIdList
		{
			get
			{
				return m_lightboxSentIdList;
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
				return "[LightboxSent]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (LightboxSentIdList != null && LightboxSentIdList.Count > 0)
			{
				JoinableList list = new JoinableList(LightboxSentIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", LightboxSent.Columns.LightboxSentId));
			}
			
			if (LightboxSentId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxSentId", LightboxSent.Columns.LightboxSentId));
				sb.AddDataParameter("@lightboxSentId", LightboxSentId.Value);
			}
			
			if (LightboxId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxId", LightboxSent.Columns.LightboxId));
				sb.AddDataParameter("@lightboxId", LightboxId);
			}
	
			if (CreatedLightboxId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@createdLightboxId", LightboxSent.Columns.CreatedLightboxId));
				sb.AddDataParameter("@createdLightboxId", CreatedLightboxId.Value);
			}
	
			if (SenderId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@senderId", LightboxSent.Columns.SenderId));
				sb.AddDataParameter("@senderId", SenderId);
			}
	
			if (RecipientEmail != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@recipientEmail", LightboxSent.Columns.RecipientEmail));
				sb.AddDataParameter("@recipientEmail", RecipientEmail);
			}						
	
			if (Subject != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@subject", LightboxSent.Columns.Subject));
				sb.AddDataParameter("@subject", Subject);
			}						
	
			if (Message != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@message", LightboxSent.Columns.Message));
				sb.AddDataParameter("@message", Message);
			}						
	
			if (DateSent != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@dateSent", LightboxSent.Columns.DateSent));
				sb.AddDataParameter("@dateSent", DateSent);
			}
	
			if (ExpiryDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@expiryDate", LightboxSent.Columns.ExpiryDate));
				sb.AddDataParameter("@expiryDate", ExpiryDate.Value);
			}
	
			if (DownloadLinks.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@downloadLinks", LightboxSent.Columns.DownloadLinks));
				sb.AddDataParameter("@downloadLinks", SqlUtils.BitValue(DownloadLinks.Value));
			}
	
			if (LightboxLinkedId != null)
			{
				sb.Criteria.Add(string.Format("{0}=@lightboxLinkedId", LightboxSent.Columns.LightboxLinkedId));
				sb.AddDataParameter("@lightboxLinkedId", LightboxLinkedId.Value);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}