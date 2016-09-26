namespace USDutyGear.Models
{
    public class CartItem
    {
        public string Title { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public string DiscountDescription { get; set; }
        public decimal Tax { get; set; }
    }
}