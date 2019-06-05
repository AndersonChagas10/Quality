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
    public class GetCollectionLevel2KeysControllerTest
    {
        [TestMethod]
        public void GetAllUserByUnitTest()
        {
            SyncServiceApiController controller = new SyncServiceApiController();
            controller.token = Config.TOKEN;

            //{"ParCompany_Id":"14","date":"06042019","ParLevel1_Id":0}
            GetCollectionLevel2KeysClass obj = new GetCollectionLevel2KeysClass()
            {
                ParCompany_Id = "14",
                date = DateTime.Now.ToString("MMddyyyy"),
                ParLevel1_Id = 0
            };

            var x = controller.GetCollectionLevel2Keys(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
