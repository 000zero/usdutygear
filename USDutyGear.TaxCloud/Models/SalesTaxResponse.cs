using System.Collections.Generic;

namespace USDutyGear.TaxCloud.Models
{
    public class SalesTaxResponse : TaxCloudResponse
    {
        public SalesTaxResponse()
        {
            CartItemsResponse = new List<CartItemResponse>();
        }

        public string CartID { get; set; }
        public List<CartItemResponse> CartItemsResponse { get; set; }
    }
}
