using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class ParReasonControllerTest
    {
        [TestMethod]
        public void ParReasonTest()
        {
            ParReasonController controller = new ParReasonController();

            var x = controller.Get();

            Assert.IsTrue(x.Result != null);
        }
    }
}
