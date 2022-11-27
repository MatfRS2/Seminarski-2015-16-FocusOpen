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
using SearchBuilder=Daydream.Data.SearchBuilder;

namespace FocusOPEN.Data
{
	[Serializable]
	public partial class BrandFinder : AbstractFinder
	{
		#region Column Variables
		
		// Primary Key
		protected Nullable <Int32> m_brandId = null;
		
		// Table columns
		protected string m_name = String.Empty;
		protected string m_shortName = String.Empty;
		protected string m_applicationName = String.Empty;
		protected string m_organisationName = String.Empty;
		protected Nullable <Boolean> m_isMasterBrand = null;
		protected string m_websiteUrl = String.Empty;
		protected string m_emailFrom = String.Empty;
		protected Nullable <Boolean> m_isBrandSelectionAllowed = null;
		protected Nullable <Boolean> m_disablePoweredByLogo = null;
		protected string m_loginPageUpperCopy = String.Empty;
		protected string m_loginPageLowerCopy = String.Empty;
		protected string m_defaultUsageRestrictionsCopy = String.Empty;
		protected string m_myAccountCopy = String.Empty;
		protected string m_adminCopy = String.Empty;
		protected string m_termsConditionsCopy = String.Empty;
		protected string m_privacyPolicyCopy = String.Empty;
		protected Nullable <Boolean> m_hideFilterSearch = null;
		protected Nullable <Boolean> m_hideCategorySearch = null;
		protected Nullable <Boolean> m_directDownloadEnabled = null;
		protected Nullable <Boolean> m_isDeleted = null;
		protected string m_filterMarkup = String.Empty;
		
		// View Variables
		
		#endregion
		
		#region Other Private Variables
		
		private readonly List<Int32> m_brandIdList = new List<Int32>();
		
		// Count of number of find properties set
		protected int m_FindCriteriaCount = 0;
		
		#endregion

