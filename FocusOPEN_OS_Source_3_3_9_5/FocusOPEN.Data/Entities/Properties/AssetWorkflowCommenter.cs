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
	/// This object represents the properties and methods of a AssetWorkflowCommenter.
	/// </summary>
	[Serializable]
	public partial class AssetWorkflowCommenter : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetWorkflowCommenterId = null;
		
		// Table variables
		protected int m_userId = 0;
		protected int m_invitingAssetWorkflowUserId = 0;
		protected string m_invitingUserMessage = String.Empty;
		protected string m_comments = String.Empty;
		protected DateTime m_createDate = DateTime.MinValue;
		protected DateTime m_lastUpdate = DateTime.MinValue;
		
		// View Variables
		protected Nullable <Int32> m_invitingUserId = null;
		protected Nullable <Int32> m_assetWorkflowId = null;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetWorkflowCommenter object.
		/// </summary>
		public Nullable <Int32> AssetWorkflowCommenterId
		{
			get
			{
				return m_assetWorkflowCommenterId;
			}
			set 
			{
				if (value != m_assetWorkflowCommenterId)
				{
					m_assetWorkflowCommenterId = value;
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
				return (m_assetWorkflowCommenterId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the UserId of the AssetWorkflowCommenter object.
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
		/// Sets or Returns the InvitingAssetWorkflowUserId of the AssetWorkflowCommenter object.
		/// </summary>
		public virtual int InvitingAssetWorkflowUserId
		{
			get
			{
				return m_invitingAssetWorkflowUserId;
			}
			set 
			{ 
				if ((value != m_invitingAssetWorkflowUserId))
				{
					m_ChangedProperties["InvitingAssetWorkflowUserId"] = new ChangedProperty("InvitingAssetWorkflowUserId", m_invitingAssetWorkflowUserId, value);
					
					m_invitingAssetWorkflowUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the InvitingUserMessage of the AssetWorkflowCommenter object.
		/// </summary>
		public virtual string InvitingUserMessage
		{
			get
			{
				return m_invitingUserMessage;
			}
			set 
			{ 
				if ((value != m_invitingUserMessage))
				{
					m_ChangedProperties["InvitingUserMessage"] = new ChangedProperty("InvitingUserMessage", m_invitingUserMessage, value);
					
					m_invitingUserMessage = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Comments of the AssetWorkflowCommenter object.
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
		/// Sets or Returns the CreateDate of the AssetWorkflowCommenter object.
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
		/// Sets or Returns the LastUpdate of the AssetWorkflowCommenter object.
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
		
		#region View Accessors
		
		public virtual Nullable <Int32> InvitingUserId
		{
			get
			{
				return m_invitingUserId;
			}
			set 
			{ 
				m_invitingUserId = value; 
			}
		}
		public virtual Nullable <Int32> AssetWorkflowId
		{
			get
			{
				return m_assetWorkflowId;
			}
			set 
			{ 
				m_assetWorkflowId = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			AssetWorkflowCommenterId,
			UserId,
			InvitingAssetWorkflowUserId,
			InvitingUserMessage,
			Comments,
			CreateDate,
			LastUpdate
		}
	}
}

