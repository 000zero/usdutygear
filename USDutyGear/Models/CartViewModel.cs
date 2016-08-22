using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class CartViewModel : USDutyGearBaseViewModel
    {
        public List<ProductCartViewModel> Items { get; set; }

        private CartViewModel(Dictionary<string, string> cart)
        {
            Items = new List<ProductCartViewModel>();
            foreach (var item in cart)
            {
                
            }
        }

        public static CartViewModel Create(Dictionary<string, string> cart)
        {
            return new CartViewModel(cart);
        }
    }

    public class ProductCartViewModel
    {
        public string Title { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}