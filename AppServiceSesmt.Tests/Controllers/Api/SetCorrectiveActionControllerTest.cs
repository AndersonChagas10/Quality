using System;
using System.Collections.Generic;
using AppServiceSesmt.Controllers.Api.Sesmt;
using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppServiceSesmt.Tests.Controllers.Api
{
    [TestClass]
    public class SetCorrectiveActionControllerTest
    {
        [TestMethod]
        public void SetCorrectiveActionTest()
        {
            SetCorrectiveActionController controller = new SetCorrectiveActionController();
            controller.token = Config.TOKEN;
            CorrectiveAction obj = new CorrectiveAction();

            List<CorrectiveAction> correctiveActions = new List<CorrectiveAction>();
            correctiveActions.Add(obj);

            var x = controller.SetCorrectiveAction(correctiveActions);

            Assert.IsTrue(x.Result != null);
        }
    }
}
