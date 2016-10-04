namespace USDutyGear.Core.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
    }
}
