using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;
using MvcEasyOrderSystem.BussinessLogic;

namespace MvcEasyOrderSystem.Controllers
{

    public class ShoppingCartController : Controller
    {
        private IGenericRepository<ShoppingCart> shoppingCartRepo;
        private ShoppingCartLogic shoppingCartLogic;

        public ShoppingCartController(IGenericRepository<ShoppingCart> inShoppingCartRepo)
        {
            shoppingCartRepo = inShoppingCartRepo;
        }

        public ShoppingCartController()
            :this(new GenericRepository<ShoppingCart>())
        {
        }


        public ActionResult AddToCart(int mealId)
        {
            shoppingCartLogic = ShoppingCartLogic.GetShoppingCart(this.HttpContext);

            shoppingCartLogic.AddToCart(mealId);
            shoppingCartLogic.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /ShoppingCart/


        public ActionResult Index()
        {
            shoppingCartLogic = ShoppingCartLogic.GetShoppingCart(this.HttpContext);

            var viewModel = new ViewModels.ShoppingCartViewModel ();

            viewModel.CartItems = shoppingCartLogic.GetShoppingCartItems().ToList();
               viewModel.TotalPrice = shoppingCartLogic.GetShoppingCartTotalPrice(viewModel.CartItems);

               ViewBag.ShowEdit = true;
               ViewBag.Title = "購物車內容";

            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/Details/5

        public ActionResult Details(int shoppingCartId = 0)
        {
            ShoppingCart shoppingcart = shoppingCartRepo.GetSingleEntity
                (x => x.ShoppingCartId == shoppingCartId);
            if (shoppingcart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingcart);
        }

        public ActionResult EmptyShoppingCart(string userId)
        {
            //TODO: Make it ajax. All shopping cart can be wrap up into a partial view
            shoppingCartLogic = ShoppingCartLogic.GetShoppingCart(this.HttpContext);

            shoppingCartLogic.EmptyCart();
            return RedirectToAction("Index");
        }

        public PartialViewResult ShoppingCartCount()
        {
            shoppingCartLogic = ShoppingCartLogic.GetShoppingCart(this.HttpContext);

            return PartialView("_ShoppingCartCount", shoppingCartLogic.GetShoppingCartCount());
        }

       
        public ActionResult Delete(int shoppingCartId = 0)
        {
            shoppingCartLogic = ShoppingCartLogic.GetShoppingCart(this.HttpContext);
            ShoppingCart shoppingcart = shoppingCartLogic.GetShoppingCartUsingShoppingCartId(shoppingCartId);

            if (shoppingcart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingcart);
        }

        //
        // POST: /ShoppingCart/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int shoppingCartId)
        {
            shoppingCartLogic = ShoppingCartLogic.GetShoppingCart(this.HttpContext);

            shoppingCartLogic.RemoveFromCart(shoppingCartId);

            shoppingCartLogic.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            shoppingCartRepo.Dispose();
            base.Dispose(disposing);
        }
    }
}