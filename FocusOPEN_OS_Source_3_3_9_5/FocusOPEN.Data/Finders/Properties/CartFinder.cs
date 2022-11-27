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
	public partial class CartFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_cartId = null;
		
		// Table columns
		protected int m_userId = 0;
		protected int m_assetId = 0;
		protected string m_notes = String.Empty;
		protected Nullable <DateTime> m_requiredByDate = null;
		protected DateTime m_dateAdded = DateTime.MinValue;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_cartIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Cart object.
		/// </summary>
		public Nullable <Int32> CartId
		{
			get
			{
				return m_cartId;
			}
			set
			{
				if (value != m_cartId)
				{
					m_cartId = value;
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

		public Nullable <DateTime> RequiredByDate
		{
			get
			{
				return m_requiredByDate;
			}
			set
			{
				if (value != m_requiredByDate)
				{
					m_requiredByDate = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime DateAdded
		{
			get
			{
				return m_dateAdded;
			}
			set
			{
				if (value != m_dateAdded)
				{
					m_dateAdded = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> CartIdList
		{
			get
			{
				return m_cartIdList;
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
				return "[v_Cart]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (CartIdList != null && CartIdList.Count > 0)
			{
				JoinableList list = new JoinableList(CartIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Cart.Columns.CartId));
			}
			
			if (CartId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@cartId", Cart.Columns.CartId));
				sb.AddDataParameter("@cartId", CartId.Value);
			}
			
			if (UserId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@userId", Cart.Columns.UserId));
				sb.AddDataParameter("@userId", UserId);
			}
	
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", Cart.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (Notes != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@notes", Cart.Columns.Notes));
				sb.AddDataParameter("@notes", Notes);
			}						
	
			if (RequiredByDate != null)
			{
				sb.Criteria.Add(string.Format("{0}=@requiredByDate", Cart.Columns.RequiredByDate));
				sb.AddDataParameter("@requiredByDate", RequiredByDate.Value);
			}
	
			if (DateAdded != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@dateAdded", Cart.Columns.DateAdded));
				sb.AddDataParameter("@dateAdded", DateAdded);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}