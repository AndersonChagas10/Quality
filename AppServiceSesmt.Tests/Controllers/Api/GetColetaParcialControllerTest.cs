using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceModel;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class GetColetaParcialControllerTest
    {
        [TestMethod]
        public void GetColetaParcialTest()
        {
            AppColetaController controller = new AppColetaController();
            controller.token = Config.TOKEN;

            GetResultsData obj = new GetResultsData()
            {
                CollectionDate = DateTime.Now,
                ParCompany_Id = 1,
                ParFrequency_Id = 2
            };

            var x = controller.GetColetaParcial(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
