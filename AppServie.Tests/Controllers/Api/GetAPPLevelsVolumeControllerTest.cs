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
    public class GetAPPLevelsVolumeControllerTest
    {
        [TestMethod]
        public void GetAPPLevelsVolume()
        {
            GetAPPLevelsVolumeController controller = new GetAPPLevelsVolumeController();
            controller.token = Config.TOKEN;

            //{"UserSgq_Id":"1","ParCompany_Id":"14","Date":"Tue, 04 Jun 2019 15:00:00 GMT","Level1ListId":"","Shift_Id":1}
            GetAPPLevelsVolumeClass obj = new GetAPPLevelsVolumeClass()
            {
                UserSgq_Id = 1,
                ParCompany_Id = 14,
                Date = DateTime.Now,
                Level1ListId = null,
                Shift_Id = 1
            };

            var x = controller.GetAPPLevelsVolume(obj);

            Assert.IsTrue(x.Result != null);
        }
    }
}
