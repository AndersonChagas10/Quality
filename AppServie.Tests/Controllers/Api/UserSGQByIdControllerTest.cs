using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class UserSGQByIdControllerTest
    {
        [TestMethod]
        public void UserSGQByIdTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();

            var x = controller.UserSGQById(1);

            Assert.IsTrue(x.Result != null);
        }
    }
}
