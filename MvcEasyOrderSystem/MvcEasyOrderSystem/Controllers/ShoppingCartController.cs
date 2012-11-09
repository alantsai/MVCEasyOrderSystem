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
    public class ShoppingCartController : Controller
    {
        private EOSystemContex db = new EOSystemContex();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            return View(db.ShoppingCarts.ToList());
        }

        //
        // GET: /ShoppingCart/Details/5

        public ActionResult Details(int id = 0)
        {
            ShoppingCart shoppingcart = db.ShoppingCarts.Find(id);
            if (shoppingcart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingcart);
        }

        //
        // GET: /ShoppingCart/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ShoppingCart/Create

        [HttpPost]
        public ActionResult Create(ShoppingCart shoppingcart)
        {
            if (ModelState.IsValid)
            {
                db.ShoppingCarts.Add(shoppingcart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shoppingcart);
        }

        //
        // GET: /ShoppingCart/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ShoppingCart shoppingcart = db.ShoppingCarts.Find(id);
            if (shoppingcart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingcart);
        }

        //
        // POST: /ShoppingCart/Edit/5

        [HttpPost]
        public ActionResult Edit(ShoppingCart shoppingcart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingcart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shoppingcart);
        }

        //
        // GET: /ShoppingCart/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ShoppingCart shoppingcart = db.ShoppingCarts.Find(id);
            if (shoppingcart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingcart);
        }

        //
        // POST: /ShoppingCart/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingcart = db.ShoppingCarts.Find(id);
            db.ShoppingCarts.Remove(shoppingcart);
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