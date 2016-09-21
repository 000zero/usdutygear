using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Shipping { get; set; }
        public string ShippingServiceCode { get; set; }
        public decimal Tax { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public CartViewModel()
        {
            Items = new List<CartItem>();
        }
    }
}