using System.Collections.Generic;
using System.Linq;
using USDutyGear.Core.Models;

namespace USDutyGear.Models
{
    public class ProductViewModel : USDutyGearBaseViewModel
    {
        private ProductViewModel(List<Product> products, List<string> details, List<string> images)
        {
            Name = products.First().Name;
            Category = products.First().Category;
            Details = details;
            Images = images;
            SelectedImage = images.FirstOrDefault();
            AvailableFinishes = products.Select(x => x.Finish).Distinct().ToList();
            Products = products.Select(x => new ProductStub
            {
                Model = x.Model,
                Finish = x.Finish,
                Price = x.Price,
                ShippingCost = x.ShippingCost,
                Sizes = x.Sizes
            }).ToList();
            SelectedProductIndex = 0;
        }

        public static ProductViewModel Create(List<Product> products, List<string> details, List<string> images)
        {
            return products.Count > 0
                ? new ProductViewModel(products, details, images)
                : null;
        }

        public string Name { get; set; }
        public string Category { get; set; }
        public List<string> AvailableFinishes { get; set; }
        public List<string> Details { get; set; }
        public List<string> Images { get; set; }
        public string SelectedImage { get; set; }
        public List<ProductStub> Products { get; set; }
        public int SelectedProductIndex { get; set; }
    }

    public class ProductStub
    {
        public string Model { get; set; }
        public string Finish { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingCost { get; set; }
        public List<int> Sizes { get; set; } 
    }
}