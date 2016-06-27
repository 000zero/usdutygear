using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDutyGear.Data;
using USDutyGear.Models;

namespace USDutyGear.Controllers
{
    public class HomeController : Controller
    {
        protected static HomeViewModel ViewModel = HomeViewModel.Create();

        public ActionResult Index()
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