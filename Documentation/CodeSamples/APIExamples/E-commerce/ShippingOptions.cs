using System;

using CMS.Ecommerce;
using CMS.SiteProvider;
using CMS.Base;

namespace APIExamples
{
    /// <summary>
    /// Holds shipping option API examples.
    /// </summary>
    /// <pageTitle>Shipping options</pageTitle>
    internal class ShippingOptions
    {
        /// <heading>Creating a new shipping option</heading>
        private void CreateShippingOption()
        {
            // Creates a new shipping option object
            ShippingOptionInfo newOption = new ShippingOptionInfo();

            // Sets the shipping option properties
            newOption.ShippingOptionDisplayName = "New option";
            newOption.ShippingOptionName = "NewOption";
            newOption.ShippingOptionSiteID = SiteContext.CurrentSiteID;
            newOption.ShippingOptionCharge = 1.0;
            newOption.ShippingOptionEnabled = true;

            // Saves the shipping option to the database
            ShippingOptionInfoProvider.SetShippingOptionInfo(newOption);
        }


        /// <heading>Updating a shipping option</heading>
        private void GetAndUpdateShippingOption()
        {
            // Gets the shipping option
            ShippingOptionInfo updateOption = ShippingOptionInfoProvider.GetShippingOptionInfo("NewOption", SiteContext.CurrentSiteName);
            if (updateOption != null)
            {
                // Updates the shipping option properties
                updateOption.ShippingOptionDisplayName = updateOption.ShippingOptionDisplayName.ToLowerCSafe();

                // Saves the changes to the database
                ShippingOptionInfoProvider.SetShippingOptionInfo(updateOption);
            }
        }


        /// <heading>Updating multiple shipping options</heading>
        private void GetAndBulkUpdateShippingOptions()
        {
            // Gets all shipping options on the current site whose code name starts with 'NewOption'
            var options = ShippingOptionInfoProvider.GetShippingOptions()
                                                    .OnSite(SiteContext.CurrentSiteID)
                                                    .WhereStartsWith("ShippingOptionName", "NewOption");

            // Loops through the shipping options
            foreach (ShippingOptionInfo option in options)
            {
                // Updates the shipping option properties
                option.ShippingOptionDisplayName = option.ShippingOptionDisplayName.ToUpper();

                // Saves the changes to the database
                ShippingOptionInfoProvider.SetShippingOptionInfo(option);
            }
        }        


        /// <heading>Applying a tax class to a shipping option</heading>
        private void AddTaxClassToOption()
        {
            // Gets the shipping option
            ShippingOptionInfo option = ShippingOptionInfoProvider.GetShippingOptionInfo("NewOption", SiteContext.CurrentSiteName);

            // Gets the tax class
            TaxClassInfo taxClass = TaxClassInfoProvider.GetTaxClassInfo("NewClass", SiteContext.CurrentSiteName);

            if ((option != null) && (taxClass != null))
            {
                // Creates a new object representing the shipping option-tax class relationship
                ShippingOptionTaxClassInfo optionTax = new ShippingOptionTaxClassInfo();

                // Assigns the tax class to the shipping option
                optionTax.TaxClassID = taxClass.TaxClassID;
                optionTax.ShippingOptionID = option.ShippingOptionID;

                // Saves the relationship to the database
                ShippingOptionTaxClassInfoProvider.SetShippingOptionTaxClassInfo(optionTax);
            }
        }


        /// <heading>Removing a tax class from a shipping option</heading>
        private void RemoveTaxClassFromOption()
        {
            // Gets the shipping option
            ShippingOptionInfo option = ShippingOptionInfoProvider.GetShippingOptionInfo("NewOption", SiteContext.CurrentSiteName);

            // Gets the tax class
            TaxClassInfo taxClass = TaxClassInfoProvider.GetTaxClassInfo("NewClass", SiteContext.CurrentSiteName);

            if ((option != null) && (taxClass != null))
            {
                // Gets the object representing the shipping option-tax class relationship
                ShippingOptionTaxClassInfo deleteOptionTax = 
                    ShippingOptionTaxClassInfoProvider.GetShippingOptionTaxClassInfo(option.ShippingOptionID, taxClass.TaxClassID);

                if (deleteOptionTax != null)
                {
                    // Deletes the relationship from the database
                    ShippingOptionTaxClassInfoProvider.DeleteShippingOptionTaxClassInfo(deleteOptionTax);
                }
            }
        }


        /// <heading>Adding a new shipping cost to a shipping option</heading>
        private void AddShippingCostToOption()
        {
            /*
             * Note: The example only works correctly for shipping options that use shipping costs
             * stored in the COM_ShippingCost database table (includes all options based on the Default carrier).
             * You may need a different approach for shipping options of custom carriers.
            */

            // Gets the shipping option
            ShippingOptionInfo option = ShippingOptionInfoProvider.GetShippingOptionInfo("NewOption", SiteContext.CurrentSiteName);
            if (option != null)
            {
                // Creates a new shipping cost object
                ShippingCostInfo cost = new ShippingCostInfo();

                // Sets the shipping cost properties
                cost.ShippingCostMinWeight = 10;
                cost.ShippingCostValue = 9.9;

                // Assigns the shipping option to which the shipping cost applies
                cost.ShippingCostShippingOptionID = option.ShippingOptionID;

                // Saves the shipping cost to the database
                ShippingCostInfoProvider.SetShippingCostInfo(cost);
            }
        }


        /// <heading>Removing a shipping cost from a shipping option</heading>
        private void RemoveShippingCostFromOption()
        {
            /*
             * Note: The example only works correctly for shipping options that use shipping costs
             * stored in the COM_ShippingCost database table (includes all options based on the Default carrier).
             * You may need a different approach for shipping options of custom carriers.
            */

            // Gets the shipping option
            ShippingOptionInfo option = ShippingOptionInfoProvider.GetShippingOptionInfo("NewOption", SiteContext.CurrentSiteName);
            if (option != null)
            {
                // Gets the cost assigned to the shipping option for a specified weight (10)
                ShippingCostInfo deleteCost = ShippingCostInfoProvider.GetShippingCostInfo(option.ShippingOptionID, 10);
                if (deleteCost != null)
                {
                    // Deletes the shipping cost
                    ShippingCostInfoProvider.DeleteShippingCostInfo(deleteCost);
                }
            }
        }


        /// <heading>Deleting a shipping option</heading>
        private void DeleteShippingOption()
        {
            // Gets the shipping option
            ShippingOptionInfo deleteOption = ShippingOptionInfoProvider.GetShippingOptionInfo("NewOption", SiteContext.CurrentSiteName);

            if (deleteOption != null)
            {
                // Deletes the shipping option
                ShippingOptionInfoProvider.DeleteShippingOptionInfo(deleteOption);
            }
        }
    }
}
