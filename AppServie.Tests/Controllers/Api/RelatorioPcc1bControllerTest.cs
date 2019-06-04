using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;
using System;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class RelatorioPcc1bControllerTest
    {
        [TestMethod]
        public void RelatorioPcc1bTest()
        {
            RelatorioPcc1bController controller = new RelatorioPcc1bController();
            controller.token = Config.TOKEN;

            var x = controller.reciveDataPCC1b2("14",DateTime.Now.ToString("yyyyMMdd"));

            Assert.IsTrue(x.Result != null);
        }
    }
}
