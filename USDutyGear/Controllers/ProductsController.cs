using System.Web.Mvc;
using USDutyGear.Models;

namespace USDutyGear.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : Controller
    {
        [Route("{model}")]
        public ActionResult Product(string model)
        {
            // create the view model object
            var vm = ProductViewModel.Create(model);

            return View(vm);
        }

        [Route("categories/{category}")]
        public ActionResult Products(string category)
        {
            var vm = ProductsViewModel.Create(category);
            return View(vm);
        }
    }
}
