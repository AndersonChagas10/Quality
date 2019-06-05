using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetAllUserByUnitControllerTest
    {
        [TestMethod]
        public void GetAllUserByUnitTest()
        {
            UserController controller = new UserController();

            var x = controller.GetAllUserByUnit(14);

            Assert.IsTrue(x.Result != null);
        }
    }
}
