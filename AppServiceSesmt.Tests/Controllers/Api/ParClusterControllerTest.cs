using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class ParClusterControllerTest
    {
        [TestMethod]
        public void ParClusterController()
        {
            ParClusterController controller = new ParClusterController();

            var x = controller.ParCluster(20, 234);

            Assert.IsTrue(x.Result != null);
        }
    }
}
