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
            UserSGQByIdController controller = new UserSGQByIdController();

            var x = controller.UserSGQById(1);

            Assert.IsTrue(x.Result != null);
        }
    }
}
