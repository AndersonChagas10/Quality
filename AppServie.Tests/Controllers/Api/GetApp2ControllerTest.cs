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
            SyncServiceApiController controller = new SyncServiceApiController();

            var x = controller.getAPP2("2.0.47");

            Assert.IsTrue(x.Result != null);
        }
    }
}
