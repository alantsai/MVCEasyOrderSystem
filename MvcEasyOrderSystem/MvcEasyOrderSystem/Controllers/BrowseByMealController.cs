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
    public class BrowseByMealController : Controller
    {
        //private EOSystemContex db = new EOSystemContex();

        private IGenericRepository<Meal> mealRepo;
        private IGenericRepository<Supplier> supplierRepo;
        private IGenericRepository<Category> categoryRepo;


        public BrowseByMealController(IGenericRepository<Meal> inMeal,
            IGenericRepository<Supplier> inSupplier, IGenericRepository<Category> inCat)
        {
            mealRepo = inMeal;
            supplierRepo = inSupplier;
            categoryRepo = inCat;
        }

        public BrowseByMealController()
            :this(new GenericRepository<Meal>(), new GenericRepository<Supplier>(),
            new GenericRepository<Category>())
        {
        }

        //
        // GET: /BrowseByMeal/

        public ActionResult Index()
        {
            //var meal = db.Meal.Include(m => m.Category).Include(m => m.Supplier);
            var meal = mealRepo.GetWithFilterAndOrder(includeProperties: "Category, Supplier");
            return View(meal.ToList());
        }

        //
        // GET: /BrowseByMeal/Details/5

        public ActionResult Details(int id = 0)
        {
            //Meal meal = db.Meal.Find(id);
            Meal meal = mealRepo.GetSingleEntity(x => x.MealId == id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }

        public ActionResult ShowByCategory(int categoryId = 0)
        {
            var mealList = mealRepo.GetWithFilterAndOrder(meal => meal.CategoryId == categoryId,
                null, string.Empty);
            return View(mealList);
        }

        ////
        //// GET: /BrowseByMeal/Create

        //public ActionResult Create()
        //{
        //    ViewBag.CategoryId = new SelectList(categoryRepo.GetWithFilterAndOrder(), "CategoryId", "CategoryName");
        //    ViewBag.SupplierId = new SelectList(supplierRepo.GetWithFilterAndOrder(), "SupplierId", "CompanyName");
        //    return View();
        //}

        ////
        //// POST: /BrowseByMeal/Create

        //[HttpPost]
        //public ActionResult Create(Meal meal)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        mealRepo.Insert(meal);
        //        mealRepo.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CategoryId = new SelectList(categoryRepo.GetWithFilterAndOrder(), "CategoryId", "CategoryName");
        //    ViewBag.SupplierId = new SelectList(supplierRepo.GetWithFilterAndOrder(), "SupplierId", "CompanyName");
        //    return View(meal);
        //}

        ////
        //// GET: /BrowseByMeal/Edit/5

        //public ActionResult Edit(int id = 0)
        //{
        //    Meal meal = mealRepo.GetSingleEntity(x => x.MealId == id);
        //    if (meal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CategoryId = new SelectList(categoryRepo.GetWithFilterAndOrder(), "CategoryId", "CategoryName");
        //    ViewBag.SupplierId = new SelectList(supplierRepo.GetWithFilterAndOrder(), "SupplierId", "CompanyName");
        //    return View(meal);
        //}

        ////
        //// POST: /BrowseByMeal/Edit/5

        //[HttpPost]
        //public ActionResult Edit(Meal meal)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        mealRepo.Update(meal);
        //        mealRepo.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CategoryId = new SelectList(categoryRepo.GetWithFilterAndOrder(), "CategoryId", "CategoryName");
        //    ViewBag.SupplierId = new SelectList(supplierRepo.GetWithFilterAndOrder(), "SupplierId", "CompanyName");
        //    return View(meal);
        //}

        ////
        //// GET: /BrowseByMeal/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Meal meal = mealRepo.GetSingleEntity(x => x.MealId == id);
        //    if (meal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(meal);
        //}

        ////
        //// POST: /BrowseByMeal/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Meal meal = mealRepo.GetSingleEntity(x => x.MealId == id);
        //    mealRepo.Delete(meal);
        //    mealRepo.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            mealRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}