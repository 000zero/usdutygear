using System.Collections.Generic;
using System.Linq;
using USDutyGear.Core.Models;
using USDutyGear.Core.Common;

namespace USDutyGear.Models
{
    public class ProductViewModel : USDutyGearBaseViewModel
    {
        private ProductViewModel(Product products, List<ProductAdjustment> adjustments, List<string> details, List<string> images)
        {
            Name = products.Name;
            Category = products.Category;
            Details = details;
            Images = images;
            SelectedImage = images.FirstOrDefault();
            Finishes = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Finish)
                .ToList();
            Sizes = adjustments
                .Where(x => x.Type == ProductAdjustmentTypes.Size)
                .ToList();
        }

        public static ProductViewModel Create(Product products, List<ProductAdjustment> adjustments, List<string> details, List<string> images)
        {
            return new ProductViewModel(products, adjustments, details, images);
        }

        public string Name { get; set; }
        public string Category { get; set; }
        public List<string> Details { get; set; }
        public List<string> Images { get; set; }
        public string SelectedImage { get; set; }
        public List<ProductAdjustment> Finishes { get; set; }
        public List<ProductAdjustment> Sizes { get; set; }
    }
}