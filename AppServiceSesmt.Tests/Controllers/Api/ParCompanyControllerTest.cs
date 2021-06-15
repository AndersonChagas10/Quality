using System;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class ParCompanyControllerTest
    {
        [TestMethod]
        public void ParCompanyTest()
        {
            ParCompanyController controller = new ParCompanyController();
            controller.token = Config.TOKEN;

            var x = controller.ParCompany(1);

            Assert.IsTrue(x.Result != null);
        }
    }
}
