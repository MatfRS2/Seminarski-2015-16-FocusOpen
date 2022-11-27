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
using System.Reflection;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components
{
	public static class NotifyEngine
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region Asset Notifications

		public static void AssetDelegated(object sender, AssetDelegatedEventArgs e)
		{
			NotifyAdmin.AssetDelegated(e.Asset, e.User);
		}

		public static void AssetProcessingComplete(Asset asset)
		{
			NotifyAdmin.AssetProcessingComplete(asset, asset.UploadedByUser);
		}

		/// <summary>
		/// Sends email notifications when an asset with a duplicate hash is uploaded (ie. duplicate file)
		/// </summary>
		public static void AssetUploadDuplicateHash(User user, IEnumerable<Asset> assetList)
		{
			m_Logger.DebugFormat("Enter NotifyEngine.AssetUploadDuplicateHash");
			NotifyAdmin.AssetUploadDuplicateHash(user, assetList);
			m_Logger.DebugFormat("Exit NotifyEngine.AssetUploadDuplicateHash");
		}

		/// <summary>
		/// Sends the asset feedback.
		/// </summary>
		/// <param name="asset">The asset for which feedback is being sent.</param>
		/// <param name="user">The user who is sending the feedback.</param>
		/// <param name="feedback">The feedback sent by the user</param>
		public static void SendAssetFeedback(Asset asset, User user, string feedback)
		{
			NotifyAdmin.SendAssetFeedback(asset, user, feedback);
			NotifyUser.SendAssetFeedback(asset, user, feedback);
		}

		#endregion

		#region User Notifications

		public static void UserCreatedByAdmin(object sender, UserEventArgs e)
		{
			NotifyUser.UserCreatedByAdmin(e.User);
		}

		public static void UnapprovedUserCreated(object sender, UserEventArgs e)
		{
			NotifyAdmin.UnapprovedUserCreated(e.User);
		}

		public static void PreApprovedUserCreated(object sender, UserEventArgs e)
		{
			NotifyUser.SendEmailConfirmation(e.User);
		}

		public static void NonEmployeeReactivated(object sender, UserEventArgs e)
		{
			NotifyUser.NonEmployeeReactivated(e.User);
		}

		public static void PasswordResetRequested(object sender, UserEventArgs e)
		{
			NotifyUser.SendNewPassword(e.User);
		}

		public static void NonEmployeeProcessed(object sender, UserEventArgs e)
		{
			User user = e.User;

			if (user.UserStatus == UserStatus.Rejected)
			{
				NotifyUser.NonEmployeeRejected(user);
			}
			else
			{
				NotifyUser.SendEmailConfirmation(user);
			}
		}

		public static void InvalidRegistrationAttempt(RegistrationSecurityException rsuex)
		{
			NotifyAdmin.InvalidRegistrationAttempt(rsuex);
		}

		public static void InvalidLoginAttempt(LoginSecurityException lsecex)
		{
			NotifyAdmin.InvalidLoginAttempt(lsecex);
		}

		public static void SendReactivateAccountMessage(User user)
		{
			NotifyUser.SendReactivateAccountMessage(user);
		}

		#endregion

		#region Lightbox Notifications

		public static void LightboxSentToUser(object sender, LightboxSentEventArgs e)
		{
			NotifyUser.LightboxSentToUser(e.LightboxSent);
		}

		#endregion

		#region Order Notifications

		public static void OrderCreated(object o, OrderEventArgs e)
		{
			Order order = e.Order;

			// List of email addresses to which notifications are being sent, so we
			// don't send anyone the same notification for the same order twice
			// (eg. if a single order contains multiple assets requiring approval by the same user)
			List<string> userEmails = new List<string>();

			// List of users to which email notifications will be sent
			List<User> users = new List<User>();

			// Get all of the users assigned to approving order items
			// so that we can email them to approve requests for assets

			foreach (OrderItem orderItem in order.OrderItemList)
			{
				if (orderItem.OrderItemStatus == OrderItemStatus.AwaitingApproval)
				{
					// Get the user assigned to approving this order item
					User authorisingUser = orderItem.AssignedToUser;

					// Get the email address of the authorising user
					string authorisingUserEmail = authorisingUser.Email.ToLower();

					if (!userEmails.Contains(authorisingUserEmail))
					{
						users.Add(authorisingUser);
						userEmails.Add(authorisingUserEmail);
					}
				}
			}

			foreach (User user in users)
			{
				NotifyAdmin.OrderItemApprovalRequired(user, order);

				// Drop out as debug users dont want to receive emails for every user
				if (Email.HasDebugMode)
					break;
			}
		}

		public static void OrderCompleted(object o, OrderEventArgs e)
		{
			NotifyUser.OrderCompleted(e.Order);
		}

		public static void OrderItemsProcessed(object o, OrderItemEventArgs e)
		{
			NotifyUser.OrderItemsProcessed(e.Order, e.ProcessedOrderItems);
		}

		public static void NewAdminOrderItemComment(object o, OrderItemCommentEventArgs e)
		{
			NotifyUser.NewOrderItemComment(e.Order, e.OrderItemComment);
		}

		public static void NewUserOrderItemComment(object o, OrderItemCommentEventArgs e)
		{
			NotifyAdmin.NewOrderItemComment(e.Order, e.OrderItemComment);
		}

		#endregion

		#region Workflow Notifications

		public static void AssetSubmittedToWorkflow(object sender, AssetEventArgs e)
		{
			// No notifications are sent when an asset is submitted to a workflow

			m_Logger.DebugFormat("AssetSubmittedToWorkflow - AssetId: {0}", e.Asset.AssetId);
		}

		public static void AssetWorkflowUserSelected(object sender, AssetWorkflowUserEventArgs e)
		{
			m_Logger.DebugFormat("AssetWorkflowUserSelected - AssetWorkflowUserId: {0}", e.AssetWorkflowUser.AssetWorkflowUserId);
			NotifyAdmin.AssetWorkflowUserSelected(e.AssetWorkflowUser);
		}

		public static void AssetWorkflowCommenterInvited(object sender, AssetWorkflowCommenterEventArgs e)
		{
			m_Logger.DebugFormat("AssetWorkflowCommenterInvited - AssetWorkflowCommenterId: {0}", e.AssetWorkflowCommenter.AssetWorkflowCommenterId);
			NotifyAdmin.AssetWorkflowCommenterInvited(e.AssetWorkflowCommenter);
		}

		public static void AssetWorkflowCommenterUpdated(object sender, AssetWorkflowCommenterEventArgs e)
		{
			m_Logger.DebugFormat("AssetWorkflowCommenterUpdated - AssetWorkflowCommenterId: {0}", e.AssetWorkflowCommenter.AssetWorkflowCommenterId);
			NotifyAdmin.AssetWorkflowCommenterUpdated(e.AssetWorkflowCommenter);
		}

		public static void AssetWorkflowRejected(object sender, AssetWorkflowRejectedEventArgs e)
		{
			m_Logger.DebugFormat("AssetWorkflowRejected - AssetWorkflowId: {0}", e.AssetWorkflow.AssetWorkflowId);
			NotifyAdmin.AssetWorkflowRejected(e.AssetWorkflow, e.RejectingUser);
		}

		public static void AssetWorkflowComplete(object sender, AssetWorkflowEventArgs e)
		{
			m_Logger.DebugFormat("AssetWorkflowComplete - AssetWorkflowId: {0}", e.AssetWorkflow.AssetWorkflowId);
			NotifyAdmin.AssetWorkflowComplete(e.AssetWorkflow);
		}

		public static void AssetWorkflowCancelled(object sender, AssetWorkflowEventArgs e)
		{
			m_Logger.DebugFormat("AssetWorkflowCancelled - AssetWorkflowId: {0}", e.AssetWorkflow.AssetWorkflowId);
			NotifyAdmin.AssetWorkflowCancelled(e.AssetWorkflow);
		}

		#endregion

		public static void FtpDownloadComplete(object sender, FtpDownloadCompleteEventArgs e)
		{
			m_Logger.DebugFormat("Enter NotifyEngine.FtpDownloadComplete");
			NotifyAdmin.FtpDownloadComplete(e.User, e.ServerMessages);
			m_Logger.DebugFormat("Exit NotifyEngine.FtpDownloadComplete");
		}

		#region Helper Methods

		public static Email GetEmailTemplate(string template)
		{
			return GetEmailTemplate(template, WebsiteBrandManager.GetBrand());
		}

		public static Email GetEmailTemplate(string template, Brand brand)
		{
			// Get the email message
			Email email = Email.CreateFromTemplate(template);

			// Set from name and email
			email.FromName = brand.ApplicationName;
			email.FromEmail = brand.EmailFrom;

			if (!StringUtils.IsEmail(email.FromEmail))
				email.FromEmail = "do-not-reply@digitalassetmanager.com";

			if (StringUtils.IsBlank(email.FromName))
				email.FromName = email.FromEmail;

			// Add common parameters
			email.AddBodyParameter("appPath", brand.WebsiteUrl);
			email.AddBodyParameter("orgName", brand.OrganisationName);
			email.AddBodyParameter("appName", brand.ApplicationName);

			// return it
			return (email);
		}

		/// <summary>
		/// Returns the subject prefixed with application name
		/// </summary>
		public static string GetSubject(string subject)
		{
			Brand brand = WebsiteBrandManager.GetBrand();
			return GetSubject(subject, brand);
		}

		/// <summary>
		/// Returns the subject prefixed with application name
		/// </summary>
		public static string GetSubject(string subject, Brand brand)
		{
			return string.Format("{0}: {1}", brand.ApplicationName, subject);
		}

		/// <summary>
		/// Sends the email message
		/// </summary>
		public static void SendMessage(Email email)
		{
			JoinableList jList = new JoinableList();

			foreach (string recipient in email.Recipients)
				jList.Add(recipient + " (TO)");

			foreach (string recipient in email.CC)
				jList.Add(recipient + " (CC)");

			foreach (string recipient in email.BCC)
				jList.Add(recipient + " (BCC)");

			try
			{
				email.Send();
				m_Logger.DebugFormat("Sent email message with subject '{0}' to '{1}', using template: {2}", email.Subject, jList, email.TemplateFilename);
			}
			catch (Exception ex)
			{
				m_Logger.Error(string.Format("Error sending email message with subject '{0}' to '{1}', using template: {2}", email.Subject, jList, email.TemplateFilename), ex);
				throw;
			}
		}

		#endregion
	}
}