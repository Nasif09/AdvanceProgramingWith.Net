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
            // Find the collect request by id
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

            // Attach the entity to the context and mark it as modified
            DB.Entry(collectRequest).State = EntityState.Modified;

            // Save changes to the database
            DB.SaveChanges();

            return RedirectToAction("Index"); // Or any other appropriate action
        }
        public ActionResult Delete(int id)
        {
            // Find the collect request by id
            var collectRequest = DB.CollectRequests.Find(id);

            if (collectRequest == null)
            {
                return HttpNotFound(); // Or handle as appropriate
            }

            // Remove associated food items
            var foodItemsToRemove = DB.FoodItems.Where(fi => fi.RequestID == id);
            DB.FoodItems.RemoveRange(foodItemsToRemove);

            // Remove the collect request
            DB.CollectRequests.Remove(collectRequest);

            // Save changes to the database
            DB.SaveChanges();

            return RedirectToAction("Index"); // Or any other appropriate action
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