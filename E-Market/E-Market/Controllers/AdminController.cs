using E_Market.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace E_Market.Controllers
{
    public class AdminController : Controller
    {
        private EMarketEntities db = new EMarketEntities();
        public ActionResult EIndex()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var data = db.Orders.Include(o => o.OrderItems).ToList();
                return View(data);
            }
        }

        public ActionResult Products()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var data = db.Products.ToList();
                return View(data);
            }
        }
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(newProduct);
                db.SaveChanges();
                return RedirectToAction("Products"); 
            }

            return View(newProduct);
        }

        public ActionResult Process(int id)
        {
            var order = db.Orders.Find(id);

            if (order != null)
            {
                if (order.Status != "Processed")
                {
                    order.Status = "Processed";
                    var orderItems = db.OrderItems.Where(item => item.OrderId == id).ToList();

                    foreach (var item in orderItems)
                    {
                        var product = db.Products.Find(item.ProductId);

                        if (product != null)
                        {
                            product.ProductCount -= item.Quantity;
                        }
                    }
                    db.SaveChanges();

                    TempData["Msg"] = "Order processed successfully.";
                }
                else
                {
                    TempData["Msg"] = "Order is already processed.";
                }

                return RedirectToAction("EIndex");
            }

            return HttpNotFound();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Home");
        }
    }
}