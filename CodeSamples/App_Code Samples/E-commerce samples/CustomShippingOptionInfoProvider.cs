using System;

using CMS;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Base;

// Custom provider registration. Uncomment the following line to enable the custom provider.
//[assembly: RegisterCustomProvider(typeof(CustomShippingOptionInfoProvider))]

/// <summary>
/// Sample shipping option info provider. 
/// </summary>
public class CustomShippingOptionInfoProvider : ShippingOptionInfoProvider
{
    #region "Example: Custom shipping option selection"

    /// <summary>
    /// Ensures that USPS shipping option is applicable only if shipping address is in USA. 
    /// </summary>
    /// <param name="cart">Shopping cart data</param>
    /// <param name="shippingOption">Shipping option to check applicability for.</param>
    /// <returns></returns>
    protected override bool IsShippingOptionApplicableInternal(ShoppingCartInfo cart, ShippingOptionInfo shippingOption)
    {
        // Do not check availability if shopping cart or shipping option object is not available
        if ((cart == null) || (shippingOption == null))
        {
            return true;
        }

        if (shippingOption.ShippingOptionName == "USPS")
        {
            IAddress address = cart.ShoppingCartShippingAddress ?? cart.ShoppingCartBillingAddress;
            var country = CountryInfoProvider.GetCountryInfo(address.AddressCountryID);
            if (country == null)
            {
                return true;
            }

            return (country.CountryThreeLetterCode == "USA");
        }

        return true;
    }

    #endregion


    #region "Example: Custom shipping calculation"

    /// <summary>
    /// Calculates shipping charge for the given shopping cart.
    /// Shipping taxes are not included. Result is in site main currency.
    /// </summary>
    /// <param name="cart">Shopping cart data</param>
    protected override double CalculateShippingInternal(ShoppingCartInfo cart)
    {
        // ------------------------
        // Please note:         
        // You can customize this method to calculate shipping by a on-line shipping calculation service as well.
        // All the data which might be required for the calculation service is stored in the ShoppingCartInfo object, e.g.:
        // - use cart.ShoppingCartBillingAddress to get billing address info
        // - use cart.ShoppingCartShippingAddress to get shipping address info        
        // etc.
        // ------------------------

        // Calculates shipping based on customer's shipping address country
        if (cart != null)
        {
            // Get shipping address details, use billing address if shipping address is not entered
            IAddress address = cart.ShoppingCartShippingAddress ?? cart.ShoppingCartBillingAddress;

            if (address != null)
            {
                // Get shipping address country
                CountryInfo country = CountryInfoProvider.GetCountryInfo(address.AddressCountryID);
                if ((country != null) && (country.CountryName.ToLowerCSafe() != "usa"))
                {
                    // Get extra shipping for non-usa customers from 'ShippingExtraCharge' custom setting 
                    double extraCharge = SettingsKeyInfoProvider.GetDoubleValue("ShippingExtraCharge");

                    // Add an extra charge to standard shipping price for non-usa customers
                    return base.CalculateShippingInternal(cart) + extraCharge;
                }
            }
        }

        // Calculate shipping option without tax in default way
        return base.CalculateShippingInternal(cart);
    }

    #endregion
}