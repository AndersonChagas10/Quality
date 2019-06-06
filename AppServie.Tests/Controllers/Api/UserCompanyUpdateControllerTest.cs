using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class UserCompanyUpdateControllerTest
    {
        [TestMethod]
        public void UserCompanyUpdateTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            var x = controller.UserCompanyUpdate("1",14);

            Assert.IsTrue(x.Result == null);
        }
    }
}
