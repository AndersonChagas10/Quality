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
        public List<PainelIndicadoresUniManutencaoDTO> GetTabela(VisaoPainel visaoPainel)
        {
            List<PainelIndicadoresUniManutencaoDTO> _mockEvolucao = new List<PainelIndicadoresUniManutencaoDTO>();
            PainelIndicadoresUniManutencaoDTO manColeta;

            List<string> indicador = new List<string>();
            var tipoCalculo = "normal";

            indicador.Add(visaoPainel.indicador);

            switch (indicador[0])
            {
                case "Manutenção R$":
                    indicador.Add("Custo Fixo Manutenção");
                    indicador.Add("Custo Fixo Utilidades");
                    tipoCalculo = "Soma";
                    break;
                case "R$/CB Manut":
                    indicador.Add("Custo Fixo Manutenção");
                    //indicador.Add("Custo Fixo Utilidades");
                    indicador.Add("Bois Abatidos");
                    //tipoCalculo = "CustoCabeca";
                    break;
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
                    indicador.Add("Quilo Sebo");
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
                    tipoCalculo = "Variadas"; // Para retornar o Orcado diretamente
                    break;
                case "Apropr Planej...to":
                    indicador.Add("Nº OS Planejadas");
                    indicador.Add("Nº OS Programadas");
                    tipoCalculo = "Variadas";
                    break;
                case "Apropriação de O.S":
                    indicador.Add("Nº Hrs Apropriadas OS");
                    indicador.Add("Nº Hrs Disponíveis Trabalhar");
                    tipoCalculo = "Variadas";
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
                List<Busca1> buscas = null;
                List<Busca2> buscas2 = null;

                var query = "SELECT top 1 Realizado.Realizado ,Orcado.Orcado FROM " +
                            "(SELECT top 1 Name Realizado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Real' UNION ALL SELECT '0') Realizado " +
                            ",(SELECT top 1 Name Orcado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Meta' UNION ALL SELECT '0') Orcado ";

                using (var db = new SgqDbDevEntities())
                {
                    buscas = db.Database.SqlQuery<Busca1>(query).ToList();
                }

                foreach (var item in buscas)
                {
                    orcado = item.orcado;
                    realizado = item.realizado;
                }

                string tipo = "";
                string tipo2 = "";

                if (visaoPainel.regional == "Todas")
                {
                    tipo = "SELECT distinct ParCompany_id from DimManBaseUni where ParCompany_id is not null";
                    tipo2 = "Select distinct ParCompany_id, EmpresaSigla, DimManBaseReg_id, EmpresaRegional, DimManBaseRegGrup_id, EmpresaRegionalGrupo, EmpresaCluster from DimManBaseUni where ParCompany_id is not null";
                }
                else
                {
                    if (visaoPainel.subRegional == "Todas")
                    {
                        tipo = "SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegionalGrupo = '" + visaoPainel.regional + "' and ParCompany_id is not null";
                        tipo2 = "Select distinct ParCompany_id, EmpresaSigla, DimManBaseReg_id, EmpresaRegional, DimManBaseRegGrup_id, EmpresaRegionalGrupo, EmpresaCluster from DimManBaseUni where EmpresaRegionalGrupo = '" + visaoPainel.regional + "' and ParCompany_id is not null";
                    }
                    else
                    {
                        tipo = "SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegional = '" + visaoPainel.subRegional + "' and ParCompany_id is not null";
                        tipo2 = "Select distinct ParCompany_id, EmpresaSigla, DimManBaseReg_id, EmpresaRegional, DimManBaseRegGrup_id, EmpresaRegionalGrupo, EmpresaCluster from DimManBaseUni where EmpresaRegional = '" + visaoPainel.subRegional + "' and ParCompany_id is not null";
                    }
                }

                string querytipocalculo = "";
                string tipoCalculo2 = "";
                string Calculo = "";

                querytipocalculo = "SELECT top 1 Calculo_Acumulado FROM DimManColetaDadosCalc WHERE Indicadores = (select DimName from DimManColetaDados where Name = '" + realizado + "')";


                using (var db = new SgqDbDevEntities())
                {
                    tipoCalculo2 = db.Database.SqlQuery<string>(querytipocalculo).FirstOrDefault();
                }


                switch (tipoCalculo2)
                {
                    case "Soma":
                        Calculo = "SUM";
                        break;
                    case "Média":
                        Calculo = "AVG";
                        break;
                    case "Maior Valor no Mês":
                        Calculo = "MAX";
                        break;
                    case "Menor Valor no Mês":
                        Calculo = "MIN";
                        break;
                    case "Ultimo Valor no Mês":
                        Calculo = "MIN";
                        break;
                    default:
                        Calculo = "SUM";
                        break;
                }

                var query2 = "\n SELECT " +
                             "\n   BASONA.Dado " +
                             "\n ,BASONA.Realizado " +
                             "\n ,BASONA.Orcado " +
                             "\n ,BASONA.qtde " +
                        "\n FROM " +
                        "\n (" +
                            "\n SELECT " +
                                "\n 'Por Unidade' TipoRelatorio " +
                                "\n , Mes.Mes dado " +
                                "\n , SUM(isnull(Base.Realizado, 0.00)) realizado " +
                                "\n , SUM(isnull(Base.Orcado, 0.00))    orcado " +
                                "\n , SUM(isnull(qtde, 0)) qtde " +
                            "\n FROM MANANOMES MES " +
                            "\n LEFT JOIN " +
                            "\n ( " +
                            //Real por unidade    
                            "\n SELECT MONTH(ISNULL(Base_dateRef, cast(Base_dateAdd AS varchar(10)))) Mes, " +
                                        "\n " + Calculo + "(ISNULL(CASE " +
                                            "\n WHEN " + realizado + " = '0' THEN 0.00 " +
                                            "\n ELSE " + realizado + " " +
                                        "\n END, 0.00)) realizado, " +
                                        "\n SUM(ISNULL(CASE " +
                                           "\n WHEN 0.00 = '0' THEN 0.00 " +
                                           "\n ELSE 0.00 " +
                                        "\n END, 0.00)) orcado, " +
                                        "\n count(1) as qtde " +
                                "\n FROM MANCOLETADADOS Man " +
                                "\n WHERE " +
                                    "\n " + realizado + " is not null and " +
                                    "\n ISNULL(YEAR(BASE_DATEREF), YEAR(BASE_DATEADD)) = '" + visaoPainel.ano + "' " +
                                    "\n AND ISNULL(MONTH(BASE_DATEREF), MONTH(BASE_DATEADD)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END " +
                                    "\n AND Man.Base_parCompany_id in (SELECT id FROM ParCompany WHERE Name = '" + visaoPainel.unidade + "')" +
                                "\n GROUP BY MONTH(ISNULL(Base_dateRef, cast(Base_dateAdd AS varchar(10)))) " +
                                "\n union all " +
                                //Orcado por Unidade
                                "\n SELECT  " +
                                "\n MONTH(ISNULL(dateRef, NULL)) " +
                                "\n ,0.00 " +
                                "\n ,SUM(ISNULL(ValueBudgetedIndicators,0)) " +
                                "\n ,0 " +
                                "\n FROM ManBudgetedIndicators Budget " +
                                "\n WHERE  " +
                                "\n ISNULL(YEAR(DATEREF), YEAR(DATEREF)) = '" + visaoPainel.ano + "' " +
                                "\n AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%'ELSE '" + visaoPainel.mes + "'END " +
                                "\n AND parCompany_id IN (SELECT id FROM ParCompany WHERE Name = '" + visaoPainel.unidade + "') " +
                                "\n AND (SELECT top 1 DimManCMDColetaDados_id FROM DimManColetaDados where name = '" + orcado + "') = Budget.DimManCMDColetaDados_id " +
                                "\n AND (MONTH (DATEREF) <= MONTH(GETDATE())) " +
                                "\n GROUP BY MONTH(ISNULL(dateRef, NULL)) " +
                                "\n )Base on MES.MesInt = Base.Mes " +
                                "\n GROUP BY Mes.Mes " +
                            "\n union all " +
                            "\n SELECT " +
                                "\n 'Por Regional' TipoRelatorio " +
                                "\n , Uni.EmpresaSigla dado " +
                                "\n , SUM(isnull(Base.Realizado, 0.00)) Realizado " +
                                "\n , SUM(isnull(Base.Orcado, 0.00))    Orcado " +
                                "\n , SUM(isnull(qtde, 0)) qtde " +
                            "\n FROM (" + tipo2 + ") Uni " + //AQUI
                            "\n LEFT JOIN( " +
                                //Real por Regional    
                                "SELECT Man.Base_parCompany_id, " +
                                        "\n " + Calculo + "(ISNULL(CASE " +
                                            "\n WHEN  " + realizado + "  = '0' THEN 0.00 " +
                                            "\n ELSE  " + realizado + "  " +
                                        "\n END, 0)) realizado, " +
                                        "\n SUM(ISNULL(CASE " +
                                            "\n WHEN 0 = '0' THEN 0.00 " +
                                            "\n ELSE 0.00 " +
                                        "\n END, 0.00)) orcado, " +
                                        "\n count(1) as qtde " +
                                "\n FROM MANCOLETADADOS Man " +
                                "\n WHERE " +
                                    "\n " + realizado + " is not null and " +
                                    "\n ISNULL(YEAR(BASE_DATEREF), YEAR(BASE_DATEADD)) = '" + visaoPainel.ano + "'" +
                                    "\n AND ISNULL(MONTH(BASE_DATEREF), MONTH(BASE_DATEADD)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END " +
                                    "\n AND Man.Base_parCompany_id in (" + tipo + ") " +
                                "\n GROUP BY Man.Base_parCompany_id " +
                                "\n union all " +
                                //Orcado por Regional    
                                "\n SELECT  " +
                                "\n parCompany_id " +
                                "\n ,0.00 " +
                                "\n ,SUM(ISNULL(ValueBudgetedIndicators,0.00)) " +
                                "\n ,0 " +
                                "\n FROM ManBudgetedIndicators Budget " +
                                "\n WHERE  " +
                                "\n ISNULL(YEAR(DATEREF), YEAR(DATEREF)) = '" + visaoPainel.ano + "' " +
                                "\n AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%'ELSE '" + visaoPainel.mes + "'END " +
                                "\n AND (SELECT top 1 DimManCMDColetaDados_id FROM DimManColetaDados where name = '" + orcado + "') = Budget.DimManCMDColetaDados_id " +
                                "\n AND parCompany_id in (" + tipo + ") " +
                                "\n AND (MONTH (DATEREF) <= MONTH(GETDATE())) " +
                                "\n GROUP BY parCompany_id " +
                            "\n )Base on uni.Parcompany_id = Base.Base_parCompany_id " +
                        //"\n WHERE Base.realizado != 0 AND Base.orcado != 0 " +
                        "\n GROUP BY Uni.EmpresaSigla)BASONA " +
                        "\n WHERE BASONA.TipoRelatorio = '" + visaoPainel.tipoRelatorio + "' " +
                        "\n ORDER BY " +
                        "\n CASE " +
                        "\n 	WHEN DADO = 'JAN' THEN 1 " +
                        "\n 	WHEN DADO = 'FEV' THEN 2 " +
                        "\n 	WHEN DADO = 'MAR' THEN 3 " +
                        "\n 	WHEN DADO = 'ABR' THEN 4 " +
                        "\n 	WHEN DADO = 'MAI' THEN 5 " +
                        "\n 	WHEN DADO = 'JUN' THEN 6 " +
                        "\n 	WHEN DADO = 'JUL' THEN 7 " +
                        "\n 	WHEN DADO = 'AGO' THEN 8 " +
                        "\n 	WHEN DADO = 'SET' THEN 9 " +
                        "\n 	WHEN DADO = 'OUT' THEN 10" +
                        "\n 	WHEN DADO = 'NOV' THEN 11" +
                        "\n 	WHEN DADO = 'DEZ' THEN 12" +
                        "\n ELSE 1                    " +
                        "\n END                          ";

                using (var db = new SgqDbDevEntities())
                {
                    buscas2 = db.Database.SqlQuery<Busca2>(query2).ToList();
                    if (i == 0)
                    {
                        vetor0.lista = buscas2;
                    }
                    else if (i == 1)
                    {
                        vetor1.lista = buscas2;
                    }
                    else if (i == 2)
                    {
                        vetor2.lista = buscas2;
                    }
                    else if (i == 3)
                    {
                        vetor3.lista = buscas2;
                    }
                    else if (i == 4)
                    {
                        vetor4.lista = buscas2;
                    }
                }
            }

            List<Busca2> f = new List<Busca2>();
            f = vetor0.lista;
            var queryAbat = "";
            var queryVar = "";
            var orcadoAbat = "";
            var orcadoVar = "";

            if (vetor1.lista != null)
            {

                for (int i = 0; i < vetor1.lista.Count; i++)
                {
                    f[i].dado = vetor1.lista[i].dado;

                    try
                    {
                        if (tipoCalculo == "media")
                        {
                            f[i].realizado = vetor1.lista[i].realizado / vetor1.lista[i].qtde * vetor1.lista[i].qtde;
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
                        else if (tipoCalculo == "Soma")
                        {
                            f[i].realizado = vetor1.lista[i].realizado + vetor2.lista[i].realizado;
                        }
                        else if (tipoCalculo == "CustoCabeca")
                        {
                            f[i].realizado = (vetor1.lista[i].realizado + vetor2.lista[i].realizado) / vetor3.lista[i].realizado;
                        }
                        else if (tipoCalculo == "Variadas")
                        {
                            f[i].realizado = vetor1.lista[i].realizado / vetor2.lista[i].realizado;
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
                        //if (tipoCalculo == "media")
                        //    f[i].orcado = vetor1.lista[i].orcado / vetor1.lista[i].qtde;
                        //else 
                        if (tipoCalculo == "Disponibilidade")
                        {
                            //queryAbat = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id = (select id from ParCompany where name like '%" + visaoPainel.unidade + "%')";
                            if (visaoPainel.tipoRelatorio == "Por Regional")
                            {
                                if (visaoPainel.regional == "Todas")
                                {
                                    queryAbat = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id in (select id from ParCompany)";
                                }
                                else if (visaoPainel.subRegional == "Todas")
                                {
                                    queryAbat = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id IN (SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegionalGrupo = '" + visaoPainel.regional + "' and ParCompany_id is not null)";
                                }
                                else
                                {
                                    queryAbat = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id IN (SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegional = '" + visaoPainel.subRegional + "' and ParCompany_id is not null)";
                                }

                            }
                            using (var db = new SgqDbDevEntities())
                            {
                                orcadoAbat = db.Database.SqlQuery<string>(queryAbat).FirstOrDefault();
                            }
                            if (vetor4.lista != null)
                            {
                                f[i].orcado = Convert.ToDecimal(orcadoAbat) / 100; // COLOCAR O INDICADOR DIRETO DA TABELA ManBudgetedIndicators

                            }
                            else
                            {
                                f[i].orcado = Convert.ToDecimal(orcadoAbat) / 100;

                            }

                        }

                        else if (tipoCalculo == "Soma")
                        {
                            f[i].orcado = vetor1.lista[i].orcado + vetor2.lista[i].orcado;
                        }
                        else if (tipoCalculo == "CustoCabeca")
                        {
                            f[i].orcado = (vetor1.lista[i].orcado + vetor2.lista[i].orcado) / vetor3.lista[i].orcado;
                        }
                        else if (tipoCalculo == "Variadas" || tipoCalculo == "media")
                        {
                            if (visaoPainel.tipoRelatorio == "Por Regional")
                            {
                                if (visaoPainel.regional == "Todas")
                                {
                                    queryVar = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id in (select id from ParCompany)";
                                }
                                else if (visaoPainel.subRegional == "Todas")
                                {
                                    queryVar = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id IN (SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegionalGrupo = '" + visaoPainel.regional + "' and ParCompany_id is not null)";
                                }
                                else
                                {
                                    queryVar = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id IN (SELECT distinct ParCompany_id from DimManBaseUni where EmpresaRegional = '" + visaoPainel.subRegional + "' and ParCompany_id is not null)";
                                }

                            }
                            else if (visaoPainel.tipoRelatorio == "Por Unidade")
                            {
                                queryVar = "SELECT cast(cast(AVG(ValueBudgetedIndicators)as int) as varchar(500))ValueBudgetedIndicators FROM ManBudgetedIndicators WHERE DimManCMDColetaDados_id IN (SELECT ID FROM DimManCMDColetaDados WHERE DimName LIKE '%" + visaoPainel.indicador + "%') and year(DateRef) LIKE '%" + visaoPainel.ano + "%' AND ISNULL(MONTH(DATEREF), MONTH(DATEREF)) LIKE CASE WHEN '" + visaoPainel.mes + "' = 0 THEN '%%' ELSE '" + visaoPainel.mes + "' END and ParCompany_id IN (SELECT distinct ParCompany_id from DimManBaseUni where EmpresaSigla = (select top 1 initials from ParCompany where name in ('" + visaoPainel.unidade + "') and ParCompany_id is not null))";
                            }

                            using (var db = new SgqDbDevEntities())
                            {
                                if (visaoPainel.indicador == "CARTA METAS")
                                {
                                }
                                orcadoVar = db.Database.SqlQuery<string>(queryVar).FirstOrDefault();
                            }
                            if (visaoPainel.indicador == "CARTA METAS")
                            {
                                f[i].orcado = Convert.ToDecimal(orcadoVar);
                            }
                            else
                            {
                                f[i].orcado = Convert.ToDecimal(orcadoVar) / 100;
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
        public List<Acompanhamento> CriaGraficoAcompanhamento(VisaoPainel visaoPainel)
        {
            List<Acompanhamento> list = new List<Acompanhamento>();
            List<Acompanhamento> list2 = new List<Acompanhamento>();

            string parametro = visaoPainel.indicador;
            var realizado = "";
            var orcado = "";
            List<Busca1> buscas;

            var query = "SELECT top 1 Realizado.Realizado ,Orcado.Orcado FROM " +
                        "(SELECT top 1 Name Realizado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Real' UNION ALL SELECT '0') Realizado " +
                        ",(SELECT top 1 Name Orcado FROM DimManColetaDados WHERE DimName like '" + parametro + "' and DimRealTarget like 'Meta' UNION ALL SELECT '0') Orcado ";

            using (var db = new SgqDbDevEntities())
            {
                buscas = db.Database.SqlQuery<Busca1>(query).ToList();
            }

            foreach (var item in buscas)
            {
                orcado = item.orcado;
                realizado = item.realizado;
            }

            //var query2 = "select day(Base_dateRef) diaMes " +
            //            "\n ,isnull(Sum(case when " + realizado + " = 0 then 0.00 else " + realizado + " end),0.00) [real] " +
            //            "\n ,0.00 [targetAjustado] " +
            //            "\n ,isnull(Sum(case when " + orcado + " = 0 then 0.00 else " + orcado + " end),0.00) [budget] " +
            //            "\n from [ManColetaDados] " +
            //            "\n left join ManCalendario on[ManColetaDados].Base_dateRef = ManCalendario.Data " +
            //            "\n where " +
            //            "\n convert(varchar(7),[ManColetaDados].Base_dateRef,120) = convert(varchar(7),DATEFROMPARTS('" + obj.ano + "','" + obj.mes + "',01),120)  " +
            //            "\n and[ManColetaDados].Base_parCompany_id in (SELECT id FROM ParCompany WHERE Name = '" + obj.unidade + "')" +
            //            "\n and ManCalendario.DiaUtil = 1 " +
            //            "\n group by day(Base_dateRef) " +
            //            "\n order by 1,2";


            //using (var db = new SgqDbDevEntities())
            //{
            //    list = db.Database.SqlQuery<Acompanhamento>(query2).ToList();
            //}


            var query2 = "SELECT " +
                        "\n day(Calendario.Data) as diaMes " +
                        "\n , CONVERT(VARCHAR(10),Calendario.Data, 103) as data " +
                        "\n , ISNULL(CAST(" + realizado + " AS DECIMAL(30,10)), 0.00) as 'real' " +
                        "\n , ISNULL(CAST(" + orcado + " AS DECIMAL(30,10)), 0.00) as 'targetAjustado' " +
                        "\n , ISNULL(isnull(Man.userAlter, Man.userAdd), '') as userResp " +
                        "\n , ISNULL(Dim.DimName, '') as Indicador " +
                        "\n , ISNULL(valores.targetAjustado, 0.00) as budget " +
                        "\n , ISNULL(valores.budget, 0.00) as budget " +
                        "\n FROM(select Data, day(Data) Dia from ManCalendario WHERE CONVERT(VARCHAR(7), Data, 120) = CONVERT(VARCHAR(7), DATEFROMPARTS('" + visaoPainel.ano + "', '" + visaoPainel.mes + "', 01), 120)) Calendario " +
                              "\n LEFT join(SELECT * FROM ManColetaDados WHERE CONVERT(VARCHAR(7), Base_dateRef, 120) = CONVERT(VARCHAR(7), DATEFROMPARTS('" + visaoPainel.ano + "', '" + visaoPainel.mes + "', 01), 120) AND " + realizado + " IS NOT NULL AND Base_parCompany_id in (SELECT id FROM ParCompany WHERE Name = '" + visaoPainel.unidade + "')) Man on Calendario.Data = Man.Base_dateRef " +
                        "\n Inner Join DimManColetaDados Dim on '" + realizado + "' = Dim.Name " +
                        "\n left join " +
                        "\n ( " +
                           "\n select day(Base_dateRef) diaMes " +
                           "\n , isnull(Sum(case when " + realizado + " = 0 then 0.00 else " + realizado + " end), 0.00)[real] " +
                           "\n , 0.00[targetAjustado] " +
                           "\n , isnull(Sum(case when " + orcado + " = 0 then 0.00 else " + orcado + " end), 0.00)[budget] " +
                           "\n from[ManColetaDados] " +
                           "\n left join ManCalendario on[ManColetaDados].Base_dateRef = ManCalendario.Data " +
                           "\n where " +
                           "\n convert(varchar(7),[ManColetaDados].Base_dateRef, 120) = convert(varchar(7), DATEFROMPARTS('" + visaoPainel.ano + "', '" + visaoPainel.mes + "', 01), 120) " +
                           "\n and[ManColetaDados].Base_parCompany_id in (SELECT id FROM ParCompany WHERE Name = '" + visaoPainel.unidade + "') " +
                           "\n and ManCalendario.DiaUtil = 1 " +
                           "\n group by day(Base_dateRef) " +
                        "\n ) " +
                        "\n as valores " +
                        "\n on valores.diaMes = day(Calendario.Data) " +
                        "\n ORDER BY 1";

            using (var db = new SgqDbDevEntities())
            {
                list2 = db.Database.SqlQuery<Acompanhamento>(query2).ToList();
            }

            return list2;
        }

    }

    public class listaDelistas
    {
        public List<Busca2> lista { get; set; }

    }

    public class Busca1
    {
        public string realizado { get; set; }
        public string orcado { get; set; }
    }

    public class Busca2
    {
        public string dado { get; set; }
        public decimal realizado { get; set; }
        public decimal orcado { get; set; }
        public int qtde { get; set; }
    }

    public class VisaoPainel
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
        public string data { get; set; }
        public decimal? real { get; set; }
        public decimal? targetAjustado { get; set; }
        public decimal? budget { get; set; }
        public string userResp { get; set; }
    }
}
