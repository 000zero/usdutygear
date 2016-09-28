using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDutyGear.Models;

namespace USDutyGear.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Checkout(CartViewModel cart)
        {
            // calculate price of products

            // get the shipping price

            // set the grand total

            // 

            return View();
        }

        [HttpPost]
        public ActionResult Receipt()
        {
            return View();
        }
    }
}
