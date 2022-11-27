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
	public partial class Workflow : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_workflowId = null;
		
		// Table variables
		protected string m_name = String.Empty;
		protected int m_brandId = 0;
		protected bool m_isDeleted = false;
		
		// View Variables
		protected string m_brandName = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Workflow object.
		/// </summary>
		public Nullable <Int32> WorkflowId
		{
			get
			{
				return m_workflowId;
			}
			set 
			{
				if (value != m_workflowId)
				{
					m_workflowId = value;
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
				return (m_workflowId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Name of the Workflow object.
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
		/// Sets or Returns the BrandId of the Workflow object.
		/// </summary>
		public virtual int BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{ 
				if (value != m_brandId)
				{
					m_ChangedProperties["BrandId"] = new ChangedProperty("BrandId", m_brandId, value);
					
					m_brandId = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDeleted of the Workflow object.
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
		
		public virtual string BrandName
		{
			get
			{
				return m_brandName;
			}
			set 
			{ 
				m_brandName = value; 
			}
		}
		
		#endregion
		
		public enum Columns
		{
			WorkflowId,
			Name,
			BrandId,
			IsDeleted
		}
	}
}

