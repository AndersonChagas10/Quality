using Dominio;
using DTO.DTO.Manutencao;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
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

            string parametro = obj.indicador;//"Bois Abatidos";
            var realizado = "";
            var orcado = "";
            List<obj> d;
            List<obj2> e;

            //var query = " SELECT Realizado,Orcado FROM (SELECT TOP 1 Name Realizado FROM DimManColetaDados WHERE DimName = '"+ parametro + "' and DimRealTarget = 'Real') Realizado,(SELECT TOP 1 Name Orcado FROM DimManColetaDados WHERE DimName = '"+ parametro + "' and DimRealTarget = 'Meta') Orcado";

            var query = "SELECT top 1 Realizado.Realizado ,Orcado.Orcado FROM " +
                        "(SELECT top 1 Name Realizado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Real' UNION ALL SELECT '0') Realizado " +
                        ",(SELECT top 1 Name Orcado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Meta' UNION ALL SELECT '0') Orcado ";

            using (var db = new SgqDbDevEntities())
            {
                d = db.Database.SqlQuery<obj>(query).ToList();
            }

            foreach (var item in d)
            {
                //if (item.orcado == "")
                //    orcado = "0";
                //else
                orcado = item.orcado;


                //if (item.realizado == "")
                //    realizado = "0";
                //else
                realizado = item.realizado;
            }

            var query2 = "SELECT " +
                            "Convert(varchar(7), ISNULL(Base_dateRef, cast(Base_dateAdd as varchar(10))), 120) Data " +
                            ",SUM(ISNULL(CASE WHEN " + realizado + " = '0' THEN 0.00 ELSE " + realizado + " END,0)) realizado " +
                            ",SUM(ISNULL(CASE WHEN " + orcado + " = '0' THEN 0.00 ELSE " + orcado + " END,0)) orcado " +
                            "FROM MANCOLETADADOS " +
                            //"WHERE 1 = 1" +
                            "WHERE ISNULL(YEAR(BASE_DATEREF),YEAR(BASE_DATEADD)) = " + obj.ano + "";
                            if (obj.unidade != null)
                            {
                                query2 += " and Base_parCompany_id = " + obj.unidade + " ";
                            }
                            query2 += "GROUP BY " +
                            "Convert(varchar(7), ISNULL(Base_dateRef, cast(Base_dateAdd as varchar(10))), 120)";
            //"HAVING SUM(Rendimento_Real)IS NOT NULL OR SUM(Rendimento_Meta) IS NOT NULL";

            using (var db = new SgqDbDevEntities())
            {
                e = db.Database.SqlQuery<obj2>(query2).ToList();
            }

            foreach (var item in e)
            {
                manColeta = new PainelIndicadoresUniManutencaoDTO();

                manColeta.realizado = item.realizado;
                manColeta.orcado = item.orcado;
                manColeta.desvio = manColeta.realizado - manColeta.orcado;

                if (manColeta.realizado == 0 || manColeta.orcado == 0)
                    manColeta.porcDesvio = 0;
                else
                    manColeta.porcDesvio = ((manColeta.realizado / manColeta.orcado) - 1) * 100;

                _mockEvolucao.Add(manColeta);
            }


            //for (int i = 0; i < 12; i++)
            //{
            //    manColeta.orcado = i + 2;
            //    manColeta.realizado = i + 1;
            //    manColeta.desvio = manColeta.orcado - manColeta.realizado;
            //    manColeta.porcDesvio = (manColeta.desvio / manColeta.orcado) * 100;

            //    _mockEvolucao.Add(manColeta);

            //    manColeta = new PainelIndicadoresUniManutencaoDTO();
            //}

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
        public decimal realizado { get; set; }
        public decimal orcado { get; set; }
    }

    public class obj3
    {
        public string indicador { get; set; }
        public Nullable<int> unidade { get; set; }
        public string ano { get; set; }
    }
}
