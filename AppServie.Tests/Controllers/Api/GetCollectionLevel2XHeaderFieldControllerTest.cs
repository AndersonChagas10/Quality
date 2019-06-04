using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;
using System;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetCollectionLevel2XHeaderFieldControllerTest
    {
        [TestMethod]
        public void GetCollectionLevel2XHeaderFieldTest()
        {
            GetCollectionLevel2XHeaderFieldController controller = new GetCollectionLevel2XHeaderFieldController();
            controller.token = Config.TOKEN;

            var x = controller.GetListCollectionHeaderField(14, "06042019");

            Assert.IsTrue(x.Result != null);
        }
    }
}
