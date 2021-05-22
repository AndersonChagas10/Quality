using System;
using AppServiceSesmt.Controllers.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class GetAppVersionIsUpdatedControllerTest
    {
        [TestMethod]
        public void GetAppVersionIsUpdatedTest()
        {
            ConfigController controller = new ConfigController();

            var x = controller.GetAppVersionIsUpdated("2.0.56");

            Assert.IsTrue(x.Result != null);

        }
    }
}
