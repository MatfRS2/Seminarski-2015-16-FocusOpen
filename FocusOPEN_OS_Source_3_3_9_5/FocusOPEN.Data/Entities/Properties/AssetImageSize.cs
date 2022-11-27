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
	/// This object represents the properties and methods of a AssetImageSize.
	/// </summary>
	[Serializable]
	public partial class AssetImageSize : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_assetImageSizeId = null;
		
		// Table variables
		protected string m_description = String.Empty;
		protected int m_height = 0;
		protected int m_width = 0;
		protected int m_dotsPerInch = 0;
		protected string m_fileSuffix = String.Empty;
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the AssetImageSize object.
		/// </summary>
		public Nullable <Int32> AssetImageSizeId
		{
			get
			{
				return m_assetImageSizeId;
			}
			set 
			{
				if (value != m_assetImageSizeId)
				{
					m_assetImageSizeId = value;
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
				return (m_assetImageSizeId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Description of the AssetImageSize object.
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
		/// Sets or Returns the Height of the AssetImageSize object.
		/// </summary>
		public virtual int Height
		{
			get
			{
				return m_height;
			}
			set 
			{ 
				if ((value != m_height))
				{
					m_ChangedProperties["Height"] = new ChangedProperty("Height", m_height, value);
					
					m_height = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the Width of the AssetImageSize object.
		/// </summary>
		public virtual int Width
		{
			get
			{
				return m_width;
			}
			set 
			{ 
				if ((value != m_width))
				{
					m_ChangedProperties["Width"] = new ChangedProperty("Width", m_width, value);
					
					m_width = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DotsPerInch of the AssetImageSize object.
		/// </summary>
		public virtual int DotsPerInch
		{
			get
			{
				return m_dotsPerInch;
			}
			set 
			{ 
				if ((value != m_dotsPerInch))
				{
					m_ChangedProperties["DotsPerInch"] = new ChangedProperty("DotsPerInch", m_dotsPerInch, value);
					
					m_dotsPerInch = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the FileSuffix of the AssetImageSize object.
		/// </summary>
		public virtual string FileSuffix
		{
			get
			{
				return m_fileSuffix;
			}
			set 
			{ 
				if ((value != m_fileSuffix))
				{
					m_ChangedProperties["FileSuffix"] = new ChangedProperty("FileSuffix", m_fileSuffix, value);
					
					m_fileSuffix = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		
		public enum Columns
		{
			AssetImageSizeId,
			Description,
			Height,
			Width,
			DotsPerInch,
			FileSuffix
		}
	}
}

