using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.ViewModels;
using System.Configuration;
using MvcEasyOrderSystem.Models.Repositry;
using MvcEasyOrderSystem.BussinessLogic;

namespace MvcEasyOrderSystem.Controllers
{
    [Authorize(Roles="User")]
    public class OrderController : Controller
    {
        private IGenericRepository<Order> orderRepo;

        private IGenericRepository<ShoppingCart> shoppingCartRepo;
        private ShoppingCartLogic shoppingCartLogic;
        private IGenericRepository<Customer> customerRepo;

        public OrderController(IGenericRepository<Order> inOrderRepo,
            IGenericRepository<ShoppingCart> inShoppingCartRepo,
            IGenericRepository<Customer> inCustomerRepo)
        {
            orderRepo = inOrderRepo;
            customerRepo = inCustomerRepo;
            shoppingCartRepo = inShoppingCartRepo;
        }

        public OrderController()
            : this(new GenericRepository<Order>(), new GenericRepository<ShoppingCart>(),
            new GenericRepository<Customer>())
        {
        }

        private EOSystemContex db = new EOSystemContex();


        public ActionResult ProcedToCheckout()
        {
            var viewModel = new CreateOrderViewModel()
                {
                    RequireDateTime = DateTime.Now,
                    PaymentMethods = db.PaymentMethod,
                    CollectionMethods = db.CollectionMethod
                };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ProcedToCheckout(CreateOrderViewModel viewModel)
        {

            var customer = orderRepo.GetSingleEntity(x => x.UserId == User.Identity.Name);

            if (ModelState.IsValid)
            {
                Order order = ConvertViewModelToOrder(viewModel, User.Identity.Name);

                orderRepo.Insert(order);
                orderRepo.SaveChanges();

                shoppingCartLogic = ShoppingCartLogic.GetShoppingCart(this.HttpContext);
               order.TotalPrice = shoppingCartLogic.ShoppingCartToOrderDetails(order);

               orderRepo.Update(order);
               orderRepo.SaveChanges();

                return RedirectToAction("Index");
            }


            viewModel.PaymentMethods = db.PaymentMethod;
            viewModel.CollectionMethods = db.CollectionMethod;

            return View(viewModel);
        }

        private Order ConvertViewModelToOrder(CreateOrderViewModel viewModel, string userId)
        {
            Order order = new Order()
            {
                UserId = userId,
                OrderDateTime = DateTime.Now,
                RequireDateTime = viewModel.RequireDateTime,
                TotalPrice = 0,
                PaymentMethodId = viewModel.PaymentMethodId,
                CollectionMethodId = viewModel.CollectionMethodId,
                Comment = viewModel.Comment

            };
            if ((viewModel.PaymentMethodId == 1) && (viewModel.IsUserRegAddress == false))
            {
                order.Address_AddCity = viewModel.AddCity;
                order.Address_AddDistrict = viewModel.AddDistrict;
                order.Address_AddFull = viewModel.AddFull;
                order.Address_PostCode = viewModel.PostCode;

            }
            else
            {
                Customer customer = customerRepo.GetSingleEntity(x => x.UserId == User.Identity.Name);
                order.Address_AddCity = customer.Address.AddCity;
                order.Address_AddDistrict = customer.Address.AddDistrict;
                order.Address_AddFull = customer.Address.AddFull;
                order.Address_PostCode = customer.Address.PostCode;
            }
            return order;
        }

        public ShoppingCartViewModel PopulateOrderDetails(Order order)
        {
            var orderDetials = order.OrderDetial;

            ShoppingCartViewModel viewModel = new ShoppingCartViewModel()
                {
                     CartItems = new List<ShoppingCart>()
                };

            decimal totalPrice = 0;

            foreach (var item in orderDetials)
            {
                var orderDetail = new ShoppingCart()
                {
                     
                    MealId = item.MealId,
                    MealName = item.Meal.MealName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };

                totalPrice += (orderDetail.UnitPrice * orderDetail.Quantity);
                viewModel.CartItems.Add(orderDetail);
            }

            viewModel.TotalPrice = totalPrice;

            return viewModel;
        }

        public PartialViewResult OrderDetail(int orderId)
        {
            var order = orderRepo.GetWithFilterAndOrder(x => x.OrderId == orderId,
                includeProperties: "OrderDetial");

            var viewModel = PopulateOrderDetails(order.SingleOrDefault());

            return PartialView(viewModel);
        }

        //
        // GET: /Checkout/

        public ActionResult Index()
        {
            var order = orderRepo.GetWithFilterAndOrder((x => x.UserId == User.Identity.Name),
                includeProperties: "CollectionMethod, Customer, PaymentMethod, Status");
            ViewBag.Header = "所有訂單";

            return View(order.ToList());
        }

        public ActionResult ShowByCollectionMethod(int id = 0)
        {
            var order = orderRepo.GetWithFilterAndOrder((x => x.UserId == User.Identity.Name
                && x.CollectionMethodId == id), includeProperties: "CollectionMethod");

            if (order.Count() != 0)
            {

                ViewBag.Header =string.Format("收貨方式({0})清單",order.FirstOrDefault().CollectionMethod.CollectionMethodName);
            }

            return View("Index", order);
        }

        public ActionResult ShowByStatus(int id = 0)
        {
            var order = orderRepo.GetWithFilterAndOrder((x => x.UserId == User.Identity.Name
                && x.StatusId == id), includeProperties: "Status");

            if (order.Count() != 0)
            {

                ViewBag.Header = string.Format("訂單狀況({0})清單", order.FirstOrDefault().Status.StatusName);
            }

            return View("Index", order);
        }

        [ChildActionOnly]
        public PartialViewResult CollectionMethodOrderMenu()
        {
            var query = orderRepo.GetWithFilterAndOrder();

            var group = from m in query
                        group m by m.CollectionMethod.CollectionMethodName into g
                        
                        select new Group<string, int> { Key = g.Key, Id = g.FirstOrDefault().CollectionMethodId, Value = g.Count()};

            ViewData["Controller"] = "ShowByCollectionMethod";
            return PartialView("_ShowMenu", group.ToList());
        }

        [ChildActionOnly]
        public PartialViewResult StatusOrderMenu()
        {
            var query = orderRepo.GetWithFilterAndOrder();

            var group = from m in query
                        group m by m.Status.StatusName into g
                        select new Group<string, int> { Key = g.Key, Id = g.FirstOrDefault().StatusId, Value = g.Count() };
            ViewData["Controller"] = "ShowByStatus";
            return PartialView("_ShowMenu", group.ToList());
        }

        //
        // GET: /Checkout/Details/5

        public ActionResult Details(int id = 0)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /Checkout/Create

        public ActionResult Create()
        {
            ViewBag.CollectionMethodId = new SelectList(db.CollectionMethod, "CollectionMethodId", "CollectionMethodName");
            ViewBag.UserId = new SelectList(db.Customer, "UserId", "FirstName");
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethod, "PaymentMethodId", "PaymentMethodName");
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "StatusName");
            return View();
        }

