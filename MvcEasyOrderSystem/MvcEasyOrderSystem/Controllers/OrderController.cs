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
using WebMatrix.WebData;
using System.Web.Security;
using System.Threading;

namespace MvcEasyOrderSystem.Controllers
{
    /// <summary>
    /// 訂單相關
    /// </summary>
    [Authorize(Roles="User, Admin")]
    public class OrderController : Controller
    {
        private IGenericRepository<Order> orderRepo;

        private IGenericRepository<ShoppingCart> shoppingCartRepo;
        private ShoppingCartLogic shoppingCartLogic;
        private IGenericRepository<Customer> customerRepo;
        private IGenericRepository<PaymentMethod> paymentMethodRepo;
        private IGenericRepository<CollectionMethod> collectionRepo;
        private IGenericRepository<Status> statusRepo;

        public OrderController(IGenericRepository<Order> inOrderRepo,
            IGenericRepository<ShoppingCart> inShoppingCartRepo,
            IGenericRepository<Customer> inCustomerRepo,
            IGenericRepository<PaymentMethod> inPayment,
            IGenericRepository<CollectionMethod> inCollection,
            IGenericRepository<Status> inStatus
            )
        {
            orderRepo = inOrderRepo;
            customerRepo = inCustomerRepo;
            shoppingCartRepo = inShoppingCartRepo;
            paymentMethodRepo = inPayment;
            collectionRepo = inCollection;
            statusRepo = inStatus;
        }

        public OrderController()
            : this(new GenericRepository<Order>(), new GenericRepository<ShoppingCart>(),
            new GenericRepository<Customer>(), new GenericRepository<PaymentMethod>(),
            new GenericRepository<CollectionMethod>(), new GenericRepository<Status>())
        {
        }

        //private EOSystemContex db = new EOSystemContex();


        public ActionResult ProcedToCheckout()
        {
            var viewModel = new CreateOrderViewModel()
                {
                    RequireDateTime = DateTime.Now,
                    PaymentMethods = paymentMethodRepo.GetWithFilterAndOrder(),
                    CollectionMethods = collectionRepo.GetWithFilterAndOrder()
                };

            return View(viewModel);
        }

        /// <summary>
        /// 購物車選擇購買以後執行的地方
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
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


            viewModel.PaymentMethods = paymentMethodRepo.GetWithFilterAndOrder();
            viewModel.CollectionMethods = collectionRepo.GetWithFilterAndOrder();

            return View(viewModel);
        }

        /// <summary>
        /// 把ProcedToCheckout()傳回來的CreateOrderViewModel轉成對應的Order
        /// 因為Order可能是「送餐到府」也有可能是「來店取餐」，所以需要做到判斷
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 把Order對應的OrderDetail轉為ShoppingCartViewModel。
        /// 主要是當對一個訂單點選「詳細」，裏面OrderDetail其實是使用購物車出現的那種模型
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 以Ajax的方式調用。在「訂單管理」點下「詳細」的時候調用
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public PartialViewResult OrderDetail(int orderId)
        {
            //TODO: Delete
            Thread.Sleep(1000);

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
            ViewBag.Title = "所有訂單";

            if (Roles.IsUserInRole("Admin"))
            {
                order = orderRepo.GetWithFilterAndOrder(
                includeProperties: "CollectionMethod, Customer, PaymentMethod, Status");
                ViewBag.ShowEdit = true;
            }

            return View(order.ToList());
        }

        /// <summary>
        /// 提供「訂單記錄」旁邊「訂單分類」點下去的後端工作。
        /// 目前「使用者」和「管理者」皆是使用這個方法，唯一差別是「管理著」可以看到全部
        /// 訂單，和更新的動作（使用ViewBage達到）。感覺不是很對的做法，不過因為時間關係。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowByCollectionMethod(int id = 0)
        {
            var order = orderRepo.GetWithFilterAndOrder((x => x.UserId == User.Identity.Name
                && x.CollectionMethodId == id), includeProperties: "CollectionMethod");

            if (Roles.IsUserInRole("Admin"))
            {
                order = orderRepo.GetWithFilterAndOrder(x => x.CollectionMethodId == id);
                ViewBag.ShowEdit = true;
            }

            if (order.Count() != 0)
            {

                ViewBag.Title =string.Format("收貨方式({0})清單",order.FirstOrDefault().CollectionMethod.CollectionMethodName);
            }

            return View("Index", order);
        }

        /// <summary>
        /// 和ShowByCollectionMethod()一樣的邏輯，不過這次是爲了「狀態分類」
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowByStatus(int id = 0)
        {
            var order = orderRepo.GetWithFilterAndOrder((x => x.UserId == User.Identity.Name
                && x.StatusId == id), includeProperties: "Status");

            if (Roles.IsUserInRole("Admin"))
            {
                order = orderRepo.GetWithFilterAndOrder(x => x.StatusId == id);
                ViewBag.ShowEdit = true;
            }

            if (order.Count() != 0)
            {

                ViewBag.Title = string.Format("訂單狀況({0})清單", order.FirstOrDefault().Status.StatusName);
            }

            return View("Index", order);
        }

        /// <summary>
        /// 製作「訂單分類」清單。
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult CollectionMethodOrderMenu()
        {
            var query = orderRepo.GetWithFilterAndOrder((x=> x.UserId == User.Identity.Name));

            if (Roles.IsUserInRole("Admin"))
            {
                query = orderRepo.GetWithFilterAndOrder();
                ViewBag.ShowEdit = true;
            }

            var group = from m in query
                        group m by m.CollectionMethod.CollectionMethodName into g
                        
                        select new Group<string, int> { Key = g.Key, Id = g.FirstOrDefault().CollectionMethodId, Value = g.Count()};

            ViewData["Controller"] = "ShowByCollectionMethod";
            return PartialView("_ShowMenu", group.ToList());
        }

        /// <summary>
        /// 製作「狀態分類」清單
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult StatusOrderMenu()
        {
            
            var query = orderRepo.GetWithFilterAndOrder(x => x.UserId == User.Identity.Name);

            if (Roles.IsUserInRole("Admin"))
            {
                query = orderRepo.GetWithFilterAndOrder();
                ViewBag.ShowEdit = true;
            }

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
            Order order = orderRepo.GetSingleEntity(x => x.OrderId == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

      

        //
        // GET: /Checkout/Edit/5
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int id = 0)
        {
            Order order = orderRepo.GetSingleEntity(x=> x.OrderId == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var viewModel = new UpdateOrderViewModel();

            viewModel.Status = statusRepo.GetWithFilterAndOrder();
            viewModel.Order = order;
            viewModel.StatusId = order.StatusId;

            return View(viewModel);
        }

        //
        // POST: /Checkout/Edit/5

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(UpdateOrderViewModel viewModel)
        {
            Order order = null;
            if (ModelState.IsValid)
            {
                order = orderRepo.GetSingleEntity(x => x.OrderId == viewModel.Order.OrderId);

                order.StatusId = viewModel.StatusId;
                order.Comment = viewModel.Order.Comment;

                orderRepo.Update(order);
                orderRepo.SaveChanges();

                return RedirectToAction("Index");
            }

            viewModel.Status = statusRepo.GetWithFilterAndOrder();
            viewModel.Order = order;
           
            return View(viewModel);
        }

       

        protected override void Dispose(bool disposing)
        {
            orderRepo.Dispose();
            customerRepo.Dispose();
            shoppingCartRepo.Dispose();
            paymentMethodRepo.Dispose();
            collectionRepo.Dispose();
            statusRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}
