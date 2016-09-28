using System.Collections.Generic;

namespace USDutyGear.TaxCloud.Models
{
    public class SalesTaxRequest : TaxCloudRequest
    {
        public SalesTaxRequest()
        {
            cartItems = new List<CartItem>();
        }

        public string customerID { get; set; }
        public bool deliverdBySeller { get; set; }
        public string cartID { get; set; }
        public Address destination { get; set; }
        public Address origin { get; set; }
        public List<CartItem> cartItems { get; set; }
    }
}
