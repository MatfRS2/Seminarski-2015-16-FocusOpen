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
	public partial class AssetLink : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetLinkId = null;
		
		// Table variables
		protected int m_assetId = 0;
		protected int m_linkedAssetId = 0;
		
		// View Variables
		protected string m_linkedAssetTitle = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetLink object.
		/// </summary>
		public Nullable <Int32> AssetLinkId
		{
			get
			{
				return m_assetLinkId;
			}
			set 
			{
				if (value != m_assetLinkId)
				{
					m_assetLinkId = value;
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
				return (m_assetLinkId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetId of the AssetLink object.
		/// </summary>
		public virtual int AssetId
		{
			get
			{
				return m_assetId;
			}
			set 
			{ 
				if (value != m_assetId)
				{
					m_ChangedProperties["AssetId"] = new ChangedProperty("AssetId", m_assetId, value);
					
					m_assetId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LinkedAssetId of the AssetLink object.
		/// </summary>
		public virtual int LinkedAssetId
		{
			get
			{
				return m_linkedAssetId;
			}
			set 
			{ 
				if (value != m_linkedAssetId)
				{
					m_ChangedProperties["LinkedAssetId"] = new ChangedProperty("LinkedAssetId", m_linkedAssetId, value);
					
					m_linkedAssetId = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual string LinkedAssetTitle
		{
			get
			{
				return m_linkedAssetTitle;
			}
			set 
			{ 
				m_linkedAssetTitle = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			AssetLinkId,
			AssetId,
			LinkedAssetId
		}
	}
}

