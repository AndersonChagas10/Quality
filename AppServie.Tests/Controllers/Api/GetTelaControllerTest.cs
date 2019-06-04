using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetTelaControllerTest
    {
        [TestMethod]
        public void GetTelaTest()
        {
            GetTelaController getTelaController = new GetTelaController();
            getTelaController.token = "camilaprata-mtz|kWRAo9vMSRhRoQu36AcDog==";

            var x = getTelaController.GetTela(14,1);

            Assert.IsTrue(x.Result != null);
        }
    }
}
