using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; }
        public decimal SubTotal { get; set; }
        public string SubTotalDisplay => SubTotal.ToString("C");
        public decimal Total { get; set; }
        public string TotalDisplay => Total.ToString("C");
        public decimal Shipping { get; set; }
        public string ShippingDisplay => Shipping.ToString("C");
        public decimal Tax { get; set; }
        public string TaxDisplay => Tax.ToString("C");

        public CartViewModel()
        {
            Items = new List<CartItem>();
        }
    }
}