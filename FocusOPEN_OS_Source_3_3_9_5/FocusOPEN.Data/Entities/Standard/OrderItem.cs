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

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a OrderItem.
	/// </summary>
	public partial class OrderItem
	{
		#region Constructor
		
		protected OrderItem()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static OrderItem New ()
		{
			return new OrderItem() ;
		}

		public static OrderItem Empty
		{
			get { return NullOrderItem.Instance; }
		}

		public static OrderItem Get (Nullable <Int32> OrderItemId)
		{
			OrderItem OrderItem = OrderItemMapper.Instance.Get (OrderItemId);
			return OrderItem ?? Empty;
		}

		public static OrderItem Update (OrderItem orderItem)
		{
			return OrderItemMapper.Instance.Update(orderItem) ;
		}

		public static void Delete (Nullable <Int32> OrderItemId)
		{
			OrderItemMapper.Instance.Delete (OrderItemId);
		}

		public static EntityList <OrderItem> FindMany (OrderItemFinder finder, int Page, int PageSize)
		{
			return OrderItemMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <OrderItem> FindMany (OrderItemFinder finder)
		{
			return OrderItemMapper.Instance.FindMany (finder);
		}

		public static OrderItem FindOne (OrderItemFinder finder)
		{
			OrderItem OrderItem = OrderItemMapper.Instance.FindOne(finder);
			return OrderItem ?? Empty;
		}

		public static int GetCount (OrderItemFinder finder)
		{
			return OrderItemMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
