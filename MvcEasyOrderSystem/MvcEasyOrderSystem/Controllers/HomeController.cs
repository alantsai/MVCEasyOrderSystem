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
    /// <summary>
    /// 主要用來顯示菜單相關
    /// </summary>
    public class HomeController : Controller
    {
        private IGenericRepository<Meal> mealRepo;
        private IGenericRepository<Supplier> supplierRepo;
        private IGenericRepository<Category> categoryRepo;
        private IGenericRepository<OrderDetial> orderDetailRepo;


        public HomeController(IGenericRepository<Meal> inMeal,
            IGenericRepository<Supplier> inSupplier, IGenericRepository<Category> inCat,
            IGenericRepository<OrderDetial> inOrder)
        {
            mealRepo = inMeal;
            supplierRepo = inSupplier;
            categoryRepo = inCat;
            orderDetailRepo = inOrder;
        }

        public HomeController()
            : this(new GenericRepository<Meal>(), new GenericRepository<Supplier>(),
            new GenericRepository<Category>(), new GenericRepository<OrderDetial>())
        {
        }

        /// <summary>
        /// 分類Menu顯示所有菜的分類和每一分類所擁有的菜
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult CategoryWithCountMenu()
        {
            var query = categoryRepo.GetWithFilterAndOrder();


            var group = from m in query
                        group m by m.CategoryName into g
                        orderby g.Single().CategoryId ascending
                        select new Group<string, int> { Key = g.Key, Id = g.Single().CategoryId, Value = g.Sum(item => item.Meal.Count()) };

            return PartialView("_CategoryWithCountMenu", group.ToList());
        }

        /// <summary>
        /// 取得銷路最好的10道菜組成一個menu list，點擊及把那道菜加入購物車
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult Top10BestSale()
        {

            var query = orderDetailRepo.GetWithFilterAndOrder();


            var group = (from m in query
                        group m by m.Meal.MealName into g
                        orderby g.Count() descending
                        select new Group<string, int> { Key = g.Key , Id = g.First().Meal.MealId })
                        .Take(10);

            return PartialView("_Top10BestSale", group.ToList());
        }

        /// <summary>
        /// 搭配JQuery UI的Autocomplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult AutoComplete(string term)
        {
            var mealName = mealRepo.GetWithFilterAndOrder(x => x.MealName.Contains(term))
                .Take(10).Select(x => new { label = x.MealName });

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

        /// <summary>
        /// CategoryWithCountMenu()的連接會執行一下Action。如果找到類別裏面的餐多於0件，則使用
        /// Index的view來顯示，如果找不到餐則回傳使用Index Action。
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 搜索功能，搭配Ajax調用
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
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
