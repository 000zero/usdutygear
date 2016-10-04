using System.Collections.Generic;

namespace USDutyGear.TaxCloud.Models
{
    public class SalesTaxResponse
    {
        public SalesTaxResponse()
        {
            CartItemsResponse = new List<CartItemResponse>();
            Messages = new List<string>();
        }

        public string CartID { get; set; }
        public List<CartItemResponse> CartItemsResponse { get; set; }
        public int ResponseType { get; set; }
        public List<string> Messages { get; set; } 
    }
}
