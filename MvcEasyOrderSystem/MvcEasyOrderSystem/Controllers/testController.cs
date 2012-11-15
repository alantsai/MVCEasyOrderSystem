using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.ViewModels;
using System.IO;

namespace MvcEasyOrderSystem.Controllers
{
    public class testController : Controller
    {
        private EOSystemContex db = new EOSystemContex();

        //
        // GET: /test/

        public ActionResult Index()
        {
            var meal = db.Meal.Include(m => m.Category).Include(m => m.Supplier);
            return View(meal.ToList());
        }

        //
        // GET: /test/Details/5

        public ActionResult Details(int id = 0)
        {
            Meal meal = db.Meal.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }

        //
        // GET: /test/Create

        public ActionResult Create()
        {
            var viewModel = new CreateMealViewModel()
            {
                 Categories = db.Category
            };

            
            return View(viewModel);
        }

        //
        // POST: /test/Create

        [HttpPost]
        public ActionResult Create(CreateMealViewModel viewModel)
        {
            HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;

            if (hpf == null || hpf.ContentLength == 0)
            {
                ModelState.AddModelError("", "並未選取圖片");
                //viewModel.Categories = db.Category;
                //return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                Meal meal = new Meal()
                    {
                         CategoryId = viewModel.CategoryId,
                         MealName = viewModel.MealName,
                         Price = viewModel.Price,
                        SupplierId = 1
                    };

                
                {
                    //string savedFileName = Path.GetFileName(hpf.FileName);
                    
                    string savedFileName = Guid.NewGuid().ToString() + "." + (hpf.FileName.Split('.')).Last();
                    string path = Path.Combine(Server.MapPath("~/content/img"), savedFileName);

                    hpf.SaveAs(path);

                    meal.Image = savedFileName;
                }
                
                db.Meal.Add(meal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.Categories = db.Category;
            return View(viewModel);
        }

        //
        // GET: /test/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Meal meal = db.Meal.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", meal.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Supplier, "SupplierId", "CompanyName", meal.SupplierId);
            return View(meal);
        }

        //
        // POST: /test/Edit/5

        [HttpPost]
        public ActionResult Edit(Meal meal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", meal.CategoryId);
            ViewBag.SupplierId = new SelectList(db.Supplier, "SupplierId", "CompanyName", meal.SupplierId);
            return View(meal);
        }

        //
        // GET: /test/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Meal meal = db.Meal.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            return View(meal);
        }

        //
        // POST: /test/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Meal meal = db.Meal.Find(id);
            db.Meal.Remove(meal);
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