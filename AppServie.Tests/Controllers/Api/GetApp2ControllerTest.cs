using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetApp2ControllerTest
    {
        [TestMethod]
        public void GetApp2Test()
        {
            GetApp2Controller controller = new GetApp2Controller();

            var x = controller.getAPP2("2.0.47");

            Assert.IsTrue(x.Result != null);
        }
    }
}
