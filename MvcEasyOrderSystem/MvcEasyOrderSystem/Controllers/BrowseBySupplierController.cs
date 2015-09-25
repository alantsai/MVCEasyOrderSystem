using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;

namespace MvcEasyOrderSystem.Controllers
{
    /// <summary>
    /// 這一個Controller沒有被用到，因為目前只有一家Supplier，如果有多家，則需要能夠呈現
    /// 某一家的相關菜單和資訊，例如使用google map來表示地址。
    /// </summary>
    [Authorize(Roles="NoOne")]
    public class BrowseBySupplierController : Controller
    {
        private EOSystemContex db = new EOSystemContex();

        private IGenericRepository<Meal> mealRepo;
        private IGenericRepository<Supplier> supplierRepo;
        private IGenericRepository<Category> categoryRepo;

        public BrowseBySupplierController(IGenericRepository<Meal> inMeal,
    IGenericRepository<Supplier> inSupplier, IGenericRepository<Category> inCat)
        {
            mealRepo = inMeal;
            supplierRepo = inSupplier;
            categoryRepo = inCat;
        }

        public BrowseBySupplierController()
            : this(new GenericRepository<Meal>(), new GenericRepository<Supplier>(),
            new GenericRepository<Category>())
        {
        }

        //
        // GET: /BrowseBySupplier/

        public ActionResult Index()
        {
            return View(db.Supplier.ToList());
        }

        //
        // GET: /BrowseBySupplier/Details/5

        public ActionResult Details(int id = 0)
        {
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // GET: /BrowseBySupplier/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BrowseBySupplier/Create

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Supplier.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        //
        // GET: /BrowseBySupplier/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /BrowseBySupplier/Edit/5

        [HttpPost]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        //
        // GET: /BrowseBySupplier/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Supplier supplier = db.Supplier.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        //
        // POST: /BrowseBySupplier/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Supplier.Find(id);
            db.Supplier.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowBySupplier(int supplierId = 0)
        {
            var mealList = mealRepo.GetWithFilterAndOrder(meal => meal.SupplierId == supplierId,
                null, string.Empty);
            return View(mealList);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
