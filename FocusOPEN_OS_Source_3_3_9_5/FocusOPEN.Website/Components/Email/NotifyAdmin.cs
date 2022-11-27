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
using System.Linq;
using System.Reflection;
using System.Xml;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components
{
	public static class NotifyAdmin
	{
		#region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#endregion

		#region Helper Methods

		/// <summary>
		/// Gets a list of active super adminisrators
		/// </summary>
		private static IEnumerable<User> GetActiveSuperAdminList()
		{
			// Create list to hold active super admins
			EntityList<User> list = new EntityList<User>();

			// Get super-admins
			UserFinder finder = new UserFinder {UserRole = UserRole.SuperAdministrator};
			EntityList<User> superAdminList = User.FindMany(finder);

			// Add active super admins to the list
			list.AddRange(superAdminList.Where(superAdmin => superAdmin.IsActive()));

			// Return the list
			return list;
		}

		/// <summary>
		/// Adds all active super admins to the email's recipient list
		/// </summary>
		private static void AddSuperAdminsToEmailList(ICollection<string> list)
		{
			// Add active super admins to the recipient list
			foreach (User superAdmin in GetActiveSuperAdminList())
			{
				// Add the super admin to the recipient list
				list.Add(superAdmin.Email);
			}
		}

		#endregion

		#region User Notifications

		public static void UnapprovedUserCreated(User user)
		{
			// Get the user responsible for authorising this user registration
			User owner = UserManager.GetAuthorisingUser(user);

			// Get the email and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.UnapprovedUserCreated");
			email.Recipients.Add(owner.Email);
			email.Subject = NotifyEngine.GetSubject("User approval required");

			// Add all super admins to the CC list
			AddSuperAdminsToEmailList(email.CC);

			// Remove the owner, as we dont want to them to get the email twice
			email.CC.Remove(owner.Email);

			// Construct the link to the 'Approve External User' page
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/AEU/U{0}/", user.UserId));
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void InvalidRegistrationAttempt(RegistrationSecurityException rsuex)
		{
			// Get the email and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.InvalidUserRegistrationAttempt");
			email.Subject = NotifyEngine.GetSubject("User registration failed");

			// Add all superadmins to the recipient list
			AddSuperAdminsToEmailList(email.Recipients);

			// Ensure that we have at least one super admin to receive the message
			if (email.Recipients.Count == 0)
			{
				m_Logger.Error("InvalidRegistrationAttempt() - No active super admins exist to receive notification");
				return;
			}

			// Add body parameters
			email.AddBodyParameter("message", rsuex.Message);
			email.AddBodyParameter("registrant-name", rsuex.Entity.FullName);
			email.AddBodyParameter("registrant-email", rsuex.Entity.Email);
			email.AddBodyParameter("registrant-ip-address", rsuex.IpAddress);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void InvalidLoginAttempt(LoginSecurityException lsecex)
		{
			// Get the email and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.InvalidUserLoginAttempt");
			email.Subject = NotifyEngine.GetSubject("User login failed");

			// Add all superadmins to the recipient list
			AddSuperAdminsToEmailList(email.Recipients);

			// Ensure that we have at least one super admin to receive the message
			if (email.Recipients.Count == 0)
			{
				m_Logger.Error("InvalidLoginAttempt() - No active super admins exist to receive notification");
				return;
			}

			// Construct the link to the 'Edit User' page
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/EUA/U{0}/", lsecex.Entity.UserId));

			// Add body parameters
			email.AddBodyParameter("message", lsecex.Message);
			email.AddBodyParameter("user-name", lsecex.Entity.FullName);
			email.AddBodyParameter("user-email", lsecex.Entity.Email);
			email.AddBodyParameter("user-ip-address", lsecex.IpAddress);
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void AccountExpiredLoginAttempt(User user)
		{
			// Get the user responsible for authorising this user registration
			User owner = UserManager.GetAuthorisingUser(user);

			// Get the email and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.AccountExpiredLoginAttempt");
			email.Recipients.Add(owner.Email);
			email.Subject = NotifyEngine.GetSubject("User login failed: account has expired");

			// Add all super admins to the CC list
			AddSuperAdminsToEmailList(email.CC);

			// Remove the owner, as we dont want to them to get the email twice
			email.CC.Remove(owner.Email);

			// Construct the link to the 'Edit User' page
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/EUA/U{0}/", user.UserId));

			// Add body parameters
			email.AddBodyParameter("user-name", user.FullName);
			email.AddBodyParameter("user-email", user.Email);
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		#endregion

		#region Asset Notifications

		public static void SendAssetFeedback(Asset asset, User user, string feedback)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetFeedback");
			
			// Send copy to the user giving the feedback
			email.BCC.Add(user.Email);

			// Send copy to the asset contact
			email.BCC.Add(asset.ContactEmail);

			// Add super admins
			AddSuperAdminsToEmailList(email.BCC);

			// Include asset title and ID in subject
			email.Subject = NotifyEngine.GetSubject(string.Format("Feedback for asset: {0} - Ref: {1}", asset.Title, asset.AssetId));

			// Add body parameters
			email.AddBodyParameter("uploader-first-name", asset.UploadedByUser.FirstName);
			email.AddBodyParameter("feedback-sender-name", user.FullName);
			email.AddBodyParameter("asset-id", asset.AssetId);
			email.AddBodyParameter("asset-title", asset.Title);
			email.AddBodyParameter("feedback", feedback);

			// RAD = Review Asset Details
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/RAD/AID{0}/", asset.AssetId));
			email.AddBodyParameter("url", url);

			NotifyEngine.SendMessage(email);
		}

		public static void AssetDelegated(Asset asset, User user)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetDelegated");
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("An asset has been delegated to you");

			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("asset-id", asset.AssetId);

			// RAD = Review Asset Details
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/RAD/AID{0}/", asset.AssetId));
			email.AddBodyParameter("url", url);

			NotifyEngine.SendMessage(email);
		}

		public static void AssetProcessingComplete(Asset asset, User uploader)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetProcessingComplete");
			email.Recipients.Add(uploader.Email);
			email.Subject = NotifyEngine.GetSubject("Asset has been processed");

			email.AddBodyParameter("first-name", uploader.FirstName);
			email.AddBodyParameter("asset-filename", asset.Filename);
			email.AddBodyParameter("asset-id", asset.AssetId);

			// RAD = Review Asset Details
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/RAD/AID{0}/", asset.AssetId));
			email.AddBodyParameter("url", url);

			NotifyEngine.SendMessage(email);
		}

		#endregion

		#region Order Notifications

		public static void OrderItemApprovalRequired(User user, Order order)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.OrderItemApprovalRequired");
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("Order contains assets requiring your approval");

			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/AUO/O{0}/", order.OrderId));
			email.AddBodyParameter("url", url);

			NotifyEngine.SendMessage(email);
		}

		public static void NewOrderItemComment(Order order, OrderItemComment comment)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.NewOrderItemComment");
			email.Recipients.Add(comment.OrderItem.AssignedToUser.Email);
			email.Subject = NotifyEngine.GetSubject("User response to request for further information about an order item");

			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/AUO/O{0}/", order.OrderId));
			email.AddBodyParameter("url", url);

			email.AddBodyParameter("order-id", order.OrderId);
			email.AddBodyParameter("asset-id", comment.OrderItem.Asset.AssetId);
			email.AddBodyParameter("comment-text", comment.CommentText);
			email.AddBodyParameter("comment-date", comment.CommentDate.ToString(Global.DateTimeFormat));
			email.AddBodyParameter("comment-user-name", comment.User.FullName);

			NotifyEngine.SendMessage(email);
		}

		#endregion

		#region Asset Workflow Notifications

		public static void AssetWorkflowUserSelected(AssetWorkflowUser assetWorkflowUser)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetWorkflowUserSelected");
			email.Recipients.Add(assetWorkflowUser.User.Email);
			email.Subject = NotifyEngine.GetSubject("An asset requires your approval");

			// Add body parameters
			email.AddBodyParameter("asset-id", assetWorkflowUser.AssetWorkflow.Asset.AssetId);
			email.AddBodyParameter("asset-type", assetWorkflowUser.AssetWorkflow.Asset.AssetType.Name);
			email.AddBodyParameter("upload-user-name", assetWorkflowUser.AssetWorkflow.Asset.UploadedByUser.FullName);

			// AAW = Approve Asset Workflow
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/AAW/AWFID{0}/", assetWorkflowUser.AssetWorkflowId));
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void AssetWorkflowCommenterInvited(AssetWorkflowCommenter assetWorkflowCommenter)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetWorkflowCommenterInvited");
			email.Recipients.Add(assetWorkflowCommenter.User.Email);
			email.Subject = NotifyEngine.GetSubject("You have been invited to comment on an asset");

			// Add body parameters
			email.AddBodyParameter("inviter-name", assetWorkflowCommenter.InvitingUser.FullName);
			email.AddBodyParameter("inviter-message", assetWorkflowCommenter.InvitingUserMessage);
			email.AddBodyParameter("asset-id", assetWorkflowCommenter.AssetWorkflow.Asset.AssetId);
			email.AddBodyParameter("asset-type", assetWorkflowCommenter.AssetWorkflow.Asset.AssetType.Name);
			email.AddBodyParameter("upload-user-name", assetWorkflowCommenter.AssetWorkflow.Asset.UploadedByUser.FullName);

			// ACI = Asset Commenter Invited
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/ACI/AWFID{0}/", assetWorkflowCommenter.AssetWorkflowId));
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void AssetWorkflowCommenterUpdated(AssetWorkflowCommenter assetWorkflowCommenter)
		{
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetWorkflowCommenterUpdated");
			email.Recipients.Add(assetWorkflowCommenter.InvitingUser.Email);
			email.Subject = NotifyEngine.GetSubject("An invited user has made a comment on a workflow");

			// Add body parameters

			email.AddBodyParameter("inviter-name", assetWorkflowCommenter.InvitingUser.FullName);
			email.AddBodyParameter("commenter-name", assetWorkflowCommenter.User.FullName);
			email.AddBodyParameter("commenter-comments", assetWorkflowCommenter.Comments);
			email.AddBodyParameter("asset-id", assetWorkflowCommenter.AssetWorkflow.Asset.AssetId);
			email.AddBodyParameter("asset-type", assetWorkflowCommenter.AssetWorkflow.Asset.AssetType.Name);
			email.AddBodyParameter("upload-user-name", assetWorkflowCommenter.AssetWorkflow.Asset.UploadedByUser.FullName);

			// AAW = Approve Asset Workflow
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/AAW/AWFID{0}/", assetWorkflowCommenter.AssetWorkflowId));
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void AssetWorkflowRejected(AssetWorkflow assetWorkflow, AssetWorkflowUser rejectingUser)
		{
			// Get the message template and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetWorkflowRejected");
			email.Subject = NotifyEngine.GetSubject("An asset has been rejected");

			// Add the upload user
			email.Recipients.Add(assetWorkflow.Asset.UploadedByUser.Email);

			// Add all of the users in the workflow
			foreach (AssetWorkflowUser awfu in assetWorkflow.AssetWorkflowUserList)
				email.Recipients.Add(awfu.User.Email);

			// Add message parameters
			email.AddBodyParameter("asset-id", assetWorkflow.Asset.AssetId);
			email.AddBodyParameter("asset-type", assetWorkflow.Asset.AssetType.Name);
			email.AddBodyParameter("upload-user-name", assetWorkflow.Asset.UploadedByUser.FullName);
			email.AddBodyParameter("rejector-name", rejectingUser.User.FullName);
			email.AddBodyParameter("rejector-comments", rejectingUser.Comments);

			// RAD = Review Asset Details
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/RAD/AID{0}/", assetWorkflow.AssetId));
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void AssetWorkflowComplete(AssetWorkflow assetWorkflow)
		{
			// Get the message template and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetWorkflowComplete");
			email.Subject = NotifyEngine.GetSubject("An asset has been approved and is ready to be published");

			// Add the upload user
			email.Recipients.Add(assetWorkflow.Asset.UploadedByUser.Email);

			// Add all of the users in the workflow
			foreach (AssetWorkflowUser awfu in assetWorkflow.AssetWorkflowUserList)
				email.Recipients.Add(awfu.User.Email);

			// Add message parameters
			email.AddBodyParameter("asset-id", assetWorkflow.Asset.AssetId);
			email.AddBodyParameter("asset-type", assetWorkflow.Asset.AssetType.Name);
			email.AddBodyParameter("upload-user-name", assetWorkflow.Asset.UploadedByUser.FullName);

			// RAD = Review Asset Details
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/RAD/AID{0}/", assetWorkflow.AssetId));
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void AssetWorkflowCancelled(AssetWorkflow assetWorkflow)
		{
			// Get the message template and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetWorkflowCancelled");
			email.Subject = NotifyEngine.GetSubject("An asset workflow has been cancelled");

			// Add the upload user
			email.Recipients.Add(assetWorkflow.Asset.UploadedByUser.Email);

			// Add all of the users in the workflow
			foreach (AssetWorkflowUser awfu in assetWorkflow.AssetWorkflowUserList)
				email.Recipients.Add(awfu.User.Email);

			// Add message parameters
			email.AddBodyParameter("asset-id", assetWorkflow.Asset.AssetId);
			email.AddBodyParameter("asset-type", assetWorkflow.Asset.AssetType.Name);
			email.AddBodyParameter("upload-user-name", assetWorkflow.Asset.UploadedByUser.FullName);

			// RAD = Review Asset Details
			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/RAD/AID{0}/", assetWorkflow.AssetId));
			email.AddBodyParameter("url", url);

			// Send it
			NotifyEngine.SendMessage(email);
		}

		#endregion

		public static void FtpDownloadComplete(User user, string messages)
		{
			// Get the message template and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.FtpDownloadComplete", user.PrimaryBrand);
			email.Subject = NotifyEngine.GetSubject("FTP download is complete", user.PrimaryBrand);

			// Add the user as a recipient
			email.Recipients.Add(user.Email);

			// Add message parameters
			email.AddBodyParameter("first-name", user.FirstName);

			// Add the FTP log to the message as an attachment
			email.AddAttachment(messages, "FtpLog.txt");

			// Send it
			NotifyEngine.SendMessage(email);
		}

		public static void AssetUploadDuplicateHash(User user, IEnumerable<Asset> assets)
		{
			// Get the message template and set the subject
			Email email = NotifyEngine.GetEmailTemplate("Admin.AssetUploadDuplicateHash", user.PrimaryBrand);
			email.Subject = NotifyEngine.GetSubject("Duplicate asset upload failed");

			// Add uploader name
			email.AddBodyParameter("uploader-name", user.FullName);

			// Add the superadmins as recipients
			AddSuperAdminsToEmailList(email.Recipients);

			// Add assets
			foreach (Asset asset in assets)
			{
				// Create the Asset node
				XmlNode node = email.BodyXmlDoc.CreateElement("Asset");

				// Ensure we've got a document element
				if (email.BodyXmlDoc.DocumentElement == null)
					return;

				// Add the asset node
				email.BodyXmlDoc.DocumentElement.AppendChild(node);

				// Add the AssetId attribute
				XmlUtils.AddAttribute(email.BodyXmlDoc, node, "AssetId", asset.AssetId);
				XmlUtils.AddAttribute(email.BodyXmlDoc, node, "Filename", asset.Filename);
			}

			// Send it
			NotifyEngine.SendMessage(email);
		}
	}
}