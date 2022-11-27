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
	/// This object represents the properties and methods of a UserPasswordHistory.
	/// </summary>
	[Serializable]
	public partial class UserPasswordHistory : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_userPasswordHistoryId = null;
		
		// Table variables
		protected int m_userId = 0;
		protected string m_password = String.Empty;
		protected DateTime m_date = DateTime.MinValue;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the UserPasswordHistory object.
		/// </summary>
		public Nullable <Int32> UserPasswordHistoryId
		{
			get
			{
				return m_userPasswordHistoryId;
			}
			set 
			{
				if (value != m_userPasswordHistoryId)
				{
					m_userPasswordHistoryId = value;
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
				return (m_userPasswordHistoryId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the UserId of the UserPasswordHistory object.
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
		/// Sets or Returns the Password of the UserPasswordHistory object.
		/// </summary>
		public virtual string Password
		{
			get
			{
				return m_password;
			}
			set 
			{ 
				if ((value != m_password))
				{
					m_ChangedProperties["Password"] = new ChangedProperty("Password", m_password, value);
					
					m_password = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Date of the UserPasswordHistory object.
		/// </summary>
		public virtual DateTime Date
		{
			get
			{
				return m_date;
			}
			set 
			{ 
				if ((value != m_date))
				{
					m_ChangedProperties["Date"] = new ChangedProperty("Date", m_date, value);
					
					m_date = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			UserPasswordHistoryId,
			UserId,
			Password,
			Date
		}
	}
}

