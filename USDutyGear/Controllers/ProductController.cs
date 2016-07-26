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
            var product = Products.GetProductByName(name);

            var details = Products.GetProductDetailsByName(name);

            var images = Products.GetProductImagesByName(name);

            var adjustments = Products.GetProductAdjustmentsByModel(product.Model);

            // create the view model object
            var vm = ProductViewModel.Create(product, adjustments, details, images);

            return View(vm);
        }
    }
}
