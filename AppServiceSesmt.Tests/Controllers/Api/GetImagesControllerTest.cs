using System;
using AppServiceSesmt.Api.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class GetImagesControllerTest
    {
        [TestMethod]
        public void GetImagesTest()
        {
            AppParamsController controller = new AppParamsController();

            var x = controller.GetImages();

            Assert.IsTrue(x.Result != null);
        }
    }
}
