using Dominio;
using System.Web.Http;
using System.Web.Http.Cors;


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
            using (var db = new SgqDbDevEntities())
            {
                string ctid = vtr.CaracteristicaTipificacaoId == null ? "NULL" : "'" + vtr.CaracteristicaTipificacaoId + "'";
                string apid = vtr.AreasParticipantesId == null ? "NULL" : "'" + vtr.AreasParticipantesId + "'";
                
                string sql = "INSERT INTO VerificacaoTipificacaoResultados (TarefaId, CaracteristicaTipificacaoId, Chave, AreasParticipantesId) VALUES " +
                                                                          "('"+ vtr.TarefaId + "', " + ctid + ", '" + vtr.Chave + "', "+ apid + ")";

                db.Database.ExecuteSqlCommand(sql);
            }
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/VT")]
    public class VTController : ApiController
    {
        [HttpPost]
        [Route("Save")]
        public void Save(VerificacaoTipificacao vt)
        {
            using (var db = new SgqDbDevEntities())
            {
                db.Database.ExecuteSqlCommand("INSERT INTO VerificacaoTipificacao(Sequencial, Banda, DataHora, UnidadeId, Chave, Status) "+
                    " VALUES ('" + vt.Sequencial + "', '" + vt.Banda + "', '" + vt.DataHora+"', '" + vt.UnidadeId + "', '" + vt.Chave + "', '" + vt.Status + "');");
            }
        }
    }


}
