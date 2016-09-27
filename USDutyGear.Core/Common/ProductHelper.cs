using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using USDutyGear.Core.Models;

namespace USDutyGear.Core.Common
{
    public static class ProductHelper
    {
        public static Tuple<string, decimal> GetTitleAndPrice(string fullModel, Product product, List<ProductAdjustment> adjustments, List<ProductPackage> packages = null)
        {
            var modelNumber = ModelNumber.Create(product.ModelRegex, fullModel);
            var title = BuildProductTitle(fullModel, product, adjustments);

            // check packages for overriding price
            if (!string.IsNullOrWhiteSpace(modelNumber.Package)) // the current product is a package model
            {
                // verify the package was passed in
                if (packages == null)
                    throw  new Exception($"Package specified in model # {fullModel} but no product package found!");

                // find the package price for this particular model number
                // NOTE: if the regex in the product_packages.applicable_model is ever not C# compliant then split it into two columns
                // one for the C# code to use here and the other for the MySQL query search in the Products.cs file
                foreach (var p in packages)
                {
                    var regex = new Regex(p.ApplicableModelRegexStr);
                    if (regex.IsMatch(fullModel.Replace($"-{modelNumber.Package}", "")))
                        return new Tuple<string, decimal>($"{title} ({p.Name})", p.Price);
                }

                throw new Exception($"Package specified in model # {fullModel} but no product package found!");
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

            // adjust for inner liner
            if (!string.IsNullOrWhiteSpace(modelNumber.InnerLiner))
                price += adjustments.FirstOrDefault(x => x.Type == ProductAdjustmentTypes.InnerLiner && x.Model == modelNumber.InnerLiner)?.PriceAdjustment ?? 0;

            // adjust for size
            if (!string.IsNullOrWhiteSpace(modelNumber.Size))
            {
                var adjustment = adjustments.FirstOrDefault(x =>
                    x.Type == ProductAdjustmentTypes.Size && 
                    x.Model == modelNumber.Size &&
                    !string.IsNullOrWhiteSpace(x.DependentModelsRegexStr) &&
                    x.DependentModelsRegex.IsMatch(fullModel)) ??
                                 adjustments.FirstOrDefault(x => x.Type == ProductAdjustmentTypes.Size && x.Model == modelNumber.Size);

                price += adjustment?.PriceAdjustment ?? 0;
            }

            return new Tuple<string, decimal>(title, price);
        }

        private static string BuildProductTitle(string model, Product product, List<ProductAdjustment> adjustments)
        {
            var match = product.ModelRegex.Match(model);
            var title = new StringBuilder(product.Title);

            title = ProductAdjustmentTypes.All.Aggregate(title, (current, adjustmentType) =>
                current.Replace($"{{{adjustmentType}}}", GetProductAdjustment(adjustments, match, adjustmentType)?.Name ?? string.Empty));

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
