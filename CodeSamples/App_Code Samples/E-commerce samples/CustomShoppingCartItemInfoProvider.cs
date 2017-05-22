using System;
using System.Collections.Generic;

using CMS;
using CMS.Ecommerce;
using CMS.Base;

// Custom provider registration. Uncomment the following line to enable the custom provider.
//[assembly: RegisterCustomProvider(typeof(CustomShoppingCartItemInfoProvider))]

/// <summary>
/// Sample shopping cart item info provider. 
/// </summary>
public class CustomShoppingCartItemInfoProvider : ShoppingCartItemInfoProvider
{
    #region "Example: Custom discounts applied to the shopping cart item: 1) Buy product A, get product B with 20% discount"

    /// <summary>
    /// Returns list of all discounts which should be applied to the specified shopping cart item.
    /// </summary>
    /// <param name="item">Shopping cart item</param>       
    protected override List<IItemDiscount> GetDiscountsInternal(ShoppingCartItemInfo item)
    {
        // SKU number of product A
        string skuA = "A";

        // Get default discounts
        List<IItemDiscount> discounts = base.GetDiscountsInternal(item);

        // Add extra discounts to product A 
        if (item.SKU.SKUNumber.ToLowerCSafe() == skuA.ToLowerCSafe())
        {
            // Add extra discount if product B is also in the cart
            AddDiscountForBundledPurchase(item, discounts, "B");
        }

        return discounts;
    }

    #region "Private methods"

    /// <summary>
    /// Adds custom discount to the given shopping cart item if another product is also in the cart.
    /// </summary>
    /// <param name="item">Shopping cart item</param>
    /// <param name="discounts">Discounts of the shopping cart item</param>
    /// <param name="anotherItemName">SKU number of another product which must be in the cart to apply the discount</param>
    private void AddDiscountForBundledPurchase(ShoppingCartItemInfo item, List<IItemDiscount> discounts, string anotherItemName)
    {
        // Add discount to the product if product B is also in the cart
        ShoppingCartItemInfo itemB = GetShoppingCartItem(item.ShoppingCart, anotherItemName);
        if (itemB != null)
        {
            // Create custom 20% discount
            ItemDiscount discount = new ItemDiscount()
            {
                ItemDiscountID = "DISCOUNT_AB",
                ItemDiscountDisplayName = string.Format("Discount for bundled purchase of product '{0}' and '{1}'", item.SKU.SKUName, itemB.SKU.SKUName),
                ItemDiscountValue = 20,
                ItemDiscountedUnits = itemB.CartItemUnits
            };

            // Add custom discount to discounts to be applied to the product
            discounts.Add(discount);
        }
    }


    /// <summary>
    /// Returns shopping cart item with specified SKU number.
    /// </summary>
    /// <param name="cart">Shopping cart</param>
    /// <param name="skuNumber">Shopping cart item SKU number</param>    
    private ShoppingCartItemInfo GetShoppingCartItem(ShoppingCartInfo cart, string skuNumber)
    {
        // Cart not found
        if (cart == null)
        {
            return null;
        }

        skuNumber = skuNumber.ToLowerCSafe();

        // Try to find item with specified SKU number
        foreach (ShoppingCartItemInfo item in cart.CartItems)
        {
            if (item.SKU.SKUNumber.ToLowerCSafe() == skuNumber)
            {
                // Item found
                return item;
            }
        }

        // Item not found
        return null;
    }

    #endregion

    #endregion
}