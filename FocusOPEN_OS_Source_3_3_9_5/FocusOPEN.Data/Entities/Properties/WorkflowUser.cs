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
	public partial class WorkflowUser : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_workflowUserId = null;
		
		// Table variables
		protected int m_workflowId = 0;
		protected int m_userId = 0;
		protected int m_position = 0;
		protected DateTime m_dateAdded = DateTime.MinValue;
		
		// View Variables
		protected string m_userFullName = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the WorkflowUser object.
		/// </summary>
		public Nullable <Int32> WorkflowUserId
		{
			get
			{
				return m_workflowUserId;
			}
			set 
			{
				if (value != m_workflowUserId)
				{
					m_workflowUserId = value;
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
				return (m_workflowUserId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the WorkflowId of the WorkflowUser object.
		/// </summary>
		public virtual int WorkflowId
		{
			get
			{
				return m_workflowId;
			}
			set 
			{ 
				if (value != m_workflowId)
				{
					m_ChangedProperties["WorkflowId"] = new ChangedProperty("WorkflowId", m_workflowId, value);
					
					m_workflowId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the UserId of the WorkflowUser object.
		/// </summary>
		public virtual int UserId
		{
			get
			{
				return m_userId;
			}
			set 
			{ 
				if (value != m_userId)
				{
					m_ChangedProperties["UserId"] = new ChangedProperty("UserId", m_userId, value);
					
					m_userId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Position of the WorkflowUser object.
		/// </summary>
		public virtual int Position
		{
			get
			{
				return m_position;
			}
			set 
			{ 
				if (value != m_position)
				{
					m_ChangedProperties["Position"] = new ChangedProperty("Position", m_position, value);
					
					m_position = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DateAdded of the WorkflowUser object.
		/// </summary>
		public virtual DateTime DateAdded
		{
			get
			{
				return m_dateAdded;
			}
			set 
			{ 
				if (value != m_dateAdded)
				{
					m_ChangedProperties["DateAdded"] = new ChangedProperty("DateAdded", m_dateAdded, value);
					
					m_dateAdded = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		public virtual string UserFullName
		{
			get
			{
				return m_userFullName;
			}
			set 
			{ 
				m_userFullName = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			WorkflowUserId,
			WorkflowId,
			UserId,
			Position,
			DateAdded
		}
	}
}

