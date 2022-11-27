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
	/// <summary>
	/// This object represents the properties and methods of a AssetFilePath.
	/// </summary>
	[Serializable]
	public partial class AssetFilePath : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetFilePathId = null;
		
		// Table variables
		protected string m_path = String.Empty;
		protected bool m_isDefault = false;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
				return (m_assetFilePathId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Path of the AssetFilePath object.
		/// </summary>
		public virtual string Path
		{
			get
			{
				return m_path;
			}
			set 
			{ 
				if ((value != m_path))
				{
					m_ChangedProperties["Path"] = new ChangedProperty("Path", m_path, value);
					
					m_path = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDefault of the AssetFilePath object.
		/// </summary>
		public virtual bool IsDefault
		{
			get
			{
				return m_isDefault;
			}
			set 
			{ 
				if ((value != m_isDefault))
				{
					m_ChangedProperties["IsDefault"] = new ChangedProperty("IsDefault", m_isDefault, value);
					
					m_isDefault = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			AssetFilePathId,
			Path,
			IsDefault
		}
	}
}

