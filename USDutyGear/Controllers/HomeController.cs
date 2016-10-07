using System.Web.Mvc;
using USDutyGear.Models;

namespace USDutyGear.Controllers
{
    public class HomeController : Controller
    {
        protected static HomeViewModel ViewModel = HomeViewModel.Create();

        public ActionResult Home()
        {
            return View(ViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View(ViewModel);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View(ViewModel);
        }
    }
}