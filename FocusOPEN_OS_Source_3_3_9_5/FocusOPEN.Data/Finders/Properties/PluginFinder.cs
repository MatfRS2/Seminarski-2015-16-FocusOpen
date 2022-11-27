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
	public partial class PluginFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_pluginId = null;
		
		// Table columns
		protected Guid m_registrationKey = Guid.Empty;
		protected string m_relativePath = String.Empty;
		protected string m_filename = String.Empty;
		protected string m_name = String.Empty;
		protected Nullable <Int32> m_checksum = null;
		protected Nullable <Int32> m_pluginType = null;
		protected Nullable <Boolean> m_isDefault = null;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_pluginIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the Plugin object.
		/// </summary>
		public Nullable <Int32> PluginId
		{
			get
			{
				return m_pluginId;
			}
			set
			{
				if (value != m_pluginId)
				{
					m_pluginId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Guid RegistrationKey
		{
			get
			{
				return m_registrationKey;
			}
			set
			{
				if (value != m_registrationKey)
				{
					m_registrationKey = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string RelativePath
		{
			get
			{
				return m_relativePath;
			}
			set
			{
				if (value != m_relativePath)
				{
					m_relativePath = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string Filename
		{
			get
			{
				return m_filename;
			}
			set
			{
				if (value != m_filename)
				{
					m_filename = value;
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

		public Nullable <Int32> Checksum
		{
			get
			{
				return m_checksum;
			}
			set
			{
				if (value != m_checksum)
				{
					m_checksum = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Int32> PluginType
		{
			get
			{
				return m_pluginType;
			}
			set
			{
				if (value != m_pluginType)
				{
					m_pluginType = value;
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
		
		public List<Int32> PluginIdList
		{
			get
			{
				return m_pluginIdList;
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
				return "[Plugins]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (PluginIdList != null && PluginIdList.Count > 0)
			{
				JoinableList list = new JoinableList(PluginIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Plugin.Columns.PluginId));
			}
			
			if (PluginId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@pluginId", Plugin.Columns.PluginId));
				sb.AddDataParameter("@pluginId", PluginId.Value);
			}
			
			if (RegistrationKey != Guid.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@registrationKey", Plugin.Columns.RegistrationKey));
				sb.AddDataParameter("@registrationKey", RegistrationKey);
			}						
	
			if (RelativePath != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@relativePath", Plugin.Columns.RelativePath));
				sb.AddDataParameter("@relativePath", RelativePath);
			}						
	
			if (Filename != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@filename", Plugin.Columns.Filename));
				sb.AddDataParameter("@filename", Filename);
			}						
	
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Plugin.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (Checksum != null)
			{
				sb.Criteria.Add(string.Format("{0}=@checksum", Plugin.Columns.Checksum));
				sb.AddDataParameter("@checksum", Checksum.Value);
			}
	
			if (PluginType != null)
			{
				sb.Criteria.Add(string.Format("{0}=@pluginType", Plugin.Columns.PluginType));
				sb.AddDataParameter("@pluginType", PluginType.Value);
			}
	
			if (IsDefault.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDefault", Plugin.Columns.IsDefault));
				sb.AddDataParameter("@isDefault", SqlUtils.BitValue(IsDefault.Value));
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}