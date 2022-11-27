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
	/// This object represents the properties and methods of a Cart.
	/// </summary>
	[Serializable]
	public partial class Cart : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_cartId = null;
		
		// Table variables
		protected int m_userId = 0;
		protected int m_assetId = 0;
		protected string m_notes = String.Empty;
		protected Nullable <DateTime> m_requiredByDate = null;
		protected DateTime m_dateAdded = DateTime.MinValue;
		
		// View Variables
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Cart object.
		/// </summary>
		public Nullable <Int32> CartId
		{
			get
			{
				return m_cartId;
			}
			set 
			{
				if (value != m_cartId)
				{
					m_cartId = value;
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
				return (m_cartId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the UserId of the Cart object.
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
		/// Sets or Returns the AssetId of the Cart object.
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
		/// Sets or Returns the Notes of the Cart object.
		/// </summary>
		public virtual string Notes
		{
			get
			{
				return m_notes;
			}
			set 
			{ 
				if ((value != m_notes))
				{
					m_ChangedProperties["Notes"] = new ChangedProperty("Notes", m_notes, value);
					
					m_notes = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the RequiredByDate of the Cart object.
		/// </summary>
		public virtual Nullable <DateTime> RequiredByDate
		{
			get
			{
				return m_requiredByDate;
			}
			set 
			{ 
				if ((value != m_requiredByDate))
				{
					m_ChangedProperties["RequiredByDate"] = new ChangedProperty("RequiredByDate", m_requiredByDate, value);
					
					m_requiredByDate = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DateAdded of the Cart object.
		/// </summary>
		public virtual DateTime DateAdded
		{
			get
			{
				return m_dateAdded;
			}
			set 
			{ 
				if ((value != m_dateAdded))
				{
					m_ChangedProperties["DateAdded"] = new ChangedProperty("DateAdded", m_dateAdded, value);
					
					m_dateAdded = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		
		#endregion
		
		public enum Columns
		{
			CartId,
			UserId,
			AssetId,
			Notes,
			RequiredByDate,
			DateAdded
		}
	}
}

