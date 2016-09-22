using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using USDutyGear.Core.Models;

namespace USDutyGear.Core.Common
{
    public static class ProductHelper
    {
        public static decimal CalculateProductPrice(string fullModel, Product product, List<ProductAdjustment> adjustments)
        {
            var modelNumber = ModelNumber.Create(product.ModelRegex, fullModel);

            // check packages for overriding price
            if (!string.IsNullOrWhiteSpace(modelNumber.Package)) // the current product is a package model
            {
                var packages = adjustments.Where(x => x.Type == ProductAdjustmentTypes.Package).ToList();

                // check for packages dependent on other models first
                if (packages.Any(x => x.DependentModels.Length > 0))
                {
                    // get the first package where all of the dependent models are contained in the collection of adjustment models
                    var dependentPackage = packages.FirstOrDefault(x => x.DependentModels.All(y => modelNumber.AdjustmentModels.Contains(y)));

                    if (dependentPackage != null)
                    {
                        // found a model dependent package so override the price
                        return dependentPackage.PriceAdjustment;
                    }
                }

                // no model dependent packages matched so return the default package price
                var package = packages.FirstOrDefault(x => x.DependentModels.Length == 0);
                return package?.PriceAdjustment ?? 0;
            }

            // if here then the current product is not a package

            // set the base price
            var price = product.Price;

            // adjust for finish
            if (!string.IsNullOrWhiteSpace(modelNumber.Finish))
                price += adjustments.FirstOrDefault(x => x.Type == ProductAdjustmentTypes.Finish && x.Model == modelNumber.Finish)?.PriceAdjustment ?? 0;

            // adjust for buckle
            if (!string.IsNullOrWhiteSpace(modelNumber.Buckle))
                price += adjustments.FirstOrDefault(x => x.Type == ProductAdjustmentTypes.Buckle && x.Model == modelNumber.Buckle)?.PriceAdjustment ?? 0;

            // adjust for snap
            if (!string.IsNullOrWhiteSpace(modelNumber.Snap))
                price += adjustments.FirstOrDefault(x => x.Type == ProductAdjustmentTypes.Snap && x.Model == modelNumber.Snap)?.PriceAdjustment ?? 0;

            // adjust for size
            if (!string.IsNullOrWhiteSpace(modelNumber.Size))
                price += adjustments.FirstOrDefault(x => x.Type == ProductAdjustmentTypes.Size && x.Model == modelNumber.Size)?.PriceAdjustment ?? 0;

            // adjust for inner liner
            if (!string.IsNullOrWhiteSpace(modelNumber.InnerLiner))
                price += adjustments.FirstOrDefault(x => x.Type == ProductAdjustmentTypes.InnerLiner && x.Model == modelNumber.InnerLiner)?.PriceAdjustment ?? 0;

            return price;
        }

        public static string BuildProductTitle(string model, Product product, List<ProductAdjustment> adjustments)
        {
            var match = product.ModelRegex.Match(model);
            var title = new StringBuilder(product.Title);

            title = ProductAdjustmentTypes.All.Aggregate(title, (current, adjustmentType) =>
                current.Replace($"{adjustmentType}", GetProductAdjustment(adjustments, match, adjustmentType)?.Name ?? string.Empty));

            if (match.Groups["Package"].Success)
                title = title.Append($" ({GetProductAdjustment(adjustments, match, ProductAdjustmentTypes.Package).Name})");

            return title.ToString();
        }

        private static ProductAdjustment GetProductAdjustment(List<ProductAdjustment> adjustments, Match match, string adjustmentType)
        {
            // extract the adjustment model from the model string via regex
            var adjustmentModel = match.Groups[adjustmentType].Value;
            // get the price adjustment by the adjustment model and type
            var adjustment = adjustments.FirstOrDefault(x => x.Model == adjustmentModel && x.Type == adjustmentType);
            return adjustment;
        }
    }
}
