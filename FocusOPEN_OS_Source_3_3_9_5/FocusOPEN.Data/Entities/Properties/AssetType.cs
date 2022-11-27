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
	public partial class AssetType : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetTypeId = null;
		
		// Table variables
		protected string m_name = String.Empty;
		protected bool m_isVisible = false;
		protected bool m_isDeleted = false;
		
		// View Variables
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetType object.
		/// </summary>
		public Nullable <Int32> AssetTypeId
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
				return (m_assetTypeId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Name of the AssetType object.
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
		/// Sets or Returns the IsVisible of the AssetType object.
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
		/// Sets or Returns the IsDeleted of the AssetType object.
		/// </summary>
		public virtual bool IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set 
			{ 
				if (value != m_isDeleted)
				{
					m_ChangedProperties["IsDeleted"] = new ChangedProperty("IsDeleted", m_isDeleted, value);
					
					m_isDeleted = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		
		#endregion
		
		public enum Columns
		{
			AssetTypeId,
			Name,
			IsVisible,
			IsDeleted
		}
	}
}

