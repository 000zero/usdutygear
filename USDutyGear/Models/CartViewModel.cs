using System.Collections.Generic;

namespace USDutyGear.Models
{
    public class CartViewModel : USDutyGearBaseViewModel
    {
        public List<ProductCartViewModel> Items { get; set; }
        public decimal CartTotal { get; set; }

        public CartViewModel()
        {
            Items = new List<ProductCartViewModel>();
        }
    }

    public class ProductCartViewModel
    {
        public string Title { get; set; }
        public string Model { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}