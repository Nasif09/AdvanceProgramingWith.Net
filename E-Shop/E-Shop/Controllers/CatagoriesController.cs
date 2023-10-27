using E_Shop.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Shop.Controllers
{
    public class CatagoriesController : Controller
    {
        // GET: Categories
        public ActionResult ShowCatagory()
        {
            var db = new Online_ShopEntities();
            var data = db.Catagories.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult AddCatagories()
        {
            var db = new Online_ShopEntities();
            ViewBag.Catagories = db.Catagories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddCatagories(Catagory c)
        {
            var db = new Online_ShopEntities();
            db.Catagories.Add(c);
            db.SaveChanges();
            return RedirectToAction("ShowCatagory");
        }

        public ActionResult Delete(int id)
        {
            var db = new Online_ShopEntities();
            var data = db.Catagories.Find(id);
            db.Catagories.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ShowCatagory");
        }

        [HttpGet]
        public ActionResult EditCatagory(int id)
        {
            var db = new Online_ShopEntities();
            var res = db.Catagories.Find(id);
            ViewBag.Catagories = new SelectList(db.Catagories, "Id", "Name");
            return View(res);
        }
        [HttpPost]
        public ActionResult EditCatagory(Catagory catagory)
        {
            var db = new Online_ShopEntities();

            bool catagoryExists = db.Catagories.Any(c => c.Name == catagory.Name);
            if (catagoryExists)
            {
                ModelState.AddModelError("Name", "A product with the same name already exists.");
                return View(catagory);
            }
            var exdata = db.Catagories.Find(catagory.Id);
            exdata.Name = catagory.Name;
            exdata.Id = catagory.Id;
            db.SaveChanges();

            return RedirectToAction("ShowCatagory");
        }
        public ActionResult CatagoryDetails(int id)
        {
            var db = new Online_ShopEntities();
            var res = db.Catagories.Find(id);
            if (res != null)
            {
                var category = db.Catagories.FirstOrDefault(c => c.Id == res.Id);

                if (category != null)
                {
                    ViewBag.CategoryName = category.Name;
                    ViewBag.CatagoryID = category.Id;
                }
                else
                {
                    ViewBag.CategoryName = "Category Not Found";
                }
            }

            return View(res);
        }
    }
}