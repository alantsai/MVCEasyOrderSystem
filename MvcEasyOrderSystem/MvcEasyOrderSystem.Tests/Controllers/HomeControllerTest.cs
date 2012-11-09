using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcEasyOrderSystem;
using MvcEasyOrderSystem.Controllers;

namespace MvcEasyOrderSystem.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // 排列
            HomeController controller = new HomeController();

            // 作用
            ViewResult result = controller.Index() as ViewResult;

            // 判斷提示
            Assert.AreEqual("修改此範本即可開始著手進行您的 ASP.NET MVC 應用程式。", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // 排列
            HomeController controller = new HomeController();

            // 作用
            ViewResult result = controller.About() as ViewResult;

            // 判斷提示
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // 排列
            HomeController controller = new HomeController();

            // 作用
            ViewResult result = controller.Contact() as ViewResult;

            // 判斷提示
            Assert.IsNotNull(result);
        }
    }
}
