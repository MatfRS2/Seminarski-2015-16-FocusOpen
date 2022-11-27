/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
using SearchBuilder=Daydream.Data.SearchBuilder;

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class AssetTypeFileExtensionFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetTypeFileExtensionId = null;
		
		// Table columns
		protected string m_extension = String.Empty;
		protected string m_name = String.Empty;
		protected int m_assetTypeId = 0;
		protected byte[] m_iconImage = null;
		protected string m_iconFilename = String.Empty;
		protected Nullable <Boolean> m_isVisible = null;
		protected Guid m_plugin = Guid.Empty;
		
		// View Variables
		protected string m_assetTypeName = String.Empty;
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetTypeFileExtensionIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetTypeFileExtension object.
		/// </summary>
		public Nullable <Int32> AssetTypeFileExtensionId
		{
			get
			{
				return m_assetTypeFileExtensionId;
			}
			set
			{
				if (value != m_assetTypeFileExtensionId)
				{
					m_assetTypeFileExtensionId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Extension
		{
			get
			{
				return m_extension;
			}
			set
			{
				if (value != m_extension)
				{
					m_extension = value;
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

		public int AssetTypeId
		{
			get
			{
				return m_assetTypeId;
			}
			set
			{
				if (value != m_assetTypeId)
				{
					m_assetTypeId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public byte[] IconImage
		{
			get
			{
				return m_iconImage;
			}
			set
			{
				if (value != m_iconImage)
				{
					m_iconImage = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string IconFilename
		{
			get
			{
				return m_iconFilename;
			}
			set
			{
				if (value != m_iconFilename)
				{
					m_iconFilename = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsVisible
		{
			get
			{
				return m_isVisible;
			}
			set
			{
				if (value != m_isVisible)
				{
					m_isVisible = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Guid Plugin
		{
			get
			{
				return m_plugin;
			}
			set
			{
				if (value != m_plugin)
				{
					m_plugin = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetTypeFileExtensionIdList
		{
			get
			{
				return m_assetTypeFileExtensionIdList;
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
		
		public virtual string AssetTypeName
		{
			get
			{
				return m_assetTypeName;
			}
			set 
			{ 
				m_assetTypeName = value; 
			}
		}
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_AssetTypeFileExtension]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetTypeFileExtensionIdList != null && AssetTypeFileExtensionIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetTypeFileExtensionIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetTypeFileExtension.Columns.AssetTypeFileExtensionId));
			}
			
			if (AssetTypeFileExtensionId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetTypeFileExtensionId", AssetTypeFileExtension.Columns.AssetTypeFileExtensionId));
				sb.AddDataParameter("@assetTypeFileExtensionId", AssetTypeFileExtensionId.Value);
			}
			
			if (Extension != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@extension", AssetTypeFileExtension.Columns.Extension));
				sb.AddDataParameter("@extension", Extension);
			}						
	
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", AssetTypeFileExtension.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (AssetTypeId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetTypeId", AssetTypeFileExtension.Columns.AssetTypeId));
				sb.AddDataParameter("@assetTypeId", AssetTypeId);
			}
	
			if (IconImage != null)
			{
				sb.Criteria.Add(string.Format("{0}=@iconImage", AssetTypeFileExtension.Columns.IconImage));
				sb.AddDataParameter("@iconImage", IconImage);
			}						
	
			if (IconFilename != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@iconFilename", AssetTypeFileExtension.Columns.IconFilename));
				sb.AddDataParameter("@iconFilename", IconFilename);
			}						
	
			if (IsVisible.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isVisible", AssetTypeFileExtension.Columns.IsVisible));
				sb.AddDataParameter("@isVisible", SqlUtils.BitValue(IsVisible.Value));
			}
	
			if (Plugin != Guid.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@plugin", AssetTypeFileExtension.Columns.Plugin));
				sb.AddDataParameter("@plugin", Plugin);
			}						
	
			if (AssetTypeName != String.Empty)
			{
				sb.Criteria.Add("AssetTypeName=@assetTypeName");
				sb.AddDataParameter("@assetTypeName", AssetTypeName);
			}						

			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}