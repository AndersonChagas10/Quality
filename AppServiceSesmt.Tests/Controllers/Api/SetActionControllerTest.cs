using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class SetActionControllerTest
    {
        [TestMethod]
        public void SetActionTest()
        {
            SetActionController controller = new SetActionController();
            controller.token = Config.TOKEN;
            Acao obj = new Acao();

            var x = controller.SetAction(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
