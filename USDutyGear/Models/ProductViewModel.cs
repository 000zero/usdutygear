using System.Collections.Generic;
using System.Linq;
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
        public List<string> Details { get; set; }
        public Dictionary<string, string[]> Images { get; set; }
        public string SelectedImage { get; set; }
        public List<ProductAdjustment> Finishes { get; set; }
        public List<ProductAdjustment> Sizes { get; set; }
        public List<ProductAdjustment> Snaps { get; set; }
        public List<ProductAdjustment> Packages { get; set; }

        private ProductViewModel(Product product, List<ProductAdjustment> adjustments, List<string> details, Dictionary<string, string[]> images)
        {
            Name = product.Name;
            Model = product.Model;
            Category = product.Category;
            Price = product.Price;
            Details = details;
            Images = images;
            SelectedImage = product.DefaultImageModel;
            Finishes = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Finish)
                .ToList();
            Sizes = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Size)
                .ToList();
            Snaps = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Snap)
                .ToList();
            Packages = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Package)
                .ToList();
        }

        public static ProductViewModel Create(Product products, List<ProductAdjustment> adjustments, List<string> details, Dictionary<string, string[]> images)
        {
            return new ProductViewModel(products, adjustments, details, images);
        }
    }
}