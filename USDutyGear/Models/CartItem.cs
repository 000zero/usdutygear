namespace USDutyGear.Models
{
    public class CartItem
    {
        public string Title { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // TODO: get rid of
        public string PriceDisplay => Price.ToString("C");
        public decimal Total { get; set; }
        // TODO: get rid of
        public string TotalDisplay => Total.ToString("C");
        public decimal Discount { get; set; }
        public string DiscountDescription { get; set; }
        public decimal Tax { get; set; }
    }
}