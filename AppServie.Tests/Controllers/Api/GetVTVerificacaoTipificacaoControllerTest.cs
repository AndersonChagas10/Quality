using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetVTVerificacaoTipificacaoControllerTest
    {
        [TestMethod]
        public void GetVTVerificacaoTipificacaoTest()
        {
            GetVTVerificacaoTipificacaoController controller = new GetVTVerificacaoTipificacaoController();
            controller.token = Config.TOKEN;

            var x = controller.GetVTVerificacaoTipificacao("06042019",14);

            Assert.IsTrue(x.Result != null);
        }
    }
}
