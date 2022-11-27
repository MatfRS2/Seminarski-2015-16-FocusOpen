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

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullBrand : Brand
	{
		#region Singleton implementation

		private NullBrand()
		{
		}

		private static readonly NullBrand m_instance = new NullBrand();

		public static NullBrand Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Name of the Brand object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ShortName of the Brand object.
		/// </summary>
		public override string ShortName
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ApplicationName of the Brand object.
		/// </summary>
		public override string ApplicationName
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the OrganisationName of the Brand object.
		/// </summary>
		public override string OrganisationName
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsMasterBrand of the Brand object.
		/// </summary>
		public override bool IsMasterBrand
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the WebsiteUrl of the Brand object.
		/// </summary>
		public override string WebsiteUrl
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the EmailFrom of the Brand object.
		/// </summary>
		public override string EmailFrom
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsBrandSelectionAllowed of the Brand object.
		/// </summary>
		public override bool IsBrandSelectionAllowed
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the DisablePoweredByLogo of the Brand object.
		/// </summary>
		public override bool DisablePoweredByLogo
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the LoginPageUpperCopy of the Brand object.
		/// </summary>
		public override string LoginPageUpperCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the LoginPageLowerCopy of the Brand object.
		/// </summary>
		public override string LoginPageLowerCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the DefaultUsageRestrictionsCopy of the Brand object.
		/// </summary>
		public override string DefaultUsageRestrictionsCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the MyAccountCopy of the Brand object.
		/// </summary>
		public override string MyAccountCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the AdminCopy of the Brand object.
		/// </summary>
		public override string AdminCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the TermsConditionsCopy of the Brand object.
		/// </summary>
		public override string TermsConditionsCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the PrivacyPolicyCopy of the Brand object.
		/// </summary>
		public override string PrivacyPolicyCopy
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the HideFilterSearch of the Brand object.
		/// </summary>
		public override bool HideFilterSearch
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the HideCategorySearch of the Brand object.
		/// </summary>
		public override bool HideCategorySearch
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the DirectDownloadEnabled of the Brand object.
		/// </summary>
		public override bool DirectDownloadEnabled
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDeleted of the Brand object.
		/// </summary>
		public override bool IsDeleted
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the FilterMarkup of the Brand object.
		/// </summary>
		public override string FilterMarkup
		{
			get { return String.Empty; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

