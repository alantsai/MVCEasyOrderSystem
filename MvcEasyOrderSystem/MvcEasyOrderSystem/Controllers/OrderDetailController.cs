using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEasyOrderSystem.Models;

namespace MvcEasyOrderSystem.Controllers
{
    public class OrderDetailController : Controller
    {
        private EOSystemContex db = new EOSystemContex();

        //
        // GET: /OrderDetail/

        public ActionResult Index()
        {
            var orderdetial = db.OrderDetial.Include(o => o.Meal).Include(o => o.Order);
            return View(orderdetial.ToList());
        }

        //
        // GET: /OrderDetail/Details/5

        public ActionResult Details(int id = 0)
        {
            OrderDetial orderdetial = db.OrderDetial.Find(id);
            if (orderdetial == null)
            {
                return HttpNotFound();
            }
            return View(orderdetial);
        }

        //
        // GET: /OrderDetail/Create

        public ActionResult Create()
        {
            ViewBag.MealId = new SelectList(db.Meal, "MealId", "MealName");
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "UserId");
            return View();
        }

        //
        // POST: /OrderDetail/Create

        [HttpPost]
        public ActionResult Create(OrderDetial orderdetial)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetial.Add(orderdetial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MealId = new SelectList(db.Meal, "MealId", "MealName", orderdetial.MealId);
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "UserId", orderdetial.OrderId);
            return View(orderdetial);
        }

        //
        // GET: /OrderDetail/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderDetial orderdetial = db.OrderDetial.Find(id);
            if (orderdetial == null)
            {
                return HttpNotFound();
            }
            ViewBag.MealId = new SelectList(db.Meal, "MealId", "MealName", orderdetial.MealId);
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "UserId", orderdetial.OrderId);
            return View(orderdetial);
        }

        //
        // POST: /OrderDetail/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderDetial orderdetial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderdetial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MealId = new SelectList(db.Meal, "MealId", "MealName", orderdetial.MealId);
            ViewBag.OrderId = new SelectList(db.Order, "OrderId", "UserId", orderdetial.OrderId);
            return View(orderdetial);
        }

        //
        // GET: /OrderDetail/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrderDetial orderdetial = db.OrderDetial.Find(id);
            if (orderdetial == null)
            {
                return HttpNotFound();
            }
            return View(orderdetial);
        }

        //
        // POST: /OrderDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetial orderdetial = db.OrderDetial.Find(id);
            db.OrderDetial.Remove(orderdetial);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}