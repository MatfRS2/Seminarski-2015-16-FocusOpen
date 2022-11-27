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
	/// This object represents the properties and methods of a AssetWorkflowUser.
	/// </summary>
	[Serializable]
	public partial class AssetWorkflowUser : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetWorkflowUserId = null;
		
		// Table variables
		protected int m_assetWorkflowId = 0;
		protected int m_userId = 0;
		protected int m_position = 0;
		protected string m_comments = String.Empty;
		protected int m_assetWorkflowUserStatusId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		protected bool m_isDeleted = false;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetWorkflowUser object.
		/// </summary>
		public Nullable <Int32> AssetWorkflowUserId
		{
			get
			{
				return m_assetWorkflowUserId;
			}
			set 
			{
				if (value != m_assetWorkflowUserId)
				{
					m_assetWorkflowUserId = value;
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
				return (m_assetWorkflowUserId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetWorkflowId of the AssetWorkflowUser object.
		/// </summary>
		public virtual int AssetWorkflowId
		{
			get
			{
				return m_assetWorkflowId;
			}
			set 
			{ 
				if ((value != m_assetWorkflowId))
				{
					m_ChangedProperties["AssetWorkflowId"] = new ChangedProperty("AssetWorkflowId", m_assetWorkflowId, value);
					
					m_assetWorkflowId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserId of the AssetWorkflowUser object.
		/// </summary>
		public virtual int UserId
		{
			get
			{
				return m_userId;
			}
			set 
			{ 
				if ((value != m_userId))
				{
					m_ChangedProperties["UserId"] = new ChangedProperty("UserId", m_userId, value);
					
					m_userId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Position of the AssetWorkflowUser object.
		/// </summary>
		public virtual int Position
		{
			get
			{
				return m_position;
			}
			set 
			{ 
				if ((value != m_position))
				{
					m_ChangedProperties["Position"] = new ChangedProperty("Position", m_position, value);
					
					m_position = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Comments of the AssetWorkflowUser object.
		/// </summary>
		public virtual string Comments
		{
			get
			{
				return m_comments;
			}
			set 
			{ 
				if ((value != m_comments))
				{
					m_ChangedProperties["Comments"] = new ChangedProperty("Comments", m_comments, value);
					
					m_comments = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AssetWorkflowUserStatusId of the AssetWorkflowUser object.
		/// </summary>
		public virtual int AssetWorkflowUserStatusId
		{
			get
			{
				return m_assetWorkflowUserStatusId;
			}
			set 
			{ 
				if ((value != m_assetWorkflowUserStatusId))
				{
					m_ChangedProperties["AssetWorkflowUserStatusId"] = new ChangedProperty("AssetWorkflowUserStatusId", m_assetWorkflowUserStatusId, value);
					
					m_assetWorkflowUserStatusId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreateDate of the AssetWorkflowUser object.
		/// </summary>
		public virtual DateTime CreateDate
		{
			get
			{
				return m_createDate;
			}
			set 
			{ 
				if ((value != m_createDate))
				{
					m_ChangedProperties["CreateDate"] = new ChangedProperty("CreateDate", m_createDate, value);
					
					m_createDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LastUpdate of the AssetWorkflowUser object.
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

		/// <summary>
		/// Sets or Returns the IsDeleted of the AssetWorkflowUser object.
		/// </summary>
		public virtual bool IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set 
			{ 
				if ((value != m_isDeleted))
				{
					m_ChangedProperties["IsDeleted"] = new ChangedProperty("IsDeleted", m_isDeleted, value);
					
					m_isDeleted = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			AssetWorkflowUserId,
			AssetWorkflowId,
			UserId,
			Position,
			Comments,
			AssetWorkflowUserStatusId,
			CreateDate,
			LastUpdate,
			IsDeleted
		}
	}
}

