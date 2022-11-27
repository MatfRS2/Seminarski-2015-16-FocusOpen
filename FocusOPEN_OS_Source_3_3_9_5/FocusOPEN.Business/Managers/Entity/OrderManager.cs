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
using System.Diagnostics;
using System.Linq;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class OrderManager
	{
		#region Events

		public static event OrderEventHandler OrderCreated;
		public static event OrderEventHandler OrderCompleted;
		public static event OrderItemsEventHandler OrderItemsProcessed;
		public static event OrderItemCommentEventHandler NewAdminOrderItemComment;
		public static event OrderItemCommentEventHandler NewUserOrderItemComment;

		#endregion

		/// <summary>
		/// Creates an order from the user's cart.
		/// </summary>
		public static Order CreateOrderFromCart(User user)
		{
			// Create new cart manager for easy access to cart
			CartManager cm = new CartManager(user);

			// Ensure that the cart contains orderable items
			ValidateCart(cm.CartList, user);

			// Create the order 
			Order order = Order.New();
			order.UserId = user.UserId.GetValueOrDefault();
			order.OrderDate = DateTime.Now;

			// Ensure there's no order items
			Debug.Assert(order.OrderItemList.Count == 0);

			// Now add the cart items to it
			foreach (Cart cartItem in cm.CartList)
			{
				Asset asset = cartItem.Asset;

				// Get the asset status for the cart item
				AssetStatus assetStatus = AssetManager.GetAssetStatusForUser(asset, user);

				// Only add assets that are actually available
				// (Ignore withdrawn and expired assets)
				if (assetStatus == AssetStatus.Available)
				{
					// Set the order item status based on whether the item is restricted or not
					bool isRestricted = EntitySecurityManager.IsAssetRestricted(user, asset);

					// Create new order item
					OrderItem orderItem = OrderItem.New();

					// Order items that are for restricted assets are set to awaiting approval
					// and need to be approved by the user who uploaded them by default
					// (though this can change, and be reassigned to the BU admin or superadmin,
					// as per the workflow and scheduled scripts)
					if (isRestricted)
					{
						orderItem.OrderItemStatus = OrderItemStatus.AwaitingApproval;
						orderItem.AssignedToUserId = asset.UploadedByUser.GetOrderItemApproverUserId();
					}
					else
					{
						orderItem.OrderItemStatus = OrderItemStatus.Preapproved;
						orderItem.AssignedToUserId = null;
					}
					
					orderItem.AssetId = cartItem.AssetId;
					orderItem.Notes = cartItem.Notes;
					orderItem.RequiredByDate = cartItem.RequiredByDate;
					orderItem.CreateDate = DateTime.Now;
					order.OrderItemList.Add(orderItem);
				}
			}

			// Save the order
			SaveOrder(order);

			// Log which assets were ordered
			foreach (OrderItem orderItem in order.OrderItemList)
				AuditLogManager.LogAssetAction(orderItem.AssetId, user, AuditAssetAction.Ordered, string.Format("OrderId: {0}", orderItem.OrderId));

			// Log order against user
			AuditLogManager.LogUserAction(user, AuditUserAction.OrderAssets, string.Format("Placed order, with OrderId: {0} containing {1} assets", order.OrderId, order.OrderItemList.Count));

			// Remove the items from the cart
			cm.EmptyCart(false);

			// Complete the order if all items have been processed
			if (AllOrderItemsProcessed(order))
			{
				// Nothing in the order required approval, so automatically complete it
				CompleteOrder(order);
			}
			else
			{
				// Fire event
				if (OrderCreated != null)
					OrderCreated(null, new OrderEventArgs(order));
			}

			return (order);
		}

		public static void ProcessOrderItems(EntityList<OrderItem> orderItems, User user)
		{
			if (orderItems.Count == 0)
				throw new SystemException("No order items");

			// Ensure that we have valid items
			ValidateOrderItemsForProcessing(orderItems);

			// Get the parent of the first order (all order items should be from the same order)
			Order order = orderItems[0].Order;

			// Create list to hold processed items
			EntityList<OrderItem> processedOrderItems = new EntityList<OrderItem>();

			// Flag to tell us whether we need to fire the 'order completion' event
			bool notifyIfComplete = false;

			// Iterate through all of the processed order items and save changes to database
			foreach (OrderItem orderItem in orderItems)
			{
				if (orderItem.IsDirty)
					notifyIfComplete = true;

				// Update database
				OrderItem.Update(orderItem);

				// Save comments
				foreach (OrderItemComment orderItemComment in orderItem.NewOrderItemCommentList)
				{
					// Save the comment
					OrderItemComment.Update(orderItemComment);

					// Fire event so user gets notified that a new comment has been made
					if (NewAdminOrderItemComment != null)
						NewAdminOrderItemComment(null, new OrderItemCommentEventArgs(order, orderItemComment));
				}

				// All new comments saved
				orderItem.NewOrderItemCommentList.Clear();

				switch (orderItem.OrderItemStatus)
				{
					case (OrderItemStatus.Approved):

						// Order items which are approved are processed so add these to the processedOrderItems list
						processedOrderItems.Add(orderItem);

						// Log that a request for this asset was approved in the asset audit history
						AuditLogManager.LogAssetAction(orderItem.AssetId, user, AuditAssetAction.ApprovedForDownload, string.Format("Approved for download for OrderId: {0} made by {1}", orderItem.OrderId, order.User.FullName));

						// Log in user audit
						AuditLogManager.LogUserAction(user, AuditUserAction.AuthoriseOrder, string.Format("Approved request for asset with AssetId: {0} for download in OrderId: {1} made by {2} (UserId: {3})", orderItem.AssetId, orderItem.OrderId, order.User.FullName, order.User.UserId));
						
						break;

					case (OrderItemStatus.Rejected):

						// Order items which are rejected are processed so add these to the processedOrderItems list
						processedOrderItems.Add(orderItem);

						// Log that a request for this asset was rejected in the asset audit history
						AuditLogManager.LogAssetAction(orderItem.AssetId, user, AuditAssetAction.RejectedForDownload, string.Format("Rejected for download for OrderId: {0} made by {1}.  Reason: {2}", orderItem.OrderId, order.User.FullName, orderItem.LastComment));

						// Log in user audit
						AuditLogManager.LogUserAction(user, AuditUserAction.AuthoriseOrder, string.Format("Rejected request for asset with AssetId: {0} for download in OrderId: {1} made by {2} (UserId: {3})", orderItem.AssetId, orderItem.OrderId, order.User.FullName, order.User.UserId));
						
						break;
				}
			}

			// Complete the order if everything has been processed
			if (AllOrderItemsProcessed(order))
			{
				CompleteOrder(order);
			}
			else
			{
				// Otherwise, fire the order items processed event to trigger
				// any notifications to this user that order items have been processed.
				// We only want to fire this if order items have actually been changed though
				// so we look through the order items and check that at least one is dirty

				if (notifyIfComplete)
					if (OrderItemsProcessed != null)
						OrderItemsProcessed(null, new OrderItemEventArgs(order, processedOrderItems));
			}

			// Update user audit with how many order items were processed in which order
			AuditLogManager.LogUserAction(user, AuditUserAction.AuthoriseOrder, string.Format("Processed {0} items in order with OrderId: {1}", processedOrderItems.Count, order.OrderId));
		}

		public static void AddOrderItemComment(Order order, int orderItemId, int userId, string comment)
		{
			OrderItemComment orderItemComment = OrderItemComment.New();
			orderItemComment.OrderItemId = orderItemId;
			orderItemComment.UserId = userId;
			orderItemComment.CommentText = comment;
			orderItemComment.CommentDate = DateTime.Now;
			OrderItemComment.Update(orderItemComment);

			// Fire event
			if (NewUserOrderItemComment != null)
				NewUserOrderItemComment(null, new OrderItemCommentEventArgs(order, orderItemComment));
		}

		#region Private Helper Methods

		private static bool AllOrderItemsProcessed(Order order)
		{
			int processedOrderItems = 0;

			foreach (OrderItem orderItem in order.OrderItemList)
			{
				OrderItemStatus ois = EnumUtils.GetEnumFromValue<OrderItemStatus>(orderItem.OrderItemStatusId);

				switch (ois)
				{
					case (OrderItemStatus.Preapproved):
					case (OrderItemStatus.Approved):
					case (OrderItemStatus.Rejected):
						processedOrderItems++;
						break;

					case (OrderItemStatus.AwaitingApproval):
						// Do nothing
						break;
				}
			}

			// Complete the order if all items have been processed
			return (processedOrderItems == order.OrderItemList.Count);
		}

		private static void CompleteOrder(Order order)
		{
			if (!AllOrderItemsProcessed(order))
				throw new InvalidOrderException("All order items must be approved or rejected before order can be completed");

			order.CompletionDate = DateTime.Now;
			SaveOrder(order);

			// Fire event
			if (OrderCompleted != null)
				OrderCompleted(null, new OrderEventArgs(order));
		}

		#endregion

		/// <summary>
		/// Validates and saves the order
		/// </summary>
		/// <exception cref="InvalidOrderException">If order fails validation rules</exception>
		private static void SaveOrder(Order order)
		{
			ValidateOrder(order);

			// Save the order
			Order.Update(order);

			// Save all of the order items
			foreach (OrderItem oi in order.OrderItemList)
			{
				if (oi.IsNew)
					oi.OrderId = order.OrderId.GetValueOrDefault();

				OrderItem.Update(oi);
			}
		}

		#region Validation

		private static void ValidateOrderItemsForProcessing(IList<OrderItem> orderItems)
		{
			int orderId = orderItems[0].OrderId;

			foreach (OrderItem orderItem in orderItems)
			{
				if (orderItem.OrderItemStatus == OrderItemStatus.Rejected && orderItem.NewOrderItemCommentList.Count == 0)
					throw new InvalidOrderItemException("Rejected order items must have a response");

				if (orderItem.OrderId != orderId)
					throw new InvalidOrderItemException("All order items must belong to the same order");
			}
		}

		/// <summary>
		/// Validates an order
		/// </summary>
		/// <exception cref="InvalidOrderException">Thrown for each error in the order</exception>
		private static void ValidateOrder(Order order)
		{
			if (order.UserId <= 0)
				throw new InvalidOrderException("System Error : Order is missing user id");

			if (order.OrderDate == DateTime.MinValue)
				throw new InvalidOrderException("Sytem Error : Order has invalid order date");

			if (order.OrderItemList.Count == 0)
				throw new InvalidOrderException("Order does not contain any assets");

			foreach (OrderItem orderItem in order.OrderItemList)
			{
				if (orderItem.CreateDate == DateTime.MinValue)
					throw new InvalidOrderException("System Error : Order item has invalid date");

				if (orderItem.AssetId <= 0)
					throw new InvalidOrderException("System Error : Order item is missing asset id");
			}
		}

		/// <summary>
		/// Validates the cart to ensure it is ready for ordering
		/// </summary>
		private static void ValidateCart(IEnumerable<Cart> cartItems, User user)
		{
			int availableCount = cartItems.Count(cartItem => AssetManager.GetAssetStatusForUser(cartItem.Asset, user) == AssetStatus.Available);

			if (availableCount == 0)
				throw new InvalidOrderException("Cart does not contain any available assets");

			if (cartItems.Any(cartItem => cartItem.RequiredByDate.HasValue && cartItem.RequiredByDate.Value < DateTime.Now.Date))
				throw new InvalidOrderException("Required by date cannot be before today.  Please leave blank or choose new date.");

			if ((from cartItem in cartItems
			     let asset = cartItem.Asset
			     let isAvailable = (AssetManager.GetAssetStatusForUser(asset, user) == AssetStatus.Available)
			     let isRestricted = EntitySecurityManager.IsAssetRestricted(user, asset)
			     where isAvailable && isRestricted && StringUtils.IsBlank(cartItem.Notes)
			     select cartItem).Any())
			{
				throw new InvalidOrderException("Assets requiring approval must have usage notes specified.");
			}
		}

		#endregion
	}
}