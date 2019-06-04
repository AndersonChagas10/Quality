using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class InsertJsonControllerTest
    {
        [TestMethod]
        public void InsertJsonTest()
        {
            InsertJsonController insertJsonController = new InsertJsonController();
            insertJsonController.token = Config.TOKEN;

            InsertJsonClass insertJson = new InsertJsonClass
            {
                ObjResultJSon = "",
                autoSend = false,
                deviceId = "1",
                deviceMac = "1"
            };

            var x = insertJsonController.InsertJson(insertJson);

            Assert.IsTrue(x.Result == "null");
        }
    }
}
