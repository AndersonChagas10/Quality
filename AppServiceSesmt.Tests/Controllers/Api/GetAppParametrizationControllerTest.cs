using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceModel;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class GetAppParametrizationControllerTest
    {
        [TestMethod]
        public void GetAppParametrizationTest()
        {

            GetAppParametrizationController controller = new GetAppParametrizationController();
            controller.token = Config.TOKEN;

            PlanejamentoColeta obj = new PlanejamentoColeta()
            {
                ParCompany_Id = 1,
                AppDate = DateTime.Now
            };

            var x = controller.GetAppParametrization(obj);

            Assert.IsTrue(x.Result != null);

        }
    }
}
