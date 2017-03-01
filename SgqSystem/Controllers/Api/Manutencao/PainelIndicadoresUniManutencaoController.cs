using Dominio;
using DTO.DTO.Manutencao;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SgqSystem.Controllers.Api.Manutencao
{
    [RoutePrefix("api/GetIndicadoresUniManutencao")]
    public class PainelIndicadoresUniManutencaoController : ApiController
    {
        [HttpPost]
        [Route("GetTabela")]
        public List<PainelIndicadoresUniManutencaoDTO> GetTabela(obj3 obj)
        {
            List<PainelIndicadoresUniManutencaoDTO> _mockEvolucao = new List<PainelIndicadoresUniManutencaoDTO>();
            PainelIndicadoresUniManutencaoDTO manColeta;

            string parametro = obj.indicador;
            var realizado = "";
            var orcado = "";
            List<obj> d;
            List<obj2> e;

            var query = "SELECT top 1 Realizado.Realizado ,Orcado.Orcado FROM " +
                        "(SELECT top 1 Name Realizado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Real' UNION ALL SELECT '0') Realizado " +
                        ",(SELECT top 1 Name Orcado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Meta' UNION ALL SELECT '0') Orcado ";

            using (var db = new SgqDbDevEntities())
            {
                d = db.Database.SqlQuery<obj>(query).ToList();
            }

            foreach (var item in d)
            {
                orcado = item.orcado;
                realizado = item.realizado;
            }

            string tipo = "";

            if (obj.subRegional == "Todas")

                tipo = "SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegionalGrupo = '" + obj.regional + "' and ParCompany_id is not null";

            else

                tipo = "SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegional = '" + obj.subRegional + "' and ParCompany_id is not null";

            var query2 = "SELECT " +
                         "BASONA.Dado " +
                        ",BASONA.Realizado " +
                        ",BASONA.Orcado " +
                    "FROM " +
                    "(" +
                        "SELECT " +
                            "'Por Unidade' TipoRelatorio " +
                            ", Mes.Mes dado " +
                            ", isnull(Base.Realizado, 0) realizado " +
                            ", isnull(Base.Orcado, 0)    orcado " +
                        "FROM MANANOMES MES " +
                        "LEFT JOIN " +
                        "( " +
                            "SELECT MONTH(ISNULL(Base_dateRef, cast(Base_dateAdd AS varchar(10)))) Mes, " +
                                    "SUM(ISNULL(CASE " +
                                        "WHEN " + realizado + " = '0' THEN 0.00 " +
                                        "ELSE " + realizado + " " +
                                    "END, 0)) realizado, " +
                                    "SUM(ISNULL(CASE " +
                                       "WHEN " + orcado + " = '0' THEN 0.00 " +
                                       "ELSE " + orcado + " " +
                                    "END, 0)) orcado " +
                            "FROM MANCOLETADADOS Man " +
                            "WHERE " +
                                "ISNULL(YEAR(BASE_DATEREF), YEAR(BASE_DATEADD)) = '" + obj.ano + "' " +
                                "AND ISNULL(MONTH(BASE_DATEREF), MONTH(BASE_DATEADD)) LIKE CASE WHEN '" + obj.mes + "' = 0 THEN '%%' ELSE '" + obj.mes + "' END " +
                                "AND Man.Base_parCompany_id in (SELECT id FROM ParCompany WHERE Name = '" + obj.unidade + "')" +
                            "GROUP BY MONTH(ISNULL(Base_dateRef, cast(Base_dateAdd AS varchar(10)))) " +
                        ")Base on MES.MesInt = Base.Mes " +
                        "union all " +
                        "SELECT " +
                            "'Por Regional' TipoRelatorio " +
                            ", Uni.EmpresaSigla dado " +
                            ", isnull(Base.Realizado, 0) Realizado " +
                            ", isnull(Base.Orcado, 0)    Orcado " +
                        "FROM DimManBaseUni Uni " +
                        "LEFT JOIN( " +
                            "SELECT Man.Base_parCompany_id, " +
                                    "SUM(ISNULL(CASE " +
                                        "WHEN  " + realizado + "  = '0' THEN 0.00 " +
                                        "ELSE  " + realizado + "  " +
                                    "END, 0)) realizado, " +
                                    "SUM(ISNULL(CASE " +
                                        "WHEN " + orcado + " = '0' THEN 0.00 " +
                                        "ELSE " + orcado + " " +
                                    "END, 0)) orcado " +
                            "FROM MANCOLETADADOS Man " +
                            "WHERE " +
                                "ISNULL(YEAR(BASE_DATEREF), YEAR(BASE_DATEADD)) = '" + obj.ano + "'" +
                                "AND ISNULL(MONTH(BASE_DATEREF), MONTH(BASE_DATEADD)) LIKE CASE WHEN '" + obj.mes + "' = 0 THEN '%%' ELSE '" + obj.mes + "' END " +
                                "AND Man.Base_parCompany_id in (" + tipo + ") " +
                            "GROUP BY Man.Base_parCompany_id " +
                        ")Base on uni.Parcompany_id = Base.Base_parCompany_id " +
                        //"WHERE Base.realizado != 0 AND Base.orcado != 0 " +
                    ")BASONA " +
                    "WHERE BASONA.TipoRelatorio = '" + obj.tipoRelatorio + "' ";

            using (var db = new SgqDbDevEntities())
            {
                e = db.Database.SqlQuery<obj2>(query2).ToList();
            }

            foreach (var item in e)
            {
                manColeta = new PainelIndicadoresUniManutencaoDTO();

                manColeta.dado = item.dado;
                manColeta.realizado = item.realizado;
                manColeta.orcado = item.orcado;
                manColeta.desvio = manColeta.realizado - manColeta.orcado;

                if (manColeta.realizado == 0 || manColeta.orcado == 0)
                    manColeta.porcDesvio = 0;
                else
                    manColeta.porcDesvio = ((manColeta.realizado / manColeta.orcado) - 1) * 100;

                _mockEvolucao.Add(manColeta);
            }

            return _mockEvolucao;
        }


        [HttpPost]
        [Route("CriaGraficoEvolucao")]
        public List<PainelIndicadoresUniManutencaoDTO> CriaGraficoEvolucao()
        {
            List<PainelIndicadoresUniManutencaoDTO> _mockEvolucao = new List<PainelIndicadoresUniManutencaoDTO>();
            PainelIndicadoresUniManutencaoDTO coleta = new PainelIndicadoresUniManutencaoDTO();

            coleta.orcado = 10;
            coleta.realizado = 20;

            _mockEvolucao.Add(coleta);

            return _mockEvolucao;
        }

        [HttpPost]
        [Route("CriaGraficoAcumulado")]
        public List<PainelIndicadoresUniManutencaoDTO> CriaGraficoAcumulado()
        {
            List<PainelIndicadoresUniManutencaoDTO> _mockEvolucao = new List<PainelIndicadoresUniManutencaoDTO>();
            PainelIndicadoresUniManutencaoDTO coleta = new PainelIndicadoresUniManutencaoDTO();

            coleta.orcado = 100;
            coleta.realizado = 200;

            _mockEvolucao.Add(coleta);

            return _mockEvolucao;
        }

    }

    public class obj
    {
        public string realizado { get; set; }
        public string orcado { get; set; }
    }

    public class obj2
    {
        public string dado { get; set; }
        public decimal realizado { get; set; }
        public decimal orcado { get; set; }
    }

    public class obj3
    {
        public string indicador { get; set; }
        public string unidade { get; set; }
        public string ano { get; set; }
        public string tipoRelatorio { get; set; }
        public string regional { get; set; }
        public string subRegional { get; set; }
        public string mes { get; set; }
    }
}
