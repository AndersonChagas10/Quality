using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceModel;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class GetResultsControllerTest
    {
        [TestMethod]
        public void GetResultsTest()
        {

            GetResultsController controller = new GetResultsController();
            GetResultsData obj = new GetResultsData()
            {
                CollectionDate = DateTime.Now,
                ParCompany_Id = 1,
                ParFrequency_Id = 1
            };

            var x = controller.GetResults(obj);

            Assert.IsTrue(x.Result != null);

        }
    }
}
