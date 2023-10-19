using E_Shop.EF;
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
        public ActionResult Details(int id)
        {
            var db = new Online_ShopEntities();
            var data = db.Products.Find(id);
            ViewBag.Catagories = (from c in db.Catagories where c.Id == data.CatagoryId select c).ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(Product d)
        {
            var db = new Online_ShopEntities();
            var data = db.Products.Find(d.Id);
            data.Name = d.Name;
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

        public ActionResult ShowAllProducts()
        {
            var db = new Online_ShopEntities();
            var data = db.Products.ToList();
            return View(data);
        }




        [HttpGet]
        public ActionResult AddCatagories()
        {
            var db = new Online_ShopEntities();
            ViewBag.Departments = db.Catagories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddCatagories(Catagory c)
        {
            var db = new Online_ShopEntities();
            db.Catagories.Add(c);
            db.SaveChanges();
            return RedirectToAction("ViewAllCatagories");
        }
        public ActionResult ViewAllCatagories()
        {
            var db = new Online_ShopEntities();
            var data = db.Catagories.ToList();
            return View(data);
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