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
	/// This object represents the properties and methods of a OrderItemComment.
	/// </summary>
	public partial class OrderItemComment
	{
		#region Lazy Loads

		private User m_User = null;
		private OrderItem m_OrderItem;

		public User User
		{
			get
			{
				if (m_User == null || m_User.UserId.GetValueOrDefault() != UserId)
					m_User = User.Get(UserId);

				return m_User;
			}
		}

		public OrderItem OrderItem
		{
			get
			{
				if (m_OrderItem == null || m_OrderItem.OrderItemId.GetValueOrDefault() != OrderItemId)
					m_OrderItem = OrderItem.Get(OrderItemId);

				return m_OrderItem;
			}
		}

		#endregion
	}
}
