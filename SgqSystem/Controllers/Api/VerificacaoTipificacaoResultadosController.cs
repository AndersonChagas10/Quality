using Dominio;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


using System.Data.SqlClient;


namespace SgqSystem.Controllers.Api
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/VTP")]
    public class VerificacaoTipificacaoResultadosController : ApiController
    {
        [HttpPost]
        [Route("Save")]
        public void Save(VerificacaoTipificacaoResultados vtr)
        {
            using (var db = new SGQ_GlobalEntities())
            {
                db.Database.ExecuteSqlCommand("INSERT INTO VerificacaoTipificacaoResultados(TarefaId, CaracteristicaTipificacaoId, Chave, AreasParticipantesId) values (" +
                    vtr.TarefaId + ", " + vtr.CaracteristicaTipificacaoId + ", " + vtr.Chave + ", " + vtr.AreasParticipantesId + ");");
            }
        }
    }
}
