using System;
using System.Data;

using CMS.Ecommerce;
using CMS.Base;
using CMS.Helpers;
using CMS;

// Custom provider registration. Uncomment the following line to enable the custom provider.
//[assembly: RegisterCustomProvider(typeof(CustomShoppingCartInfoProvider))]

/// <summary>
/// Sample shopping cart info provider. 
/// </summary>
public class CustomShoppingCartInfoProvider : ShoppingCartInfoProvider
{
    #region "Example: Custom discount applied to the order total price"

    /// <summary>
    /// Calculates discount which should be applied to the total items price.
    /// </summary>
    /// <param name="cart">Shopping cart</param>        
    protected override double CalculateOrderDiscountInternal(ShoppingCartInfo cart)
    {
        // Default order discount
        double result = base.CalculateOrderDiscountInternal(cart);

        // Order total price
        double totalPrice = cart.TotalItemsPriceInMainCurrency;

        // Example of order discount based on the time of shopping - Happy hours (4 PM - 7 PM)
        if ((DateTime.Now.Hour >= 16) && (DateTime.Now.Hour <= 19))
        {
            // 20% discount 
            result = result + totalPrice * 0.2;
        }

        return result;
    }

    #endregion


    #region "Example: Add product A to the shopping cart -> product B is also added (automatically)"

    /// <summary>
    /// Adds new item to the shopping cart object and returns its object.
    /// </summary>
    /// <param name="cart">Shopping cart</param>
    /// <param name="itemParams">Parameters from which the new shopping cart item is initialized.</param>
    protected override ShoppingCartItemInfo AddShoppingCartItemInternal(ShoppingCartInfo cart, ShoppingCartItemParameters itemParams)
    {
        // Add item to the shopping cart
        ShoppingCartItemInfo item = base.AddShoppingCartItemInternal(cart, itemParams);

        // SKU number of product A
        string skuA = "A";

        // SKU number of product B
        string skuB = "B";

        // Add product B to the shopping cart automatically after the product A is added
        if (item.SKU.SKUNumber.ToLowerCSafe() == skuA.ToLowerCSafe())
        {
            DataSet products = SKUInfoProvider.GetSKUs().WhereEquals("SKUNumber", skuB);
            if (!DataHelper.DataSourceIsEmpty(products))
            {
                // Get data of the product B
                SKUInfo productB = new SKUInfo(products.Tables[0].Rows[0]);

                // Configure product B
                ShoppingCartItemParameters itemBParams = new ShoppingCartItemParameters(productB.SKUID, itemParams.Quantity);

                // Add/update product B in the shopping cart
                SetShoppingCartItem(cart, itemBParams);
            }
        }
        // Set newly added item in database (if shopping cart exists in database)
        if ((cart.ShoppingCartID > 0) && (item.CartItemID == 0))
        {
            ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(item);
        }

        return item;
    }

    #endregion


    #region "Example: Adding custom SKU columns to shopping cart content table and displaying them on the invoice"

    // These two simple customizations enable you to display values from your custom SKU columns in your order content table on invoice, 
    // meaning you can then use {%MyColumn1%} and {%MyColumn2%} macros in "Ecommerce.Transformations.Order_ContentTable" TXT/XML transformation 
    // which is used for displaying invoice content table by default.

    /// <summary>
    /// Creates an empty shopping cart content table.
    /// </summary> 
    protected override DataTable CreateContentTableInternal()
    {
        // Create default table
        DataTable table = base.CreateContentTableInternal();

        // Add custom culumns
        table.Columns.Add(new DataColumn("MyColumn1", typeof(int)));
        table.Columns.Add(new DataColumn("MyColumn2", typeof(double)));

        return table;
    }


    /// <summary>
    /// Creates one row of the content table with the data which are retrieved from the specified shopping cart item.
    /// </summary>
    /// <param name="item">Shopping cart item</param>
    /// <param name="table">Table from which the content row is created</param>
    protected override DataRow CreateContentRowInternal(ShoppingCartItemInfo item, DataTable table)
    {
        // Get row with default data
        DataRow row = base.CreateContentRowInternal(item, table);

        // Set values to custom columns
        row["MyColumn1"] = ValidationHelper.GetInteger(item.SKU.GetValue("MyColumn1"), 0);
        row["MyColumn2"] = ValidationHelper.GetDouble(item.SKU.GetValue("MyColumn2"), 0);

        return row;
    }

    #endregion
}