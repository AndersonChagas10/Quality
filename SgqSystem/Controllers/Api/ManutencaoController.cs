using Dominio;
using SgqSystem.Handlres;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace SgqSystem.Controllers.Api
{
    [HandleApi()]
    [RoutePrefix("api/Manutencao")]
    public class ManutencaoController : ApiController
    {

        [HttpPost]
        [Route("getSelectGrafico1/{dataIni}/{dataFim}/{meses}/{anos}")]
        public List<Reg> getSelectGrafico1(string dataIni, string dataFim, string meses, string anos)
        {

            var lista = new List<Reg>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select EmpresaRegional as Regional, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "group by EmpresaRegional ";
                sql += "order by EmpresaRegional ";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoEvolutivoPorUnidade/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}")]
        public List<Uni> getSelectGraficoEvolutivoPorUnidade(string dataIni, string dataFim, string meses, string anos, string unidade)
        {

            var lista = new List<Uni>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select CONCAT(YEAR(MesAno),'-',CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END ) MesAno, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "AND EmpresaSigla = \'" + unidade + "\' ";
                sql += "group by CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";
                sql += "ORDER BY CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";

                lista = db.Database.SqlQuery<Uni>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoEvolutivoPorUnidadeEConta/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}/{conta}")]
        public List<Uni> getSelectGraficoEvolutivoPorUnidadeEConta(string dataIni, string dataFim, string meses, string anos, string unidade, string conta)
        {

            var lista = new List<Uni>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select CONCAT(YEAR(MesAno),'-',CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END ) MesAno, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "AND EmpresaSigla = \'" + unidade + "\' ";
                sql += "AND ContaContabil = \'" + conta + "\' ";
                sql += "group by CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";
                sql += "ORDER BY CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";

                lista = db.Database.SqlQuery<Uni>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoFatoresTecnicoMateriaPrimaRegionalEConta/{dataIni}/{dataFim}/{meses}/{anos}/{regional}/{conta}/{tipo}")]
        public List<FatoresTecnicosMateriaPrima> getSelectGraficoFatoresTecnicoMateriaPrimaRegionalEConta(string dataIni, string dataFim, string meses, string anos, string regional, string conta, string tipo)
        {

            var lista = new List<FatoresTecnicosMateriaPrima>();

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);
            regionalDecode = regionalDecode.Replace("|", "/");

            var contaDecode = HttpUtility.UrlDecode(conta, System.Text.Encoding.Default);
            contaDecode = contaDecode.Replace("|", "/");

            var tipoDecode = HttpUtility.UrlDecode(tipo, System.Text.Encoding.Default);
            tipoDecode = tipoDecode.Replace("|", "/");

            using (var db = new SgqDbDevEntities())
            {

                var TipoConsumo = "";
                var tipoDeSQL = "NULL AS Orcado, NULL AS Realizado ";

                switch (tipoDecode)
                {
                    case "MCAL":
                        TipoConsumo = "003.MCAL. Vapor";
                        tipoDeSQL = "ConsumoOrcado AS Orcado, ConsumoRealizado AS Realizado ";
                        break;

                    case "KW":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ConsumoOrcado AS Orcado, ConsumoRealizado AS Realizado ";
                        break;

                    case "M³":
                        TipoConsumo = "002.M3. Agua";
                        tipoDeSQL = "ConsumoOrcado AS Orcado, ConsumoRealizado AS Realizado ";
                        break;

                    case "Bois/Proc":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ProducaoOrcada AS Orcado, ProducaoRealizada AS Realizado ";
                        break;

                    case "MCAL/BOI":
                        TipoConsumo = "003.MCAL. Vapor";
                        tipoDeSQL = "ConsumoPorBoiOrcado AS Orcado, ConsumoPorBoiRealizado AS Realizado ";
                        break;

                    case "M³/BOI":
                        TipoConsumo = "002.M3. Agua";
                        tipoDeSQL = "ConsumoPorBoiOrcado AS Orcado, ConsumoPorBoiRealizado AS Realizado ";
                        break;

                    case "KW/BOI":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ConsumoPorBoiOrcado AS Orcado, ConsumoPorBoiRealizado AS Realizado ";
                        break;

                    case "Preço MCAL":
                        TipoConsumo = "003.MCAL. Vapor";
                        tipoDeSQL = "ROUND(DespesaOrcada/NULLIF(ConsumoOrcado, 0),2) AS Orcado, ROUND(DespesaRealizada/NULLIF(ConsumoRealizado, 0),2) AS Realizado ";
                        break;

                    case "Preço M³":
                        TipoConsumo = "002.M3. Agua";
                        tipoDeSQL = "ROUND(DespesaOrcada/NULLIF(ConsumoOrcado, 0),2) AS Orcado, ROUND(DespesaRealizada/NULLIF(ConsumoRealizado, 0),2) AS Realizado ";
                        break;

                    case "Preço KW":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ROUND(DespesaOrcada/NULLIF(ConsumoOrcado, 0),2) AS Orcado, ROUND(DespesaRealizada/NULLIF(ConsumoRealizado, 0),2) AS Realizado ";
                        break;
                }

                var sql = "";
                sql += " ";

                sql += "SELECT ";
                sql += "EmpresaSigla, ";
                sql += tipoDeSQL;
                sql += "FROM ( ";
                sql += "SELECT ";
                sql += "EmpresaSigla ";
                sql += ", ROUND(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) ProducaoRealizada ";
                sql += ", ROUND(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)), 0) ProducaoOrcada ";
                sql += ",ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / 1000, 0)   AS ConsumoRealizado ";
                sql += " , ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) / 1000, 0)		AS ConsumoOrcado ";
                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)) / 1000, 0)	AS DespesaRealizada ";
                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / 1000, 0)		AS DespesaOrcada ";

                sql += ", ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) AS ConsumoPorBoiRealizado ";
                sql += ", ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)), 0) AS ConsumoPorBoiOrcado ";

                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) AS DespesaPorBoiRealizado ";
                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)), 0) AS DespesaPorBoiOrcado ";

                sql += "FROM manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                sql += "and EmpresaRegional = \'" + regionalDecode + "\'  ";
                sql += "GROUP BY EmpresaSigla ";
                sql += ") TABELA ORDER BY 1";
               
                lista = db.Database.SqlQuery<FatoresTecnicosMateriaPrima>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoFatoresTecnicoMateriaPrimaUnidadeEConta/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}/{conta}/{tipo}")]
        public List<FatoresTecnicosMateriaPrima> getSelectGraficoFatoresTecnicoMateriaPrimaUnidadeEConta(string dataIni, string dataFim, string meses, string anos, string unidade, string conta, string tipo)
        {

            var lista = new List<FatoresTecnicosMateriaPrima>();

            var unidadeDecode = HttpUtility.UrlDecode(unidade, System.Text.Encoding.Default);
            unidadeDecode = unidadeDecode.Replace("|", "/");

            var contaDecode = HttpUtility.UrlDecode(conta, System.Text.Encoding.Default);
            contaDecode = contaDecode.Replace("|", "/");

            var tipoDecode = HttpUtility.UrlDecode(tipo, System.Text.Encoding.Default);
            tipoDecode = tipoDecode.Replace("|", "/");

            using (var db = new SgqDbDevEntities())
            {

                var TipoConsumo = "";
                var tipoDeSQL = "NULL AS Orcado, NULL AS Realizado ";

                switch (tipoDecode)
                {
                    case "MCAL":
                        TipoConsumo = "003.MCAL. Vapor";
                        tipoDeSQL = "ConsumoOrcado AS Orcado, ConsumoRealizado AS Realizado ";
                        break;

                    case "KW":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ConsumoOrcado AS Orcado, ConsumoRealizado AS Realizado ";
                        break;

                    case "M³":
                        TipoConsumo = "002.M3. Agua";
                        tipoDeSQL = "ConsumoOrcado AS Orcado, ConsumoRealizado AS Realizado ";
                        break;

                    case "Bois/Proc":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ProducaoOrcada AS Orcado, ProducaoRealizada AS Realizado ";
                        break;

                    case "MCAL/BOI":
                        TipoConsumo = "003.MCAL. Vapor";
                        tipoDeSQL = "ConsumoPorBoiOrcado AS Orcado, ConsumoPorBoiRealizado AS Realizado ";
                        break;

                    case "M³/BOI":
                        TipoConsumo = "002.M3. Agua";
                        tipoDeSQL = "ConsumoPorBoiOrcado AS Orcado, ConsumoPorBoiRealizado AS Realizado ";
                        break;

                    case "KW/BOI":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ConsumoPorBoiOrcado AS Orcado, ConsumoPorBoiRealizado AS Realizado ";
                        break;

                    case "Preço MCAL":
                        TipoConsumo = "003.MCAL. Vapor";
                        tipoDeSQL = "ROUND(DespesaOrcada/NULLIF(ConsumoOrcado, 0),2) AS Orcado, ROUND(DespesaRealizada/NULLIF(ConsumoRealizado, 0),2) AS Realizado ";
                        break;

                    case "Preço M³":
                        TipoConsumo = "002.M3. Agua";
                        tipoDeSQL = "ROUND(DespesaOrcada/NULLIF(ConsumoOrcado, 0),2) AS Orcado, ROUND(DespesaRealizada/NULLIF(ConsumoRealizado, 0),2) AS Realizado ";
                        break;

                    case "Preço KW":
                        TipoConsumo = "001.KWH. Energia Eletrica - Concessionaria";
                        tipoDeSQL = "ROUND(DespesaOrcada/NULLIF(ConsumoOrcado, 0),2) AS Orcado, ROUND(DespesaRealizada/NULLIF(ConsumoRealizado, 0),2) AS Realizado ";
                        break;
                }

                var sql = "";
                sql += " ";

                sql += "SELECT ";
                sql += "EmpresaSigla, AnoMes,  ";
                sql += tipoDeSQL;
                sql += "FROM ( ";
                sql += "SELECT ";
                sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes ";
                sql += ", ROUND(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) ProducaoRealizada ";
                sql += ", ROUND(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)), 0) ProducaoOrcada ";
                sql += ",ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / 1000, 0)   AS ConsumoRealizado ";
                sql += " , ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) / 1000, 0)		AS ConsumoOrcado ";
                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)) / 1000, 0)	AS DespesaRealizada ";
                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / 1000, 0)		AS DespesaOrcada ";

                sql += ", ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) AS ConsumoPorBoiRealizado ";
                sql += ", ROUND(SUM(CAST(CASE WHEN TipoConsumo = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)), 0) AS ConsumoPorBoiOrcado ";

                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) AS DespesaPorBoiRealizado ";
                sql += ", ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)), 0) AS DespesaPorBoiOrcado ";

                sql += "FROM manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                sql += "and EmpresaSigla = \'" + unidadeDecode + "\'  ";
                sql += "GROUP BY EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";
                sql += ") TABELA ORDER BY 2, 1";

                lista = db.Database.SqlQuery<FatoresTecnicosMateriaPrima>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getTabela1/{dataIni}/{dataFim}/{meses}/{anos}")]
        public List<Pacote> getTabela1(string dataIni, string dataFim, string meses, string anos)
        {
            var lista = new List<Pacote>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "select distinct(Pacote) as Name from Manutencao WHERE MesAno BETWEEN '20150101' AND '20180101'AND TipoInformacao = 'CustoFixo' order by Pacote";

                lista = db.Database.SqlQuery<Pacote>(sql).ToList();



                foreach (var item in lista)
                {
                    sql = " select EmpresaRegional as Regional, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao ";
                    sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo' and Pacote in ( '" + item.Name + "')  group by EmpresaRegional  order by EmpresaRegional asc; ";

                    item.ListaRegionais = db.Database.SqlQuery<Reg>(sql).ToList();

                    sql = " select Pacote as Pacote, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao ";
                    sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo' and Pacote in ( '" + item.Name + "')  group by Pacote  order by Pacote asc; ";
                    item.total = db.Database.SqlQuery<TotalPacote>(sql).FirstOrDefault();

                    sql = "select ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal from Manutencao WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo'   group by EmpresaRegional  order by EmpresaRegional asc; ";
                    item.totalColunaReg = db.Database.SqlQuery<Reg>(sql).ToList();



                }

            }

            return lista;
        }

        /****
         * método do Tatão
         * 
         * *****/


        [HttpPost]
        [Route("getTabela2/{dataIni}/{dataFim}/{meses}/{anos}/{pacote}/{regional}")]
        public List<Pacote> getSelectTabela2(string dataIni, string dataFim, string meses, string anos, string pacote, string regional)
        {

            var pacoteDecode = HttpUtility.UrlDecode(pacote, System.Text.Encoding.Default);
            pacoteDecode = pacoteDecode.Replace("|", "/");

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);
            regionalDecode = regionalDecode.Replace("|", "/");

            var lista = new List<Pacote>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "select distinct(ContaContabil) as Name from Manutencao WHERE MesAno BETWEEN '20150101' AND '20180101'AND TipoInformacao = 'CustoFixo' and Pacote in (\'" + pacoteDecode + "\') and EmpresaRegional in (\'" + regionalDecode + "\') group by ContaContabil order by ContaContabil";

                lista = db.Database.SqlQuery<Pacote>(sql).ToList();

                foreach (var item in lista)
                {
                    sql = "select Regional, sum(Orçada) as Orçada, sum(Realizada) as Realizada, sum(DesvioPorc) as DesvioPorc, sum(DesvioReal) as DesvioReal from(select EmpresaSigla as Regional, ";
                    sql += "\n ROUND(SUM(DespesaOrcada), 0) AS Orçada,";
                    sql += "\n ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "\n CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "\n ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += "\n from Manutencao";
                    sql += "\n WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\'";
                    sql += "\n AND TipoInformacao = 'CustoFixo'";
                    sql += "\n and Pacote in (\'" + pacoteDecode + "\')";
                    sql += "\n and EmpresaRegional in (\'" + regionalDecode + "\')";
                    sql += "\n and ContaContabil in ('" + item.Name + "')";
                    sql += "\n group by EmpresaSigla";
                    sql += "\n union all select EmpresaSigla as Regional,  0 AS Orçada,  0 AS Realizada,  0 AS DesvioPorc,  0 AS DesvioReal  from Manutencao";
                    sql += "\n WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo' and Pacote in (\'" + pacoteDecode + "\') and EmpresaRegional in (\'" + regionalDecode + "\') ";
                    sql += "\n group by EmpresaSigla )teste  group by Regional  order by 1 asc";

                    item.ListaRegionais = db.Database.SqlQuery<Reg>(sql).ToList();

                    sql = " select ContaContabil as Pacote, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao ";
                    sql += " WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo'";
                    sql += " and Pacote in (\'" + pacoteDecode + "\') and EmpresaRegional in (\'" + regionalDecode + "\') and ContaContabil in ( '" + item.Name + "')  group by ContaContabil  order by ContaContabil asc; ";
                    item.total = db.Database.SqlQuery<TotalPacote>(sql).FirstOrDefault();

                    sql = "select Regional, sum(Orçada) as Orçada, sum(Realizada) as Realizada, sum(DesvioPorc) as DesvioPorc, sum(DesvioReal) as DesvioReal from(";
                    sql += " select EmpresaSigla as Regional, ";
                    sql += " ROUND(SUM(DespesaOrcada), 0) AS Orçada,";
                    sql += " ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += " CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += " ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao";
                    sql += " WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\'";
                    sql += " AND TipoInformacao = 'CustoFixo'";
                    sql += " and Pacote in (\'" + pacoteDecode + "\')";
                    sql += " and EmpresaRegional in (\'" + regionalDecode + "\')";
                    sql += " group by EmpresaSigla";
                    sql += " union all select EmpresaSigla as Regional,  0 AS Orçada,  0 AS Realizada,  0 AS DesvioPorc,  0 AS DesvioReal  from Manutencao";
                    sql += " WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo' and Pacote in (\'" + pacoteDecode + "\') and EmpresaRegional in (\'" + regionalDecode + "\') ";
                    sql += " group by EmpresaSigla )teste  group by Regional  order by 1 asc";
                    item.totalColunaReg = db.Database.SqlQuery<Reg>(sql).ToList();

                }

            }

            return lista;
        }



        [HttpPost]
        [Route("getGrafico2/{dataIni}/{dataFim}/{meses}/{anos}/{regional}")]
        public List<Pacote> getSelectGrafico2(string dataIni, string dataFim, string meses, string anos, string regional)
        {

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);
            regionalDecode = regionalDecode.Replace("|", "/");

            var lista = new List<Pacote>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "SELECT EmpresaSigla, MesAno,";
                sql += " \n sum(cast(case when TipoProducao = '011.QT. Bois Processados' then ProducaoRealizada else 0 end as float)) / sum(cast(case when TipoProducao = '011.QT. Bois Processados' then ProducaoOrcada  else 0 end as float)) - 1";
                sql += " \n as [DesvQtdeBoi],";
                sql += " \n sum(cast(case when TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoRealizado else 0 end as float))    /";
                sql += " \n sum(cast(case when TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoOrcado else 0 end as float)) - 1";
                sql += " \n as [DesvQtdeConsumoUN], ";
                sql += " \n (sum(cast(case when ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' then DespesaRealizada else 0 end as float)) / sum(cast(case when TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoRealizado else 0 end as float)))";
                sql += " \n / (sum(cast(case when ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' then DespesaOrcada else 0 end as float)) / sum(cast(case when TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoOrcado else 0 end as float)))- 1";
                sql += " \n AS[DesvPrecoUN]	, (sum(cast(case when TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoRealizado else 0 end as float))";
                sql += " \n / sum(cast(case when TipoProducao = '011.QT. Bois Processados' then ProducaoRealizada else 0 end as float))";
                sql += " \n  )";
                sql += " \n / -------------------------------------------------------------------------------------------------------";
                sql += " \n      (";
                sql += " \n       sum(cast(case when TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoOrcado else 0 end as float))";
                sql += " \n         / sum(cast(case when TipoProducao = '011.QT. Bois Processados' then ProducaoOrcada else 0 end as float))";
                sql += " \n      )";
                sql += " \n - 1  as [DesvUNBoi]";
                sql += " \n FROM    Manutencao WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' and EmpresaCluster != 'Cluster 1 [Desossa 0%]' GROUP BY  EmpresaSigla ,MesAno";



                lista = db.Database.SqlQuery<Pacote>(sql).ToList();



            }

            return lista;
        }



        [HttpPost]
        [Route("getGraficoRegionalTecnico/{dataIni}/{dataFim}/{meses}/{anos}/{regional}/{conta}")]
        public List<FatoresTecReg> getGraficoRegionalTecnico(string dataIni, string dataFim, string meses, string anos, string regional, string conta)
        {

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);
            regionalDecode = regionalDecode.Replace("|", "/");

            var contaDecode = HttpUtility.UrlDecode(conta, System.Text.Encoding.Default);
            contaDecode = contaDecode.Replace("|", "/");

            var lista = new List<FatoresTecReg>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                if( conta == "ENERGIA ELÉTRICA CONTRATADA")
                {
                    sql = "SELECT ";
                    sql += "EmpresaSigla ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) - 1   AS DesvQtdeBoi ";
                    sql += " , ";
                    sql += "  SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) ";
                    sql += "   / ";
                    sql += "  SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) - 1 ";
                    sql += " AS DesvQtdeConsumoUN ";
                    sql += ", ";
                    sql += "(SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' then DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TIPOCONSUMO = '001.KWH. ENERGIA ELETRICA - CONCESSIONARIA' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT))) ";
                    sql += "/ ";
                    sql += "(SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) ";
                    sql += "- 1 ";
                    sql += "AS DesvPrecoUN ";
                    sql += ", ";
                    sql += "( ";
                    sql += "SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoRealizado ELSE 0 END AS FLOAT)) ";
                    sql += "/ SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) ";
                    sql += ") ";
                    sql += "/ ";
                    sql += "( ";
                    sql += "SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) ";
                    sql += "/ SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) ";
                    sql += ") ";
                    sql += "- 1 ";
                    sql += "as DesvUNBoi ";
                    sql += " ";
                    sql += " ";
                    sql += "FROM ";
                    sql += "manutencao ";
                    sql += "WHERE ";
                    sql += "MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "and EmpresaRegional = \'" + regionalDecode + "\' ";
                    sql += "GROUP BY ";
                    sql += "EmpresaSigla ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "ORDER BY 1 ";
                }
                else if (conta == "COMBUSTÍVEL PARA CALDEIRA")
                {
                    sql = "SELECT ";
                    sql += "EmpresaSigla ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) - 1   AS DesvQtdeBoi ";
                    sql += ", CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE  SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) - 1  END AS DesvQtdeConsumoUN ";
                    sql += ", CASE WHEN(CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) END) = 0 THEN 0 ELSE(CASE WHEN SUM(CAST(CASE WHEN TIPOCONSUMO = '003.MCAL. Vapor' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' and TipoInformacao = 'CustoFixo' then DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TIPOCONSUMO = '003.MCAL. Vapor' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT))) END) / (CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) END) -1 END AS DesvPrecoUN ";
                    sql += ", (SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT))) / (SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT))) as DesvUNBoi ";

                    sql += " ";
                    sql += " ";
                    sql += "FROM ";
                    sql += "manutencao ";
                    sql += "WHERE ";
                    sql += "MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "and EmpresaRegional = \'" + regionalDecode + "\' ";
                    sql += "GROUP BY ";
                    sql += "EmpresaSigla ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "ORDER BY 1 ";
                }
                else if (conta == "ÁGUA")
                {
                    sql = "SELECT ";
                    sql += "EmpresaSigla ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) - 1   AS DesvQtdeBoi ";
                    sql += ", CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) - 1 END  AS DesvQtdeConsumoUN ";
                    sql += ", CASE WHEN SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) = 0 OR SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(CASE WHEN SUM(CAST(CASE WHEN TIPOCONSUMO = '002.M3. Agua' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' and TipoInformacao = 'CustoFixo' then DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TIPOCONSUMO = '002.M3. Agua' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)) END) / (SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) - 1 END AS DesvPrecoUN ";
                    sql += ", CASE WHEN SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) END  ";
                    sql += "as DesvUNBoi ";

                    sql += "FROM ";
                    sql += "manutencao ";
                    sql += "WHERE ";
                    sql += "MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "and EmpresaRegional = \'" + regionalDecode + "\' ";
                    sql += "GROUP BY ";
                    sql += "EmpresaSigla ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "ORDER BY 1 ";
                }
                else
                {
                    sql = "SELECT NULL AS EmpresaSigla, NULL AS DesvQtdeBoi, NULL AS DesvQtdeConsumoUN, NULL AS DesvPrecoUN, NULL AS DesvUNBoi FROM Manutencao WHERE 1 = 2";
                }

                lista = db.Database.SqlQuery<FatoresTecReg>(sql).ToList();

            }

            return lista;
        }



        [HttpPost]
        [Route("getGraficoUnidadeTecnico/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}/{conta}")]
        public List<FatoresTecReg> getGraficoUnidadeTecnico(string dataIni, string dataFim, string meses, string anos, string unidade, string conta)
        {

            var unidadeDecode = HttpUtility.UrlDecode(unidade, System.Text.Encoding.Default);
            unidadeDecode = unidadeDecode.Replace("|", "/");

            var contaDecode = HttpUtility.UrlDecode(conta, System.Text.Encoding.Default);
            contaDecode = contaDecode.Replace("|", "/");

            var lista = new List<FatoresTecReg>();

            using (var db = new SgqDbDevEntities())
            {
      

                var sql = "";

                if (conta == "ENERGIA ELÉTRICA CONTRATADA")
                {
                    sql = "SELECT ";
                    sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes  ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";
                    sql += " , ";
                    sql += "  SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) - 1 ";
                    sql += "  AS DesvQtdeBoi ";
                    //sql += " , ";
                    //sql += "  SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) ";
                    //sql += "   / ";
                    //sql += "  SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) - 1 ";
                    //sql += " AS DesvQtdeConsumoUN ";
                    //sql += ", ";
                    //sql += "(SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' then DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TIPOCONSUMO = '001.KWH. ENERGIA ELETRICA - CONCESSIONARIA' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT))) ";
                    //sql += "/ ";
                    //sql += "(SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) ";
                    //sql += "- 1 ";
                    //sql += "AS DesvPrecoUN ";
                    sql += ", ";
                    sql += "( ";
                    sql += "SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then ConsumoRealizado ELSE 0 END AS FLOAT)) ";
                    sql += "/ SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) ";
                    sql += ") ";
                    sql += "/ ";
                    sql += "( ";
                    sql += "SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) ";
                    sql += "/ SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) ";
                    sql += ") ";
                    sql += "- 1 ";
                    sql += "as DesvUNBoi ";

                    sql += ",CASE WHEN (SUM(CAST(CASE WHEN TIPOCONSUMO = '001.KWH. ENERGIA ELETRICA - CONCESSIONARIA' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT))) = 0 OR (SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT))) = 0 OR (SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) = 0 THEN 0 ELSE (SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' then DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TIPOCONSUMO = '001.KWH. ENERGIA ELETRICA - CONCESSIONARIA' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT))) / (SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) - 1 END AS DesvPrecoUN ";
                    sql += " ";
                    sql += " ";
                    sql += "FROM ";
                    sql += "manutencao ";
                    sql += "WHERE ";
                    sql += "MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "and EmpresaSigla = \'" + unidadeDecode + "\' ";
                    sql += "GROUP BY ";
                    sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END)  ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "ORDER BY 2 ";
                }
                else if (conta == "COMBUSTÍVEL PARA CALDEIRA")
                {
                    sql = "SELECT ";
                    sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes  ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";
                    sql += ",   SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) - 1   AS DesvQtdeBoi ";
                    sql += ", CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE  SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) - 1  END AS DesvQtdeConsumoUN ";
                    sql += ", CASE WHEN(CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) END) = 0 THEN 0 ELSE(CASE WHEN SUM(CAST(CASE WHEN TIPOCONSUMO = '003.MCAL. Vapor' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' and TipoInformacao = 'CustoFixo' then DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TIPOCONSUMO = '003.MCAL. Vapor' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT))) END) / (CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) END) -1 END AS DesvPrecoUN ";
                    sql += ", SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE  (SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT))) / (SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT))) END as DesvUNBoi ";

                    sql += " ";
                    sql += " ";
                    sql += "FROM ";
                    sql += "manutencao ";
                    sql += "WHERE ";
                    sql += "MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "and EmpresaSigla = \'" + unidadeDecode + "\' ";
                    sql += "GROUP BY ";
                    sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END)  ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "ORDER BY 2 ";
                }
                else if (conta == "ÁGUA")
                {
                    sql = "SELECT ";
                    sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes  ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";
                    sql += ", SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada  ELSE 0 END AS FLOAT)) - 1   AS DesvQtdeBoi ";
                    sql += ", CASE WHEN SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) - 1 END  AS DesvQtdeConsumoUN ";
                    sql += ", CASE WHEN SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) = 0 OR SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE(CASE WHEN SUM(CAST(CASE WHEN TIPOCONSUMO = '002.M3. Agua' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' and TipoInformacao = 'CustoFixo' then DespesaRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TIPOCONSUMO = '002.M3. Agua' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)) END) / (SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' and TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN CONSUMOORCADO ELSE 0 END AS FLOAT))) - 1 END AS DesvPrecoUN ";
                    //sql += ", CASE WHEN SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) END  ";
                    //sql += "as DesvUNBoi ";

                    sql += ", CASE WHEN (SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT))) = 0 OR (SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT))) = 0 THEN 0 ELSE CASE WHEN SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) = 0 THEN 0 ELSE SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)) / SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) END END as DesvUNBoi ";

                    sql += "FROM ";
                    sql += "manutencao ";
                    sql += "WHERE ";
                    sql += "MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "and EmpresaSigla = \'" + unidadeDecode + "\' ";
                    sql += "GROUP BY ";
                    sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END)  ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "ORDER BY 2 ";
                }
                else
                {
                    sql = "SELECT NULL AS EmpresaSigla, NULL AS DesvQtdeBoi, NULL AS DesvQtdeConsumoUN, NULL AS DesvPrecoUN, NULL AS DesvUNBoi FROM Manutencao WHERE 1 = 2";
                }


                lista = db.Database.SqlQuery<FatoresTecReg>(sql).ToList();



            }

            return lista;
        }
    }
}

