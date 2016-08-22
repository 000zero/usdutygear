using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDutyGear.Models;

namespace USDutyGear.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : Controller
    {
        // GET: Cart
        [HttpPost]
        [Route("")]
        public ActionResult Index(Dictionary<string, string> cart)
        {
            // get all the product objects for the items in the cart

            // go through the cart and figure out the price
            var vm = CartViewModel.Create(cart);

            return View(vm);
        }
    }
}
