using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; }
        public decimal Total { get; set; }

        public CartViewModel()
        {
            Items = new List<CartItem>();
        }
    }
}