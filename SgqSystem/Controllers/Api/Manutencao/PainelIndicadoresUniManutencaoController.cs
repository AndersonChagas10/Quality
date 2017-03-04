using Dominio;
using DTO.DTO.Manutencao;
using System;
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

            List<string> indicador = new List<string>();
            var tipoCalculo = "normal";

            indicador.Add(obj.indicador);

            switch (indicador[0])
            {
                case "KWh/Boi Proc":
                    indicador.Add("KWh");
                    indicador.Add("Bois Processados");
                    break;
                case "Mcal/Boi Proc":
                    indicador.Add("Mcal");
                    indicador.Add("Bois Processados");
                    break;
                case "Água M³/Boi":
                    indicador.Add("Consumo Agua m³");
                    indicador.Add("Bois Abatidos");
                    break;
                case "Rend.(Sebo+Farinha)":
                    indicador.Add("Consumo Agua m³");
                    indicador.Add("Bois Abatidos");
                    break;
                case "Sebo Flotado":
                    indicador.Add("Litros de Sebo");
                    indicador.Add("Bois Abatidos");
                    break;
                case "Disponibilidade Ab":
                    indicador.Add("Nº Hrs Disponíveis Trabalhar Ab");
                    indicador.Add("Parada Abate (min)");
                    tipoCalculo = "Disponibilidade";
                    break;
                case "Disponibilidade Des":
                    indicador.Add("Nº Hrs Disponíveis Trabalhar Des");
                    indicador.Add("Parada Des. (min)");
                    tipoCalculo = "Disponibilidade";
                    break;
                case "Disponibilidade Total":
                    indicador.Add("Nº Hrs Disponíveis Trabalhar Ab");
                    indicador.Add("Nº Hrs Disponíveis Trabalhar Des");
                    indicador.Add("Parada Abate (min)");
                    indicador.Add("Parada Des. (min)");
                    tipoCalculo = "Disponibilidade";
                    break;
                case "Paradas Total":
                    indicador.Add("Parada Abate (min)");
                    indicador.Add("Parada Des. (min)");
                    tipoCalculo = "Soma";
                    break;
                case "Eficiên.Programção":
                    indicador.Add("Nº OS Programadas");
                    indicador.Add("Nº OS Executadas");
                    break;
                case "Apropr Planej...to":
                    indicador.Add("Nº OS Planejadas");
                    indicador.Add("Nº OS Programadas");
                    break;
                case "Apropriação de O.S":
                    indicador.Add("Nº Hrs Apropriadas OS");
                    indicador.Add("Nº Hrs Disponíveis Trabalhar");
                    break;
                case "Absenteísmo":
                    indicador.Add("Nº Colaboradores Registrados");
                    indicador.Add("Nº Colaboradores Ausentes");
                    break;
                case "Rotatividade":
                    indicador.Add("Nº Colaboradores Registrados");
                    indicador.Add("Nº Colaboradores Desligados");
                    break;
                case "Devoluções %":
                    indicador.Add("Total Vendas R$");
                    indicador.Add("Total Devolvido R$");
                    break;

                //Media
                case "Taxa de Frequência":
                    indicador.Add("Taxa de Frequência");
                    indicador.Add("Taxa de Frequência");
                    tipoCalculo = "media";
                    break;
                case "Pilar Manutenção":
                    indicador.Add("Pilar Manutenção");
                    indicador.Add("Pilar Manutenção");
                    tipoCalculo = "media";
                    break;
                case "CARTA METAS":
                    indicador.Add("CARTA METAS");
                    indicador.Add("CARTA METAS");
                    tipoCalculo = "media";
                    break;

                case "Head Count":
                    indicador.Add("Head Count");
                    indicador.Add("Head Count");
                    break;
                default:
                    break;
            }

            var numeroDeVariaveis = indicador.Count;
            listaDelistas vetor0 = new listaDelistas();
            listaDelistas vetor1 = new listaDelistas();
            listaDelistas vetor2 = new listaDelistas();
            listaDelistas vetor3 = new listaDelistas();
            listaDelistas vetor4 = new listaDelistas();

            for (var i = 0; i < numeroDeVariaveis; i++)
            {

                string parametro = indicador[i];
                var realizado = "";
                var orcado = "";
                List<obj> d = null;
                List<obj2> e = null;

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
                string tipo2 = "";

                if (obj.subRegional == "Todas")
                {

                    tipo = "SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegionalGrupo = '" + obj.regional + "' and ParCompany_id is not null";
                    tipo2 = "Select distinct ParCompany_id, EmpresaSigla, DimManBaseReg_id, EmpresaRegional, DimManBaseRegGrup_id, EmpresaRegionalGrupo, EmpresaCluster from DimManBaseUni where EmpresaRegionalGrupo = '" + obj.regional + "' and ParCompany_id is not null";

                }
                else
                {
                    tipo = "SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegional = '" + obj.subRegional + "' and ParCompany_id is not null";
                    tipo2 = "Select distinct ParCompany_id, EmpresaSigla, DimManBaseReg_id, EmpresaRegional, DimManBaseRegGrup_id, EmpresaRegionalGrupo, EmpresaCluster from DimManBaseUni where EmpresaRegional = '" + obj.subRegional + "' and ParCompany_id is not null";
                }

                var query2 = "\n SELECT " +
                             "\n BASONA.Dado " +
                            "\n ,BASONA.Realizado " +
                            "\n ,BASONA.Orcado " +
                            "\n ,BASONA.qtde " +
                        "\n FROM " +
                        "\n (" +
                            "\n SELECT " +
                                "\n 'Por Unidade' TipoRelatorio " +
                                "\n , Mes.Mes dado " +
                                "\n , isnull(Base.Realizado, 0) realizado " +
                                "\n , isnull(Base.Orcado, 0)    orcado " +
                                "\n , isnull(qtde, 0) qtde " +
                            "\n FROM MANANOMES MES " +
                            "\n LEFT JOIN " +
                            "\n ( " +
                                "\n SELECT MONTH(ISNULL(Base_dateRef, cast(Base_dateAdd AS varchar(10)))) Mes, " +
                                        "\n SUM(ISNULL(CASE " +
                                            "\n WHEN " + realizado + " = '0' THEN 0.00 " +
                                            "\n ELSE " + realizado + " " +
                                        "\n END, 0)) realizado, " +
                                        "\n SUM(ISNULL(CASE " +
                                           "\n WHEN " + orcado + " = '0' THEN 0.00 " +
                                           "\n ELSE " + orcado + " " +
                                        "\n END, 0)) orcado, " +
                                        "\n count(1) as qtde " +
                                "\n FROM MANCOLETADADOS Man " +
                                "\n WHERE " +
                                    "\n " + realizado + " is not null and " +
                                    "\n ISNULL(YEAR(BASE_DATEREF), YEAR(BASE_DATEADD)) = '" + obj.ano + "' " +
                                    "\n AND ISNULL(MONTH(BASE_DATEREF), MONTH(BASE_DATEADD)) LIKE CASE WHEN '" + obj.mes + "' = 0 THEN '%%' ELSE '" + obj.mes + "' END " +
                                    "\n AND Man.Base_parCompany_id in (SELECT id FROM ParCompany WHERE Name = '" + obj.unidade + "')" +
                                "\n GROUP BY MONTH(ISNULL(Base_dateRef, cast(Base_dateAdd AS varchar(10)))) " +
                            "\n )Base on MES.MesInt = Base.Mes " +
                            "\n union all " +
                            "\n SELECT " +
                                "\n 'Por Regional' TipoRelatorio " +
                                "\n , Uni.EmpresaSigla dado " +
                                "\n , isnull(Base.Realizado, 0) Realizado " +
                                "\n , isnull(Base.Orcado, 0)    Orcado " +
                                "\n , isnull(qtde, 0) qtde " +
                            "\n FROM (" + tipo2 + ") Uni " + //AQUI
                            "\n LEFT JOIN( " +
                                "SELECT Man.Base_parCompany_id, " +
                                        "\n SUM(ISNULL(CASE " +
                                            "\n WHEN  " + realizado + "  = '0' THEN 0.00 " +
                                            "\n ELSE  " + realizado + "  " +
                                        "\n END, 0)) realizado, " +
                                        "\n SUM(ISNULL(CASE " +
                                            "\n WHEN " + orcado + " = '0' THEN 0.00 " +
                                            "\n ELSE " + orcado + " " +
                                        "\n END, 0)) orcado, " +
                                        "\n count(1) as qtde " +
                                "\n FROM MANCOLETADADOS Man " +
                                "\n WHERE " +
                                    "\n " + realizado + " is not null and " +
                                    "\n ISNULL(YEAR(BASE_DATEREF), YEAR(BASE_DATEADD)) = '" + obj.ano + "'" +
                                    "\n AND ISNULL(MONTH(BASE_DATEREF), MONTH(BASE_DATEADD)) LIKE CASE WHEN '" + obj.mes + "' = 0 THEN '%%' ELSE '" + obj.mes + "' END " +
                                    "\n AND Man.Base_parCompany_id in (" + tipo + ") " +
                                "\n GROUP BY Man.Base_parCompany_id " +
                            "\n )Base on uni.Parcompany_id = Base.Base_parCompany_id " +
                        //"\n WHERE Base.realizado != 0 AND Base.orcado != 0 " +
                        "\n )BASONA " +
                        "\n WHERE BASONA.TipoRelatorio = '" + obj.tipoRelatorio + "' ";

                using (var db = new SgqDbDevEntities())
                {
                    e = db.Database.SqlQuery<obj2>(query2).ToList();
                    if (i == 0)
                    {
                        vetor0.lista = e;
                    }
                    else if (i == 1)
                    {
                        vetor1.lista = e;
                    }
                    else if (i == 2)
                    {
                        vetor2.lista = e;
                    }
                    else if (i == 3)
                    {
                        vetor3.lista = e;
                    }
                    else if (i == 4)
                    {
                        vetor4.lista = e;
                    }
                }
            }

            List<obj2> f = new List<obj2>();
            f = vetor0.lista;

            if (vetor1.lista != null)
            {

                for (int i = 0; i < vetor1.lista.Count; i++)
                {
                    f[i].dado = vetor1.lista[i].dado;

                    try
                    {
                        if (tipoCalculo == "media")
                        {
                            f[i].realizado = vetor1.lista[i].realizado / vetor1.lista[i].qtde;
                        }
                        else if (tipoCalculo == "Disponibilidade")
                        {
                            if (vetor4.lista != null)
                            {
                                f[i].realizado = 1 - ((vetor3.lista[i].realizado + vetor4.lista[i].realizado) / ((vetor1.lista[i].realizado * 60) + (vetor2.lista[i].realizado * 60)));
                            }
                            else
                            {
                                f[i].realizado = 1 - ((vetor2.lista[i].realizado) / ((vetor1.lista[i].realizado * 60)));
                            }

                        }
                        else

                            f[i].realizado = vetor1.lista[i].realizado / vetor2.lista[i].realizado;
                    }
                    catch (DivideByZeroException e)
                    {
                        f[i].realizado = 0;
                    }

                    try
                    {
                        if (tipoCalculo == "media")
                            f[i].orcado = vetor1.lista[i].orcado / vetor1.lista[i].qtde;
                        else if (tipoCalculo == "Disponibilidade")
                        {
                            if (vetor4.lista != null)
                            {
                                f[i].orcado = 1 - ((vetor3.lista[i].orcado + vetor4.lista[i].orcado) / ((vetor1.lista[i].orcado * 60) + (vetor2.lista[i].orcado * 60)));
                            }
                            else
                            {
                                f[i].orcado = 1 - ((vetor2.lista[i].orcado) / ((vetor1.lista[i].orcado * 60)));
                            }

                        }
                        else
                            f[i].orcado = vetor1.lista[i].orcado / vetor2.lista[i].orcado;
                    }
                    catch (DivideByZeroException e)
                    {
                        f[i].orcado = 0;
                    }

                }
            }

            foreach (var item in f)
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

        //[HttpPost]
        //[Route("CriaGraficoEvolucao")]
        //public List<PainelIndicadoresUniManutencaoDTO> CriaGraficoEvolucao()
        //{
        //    List<PainelIndicadoresUniManutencaoDTO> _mockEvolucao = new List<PainelIndicadoresUniManutencaoDTO>();
        //    PainelIndicadoresUniManutencaoDTO coleta = new PainelIndicadoresUniManutencaoDTO();

        //    coleta.orcado = 10;
        //    coleta.realizado = 20;

        //    _mockEvolucao.Add(coleta);

        //    return _mockEvolucao;
        //}

        //[HttpPost]
        //[Route("CriaGraficoAcumulado")]
        //public List<PainelIndicadoresUniManutencaoDTO> CriaGraficoAcumulado()
        //{
        //    List<PainelIndicadoresUniManutencaoDTO> _mockEvolucao = new List<PainelIndicadoresUniManutencaoDTO>();
        //    PainelIndicadoresUniManutencaoDTO coleta = new PainelIndicadoresUniManutencaoDTO();

        //    coleta.orcado = 100;
        //    coleta.realizado = 200;

        //    _mockEvolucao.Add(coleta);

        //    return _mockEvolucao;
        //}


        [HttpPost]
        [Route("CriaGraficoAcompanhamento")]
        public List<Acompanhamento> CriaGraficoAcompanhamento(obj3 obj)
        {
            List<Acompanhamento> list = new List<Acompanhamento>();

            string parametro = obj.indicador;
            var realizado = "";
            var orcado = "";
            List<obj> d;

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

            var query2 = "select day(Base_dateRef) diaMes " +
                        "\n ,isnull(Sum(case when " + realizado + " = 0 then 0.00 else " + realizado + " end),0.00) [real] " +
                        "\n ,0.00 [targetAjustado] " +
                        "\n ,isnull(Sum(case when " + orcado + " = 0 then 0.00 else " + orcado + " end),0.00) [budget] " +
                        "\n from[ManColetaDados] " +
                        "\n left join ManCalendario on[ManColetaDados].Base_dateRef = ManCalendario.Data " +
                        "\n where " +
                        "\n convert(varchar(7),[ManColetaDados].Base_dateRef,120) = convert(varchar(7),DATEFROMPARTS('" + obj.ano + "','" + obj.mes + "',01),120)  " +
                        "\n and[ManColetaDados].Base_parCompany_id in (SELECT id FROM ParCompany WHERE Name = '" + obj.unidade + "')" +
                        "\n and ManCalendario.DiaUtil = 1 " +
                        "\n group by day(Base_dateRef) " +
                        "\n order by 1,2";

            using (var db = new SgqDbDevEntities())
            {
                list = db.Database.SqlQuery<Acompanhamento>(query2).ToList();
            }

            return list;
        }
    }

    public class listaDelistas
    {
        public List<obj2> lista { get; set; }

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
        public int qtde { get; set; }
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

    public class Acompanhamento
    {
        public int diaMes { get; set; }
        public decimal? real { get; set; }
        public decimal? targetAjustado { get; set; }
        public decimal? budget { get; set; }
        public string userAdd { get; set; }
        public string userAlter { get; set; }
    }
}
