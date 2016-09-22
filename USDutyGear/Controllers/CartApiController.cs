using System.Net;
using System.Web.Http;
using System.Net.Http;
using USDutyGear.Data;
using USDutyGear.Models;
using USDutyGear.Core.Common;


namespace USDutyGear.Controllers
{
    [RoutePrefix("api/cart")]
    public class CartApiController : ApiController
    {
        [HttpPost]
        [Route("")]
        public HttpResponseMessage GetCartViewModel(CartViewModel cart)
        {
            // cart comes in with only the models and quantities, need to set the rest
            cart.SubTotal = 0.0m;
            foreach (var item in cart.Items)
            {
                var tokens = item.Model.Split('-');

                // get the product model
                var product = Products.GetProductByModel(tokens[0]);
                var adjustments = Products.GetProductAdjustmentsByModel(product.Model);

                // calculate the price of this particular object
                item.Price = ProductHelper.CalculateProductPrice(item.Model, product, adjustments);
                item.Title = ProductHelper.BuildProductTitle(item.Model, product, adjustments);
                item.Total = item.Price * item.Quantity;

                cart.SubTotal += item.Total;
            }

            // apply shipping and tax
            cart.Shipping = 7.99m;

            // TODO: apply tax

            cart.GrandTotal = cart.SubTotal + cart.Shipping;

            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }
    }
}
