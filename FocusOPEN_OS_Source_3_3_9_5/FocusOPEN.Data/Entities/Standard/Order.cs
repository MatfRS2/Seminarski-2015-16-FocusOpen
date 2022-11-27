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
	/// This object represents the properties and methods of a Order.
	/// </summary>
	public partial class Order
	{
		#region Constructor
		
		protected Order()
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
		
		public static Order New ()
		{
			return new Order() ;
		}

		public static Order Empty
		{
			get { return NullOrder.Instance; }
		}

		public static Order Get (Nullable <Int32> OrderId)
		{
			Order Order = OrderMapper.Instance.Get (OrderId);
			return Order ?? Empty;
		}

		public static Order Update (Order order)
		{
			return OrderMapper.Instance.Update(order) ;
		}

		public static void Delete (Nullable <Int32> OrderId)
		{
			OrderMapper.Instance.Delete (OrderId);
		}

		public static EntityList <Order> FindMany (OrderFinder finder, int Page, int PageSize)
		{
			return OrderMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Order> FindMany (OrderFinder finder)
		{
			return OrderMapper.Instance.FindMany (finder);
		}

		public static Order FindOne (OrderFinder finder)
		{
			Order Order = OrderMapper.Instance.FindOne(finder);
			return Order ?? Empty;
		}

		public static int GetCount (OrderFinder finder)
		{
			return OrderMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
