using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class CollectRequestController : Controller
    {
        private readonly Zero_HungerEntities3 DB; 

        public CollectRequestController()
        {
            DB = new Zero_HungerEntities3();
        }

        public ActionResult Index()
        {
            var collectRequests = DB.CollectRequests.ToList();
            return View(collectRequests);
        }

        [HttpGet]
        public ActionResult OpenCollectRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OpenCollectRequest(CollectRequest collectRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(collectRequest);
            }
            collectRequest.Status = "Requested";
            DB.CollectRequests.Add(collectRequest);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var collectRequest = DB.CollectRequests.Include("FoodItems").FirstOrDefault(c => c.RequestID == id);

            if (collectRequest == null)
            {
                return HttpNotFound();
            }
            return View(collectRequest);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {

            var collectRequest = DB.CollectRequests.Find(id);

            if (collectRequest == null)
            {
                return HttpNotFound();
            }
            return View(collectRequest);
        }

        [HttpPost]
        public ActionResult Edit(CollectRequest collectRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(collectRequest);
            }

            DB.Entry(collectRequest).State = EntityState.Modified;

            DB.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var collectRequest = DB.CollectRequests.Find(id);

            if (collectRequest == null)
            {
                return HttpNotFound();
            }
            var foodItemsToRemove = DB.FoodItems.Where(fi => fi.RequestID == id);
            DB.FoodItems.RemoveRange(foodItemsToRemove);
            DB.CollectRequests.Remove(collectRequest);
            DB.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DB.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}