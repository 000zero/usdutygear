using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using USDutyGear.Core.Models;
using USDutyGear.Data;

namespace USDutyGear.MissionControl.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            // set null so all products come back
            var products = Products.GetProducts(isActive: null);

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            var product = Products.GetProductById(id);

            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View(new Product());
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var product = new Product()

                {
                    Name = collection["Name"],
                    Category = collection["Category"],
                    Model = collection["Model"],
                    Title = collection["Title"],
                    Price = Convert.ToDecimal(collection["Price"]),
                    ModelTemplate = collection["ModelTemplate"],
                    ModelRegex = new Regex(collection["ModelRegex"]),
                    DisplayOrder = Convert.ToInt32(collection["DisplayOrder"]),
                    Description = collection["Description"]
                };

                Products.Save(product);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            var product = Products.GetProductById(id);

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var product = CreateProductFromFormCollection(collection);

                Products.Save(product);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static Product CreateProductFromFormCollection(FormCollection collection)
        {
            var product = new Product()

            {
                Name = collection["Name"],
                Category = collection["Category"],
                Model = collection["Model"],
                Title = collection["Title"],
                Price = Convert.ToDecimal(collection["Price"]),
                ModelTemplate = collection["ModelTemplate"],
                ModelRegex = new Regex(collection["ModelRegex"]),
                DisplayOrder = Convert.ToInt32(collection["DisplayOrder"]),
                Description = collection["Description"]
            };

            return product;

        }
    }
}
