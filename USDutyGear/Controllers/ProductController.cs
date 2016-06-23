using System.Web.Mvc;
using USDutyGear.Models;
using USDutyGear.Data;
using USDutyGear.Core.Common;

namespace USDutyGear.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        [Route("{name}")]
        public ActionResult GetProductOverviewByName(string name)
        {
            name = MapRouteNameToProductName(name);

            // get all products of the same name
            var products = Products.GetProductsByName(name);

            var details = Products.GetProductDetailsByName(name);

            // create the view model object
            var vm = ProductViewModel.Create(products, details);

            return View(vm);
        }

        private string MapRouteNameToProductName(string name)
        {
            switch (name.ToLower())
            {
                case "linerbelt": return ProductNames.LinerBelt;
                case "belt": return ProductNames.Belt;
                case "sambrownebelt": return ProductNames.SamBrowneBelt;
                case "dutybelt": return ProductNames.DutyBelt;
                case "beltkeeper": return ProductNames.BeltKeeper;
                case "keyholderkeeper": return ProductNames.KeyHolderKeeper;
                case "gloveholder": return ProductNames.GloveHolder;
                case "magazineholder": return ProductNames.MagazineHolder;
                default: return null;
            }
        }
    }
}
