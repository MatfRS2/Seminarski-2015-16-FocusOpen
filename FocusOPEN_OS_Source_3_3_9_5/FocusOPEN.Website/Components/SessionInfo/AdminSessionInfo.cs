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
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	[Serializable]
	public class AdminSessionInfo
	{
		#region Private variables

		private AdminSavedAssetSearch m_AdminSavedAssetSearch;
		private AdminSavedUserSearch m_AdminSavedUserSearch;
		private AdminSavedOrderSearch m_adminSavedOrderSearch;
		private AdminSavedAuditUserHistorySearch m_adminSavedAuditUserHistorySearch;
		private AdminSavedAssetPopularitySearch m_AdminSavedAssetPopularitySearch;
		private List<Int32> m_UploadedAssetsList;
		private AssetPager m_AssetPager;
        private bool m_ShowUploadExtendedOptions;
        private ContextType m_AssetContext;
		#endregion

		#region Accessors

		public AdminSavedAssetSearch AdminSavedAssetSearch
		{
			get
			{
				return m_AdminSavedAssetSearch;
			}
			set
			{
				m_AdminSavedAssetSearch = value;
			}
		}

		public AdminSavedUserSearch AdminSavedUserSearch
		{
			get
			{
				return m_AdminSavedUserSearch;
			}
			set
			{
				m_AdminSavedUserSearch = value;
			}
		}

		public AdminSavedOrderSearch AdminSavedOrderSearch
		{
			get
			{
				return m_adminSavedOrderSearch;
			}
			set
			{
				m_adminSavedOrderSearch = value;
			}
		}

		public AdminSavedAuditUserHistorySearch AdminSavedAuditUserHistorySearch
		{
			get
			{
				return m_adminSavedAuditUserHistorySearch;
			}
			set
			{
				m_adminSavedAuditUserHistorySearch = value;
			}
		}

		public AdminSavedAssetPopularitySearch AdminSavedAssetPopularitySearch
		{
			get
			{
				return m_AdminSavedAssetPopularitySearch;
			}
			set
			{
				m_AdminSavedAssetPopularitySearch = value;
			}
		}

		public List<int> UploadedAssetsList
		{
			get
			{
				return m_UploadedAssetsList;
			}
		}

		public AssetPager AssetPager
		{
			get
			{
				return m_AssetPager;
			}
			set
			{
				m_AssetPager = value;
			}
		}

        /// <summary>
        /// Stores the current preferences for the extended options on the Asset Upload form.
        /// </summary>
        public bool ShowUploadExtendedOptions
        {
            get
            {
                return m_ShowUploadExtendedOptions;
            }
            set
            {
                m_ShowUploadExtendedOptions = value;
            }
        }


        /// <summary>
        /// Stores the current context for displaying the asset previews
        /// </summary>
        public ContextType AssetContext
        {
            get
            {
                return m_AssetContext;
            }
            set
            {
                m_AssetContext = value;
            }
        }




		#endregion

		public AdminSessionInfo()
		{
			Reset();
		}

		public void Reset()
		{
			m_AdminSavedAssetSearch = new AdminSavedAssetSearch();
			m_AdminSavedUserSearch = new AdminSavedUserSearch();
			m_adminSavedOrderSearch = new AdminSavedOrderSearch();
			m_adminSavedAuditUserHistorySearch = new AdminSavedAuditUserHistorySearch();
			m_AdminSavedAssetPopularitySearch = new AdminSavedAssetPopularitySearch();
			m_UploadedAssetsList = new List<int>();
			m_AssetPager = new AssetPager();
            m_ShowUploadExtendedOptions = false;
            m_AssetContext = ContextType.Standard;
		}
	}
}