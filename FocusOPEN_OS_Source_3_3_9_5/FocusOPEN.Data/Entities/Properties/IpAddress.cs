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
	/// This object represents the properties and methods of a IpAddress.
	/// </summary>
	[Serializable]
	public partial class IpAddress : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_ipAddressId = null;
		
		// Table variables
		protected string m_ipAddressValue = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the IpAddress object.
		/// </summary>
		public Nullable <Int32> IpAddressId
		{
			get
			{
				return m_ipAddressId;
			}
			set 
			{
				if (value != m_ipAddressId)
				{
					m_ipAddressId = value;
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
				return (m_ipAddressId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the IpAddressValue of the IpAddress object.
		/// </summary>
		public virtual string IpAddressValue
		{
			get
			{
				return m_ipAddressValue;
			}
			set 
			{ 
				if ((value != m_ipAddressValue))
				{
					m_ChangedProperties["IpAddressValue"] = new ChangedProperty("IpAddressValue", m_ipAddressValue, value);
					
					m_ipAddressValue = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			IpAddressId,
			IpAddressValue
		}
	}
}

