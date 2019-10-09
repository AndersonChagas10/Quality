using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;
using SgqService.ViewModels;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class AuthenticationLoginControllerTest
    {
        [TestMethod]
        public void AuthenticationLoginTest()
        {
            UserController controller = new UserController();

            UserViewModel obj = new UserViewModel()
            {
                Name = "camilaprata-mtz",
                Password = "kWRAo9vMSRhRoQu36AcDog=="
            };

            var x = controller.AuthenticationLogin(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
