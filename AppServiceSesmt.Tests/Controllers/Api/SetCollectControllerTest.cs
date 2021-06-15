using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class SetCollectControllerTest
    {
        [TestMethod]
        public void SetCollectTest()
        {
            SetCollectController controller = new SetCollectController();
            controller.token = Config.TOKEN;
            Collection obj = new Collection();

            List<Collection> correctiveActions = new List<Collection>();
            correctiveActions.Add(obj);

            var x = controller.SetCollect(correctiveActions);

            Assert.IsTrue(x.Result != null);
        }
    }
}
