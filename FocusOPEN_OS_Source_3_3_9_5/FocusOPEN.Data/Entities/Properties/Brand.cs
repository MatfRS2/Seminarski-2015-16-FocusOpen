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
	[Serializable]
	public partial class Brand : AbstractEntity, INullable, ICloneable
	{
		#region Private Variables
		
		// Primary Key
		protected Nullable <Int32> m_brandId = null;
		
		// Table variables
		protected string m_name = String.Empty;
		protected string m_shortName = String.Empty;
		protected string m_applicationName = String.Empty;
		protected string m_organisationName = String.Empty;
		protected bool m_isMasterBrand = false;
		protected string m_websiteUrl = String.Empty;
		protected string m_emailFrom = String.Empty;
		protected bool m_isBrandSelectionAllowed = false;
		protected bool m_disablePoweredByLogo = false;
		protected string m_loginPageUpperCopy = String.Empty;
		protected string m_loginPageLowerCopy = String.Empty;
		protected string m_defaultUsageRestrictionsCopy = String.Empty;
		protected string m_myAccountCopy = String.Empty;
		protected string m_adminCopy = String.Empty;
		protected string m_termsConditionsCopy = String.Empty;
		protected string m_privacyPolicyCopy = String.Empty;
		protected bool m_hideFilterSearch = false;
		protected bool m_hideCategorySearch = false;
		protected bool m_directDownloadEnabled = false;
		protected bool m_isDeleted = false;
		protected string m_filterMarkup = String.Empty;
		
		// View Variables
		
		// Other variables
		private readonly Dictionary<String, ChangedProperty> m_ChangedProperties = new Dictionary<String, ChangedProperty>();
		
		#endregion
		
		#region Standard Accessors
		
		/// <summary>
		/// Returns the Id of the Brand object.
		/// </summary>
		public Nullable <Int32> BrandId
		{
			get
			{
				return m_brandId;
			}
			set 
			{
				if (value != m_brandId)
				{
					m_brandId = value;
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
				return (m_brandId.HasValue == false);
			}
		}
		
		#endregion
		
		#region Table Accessors
		
		/// <summary>
		/// Sets or Returns the Name of the Brand object.
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
		/// Sets or Returns the ShortName of the Brand object.
		/// </summary>
		public virtual string ShortName
		{
			get
			{
				return m_shortName;
			}
			set 
			{ 
				if (value != m_shortName)
				{
					m_ChangedProperties["ShortName"] = new ChangedProperty("ShortName", m_shortName, value);
					
					m_shortName = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the ApplicationName of the Brand object.
		/// </summary>
		public virtual string ApplicationName
		{
			get
			{
				return m_applicationName;
			}
			set 
			{ 
				if (value != m_applicationName)
				{
					m_ChangedProperties["ApplicationName"] = new ChangedProperty("ApplicationName", m_applicationName, value);
					
					m_applicationName = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the OrganisationName of the Brand object.
		/// </summary>
		public virtual string OrganisationName
		{
			get
			{
				return m_organisationName;
			}
			set 
			{ 
				if (value != m_organisationName)
				{
					m_ChangedProperties["OrganisationName"] = new ChangedProperty("OrganisationName", m_organisationName, value);
					
					m_organisationName = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsMasterBrand of the Brand object.
		/// </summary>
		public virtual bool IsMasterBrand
		{
			get
			{
				return m_isMasterBrand;
			}
			set 
			{ 
				if (value != m_isMasterBrand)
				{
					m_ChangedProperties["IsMasterBrand"] = new ChangedProperty("IsMasterBrand", m_isMasterBrand, value);
					
					m_isMasterBrand = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the WebsiteUrl of the Brand object.
		/// </summary>
		public virtual string WebsiteUrl
		{
			get
			{
				return m_websiteUrl;
			}
			set 
			{ 
				if (value != m_websiteUrl)
				{
					m_ChangedProperties["WebsiteUrl"] = new ChangedProperty("WebsiteUrl", m_websiteUrl, value);
					
					m_websiteUrl = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the EmailFrom of the Brand object.
		/// </summary>
		public virtual string EmailFrom
		{
			get
			{
				return m_emailFrom;
			}
			set 
			{ 
				if (value != m_emailFrom)
				{
					m_ChangedProperties["EmailFrom"] = new ChangedProperty("EmailFrom", m_emailFrom, value);
					
					m_emailFrom = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsBrandSelectionAllowed of the Brand object.
		/// </summary>
		public virtual bool IsBrandSelectionAllowed
		{
			get
			{
				return m_isBrandSelectionAllowed;
			}
			set 
			{ 
				if (value != m_isBrandSelectionAllowed)
				{
					m_ChangedProperties["IsBrandSelectionAllowed"] = new ChangedProperty("IsBrandSelectionAllowed", m_isBrandSelectionAllowed, value);
					
					m_isBrandSelectionAllowed = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DisablePoweredByLogo of the Brand object.
		/// </summary>
		public virtual bool DisablePoweredByLogo
		{
			get
			{
				return m_disablePoweredByLogo;
			}
			set 
			{ 
				if (value != m_disablePoweredByLogo)
				{
					m_ChangedProperties["DisablePoweredByLogo"] = new ChangedProperty("DisablePoweredByLogo", m_disablePoweredByLogo, value);
					
					m_disablePoweredByLogo = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LoginPageUpperCopy of the Brand object.
		/// </summary>
		public virtual string LoginPageUpperCopy
		{
			get
			{
				return m_loginPageUpperCopy;
			}
			set 
			{ 
				if (value != m_loginPageUpperCopy)
				{
					m_ChangedProperties["LoginPageUpperCopy"] = new ChangedProperty("LoginPageUpperCopy", m_loginPageUpperCopy, value);
					
					m_loginPageUpperCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the LoginPageLowerCopy of the Brand object.
		/// </summary>
		public virtual string LoginPageLowerCopy
		{
			get
			{
				return m_loginPageLowerCopy;
			}
			set 
			{ 
				if (value != m_loginPageLowerCopy)
				{
					m_ChangedProperties["LoginPageLowerCopy"] = new ChangedProperty("LoginPageLowerCopy", m_loginPageLowerCopy, value);
					
					m_loginPageLowerCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DefaultUsageRestrictionsCopy of the Brand object.
		/// </summary>
		public virtual string DefaultUsageRestrictionsCopy
		{
			get
			{
				return m_defaultUsageRestrictionsCopy;
			}
			set 
			{ 
				if (value != m_defaultUsageRestrictionsCopy)
				{
					m_ChangedProperties["DefaultUsageRestrictionsCopy"] = new ChangedProperty("DefaultUsageRestrictionsCopy", m_defaultUsageRestrictionsCopy, value);
					
					m_defaultUsageRestrictionsCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the MyAccountCopy of the Brand object.
		/// </summary>
		public virtual string MyAccountCopy
		{
			get
			{
				return m_myAccountCopy;
			}
			set 
			{ 
				if (value != m_myAccountCopy)
				{
					m_ChangedProperties["MyAccountCopy"] = new ChangedProperty("MyAccountCopy", m_myAccountCopy, value);
					
					m_myAccountCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the AdminCopy of the Brand object.
		/// </summary>
		public virtual string AdminCopy
		{
			get
			{
				return m_adminCopy;
			}
			set 
			{ 
				if (value != m_adminCopy)
				{
					m_ChangedProperties["AdminCopy"] = new ChangedProperty("AdminCopy", m_adminCopy, value);
					
					m_adminCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the TermsConditionsCopy of the Brand object.
		/// </summary>
		public virtual string TermsConditionsCopy
		{
			get
			{
				return m_termsConditionsCopy;
			}
			set 
			{ 
				if (value != m_termsConditionsCopy)
				{
					m_ChangedProperties["TermsConditionsCopy"] = new ChangedProperty("TermsConditionsCopy", m_termsConditionsCopy, value);
					
					m_termsConditionsCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the PrivacyPolicyCopy of the Brand object.
		/// </summary>
		public virtual string PrivacyPolicyCopy
		{
			get
			{
				return m_privacyPolicyCopy;
			}
			set 
			{ 
				if (value != m_privacyPolicyCopy)
				{
					m_ChangedProperties["PrivacyPolicyCopy"] = new ChangedProperty("PrivacyPolicyCopy", m_privacyPolicyCopy, value);
					
					m_privacyPolicyCopy = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the HideFilterSearch of the Brand object.
		/// </summary>
		public virtual bool HideFilterSearch
		{
			get
			{
				return m_hideFilterSearch;
			}
			set 
			{ 
				if (value != m_hideFilterSearch)
				{
					m_ChangedProperties["HideFilterSearch"] = new ChangedProperty("HideFilterSearch", m_hideFilterSearch, value);
					
					m_hideFilterSearch = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the HideCategorySearch of the Brand object.
		/// </summary>
		public virtual bool HideCategorySearch
		{
			get
			{
				return m_hideCategorySearch;
			}
			set 
			{ 
				if (value != m_hideCategorySearch)
				{
					m_ChangedProperties["HideCategorySearch"] = new ChangedProperty("HideCategorySearch", m_hideCategorySearch, value);
					
					m_hideCategorySearch = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the DirectDownloadEnabled of the Brand object.
		/// </summary>
		public virtual bool DirectDownloadEnabled
		{
			get
			{
				return m_directDownloadEnabled;
			}
			set 
			{ 
				if (value != m_directDownloadEnabled)
				{
					m_ChangedProperties["DirectDownloadEnabled"] = new ChangedProperty("DirectDownloadEnabled", m_directDownloadEnabled, value);
					
					m_directDownloadEnabled = value; 
					m_isDirty = true;
				}
			}
		}

		/// <summary>
		/// Sets or Returns the IsDeleted of the Brand object.
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

		/// <summary>
		/// Sets or Returns the FilterMarkup of the Brand object.
		/// </summary>
		public virtual string FilterMarkup
		{
			get
			{
				return m_filterMarkup;
			}
			set 
			{ 
				if (value != m_filterMarkup)
				{
					m_ChangedProperties["FilterMarkup"] = new ChangedProperty("FilterMarkup", m_filterMarkup, value);
					
					m_filterMarkup = value; 
					m_isDirty = true;
				}
			}
		}

		#endregion
		
		#region View Accessors
		
		
		#endregion
		
		public enum Columns
		{
			BrandId,
			Name,
			ShortName,
			ApplicationName,
			OrganisationName,
			IsMasterBrand,
			WebsiteUrl,
			EmailFrom,
			IsBrandSelectionAllowed,
			DisablePoweredByLogo,
			LoginPageUpperCopy,
			LoginPageLowerCopy,
			DefaultUsageRestrictionsCopy,
			MyAccountCopy,
			AdminCopy,
			TermsConditionsCopy,
			PrivacyPolicyCopy,
			HideFilterSearch,
			HideCategorySearch,
			DirectDownloadEnabled,
			IsDeleted,
			FilterMarkup
		}
	}
}

