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
	public partial class Plugin : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_pluginId = null;
		
		// Table variables
		protected Guid m_registrationKey = Guid.Empty;
		protected string m_relativePath = String.Empty;
		protected string m_filename = String.Empty;
		protected string m_name = String.Empty;
		protected Nullable <Int32> m_checksum = null;
		protected Nullable <Int32> m_pluginType = null;
		protected Nullable <Boolean> m_isDefault = null;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
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
				return (m_pluginId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the RegistrationKey of the Plugin object.
		/// </summary>
		public virtual Guid RegistrationKey
		{
			get
			{
				return m_registrationKey;
			}
			set 
			{ 
				if (value != m_registrationKey)
				{
					m_ChangedProperties["RegistrationKey"] = new ChangedProperty("RegistrationKey", m_registrationKey, value);
					
					m_registrationKey = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the RelativePath of the Plugin object.
		/// </summary>
		public virtual string RelativePath
		{
			get
			{
				return m_relativePath;
			}
			set 
			{ 
				if (value != m_relativePath)
				{
					m_ChangedProperties["RelativePath"] = new ChangedProperty("RelativePath", m_relativePath, value);
					
					m_relativePath = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Filename of the Plugin object.
		/// </summary>
		public virtual string Filename
		{
			get
			{
				return m_filename;
			}
			set 
			{ 
				if (value != m_filename)
				{
					m_ChangedProperties["Filename"] = new ChangedProperty("Filename", m_filename, value);
					
					m_filename = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Name of the Plugin object.
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
		/// Sets or Returns the Checksum of the Plugin object.
		/// </summary>
		public virtual Nullable <Int32> Checksum
		{
			get
			{
				return m_checksum;
			}
			set 
			{ 
				if (value != m_checksum)
				{
					m_ChangedProperties["Checksum"] = new ChangedProperty("Checksum", m_checksum, value);
					
					m_checksum = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PluginType of the Plugin object.
		/// </summary>
		public virtual Nullable <Int32> PluginType
		{
			get
			{
				return m_pluginType;
			}
			set 
			{ 
				if (value != m_pluginType)
				{
					m_ChangedProperties["PluginType"] = new ChangedProperty("PluginType", m_pluginType, value);
					
					m_pluginType = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDefault of the Plugin object.
		/// </summary>
		public virtual Nullable <Boolean> IsDefault
		{
			get
			{
				return m_isDefault;
			}
			set 
			{ 
				if (value != m_isDefault)
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
			PluginId,
			RegistrationKey,
			RelativePath,
			Filename,
			Name,
			Checksum,
			PluginType,
			IsDefault
		}
	}
}

