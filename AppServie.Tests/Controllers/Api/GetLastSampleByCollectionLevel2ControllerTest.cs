using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetLastSampleByCollectionLevel2ControllerTest
    {
        [TestMethod]
        public void GetLastSampleByCollectionLevel2Test()
        {
            SyncServiceApiController insertJsonController = new SyncServiceApiController();
            insertJsonController.token = Config.TOKEN;

            GetLastSampleByCollectionLevel2Class obj = new GetLastSampleByCollectionLevel2Class
            {
            };

            var x = insertJsonController.GetLastSampleByCollectionLevel2(obj);

            Assert.IsTrue(x.Result == 0);
        }
    }
}
