using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;
using System.Web.Mvc;
using System.Configuration;
using WebMatrix.WebData;
using MvcEasyOrderSystem.Controllers;

namespace MvcEasyOrderSystem.BussinessLogic
{
    public class ShoppingCartLogic
    {
        public string UserIdSessionKey = ConfigurationManager.AppSettings["UserIdSession"];
        public string UserId { get; set; }
        private IGenericRepository<ShoppingCart> shoppingCartRepo;
        private IGenericRepository<Meal> mealRepo;
        private IGenericRepository<OrderDetial> orderDetailRepo;

        private ShoppingCartLogic(IGenericRepository<ShoppingCart> inShoppingCartRepo,
            IGenericRepository<Meal> inMealRepo,
            IGenericRepository<OrderDetial> inOrderDetailRepo)
        {
            shoppingCartRepo = inShoppingCartRepo;
            mealRepo = inMealRepo;
            orderDetailRepo = inOrderDetailRepo;
        }

        private ShoppingCartLogic()
            : this(new GenericRepository<ShoppingCart>(), new GenericRepository<Meal>(),
            new GenericRepository<OrderDetial>())
        {
        }


        public string GetUserId(HttpContextBase context)
        {
            if (context.Session[UserIdSessionKey] == null)
            {
                if (string.IsNullOrEmpty(context.User.Identity.Name))
                {
                    context.Session[UserIdSessionKey] = Guid.NewGuid().ToString();
                }
                else
                {
                    
                    context.Session[UserIdSessionKey] =context.User.Identity.Name.ToString();
                }
            }
            return context.Session[UserIdSessionKey].ToString();

        }

        public static ShoppingCartLogic GetShoppingCart(HttpContextBase inContext)
        {
            ShoppingCartLogic cart = new ShoppingCartLogic();
            cart.UserId = cart.GetUserId(inContext);
            return cart;
        }

        public IEnumerable<ShoppingCart> GetShoppingCartItems()
        {
            var result = shoppingCartRepo.GetWithFilterAndOrder(x => x.UserId == UserId);
            return result;
        }

        public decimal GetShoppingCartTotalPrice(IEnumerable<ShoppingCart> items)
        {
            decimal totalPrice = 0;

            foreach (var item in items)
            {
                totalPrice = totalPrice + item.FullPrice;
            }

            return totalPrice;
        }

        public void AddToCart(int mealId)
        {
            ShoppingCart result = shoppingCartRepo.GetSingleEntity(
                x => x.UserId == UserId && x.MealId == mealId);

            if (result == null)
            {
                var mealDetail = mealRepo.GetSingleEntity(x => x.MealId == mealId);
                shoppingCartRepo.Insert(new ShoppingCart
                {
                    MealId = mealDetail.MealId,
                    MealName = mealDetail.MealName,
                    Quantity = 1,
                    UnitPrice = mealDetail.Price,
                    UserId = UserId,
                });

            }
            else
            {
                result.Quantity = result.Quantity + 1;
                shoppingCartRepo.Update(result);
            }

        }

        public void SaveChanges()
        {
            shoppingCartRepo.SaveChanges();
        }

        public void RemoveFromCart(int shoppingCartId)
        {
            ShoppingCart result = GetShoppingCartUsingShoppingCartId(shoppingCartId);

            shoppingCartRepo.Delete(result);


        }

        public void EmptyCart()
        {
            var cartItems = shoppingCartRepo.GetWithFilterAndOrder(x => x.UserId == UserId);

            foreach (var item in cartItems)
            {
                shoppingCartRepo.Delete(item);
            }

            shoppingCartRepo.SaveChanges();

        }

        public ShoppingCart GetShoppingCartUsingShoppingCartId(int shoppingCartId)
        {
            return (shoppingCartRepo.GetSingleEntity
                 (x => x.ShoppingCartId == shoppingCartId));
        }

        //TODO:Added
        public decimal ShoppingCartToOrderDetails(Order order)
        {
            var cartItems = GetShoppingCartItems();

            decimal totalPrice = 0;

            foreach(var item in cartItems)
            {
                var orderDetail = new OrderDetial()
                {
                    MealId = item.MealId,
                    OrderId = order.OrderId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                totalPrice += (orderDetail.UnitPrice * orderDetail.Quantity);
                orderDetailRepo.Insert(orderDetail);
            }

            orderDetailRepo.SaveChanges();
            EmptyCart();

            return totalPrice;
        }
        
        public void MigrateShoppingCartUserIdToUserId(string inUserId)
        {
            var cart = shoppingCartRepo.GetWithFilterAndOrder
                (x => x.UserId == UserId) ;

            foreach (var item in cart)
            {
                item.UserId = inUserId;
            }

            shoppingCartRepo.SaveChanges();
        }


    }
}