		#region Simple Accessors
		
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
					m_FindCriteriaCount++;
				}
			}
		}

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				if (value != m_name)
				{
					m_name = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string ShortName
		{
			get
			{
				return m_shortName;
			}
			set
			{
				if (value != m_shortName)
				{
					m_shortName = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string ApplicationName
		{
			get
			{
				return m_applicationName;
			}
			set
			{
				if (value != m_applicationName)
				{
					m_applicationName = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string OrganisationName
		{
			get
			{
				return m_organisationName;
			}
			set
			{
				if (value != m_organisationName)
				{
					m_organisationName = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsMasterBrand
		{
			get
			{
				return m_isMasterBrand;
			}
			set
			{
				if (value != m_isMasterBrand)
				{
					m_isMasterBrand = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string WebsiteUrl
		{
			get
			{
				return m_websiteUrl;
			}
			set
			{
				if (value != m_websiteUrl)
				{
					m_websiteUrl = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string EmailFrom
		{
			get
			{
				return m_emailFrom;
			}
			set
			{
				if (value != m_emailFrom)
				{
					m_emailFrom = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsBrandSelectionAllowed
		{
			get
			{
				return m_isBrandSelectionAllowed;
			}
			set
			{
				if (value != m_isBrandSelectionAllowed)
				{
					m_isBrandSelectionAllowed = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> DisablePoweredByLogo
		{
			get
			{
				return m_disablePoweredByLogo;
			}
			set
			{
				if (value != m_disablePoweredByLogo)
				{
					m_disablePoweredByLogo = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string LoginPageUpperCopy
		{
			get
			{
				return m_loginPageUpperCopy;
			}
			set
			{
				if (value != m_loginPageUpperCopy)
				{
					m_loginPageUpperCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string LoginPageLowerCopy
		{
			get
			{
				return m_loginPageLowerCopy;
			}
			set
			{
				if (value != m_loginPageLowerCopy)
				{
					m_loginPageLowerCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string DefaultUsageRestrictionsCopy
		{
			get
			{
				return m_defaultUsageRestrictionsCopy;
			}
			set
			{
				if (value != m_defaultUsageRestrictionsCopy)
				{
					m_defaultUsageRestrictionsCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string MyAccountCopy
		{
			get
			{
				return m_myAccountCopy;
			}
			set
			{
				if (value != m_myAccountCopy)
				{
					m_myAccountCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string AdminCopy
		{
			get
			{
				return m_adminCopy;
			}
			set
			{
				if (value != m_adminCopy)
				{
					m_adminCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string TermsConditionsCopy
		{
			get
			{
				return m_termsConditionsCopy;
			}
			set
			{
				if (value != m_termsConditionsCopy)
				{
					m_termsConditionsCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string PrivacyPolicyCopy
		{
			get
			{
				return m_privacyPolicyCopy;
			}
			set
			{
				if (value != m_privacyPolicyCopy)
				{
					m_privacyPolicyCopy = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> HideFilterSearch
		{
			get
			{
				return m_hideFilterSearch;
			}
			set
			{
				if (value != m_hideFilterSearch)
				{
					m_hideFilterSearch = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> HideCategorySearch
		{
			get
			{
				return m_hideCategorySearch;
			}
			set
			{
				if (value != m_hideCategorySearch)
				{
					m_hideCategorySearch = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> DirectDownloadEnabled
		{
			get
			{
				return m_directDownloadEnabled;
			}
			set
			{
				if (value != m_directDownloadEnabled)
				{
					m_directDownloadEnabled = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public Nullable <Boolean> IsDeleted
		{
			get
			{
				return m_isDeleted;
			}
			set
			{
				if (value != m_isDeleted)
				{
					m_isDeleted = value;
					m_FindCriteriaCount++;
				}
			}
		}

		public string FilterMarkup
		{
			get
			{
				return m_filterMarkup;
			}
			set
			{
				if (value != m_filterMarkup)
				{
					m_filterMarkup = value;
					m_FindCriteriaCount++;
				}
			}
		}
		
		public List<Int32> BrandIdList
		{
			get
			{
				return m_brandIdList;
			}
		}
		
		public int FindCriteriaCount
		{
			get
			{
				return m_FindCriteriaCount;
			}
		}
		
		#endregion
		
		#region View Accessors
		
		
		#endregion

		#region IFinder Implementation

		public override string Table
		{
			get
			{
				return "[v_Brand]";
			}
		}

		protected override void SetSearchCriteria (ref SearchBuilder sb)
		{
			if (BrandIdList != null && BrandIdList.Count > 0)
			{
				JoinableList list = new JoinableList(BrandIdList);
				sb.Criteria.Add(string.Format("{0} IN (" + list + ")", Brand.Columns.BrandId));
			}
			
			if (BrandId.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@brandId", Brand.Columns.BrandId));
				sb.AddDataParameter("@brandId", BrandId.Value);
			}
			
			if (Name != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@name", Brand.Columns.Name));
				sb.AddDataParameter("@name", Name);
			}						
	
			if (ShortName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@shortName", Brand.Columns.ShortName));
				sb.AddDataParameter("@shortName", ShortName);
			}						
	
			if (ApplicationName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@applicationName", Brand.Columns.ApplicationName));
				sb.AddDataParameter("@applicationName", ApplicationName);
			}						
	
			if (OrganisationName != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@organisationName", Brand.Columns.OrganisationName));
				sb.AddDataParameter("@organisationName", OrganisationName);
			}						
	
			if (IsMasterBrand.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isMasterBrand", Brand.Columns.IsMasterBrand));
				sb.AddDataParameter("@isMasterBrand", SqlUtils.BitValue(IsMasterBrand.Value));
			}
	
			if (WebsiteUrl != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@websiteUrl", Brand.Columns.WebsiteUrl));
				sb.AddDataParameter("@websiteUrl", WebsiteUrl);
			}						
	
			if (EmailFrom != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@emailFrom", Brand.Columns.EmailFrom));
				sb.AddDataParameter("@emailFrom", EmailFrom);
			}						
	
			if (IsBrandSelectionAllowed.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isBrandSelectionAllowed", Brand.Columns.IsBrandSelectionAllowed));
				sb.AddDataParameter("@isBrandSelectionAllowed", SqlUtils.BitValue(IsBrandSelectionAllowed.Value));
			}
	
			if (DisablePoweredByLogo.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@disablePoweredByLogo", Brand.Columns.DisablePoweredByLogo));
				sb.AddDataParameter("@disablePoweredByLogo", SqlUtils.BitValue(DisablePoweredByLogo.Value));
			}
	
			if (LoginPageUpperCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@loginPageUpperCopy", Brand.Columns.LoginPageUpperCopy));
				sb.AddDataParameter("@loginPageUpperCopy", LoginPageUpperCopy);
			}						
	
			if (LoginPageLowerCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@loginPageLowerCopy", Brand.Columns.LoginPageLowerCopy));
				sb.AddDataParameter("@loginPageLowerCopy", LoginPageLowerCopy);
			}						
	
			if (DefaultUsageRestrictionsCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@defaultUsageRestrictionsCopy", Brand.Columns.DefaultUsageRestrictionsCopy));
				sb.AddDataParameter("@defaultUsageRestrictionsCopy", DefaultUsageRestrictionsCopy);
			}						
	
			if (MyAccountCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@myAccountCopy", Brand.Columns.MyAccountCopy));
				sb.AddDataParameter("@myAccountCopy", MyAccountCopy);
			}						
	
			if (AdminCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@adminCopy", Brand.Columns.AdminCopy));
				sb.AddDataParameter("@adminCopy", AdminCopy);
			}						
	
			if (TermsConditionsCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@termsConditionsCopy", Brand.Columns.TermsConditionsCopy));
				sb.AddDataParameter("@termsConditionsCopy", TermsConditionsCopy);
			}						
	
			if (PrivacyPolicyCopy != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@privacyPolicyCopy", Brand.Columns.PrivacyPolicyCopy));
				sb.AddDataParameter("@privacyPolicyCopy", PrivacyPolicyCopy);
			}						
	
			if (HideFilterSearch.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@hideFilterSearch", Brand.Columns.HideFilterSearch));
				sb.AddDataParameter("@hideFilterSearch", SqlUtils.BitValue(HideFilterSearch.Value));
			}
	
			if (HideCategorySearch.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@hideCategorySearch", Brand.Columns.HideCategorySearch));
				sb.AddDataParameter("@hideCategorySearch", SqlUtils.BitValue(HideCategorySearch.Value));
			}
	
			if (DirectDownloadEnabled.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@directDownloadEnabled", Brand.Columns.DirectDownloadEnabled));
				sb.AddDataParameter("@directDownloadEnabled", SqlUtils.BitValue(DirectDownloadEnabled.Value));
			}
	
			if (IsDeleted.HasValue)
			{
				sb.Criteria.Add(string.Format("{0}=@isDeleted", Brand.Columns.IsDeleted));
				sb.AddDataParameter("@isDeleted", SqlUtils.BitValue(IsDeleted.Value));
			}
	
			if (FilterMarkup != String.Empty)
			{
				sb.Criteria.Add(string.Format("{0}=@filterMarkup", Brand.Columns.FilterMarkup));
				sb.AddDataParameter("@filterMarkup", FilterMarkup);
			}						
	
			SetCustomSearchCriteria(ref sb);
		}

		#endregion
	}
}