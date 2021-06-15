using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class ParClusterGroupControllerTest
    {
        [TestMethod]
        public void ParClusterGroupTest()
        {
            ParClusterGroupController controller = new ParClusterGroupController();
            controller.token = Config.TOKEN;

            var x = controller.ParClusterGroup(234);

            Assert.IsTrue(x.Result != null);
        }
    }
}
