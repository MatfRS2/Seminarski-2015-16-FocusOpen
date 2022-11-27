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
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	[Serializable]
	public class BaseSavedAdminSearch<T> where T : AbstractFinder, new()
	{
		#region Private variables

		private T m_Finder;
		private int m_Page;
		private int m_PageSize;
		private string m_SortExpression;
		private bool m_SortAscending;

		#endregion

		#region Accessors

		public T Finder
		{
			get
			{
				if (m_Finder == null)
					m_Finder = new T();

				return m_Finder;
			}
			set
			{
				m_Finder = value;
				HasSearch = true;
			}
		}

		public int Page
		{
			get
			{
				return m_Page;
			}
			set
			{
				m_Page = value;
			}
		}

		public int PageSize
		{
			get
			{
				return m_PageSize;
			}
			set
			{
				m_PageSize = value;
			}
		}

		public string SortExpression
		{
			get
			{
				return m_SortExpression;
			}
			set
			{
				m_SortExpression = value;
			}
		}

		public bool SortAscending
		{
			get
			{
				return m_SortAscending;
			}
			set
			{
				m_SortAscending = value;
			}
		}

		public bool HasSearch { get; private set; }

		#endregion

		#region Constructors

		public BaseSavedAdminSearch()
		{
			Reset();
		}

		#endregion

		/// <summary>
		/// Reset the saved search to its initial state
		/// </summary>
		public void Reset()
		{
			m_Finder = default(T);
			m_Page = 0;
			m_PageSize = 0;
			m_SortExpression = string.Empty;
			m_SortAscending = true;
			HasSearch = false;
		}
	}

	[Serializable]
	public class AdminSavedAssetSearch : BaseSavedAdminSearch<AssetFinder>
	{
		#region Accessors

		public AssetPublicationStatus AssetPublicationStatus { get; set; }

		public bool ShowThumbnails { get; set; }

		#endregion

		#region Constructors

		public AdminSavedAssetSearch()
		{
			AssetPublicationStatus = AssetPublicationStatus.AllAssets;
		}

		#endregion
	}

	[Serializable]
	public class AdminSavedUserSearch : BaseSavedAdminSearch<UserFinder>
	{
	}

	[Serializable]
	public class AdminSavedOrderSearch : BaseSavedAdminSearch<OrderFinder>
	{
	}

	[Serializable]
	public class AdminSavedAuditUserHistorySearch : BaseSavedAdminSearch<AuditUserHistoryFinder>
	{
	}

	[Serializable]
	public class AdminSavedAssetPopularitySearch : BaseSavedAdminSearch<AssetFinder>
	{
	}
}