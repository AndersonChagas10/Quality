using System;
using System.Collections.Generic;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceModel;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class GetDicionarioEstaticoControllerTest
    {
        [TestMethod]
        public void GetDicionarioEstaticoTest()
        {

            GetDicionarioEstaticoController controller = new GetDicionarioEstaticoController();
            controller.token = Config.TOKEN;

            var x = controller.GetDicionarioEstatico();

            Assert.IsTrue(x.Result != null);

        }
    }
}
