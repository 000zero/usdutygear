using System.Net;
using System.Web.Http;
using System.Net.Http;
using USDutyGear.Common;
using USDutyGear.Models;


namespace USDutyGear.Controllers
{
    [RoutePrefix("api/cart")]
    public class CartApiController : ApiController
    {
        [HttpPost]
        [Route("")]
        public HttpResponseMessage GetCartViewModel(CartViewModel cart)
        {
            cart.Items = CartHelper.FillCartItemInfo(cart.Items);

            return Request.CreateResponse(HttpStatusCode.OK, cart);
        }
    }
}
