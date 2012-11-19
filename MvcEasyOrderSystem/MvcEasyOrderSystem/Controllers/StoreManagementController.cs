using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;
using MvcEasyOrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcEasyOrderSystem.Controllers
{
    /// <summary>
    /// 「管理者」用來管理菜單只用
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class StoreManagementController : Controller
    {
        private IGenericRepository<Category> categoryRepo;
        private IGenericRepository<Meal> mealRepo;

        public StoreManagementController(IGenericRepository<Category> inCategoryRepo,
            IGenericRepository<Meal> inMealRepo)
        {
            categoryRepo = inCategoryRepo;
            mealRepo = inMealRepo;
        }

        public StoreManagementController()
            : this(new GenericRepository<Category>(),
            new GenericRepository<Meal>())
        {
        }


        //
        // GET: /StoreManagement/

        public ActionResult Index()
        {
            var meal = mealRepo.GetWithFilterAndOrder(includeProperties: "Category");
            //var meal = db.Meal.Include(m => m.Category).Include(m => m.Supplier);
            return View(meal.ToList());
        }


        public ActionResult Create()
        {
            var viewModel = new CreateMealViewModel()
            {
                Categories = categoryRepo.GetWithFilterAndOrder()
            };


            return View(viewModel);
        }



        [HttpPost]
        public ActionResult Create(CreateMealViewModel viewModel)
        {
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;

            if (hpf == null || hpf.ContentLength == 0)
            {
                ModelState.AddModelError("", "並未選取圖片");
            }

            if (ModelState.IsValid)
            {
                Meal meal = new Meal()
                {
                    CategoryId = viewModel.CategoryId,
                    MealName = viewModel.MealName,
                    Price = viewModel.Price,
                    //TODO: For now only one supplier
                    SupplierId = 1
                };
                {
                    string savedFileName = Guid.NewGuid().ToString() + "." + (hpf.FileName.Split('.')).Last();
                    string path = Path.Combine(Server.MapPath("~/Content/ProductImg"), savedFileName);

                    hpf.SaveAs(path);

                    meal.Image = savedFileName;
                }

                mealRepo.Insert(meal);
                mealRepo.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Categories = categoryRepo.GetWithFilterAndOrder();
            return View(viewModel);
        }

        //
        // GET: /test/Details/5

        public ActionResult Details(int id = 0)
        {
            Meal meal = mealRepo.GetSingleEntity(x => x.MealId == id);

            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }


        public ActionResult Edit(int id = 0)
        {
            Meal meal = mealRepo.GetSingleEntity(x => x.MealId == id);

            var viewModel = new CreateMealViewModel()
            {
                Categories = categoryRepo.GetWithFilterAndOrder(),
                MealName = meal.MealName,
                CategoryId = meal.CategoryId,
                Price = meal.Price,
                Image = meal.Image,
                MealId = meal.MealId
            };
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult Edit(CreateMealViewModel viewModel)
        {
            

            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;



            if (ModelState.IsValid)
            {
                Meal meal = mealRepo.GetSingleEntity(x => x.MealId == viewModel.MealId);

                meal.CategoryId = viewModel.CategoryId;
                meal.MealName = viewModel.MealName;
                meal.Price = viewModel.Price;

                if (hpf != null && hpf.ContentLength != 0)
                {
                    string savedFileName = Guid.NewGuid().ToString() + "." + (hpf.FileName.Split('.')).Last();
                    string path = Path.Combine(Server.MapPath("~/Content/ProductImg"), savedFileName);

                    hpf.SaveAs(path);

                    meal.Image = savedFileName;
                }

                mealRepo.Update(meal);
                mealRepo.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Categories = categoryRepo.GetWithFilterAndOrder();

            return View(viewModel);
        }

    }
}
