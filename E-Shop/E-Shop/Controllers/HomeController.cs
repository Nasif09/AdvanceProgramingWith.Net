using E_Shop.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabTask1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new Online_ShopEntities();
            var data = db.Products.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            var db = new Online_ShopEntities();
            ViewBag.Catagories = new SelectList(db.Catagories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            var db = new Online_ShopEntities();
            bool productExists = db.Products.Any(p => p.Name == product.Name);
            if (productExists)
            {
                ModelState.AddModelError("Name", "A product with the same name already exists.");
                return View(product);
            }
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var db = new Online_ShopEntities();
            var res = db.Products.Find(id);
            if (res != null)
            {
                var category = db.Catagories.FirstOrDefault(c => c.Id == res.CatagoryId);

                if (category != null)
                {
                    ViewBag.CatagoryName = category.Name;
                }
                else
                {
                    ViewBag.CatagoryName = "Category Not Found";
                }
            }

            return View(res);
        }
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new Online_ShopEntities();
            var res = db.Products.Find(id);
            ViewBag.Catagories = new SelectList(db.Catagories, "Id", "Name");
            return View(res);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var db = new Online_ShopEntities();

            bool productExists = db.Products.Any(p => p.Name == product.Name);
            if (productExists)
            {
                ModelState.AddModelError("Name", "A product with the same name already exists.");
                return View(product);
            }
            var exdata = db.Products.Find(product.Id);
            exdata.Name = product.Name;
            exdata.CatagoryId = product.CatagoryId;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var db = new Online_ShopEntities();
            var data = db.Products.Find(id);
            db.Products.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Index");
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