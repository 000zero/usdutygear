using System;

namespace USDutyGear.Core.Models
{
    public class ProductPromotion
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Percentage { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public bool IsEnabled { get; set; }
    }
}
