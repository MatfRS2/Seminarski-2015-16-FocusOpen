/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a AssetWorkflow.
	/// </summary>
	public partial class AssetWorkflow
	{
		private Asset m_Asset = null;
		private List<AssetWorkflowUser> m_AssetWorkflowUserList = null;
		private List<AssetWorkflowCommenter> m_AssetWorkflowCommenterList = null;

		#region Lazy Loads

		public Asset Asset
		{
			get
			{
				if (m_Asset == null)
					m_Asset = Asset.Get(AssetId);

				return m_Asset;
			}
		}

		/// <summary>
		/// Gets a list of workflow users in the workflow ordered by position ascending
		/// </summary>
		public List<AssetWorkflowUser> AssetWorkflowUserList
		{
			get
			{
				if (m_AssetWorkflowUserList == null)
				{
					AssetWorkflowUserFinder finder = new AssetWorkflowUserFinder {AssetWorkflowId = AssetWorkflowId.GetValueOrDefault(-1)};
					finder.SortExpressions.Add(new AscendingSort(AssetWorkflowUser.Columns.Position));
					m_AssetWorkflowUserList = AssetWorkflowUser.FindMany(finder);
				}

				return m_AssetWorkflowUserList;
			}
		}

		/// <summary>
		/// Gets a list of workflow commenters order by create date ascending
		/// </summary>
		public List<AssetWorkflowCommenter> AssetWorkflowCommenterList
		{
			get
			{
				if (m_AssetWorkflowCommenterList == null)
				{
					AssetWorkflowCommenterFinder finder = new AssetWorkflowCommenterFinder {AssetWorkflowId = AssetWorkflowId.GetValueOrDefault(-1)};
					finder.SortExpressions.Add(new AscendingSort(AssetWorkflowCommenter.Columns.CreateDate));
					m_AssetWorkflowCommenterList = AssetWorkflowCommenter.FindMany(finder);
				}

				return m_AssetWorkflowCommenterList;
			}
		}

		#endregion
	}
}