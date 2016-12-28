using Dominio;
using Dominio.Interfaces.Services;
using DTO.DTO.Params;
using SgqSystem.Handlres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Manutencao")]
    public class ManutencaoController : ApiController
    {
        [HttpPost]
        [Route("getTabela1")]
        public List<ManutencaoClasse> getSelectTabela1()
        {

            var lista = new List<ManutencaoClasse>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "SELECT EmpresaRegional ,Pacote ,SUM(DespesaOrcada) AS DespesaOrcada, SUM(DespesaRealizada)AS DespesaRealizada FROM Manutencao ";
                sql += " WHERE MesAno BETWEEN '20150101' AND '20180101' AND TipoInformacao = 'CustoFixo' ";
                sql += " GROUP BY EmpresaRegional, Pacote";

                lista = db.Database.SqlQuery<ManutencaoClasse>(sql).ToList();
            }

            return lista;
        }
    }

    public class ManutencaoClasse
    {
        public string TipoInformacao { get; set; }
        public DateTime MesAno { get; set; }
        public int? EmpresaCodigo { get; set; }
        public string EmpresaSigla { get; set; }
        public string EmpresaRegional { get; set; }
        public string EmpresaRegionalGrupo { get; set; }
        public string EmpresaCluster { get; set; }
        public string CentroDeCusto { get; set; }
        public string Pacote { get; set; }
        public string ContaContabilCodigo { get; set; }
        public string ContaContabil { get; set; }
        public double? DespesaOrcada { get; set; }
        public double? DespesaRealizada { get; set; }
        public double? ConsumoOrcado { get; set; }
        public double? ConsumoRealizado { get; set; }
        public string TipoConsumo { get; set; }
        public double? ProducaoOrcada { get; set; }
        public double? ProducaoRealizada { get; set; }
        public string TipoProducao { get; set; }
        public int? Id { get; set; }
    }


}
