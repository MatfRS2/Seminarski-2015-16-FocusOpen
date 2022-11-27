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
	/// This object represents the properties and methods of a HomepageType.
	/// </summary>
	[Serializable]
	public partial class HomepageType : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_homepageTypeId = null;
		
		// Table variables
		protected string m_description = String.Empty;
		protected string m_shortName = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the HomepageType object.
		/// </summary>
		public Nullable <Int32> HomepageTypeId
		{
			get
			{
				return m_homepageTypeId;
			}
			set 
			{
				if (value != m_homepageTypeId)
				{
					m_homepageTypeId = value;
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
				return (m_homepageTypeId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Description of the HomepageType object.
		/// </summary>
		public virtual string Description
		{
			get
			{
				return m_description;
			}
			set 
			{ 
				if ((value != m_description))
				{
					m_ChangedProperties["Description"] = new ChangedProperty("Description", m_description, value);
					
					m_description = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ShortName of the HomepageType object.
		/// </summary>
		public virtual string ShortName
		{
			get
			{
				return m_shortName;
			}
			set 
			{ 
				if ((value != m_shortName))
				{
					m_ChangedProperties["ShortName"] = new ChangedProperty("ShortName", m_shortName, value);
					
					m_shortName = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			HomepageTypeId,
			Description,
			ShortName
		}
	}
}

