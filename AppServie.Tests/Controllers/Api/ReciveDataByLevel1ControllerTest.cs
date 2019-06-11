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
    public class ReciveDataByLevel1ControllerTest
    {
        [TestMethod]
        public void ReciveDataByLevel1TestAsync()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            var x = controller.ReciveDataByLevel1("14", DateTime.Now.ToString("MMddyyyy"), "1");

            Assert.IsTrue(x.Result != null);
        }
    }
}
