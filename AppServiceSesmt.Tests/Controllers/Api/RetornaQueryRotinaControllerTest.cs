using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class RetornaQueryRotinaControllerTest
    {
        [TestMethod]
        public void RetornaQueryRotinaTest()
        {
            RetornaQueryRotinaController controller = new RetornaQueryRotinaController();

            //PlanejamentoColetaViewModel obj = new PlanejamentoColetaViewModel()
            //{
            //    AppDate = DateTime.Now,
            //    ParClusterGroup_Id = 1,
            //    ParCluster_Id = 1,
            //    ParCompany_Id = 1,
            //    ParFrequency_Id = 1
            //};

            var x = controller.RetornaQueryRontina("");

            Assert.IsTrue(x.Result != null);
        }
    }
}
