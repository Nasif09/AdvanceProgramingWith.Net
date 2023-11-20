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
        public ActionResult ViewCollectRequests()
        {
            var collectRequests = DB.CollectRequests.ToList();
            return View(collectRequests);
        }

        public ActionResult CollectRequestDetails(int id)
        {
            var collectRequest = DB.CollectRequests.Find(id);

            if (collectRequest == null)
            {
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
            return RedirectToAction("ViewCollectRequests"); 
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