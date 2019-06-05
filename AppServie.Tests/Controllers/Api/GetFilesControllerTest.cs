using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetFilesControllerTest
    {
        [TestMethod]
        public void GetFilesTest()
        {
            AppParamsController controller = new AppParamsController();

            var x = controller.GetFiles();

            Assert.IsTrue(x.Result != null);
        }
    }
}
