using System.Collections.Generic;

namespace USDutyGear.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Model { get; set; }
        public List<int> Sizes { get; set; }
        public string Finish { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingCost { get; set; }
        public string Sku { get; set; }
    }
}
