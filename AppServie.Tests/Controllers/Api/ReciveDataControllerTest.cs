using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class ReciveDataControllerTest
    {
        [TestMethod]
        public void ReciveDataTest()
        {
            ReciveDataController controller = new ReciveDataController();
            controller.token = Config.TOKEN;

            var x = controller.ReciveData("14", "06042019");

            Assert.IsTrue(x.Result != null);
        }
    }
}
