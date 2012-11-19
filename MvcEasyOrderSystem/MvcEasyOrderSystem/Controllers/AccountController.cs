using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using MvcEasyOrderSystem.Filters;
using MvcEasyOrderSystem.Models;
using MvcEasyOrderSystem.Models.Repositry;
using MvcEasyOrderSystem.BussinessLogic;
using System.Net.Mail;
using System.Net;
using MvcEasyOrderSystem.ViewModels;
using System.Configuration;

namespace MvcEasyOrderSystem.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        IGenericRepository<Customer> customerRepo;

        public AccountController(IGenericRepository<Customer> inCutomerRepo)
        {
            customerRepo = inCutomerRepo;
        }

        public AccountController()
            : this(new GenericRepository<Customer>())
        {
        }

        /// <summary>
        /// 用來寄email，目前使用Google smtp然後帳號和server都存在webconfig
        /// </summary>
        /// <param name="desAddress">寄去的地址</param>
        /// <param name="title">標題</param>
        /// <param name="body">內容</param>
        public void SentEmail(string desAddress, string title, string body)
        {
            string smtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString();
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"].ToString());

            string email = ConfigurationManager.AppSettings["SystemEmail"].ToString();
            string password = ConfigurationManager.AppSettings["EmailPassword"].ToString();

            var client = new SmtpClient(smtpServer, smtpPort)
              {
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
              };
            client.Send(email, desAddress,title, body);
        }

        /// <summary>
        /// 把購物車UserId指向真正的UserId
        /// 在登入的時候執行
        /// </summary>
        /// <param name="userId">真正UserId</param>
        public void MigrateShoppingCart(string userId)
        {
            var cart = ShoppingCartLogic.GetShoppingCart(this.HttpContext);
            cart.MigrateShoppingCartUserIdToUserId(userId);
            Session[ConfigurationManager.AppSettings["UserIdSession"]] = userId;
        }

        #region ForgetPassword

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //TODO：view bug on redirect
        /// <summary>
        /// 如果帳號和email都對，就寄一封email含有ResetPasswordToken的信到使用者註冊的信箱
        /// 最後轉址到輸入Token和新密碼的部份，不過會壞掉。但是如果手動到ResetPassword這一個
        /// Action則不會。
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = viewModel.UserName;

                var customer = customerRepo.GetSingleEntity(x => x.UserId == userId);
                if (customer != null)
                {
                    string url = "http://" + Request.Url.Host + "/Account/ResetPassword";
                    string title = "密碼重設";
                    string body = "請使用以下Token來重設密碼:\n" +
                        WebSecurity.GeneratePasswordResetToken(viewModel.UserName);

                    SentEmail(customer.Email, title, body);

                    return RedirectToAction("ResetPassword");
                }

            }

            ModelState.AddModelError("", "所提供的使用者名稱和email不符。");

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string resetToken = "")
        {
            return View(resetToken);
        }

        //TODO: Bug when given wrong token
        /// <summary>
        /// 用來重設密碼
        /// </summary>
        /// <param name="resetToken"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(string resetToken, string password)
        {
            bool result = WebSecurity.ResetPassword(resetToken, password);

            if (result == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "查無此Token或已經過期");

            return View(resetToken);
        }

        #endregion


        public ActionResult EditUserInfo()
        {

            Customer customer = customerRepo.GetSingleEntity(x => x.UserId == User.Identity.Name);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5
        /// <summary>
        /// 修改使用者相關訊息，對應Customer Table
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserInfo(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customerRepo.Update(customer);
                customerRepo.SaveChanges();
                ViewBag.Message = "修改成功";
            }
            return View(customer);
        }


        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        /// <summary>
        /// Simplemembership自帶的，不過加上每一次登入需要做MigrateShoppingCart的動作。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                MigrateShoppingCart(model.UserName);
                return RedirectToLocal(returnUrl);
            }

            // 如果執行到這裡，發生某項失敗，則重新顯示表單
            ModelState.AddModelError("", "所提供的使用者名稱或密碼不正確。");
            return View(model);
        }

        //
        // POST: /Account/LogOff


        public ActionResult LogOff()
        {
            Session[ConfigurationManager.AppSettings["UserIdSession"]] = null;
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        /// <summary>
        /// 增加了需要輸入相關訊息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // 嘗試註冊使用者
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);

                    MigrateShoppingCart(model.UserName);

                    Roles.AddUserToRole(model.UserName, "User");


                    model.Customer.UserId = model.UserName;
                    customerRepo.Insert(model.Customer);
                    customerRepo.SaveChanges();


                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // 如果執行到這裡，發生某項失敗，則重新顯示表單
            return View(model);
        }


        
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "您的密碼已變更。"
                : message == ManageMessageId.SetPasswordSuccess ? "已設定您的密碼。"
                : message == ManageMessageId.RemoveLoginSuccess ? "已移除外部登入。"
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // 在特定失敗狀況下，ChangePassword 會擲回例外狀況，而非傳回 false。
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "目前密碼不正確或是新密碼無效。");
                    }
                }
            }
            else
            {
                // 使用者沒有本機密碼，因此，請移除遺漏
                // OldPassword 欄位所導致的任何驗證錯誤
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // 如果執行到這裡，發生某項失敗，則重新顯示表單
            return View(model);
        }



        #region Helper
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 請參閱 http://go.microsoft.com/fwlink/?LinkID=177550 了解
            // 狀態碼的完整清單。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "使用者名稱已經存在。請輸入不同的使用者名稱。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "該電子郵件地址的使用者名稱已經存在。請輸入不同的電子郵件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "所提供的密碼無效。請輸入有效的密碼值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "所提供的電子郵件地址無效。請檢查這項值，然後再試一次。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "所提供的密碼擷取解答無效。請檢查這項值，然後再試一次。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "所提供的密碼擷取問題無效。請檢查這項值，然後再試一次。";

                case MembershipCreateStatus.InvalidUserName:
                    return "所提供的使用者名稱無效。請檢查這項值，然後再試一次。";

                case MembershipCreateStatus.ProviderError:
                    return "驗證提供者傳回錯誤。請確認您的輸入，然後再試一次。如果問題仍然存在，請聯繫您的系統管理員。";

                case MembershipCreateStatus.UserRejected:
                    return "使用者建立要求已取消。請確認您的輸入，然後再試一次。如果問題仍然存在，請聯繫您的系統管理員。";

                default:
                    return "發生未知的錯誤。請確認您的輸入，然後再試一次。如果問題仍然存在，請聯繫您的系統管理員。";
            }
        }
        #endregion
    }
}
