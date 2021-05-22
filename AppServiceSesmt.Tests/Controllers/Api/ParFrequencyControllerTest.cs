using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceModel;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class ParFrequencyControllerTest
    {
        [TestMethod]
        public void ParFrequencyTest()
        {
            ParFrequencyController controller = new ParFrequencyController();

            PlanejamentoColetaViewModel obj = new PlanejamentoColetaViewModel()
            {
                AppDate = DateTime.Now,
                ParClusterGroup_Id = 1,
                ParCluster_Id = 1,
                ParCompany_Id = 1,
                ParFrequency_Id = 1
            };

            var x = controller.ParFrequency(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
