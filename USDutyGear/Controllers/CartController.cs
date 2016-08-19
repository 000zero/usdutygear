using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USDutyGear.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : Controller
    {
        // GET: Cart
        [HttpPost]
        [Route("")]
        public ActionResult Index(Dictionary<string, int> cart)
        {
            // go through the cart and figure out the price

            return View();
        }
    }
}