        //
        // POST: /Checkout/Create

        [HttpPost]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CollectionMethodId = new SelectList(db.CollectionMethod, "CollectionMethodId", "CollectionMethodName", order.CollectionMethodId);
            ViewBag.UserId = new SelectList(db.Customer, "UserId", "FirstName", order.UserId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethod, "PaymentMethodId", "PaymentMethodName", order.PaymentMethodId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "StatusName", order.StatusId);
            return View(order);
        }

        //
        // GET: /Checkout/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CollectionMethodId = new SelectList(db.CollectionMethod, "CollectionMethodId", "CollectionMethodName", order.CollectionMethodId);
            ViewBag.UserId = new SelectList(db.Customer, "UserId", "FirstName", order.UserId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethod, "PaymentMethodId", "PaymentMethodName", order.PaymentMethodId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "StatusName", order.StatusId);
            return View(order);
        }

        //
        // POST: /Checkout/Edit/5

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CollectionMethodId = new SelectList(db.CollectionMethod, "CollectionMethodId", "CollectionMethodName", order.CollectionMethodId);
            ViewBag.UserId = new SelectList(db.Customer, "UserId", "FirstName", order.UserId);
            ViewBag.PaymentMethodId = new SelectList(db.PaymentMethod, "PaymentMethodId", "PaymentMethodName", order.PaymentMethodId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "StatusName", order.StatusId);
            return View(order);
        }

        //
        // GET: /Checkout/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Checkout/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
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