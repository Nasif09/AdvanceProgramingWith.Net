using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class NGOController : Controller
    {
        private readonly Zero_HungerEntities3 DB;

        public NGOController()
        {
            DB = new Zero_HungerEntities3();
        }

        // Action for displaying all details of collect requests
        public ActionResult ViewCollectRequests()
        {
            var collectRequests = DB.CollectRequests.ToList();
            return View(collectRequests);
        }

        // Action for displaying details of a specific collect request
        public ActionResult CollectRequestDetails(int id)
        {
            var collectRequest = DB.CollectRequests.Find(id);

            if (collectRequest == null)
            {
                // Handle the case where the collect request is not found
                return HttpNotFound();
            }

            return View(collectRequest);
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

            return RedirectToAction("ViewCollectRequests"); // Or any other appropriate action
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