using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetPhaseLevel2ControllerTest
    {
        [TestMethod]
        public void GetPhaseLevel2Test()
        {
            GetPhaseLevel2Controller controller = new GetPhaseLevel2Controller();
            controller.token = Config.TOKEN;

            var x = controller.GetPhaseLevel2(14, "06042019");
        }
    }
}
