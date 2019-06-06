using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetResultEvaluationDefectsControllerTest
    {
        [TestMethod]
        public void GetResultEvaluationDefectsTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            GetResultEvaluationDefects obj = new GetResultEvaluationDefects
            {
            };

            var x = controller.GetResultEvaluationDefects(obj);

            Assert.IsTrue(x.Result == null);
        }
    }
}
