using Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Par
{
    [RoutePrefix("api/Params")]
    public class ParApiController : ApiController
    {

        #region Construtor

        private IParApp _parApp;

        public ParApiController(IParApp parApp)
        {
            _parApp = parApp;
        }

        #endregion

        #region Metodos

        [HttpPost]
        [Route("AddLevel01")]
        public void AddLevel01()
        {
            _parApp.AddUpdateParLevel1();
        }

        [HttpPost]
        [Route("AddLevel02")]
        public void AddLEvel02()
        {
            _parApp.AddUpdateParLevel2();
        }

        public void AddLEvel03()
        {
            _parApp.AddUpdateParLevel3();
        }

        #endregion
    }
}
