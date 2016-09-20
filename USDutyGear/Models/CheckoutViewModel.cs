using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class CheckoutViewModel
    {
        public List<CartItem> Items { get; set; }
        public decimal Shipping { get; set; }
        public string ShippingDescription { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
    }
}