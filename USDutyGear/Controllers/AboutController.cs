using System.Web.Mvc;
using USDutyGear.Models;

namespace USDutyGear.Controllers
{
    public class AboutController : Controller
    {
        private static USDutyGearBaseViewModel vm;

        static AboutController()
        {
            vm = new USDutyGearBaseViewModel();
        }

        // GET: About
        public ActionResult About()
        {
            return View(vm);
        }

        public ActionResult PrivacyPolicy()
        {
            return View(vm);
        }

        public ActionResult WarrantyPolicy()
        {
            return View(vm);
        }
    }
}