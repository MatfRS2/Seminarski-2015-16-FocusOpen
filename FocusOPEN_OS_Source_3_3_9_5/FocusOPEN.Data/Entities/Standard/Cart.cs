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
	/// This object represents the properties and methods of a Cart.
	/// </summary>
	public partial class Cart
	{
		#region Constructor
		
		protected Cart()
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
		
		public static Cart New ()
		{
			return new Cart() ;
		}

		public static Cart Empty
		{
			get { return NullCart.Instance; }
		}

		public static Cart Get (Nullable <Int32> CartId)
		{
			Cart Cart = CartMapper.Instance.Get (CartId);
			return Cart ?? Empty;
		}

		public static Cart Update (Cart cart)
		{
			return CartMapper.Instance.Update(cart) ;
		}

		public static void Delete (Nullable <Int32> CartId)
		{
			CartMapper.Instance.Delete (CartId);
		}

		public static EntityList <Cart> FindMany (CartFinder finder, int Page, int PageSize)
		{
			return CartMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Cart> FindMany (CartFinder finder)
		{
			return CartMapper.Instance.FindMany (finder);
		}

		public static Cart FindOne (CartFinder finder)
		{
			Cart Cart = CartMapper.Instance.FindOne(finder);
			return Cart ?? Empty;
		}

		public static int GetCount (CartFinder finder)
		{
			return CartMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
