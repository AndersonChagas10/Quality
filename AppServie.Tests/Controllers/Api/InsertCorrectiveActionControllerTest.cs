using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class InsertCorrectiveActionControllerTest
    {
        [TestMethod]
        public void InsertCorrectiveActionTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            InsertCorrectiveActionClass obj = new InsertCorrectiveActionClass
            {
                
            };

            var x = controller.InsertCorrectiveAction(obj);

            Assert.IsTrue(x.Result == null);
        }
    }
}
