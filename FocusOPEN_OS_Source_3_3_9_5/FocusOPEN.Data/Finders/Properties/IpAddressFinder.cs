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
	public partial class IpAddressFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_ipAddressId = null;
		
		// Table columns
		protected string m_ipAddressValue = String.Empty;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_ipAddressIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the IpAddress object.
		/// </summary>
		public Nullable <Int32> IpAddressId
		{
			get
			{
				return m_ipAddressId;
			}
			set
			{
				if (value != m_ipAddressId)
				{
					m_ipAddressId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string IpAddressValue
		{
			get
			{
				return m_ipAddressValue;
			}
			set
			{
				if (value != m_ipAddressValue)
				{
					m_ipAddressValue = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> IpAddressIdList
		{
			get
			{
				return m_ipAddressIdList;
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
				return "[IpAddress]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (IpAddressIdList != null && IpAddressIdList.Count > 0)
			{
				JoinableList list = new JoinableList(IpAddressIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", IpAddress.Columns.IpAddressId));
			}
			
			if (IpAddressId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@ipAddressId", IpAddress.Columns.IpAddressId));
				sb.AddDataParameter("@ipAddressId", IpAddressId.Value);
			}
			
			if (IpAddressValue != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@ipAddressValue", IpAddress.Columns.IpAddressValue));
				sb.AddDataParameter("@ipAddressValue", IpAddressValue);
			}						
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}