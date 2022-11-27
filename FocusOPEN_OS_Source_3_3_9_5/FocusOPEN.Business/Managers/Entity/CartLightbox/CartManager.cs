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
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public class CartManager : BaseCLOManager
	{
		#region Private Variables

		private List<Cart> m_CartItems = null;

		#endregion

		#region Constructor

		public CartManager(User user) : base(user)
		{
		}

		#endregion

		public List<Cart> CartList
		{
			get
			{
				if (m_CartItems == null)
				{
					// First get all of the items in the cart
					CartFinder cartFinder = new CartFinder {UserId = User.UserId.GetValueOrDefault(-1)};
					List<Cart> cartItems = Cart.FindMany(cartFinder);

					// Only get assets that the user can actually access
					m_CartItems = (from item in cartItems
					               where (EntitySecurityManager.CanViewAssetInfo(User, item.Asset))
					               orderby item.CartId ascending
					               select item).ToList();
				}

				return (m_CartItems);
			}
		}

		public Cart GetCartById(int cartId)
		{
			foreach (Cart cart in CartList)
			{
				if (cart.CartId == cartId)
					return cart;
			}

			return Cart.Empty;
		}

		/// <summary>
		/// Checks if the specified user has an asset with the specified
		/// ID in their cart.  Returns true if asset is in cart, otherwise false.
		/// </summary>
		public bool CartContainsAsset(int assetId)
		{
			return CartList.Any(cart => cart.AssetId == assetId);
		}

		/// <summary>
		/// Adds the asset with the specified ID to the user's cart.
		/// </summary>
		public void AddAssetToCart(int assetId)
		{
			if (CartContainsAsset(assetId))
				return;
			
			Cart cart = Cart.New();

			cart.UserId = User.UserId.GetValueOrDefault();
			cart.AssetId = assetId;
			cart.DateAdded = DateTime.Now;

			Cart.Update(cart);

			AuditLogManager.LogAssetAction(assetId, User, AuditAssetAction.AddedToCart);
			AuditLogManager.LogUserAction(User, AuditUserAction.AddToCart, string.Format("Added AssetId: {0} to cart", assetId));

			m_CartItems = null;
		}

		/// <summary>
		/// Remove the cart item for the specified asset from the user's cart
		/// </summary>
		public void RemoveAssetFromCart(int assetId)
		{
			foreach (Cart cart in CartList)
			{
				if (cart.AssetId == assetId)
				{
					Cart.Delete(cart.CartId);

					AuditLogManager.LogAssetAction(assetId, User, AuditAssetAction.RemovedFromCart);
					AuditLogManager.LogUserAction(User, AuditUserAction.RemoveFromCart, string.Format("Removed AssetId: {0} from cart", assetId));
				}
			}
			m_CartItems = null;
		}

		public void RemoveCartItemFromCart(int cartId)
		{
			Cart cart = GetCartById(cartId);
			RemoveCartItemFromCart(cart);
		}

		public void RemoveCartItemFromCart(Cart cart)
		{
			Cart.Delete(cart.CartId);

			AuditLogManager.LogAssetAction(cart.AssetId, User, AuditAssetAction.RemovedFromCart);
			AuditLogManager.LogUserAction(User, AuditUserAction.RemoveFromCart, string.Format("Removed AssetId: {0} from cart", cart.AssetId));
			
			m_CartItems = null;
		}

		/// <summary>
		/// Empties the specified user's cart
		/// </summary>
		/// <param name="log">Boolean value specifying whether the asset and user logs should be updated</param>
		public void EmptyCart(bool log)
		{
			foreach (Cart cartItem in CartList)
			{
				Cart.Delete(cartItem.CartId);

				if (log)
				{
					AuditLogManager.LogAssetAction(cartItem.AssetId, User, AuditAssetAction.RemovedFromCart, "User emptied cart");
					AuditLogManager.LogUserAction(User, AuditUserAction.EmptyCart, string.Format("User emptied cart.  Removed AssetId: {0} from cart", cartItem.AssetId));
				}
			}

			m_CartItems = null;
		}
	}
}