using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceModel;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class GetParFrequencyXParLevel1ControllerTest
    {
        [TestMethod]
        public void ParFrequencyTest()
        {
            GetParFrequencyXParLevel1Controller controller = new GetParFrequencyXParLevel1Controller();
            controller.token = Config.TOKEN;

            PlanejamentoColetaViewModel obj = new PlanejamentoColetaViewModel()
            {
                AppDate = DateTime.Now,
                ParClusterGroup_Id = 1,
                ParCluster_Id = 1,
                ParCompany_Id = 1,
                ParFrequency_Id = 1
            };

            var x = controller.GetParFrequencyXParLevel1(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
