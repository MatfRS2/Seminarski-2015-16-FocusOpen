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
	public partial class UserPasswordHistoryFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_userPasswordHistoryId = null;
		
		// Table columns
		protected int m_userId = 0;
		protected string m_password = String.Empty;
		protected DateTime m_date = DateTime.MinValue;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_userPasswordHistoryIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the UserPasswordHistory object.
		/// </summary>
		public Nullable <Int32> UserPasswordHistoryId
		{
			get
			{
				return m_userPasswordHistoryId;
			}
			set
			{
				if (value != m_userPasswordHistoryId)
				{
					m_userPasswordHistoryId = value;
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

		public string Password
		{
			get
			{
				return m_password;
			}
			set
			{
				if (value != m_password)
				{
					m_password = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime Date
		{
			get
			{
				return m_date;
			}
			set
			{
				if (value != m_date)
				{
					m_date = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> UserPasswordHistoryIdList
		{
			get
			{
				return m_userPasswordHistoryIdList;
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
				return "[UserPasswordHistory]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (UserPasswordHistoryIdList != null && UserPasswordHistoryIdList.Count > 0)
			{
				JoinableList list = new JoinableList(UserPasswordHistoryIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", UserPasswordHistory.Columns.UserPasswordHistoryId));
			}
			
			if (UserPasswordHistoryId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@userPasswordHistoryId", UserPasswordHistory.Columns.UserPasswordHistoryId));
				sb.AddDataParameter("@userPasswordHistoryId", UserPasswordHistoryId.Value);
			}
			
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", UserPasswordHistory.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (Password != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@password", UserPasswordHistory.Columns.Password));
				sb.AddDataParameter("@password", Password);
			}						
	
			if (Date != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@date", UserPasswordHistory.Columns.Date));
				sb.AddDataParameter("@date", Date);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}