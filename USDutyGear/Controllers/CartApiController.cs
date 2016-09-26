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
                var packages = Products.GetProductPackages(product.Model);

                // calculate the price of this particular object
                var results = ProductHelper.GetTitleAndPrice(item.Model, product, adjustments, packages);
                item.Title = results.Item1;
                item.Price = results.Item2;
                item.Total = item.Price * item.Quantity;

                cart.SubTotal += item.Total;
            }

            // apply shipping and tax if a service code is specified
            cart.Shipping = 7.99m;

            // TODO: apply tax

            cart.GrandTotal = cart.SubTotal + cart.Shipping;

            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }
    }
}
