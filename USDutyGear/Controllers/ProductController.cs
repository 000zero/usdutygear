using System.Web.Mvc;
using USDutyGear.Models;
using USDutyGear.Data;
using USDutyGear.Common;

namespace USDutyGear.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : Controller
    {
        [Route("{name}")]
        public ActionResult Index(string name)
        {
            name = CategoryHelper.MapRouteKeyToProductName(name);

            // get all products of the same name
            var products = Products.GetProductsByName(name);

            var details = Products.GetProductDetailsByName(name);

            var images = Products.GetProductImagesByName(name);

            // create the view model object
            var vm = ProductViewModel.Create(products, details, images);

            return View(vm);
        }
    }
}
