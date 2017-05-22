using System;

using CMS.Ecommerce;
using CMS.SiteProvider;
using CMS.Base;

namespace APIExamples
{
    /// <summary>
    /// Holds discount API examples.
    /// </summary>
    /// <pageTitle>Discounts</pageTitle>
    internal class Discounts
    {
        /// <summary>
        /// Holds volume discount API examples.
        /// </summary>
        /// <groupHeading>Volume discounts</groupHeading>
        private class VolumeDiscounts
        {
            /// <heading>Creating a volume discount</heading>
            private void CreateVolumeDiscount()
            {
                // Gets a product for the volume discount
                SKUInfo product = SKUInfoProvider.GetSKUs()
                                               .WhereStartsWith("SKUName", "New")
                                               .FirstObject;

                if (product != null)
                {
                    // Creates a new volume discount object
                    VolumeDiscountInfo newDiscount = new VolumeDiscountInfo();

                    // Sets the volume discount properties
                    newDiscount.VolumeDiscountMinCount = 100;
                    newDiscount.VolumeDiscountValue = 20;
                    newDiscount.VolumeDiscountSKUID = product.SKUID;
                    newDiscount.VolumeDiscountIsFlatValue = false;

                    // Saves the volume discount to the database
                    VolumeDiscountInfoProvider.SetVolumeDiscountInfo(newDiscount);
                }
            }


            /// <heading>Updating a volume discount</heading>
            private void GetAndUpdateVolumeDiscount()
            {
                // Gets a product
                SKUInfo product = SKUInfoProvider.GetSKUs()
                                                .WhereEquals("SKUName", "NewProduct")
                                                .FirstObject;

                if (product != null)
                {
                    // Gets the first volume discount defined for the product
                    VolumeDiscountInfo discount = VolumeDiscountInfoProvider.GetVolumeDiscounts(product.SKUID).FirstObject;
                    if (discount != null)
                    {
                        // Updates the volume discount properties
                        discount.VolumeDiscountMinCount = 800;

                        // Saves the changes to the database
                        VolumeDiscountInfoProvider.SetVolumeDiscountInfo(discount);
                    }
                }
            }


            /// <heading>Updating multiple volume discounts</heading>
            private void GetAndBulkUpdateVolumeDiscounts()
            {
                // Gets a product
                SKUInfo product = SKUInfoProvider.GetSKUs()
                                               .WhereStartsWith("SKUName", "New")
                                               .FirstObject;

                if (product != null)
                {
                    // Gets the product's volume discounts
                    var discounts = VolumeDiscountInfoProvider.GetVolumeDiscounts(product.SKUID);

                    // Loops through the volume discounts
                    foreach (VolumeDiscountInfo discount in discounts)
                    {
                        // Updates the volume discount properties
                        discount.VolumeDiscountMinCount = 500;

                        // Saves the changes to the database
                        VolumeDiscountInfoProvider.SetVolumeDiscountInfo(discount);
                    }
                }
            }


            /// <heading>Deleting a volume discount</heading>
            private void DeleteVolumeDiscount()
            {
                // Gets a product
                SKUInfo product = SKUInfoProvider.GetSKUs()
                                               .WhereStartsWith("SKUName", "New")
                                               .FirstObject;

                if (product != null)
                {
                    // Gets the first volume discount defined for the product
                    VolumeDiscountInfo discount = VolumeDiscountInfoProvider.GetVolumeDiscounts(product.SKUID).FirstObject;
                    if (discount != null)
                    {
                        // Deletes the volume discount
                        VolumeDiscountInfoProvider.DeleteVolumeDiscountInfo(discount);
                    }
                }
            }
        }


        /// <summary>
        /// Holds product coupon API examples.
        /// </summary>
        /// <groupHeading>Product coupons</groupHeading>
        private class ProductCoupons
        {
            /// <heading>Creating a product coupon</heading>
            private void CreateProductCoupon()
            {
                // Creates a new product coupon object
                DiscountCouponInfo newCoupon = new DiscountCouponInfo();

                // Sets the product coupon properties
                newCoupon.DiscountCouponDisplayName = "New coupon";
                newCoupon.DiscountCouponCode = "NewCoupon";
                newCoupon.DiscountCouponIsExcluded = true;
                newCoupon.DiscountCouponIsFlatValue = true;
                newCoupon.DiscountCouponValue = 200;
                newCoupon.DiscountCouponValidFrom = DateTime.Now;
                newCoupon.DiscountCouponSiteID = SiteContext.CurrentSiteID;

                // Saves the product coupon to the databae
                DiscountCouponInfoProvider.SetDiscountCouponInfo(newCoupon);
            }


