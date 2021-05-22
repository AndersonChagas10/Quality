using Microsoft.VisualStudio.TestTools.UnitTesting;
using SgqService.ViewModels;
using AppServiceSesmt.Api.Controllers;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class AuthenticationLoginControllerTest
    {
        [TestMethod]
        public void AuthenticationLoginTest()
        {
            AuthenticationLoginController controller = new AuthenticationLoginController();

            UserViewModel obj = new UserViewModel()
            {
                Name = "isabella-grt",
                Password = "kWRAo9vMSRhRoQu36AcDog=="
            };

            var x = controller.AuthenticationLogin(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
