using System.Collections.Generic;

namespace USDutyGear.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Model { get; set; }
        public List<string> Finishes { get; set; }
        public List<string> Details { get; set; }
        public decimal Price { get; set; }
        public string Sku { get; set; }
    }
}
