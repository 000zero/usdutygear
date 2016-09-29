using System.Linq;
using System.Collections.Generic;
using Microsoft.Ajax.Utilities;
using USDutyGear.Data;
using USDutyGear.Core.Models;
using USDutyGear.Core.Common;

namespace USDutyGear.Models
{
    public class ProductViewModel : USDutyGearBaseViewModel
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<string> Details { get; set; }
        public Dictionary<string, string[]> Images { get; set; }
        public string SelectedImage { get; set; }
        public List<ProductAdjustment> Finishes { get; set; }
        public List<ProductAdjustment> Sizes { get; set; }
        public List<ProductAdjustment> Snaps { get; set; }
        public List<ProductAdjustment> Buckles { get; set; }
        public List<ProductAdjustment> InnerLiners { get; set; }
        public List<ProductPackage> Packages { get; set; }
        public Dictionary<string, decimal> Prices { get; set; } 

        private ProductViewModel(Product product, List<ProductAdjustment> adjustments, List<string> details, Dictionary<string, string[]> images, List<ProductPackage> packages)
        {
            Name = product.Name;
            Model = product.Model;
            Category = product.Category;
            Price = product.Price;
            Description = product.Description;
            Details = details;
            Images = images;
            Finishes = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Finish)
                .OrderBy(x => x.Priority)
                .ToList();
            Snaps = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Snap)
                .OrderBy(x => x.Priority)
                .ToList();
            Buckles = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Buckle)
                .OrderBy(x => x.Priority)
                .ToList();
            InnerLiners = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.InnerLiner)
                .OrderBy(x => x.Priority)
                .ToList();
            Sizes = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Size)
                .DistinctBy(x => x.Model)
                .OrderBy(x => x.Priority)
                .ToList();

            if ((packages?.Count ?? 0) > 0)
            {
                // add single package
                Packages = new List<ProductPackage>
                {
                    new ProductPackage
                    {
                        Name = "Single",
                        Model = null,
                        Price = 0
                    }
                };

                Packages.AddRange(packages.DistinctBy(x => x.Model));
            }
            else
                Packages = null;

            // build price list
            Prices = new Dictionary<string, decimal>();
            foreach (var m in Products.GetPossibleModels(product.Model))
                Prices.Add(m, ProductHelper.GetTitleAndPrice(m, product, adjustments, packages).Item2);
        }

        public static ProductViewModel Create(string model)
        {
            // get the product by model then load up the products related table data
            var product = Products.GetProductByModel(model);
            var details = Products.GetProductDetailsByName(product.Model);
            var images = Products.GetProductImagesByModel(product.Model);
            var adjustments = Products.GetProductAdjustmentsByModel(product.Model);
            var packages = Products.GetProductPackages(product.Model);

            return new ProductViewModel(product, adjustments, details, images, packages);
        }
    }
}