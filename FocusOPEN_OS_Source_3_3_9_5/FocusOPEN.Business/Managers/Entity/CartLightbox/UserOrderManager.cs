/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Data;
using Daydream.Data;

namespace FocusOPEN.Business
{
	public class UserOrderManager : BaseCLOManager
	{
		private EntityList<Order> m_OrderList;

		public UserOrderManager(User user) : base(user)
		{
		}

		public EntityList<Order> Orders
		{
			get
			{
				if (m_OrderList == null)
				{
					OrderFinder finder = new OrderFinder();
					finder.UserId = User.UserId.GetValueOrDefault(-1);
					finder.SortExpressions.Add(new DescendingSort(Order.Columns.OrderId));
					m_OrderList = Order.FindMany(finder);
				}
				return m_OrderList;
			}
		}

		public EntityList<Order> GetOrders(int max)
		{
			EntityList<Order> returnList = new EntityList<Order>();

			int i = 0;

			foreach (Order order in Orders)
			{
				returnList.Add(order);

				if (++i >= max)
					break;
			}

			return returnList;
		}

		public Order GetOrderById(int orderId)
		{
			foreach (Order order in Orders)
			{
				if (order.OrderId.Equals(orderId))
					return order;
			}

			return Order.Empty;
		}
	}
}