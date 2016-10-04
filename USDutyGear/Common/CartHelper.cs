using System.Collections.Generic;
using USDutyGear.Core.Common;
using USDutyGear.Data;
using USDutyGear.Models;

namespace USDutyGear.Common
{
    public static class CartHelper
    {
        /// <summary>
        /// Sets the Title, Price, and Totals for the cart based on the model number and quantites
        /// </summary>
        public static List<CartItem> FillCartItemInfo(List<CartItem> items)
        {
            // cart comes in with only the models and quantities, need to set the rest
            foreach (var item in items)
            {
                var tokens = item.Model.Split('-');

                // get the product model
                var product = Products.GetProductByModel(tokens[0]);
                var adjustments = Products.GetProductAdjustmentsByModel(product.Model);
                var packages = Products.GetProductPackages(product.Model);

                // calculate the price of this particular object
                var results = ProductHelper.GetTitleAndPrice(item.Model, product, adjustments, packages);
                item.Title = results.Item1;
                item.Price = results.Item2;
                item.Total = item.Price * item.Quantity;
            }

            return items;
        }
    }
}