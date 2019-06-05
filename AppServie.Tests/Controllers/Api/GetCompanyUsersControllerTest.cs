﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AppServie;
using AppServie.Controllers;
using AppServie.Api.Controllers;
using ServiceModel;

namespace AppServie.Tests.Controllers
{
    [TestClass]
    public class GetCompanyUsersControllerTest
    {
        [TestMethod]
        public void GetCompanyUsersTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            var x = controller.getCompanyUsers("14");

            Assert.IsTrue(x.Result != null);
        }
    }
}
