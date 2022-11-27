/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a AssetWorkflowCommenter.
	/// </summary>
	public partial class AssetWorkflowCommenter
	{
		#region Lazy Loads

		private User m_User;
		private User m_InvitingUser;
		private AssetWorkflow m_AssetWorkflow;

		public User InvitingUser
		{
			get
			{
				if (m_InvitingUser == null || m_InvitingUser.UserId.GetValueOrDefault() != InvitingUserId)
					m_InvitingUser = User.Get(InvitingUserId);

				return m_InvitingUser;
			}
		}

		public User User
		{
			get
			{
				if (m_User == null || m_User.UserId.GetValueOrDefault() != UserId)
					m_User = User.Get(UserId);

				return m_User;
			}
		}

		public AssetWorkflow AssetWorkflow
		{
			get
			{
				if (m_AssetWorkflow == null || m_AssetWorkflow.AssetWorkflowId.GetValueOrDefault() != AssetWorkflowId)
					m_AssetWorkflow = AssetWorkflow.Get(AssetWorkflowId);

				return m_AssetWorkflow;
			}
		}

		#endregion
	}
}