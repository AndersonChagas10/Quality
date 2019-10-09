using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class InsertDeviationControllerTest
    {
        [TestMethod]
        public void InsertDeviationTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            InsertDeviationClass obj = new InsertDeviationClass
            {
                Deviations = ""
            };

            var x = controller.InsertDeviation(obj);

            Assert.IsTrue(x.Result == null);
        }
    }
}
