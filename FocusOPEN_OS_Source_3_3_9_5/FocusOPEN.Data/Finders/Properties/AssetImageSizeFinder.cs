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
	public partial class AssetImageSizeFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetImageSizeId = null;
		
		// Table columns
		protected string m_description = String.Empty;
		protected int m_height = 0;
		protected int m_width = 0;
		protected int m_dotsPerInch = 0;
		protected string m_fileSuffix = String.Empty;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetImageSizeIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetImageSize object.
		/// </summary>
		public Nullable <Int32> AssetImageSizeId
		{
			get
			{
				return m_assetImageSizeId;
			}
			set
			{
				if (value != m_assetImageSizeId)
				{
					m_assetImageSizeId = value;
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

		public int Height
		{
			get
			{
				return m_height;
			}
			set
			{
				if (value != m_height)
				{
					m_height = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int Width
		{
			get
			{
				return m_width;
			}
			set
			{
				if (value != m_width)
				{
					m_width = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int DotsPerInch
		{
			get
			{
				return m_dotsPerInch;
			}
			set
			{
				if (value != m_dotsPerInch)
				{
					m_dotsPerInch = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string FileSuffix
		{
			get
			{
				return m_fileSuffix;
			}
			set
			{
				if (value != m_fileSuffix)
				{
					m_fileSuffix = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetImageSizeIdList
		{
			get
			{
				return m_assetImageSizeIdList;
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
				return "[AssetImageSize]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetImageSizeIdList != null && AssetImageSizeIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetImageSizeIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetImageSize.Columns.AssetImageSizeId));
			}
			
			if (AssetImageSizeId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetImageSizeId", AssetImageSize.Columns.AssetImageSizeId));
				sb.AddDataParameter("@assetImageSizeId", AssetImageSizeId.Value);
			}
			
			if (Description != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@description", AssetImageSize.Columns.Description));
				sb.AddDataParameter("@description", Description);
			}						
	
			if (Height != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@height", AssetImageSize.Columns.Height));
				sb.AddDataParameter("@height", Height);
			}
	
			if (Width != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@width", AssetImageSize.Columns.Width));
				sb.AddDataParameter("@width", Width);
			}
	
			if (DotsPerInch != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@dotsPerInch", AssetImageSize.Columns.DotsPerInch));
				sb.AddDataParameter("@dotsPerInch", DotsPerInch);
			}
	
			if (FileSuffix != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@fileSuffix", AssetImageSize.Columns.FileSuffix));
				sb.AddDataParameter("@fileSuffix", FileSuffix);
			}						
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}