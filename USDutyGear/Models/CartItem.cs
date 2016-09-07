using USDutyGear.Core.Models;

namespace USDutyGear.Models
{
    public class CartItem
    {
        public string Title { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string PriceDisplay => Price.ToString("C");
        public decimal Total { get; set; }
        public string TotalDisplay => Total.ToString("C");
        public ProductDiscount Discount { get; set; }
    }
}