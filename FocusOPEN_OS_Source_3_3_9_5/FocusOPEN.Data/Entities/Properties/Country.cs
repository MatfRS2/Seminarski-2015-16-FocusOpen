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
	/// This object represents the properties and methods of a Country.
	/// </summary>
	[Serializable]
	public partial class Country : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_countryId = null;
		
		// Table variables
		protected string m_code = String.Empty;
		protected string m_name = String.Empty;
		protected int m_rank = 0;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Country object.
		/// </summary>
		public Nullable <Int32> CountryId
		{
			get
			{
				return m_countryId;
			}
			set 
			{
				if (value != m_countryId)
				{
					m_countryId = value;
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
				return (m_countryId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Code of the Country object.
		/// </summary>
		public virtual string Code
		{
			get
			{
				return m_code;
			}
			set 
			{ 
				if ((value != m_code))
				{
					m_ChangedProperties["Code"] = new ChangedProperty("Code", m_code, value);
					
					m_code = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Name of the Country object.
		/// </summary>
		public virtual string Name
		{
			get
			{
				return m_name;
			}
			set 
			{ 
				if ((value != m_name))
				{
					m_ChangedProperties["Name"] = new ChangedProperty("Name", m_name, value);
					
					m_name = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Rank of the Country object.
		/// </summary>
		public virtual int Rank
		{
			get
			{
				return m_rank;
			}
			set 
			{ 
				if ((value != m_rank))
				{
					m_ChangedProperties["Rank"] = new ChangedProperty("Rank", m_rank, value);
					
					m_rank = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			CountryId,
			Code,
			Name,
			Rank
		}
	}
}

