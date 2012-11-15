using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;
using MvcEasyOrderSystem.ViewModels;
using System.Threading;


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

        [ChildActionOnly]
        public PartialViewResult CategoryWithCountMenu()
        {
            var query = categoryRepo.GetWithFilterAndOrder();

            var group = from m in query
                        group m by m.CategoryName into g
                        orderby g.Single().CategoryId ascending
                        select new Group<string, int> { Key = g.Key, Id= g.Single().CategoryId, Value = g.Sum(item => item.Meal.Count()) };

            return PartialView("_CategoryWithCountMenu", group.ToList());
        }

        
        public ActionResult AutoComplete(string term)
        {
            var mealName = mealRepo.GetWithFilterAndOrder(x=>x.MealName.Contains(term))
                .Take(10).Select(x=> new {label = x.MealName});

            return Json(mealName, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /BrowseByMeal/

        public ActionResult Index()
        {
            //var meal = db.Meal.Include(m => m.Category).Include(m => m.Supplier);
            var meal = mealRepo.GetWithFilterAndOrder(includeProperties: "Category, Supplier");

            IEnumerable<System.Linq.IGrouping<string, Meal>> group = from m in meal
                        group m by m.Category.CategoryName;

            ViewBag.Header = "所有菜單";
                        
            return View(group);
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

            IEnumerable<System.Linq.IGrouping<string, Meal>> group = from m in mealList
                                                                     group m by m.Category.CategoryName;
            ViewBag.Header = group.FirstOrDefault().Key;

            return View("Index", group);
        }

        
        public PartialViewResult SearchByMealName(string q)
        {
            //TODO: Must be deleted when in actual use
            Thread.Sleep(2000);

            var meal = mealRepo.GetWithFilterAndOrder(x => x.MealName.Contains(q), includeProperties: "Category");
            var group = from m in meal
                        group m by m.Category.CategoryName;

            return PartialView("_GroupOfMeal", group);
        }

        protected override void Dispose(bool disposing)
        {
            mealRepo.Dispose();
            base.Dispose(disposing);
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

    }
}