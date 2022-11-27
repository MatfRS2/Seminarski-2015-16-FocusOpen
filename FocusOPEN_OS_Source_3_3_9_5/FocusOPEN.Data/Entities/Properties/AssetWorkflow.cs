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
	/// This object represents the properties and methods of a AssetWorkflow.
	/// </summary>
	[Serializable]
	public partial class AssetWorkflow : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetWorkflowId = null;
		
		// Table variables
		protected int m_assetId = 0;
		protected int m_submittedByUserId = 0;
		protected DateTime m_createDate = DateTime.MinValue;
		protected bool m_isComplete = false;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetWorkflow object.
		/// </summary>
		public Nullable <Int32> AssetWorkflowId
		{
			get
			{
				return m_assetWorkflowId;
			}
			set 
			{
				if (value != m_assetWorkflowId)
				{
					m_assetWorkflowId = value;
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
				return (m_assetWorkflowId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the AssetId of the AssetWorkflow object.
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
		/// Sets or Returns the SubmittedByUserId of the AssetWorkflow object.
		/// </summary>
		public virtual int SubmittedByUserId
		{
			get
			{
				return m_submittedByUserId;
			}
			set 
			{ 
				if ((value != m_submittedByUserId))
				{
					m_ChangedProperties["SubmittedByUserId"] = new ChangedProperty("SubmittedByUserId", m_submittedByUserId, value);
					
					m_submittedByUserId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the CreateDate of the AssetWorkflow object.
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
		/// Sets or Returns the IsComplete of the AssetWorkflow object.
		/// </summary>
		public virtual bool IsComplete
		{
			get
			{
				return m_isComplete;
			}
			set 
			{ 
				if ((value != m_isComplete))
				{
					m_ChangedProperties["IsComplete"] = new ChangedProperty("IsComplete", m_isComplete, value);
					
					m_isComplete = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			AssetWorkflowId,
			AssetId,
			SubmittedByUserId,
			CreateDate,
			IsComplete
		}
	}
}

