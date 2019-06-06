using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class SendEmailAlertaControllerTest
    {
        [TestMethod]
        public void SendEmailAlertaTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            var x = controller.SendEmailAlerta();

            Assert.IsTrue(x.Result == null);
        }
    }
}