            /// <heading>Updating a product coupon</heading>
            private void GetAndUpdateProductCoupon()
            {
                // Gets the product coupon
                DiscountCouponInfo updateCoupon = DiscountCouponInfoProvider.GetDiscountCouponInfo("NewCoupon", SiteContext.CurrentSiteName);
                if (updateCoupon != null)
                {
                    // Updates the product coupon properties
                    updateCoupon.DiscountCouponDisplayName = updateCoupon.DiscountCouponDisplayName.ToLowerCSafe();

                    // Saves the changes to the database
                    DiscountCouponInfoProvider.SetDiscountCouponInfo(updateCoupon);
                }
            }


            /// <heading>Updating multiple product coupons</heading>
            private void GetAndBulkUpdateDiscountCoupons()
            {
                // Gets all product coupons whose code starts with 'New'
                var coupons = DiscountCouponInfoProvider.GetDiscountCoupons()
                                                        .WhereStartsWith("DiscountCouponCode", "New");

                // Loops through the product coupons
                foreach (DiscountCouponInfo modifyCoupon in coupons)
                {
                    // Updates the product coupon properties
                    modifyCoupon.DiscountCouponDisplayName = modifyCoupon.DiscountCouponDisplayName.ToUpper();

                    // Saves the changes to the database
                    DiscountCouponInfoProvider.SetDiscountCouponInfo(modifyCoupon);
                }
            }


            /// <heading>Adding products to a product coupon</heading>
            private void AddProductToCoupon()
            {
                // Gets the discounted product
                SKUInfo product = SKUInfoProvider.GetSKUs()
                                               .WhereStartsWith("SKUName", "New")
                                               .WhereNull("SKUOptionCategoryID")
                                               .FirstObject;

                // Gets the product coupon
                DiscountCouponInfo discountCoupon = DiscountCouponInfoProvider.GetDiscountCouponInfo("NewCoupon", SiteContext.CurrentSiteName);

                if ((discountCoupon != null) && (product != null))
                {
                    // Adds the product to the coupon
                    SKUDiscountCouponInfoProvider.AddDiscountCouponToSKU(product.SKUID, discountCoupon.DiscountCouponID);
                }
            }


            /// <heading>Removing products from a product coupon</heading>
            private void RemoveProductFromCoupon()
            {
                // Gets the discounted product
                var product = SKUInfoProvider.GetSKUs()
                                               .WhereStartsWith("SKUName", "New")
                                               .WhereNull("SKUOptionCategoryID")
                                               .FirstObject;

                // Gets the product coupon
                DiscountCouponInfo discountCoupon = DiscountCouponInfoProvider.GetDiscountCouponInfo("NewCoupon", SiteContext.CurrentSiteName);

                if ((discountCoupon != null) && (product != null))
                {
                    // Gets the object representing the product-coupon relationship
                    SKUDiscountCouponInfo skuDicountCoupon = SKUDiscountCouponInfoProvider.GetSKUDiscountCouponInfo(product.SKUID, discountCoupon.DiscountCouponID);

                    if (skuDicountCoupon != null)
                    {
                        // Removes the product from the product coupon
                        SKUDiscountCouponInfoProvider.DeleteSKUDiscountCouponInfo(skuDicountCoupon);
                    }
                }
            }


            /// <heading>Deleting a product coupon</heading>
            private void DeleteDiscountCoupon()
            {
                // Gets the product coupon
                DiscountCouponInfo deleteCoupon = DiscountCouponInfoProvider.GetDiscountCouponInfo("NewCoupon", SiteContext.CurrentSiteName);

                if (deleteCoupon != null)
                {
                    // Deletes the product coupon from the database
                    DiscountCouponInfoProvider.DeleteDiscountCouponInfo(deleteCoupon);
                }
            }
        }
    }
}
