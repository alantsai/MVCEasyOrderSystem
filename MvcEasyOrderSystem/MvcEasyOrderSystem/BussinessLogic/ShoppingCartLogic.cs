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
    /// <summary>
    /// 提供一些購物車相關動作。
    /// 此class使用GetShoppingCart()來實例(create instance)自己。
    /// </summary>
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

        /// <summary>
        /// 如果使用者已經登陸，則得到使用者的登錄名稱(Name 同時在我的DB裏面作為Customer Table
        /// 的PK)，要不然使用Guid來給與一個暫時使用者Id。
        /// </summary>
        /// <param name="context">得到目前Request</param>
        /// <returns>使用者Id，對應Customer Table PK</returns>
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


        /// <summary>
        /// 靜態方法，用來實例ShoppingCartLogic
        /// </summary>
        /// <param name="inContext">目前Context</param>
        /// <returns></returns>
        public static ShoppingCartLogic GetShoppingCart(HttpContextBase inContext)
        {
            ShoppingCartLogic cart = new ShoppingCartLogic();
            cart.UserId = cart.GetUserId(inContext);
            return cart;
        }

        /// <summary>
        /// 得到目前所有這使用者所放入購物車的物品
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ShoppingCart> GetShoppingCartItems()
        {
            var result = shoppingCartRepo.GetWithFilterAndOrder(x => x.UserId == UserId);
            return result;
        }

        /// <summary>
        /// 得到購物車所有物品的加總
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public decimal GetShoppingCartTotalPrice(IEnumerable<ShoppingCart> items)
        {
            decimal totalPrice = 0;

            foreach (var item in items)
            {
                totalPrice = totalPrice + item.FullPrice;
            }

            return totalPrice;
        }

        /// <summary>
        /// 把一個餐加入到購物車，如果已經存在，則是把數量加1
        /// </summary>
        /// <param name="mealId">Meal Table 的PK</param>
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

        /// <summary>
        /// 刪除購物車裏面其中一筆資料
        /// </summary>
        /// <param name="shoppingCartId"></param>
        public void RemoveFromCart(int shoppingCartId)
        {
            ShoppingCart result = GetShoppingCartUsingShoppingCartId(shoppingCartId);

            shoppingCartRepo.Delete(result);


        }

        /// <summary>
        /// 清空整個購物車
        /// </summary>
        public void EmptyCart()
        {
            var cartItems = shoppingCartRepo.GetWithFilterAndOrder(x => x.UserId == UserId);

            foreach (var item in cartItems)
            {
                shoppingCartRepo.Delete(item);
            }

            shoppingCartRepo.SaveChanges();

        }

        /// <summary>
        /// 得到某一個指定的購物車
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <returns></returns>
        public ShoppingCart GetShoppingCartUsingShoppingCartId(int shoppingCartId)
        {
            return (shoppingCartRepo.GetSingleEntity
                 (x => x.ShoppingCartId == shoppingCartId));
        }

        /// <summary>
        /// 把購物車裏面每一筆資料轉換成對應的OrderDetail，用來做購買完成下定單的動作。
        /// </summary>
        /// <param name="order">主要爲了Order的Id，這樣OrderDetail才知道對應那個</param>
        /// <returns>傳回這一筆Order的總金額</returns>
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
        
        /// <summary>
        /// 當使用著登陸以後，需要把本來臨時給予的假UserId（使用Guid達到）轉換成為真實的
        /// UserId。
        /// </summary>
        /// <param name="inUserId">傳入真實UserId</param>
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

        /// <summary>
        /// 得到目前購物車總數
        /// </summary>
        /// <returns></returns>
        public int GetShoppingCartCount()
        {
            return shoppingCartRepo.GetWithFilterAndOrder(x => x.UserId == UserId).Count();
        }

    }
}