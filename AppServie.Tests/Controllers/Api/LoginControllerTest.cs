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
    public class LoginControllerTest
    {
        [TestMethod]
        public void LogadoSemDataTest()
        {
            LoginApiController login = new LoginApiController();

            Assert.IsTrue(login.Logado() == "onLine");
        }

        [TestMethod]
        public void LogadoDataAtualTest()
        {
            LoginApiController login = new LoginApiController();

            Assert.IsTrue(login.Logado(DateTime.Now) == "onLine");
        }

        [TestMethod]
        public void LogadoDataDoisDiasAtrasTest()
        {
            LoginApiController login = new LoginApiController();

            Assert.IsTrue(login.Logado(DateTime.Now.AddDays(-2)) == "dataInvalida");
        }
    }
}
