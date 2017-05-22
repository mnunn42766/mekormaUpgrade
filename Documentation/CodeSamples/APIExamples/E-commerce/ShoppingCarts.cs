using System;
using System.Collections.Generic;

using CMS.Ecommerce;

namespace APIExamples
{
    /// <summary>
    /// Holds shopping cart API examples.
    /// </summary>
    /// <pageTitle>Shopping carts</pageTitle>
    internal class ShoppingCarts
    {
        /// <heading>Adding products to a shopping cart</heading>
        private void AddProductToShoppingCart()
        {
            // Gets a product to add to the shopping cart
            SKUInfo product = SKUInfoProvider.GetSKUs()
                                                .WhereEquals("SKUName", "NewProduct")
                                                .WhereNull("SKUOptionCategoryID")
                                                .FirstObject;

            if (product != null)
            {
                // Gets the current shopping cart
                ShoppingCartInfo cart = ECommerceContext.CurrentShoppingCart;

                // Saves the cart to the database
                ShoppingCartInfoProvider.SetShoppingCartInfo(cart);

                // Prepares a shopping cart item representing 1 unit of the product
                ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, 1);
                ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);

                // Saves the shopping cart item to the shopping cart
                ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
            }
        }


        /// <heading>Updating the number of units of a shopping cart item</heading>
        private void UpdateShoppingCartItemUnits()
        {
            // Gets the product
            SKUInfo product = SKUInfoProvider.GetSKUs()
                                                .WhereEquals("SKUName", "NewProduct")
                                                .WhereNull("SKUOptionCategoryID")
                                                .FirstObject;

            if (product != null)
            {
                // Prepares the shopping cart item
                ShoppingCartItemInfo item = null;

                // Gets the current shopping cart
                ShoppingCartInfo cart = ECommerceContext.CurrentShoppingCart;

                // Loops through the items in the shopping cart
                foreach (ShoppingCartItemInfo cartItem in cart.CartItems)
                {
                    // Gets the first shopping cart item matching the specified product
                    if (cartItem.SKUID == product.SKUID)
                    {
                        item = cartItem;
                        break;
                    }
                }

                if (item != null)
                {
                    // Updates the shopping cart item properties (sets the number of units to 2)
                    ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, 2);
                    item = cart.SetShoppingCartItem(parameters);

                    // Saves the shopping cart to the database
                    ShoppingCartInfoProvider.SetShoppingCartInfo(cart);

                    // Saves the shopping cart item to the database
                    ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(item);
                }
            }
        }


        /// <heading>Removing products from a shopping cart</heading>
        private void RemoveProductFromShoppingCart()
        {
            // Gets the current shopping cart
            ShoppingCartInfo cart = ECommerceContext.CurrentShoppingCart;

            // Gets the product
            SKUInfo product = SKUInfoProvider.GetSKUs()
                                                .WhereEquals("SKUName", "NewProduct")
                                                .WhereNull("SKUOptionCategoryID")
                                                .FirstObject;

            if ((cart != null) && (product != null))
            {
                // Gets the first item matching the product in the current shopping cart
                ShoppingCartItemInfo item = ShoppingCartItemInfoProvider.GetShoppingCartItems()
                                                        .WhereEquals("SKUID", product.SKUID)
                                                        .WhereEquals("ShoppingCartID", cart.ShoppingCartID)
                                                        .FirstObject;

                if (item != null)
                {
                    // Removes the item from the shopping cart
                    cart.RemoveShoppingCartItem(item.CartItemID);

                    // Removes the item from the database
                    ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(item);
                }
            }
        }
    }
}
