using System.Web.Mvc;
using USDutyGear.Models;
using USDutyGear.Data;
using USDutyGear.Common;

namespace USDutyGear.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : Controller
    {
        [Route("{model}")]
        public ActionResult Product(string model)
        {
            //name = CategoryHelper.MapRouteKeyToProductName(name);

            // get all products of the same name
            var product = Products.GetProductByModel(model);

            var details = Products.GetProductDetailsByName(product.Model);

            var images = Products.GetProductImagesByModel(product.Model);

            var adjustments = Products.GetProductAdjustmentsByModel(product.Model);

            var packages = Products.GetProductPackages(product.Model);

            // create the view model object
            var vm = ProductViewModel.Create(product, adjustments, details, images, packages);

            return View(vm);
        }
    }
}
