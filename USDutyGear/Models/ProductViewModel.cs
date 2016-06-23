using System.Collections.Generic;
using System.Linq;
using USDutyGear.Core.Models;

namespace USDutyGear.Models
{
    public class ProductViewModel
    {
        private ProductViewModel(List<Product> products, List<string> details)
        {
            Name = products.First().Name;
            Category = products.First().Category;
            Details = details;
            AvailableFinishes = products.Select(x => x.Finish).Distinct().ToList();
            Products = products.Select(x => new ProductStub
            {
                Model = x.Model,
                Finish = x.Finish,
                Price = x.Price,
                ShippingCost = x.ShippingCost,
                Sizes = x.Sizes
            }).ToList();

        }

        public static ProductViewModel Create(List<Product> products, List<string> details)
        {
            return products.Count > 0
                ? new ProductViewModel(products, details)
                : null;
        }

        public string Name { get; set; }
        public string Category { get; set; }
        public List<string> AvailableFinishes { get; set; }
        public List<string> Details { get; set; }
        public List<ProductStub> Products { get; set; } 
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