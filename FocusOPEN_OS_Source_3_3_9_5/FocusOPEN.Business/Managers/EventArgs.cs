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

namespace FocusOPEN.Business
{
	#region Delegates

	public delegate void UserEventHandler(object sender, UserEventArgs e);

	public delegate void AssetEventHandler(object sender, AssetEventArgs e);

	public delegate void LightboxSentEventHandler(object sender, LightboxSentEventArgs e);

	public delegate void OrderEventHandler(object o, OrderEventArgs e);

	public delegate void OrderItemsEventHandler(object o, OrderItemEventArgs e);

	public delegate void OrderItemCommentEventHandler(object o, OrderItemCommentEventArgs e);

	public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

	public delegate void AssetWorkflowEventHandler(object sender, AssetWorkflowEventArgs e);

	public delegate void AssetWorkflowRejectedEventHandler(object sender, AssetWorkflowRejectedEventArgs e);

	public delegate void AssetWorkflowUserHandler(object sender, AssetWorkflowUserEventArgs e);

	public delegate void AssetWorkflowCommenterHandler(object sender, AssetWorkflowCommenterEventArgs e);

	public delegate void AssetDelegatedEventHandler(object sender, AssetDelegatedEventArgs e);

	#endregion

	public class UserEventArgs : EventArgs
	{
		public readonly User User;

		public UserEventArgs(User user)
		{
			User = user;
		}
	}

	public class AssetEventArgs : EventArgs
	{
		public readonly Asset Asset;

		public AssetEventArgs(Asset asset)
		{
			Asset = asset;
		}
	}

	public class OrderEventArgs : EventArgs
	{
		public readonly Order Order;

		public OrderEventArgs(Order order)
		{
			Order = order;
		}
	}

	public class OrderItemEventArgs : EventArgs
	{
		public readonly Order Order;
		public readonly EntityList<OrderItem> ProcessedOrderItems;

		public OrderItemEventArgs(Order order, EntityList<OrderItem> processedOrderItems)
		{
			Order = order;
			ProcessedOrderItems = processedOrderItems;
		}
	}

	public class OrderItemCommentEventArgs : OrderEventArgs
	{
		public readonly OrderItemComment OrderItemComment;

		public OrderItemCommentEventArgs(Order order, OrderItemComment orderItemComment) : base(order)
		{
			OrderItemComment = orderItemComment;
		}
	}

	public class LightboxSentEventArgs : EventArgs
	{
		public readonly LightboxSent LightboxSent;

		public LightboxSentEventArgs(LightboxSent lightboxSent)
		{
			LightboxSent = lightboxSent;
		}
	}

	public class ProgressEventArgs : EventArgs
	{
		public readonly string ProgressText = string.Empty;

		public ProgressEventArgs(string progressText)
		{
			ProgressText = progressText;
		}
	}

	public class AssetWorkflowEventArgs : EventArgs
	{
		public readonly AssetWorkflow AssetWorkflow = null;

		public AssetWorkflowEventArgs(AssetWorkflow assetWorkflow)
		{
			AssetWorkflow = assetWorkflow;
		}
	}

	public class AssetWorkflowUserEventArgs : EventArgs
	{
		public readonly AssetWorkflowUser AssetWorkflowUser;

		public AssetWorkflowUserEventArgs(AssetWorkflowUser assetWorkflowUser)
		{
			AssetWorkflowUser = assetWorkflowUser;
		}
	}

	public class AssetWorkflowRejectedEventArgs : EventArgs
	{
		public readonly AssetWorkflow AssetWorkflow;
		public readonly AssetWorkflowUser RejectingUser;

		public AssetWorkflowRejectedEventArgs(AssetWorkflow assetWorkflow, AssetWorkflowUser rejectingUser)
		{
			AssetWorkflow = assetWorkflow;
			RejectingUser = rejectingUser;
		}
	}

	public class AssetWorkflowCommenterEventArgs : EventArgs
	{
		public readonly AssetWorkflowCommenter AssetWorkflowCommenter;

		public AssetWorkflowCommenterEventArgs(AssetWorkflowCommenter assetWorkflowCommenter)
		{
			AssetWorkflowCommenter = assetWorkflowCommenter;
		}
	}

	public class AssetDelegatedEventArgs : EventArgs
	{
		public readonly Asset Asset;
		public readonly User User;

		public AssetDelegatedEventArgs(Asset asset, User user)
		{
			Asset = asset;
			User = user;
		}
	}
}