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
	public partial class AssetFileFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetFileId = null;
		
		// Table columns
		protected int m_assetId = 0;
		protected byte[] m_fileContent = null;
		protected string m_filename = String.Empty;
		protected string m_fileExtension = String.Empty;
		protected int m_assetFileTypeId = 0;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_assetFileIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
		/// <summary>
		/// Returns the Id of the AssetFile object.
		/// </summary>
		public Nullable <Int32> AssetFileId
		{
			get
			{
				return m_assetFileId;
			}
			set
			{
				if (value != m_assetFileId)
				{
					m_assetFileId = value;
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

		public byte[] FileContent
		{
			get
			{
				return m_fileContent;
			}
			set
			{
				if (value != m_fileContent)
				{
					m_fileContent = value;
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

		public string FileExtension
		{
			get
			{
				return m_fileExtension;
			}
			set
			{
				if (value != m_fileExtension)
				{
					m_fileExtension = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public int AssetFileTypeId
		{
			get
			{
				return m_assetFileTypeId;
			}
			set
			{
				if (value != m_assetFileTypeId)
				{
					m_assetFileTypeId = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public DateTime LastUpdate
		{
			get
			{
				return m_lastUpdate;
			}
			set
			{
				if (value != m_lastUpdate)
				{
					m_lastUpdate = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> AssetFileIdList
		{
			get
			{
				return m_assetFileIdList;
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
				return "[AssetFile]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (AssetFileIdList != null && AssetFileIdList.Count > 0)
			{
				JoinableList list = new JoinableList(AssetFileIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", AssetFile.Columns.AssetFileId));
			}
			
			if (AssetFileId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@assetFileId", AssetFile.Columns.AssetFileId));
				sb.AddDataParameter("@assetFileId", AssetFileId.Value);
			}
			
			if (AssetId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetId", AssetFile.Columns.AssetId));
				sb.AddDataParameter("@assetId", AssetId);
			}
	
			if (FileContent != null)
			{
				sb.Criteria.Add(string.Format("{0}=@fileContent", AssetFile.Columns.FileContent));
				sb.AddDataParameter("@fileContent", FileContent);
			}						
	
			if (Filename != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@filename", AssetFile.Columns.Filename));
				sb.AddDataParameter("@filename", Filename);
			}						
	
			if (FileExtension != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@fileExtension", AssetFile.Columns.FileExtension));
				sb.AddDataParameter("@fileExtension", FileExtension);
			}						
	
			if (AssetFileTypeId != 0)
			{
				sb.Criteria.Add(string.Format("{0}=@assetFileTypeId", AssetFile.Columns.AssetFileTypeId));
				sb.AddDataParameter("@assetFileTypeId", AssetFileTypeId);
			}
	
			if (LastUpdate != DateTime.MinValue)
			{
				sb.Criteria.Add(string.Format("{0}=@lastUpdate", AssetFile.Columns.LastUpdate));
				sb.AddDataParameter("@lastUpdate", LastUpdate);
			}
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}