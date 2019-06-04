using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;
using System;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetListParMultipleValuesXParCompanyControllerTest
    {
        [TestMethod]
        public void GetListParMultipleValuesXParCompanyTest()
        {
            GetListParMultipleValuesXParCompanyController controller = new GetListParMultipleValuesXParCompanyController();
            controller.token = Config.TOKEN;

            var x = controller.GetListParMultipleValuesXParCompany(14, "3987891");

            Assert.IsTrue(x.Result != null);
        }
    }
}
