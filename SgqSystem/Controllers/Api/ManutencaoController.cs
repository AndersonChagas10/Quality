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

        public string queryReg(string regional)
        {
            var regionais = new List<string>(regional.Split(','));
            var valor = "";
            var count = 0;

            foreach (string r in regionais)
            {
                valor += "'" + r + "'";
                if (count < regionais.Count - 1)
                    valor += ",";
                count++;
            }

            return " and EmpresaRegional in (" + valor + ")";
        }

        [HttpPost]
        [Route("getSelectGrafico1/{dataIni}/{dataFim}/{meses}/{anos}/{regFiltro}/{regional}")]
        public List<Reg> getSelectGrafico1(string dataIni, string dataFim, string meses, string anos, string regFiltro, string regional)
        {
            var lista = new List<Reg>();

            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");

            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            var regionalFilter = regional;
            //regionalFilter = regionalFilter.Replace("|", "/");

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select EmpresaRegional as Regional, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE 1=1 ";
                sql += "and MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "AND EmpresaRegionalGrupo = '" + regionalFilter + "' ";
                sql += regionaisFiltradas;
                sql += "group by EmpresaRegional ";
                sql += "order by EmpresaRegional ";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }


        [HttpPost]
        [Route("getSelectGraficoRegionalPorPacote/{dataIni}/{dataFim}/{meses}/{anos}/{regFiltro}/{regional}")]
        public List<Reg> getSelectGraficoRegionalPorPacote(string dataIni, string dataFim, string meses, string anos, string regFiltro, string regional)
        {
            var lista = new List<Reg>();

            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");

            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            var regionalFilter = regional;
            regionalFilter = regionalFilter.Replace("|", "/");


            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select concat(EmpresaRegional, ' <br><br> ', pacote) as Regional, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE 1=1 ";
                sql += "and MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "AND EmpresaRegional = '" + regionalFilter + "' ";
                sql += regionaisFiltradas;
                sql += "group by EmpresaRegional, pacote ";
                sql += "order by EmpresaRegional, pacote ";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoRegionalGrupoPorPacote")]
        public List<Reg> getSelectGraficoRegionalGrupoPorPacote(Ajax obj)
        {
            var lista = new List<Reg>();

            if (obj.anos == "null") obj.anos = "";
            if (obj.meses == "null") obj.meses = "";

            string regionaisFiltradas = "";

            if (obj.regFiltro != null)
            {

                var regionalFiltDecode = HttpUtility.UrlDecode(obj.regFiltro, System.Text.Encoding.Default);
                regionalFiltDecode = regionalFiltDecode.Replace("|", "/");

                if (regionalFiltDecode != "null") regionaisFiltradas = queryReg(regionalFiltDecode);

            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select concat(EmpresaRegionalGrupo, '<br><br>', pacote) as Regional, ";
                //sql = "select pacote as Regional, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE 1=1 ";
                sql += "and MesAno BETWEEN \'" + obj.dataIni + "\' AND \'" + obj.dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "AND EmpresaRegionalGrupo = '" + obj.regional + "' ";
                sql += regionaisFiltradas;
                sql += "group by EmpresaRegionalGrupo, pacote ";
                sql += "order by EmpresaRegionalGrupo, pacote ";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectEmpresaRegionalList")]
        public List<Reg> getSelectEmpresaRegionalList()
        {
            var lista = new List<Reg>();

            using (var db = new SgqDbDevEntities())
            {
                var sql = "select distinct EmpresaRegional as Regional from manutencao";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGrafico0/{dataIni}/{dataFim}/{meses}/{anos}")]
        public List<Reg> getSelectGrafico0(string dataIni, string dataFim, string meses, string anos)
        {
            var lista = new List<Reg>();

            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            //var regionalFiltDecode = HttpUtility.UrlDecode(regFiltrada, System.Text.Encoding.Default);
            //regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select EmpresaRegionalGrupo as Regional, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE 1=1 ";
                sql += "and MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += regionaisFiltradas;
                sql += "group by EmpresaRegionalGrupo ";
                sql += "order by EmpresaRegionalGrupo ";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGrafDespReg/{dataIni}/{dataFim}/{meses}/{anos}/{regional}/{pacote}")]
        public List<Reg> getSelectGrafDespReg(string dataIni, string dataFim, string meses, string anos, string regional, string pacote)
        {
            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var eRegional = "";

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";
                sql = "SELECT TOP 1 RETORNO FROM ( ";
                sql += "SELECT top 1 '1' retorno FROM DimManBaseRegGrup WHERE EmpresaRegionalGrupo like '%" + regional + "%' UNION ALL ";
                sql += "SELECT '0') AS RETORNO ";

                eRegional = db.Database.SqlQuery<string>(sql).FirstOrDefault();
            }

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);

            if (eRegional == "1")
            {
                regionalDecode = regionalDecode.Trim();
            }
            else
            {
                regionalDecode = regionalDecode.Replace("|", "/").Trim();
            }

            //regionalDecode = regional;

            var pacoteDecode = HttpUtility.UrlDecode(pacote, System.Text.Encoding.Default);
            pacoteDecode = pacoteDecode.Replace("|", "/").Trim();

            var lista = new List<Reg>();

            //var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            //regionalFiltDecode = regionalFiltDecode.Replace("|", "/");

            //string regionaisFiltradas = "";
            //string regionalFiltDecode = "";

            //if (regionalFiltDecode != "null")
            //{
            //    regionaisFiltradas = queryReg(regionalFiltDecode);
            //}

            string pacotes = "";

            if (pacote != null)
            {
                pacotes = " AND Pacote = '" + pacoteDecode + "' ";
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select EmpresaSigla as Regional, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada ";
                sql += "from Manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "\n and (EmpresaRegionalGrupo like \'%" + regionalDecode + "%\' OR EmpresaRegional like \'%" + regionalDecode + "%\') ";
                sql += pacotes;
                //sql += regionaisFiltradas;
                sql += " group by EmpresaSigla ";
                sql += " order by EmpresaSigla ";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGrafDespRegGrup")]
        public List<Reg> getSelectGrafDespRegGrup(Ajax obj)
        {
            if (obj.anos == "null") obj.anos = "";
            if (obj.meses == "null") obj.meses = "";

            var regionalDecode = HttpUtility.UrlDecode(obj.regional, System.Text.Encoding.Default);
            //regionalDecode = regionalDecode.Replace("|", "/");

            var lista = new List<Reg>();

            //var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            //regionalFiltDecode = regionalFiltDecode.Replace("|", "/");

            string regionaisFiltradas = "";
            string regionalFiltDecode = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            string pacotes = "";

            if (obj.pacote != null)
            {
                pacotes = "AND Pacote = " + obj.pacote;
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select EmpresaSigla as Regional, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada ";
                sql += "from Manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + obj.dataIni + "\' AND \'" + obj.dataFim + "\' ";
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "\n and (EmpresaRegionalGrupo like \'%" + regionalDecode + "%\' OR EmpresaRegional like \'%" + regionalDecode + "%\') ";
                sql += pacotes;
                //sql += regionaisFiltradas;
                sql += "group by EmpresaSigla ";
                sql += "order by EmpresaSigla ";

                lista = db.Database.SqlQuery<Reg>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoEvolutivoPorUnidade/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}/{regFiltro}")]
        public List<Uni> getSelectGraficoEvolutivoPorUnidade(string dataIni, string dataFim, string meses, string anos, string unidade, string regFiltro)
        {
            string sqlData = "";

            var lista = new List<Uni>();

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //_mockFiltroMes.Add("01");
            //_mockFiltroMes.Add("03");
            //_mockFiltroMes.Add("07");
            //_mockFiltroAno.Add("2015");
            //_mockFiltroAno.Add("2016");
            //_mockFiltroAno.Add("2017");

            var _mockFiltroMes = new List<string>();
            var _mockFiltroAno = new List<string>();

            if (meses != "null") _mockFiltroMes = new List<string>(meses.Split(','));

            if (anos != "null") _mockFiltroAno = new List<string>(anos.Split(','));

            if (_mockFiltroMes.Count != 0 || _mockFiltroAno.Count != 0)
            {
                string mes = "";
                for (int i = 0; i < _mockFiltroMes.Count; i++)
                {
                    if (i == 0)
                    {
                        mes = "'" + _mockFiltroMes[i] + "'";
                    }
                    else
                    {
                        mes += ',' + "'" + _mockFiltroMes[i] + "'";
                    }

                }

                string ano = "";
                for (int i = 0; i < _mockFiltroAno.Count; i++)
                {
                    if (i == 0)
                    {
                        ano = "'" + _mockFiltroAno[i] + "'";
                    }
                    else
                    {
                        ano += ',' + "'" + _mockFiltroAno[i] + "'";
                    }

                }
                if (ano != "" && mes != "")
                    sqlData = " and year(MesAno) in (" + ano + ") and MONTH(MesAno) in (" + mes + ") ";
                else if (mes != "")
                    sqlData = " and MONTH(MesAno) in (" + mes + ") ";
                else if (ano != "")
                    sqlData = " and year(MesAno) in (" + ano + ") ";
            }
            else
            {
                sqlData = " and MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
            }

            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select CONCAT(YEAR(MesAno),'-',CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END ) MesAno, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE 1=1 ";
                sql += sqlData;
                sql += "AND TipoInformacao = 'CustoFixo' ";
                sql += "AND EmpresaSigla = \'" + unidade + "\' ";
                sql += regionaisFiltradas;
                sql += "group by CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";
                sql += "ORDER BY CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";

                lista = db.Database.SqlQuery<Uni>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoEvolutivoPorUnidadeEConta/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}/{conta}/{regFiltro}")]
        public List<Uni> getSelectGraficoEvolutivoPorUnidadeEConta(string dataIni, string dataFim, string meses, string anos, string unidade, string conta, string regFiltro)
        {

            var lista = new List<Uni>();
            var sqlData = "";
            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");

            var _mockFiltroMes = new List<string>();
            var _mockFiltroAno = new List<string>();

            if (meses != "null") _mockFiltroMes = new List<string>(meses.Split(','));

            if (anos != "null") _mockFiltroAno = new List<string>(anos.Split(','));

            //_mockFiltroMes.Add("01");
            //_mockFiltroMes.Add("03");
            //_mockFiltroMes.Add("07");
            //_mockFiltroAno.Add("2015");
            //_mockFiltroAno.Add("2016");
            //_mockFiltroAno.Add("2017");

            if (_mockFiltroMes.Count != 0 || _mockFiltroAno.Count != 0)
            {
                string mes = "";
                for (int i = 0; i < _mockFiltroMes.Count; i++)
                {
                    if (i == 0)
                    {
                        mes = "'" + _mockFiltroMes[i] + "'";
                    }
                    else
                    {
                        mes += ',' + "'" + _mockFiltroMes[i] + "'";
                    }

                }

                string ano = "";
                for (int i = 0; i < _mockFiltroAno.Count; i++)
                {
                    if (i == 0)
                    {
                        ano = "'" + _mockFiltroAno[i] + "'";
                    }
                    else
                    {
                        ano += ',' + "'" + _mockFiltroAno[i] + "'";
                    }

                }
                if (ano != "" && mes != "")
                    sqlData = " and year(MesAno) in (" + ano + ") and MONTH(MesAno) in (" + mes + ") ";
                else if (mes != "")
                    sqlData = " and MONTH(MesAno) in (" + mes + ") ";
                else if (ano != "")
                    sqlData = " and year(MesAno) in (" + ano + ") ";
            }
            else
            {
                sqlData = " and MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
            }
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                sql = "select CONCAT(YEAR(MesAno),'-',CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END ) MesAno, ";
                sql += "ROUND(SUM(DespesaOrcada) / 1000, 0) AS Orçada, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000, 0) AS Realizada, ";
                sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc, ";
                sql += "ROUND(SUM(DespesaRealizada) / 1000 - SUM(DespesaOrcada) / 1000, 0) AS DesvioReal ";
                sql += "from Manutencao ";
                sql += "WHERE TipoInformacao = 'CustoFixo' ";
                sql += sqlData;

                sql += "AND EmpresaSigla = \'" + unidade + "\' ";
                sql += "AND ContaContabil = \'" + conta + "\' ";
                sql += regionaisFiltradas;
                sql += "group by CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";
                sql += "ORDER BY CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";

                lista = db.Database.SqlQuery<Uni>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoFatoresTecnicoMateriaPrimaRegionalEConta/{dataIni}/{dataFim}/{meses}/{anos}/{regional}/{conta}/{tipo}/{regFiltro}")]
        public List<FatoresTecnicosMateriaPrima> getSelectGraficoFatoresTecnicoMateriaPrimaRegionalEConta(string dataIni, string dataFim, string meses, string anos, string regional, string conta, string tipo, string regFiltro)
        {

            var lista = new List<FatoresTecnicosMateriaPrima>();

            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

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

                sql += "		,ROUND(SUM(CAST(CASE WHEN TipoProducao  = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) ProducaoRealizada ";

                sql += "		,ROUND(SUM(CAST(CASE WHEN TipoProducao  = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)), 0) ProducaoOrcada ";

                sql += "		,ROUND(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / 1000, 0) AS ConsumoRealizado ";

                sql += "		,ROUND(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) / 1000, 0) AS ConsumoOrcado ";

                sql += "		,ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)) / 1000, 0) AS DespesaRealizada ";

                sql += "		,ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / 1000, 0) AS DespesaOrcada ";

                sql += "		,ROUND(NULLIF(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0)  ";
                sql += "			/  ";
                sql += "			   NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0), 0) AS ConsumoPorBoiRealizado ";

                sql += "		,ROUND(NULLIF(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)  ";
                sql += "			/  ";
                sql += "				NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0), 0) AS ConsumoPorBoiOrcado ";

                sql += "		,ROUND(NULLIF(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0)  ";
                sql += "			/ NULLIF(SUM(CAST(CASE WHEN TipoProducao   = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0), 0) AS DespesaPorBoiRealizado ";

                sql += "		,ROUND(NULLIF(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0)  ";
                sql += "			/  ";
                sql += "			  NULLIF(SUM(CAST(CASE WHEN TipoProducao   = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0), 0) AS DespesaPorBoiOrcado ";

                sql += "FROM manutencao ";
                sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "and EmpresaRegional = \'" + regionalDecode + "\'  ";
                sql += regionaisFiltradas;
                sql += "GROUP BY EmpresaSigla ";
                sql += ") TABELA ORDER BY 1";

                lista = db.Database.SqlQuery<FatoresTecnicosMateriaPrima>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getSelectGraficoFatoresTecnicoMateriaPrimaUnidadeEConta/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}/{conta}/{tipo}/{regFiltro}")]
        public List<FatoresTecnicosMateriaPrima> getSelectGraficoFatoresTecnicoMateriaPrimaUnidadeEConta(string dataIni, string dataFim, string meses, string anos, string unidade, string conta, string tipo, string regFiltro)
        {

            var lista = new List<FatoresTecnicosMateriaPrima>();
            var sqlData = "";
            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var _mockFiltroMes = new List<string>();
            var _mockFiltroAno = new List<string>();

            if (meses != "") _mockFiltroMes = new List<string>(meses.Split(','));

            if (anos != "") _mockFiltroAno = new List<string>(anos.Split(','));

            if (_mockFiltroMes.Count != 0 || _mockFiltroAno.Count != 0)
            {
                string mes = "";
                for (int i = 0; i < _mockFiltroMes.Count; i++)
                {
                    if (i == 0)
                    {
                        mes = "'" + _mockFiltroMes[i] + "'";
                    }
                    else
                    {
                        mes += ',' + "'" + _mockFiltroMes[i] + "'";
                    }

                }

                string ano = "";
                for (int i = 0; i < _mockFiltroAno.Count; i++)
                {
                    if (i == 0)
                    {
                        ano = "'" + _mockFiltroAno[i] + "'";
                    }
                    else
                    {
                        ano += ',' + "'" + _mockFiltroAno[i] + "'";
                    }

                }

                if (ano != "''" && mes != "''" && ano != "" && mes != "")
                    sqlData = " year(MesAno) in (" + ano + ") and MONTH(MesAno) in (" + mes + ") ";
                else if (mes != "''" && mes != "")
                    sqlData = " MONTH(MesAno) in (" + mes + ") ";
                else if (ano != "''" && ano != "")
                    sqlData = " year(MesAno) in (" + ano + ") ";
            }
            else
            {
                sqlData = " MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
            }

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

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

                sql += "\n SELECT ";
                sql += "\n EmpresaSigla, AnoMes,  ";
                sql += "\n " + tipoDeSQL;
                sql += "\n FROM ( ";
                sql += "\n SELECT ";
                sql += "\n EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes ";
                sql += "\n 		,ROUND(SUM(CAST(CASE WHEN TipoProducao  = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)), 0) ProducaoRealizada ";
                sql += "\n 		,ROUND(SUM(CAST(CASE WHEN TipoProducao  = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)), 0) ProducaoOrcada ";
                sql += "\n 		,ROUND(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) / 1000, 0) AS ConsumoRealizado ";
                sql += "\n 		,ROUND(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)) / 1000, 0) AS ConsumoOrcado ";
                sql += "\n 		,ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)) / 1000, 0) AS DespesaRealizada ";
                sql += "\n 		,ROUND(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)) / 1000, 0) AS DespesaOrcada ";
                sql += "\n 		,ROUND(NULLIF(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0)  ";
                sql += "\n 			/  ";
                sql += "\n 			   NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0), 0) AS ConsumoPorBoiRealizado ";
                sql += "\n 		,ROUND(NULLIF(SUM(CAST(CASE WHEN TipoConsumo   = \'" + TipoConsumo + "\' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)  ";
                sql += "\n 			/  ";
                sql += "\n 				NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0), 0) AS ConsumoPorBoiOrcado ";
                sql += "\n 		,ROUND(NULLIF(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0)  ";
                sql += "\n 			/ NULLIF(SUM(CAST(CASE WHEN TipoProducao   = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0), 0) AS DespesaPorBoiRealizado ";
                sql += "\n 		,ROUND(NULLIF(SUM(CAST(CASE WHEN ContaContabil = \'" + contaDecode + "\' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0)  ";
                sql += "\n 			/  ";
                sql += "\n 			  NULLIF(SUM(CAST(CASE WHEN TipoProducao   = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0), 0) AS DespesaPorBoiOrcado ";

                sql += "\n FROM manutencao ";
                //sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                sql += "\n WHERE";
                sql += "\n " + sqlData;
                //sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                sql += "\n and EmpresaSigla = \'" + unidadeDecode + "\'  ";
                sql += "\n " + regionaisFiltradas;
                sql += "\n GROUP BY EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) ";
                sql += "\n ) TABELA ORDER BY 2, 1";

                lista = db.Database.SqlQuery<FatoresTecnicosMateriaPrima>(sql).ToList();
            }

            return lista;
        }

        [HttpPost]
        [Route("getTabela1/{dataIni}/{dataFim}/{meses}/{anos}/{regFiltro}")]
        public List<Pacote> getTabela1(string dataIni, string dataFim, string meses, string anos, string regFiltro)
        {
            var lista = new List<Pacote>();

            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "select distinct(Pacote) as Name from Manutencao WHERE MesAno BETWEEN '20150101' AND '20180101'AND TipoInformacao = 'CustoFixo' order by Pacote";

                lista = db.Database.SqlQuery<Pacote>(sql).ToList();

                foreach (var item in lista)
                {
                    sql = " select EmpresaRegional as Regional, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal ";
                    sql += "from Manutencao ";
                    sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo' and Pacote in ( '" + item.Name + "') ";
                    sql += regionaisFiltradas;
                    sql += " group by EmpresaRegional  order by EmpresaRegional asc; ";

                    item.ListaRegionais = db.Database.SqlQuery<Reg>(sql).ToList();

                    sql = " select Pacote as Pacote, ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,";
                    sql += "CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,";
                    sql += "ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal";
                    sql += " from Manutencao ";
                    sql += "WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo' and Pacote in ( '" + item.Name + "') ";
                    sql += regionaisFiltradas;
                    sql += " group by Pacote  order by Pacote asc; ";
                    item.total = db.Database.SqlQuery<TotalPacote>(sql).FirstOrDefault();

                    sql = "select ROUND(SUM(DespesaOrcada), 0) AS Orçada,  ROUND(SUM(DespesaRealizada), 0) AS Realizada,CASE WHEN SUM(DespesaOrcada) = 0 THEN 0 ELSE ROUND((SUM(DespesaRealizada) / SUM(DespesaOrcada) - 1) * 100, 0) END AS DesvioPorc,ROUND(SUM(DespesaRealizada) - SUM(DespesaOrcada), 0) AS DesvioReal from Manutencao WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' AND TipoInformacao = 'CustoFixo' ";
                    sql += regionaisFiltradas;
                    sql += " group by EmpresaRegional  order by EmpresaRegional asc; ";
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
        [Route("getTabela2/{dataIni}/{dataFim}/{meses}/{anos}/{pacote}/{regional}/{regFiltro}")]
        public List<Pacote> getSelectTabela2(string dataIni, string dataFim, string meses, string anos, string pacote, string regional, string regFiltro)
        {

            var pacoteDecode = HttpUtility.UrlDecode(pacote, System.Text.Encoding.Default);
            pacoteDecode = pacoteDecode.Replace("|", "/");

            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);
            regionalDecode = regionalDecode.Replace("|", "/");

            var lista = new List<Pacote>();

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "select distinct(ContaContabil) as Name from Manutencao WHERE MesAno BETWEEN '20150101' AND '20180101'AND TipoInformacao = 'CustoFixo' and Pacote in (\'" + pacoteDecode + "\') and EmpresaRegional in (\'" + regionalDecode + "\')";
                sql += regionaisFiltradas;
                sql += " group by ContaContabil order by ContaContabil";

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
                    sql += regionaisFiltradas;
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
                    sql += " and Pacote in (\'" + pacoteDecode + "\') and EmpresaRegional in (\'" + regionalDecode + "\') and ContaContabil in ( '" + item.Name + "') ";
                    sql += regionaisFiltradas;
                    sql += " group by ContaContabil  order by ContaContabil asc; ";
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
                    sql += regionaisFiltradas;
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
        [Route("getGrafico2/{dataIni}/{dataFim}/{meses}/{anos}/{regional}/{regFiltro}")]
        public List<Pacote> getSelectGrafico2(string dataIni, string dataFim, string meses, string anos, string regional, string regFiltro)
        {

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);
            regionalDecode = regionalDecode.Replace("|", "/");

            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var lista = new List<Pacote>();

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

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
                sql += " \n FROM    Manutencao WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\'";
                sql += regionaisFiltradas;
                sql += " GROUP BY  EmpresaSigla ,MesAno";



                lista = db.Database.SqlQuery<Pacote>(sql).ToList();



            }

            return lista;
        }



        [HttpPost]
        [Route("getGraficoRegionalTecnico/{dataIni}/{dataFim}/{meses}/{anos}/{regional}/{conta}/{regFiltro}")]
        public List<FatoresTecReg> getGraficoRegionalTecnico(string dataIni, string dataFim, string meses, string anos, string regional, string conta, string regFiltro)
        {
            if (anos == "null") anos = "";
            if (meses == "null") meses = "";

            var regionalDecode = HttpUtility.UrlDecode(regional, System.Text.Encoding.Default);
            regionalDecode = regionalDecode.Replace("|", "/");

            var contaDecode = HttpUtility.UrlDecode(conta, System.Text.Encoding.Default);
            contaDecode = contaDecode.Replace("|", "/");

            var lista = new List<FatoresTecReg>();

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            using (var db = new SgqDbDevEntities())
            {
                var sql = "";

                if (conta == "ENERGIA ELÉTRICA CONTRATADA")
                {
                    sql = " SELECT ";
                    sql += "	EmpresaSigla,";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE NULL END AS FLOAT)),0) BOIS_MORTOS, ";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE NULL END AS FLOAT)),0) BOIS_MARCADOS_PARA_MORRER, ";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE NULL END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE NULL END AS FLOAT)),0) -1 AS DesvQtdeBoi, ";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoRealizado ELSE NULL END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE NULL END AS FLOAT)),0) - 1 AS DesvQtdeConsumoUN, ";

                    sql += "		(NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += "	NULLIF(SUM(CAST(CASE WHEN TIPOCONSUMO = '001.KWH. Energia Eletrica - Concessionaria' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)),0)) ";
                    sql += "	/ ";
                    sql += "		(NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += "	NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)),0)) - 1 AS DesvPrecoUN, ";

                    sql += " (NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' then (CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " (NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)),0) ";
                    sql += "       / ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0))) -1 AS DesvUNBoi ";

                    sql += " FROM Manutencao ";
                    sql += " WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += " and EmpresaRegional = \'" + regionalDecode + "\' ";
                    sql += regionaisFiltradas;
                    sql += " GROUP BY EmpresaSigla ";
                    sql += " ORDER BY 1 ";
                }
                else if (conta == "COMBUSTÍVEL PARA CALDEIRA")
                {
                    sql = " SELECT ";
                    sql += "	EmpresaSigla,";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE NULL END AS FLOAT)),0) BOIS_MORTOS, ";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE NULL END AS FLOAT)),0) BOIS_MARCADOS_PARA_MORRER, ";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE NULL END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE NULL END AS FLOAT)),0) -1 AS DesvQtdeBoi,";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoRealizado ELSE NULL END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE NULL END AS FLOAT)),0) - 1 AS DesvQtdeConsumoUN, ";

                    sql += "		(NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += "	NULLIF(SUM(CAST(CASE WHEN TIPOCONSUMO = '003.MCAL. Vapor' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)),0)) ";
                    sql += "	/ ";
                    sql += "		(NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += "	NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)),0)) - 1 AS DesvPrecoUN, ";

                    sql += " (NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' then (CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " (NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)),0) ";
                    sql += "       / ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0))) -1 AS DesvUNBoi ";

                    sql += " FROM Manutencao ";
                    sql += " WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += " and EmpresaRegional = \'" + regionalDecode + "\' ";
                    sql += regionaisFiltradas;
                    sql += " GROUP BY EmpresaSigla ";
                    sql += " ORDER BY 1 ";

                }
                else if (conta == "ÁGUA")
                {
                    sql = " SELECT ";
                    sql += "	EmpresaSigla,";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE NULL END AS FLOAT)),0) BOIS_MORTOS, ";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE NULL END AS FLOAT)),0) BOIS_MARCADOS_PARA_MORRER, ";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE NULL END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE NULL END AS FLOAT)),0) -1 AS DesvQtdeBoi,";

                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoRealizado ELSE NULL END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE NULL END AS FLOAT)),0) - 1 AS DesvQtdeConsumoUN, ";

                    sql += " (NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += "	NULLIF(SUM(CAST(CASE WHEN TIPOCONSUMO = '002.M3. Agua' THEN CONSUMOREALIZADO ELSE 0 END AS FLOAT)),0)) ";
                    sql += "	/ ";
                    sql += "	(NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN CONSUMOORCADO ELSE 0 END AS FLOAT)),0)) - 1 AS DesvPrecoUN, ";

                    sql += " (NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then (CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/ ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "	/ ";
                    sql += " (NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN(CASE WHEN ConsumoOrcado = 0 THEN ConsumoRealizado ELSE ConsumoOrcado END) ELSE 0 END AS FLOAT)),0) ";
                    sql += "       / ";
                    sql += " NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0))) -1 AS DesvUNBoi ";

                    sql += " FROM Manutencao ";
                    sql += " WHERE MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
                    sql += " and EmpresaRegional = \'" + regionalDecode + "\' ";
                    sql += regionaisFiltradas;
                    sql += " GROUP BY EmpresaSigla ";
                    sql += " ORDER BY 1 ";
                }
                else
                {
                    sql = "SELECT NULL AS EmpresaSigla, NULL AS DesvQtdeBoi, NULL AS DesvQtdeConsumoUN, NULL AS DesvPrecoUN, NULL AS DesvUNBoi FROM Manutencao WHERE 1 = 2 ";
                    sql += regionaisFiltradas;
                }

                lista = db.Database.SqlQuery<FatoresTecReg>(sql).ToList();

            }

            return lista;
        }



        [HttpPost]
        [Route("getGraficoUnidadeTecnico/{dataIni}/{dataFim}/{meses}/{anos}/{unidade}/{conta}/{regFiltro}")]
        public List<FatoresTecReg> getGraficoUnidadeTecnico(string dataIni, string dataFim, string meses, string anos, string unidade, string conta, string regFiltro)
        {

            var unidadeDecode = HttpUtility.UrlDecode(unidade, System.Text.Encoding.Default);
            unidadeDecode = unidadeDecode.Replace("|", "/");

            var contaDecode = HttpUtility.UrlDecode(conta, System.Text.Encoding.Default);
            contaDecode = contaDecode.Replace("|", "/");

            var lista = new List<FatoresTecReg>();

            var regionalFiltDecode = HttpUtility.UrlDecode(regFiltro, System.Text.Encoding.Default);
            regionalFiltDecode = regionalFiltDecode.Replace("|", "/");
            //string regionalFiltDecode = "";
            string regionaisFiltradas = "";

            if (regionalFiltDecode != "null")
            {
                regionaisFiltradas = queryReg(regionalFiltDecode);
            }

            var _mockFiltroMes = new List<string>();
            var _mockFiltroAno = new List<string>();

            if (meses != "null") _mockFiltroMes = new List<string>(meses.Split(','));

            if (anos != "null") _mockFiltroAno = new List<string>(anos.Split(','));

            var sqlData = "";

            if (_mockFiltroMes.Count != 0 || _mockFiltroAno.Count != 0)
            {
                string mes = "";
                for (int i = 0; i < _mockFiltroMes.Count; i++)
                {
                    if (i == 0)
                    {
                        mes = "'" + _mockFiltroMes[i] + "'";
                    }
                    else
                    {
                        mes += ',' + "'" + _mockFiltroMes[i] + "'";
                    }

                }

                string ano = "";
                for (int i = 0; i < _mockFiltroAno.Count; i++)
                {
                    if (i == 0)
                    {
                        ano = "'" + _mockFiltroAno[i] + "'";
                    }
                    else
                    {
                        ano += ',' + "'" + _mockFiltroAno[i] + "'";
                    }

                }
                if (ano != "" && mes != "")
                    sqlData = " year(MesAno) in (" + ano + ") and MONTH(MesAno) in (" + mes + ") ";
                else if (mes != "")
                    sqlData = " MONTH(MesAno) in (" + mes + ") ";
                else if (ano != "")
                    sqlData = " year(MesAno) in (" + ano + ") ";
            }
            else
            {
                sqlData = " MesAno BETWEEN \'" + dataIni + "\' AND \'" + dataFim + "\' ";
            }

            using (var db = new SgqDbDevEntities())
            {


                var sql = "";

                if (conta == "ENERGIA ELÉTRICA CONTRATADA")
                {
                    sql = "\n SELECT ";
                    sql += "\n 	EmpresaSigla ";
                    sql += "\n 	,CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes ";

                    sql += "\n 	,SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";

                    sql += "\n 	,SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";

                    sql += "\n 	,NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) ,0) ";
                    sql += "\n 			/  ";
                    sql += "\n 	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0) - 1 AS DesvQtdeConsumoUN ";

                    sql += "\n 	,NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "\n 			/  ";
                    sql += "\n 	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0) - 1 AS DesvQtdeBoi ";

                    sql += "\n 	,NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "\n 		/  ";
                    sql += "\n 	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0) - 1  AS DesvQtdeConsumoUN ";

                    sql += "\n 	,NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0)  ";
                    sql += "\n 			/  ";
                    sql += "\n 	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "\n 		/  ";
                    sql += "\n 	 (NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'ENERGIA ELÉTRICA CONTRATADA' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0) ";
                    sql += "\n 			/  ";
                    sql += "\n 	  NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)) - 1 AS DesvPrecoUN ";

                    sql += "\n 	,(NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "\n 			/  ";
                    sql += "\n 	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0)  ";
                    sql += "\n 		)/(  ";
                    sql += "\n 	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '001.KWH. Energia Eletrica - Concessionaria' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)  ";
                    sql += "\n 			/ ";
                    sql += "\n 	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0)) -1 AS DesvUNBoi ";
                    sql += "\n FROM ";
                    sql += "\n manutencao ";
                    sql += "\n WHERE ";
                    sql += "\n " + sqlData;
                    //sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "\n and EmpresaSigla = \'" + unidadeDecode + "\' ";
                    sql += "\n " + regionaisFiltradas;
                    sql += "\n GROUP BY ";
                    sql += "\n EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END)  ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "\n ORDER BY 2 ";
                }
                else if (conta == "COMBUSTÍVEL PARA CALDEIRA")
                {
                    sql = "SELECT ";
                    sql += "	EmpresaSigla ";
                    sql += "	,CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes ";

                    sql += "	,SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";

                    sql += "	,SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) ,0) ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0) - 1 AS DesvQtdeConsumoUN ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0) - 1 AS DesvQtdeBoi ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0) - 1  AS DesvQtdeConsumoUN ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0)  ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/  ";
                    sql += "	 (NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'COMBUSTÍVEL PARA CALDEIRA' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0) ";
                    sql += "			/  ";
                    sql += "	  NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)) - 1 AS DesvPrecoUN ";

                    sql += "	,(NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0)  ";
                    sql += "		)/(  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '003.MCAL. Vapor' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)  ";
                    sql += "			/ ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0))-1 AS DesvUNBoi ";

                    sql += "FROM ";
                    sql += "manutencao ";
                    sql += "WHERE ";
                    sql += sqlData;
                    //sql += "and EmpresaCluster != 'Cluster 1 [Desossa 0%]' ";
                    sql += "and EmpresaSigla = \'" + unidadeDecode + "\' ";
                    sql += regionaisFiltradas;
                    sql += "GROUP BY ";
                    sql += "EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END)  ";
                    //sql += "HAVING SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' then(CASE WHEN ConsumoRealizado = 0 THEN ConsumoOrcado ELSE ConsumoRealizado END) ELSE 0 END AS FLOAT)) > 0 ";
                    sql += "ORDER BY 2 ";
                }
                else if (conta == "ÁGUA")
                {
                    sql = " SELECT ";
                    sql += "	EmpresaSigla ";
                    sql += "	,CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END) AnoMes ";

                    sql += "	,SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)) BOIS_MORTOS ";

                    sql += "	,SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)) BOIS_MARCADOS_PARA_MORRER ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoRealizado ELSE 0 END AS FLOAT)) ,0) ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0) - 1 AS DesvQtdeConsumoUN ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0) ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0) - 1 AS DesvQtdeBoi ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0) - 1  AS DesvQtdeConsumoUN ";

                    sql += "	,NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' AND TipoInformacao = 'CustoFixo' THEN DespesaRealizada ELSE 0 END AS FLOAT)),0)  ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "		/  ";
                    sql += "	 (NULLIF(SUM(CAST(CASE WHEN ContaContabil = 'ÁGUA' AND TipoInformacao = 'CustoFixo' THEN DespesaOrcada ELSE 0 END AS FLOAT)),0) ";
                    sql += "			/  ";
                    sql += "	  NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)) - 1 AS DesvPrecoUN ";

                    sql += "	,(NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoRealizado ELSE 0 END AS FLOAT)),0) ";
                    sql += "			/  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoRealizada ELSE 0 END AS FLOAT)),0)  ";
                    sql += "		)/(  ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoConsumo = '002.M3. Agua' THEN ConsumoOrcado ELSE 0 END AS FLOAT)),0)  ";
                    sql += "			/ ";
                    sql += "	 NULLIF(SUM(CAST(CASE WHEN TipoProducao = '011.QT. Bois Processados' THEN ProducaoOrcada ELSE 0 END AS FLOAT)),0))-1 AS DesvUNBoi ";

                    sql += " FROM Manutencao ";
                    sql += " WHERE ";
                    sql += sqlData;
                    sql += " and EmpresaSigla = \'" + unidadeDecode + "\' ";
                    sql += regionaisFiltradas;
                    sql += " GROUP BY EmpresaSigla, CONCAT(YEAR(MesAno), '-', CASE WHEN LEN(MONTH(MesAno)) = 1 THEN CONCAT('0', CAST(MONTH(MesAno) AS VARCHAR)) ELSE CAST(MONTH(MesAno) AS VARCHAR) END)  ";
                    sql += "ORDER BY 2 ";
                }
                else
                {
                    sql = "SELECT NULL AS EmpresaSigla, NULL AS DesvQtdeBoi, NULL AS DesvQtdeConsumoUN, NULL AS DesvPrecoUN, NULL AS DesvUNBoi FROM Manutencao WHERE 1 = 2 ";
                    sql += regionaisFiltradas;
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
    public double? Realizado { get; set; }

    public string EmpresaSigla { get; set; }
    public string AnoMes { get; set; }

}

public class Ajax
{
    public string dataIni { get; set; }
    public string dataFim { get; set; }
    public string meses { get; set; }
    public string anos { get; set; }
    public string regFiltro { get; set; }
    public string regional { get; set; }
    public string pacote { get; set; }
}