public class Pacote
    {
        public string Name { get; set; }
        public List<Reg> ListaRegionais { get; set; }
        public TotalPacote total { get; set; }
        public List<Reg> totalColunaReg { get; set; }
}
   

    public class Reg
    {
        public string Regional { get; set; }
        public double? Orçada { get; set; }
        public double? Realizada { get; set; }
        public double? DesvioPorc { get; set; }
        public double? DesvioReal { get; set; }
    }

    public class Uni
    {
        public string MesAno { get; set; }
        public double? Orçada { get; set; }
        public double? Realizada { get; set; }
        public double? DesvioPorc { get; set; }
        public double? DesvioReal { get; set; }
    }

    public class TotalPacote
    {
        public string Pacote { get; set; }
        public double? Orçada { get; set; }
        public double? Realizada { get; set; }
        public double? DesvioPorc { get; set; }
        public double? DesvioReal { get; set; }
    }

    public class Desvio
    {
        public string nome { set; get; }
        public string Regional { get; set; }
        public double? Orçada { get; set; }
        public double? Realizada { get; set; }
        public double? DesvioPorc { get; set; }
        public double? DesvioReal { get; set; }
    }


    public class FatoresTecReg
    {
        public string EmpresaSigla { set; get; }
        public double? DesvQtdeBoi { get; set; }
        public double? DesvQtdeConsumoUN { get; set; }
        public double? DesvPrecoUN { get; set; }
        public double? DesvUNBoi { get; set; }
        public string AnoMes { get; set; }
        public double? BOIS_MORTOS { get; set; }
        public double? BOIS_MARCADOS_PARA_MORRER { get; set; }

}

public class FatoresTecnicosMateriaPrima
{

    public double? Orcado { get; set; }
    public double? Realizado  { get; set; }
 
    public string EmpresaSigla { get; set; }
    public string AnoMes { get; set; }

}