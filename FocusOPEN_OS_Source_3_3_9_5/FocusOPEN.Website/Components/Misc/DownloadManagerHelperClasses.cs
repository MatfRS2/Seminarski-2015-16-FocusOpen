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
using FocusOPEN.Shared;

namespace FocusOPEN.Website.MyAccount
{
	[Serializable]
	internal class SelectedOrderItem
	{
		public readonly int AssetImageSizeId = 0;
		public readonly DownloadFormat DownloadFormat = DownloadFormat.Original;
		public readonly int OrderItemId = 0;

		public SelectedOrderItem(int orderItemId)
		{
			OrderItemId = orderItemId;
		}

		public SelectedOrderItem(int orderItemId, DownloadFormat downloadFormat, int assetImageSizeId)
			: this(orderItemId)
		{
			DownloadFormat = downloadFormat;
			AssetImageSizeId = assetImageSizeId;
		}
	}

	[Serializable]
	internal class SelectedOrderItemList : List<SelectedOrderItem>
	{
		public SelectedOrderItem Get(int orderItemId)
		{
			foreach (SelectedOrderItem soi in this)
			{
				if (soi.OrderItemId == orderItemId)
					return soi;
			}
			return new SelectedOrderItem(0, DownloadFormat.Original, 0);
		}

		public bool Contains(int orderItemId)
		{
			return this.Any(soi => soi.OrderItemId == orderItemId);
		}

		public void AddUpdate(int orderItemId)
		{
			if (Contains(orderItemId))
				Remove(orderItemId);

			Add(new SelectedOrderItem(orderItemId));
		}

		public void AddUpdate(int orderItemId, DownloadFormat downloadFormat, int assetImageSizeId)
		{
			if (Contains(orderItemId))
				Remove(orderItemId);

			Add(new SelectedOrderItem(orderItemId, downloadFormat, assetImageSizeId));
		}

		public void Remove(int orderItemId)
		{
			RemoveAll(soi => soi.OrderItemId == orderItemId);
		}
	}
}