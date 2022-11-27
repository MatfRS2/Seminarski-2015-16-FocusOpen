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
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components
{
	public static class NotifyUser
	{
		#region User Events

		public static void UserCreatedByAdmin(User user)
		{
			SendConfirmEmailMessage(user, "User.CreatedByAdmin");
		}

		public static void SendEmailConfirmation(User user)
		{
			SendConfirmEmailMessage(user, "User.ConfirmEmailAddress");
		}

		private static void SendConfirmEmailMessage(User user, string template)
		{
			string hash, guid;
			UserManager.ChangeUserHash(user.UserId.GetValueOrDefault(), out hash, out guid);

			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/CUA/U{0}-E{1}-G{2}/", user.UserId, hash, guid));

			Email email = NotifyEngine.GetEmailTemplate(template);
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("Please confirm your email address");
			
			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("email", user.Email);
			email.AddBodyParameter("password", user.UnencryptedPassword);
			email.AddBodyParameter("url", url);

			NotifyEngine.SendMessage(email);
		}

		public static void SendReactivateAccountMessage(User user)
		{
			string hash, guid;
			UserManager.ChangeUserHash(user.UserId.GetValueOrDefault(), out hash, out guid);

			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/RUA/U{0}-E{1}-G{2}/", user.UserId, hash, guid));

			Email email = NotifyEngine.GetEmailTemplate("User.ReactivateAccount");
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("Please reactivate your account");

			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("url", url);

			NotifyEngine.SendMessage(email);
		}

		public static void SendNewPassword(User user)
		{
			Email email = NotifyEngine.GetEmailTemplate("User.PasswordReset");
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("Your password has been reset");

			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("email", user.Email);
			email.AddBodyParameter("password", user.UnencryptedPassword);

			NotifyEngine.SendMessage(email);
		}

		public static void NonEmployeeReactivated(User user)
		{
			Email email = NotifyEngine.GetEmailTemplate("User.NonEmployeeReactivated");
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("Your account has been reactivated");

			email.AddBodyParameter("user-id", user.UserId.GetValueOrDefault());
			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("last-name", user.LastName);

			NotifyEngine.SendMessage(email);
		}

		public static void NonEmployeeRejected(User user)
		{
			Email email = NotifyEngine.GetEmailTemplate("User.NonEmployeeRejected");
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("Account not approved");

			email.AddBodyParameter("user-id", user.UserId.GetValueOrDefault());
			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("last-name", user.LastName);
			email.AddBodyParameter("notes", user.Notes);

			NotifyEngine.SendMessage(email);
		}

		#endregion

		#region Lightbox Events

		public static void LightboxSentToUser(LightboxSent lightboxSent)
		{
			Email email;

			if (lightboxSent.RecipientUser.IsNull)
			{
				email = NotifyEngine.GetEmailTemplate("User.LightboxSentToUnregisteredUser");
				email.Recipients.Add(lightboxSent.RecipientEmail);

				string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/VCS/LSID{0}-SUID{1}-DST{2}/", lightboxSent.LightboxSentId, lightboxSent.SenderId, lightboxSent.DateSent.Ticks));
				email.AddBodyParameter("url", url);
			}
			else
			{
				email = NotifyEngine.GetEmailTemplate("User.LightboxSentToRegisteredUser");
				email.Recipients.Add(lightboxSent.RecipientUser.Email);

				string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/VLB/LID{0}/",(lightboxSent.LightboxLinkedId.GetValueOrDefault() > 0?lightboxSent.LightboxId:lightboxSent.CreatedLightboxId)));
				email.AddBodyParameter("url", url);
				email.AddBodyParameter("recipient-name", lightboxSent.RecipientUser.FullName);
			}

			email.AddBodyParameter("sender-name", lightboxSent.Sender.FullName);
			email.AddBodyParameter("asset-count", lightboxSent.Lightbox.GetAssetList().Count);
			email.AddBodyParameter("lightbox-name", lightboxSent.Lightbox.Name);
			email.AddBodyParameter("message", lightboxSent.Message);

			email.Subject = (StringUtils.IsBlank(lightboxSent.Subject)) ? string.Format("You have received a lightbox (Ref: {0}) : {1}", lightboxSent.LightboxId, lightboxSent.Lightbox.Name) : lightboxSent.Subject;
			
			NotifyEngine.SendMessage(email);
		}

		#endregion

		#region Order Events

		public static void OrderCompleted(Order order)
		{
			Email email = NotifyEngine.GetEmailTemplate("User.OrderCompleted");
			email.Recipients.Add(order.User.Email);
			email.Subject = NotifyEngine.GetSubject("Your order has been completed");

			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/VWO/O{0}/", order.OrderId));
			email.AddBodyParameter("url", url);

			email.AddBodyParameter("first-name", order.User.FirstName);
			email.AddBodyParameter("order-id", order.OrderId);

			NotifyEngine.SendMessage(email);
		}

		public static void OrderItemsProcessed(Order order, IList<OrderItem> processedOrderItems)
		{
			Email email = NotifyEngine.GetEmailTemplate("User.OrderItemsProcessed");
			email.Recipients.Add(order.User.Email);
			email.Subject = NotifyEngine.GetSubject("Order items have been processed");

			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/VWO/O{0}/", order.OrderId));
			email.AddBodyParameter("url", url);

			email.AddBodyParameter("first-name", order.User.FirstName);
			email.AddBodyParameter("order-id", order.OrderId);

			NotifyEngine.SendMessage(email);
		}

		public static void NewOrderItemComment(Order order, OrderItemComment comment)
		{
			Email email = NotifyEngine.GetEmailTemplate("User.NewOrderItemComment");
			email.Recipients.Add(order.User.Email);

			switch (comment.OrderItem.OrderItemStatus)
			{
				case OrderItemStatus.Approved:
					email.Subject = NotifyEngine.GetSubject(string.Format("The request for asset with reference '{0}' has been approved", comment.OrderItem.AssetId));
					break;

				case OrderItemStatus.AwaitingApproval:
					email.Subject = NotifyEngine.GetSubject(string.Format("Further information is required about your request for asset with reference: {0}", comment.OrderItem.AssetId));
					break;

				case OrderItemStatus.Rejected:
					email.Subject = NotifyEngine.GetSubject(string.Format("The request for asset with reference '{0}' has been denied", comment.OrderItem.AssetId));
					break;

				case OrderItemStatus.Preapproved:
					throw new SystemException("Comments cannot be left on order items that are pre-approved");
			}

			string url = SiteUtils.GetWebsiteUrl(string.Format("~/Go.ashx/VOI/O{0}-OI{1}/", order.OrderId, comment.OrderItemId));
			email.AddBodyParameter("url", url);

			email.AddBodyParameter("first-name", order.User.FirstName);
			email.AddBodyParameter("order-id", order.OrderId);
			email.AddBodyParameter("asset-id", comment.OrderItem.Asset.AssetId);
			email.AddBodyParameter("comment-text", comment.CommentText);
			email.AddBodyParameter("comment-date", comment.CommentDate.ToString(Global.DateTimeFormat));
			email.AddBodyParameter("comment-user-name", comment.User.FullName);
			email.AddBodyParameter("current-status", comment.OrderItem.OrderItemStatus.ToString().ToLower());

			NotifyEngine.SendMessage(email);
		}

		#endregion

		public static void SendAssetFeedback(Asset asset, User user, string feedback)
		{
			Email email = NotifyEngine.GetEmailTemplate("User.AssetFeedback");
			email.Recipients.Add(user.Email);
			email.Subject = NotifyEngine.GetSubject("Thank you for your feedback");

			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("asset-id", asset.AssetId);
			email.AddBodyParameter("asset-title", asset.Title);
			email.AddBodyParameter("feedback", feedback);

			NotifyEngine.SendMessage(email);
		}
	}
}