using E_ShopTask3.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_ShopTask3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["CustomerUsernmae"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var db = new Online_MarketEntities5();
                var data = db.Products.ToList();
                return View(data);
            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var db = new Online_MarketEntities5();
            var matchs = db.Customers.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (matchs != null)
            {
                Session["CustomerUsernmae"] = matchs.Username;
                Session["CustomerPassword"] = matchs.Password;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }

        [HttpGet]
        public ActionResult AddToCart(int id)
        {
            if (Session["CustomerUsernmae"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var db = new Online_MarketEntities5();
                var productToAdd = (from d in db.Products where d.ProductId == id select d).SingleOrDefault();
                List<Product> cart = Session["Cart"] as List<Product> ?? new List<Product>();

                // Check if the product with the same name and category already exists in the cart
                var existingProduct = cart.FirstOrDefault(p => p.ProductId == productToAdd.ProductId);

                if (existingProduct != null)
                {
                    // If the product exists, update its quantity in the session
                    cart.Remove(existingProduct);
                    existingProduct.Quantity += 1;
                    cart.Add(existingProduct);
                }
                else
                {
                    // If the product does not exist, add it to the cart with quantity 1
                    productToAdd.Quantity = 1;
                    cart.Add(productToAdd);
                }

                Session["Cart"] = cart;
                return View(cart);
            }
        }
        [HttpGet]
        public ActionResult ConfirmOrder()
        {
            if (Session["CustomerUsernmae"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve cart items from session
            var cart = Session["Cart"] as List<Product> ?? new List<Product>();

            // Calculate total quantity and price
            int totalQuantity = cart.Sum(p => p.Quantity);
            decimal totalPrice = cart.Sum(p => p.Price * p.Quantity);

            // Get the currently logged-in customer's ID
            int customerId = GetCustomerIdFromUsername(Session["CustomerUsernmae"].ToString());

            // Create a new order object with the calculated values
            Order order = new Order
            {
                CustomerId = customerId,
                IssueDate = DateTime.Now,
                Quantity = totalQuantity,
                Price = totalPrice,
                Status = "Pending" // You can set the initial status as needed
            };

            // Save the order to the database
            var db = new Online_MarketEntities5();
            db.Orders.Add(order);
            db.SaveChanges();

            // Optionally: Save order items to a separate table, if necessary
            foreach (var product in cart)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = product.ProductId,
                    Quantity = product.Quantity
                };
                db.OrderItems.Add(orderItem);
            }
            db.SaveChanges();

            // Clear the cart in the session after the order is confirmed
            Session["Cart"] = new List<Product>();

            return View(order);
        }

        private int GetCustomerIdFromUsername(string username)
        {
            using (var db = new Online_MarketEntities5()) // Replace Online_MarketEntities3 with your actual DbContext class
            {
                var customer = db.Customers.FirstOrDefault(c => c.Username == username);
                if (customer != null)
                {
                    return customer.CustomerId;
                }
                else
                {
                    // Handle the case where the customer with the given username is not found
                    // You might want to log this or return a default/fallback customer ID.
                    // For now, returning -1 as a placeholder value.
                    return -1;
                }
            }
        }


        [HttpGet]
        public ActionResult ViewCart()
        {
            if (Session["CustomerUsernmae"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                List<Product> cart = Session["Cart"] as List<Product> ?? new List<Product>();
                //return RedirectToAction("Index");
                return View(cart);
            }
        }

        public ActionResult RemoveFromCart(int id)
        {
            if (Session["CustomerUsernmae"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                List<Product> cart = Session["Cart"] as List<Product> ?? new List<Product>();
                var productToRemove = cart.FirstOrDefault(p => p.ProductId == id);

                if (productToRemove != null)
                {
                    cart.Remove(productToRemove);
                    Session["Cart"] = cart;
                }

                return RedirectToAction("ViewCart");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}