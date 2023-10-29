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
                var db = new Online_MarketEntities3();
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
            var db = new Online_MarketEntities3();
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
                var db = new Online_MarketEntities3();
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