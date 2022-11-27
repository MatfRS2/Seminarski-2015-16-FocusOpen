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

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class AssetTypeFileExtension : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetTypeFileExtensionId = null;
		
		// Table variables
		protected string m_extension = String.Empty;
		protected string m_name = String.Empty;
		protected int m_assetTypeId = 0;
		protected byte[] m_iconImage = null;
		protected string m_iconFilename = String.Empty;
		protected bool m_isVisible = false;
		protected Guid m_plugin = Guid.Empty;
		
		// View Variables
		protected string m_assetTypeName = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
				return (m_assetTypeFileExtensionId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Extension of the AssetTypeFileExtension object.
		/// </summary>
		public virtual string Extension
		{
			get
			{
				return m_extension;
			}
			set 
			{ 
				if (value != m_extension)
				{
					m_ChangedProperties["Extension"] = new ChangedProperty("Extension", m_extension, value);
					
					m_extension = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Name of the AssetTypeFileExtension object.
		/// </summary>
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set 
			{ 
				if (value != m_name)
				{
					m_ChangedProperties["Name"] = new ChangedProperty("Name", m_name, value);
					
					m_name = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetTypeId of the AssetTypeFileExtension object.
		/// </summary>
		public virtual int AssetTypeId
		{
			get
			{
				return m_assetTypeId;
			}
			set 
			{ 
				if (value != m_assetTypeId)
				{
					m_ChangedProperties["AssetTypeId"] = new ChangedProperty("AssetTypeId", m_assetTypeId, value);
					
					m_assetTypeId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IconImage of the AssetTypeFileExtension object.
		/// </summary>
		public virtual byte[] IconImage
		{
			get
			{
				return m_iconImage;
			}
			set 
			{ 
				if (value != m_iconImage)
				{
					m_ChangedProperties["IconImage"] = new ChangedProperty("IconImage", m_iconImage, value);
					
					m_iconImage = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IconFilename of the AssetTypeFileExtension object.
		/// </summary>
		public virtual string IconFilename
		{
			get
			{
				return m_iconFilename;
			}
			set 
			{ 
				if (value != m_iconFilename)
				{
					m_ChangedProperties["IconFilename"] = new ChangedProperty("IconFilename", m_iconFilename, value);
					
					m_iconFilename = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsVisible of the AssetTypeFileExtension object.
		/// </summary>
		public virtual bool IsVisible
		{
			get
			{
				return m_isVisible;
			}
			set 
			{ 
				if (value != m_isVisible)
				{
					m_ChangedProperties["IsVisible"] = new ChangedProperty("IsVisible", m_isVisible, value);
					
					m_isVisible = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Plugin of the AssetTypeFileExtension object.
		/// </summary>
		public virtual Guid Plugin
		{
			get
			{
				return m_plugin;
			}
			set 
			{ 
				if (value != m_plugin)
				{
					m_ChangedProperties["Plugin"] = new ChangedProperty("Plugin", m_plugin, value);
					
					m_plugin = value; 
					m_isDirty = true;
				}
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
		
		public enum Columns
		{
			AssetTypeFileExtensionId,
			Extension,
			Name,
			AssetTypeId,
			IconImage,
			IconFilename,
			IsVisible,
			Plugin
		}
	}
}

