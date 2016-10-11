using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDutyGear.Data;

namespace USDutyGear.MissionControl.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            var orders = Orders.GetOrders();

            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            var order = Orders.GetOrder(id);

            return View(order);
        }
    }
}
