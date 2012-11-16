using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;
using MvcEasyOrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcEasyOrderSystem.Controllers
{
    public class HomeController : Controller
    { private IGenericRepository<Meal> mealRepo;
        private IGenericRepository<Supplier> supplierRepo;
        private IGenericRepository<Category> categoryRepo;


        public HomeController(IGenericRepository<Meal> inMeal,
            IGenericRepository<Supplier> inSupplier, IGenericRepository<Category> inCat)
        {
            mealRepo = inMeal;
            supplierRepo = inSupplier;
            categoryRepo = inCat;
        }

        public HomeController()
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
            var meal = mealRepo.GetWithFilterAndOrder(includeProperties: "Category, Supplier");

            IEnumerable<System.Linq.IGrouping<string, Meal>> group = from m in meal
                        group m by m.Category.CategoryName;

            ViewBag.Header = "所有菜單";
                        
            return View(group);
        }

        public ActionResult ShowByCategory(int categoryId = 0)
        {
            var mealList = mealRepo.GetWithFilterAndOrder(meal => meal.CategoryId == categoryId,
                null, string.Empty);

            IEnumerable<System.Linq.IGrouping<string, Meal>> group = from m in mealList
                                                                     group m by m.Category.CategoryName;
            if (group.Count() == 0)
            {
                return RedirectToAction("Index");
            }

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

    }
}
