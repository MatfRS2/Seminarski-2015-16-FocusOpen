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
	public partial class AssetFile : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetFileId = null;
		
		// Table variables
		protected int m_assetId = 0;
		protected byte[] m_fileContent = null;
		protected string m_filename = String.Empty;
		protected string m_fileExtension = String.Empty;
		protected int m_assetFileTypeId = 0;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
					m_isDirty = true;
				}
			}
		}
		
		public Dictionary<String, ChangedProperty> ChangedProperties
		{
			get
			{
				return m_ChangedProperties;
			}
		}

		public override bool IsNew
		{
			get
			{
				return (m_assetFileId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetId of the AssetFile object.
		/// </summary>
		public virtual int AssetId
		{
			get
			{
				return m_assetId;
			}
			set 
			{ 
				if ((value != m_assetId))
				{
					m_ChangedProperties["AssetId"] = new ChangedProperty("AssetId", m_assetId, value);
					
					m_assetId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FileContent of the AssetFile object.
		/// </summary>
		public virtual byte[] FileContent
		{
			get
			{
				return m_fileContent;
			}
			set 
			{ 
				if ((value != m_fileContent))
				{
					m_ChangedProperties["FileContent"] = new ChangedProperty("FileContent", m_fileContent, value);
					
					m_fileContent = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Filename of the AssetFile object.
		/// </summary>
		public virtual string Filename
		{
			get
			{
				return m_filename;
			}
			set 
			{ 
				if ((value != m_filename))
				{
					m_ChangedProperties["Filename"] = new ChangedProperty("Filename", m_filename, value);
					
					m_filename = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FileExtension of the AssetFile object.
		/// </summary>
		public virtual string FileExtension
		{
			get
			{
				return m_fileExtension;
			}
			set 
			{ 
				if ((value != m_fileExtension))
				{
					m_ChangedProperties["FileExtension"] = new ChangedProperty("FileExtension", m_fileExtension, value);
					
					m_fileExtension = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetFileTypeId of the AssetFile object.
		/// </summary>
		public virtual int AssetFileTypeId
		{
			get
			{
				return m_assetFileTypeId;
			}
			set 
			{ 
				if ((value != m_assetFileTypeId))
				{
					m_ChangedProperties["AssetFileTypeId"] = new ChangedProperty("AssetFileTypeId", m_assetFileTypeId, value);
					
					m_assetFileTypeId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastUpdate of the AssetFile object.
		/// </summary>
		public virtual DateTime LastUpdate
		{
			get
			{
				return m_lastUpdate;
			}
			set 
			{ 
				if ((value != m_lastUpdate))
				{
					m_ChangedProperties["LastUpdate"] = new ChangedProperty("LastUpdate", m_lastUpdate, value);
					
					m_lastUpdate = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			AssetFileId,
			AssetId,
			FileContent,
			Filename,
			FileExtension,
			AssetFileTypeId,
			LastUpdate
		}
	}
}

