using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MRTmvc.Models;

namespace MRTmvc.Controllers
{
    public class AdminnController : Controller
    {
        private MrtContext db = new MrtContext();

        
        public ActionResult IndexAdmin()
        {
                return View();
           

        }




        // GET: Adminn
        public ActionResult UserRoute()
        {
           
                return View(db.Routes.ToList());
           
        }

            // GET: Adminn/Details/5
            public ActionResult Details(int? id)
        {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Route route = db.Routes.Find(id);
                if (route == null)
                {
                    return HttpNotFound();
                }
                return View(route);

            



        }

        // GET: Adminn/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Adminn/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StoreID,UserID,Email,tripType,LevelCategory,StationFrom,StationTo,NumTicket,DateTime,Charge,DiscountPercent")] Route route)
        {
            if (ModelState.IsValid)
            {
                db.Routes.Add(route);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(route);
        }

        // GET: Adminn/Edit/5
        public ActionResult Edit(int? id)
        {


           
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Route route = db.Routes.Find(id);
                if (route == null)
                {
                    return HttpNotFound();
                }
                return View(route);
            
        }

        // POST: Adminn/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StoreID,UserID,Email,tripType,LevelCategory,StationFrom,StationTo,NumTicket,DateTime,Charge,DiscountPercent")] Route route)
        {
            if (ModelState.IsValid)
            {
                db.Entry(route).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(route);
        }

        // GET: Adminn/Delete/5
        public ActionResult Delete(int? id)
        {
           
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Route route = db.Routes.Find(id);
                if (route == null)
                {
                    return HttpNotFound();
                }
                return View(route);
            
        }

        // POST: Adminn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Route route = db.Routes.Find(id);
            db.Routes.Remove(route);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
