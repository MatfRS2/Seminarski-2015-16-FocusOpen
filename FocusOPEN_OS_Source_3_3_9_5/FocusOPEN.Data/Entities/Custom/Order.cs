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

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Order.
	/// </summary>
	public partial class Order
	{
		#region Lazy Loads

		private List<OrderItem> m_OrderItemList = null;
		private User m_User;

		public virtual User User
		{
			get
			{
				if (m_User == null)
					m_User = User.Get(UserId);

				return (m_User);
			}
		}

		public virtual List<OrderItem> OrderItemList
		{
			get
			{
				if (m_OrderItemList == null)
				{
					OrderItemFinder finder = new OrderItemFinder {OrderId = OrderId.GetValueOrDefault(-1)};
					m_OrderItemList = OrderItem.FindMany(finder);
				}
				return (m_OrderItemList);
			}
		}

		#endregion
	}
}