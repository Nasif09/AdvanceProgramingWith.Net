using E_Market.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace E_Market.Controllers
{
    public class CustomerController : Controller
    {
        private EMarketEntities db = new EMarketEntities();
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var db = new EMarketEntities();
                var data = db.Products.ToList();
                return View(data);
            }
        }
    public ActionResult AddToCart(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                var cart = Session["Cart"] as List<Dictionary<string, object>> ?? new List<Dictionary<string, object>>();
                var existingItem = cart.FirstOrDefault(item => (int)item["ProductId"] == id);

                if (existingItem != null)
                {
                    existingItem["Quantity"] = (int)existingItem["Quantity"] + 1;
                }
                else
                {
                    var newItem = new Dictionary<string, object>
                {
                    { "ProductId", id },
                    { "ProductName", product.Name },
                    { "Price", product.Price },
                    { "Quantity", 1 }
                };

                    cart.Add(newItem);
                }
                Session["Cart"] = cart;
                return RedirectToAction("ViewCart");
            }
            return HttpNotFound();
        }
        public ActionResult ConfirmOrder()
        {
            var cart = Session["Cart"] as List<Dictionary<string, object>>;
            if (cart != null && cart.Any())
            {
                var userId = (int)Session["UserId"];
                var order = new Order
                {
                    CustomerId = userId,
                    IssueDate = DateTime.Now,
                    Quantity = cart.Sum(item => Convert.ToInt32(item["Quantity"])),
                    Price = cart.Sum(item => Convert.ToInt32(item["Price"]) * Convert.ToInt32(item["Quantity"])),
                    //Price = 500,
                    Status = "Confirmed" 
                };
                db.Orders.Add(order);
                db.SaveChanges();
                var orderId = order.OrderId;
                foreach (var item in cart)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = orderId,
                        ProductId = (int)item["ProductId"],
                        Quantity = Convert.ToInt32(item["Quantity"])
                    };

                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges();
                Session["Cart"] = null;
                return RedirectToAction("OrderConfirmation", new { orderId = orderId });
            }
            return RedirectToAction("ViewCart");
        }
        public ActionResult OrderConfirmation(int orderId)
        {
            var order = db.Orders.Find(orderId);
            return View(order);
        }
        public ActionResult ViewCart()
        {
            var cart = Session["Cart"] as List<Dictionary<string, object>> ?? new List<Dictionary<string, object>>();

            return View(cart);
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login", "Home");
        }

    }
}
