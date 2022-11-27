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
	public partial class AssetFilePathFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetFilePathId = null;
		
		// Table columns
		protected string m_path = String.Empty;
		protected Nullable <Boolean> m_isDefault = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetFilePathIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetFilePath object.
		/// </summary>
		public Nullable <Int32> AssetFilePathId
		{
			get
			{
				return m_assetFilePathId;
			}
			set
			{
				if (value != m_assetFilePathId)
				{
					m_assetFilePathId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Path
		{
			get
			{
				return m_path;
			}
			set
			{
				if (value != m_path)
				{
					m_path = value;
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
		
		public List<Int32> AssetFilePathIdList
		{
			get
			{
				return m_assetFilePathIdList;
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
				return "[AssetFilePath]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetFilePathIdList != null && AssetFilePathIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetFilePathIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetFilePath.Columns.AssetFilePathId));
			}
			
			if (AssetFilePathId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetFilePathId", AssetFilePath.Columns.AssetFilePathId));
				sb.AddDataParameter("@assetFilePathId", AssetFilePathId.Value);
			}
			
			if (Path != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@path", AssetFilePath.Columns.Path));
				sb.AddDataParameter("@path", Path);
			}						
	
			if (IsDefault.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDefault", AssetFilePath.Columns.IsDefault));
				sb.AddDataParameter("@isDefault", SqlUtils.BitValue(IsDefault.Value));
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}