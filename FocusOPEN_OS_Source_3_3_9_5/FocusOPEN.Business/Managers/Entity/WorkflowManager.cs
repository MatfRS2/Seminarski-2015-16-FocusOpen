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
using System.Linq;
using System.Reflection;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	public static class WorkflowManager
	{
		#region Private variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Events

		/// <summary>
		/// Fired when a asset is submitted to a new workflow
		/// </summary>
		public static event AssetEventHandler AssetSubmittedToWorkflow;

		/// <summary>
		/// Fired when an asset workflow is completed successfully
		/// This event is not fired when a workflow is completed prematurely due to rejection
		/// </summary>
		public static event AssetWorkflowEventHandler AssetWorkflowComplete;

		/// <summary>
		/// Fired when an asset workflow user rejects the asset
		/// </summary>
		public static event AssetWorkflowRejectedEventHandler AssetWorkflowRejected;

		/// <summary>
		/// Fired when an asset workflow user is selected and needs to review an asset
		/// </summary>
		public static event AssetWorkflowUserHandler AssetWorkflowUserSelected;

		/// <summary>
		/// Fired when a user is invited to comment on a workflow
		/// </summary>
		public static event AssetWorkflowCommenterHandler AssetWorkflowCommenterInvited;
		
		/// <summary>
		/// Fired when someone invited to comment makes a comment
		/// </summary>
		public static event AssetWorkflowCommenterHandler AssetWorkflowCommenterUpdated;

		/// <summary>
		/// Fired when an asset workflow is cancelled
		/// </summary>
		public static event AssetWorkflowEventHandler AssetWorkflowCancelled;

		#endregion

		#region Invite Comments Methods

		public static void InviteUserToWorkflow(AssetWorkflowUser assetWorkflowUser, string inviteeEmail, string message)
		{
			// Ensure message is specified
			if (StringUtils.IsBlank(message))
				throw new ValidationException("Message is required (this will be sent to the user being invited to comment)");

			// Ensure email address is specified
			if (StringUtils.IsBlank(inviteeEmail))
				throw new ValidationException("Email address is required");

			// Get the user by email address
			User invitedUser = User.GetByEmail(inviteeEmail);

			// Ensure email address belongs to a registered user
			if (invitedUser.IsNull)
				throw new ValidationException("Email does not belong to a registered user");

			// Ensure invited user has upload privileges
			if (invitedUser.UserRoleId < Convert.ToInt32(UserRole.UploadUser))
				throw new ValidationException("Email does not belong to a user with upload privileges");

			// Create the commenter record
			AssetWorkflowCommenter awfc = AssetWorkflowCommenter.New();
			awfc.AssetWorkflowId = assetWorkflowUser.AssetWorkflowId;
			awfc.InvitingAssetWorkflowUserId = assetWorkflowUser.AssetWorkflowUserId.GetValueOrDefault();
			awfc.InvitingUserId = assetWorkflowUser.UserId;
			awfc.InvitingUserMessage = message;
			awfc.UserId = invitedUser.UserId.GetValueOrDefault();
			awfc.CreateDate = DateTime.Now;
			awfc.LastUpdate = DateTime.Now;
			AssetWorkflowCommenter.Update(awfc);

			// Refresh from DB
			// awfc = AssetWorkflowCommenter.Get(awfc.AssetWorkflowCommenterId);

			// Now we need to notify them
			if (AssetWorkflowCommenterInvited != null)
				AssetWorkflowCommenterInvited(null, new AssetWorkflowCommenterEventArgs(awfc));
		}

		public static void SaveAssetWorkflowCommenter(AssetWorkflowCommenter awfc)
		{
			if (awfc.AssetWorkflow.IsComplete)
				throw new ValidationException("Asset workflow is complete and comments have been disabled");

			if (StringUtils.IsBlank(awfc.Comments))
				throw new ValidationException("Comments are required");

			// Save the commenter
			AssetWorkflowCommenter.Update(awfc);

			// Now notify the user who invited this commenter
			if (AssetWorkflowCommenterUpdated != null)
				AssetWorkflowCommenterUpdated(null, new AssetWorkflowCommenterEventArgs(awfc));
		}

		#endregion

		#region Save and advance workflow methods

		/// <summary>
		/// Submits an asset to a workflow, and starts the approval process
		/// </summary>
		public static void SubmitAssetToWorkflow(Asset asset, User submittedByUser)
		{
			m_Logger.DebugFormat("Asset: {0} submitted to workflow", asset.AssetId);

			// First ensure we have a workflow
			if (asset.WorkflowId.GetValueOrDefault() == 0)
				throw new SystemException("Asset submitted to workflow, but workflow is not specified");

			// First get all of the users in the workflow
			WorkflowUserFinder finder = new WorkflowUserFinder {WorkflowId = asset.WorkflowId.GetValueOrDefault()};
			finder.SortExpressions.Add(new AscendingSort(WorkflowUser.Columns.Position));
			List<WorkflowUser> workflowUserList = WorkflowUser.FindMany(finder);

			// Ensure workflow has users
			if (workflowUserList.Count == 0)
				throw new SystemException("Asset submitted to workflow, but workflow does not have any users");

			// Create a new asset workflow
			AssetWorkflow aw = AssetWorkflow.New();
			aw.AssetId = asset.AssetId.GetValueOrDefault();
			aw.IsComplete = false;
			aw.SubmittedByUserId = submittedByUser.UserId.GetValueOrDefault();
			aw.CreateDate = DateTime.Now;
			AssetWorkflow.Update(aw);

			// Now add all the users to it
			for (int position = 0; position < workflowUserList.Count; position++)
			{
				// Get the current workflow user
				WorkflowUser wfu = workflowUserList[position];

				// Ensure it's not a duplicate
				if (aw.AssetWorkflowUserList.Any(awfu => awfu.UserId == wfu.UserId))
					continue;

				// Set-up the asset workflow user
				AssetWorkflowUser awu = AssetWorkflowUser.New();
				awu.AssetWorkflowId = aw.AssetWorkflowId.GetValueOrDefault();
				awu.UserId = wfu.UserId;
				awu.Position = position+1;
				awu.AssetWorkflowUserStatus = AssetWorkflowUserStatus.Waiting;
				awu.CreateDate = DateTime.Now;
				awu.LastUpdate = DateTime.Now;

				// Save the workflow user to the DB
				AssetWorkflowUser.Update(awu);

				// Add the user to the workflow
				aw.AssetWorkflowUserList.Add(awu);
			}

			if (AssetSubmittedToWorkflow != null)
				AssetSubmittedToWorkflow(null, new AssetEventArgs(asset));

			// Now notify the next user.  First get them
			AssetWorkflowUser nextUserInWorkflow = aw.AssetWorkflowUserList[0];

			// The send notification
			NotifyUser(nextUserInWorkflow);
		}

		public static void SaveWorkflowUserAndAdvance(AssetWorkflowUser awu)
		{
			// If rejecting, comments are mandatory
			if (awu.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Rejected && StringUtils.IsBlank(awu.Comments))
				throw new AssetWorkflowUserException("Comments are required if rejecting an asset", awu);

			// Update modification date
			awu.LastUpdate = DateTime.Now;

			// Now save it
			AssetWorkflowUser.Update(awu);

			// If workflow item is still pending, our work is done here
			if (awu.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Pending)
				return;

			// If workflow item was rejected, don't continue
			if (awu.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Rejected)
			{
				RejectAndCompleteWorkflow(awu.AssetWorkflow, awu);
				return;
			}

			// Get a list of all users in the same asset workflow
			AssetWorkflowUserFinder finder = new AssetWorkflowUserFinder {AssetWorkflowId = awu.AssetWorkflowId};
			finder.SortExpressions.Add(new AscendingSort(AssetWorkflowUser.Columns.Position));
			List<AssetWorkflowUser> assetWorkflowUserList = AssetWorkflowUser.FindMany(finder);

			// Now we need to figure out who's next, or whether we're all done, so get list of all users after the current one who are still waiting
			List<AssetWorkflowUser> remainingUsers = assetWorkflowUserList.FindAll(o => (o.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Waiting) && (o.Position > awu.Position));

			if (remainingUsers.Count == 0)
			{
				// No users left, so we can complete this workflow
				CompleteWorkflow(awu.AssetWorkflow);
				return;
			}

			// Otherwise, get the next user in the workflow
			AssetWorkflowUser nextUserInWorkflow = remainingUsers[0];

			// and notify them
			NotifyUser(nextUserInWorkflow);
		}

		public static void CancelWorkflow(AssetWorkflow assetWorkflow)
		{
			assetWorkflow.IsComplete = true;
			AssetWorkflow.Update(assetWorkflow);

			m_Logger.DebugFormat("Cancelled AssetWorkflow - AssetWorkflowId: {0}", assetWorkflow.AssetWorkflowId);

			if (AssetWorkflowCancelled != null)
				AssetWorkflowCancelled(null, new AssetWorkflowEventArgs(assetWorkflow));
		}

		#endregion

		#region Private Helper Methods

		private static void NotifyUser(AssetWorkflowUser assetWorkflowUser)
		{
			// First set this user to pending
			assetWorkflowUser.AssetWorkflowUserStatus = AssetWorkflowUserStatus.Pending;

			// Save them
			AssetWorkflowUser.Update(assetWorkflowUser);

			// Now trigger event to send nofication
			if (AssetWorkflowUserSelected != null)
				AssetWorkflowUserSelected(null, new AssetWorkflowUserEventArgs(assetWorkflowUser));
		}

		private static void RejectAndCompleteWorkflow(AssetWorkflow assetWorkflow, AssetWorkflowUser rejectingUser)
		{
			assetWorkflow.IsComplete = true;
			AssetWorkflow.Update(assetWorkflow);

			assetWorkflow.Asset.AssetPublishStatus = AssetPublishStatus.NotApproved;
			Asset.Update(assetWorkflow.Asset);

			if (AssetWorkflowRejected != null)
				AssetWorkflowRejected(null, new AssetWorkflowRejectedEventArgs(assetWorkflow, rejectingUser));
		}

		private static void CompleteWorkflow(AssetWorkflow assetWorkflow)
		{
			assetWorkflow.IsComplete = true;
			AssetWorkflow.Update(assetWorkflow);

			assetWorkflow.Asset.AssetPublishStatus = AssetPublishStatus.Approved;
			Asset.Update(assetWorkflow.Asset);

			if (AssetWorkflowComplete != null)
				AssetWorkflowComplete(null, new AssetWorkflowEventArgs(assetWorkflow));
		}

		#endregion

		/// <summary>
		/// Gets workflow info
		/// </summary>
		/// <param name="asset">The asset for which workflow info is required</param>
		/// <returns></returns>
		public static WorkflowInfo GetWorkflowInfo(Asset asset)
		{
			WorkflowInfo workflowInfo = new WorkflowInfo();

			if (asset.AssetWorkflowList.Count > 0)
			{
				// Get the most recent workflow
				AssetWorkflow aw = asset.AssetWorkflowList[0];

				// Get the total number of users in the workflow
				workflowInfo.TotalWorkflowUserCount = aw.AssetWorkflowUserList.Count;

				// Add all of the users in the workflow
				foreach (AssetWorkflowUser awfu in aw.AssetWorkflowUserList)
				{
					WorkflowInfoItem wi = new WorkflowInfoItem
					                      	{
					                      		WorkflowInfo = workflowInfo,
					                      		UserName = awfu.User.FullName,
					                      		UserEmail = awfu.User.Email,
					                      		Position = awfu.Position,
					                      		Comments = awfu.Comments,
					                      		AssetWorkflowUserStatus = awfu.AssetWorkflowUserStatus,
					                      		IsComment = false,
					                      		Date = awfu.LastUpdate,
					                      		AssetWorkflowUser = awfu
					                      	};

					workflowInfo.WorkflowInfoItemList.Add(wi);
				}

				// Get the total number of commenters in this workflow
				workflowInfo.TotalCommenterCount = aw.AssetWorkflowCommenterList.Count;

				// Add all of the commenters in the workflow
				foreach (AssetWorkflowCommenter awfc in aw.AssetWorkflowCommenterList)
				{
					WorkflowInfoItem wi = new WorkflowInfoItem
					                      	{
					                      		WorkflowInfo = workflowInfo,
					                      		UserName = awfc.User.FullName,
					                      		UserEmail = awfc.User.Email,
					                      		Comments = awfc.Comments,
					                      		AssetWorkflowUserStatus = AssetWorkflowUserStatus.Approved,
					                      		IsComment = true,
					                      		AssetWorkflowCommenter = awfc
					                      	};

					wi.Date = StringUtils.IsBlank(wi.Comments) ? awfc.CreateDate : awfc.LastUpdate;

					workflowInfo.WorkflowInfoItemList.Add(wi);
				}

				// Now that we've got the data, we need to sort it by date
				workflowInfo.WorkflowInfoItemList.Sort(delegate(WorkflowInfoItem left, WorkflowInfoItem right)
				{
					// First get the dates from each object
					DateTime leftDate = left.Date;
					DateTime rightDate = right.Date;

					// For items that are pending or waiting, we want to ignore the date as the item hasn't been actioned yet, and
					// therefore needs to be pushed to the bottom of the list, so we set the date to max.

					if (left.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Pending || left.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Waiting)
						leftDate = DateTime.MaxValue;

					if (right.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Pending || right.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Waiting)
						rightDate = DateTime.MaxValue;

					// Compare using position if dates are same.  Otherwise, use dates.
					if (leftDate == rightDate)
						return left.Position.CompareTo(right.Position);
					
					return leftDate.CompareTo(rightDate);
					
				});
			}

			return workflowInfo;
		}

		#region Workflow Methods

		public static void SaveWorkflow(Workflow workflow)
		{
			ErrorList errors = new ErrorList();

			if (StringUtils.IsBlank(workflow.Name))
				errors.Add("Name is required");

			if (workflow.WorkflowUserList.Count == 0)
				errors.Add("Workflow must contain at least one user");

			if (errors.Count > 0)
				throw new ValidationException(errors);

			Workflow.Update(workflow);
		}

		#endregion

	}

	#region Helper Classes

	public class WorkflowInfo
	{
		public readonly List<WorkflowInfoItem> WorkflowInfoItemList = new List<WorkflowInfoItem>();
		public int TotalCommenterCount;
		public int TotalWorkflowUserCount;
	}

	public class WorkflowInfoItem
	{
		public WorkflowInfo WorkflowInfo;
		public AssetWorkflowUser AssetWorkflowUser = AssetWorkflowUser.Empty;
		public AssetWorkflowCommenter AssetWorkflowCommenter = AssetWorkflowCommenter.Empty;

		public string UserName;
		public string UserEmail;
		public string Comments;
		public AssetWorkflowUserStatus AssetWorkflowUserStatus;
		public bool IsComment;
		public DateTime Date;
		public int Position;
	}

	#endregion
}