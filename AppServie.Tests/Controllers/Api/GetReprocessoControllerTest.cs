using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetReprocessoControllerTest
    {
        [TestMethod]
        public void GetReprocessoTest()
        {
            ReprocessoController controller = new ReprocessoController();
            controller.token = Config.TOKEN;

            var x = controller.Get(14);
        }
    }
}
