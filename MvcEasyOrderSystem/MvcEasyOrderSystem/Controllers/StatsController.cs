using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;
using MvcEasyOrderSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace MvcEasyOrderSystem.Controllers
{
    /// <summary>
    /// 用來做報表分析的部份。因為時間有限所以只有做一個。
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class StatsController : Controller
    {
        private IGenericRepository<Meal> mealRepo;
        private IGenericRepository<Supplier> supplierRepo;
        private IGenericRepository<Category> categoryRepo;
        private IGenericRepository<OrderDetial> orderDetailRepo;


        public StatsController(IGenericRepository<Meal> inMeal,
            IGenericRepository<Supplier> inSupplier, IGenericRepository<Category> inCat,
            IGenericRepository<OrderDetial> inOrder)
        {
            mealRepo = inMeal;
            supplierRepo = inSupplier;
            categoryRepo = inCat;
            orderDetailRepo = inOrder;
        }

        public StatsController()
            : this(new GenericRepository<Meal>(), new GenericRepository<Supplier>(),
            new GenericRepository<Category>(), new GenericRepository<OrderDetial>())
        {
        }
        //
        // GET: /Stats/

        public ActionResult Index()
        {

            var gQuery = (from m in orderDetailRepo.GetWithFilterAndOrder()
                          group m by m.Meal.MealName into g
                          orderby g.Sum(item => item.Quantity * 1) descending
                          select new Group<string, int> { Key = g.Key, Value = g.Sum(item => item.Quantity * 1) })
                        .Take(5);

            return View(gQuery.ToList());
        }

        public FileResult GetSaleRankChart()
        {
            var gQuery = (from m in orderDetailRepo.GetWithFilterAndOrder()
                         group m by m.Meal.MealName into g
                          orderby g.Sum(item => item.Quantity * 1) descending
                         select new Group<string, int> { Key = g.Key, Value = g.Sum(item=> item.Quantity * 1) })
            .Take(5);

            var chart = new Chart();
            chart.ChartAreas.Add(new ChartArea("Default"));
            chart.Width = 500;
            chart.Height = 400;
            chart.ChartAreas["Default"].Area3DStyle.Enable3D = true;
            chart.ChartAreas["Default"].Area3DStyle.Inclination = 15;
            chart.ChartAreas["Default"].Area3DStyle.Rotation = 15;
            chart.Series.Add(new Series("Data"));

            foreach (var item in gQuery)
            {
                chart.Series["Data"].Points.AddXY(item.Key, item.Value);
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            return File(ms, "image/png");
           

        }

    }
}
