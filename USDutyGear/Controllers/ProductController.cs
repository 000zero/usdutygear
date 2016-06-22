using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDutyGear.Models;
using USDutyGear.Core.Models;
using USDutyGear.Data;

namespace USDutyGear.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Product(string name)
        {
            // get all products of the same name
            var products = Products.GetProductsByName(name);

            // create the view model object

            return View();
        }
    }
}
