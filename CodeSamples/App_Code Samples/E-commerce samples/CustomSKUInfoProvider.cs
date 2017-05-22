using System;

using CMS;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Base;

// Custom provider registration. Uncomment the following line to enable the custom provider.
//[assembly: RegisterCustomProvider(typeof(CustomSKUInfoProvider))]

/// <summary>
/// Sample shopping cart info provider. 
/// </summary>
public class CustomSKUInfoProvider : SKUInfoProvider
{
    #region "Example: Custom SKU base price: 1) SKU price according to the culture 2) SKU price according to the SKU status"

    /// <summary>
    /// Returns catalog price of the given product based on the SKU data and shopping cart data. 
    /// By default price is loaded from the SKUPrice field.
    /// </summary>
    /// <param name="sku">SKU data</param>
    /// <param name="cart">Shopping cart data</param>
    protected override double GetSKUPriceInternal(SKUInfo sku, ShoppingCartInfo cart)
    {
        double price = 0;

        // 1) Get base SKU price according to the shopping cart data (current culture)
        if (cart != null)
        {
            switch (cart.ShoppingCartCulture.ToLowerCSafe())
            {
                case "en-us":
                    // Get price form SKUEnUsPrice field
                    price = ValidationHelper.GetDouble(sku.GetValue("SKUEnUsPrice"), 0);
                    break;

                case "cs-cz":
                    // Get price form SKUCsCzPrice field
                    price = ValidationHelper.GetDouble(sku.GetValue("SKUCsCzPrice"), 0);
                    break;
            }
        }

        //// 2) Get base SKU price according to the product properties (product status)
        //PublicStatusInfo status = PublicStatusInfoProvider.GetPublicStatusInfo(sku.SKUPublicStatusID);
        //if ((status != null) && (status.PublicStatusName.ToLowerCSafe() == "discounted"))
        //{
        //    // Get price form SKUDiscountedPrice field
        //    price = ValidationHelper.GetDouble(sku.GetValue("SKUDiscountedPrice"), 0);
        //}

        if (price == 0)
        {
            // Get price from the SKUPrice field by default
            return base.GetSKUPriceInternal(sku, cart);
        }

        // Return custom price
        return price;
    }

    #endregion
}