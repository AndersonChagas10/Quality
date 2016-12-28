using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SgqSystem.Controllers.Api
{

    public partial class PCC1B
    {
        public int Sequencial;
        public int Banda;

        public PCC1B(int sequencial, int banda)
        {
            Sequencial = sequencial;
            Banda = banda;
        }
    };

    [HandleApi()]
    [RoutePrefix("api/PCC1B")]
    public class PCC1BController : ApiController
    {
        [HttpPost]
        [Route("Next")]
        public PCC1B Save()
        {
            var pcc1b = new PCC1B(1, 2);

            return pcc1b;
        }
    }
}
