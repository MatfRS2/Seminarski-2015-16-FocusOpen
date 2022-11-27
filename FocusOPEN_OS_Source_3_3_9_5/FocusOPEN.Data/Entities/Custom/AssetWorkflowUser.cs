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
	/// This object represents the properties and methods of a AssetWorkflowUser.
	/// </summary>
	public partial class AssetWorkflowUser
	{
		#region Lazy Loads

		private AssetWorkflow m_AssetWorkflow = null;
		private User m_User = null;
		private List<AssetWorkflowCommenter> m_InvitedAssetWorkflowCommenterList;

		public AssetWorkflow AssetWorkflow
		{
			get
			{
				if (m_AssetWorkflow == null)
					m_AssetWorkflow = AssetWorkflow.Get(AssetWorkflowId);

				return m_AssetWorkflow;
			}
		}

		public User User
		{
			get
			{
				if (m_User == null)
					m_User = User.Get(UserId);

				return m_User;
			}
		}

		public List<AssetWorkflowCommenter> InvitedAssetWorkflowCommenterList
		{
			get
			{
				if (m_InvitedAssetWorkflowCommenterList == null)
				{
					AssetWorkflowCommenterFinder finder = new AssetWorkflowCommenterFinder {InvitingAssetWorkflowUserId = AssetWorkflowUserId.GetValueOrDefault()};
					finder.SortExpressions.Add(new AscendingSort(AssetWorkflowCommenter.Columns.LastUpdate));
					m_InvitedAssetWorkflowCommenterList = AssetWorkflowCommenter.FindMany(finder);
				}

				return m_InvitedAssetWorkflowCommenterList;
			}
		}

		#endregion

		public AssetWorkflowUserStatus AssetWorkflowUserStatus
		{
			get
			{
				return EnumUtils.GetEnumFromValue<AssetWorkflowUserStatus>(AssetWorkflowUserStatusId);
			}
			set
			{
				AssetWorkflowUserStatusId = Convert.ToInt32(value);
			}
		}
	}
}